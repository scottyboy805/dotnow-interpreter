using System.Runtime.InteropServices;

namespace dotnow.Runtime.Types
{
    [StructLayout(LayoutKind.Explicit)]
    internal struct F32
    {
        // Internal
        [FieldOffset(0)]
        internal TypeID type;
        [FieldOffset(1)]
        internal float value;

        // Public
        public static readonly int Size = sizeof(float);                // Sizeof single only
        public static readonly int SizeTyped = Marshal.SizeOf<F32>();   // Sizeof single + 1 byte type id

        // Methods
        public override string ToString()
        {
            return string.Format("{0}: {1}", type, value);
        }
    }
}
