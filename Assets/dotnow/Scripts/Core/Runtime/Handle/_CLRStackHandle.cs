using System;

namespace dotnow.Runtime.Handle
{
    [Flags]
    internal enum _CLRStackFlags : ushort
    {
        None = 0,

        Argument = 1 << 1,
        Local = 1 << 2,
        ValueType = 1 << 3,
        Interop = 1 << 4,
    }

    internal unsafe struct _CLRStackHandle
    {
        // Public
        public _CLRTypeHandle stackType;    // The type of the stack data
        public uint offset;                 // The offset from the stack base pointer where the data is stored
        public _CLRStackFlags flags;        // Additional flags describing the stack data

        // Public
        public static readonly int Size = sizeof(_CLRStackHandle);

        // Constructor
        public _CLRStackHandle(Type type, uint offset, bool isArg)
        {
            this.stackType = new _CLRTypeHandle(type);
            this.offset = offset;
            this.flags = (isArg == true) ? _CLRStackFlags.Argument : _CLRStackFlags.Local;

            // Value type
            if ((stackType.flags & _CLRTypeFlags.ValueType) != 0) flags |= _CLRStackFlags.ValueType;

            // Interop
            if ((stackType.flags & _CLRTypeFlags.Interop) != 0) flags |= _CLRStackFlags.Interop;
        }
    }
}
