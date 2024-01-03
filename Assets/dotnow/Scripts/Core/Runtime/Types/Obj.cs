using System;
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
        internal int typeKey;       // Points to the System.Type token of the object
        [FieldOffset(4)]
        internal IntPtr ptr;        // Points to pinned memory instance - could be 4 or 8 bytes - but must allow space for 8 bytes in all cases

        [FieldOffset(12)]
        internal ObjFlags flags;    // Info about the type of object - struct, boxed, etc
        [FieldOffset(13)]
        internal TypeID type;

        // Public
        public static readonly int Size = Marshal.SizeOf<Obj>();
        public static readonly int SizeTyped = Marshal.SizeOf<Obj>();
        public static readonly Obj Null = default;

        // Properties
        public bool IsNull
        {
            get { return ptr == IntPtr.Zero; }
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
