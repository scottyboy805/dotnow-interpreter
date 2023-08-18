﻿using System.Runtime.InteropServices;

namespace dotnow.Runtime.Types
{ 
    [StructLayout(LayoutKind.Explicit)]
    internal struct I32
    {
        // Internal
        [FieldOffset(0)]
        internal TypeID type;
        [FieldOffset(1)]
        internal int signed;
        [FieldOffset(1)]
        internal uint unsigned;

        // Public
        public static readonly int Size = sizeof(int);                  // Sizeof int32 only
        public static readonly int SizeTyped = Marshal.SizeOf<I32>();   // Sizeof int32 + 1 byte type id


        public static readonly I32 ZeroSigned = new I32 { type = TypeID.Int32 };
        public static readonly I32 OneSigned = new I32 { type = TypeID.Int32, signed = 1 };
        public static readonly I32 TwoSigned = new I32 { type = TypeID.Int32, signed = 2 };
        public static readonly I32 ThreeSigned = new I32 { type = TypeID.Int32, signed = 3 };
        public static readonly I32 FourSigned = new I32 { type = TypeID.Int32, signed = 4 };
        public static readonly I32 FiveSigned = new I32 { type = TypeID.Int32, signed = 5 };
        public static readonly I32 SixSigned = new I32 { type = TypeID.Int32, signed = 6 };
        public static readonly I32 SevenSigned = new I32 { type = TypeID.Int32, signed = 7 };
        public static readonly I32 EightSigned = new I32 { type = TypeID.Int32, signed = 8 };
        public static readonly I32 MinusOne = new I32 { type = TypeID.Int32, signed = -1 };

        // Methods
        public override string ToString()
        {
            if(type == TypeID.Int32)
                return string.Format("{0}: {1}", type, signed);

            return string.Format("{0}: {1}", type, unsigned);
        }
    }
}