using System;
using System.Runtime.CompilerServices;

namespace dotnow.Runtime.Handle
{
    [Flags]
    internal enum _CLRFieldFlags : ushort
    {
        None = 0,

        Static = 1 << 0,        // Field is static - no instance
        Interop = 1 << 1,       // Field is defined in an interop assembly - must be called via reflection
    }

    internal unsafe struct _CLRFieldHandle
    {
        // Public
        public _CLRTypeHandle fieldType;    // The type of the field
        public int fieldToken;              // Type token to resolve the System.Reflection.FieldInfo
        public uint offset;                 // The offset into the instance where the field data begins
        public _CLRFieldFlags flags;        // Additional flags describing the field

        // Public
        public static readonly int Size = sizeof(_CLRFieldHandle);

        // Methods
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ReadFieldMemory(IntPtr instance, void* dest)
        {
            // Get offset ptr
            void* ptr = (void*)(instance + (int)offset);

            // Copy memory
            __memory.Copy(ptr, dest, fieldType.size);

            // Get size
            return (int)fieldType.size;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ReadFieldMemoryTypeID(IntPtr instance, void* dest)
        {
            // Get offset ptr
            void* ptr = (void*)(instance + (int)offset);

            // Copy memory
            __memory.Copy(ptr, dest, fieldType.size);

            // Set last value as type id
            *(TypeID*)((byte*)dest + fieldType.size) = fieldType.typeID;

            // Get size
            return (int)fieldType.size + 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int WriteFieldMemory(IntPtr instance, void* src)
        {
            // Get offset ptr
            void* ptr = (void*)(instance + (int)offset);

            // Copy memory
            __memory.Copy(src, ptr, fieldType.size);

            // Get size
            return (int)fieldType.size;
        }
    }
}
