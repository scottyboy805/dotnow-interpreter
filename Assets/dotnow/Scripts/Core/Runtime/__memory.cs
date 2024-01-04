using dotnow.Runtime.Handle;
using dotnow.Runtime.Types;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace dotnow.Runtime
{
    internal unsafe class __memory
    {
        // Type
        private struct TrackedMemory
        {
            public int referenceCount;
            public object managedObject;
        }

        // Private
        private static List<TrackedMemory> heap = new List<TrackedMemory>(1024);

        // Constructor
        ~__memory() 
        {
            heap.Clear();
            heap = null;
        }

        // Methods
        public static int PinManagedObject(object managedObject)
        {
            // Get address
            int addr = heap.Count;

            // Create tracked memory
            TrackedMemory tracked = new TrackedMemory
            {
                referenceCount = 1,
                managedObject = managedObject,
            };

            // Push tracked object
            lock (heap)
            {
                heap.Add(tracked);
            }

            // Get the pinned address
            return addr;
        }

        public static void FreeManagedObject(int addr)
        {
            if (heap[addr].referenceCount <= 1)
            {
                lock (heap)
                {
                    // We can release the memory to GC
                    heap.RemoveAt(addr);
                }
            }
            else
            {
                // Get the tracked memory meta
                TrackedMemory mem = heap[addr];

                // Decrease references
                mem.referenceCount--;

                lock (heap)
                {
                    // Update heap collection
                    heap[addr] = mem;
                }
            }
        }

        public static object GetManagedObject(int addr)
        {
            lock (heap)
            {
                return heap[addr].managedObject;
            }
        }

        public static IntPtr Allocate(AppDomain domain, in _CLRTypeHandle type, bool forceStackAlloc, ref byte* stackAllocPtr)
        {
            // Check for interop
            if ((type.flags & _CLRTypeFlags.Interop) != 0)
                throw new NotSupportedException("Only CLR types can be allocated manually - Use activator for interop types");

            // Resolve the type
            Type allocType = domain.ResolveType(type.token);

            // Get interfaces
            Type[] baseInterfaces = allocType.GetInterfaces();

            // Allocates a block or memory on the heap or stack to store a clr instance
            // Instance data is structured as follows:
            // -- Instance base addr
            // [CLRInstance] structure metadata
            // -- Instance addr - main ptr used at runtime
            // [Raw data] raw memory where instance fields are located and is dynamically sized based on instance allocated

            // Calculate allocate size
            uint allocSize = (uint)CLRInstance.Size + type.size + (uint)((baseInterfaces.Length + 1) * sizeof(int));

            // Store pointer
            byte* ptr = null;

            // Allocate CLR instance on heap or stack
            if((type.flags & _CLRTypeFlags.ValueType) != 0 || forceStackAlloc == true)
            {
                // Allocate on the stack
                ptr = stackAllocPtr;

                // Advance ptr
                stackAllocPtr += allocSize;
            }
            else
            {
                // Allocate on the heap
                ptr = (byte*)Marshal.AllocHGlobal((int)allocSize);

                // Zero memory
                __memory.Zero(ptr, allocSize);
            }

            // Get main ptr
            byte* instancePtr = ptr + CLRInstance.Size;

            // Create instance metadata
            *(CLRInstance*)ptr = new CLRInstance
            {
                type = type,
                instancePointer = (IntPtr)instancePtr,
                proxyCount = baseInterfaces.Length + 1,
            };

            // Allocate base proxy object
            *(int*)instancePtr = __memory.PinManagedObject(domain.CreateCLRProxyBindingOrImplicitInteropInstance(allocType.BaseType));

            // Allocate interface proxy objects
            for (int i = 0; i < baseInterfaces.Length; i++)
                *((int*)instancePtr + i + 1) = __memory.PinManagedObject(domain.CreateCLRProxyBinding(baseInterfaces[i]));

            // Get address of object
            return (*(CLRInstance*)ptr).instancePointer;
        }

        public static void Free(IntPtr ptr)
        {
            // Get the instance
            CLRInstance inst = *((CLRInstance*)ptr - CLRInstance.Size);

            // Check for stack allocated
            if ((inst.type.flags & _CLRTypeFlags.ValueType) != 0)
                throw new InvalidOperationException("Cannot free memory that was allocated on the stack");

            // Check for referenced
            if (inst.referenceCount > 0)
                throw new InvalidOperationException("Cannot free memory because it is still being referenced");

            // Free managed objects
            for (int i = 0; i < inst.proxyCount; i++)
                __memory.FreeManagedObject(*((int*)inst.instancePointer + i + i));

            // Free the native memory
            Marshal.FreeHGlobal(ptr);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Copy(void* source, void* destination, long size)
        {
            Buffer.MemoryCopy(source, destination, size, size);       
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Zero(void* mem, uint size)
        {
            for (uint i = 0; i < size; i++)
                *((byte*)mem + i) = 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int SizeOfSlow(Type type)
        {
            // Check null
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            // Check for clr type
            if (type.IsCLRType() == false)
                return Marshal.SizeOf(type);

            // Get allocated size
            if (type.IsValueType == false)
                return Obj.Size;

            // Get allocation size of clr value type
            return SizeOfAllocated((CLRType)type);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int SizeOf(TypeID type)
        {
            switch(type)
            {
                case TypeID.Int8:
                case TypeID.UInt8:
                case TypeID.Int16:
                case TypeID.UInt16: // Small primitives are always promoted to 32bit
                case TypeID.Int32:
                case TypeID.UInt32: return I32.Size;
                case TypeID.Int64:
                case TypeID.UInt64: return I64.Size;
                case TypeID.Single: return F32.Size;
                case TypeID.Double: return F64.Size;

                case TypeID.Object: return Obj.Size;
            }

            throw new NotSupportedException("Unable to get size information for unsupported type id: " + type);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int SizeOfTypedSlow(Type type)
        {
            // Check null
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            // Get type code - this is slow
            TypeCode code = Type.GetTypeCode(type);

            // Get size required
            return SizeOfTyped(code);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int SizeOfTyped(TypeID type)
        {
            switch (type)
            {
                case TypeID.Int8:
                case TypeID.UInt8:
                case TypeID.Int16:
                case TypeID.UInt16: // Small primitives are always promoted to 32bit
                case TypeID.Int32:
                case TypeID.UInt32: return I32.SizeTyped;
                case TypeID.Int64:
                case TypeID.UInt64: return I64.SizeTyped;
                case TypeID.Single: return F32.SizeTyped;
                case TypeID.Double: return F64.SizeTyped;

                case TypeID.Object: return Obj.SizeTyped;
            }

            throw new NotSupportedException("Unable to get size information for unsupported type id: " + type);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int SizeOfTyped(TypeCode type)
        {
            switch (type)
            {
                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Boolean:
                case TypeCode.Char:
                case TypeCode.Int16:
                case TypeCode.UInt16: // Small primitives are always promoted to 32bit
                case TypeCode.Int32:
                case TypeCode.UInt32: return I32.SizeTyped;
                case TypeCode.Int64:
                case TypeCode.UInt64: return I64.SizeTyped;
                case TypeCode.Single: return F32.SizeTyped;
                case TypeCode.Double: return F64.SizeTyped;

                case TypeCode.Object: return Obj.SizeTyped;
            }

            throw new NotSupportedException("Unable to get size information for unsupported type id: " + type);
        }

        public static int SizeOfAllocated(CLRType type)
        {
            throw new NotImplementedException();
        }
    }
}
