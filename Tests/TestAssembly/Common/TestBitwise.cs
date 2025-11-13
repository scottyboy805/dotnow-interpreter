namespace TestAssembly
{
    public static class TestBitwise
    {
        // ===== GRANULAR BITWISE AND TESTS BY TYPE =====

        public static object TestBitwiseAndInt()
        {
            return 0xFF & 0xF0;  // int & int
        }

        public static object TestBitwiseAndUInt()
        {
            return 0xFFU & 0xF0U;  // uint & uint
        }

        public static object TestBitwiseAndLong()
        {
            return 0xFFL & 0xF0L;  // long & long
        }

        public static object TestBitwiseAndShort()
        {
            return (short)((short)0xFF & (short)0xF0);  // short & short
        }

        public static object TestBitwiseAndUShort()
        {
            return (ushort)((ushort)0xFF & (ushort)0xF0);  // ushort & ushort
        }

        public static object TestBitwiseAndByte()
        {
            return (byte)((byte)0xFF & (byte)0xF0);  // byte & byte
        }

        public static object TestBitwiseAndSByte()
        {
            return (sbyte)((sbyte)0x7F & (sbyte)0x70);  // sbyte & sbyte
        }

        // ===== GRANULAR BITWISE OR TESTS BY TYPE =====

        public static object TestBitwiseOrInt()
        {
            return 0x0F | 0xF0;  // int | int
        }

        public static object TestBitwiseOrUInt()
        {
            return 0x0FU | 0xF0U;  // uint | uint
        }

        public static object TestBitwiseOrLong()
        {
            return 0x0FL | 0xF0L;  // long | long
        }

        public static object TestBitwiseOrShort()
        {
            return (short)((short)0x0F | (short)0xF0);  // short | short
        }

        public static object TestBitwiseOrUShort()
        {
            return (ushort)((ushort)0x0F | (ushort)0xF0);  // ushort | ushort
        }

        public static object TestBitwiseOrByte()
        {
            return (byte)((byte)0x0F | (byte)0xF0);  // byte | byte
        }

        public static object TestBitwiseOrSByte()
        {
            return (sbyte)((sbyte)0x0F | (sbyte)0x70);  // sbyte | sbyte
        }


        // ===== GRANULAR BITWISE XOR TESTS BY TYPE =====

        public static object TestBitwiseXorInt()
        {
            return 0xFF ^ 0xF0;  // int ^ int
        }

        public static object TestBitwiseXorUInt()
        {
            return 0xFFU ^ 0xF0U;  // uint ^ uint
        }

        public static object TestBitwiseXorLong()
        {
            return 0xFFL ^ 0xF0L;  // long ^ long
        }

        public static object TestBitwiseXorShort()
        {
            return (short)((short)0xFF ^ (short)0xF0);  // short ^ short
        }

        public static object TestBitwiseXorUShort()
        {
            return (ushort)((ushort)0xFF ^ (ushort)0xF0);  // ushort ^ ushort
        }

        public static object TestBitwiseXorByte()
        {
            return (byte)((byte)0xFF ^ (byte)0xF0);  // byte ^ byte
        }

        public static object TestBitwiseXorSByte()
        {
            return (sbyte)((sbyte)0x7F ^ (sbyte)0x70);  // sbyte ^ sbyte
        }

        // ===== GRANULAR BITWISE NOT TESTS BY TYPE =====

        public static object TestBitwiseNotInt()
        {
            return ~0x0F;  // ~int
        }

        public static object TestBitwiseNotUInt()
        {
            return ~0x0FU;  // ~uint
        }

        public static object TestBitwiseNotLong()
        {
            return ~0x0FL;  // ~long
        }

        public static object TestBitwiseNotShort()
        {
            return (short)(~(short)0x0F);  // ~short
        }

        public static object TestBitwiseNotUShort()
        {
            return unchecked((ushort)(~(ushort)0x0F));  // ~ushort
        }

        public static object TestBitwiseNotByte()
        {
            return unchecked((byte)(~(byte)0x0F));  // ~byte
        }


        // ===== GRANULAR LEFT SHIFT TESTS BY TYPE =====

        public static object TestLeftShiftInt()
        {
            return 1 << 4;  // int << int
        }

        public static object TestLeftShiftUInt()
        {
            return 1U << 4;  // uint << int
        }

        public static object TestLeftShiftLong()
        {
            return 1L << 4;  // long << int
        }

        public static object TestLeftShiftShort()
        {
            return (short)((short)1 << 4);  // short << int
        }

        public static object TestLeftShiftUShort()
        {
            return (ushort)((ushort)1 << 4);  // ushort << int
        }

        public static object TestLeftShiftByte()
        {
            return (byte)((byte)1 << 4);  // byte << int
        }

        public static object TestLeftShiftSByte()
        {
            return (sbyte)((sbyte)1 << 4);  // sbyte << int
        }

        // ===== GRANULAR RIGHT SHIFT TESTS BY TYPE =====

        public static object TestRightShiftInt()
        {
            return 16 >> 2;  // int >> int
        }

        public static object TestRightShiftUInt()
        {
            return 16U >> 2;  // uint >> int
        }

        public static object TestRightShiftLong()
        {
            return 16L >> 2;  // long >> int
        }

        public static object TestRightShiftShort()
        {
            return (short)((short)16 >> 2);  // short >> int
        }

        public static object TestRightShiftUShort()
        {
            return (ushort)((ushort)16 >> 2);  // ushort >> int
        }

        public static object TestRightShiftByte()
        {
            return (byte)((byte)16 >> 2);  // byte >> int
        }

        public static object TestRightShiftSByte()
        {
            return (sbyte)((sbyte)16 >> 2);  // sbyte >> int
        }
    }
}