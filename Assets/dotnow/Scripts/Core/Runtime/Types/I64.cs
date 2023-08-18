using System.Runtime.InteropServices;
using UnityEngine;

namespace dotnow.Runtime.Types
{
    [StructLayout(LayoutKind.Explicit)]
    internal struct I64
    {
        // Internal
        [FieldOffset(0)]
        internal TypeID type;
        [FieldOffset(1)]
        internal long signed;
        [FieldOffset(1)]
        internal ulong unsigned;

        // Public
        public static readonly int Size = sizeof(long);                 // Sizeof int64 only
        public static readonly int SizeTyped = Marshal.SizeOf<I64>();   // Sizeof int64 + 1 byte type id

        // Methods
        public override string ToString()
        {
            if(type == TypeID.Int64)
                return string.Format("{0}: {1}", type, signed);

            return string.Format("{0}: {1}", type, unsigned);
        }
    }
}
