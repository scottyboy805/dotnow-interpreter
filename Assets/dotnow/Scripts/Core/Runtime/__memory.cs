using dotnow.Runtime.Handle;
using dotnow.Runtime.Types;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEditor.Build.Content;

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
            Type allocType = domain.ResolveType(type.typeToken);

            // Get interfaces
            Type[] baseInterfaces = allocType.GetInterfaces();

            // Allocates a block or memory on the heap or stack to store a clr instance
            // Instance data is structured as follows:
            // -- Instance base addr
            // [CLRInstance] structure metadata
            // -- Reference Counter
            // -- Instance addr - main ptr used at runtime
            // [Raw data] raw memory where instance fields are located and is dynamically sized based on instance allocated

            // Calculate allocate size
            uint allocSize = (uint)CLRInstance.Size 
                + type.size                                             // Size required by instance fields
                + (uint)((baseInterfaces.Length + 1) * sizeof(int))     // Array of int indexes for base and interface proxies
                + sizeof(int);                                          // Reference counter

            // Store pointer
            byte* ptr = null;

            // Check for stack alloc
            bool stackAlloc = (type.flags & _CLRTypeFlags.ValueType) != 0 || forceStackAlloc == true;

            // Allocate CLR instance on heap or stack
            if (stackAlloc == true)
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
            byte* instancePtr = ptr + CLRInstance.Size + sizeof(int);

            // Create instance metadata
            *(CLRInstance*)ptr = new CLRInstance
            {
                type = type,
                instancePointer = (IntPtr)instancePtr,
                proxyCount = baseInterfaces.Length + 1,
            };

            // Stack alloc flag
            if (stackAlloc == true)
                (*(CLRInstance*)ptr).type.flags |= _CLRTypeFlags.StackAlloc;

            // Allocate base proxy object
            *((int*)instancePtr + 1) = __memory.PinManagedObject(domain.CreateCLRProxyBindingOrImplicitInteropInstance(allocType.BaseType));

            // Allocate interface proxy objects
            for (int i = 0; i < baseInterfaces.Length; i++)
                *((int*)instancePtr + i + 2) = __memory.PinManagedObject(domain.CreateCLRProxyBinding(baseInterfaces[i]));

            // Get address of object
            return (*(CLRInstance*)ptr).instancePointer;
        }

        public static IntPtr AllocateInterop(AppDomain domain, Type interopType, ConstructorInfo ctor, object[] args)
        {
            // Create instance
            object instance = domain.CreateInstance(interopType, ctor, args);

            // Pin managed object
            return (IntPtr)__memory.PinManagedObject(instance);
        }

        public static IntPtr AllocateArray(AppDomain domain, in _CLRTypeHandle elementType, long length, bool stackAlloc, ref byte* stackPtr)
        {
            // Check for interop
            if ((elementType.flags & _CLRTypeFlags.Interop) != 0)
                throw new NotSupportedException("Only CLR arrays can be allocated manually - Use activator for interop types");

            // Resolve the type
            Type allocType = domain.ResolveType(elementType.typeToken);

            // Get interfaces
            Type[] baseInterfaces = allocType.GetInterfaces();

            // Calculate allocate size
            //uint allocSize = (uint)CLRInstance.Size
            //    + (elementType.size * length)                           // Size required by instance fields
            //    + (uint)((baseInterfaces.Length + 1) * sizeof(int))     // Array of int indexes for base and interface proxies
            //    + sizeof(int);

            return IntPtr.Zero;
        }

        public static void Free(IntPtr ptr)
        {
            // Get the instance
            CLRInstance inst = *((CLRInstance*)ptr - CLRInstance.Size);

            // Check for stack allocated
            if ((inst.type.flags & _CLRTypeFlags.StackAlloc) != 0)
                throw new InvalidOperationException("Cannot free memory that was allocated on the stack");

            // Check for referenced
            if (inst.IsReferenced == true)
                throw new InvalidOperationException("Cannot free memory because it is still being referenced");

            // Free managed objects
            for (int i = 0; i < inst.proxyCount; i++)
                __memory.FreeManagedObject(*((int*)inst.instancePointer + i + 1));

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

            // Get reference size
            if (type.IsClass == true)
                return Obj.Size;

            // Check for user value type
            if(type.IsValueType == true && type.IsCLRType() == true && type.IsPrimitive == false)
                return (int)((CLRType)type).Handle.size;

            // Get allocation size of clr value type
            return SizeOf(Type.GetTypeCode(type));
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
        public static int SizeOf(TypeCode type)
        {
            switch (type)
            {
                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16: // Small primitives are always promoted to 32bit
                case TypeCode.Int32:
                case TypeCode.UInt32: return I32.Size;
                case TypeCode.Int64:
                case TypeCode.UInt64: return I64.Size;
                case TypeCode.Single: return F32.Size;
                case TypeCode.Double: return F64.Size;

                case TypeCode.Object: return Obj.Size;
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

        public static int SizeOfInstance(CLRType type)
        {
            throw new NotImplementedException();
        }
    }
}
