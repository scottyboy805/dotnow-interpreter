using System.Runtime.InteropServices;

namespace dotnow.Runtime.Types
{
    [StructLayout(LayoutKind.Explicit)]
    internal struct F64
    {
        // Internal
        [FieldOffset(0)]
        internal double value;
        [FieldOffset(8)]
        internal TypeID type;

        // Public
        public static readonly int Size = sizeof(double);               // Sizeof double only
        public static readonly int SizeTyped = Marshal.SizeOf<F64>();   // Sizeof double + 1 byte type id

        // Methods
        public override string ToString()
        {
            return string.Format("{0}: {1}", type, value);
        }
    }
}
