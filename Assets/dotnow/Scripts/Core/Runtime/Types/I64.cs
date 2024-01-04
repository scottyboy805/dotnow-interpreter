using System.Runtime.InteropServices;

namespace dotnow.Runtime.Types
{
    [StructLayout(LayoutKind.Explicit)]
    internal struct I64
    {
        // Internal        
        [FieldOffset(0)]
        internal long signed;
        [FieldOffset(0)]
        internal ulong unsigned;
        [FieldOffset(8)]
        internal TypeID type;

        // Public
        public static readonly int Size = sizeof(long);                // Sizeof int64 only
        public static readonly int SizeTyped = sizeof(long) + 1;       // Sizeof int64 + 1 byte type id

        // Methods
        public override string ToString()
        {
            if(type == TypeID.Int64)
                return string.Format("{0}: {1}", type, signed);

            return string.Format("{0}: {1}", type, unsigned);
        }
    }
}
