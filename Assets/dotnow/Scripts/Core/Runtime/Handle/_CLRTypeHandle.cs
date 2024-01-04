using System;

namespace dotnow.Runtime.Handle
{
    [Flags]
    internal enum _CLRTypeFlags : ushort
    {
        None = 0,

        Abstract = 1 << 0,
        ValueType = 1 << 1,
        Enum = 1 << 2,
        Array = 1 << 3,
        Interop = 1 << 4,
    }

    internal unsafe struct _CLRTypeHandle
    {
        // Public
        public int token;               // Type token to resolve the System.Type
        public uint size;               // The allocation size of the type
        public _CLRTypeFlags flags;
        public TypeID typeID;

        // Public
        public static readonly int Size = sizeof(_CLRTypeHandle);

        // Constructor
        public _CLRTypeHandle(Type fromType)
        {
            this.token = fromType.MetadataToken;
            this.size = (uint)__memory.SizeOfSlow(fromType);
            this.flags = GetTypeFlags(fromType);
            this.typeID = fromType.GetTypeID();
        }

        // Methods
        internal static _CLRTypeFlags GetTypeFlags(Type fromType)
        {
            _CLRTypeFlags flags = 0;

            // Abstract
            if(fromType.IsAbstract == true) flags |= _CLRTypeFlags.Abstract;

            // Value type
            if(fromType.IsValueType == true) flags |= _CLRTypeFlags.ValueType;

            // Enum
            if(fromType.IsEnum == true) flags |= _CLRTypeFlags.Enum;

            // Array
            if(fromType.IsArray == true) flags |= _CLRTypeFlags.Array;

            // Interop
            if (fromType.IsCLRType() == false) flags |= _CLRTypeFlags.Interop;

            return flags;
        }
    }
}
