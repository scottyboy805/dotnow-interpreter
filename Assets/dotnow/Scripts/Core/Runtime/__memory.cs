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
        private List<TrackedMemory> heap = new List<TrackedMemory>(1024);

        // Methods
        public int Pin(object managedObject)
        {
            // Get address
            int addr = heap.Count;

            // Push tracked object
            heap.Add(new TrackedMemory
            {
                referenceCount = 1,
                managedObject = managedObject,
            });

            // Get the pinned address
            return addr;
        }

        public void Free(int addr)
        {
            if (heap[addr].referenceCount <= 1)
            {
                // We can release the memory to GC
                heap.RemoveAt(addr);
            }
            else
            {
                // Get the tracked memory meta
                TrackedMemory mem = heap[addr];

                // Decrease references
                mem.referenceCount--;

                // Update heap collection
                heap[addr] = mem;
            }
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
