using System.Runtime.InteropServices;

namespace dotnow.Runtime.Types
{
    [StructLayout(LayoutKind.Explicit)]
    internal struct F32
    {
        // Internal
        [FieldOffset(0)]
        internal float value;
        [FieldOffset(4)]
        internal TypeID type;

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
