using System;

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
        public int token;                   // Type token to resolve the System.Reflection.FieldInfo
        public uint offset;                 // The offset into the instance where the field data begins
        public _CLRFieldFlags flags;        // Additional flags describing the field

        // Public
        public static readonly int Size = sizeof(_CLRFieldHandle);
    }
}
