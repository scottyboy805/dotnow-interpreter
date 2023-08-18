﻿using System;
using System.Runtime.InteropServices;

namespace dotnow.Runtime.Types
{
    [Flags]
    internal enum ObjFlags : byte
    {
        ValueType = 1 << 1,
        BoxedPrimitive = 1 << 2,
    }

    [StructLayout(LayoutKind.Explicit)]
    internal struct Obj
    {
        // Internal
        [FieldOffset(0)]
        internal TypeID type;
        [FieldOffset(1)]
        internal ObjFlags flags;    // Info about the type of object - struct, boxed, etc
        [FieldOffset(2)]
        internal int typeKey;       // Points to the System.Type of the object
        [FieldOffset(6)]
        internal int ptr;           // Points to pinned memory instance

        // Public
        public static readonly int Size = Marshal.SizeOf<Obj>() - 1;
        public static readonly int SizeTyped = Marshal.SizeOf<Obj>();
        public static readonly Obj Null = default;

        // Properties
        public bool IsNull
        {
            get { return ptr == 0; }
        }

        // Methods
        public Type GetRuntimeType(AppDomain domain)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return string.Format("{0}: {1}", type, IsNull ? "null" : ptr.ToString());
        }
    }
}