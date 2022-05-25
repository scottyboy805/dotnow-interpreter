
namespace TestModule
{
    public class TestSubtract
    {
        #region SameType
        public static object TestSByteSubtractSByte()
        {
            sbyte a = 10;
            sbyte b = 20;
            return a - b;
        }

        public static object TestShortSubtractShort()
        {
            short a = 10;
            short b = 20;
            return a - b;
        }

        public static object TestIntSubtractInt()
        {
            int a = 10;
            int b = 20;
            return a - b;
        }

        public static object TestLongSubtractLong()
        {
            long a = 10;
            long b = 20;
            return a - b;
        }

        public static object TestByteSubtractByte()
        {
            byte a = 10;
            byte b = 20;
            return a - b;
        }

        public static object TestUshortSubtractUshort()
        {
            ushort a = 10;
            ushort b = 20;
            return a - b;
        }

        public static object TestUintSubtractUint()
        {
            uint a = 10;
            uint b = 20;
            return a - b;
        }

        public static object TestUlongSubtractUlong()
        {
            ulong a = 10;
            ulong b = 20;
            return a - b;
        }

        public static object TestFloatSubtractFloat()
        {
            float a = 10;
            float b = 20;
            return a - b;
        }

        public static object TestDoubleSubtractDouble()
        {
            double a = 10;
            double b = 20;
            return a - b;
        }
        #endregion

        #region DifferentType Sbyte

        public static object TestSByteSubtractShort()
        {
            sbyte a = 10;
            short b = 20;
            return a - b;
        }

        public static object TestSByteSubtractInt()
        {
            sbyte a = 10;
            int b = 20;
            return a - b;
        }

        public static object TestSByteSubtractLong()
        {
            sbyte a = 10;
            long b = 20;
            return a - b;
        }

        public static object TestSByteSubtractByte()
        {
            sbyte a = 10;
            byte b = 20;
            return a - b;
        }

        public static object TestSByteSubtractUshort()
        {
            sbyte a = 10;
            ushort b = 20;
            return a - b;
        }

        public static object TestSByteSubtractUint()
        {
            sbyte a = 10;
            uint b = 20;
            return a - b;
        }

        // Amgiguous operator
        //public static object TestSByteSubtractUlong()
        //{
        //    sbyte a = 10;
        //    ulong b = 20;
        //    return a - b;
        //}

        public static object TestSByteSubtractFloat()
        {
            sbyte a = 10;
            float b = 20;
            return a - b;
        }

        public static object TestSByteSubtractDouble()
        {
            sbyte a = 10;
            double b = 20;
            return a - b;
        }
        #endregion

        #region DifferentType Short
        public static object TestShortSubtractSbyte()
        {
            short a = 10;
            sbyte b = 20;
            return a - b;
        }

        public static object TestShortSubtractInt()
        {
            short a = 10;
            int b = 20;
            return a - b;
        }

        public static object TestShortSubtractLong()
        {
            short a = 10;
            long b = 20;
            return a - b;
        }

        public static object TestShortSubtractByte()
        {
            short a = 10;
            byte b = 20;
            return a - b;
        }

        public static object TestShortSubtractUshort()
        {
            short a = 10;
            ushort b = 20;
            return a - b;
        }

        public static object TestShortSubtractUint()
        {
            short a = 10;
            uint b = 20;
            return a - b;
        }

        // Amgiguous operator
        //public static object TestShortSubtractUlong()
        //{
        //    short a = 10;
        //    ulong b = 20;
        //    return a - b;
        //}

        public static object TestShortSubtractFloat()
        {
            short a = 10;
            float b = 20;
            return a - b;
        }

        public static object TestShortSubtractDouble()
        {
            short a = 10;
            double b = 20;
            return a - b;
        }
        #endregion

        #region DifferentType Int
        public static object TestIntSubtractSbyte()
        {
            int a = 10;
            sbyte b = 20;
            return a - b;
        }

        public static object TestIntSubtractShort()
        {
            int a = 10;
            short b = 20;
            return a - b;
        }

        public static object TestIntSubtractLong()
        {
            int a = 10;
            long b = 20;
            return a - b;
        }

        public static object TestIntSubtractByte()
        {
            int a = 10;
            byte b = 20;
            return a - b;
        }

        public static object TestIntSubtractUshort()
        {
            int a = 10;
            ushort b = 20;
            return a - b;
        }

        public static object TestIntSubtractUint()
        {
            int a = 10;
            uint b = 20;
            return a - b;
        }

        // Amgiguous operator
        //public static object TestIntSubtractUlong()
        //{
        //    int a = 10;
        //    ulong b = 20;
        //    return a - b;
        //}

        public static object TestIntSubtractFloat()
        {
            int a = 10;
            float b = 20;
            return a - b;
        }

        public static object TestIntSubtractDouble()
        {
            int a = 10;
            double b = 20;
            return a - b;
        }
        #endregion

        #region DifferentType Long
        public static object TestLongSubtractSbyte()
        {
            long a = 10;
            sbyte b = 20;
            return a - b;
        }

        public static object TestLongSubtractShort()
        {
            long a = 10;
            short b = 20;
            return a - b;
        }

        public static object TestLongSubtractInt()
        {
            long a = 10;
            int b = 20;
            return a - b;
        }

        public static object TestLongSubtractByte()
        {
            long a = 10;
            byte b = 20;
            return a - b;
        }

        public static object TestLongSubtractUshort()
        {
            long a = 10;
            ushort b = 20;
            return a - b;
        }

        public static object TestLongSubtractUint()
        {
            long a = 10;
            uint b = 20;
            return a - b;
        }

        // Amgiguous operator
        //public static object TestLongSubtractUlong()
        //{
        //    long a = 10;
        //    ulong b = 20;
        //    return a - b;
        //}

        public static object TestLongSubtractFloat()
        {
            long a = 10;
            float b = 20;
            return a - b;
        }

        public static object TestLongSubtractDouble()
        {
            long a = 10;
            double b = 20;
            return a - b;
        }
        #endregion

        #region DifferentType Byte

        public static object TestByteSubtractShort()
        {
            byte a = 10;
            short b = 20;
            return a - b;
        }

        public static object TestByteSubtractInt()
        {
            byte a = 10;
            int b = 20;
            return a - b;
        }

        public static object TestByteSubtractLong()
        {
            byte a = 10;
            long b = 20;
            return a - b;
        }

        public static object TestByteSubtractSbyte()
        {
            byte a = 10;
            sbyte b = 20;
            return a - b;
        }

        public static object TestByteSubtractUshort()
        {
            byte a = 10;
            ushort b = 20;
            return a - b;
        }

        public static object TestByteSubtractUint()
        {
            byte a = 10;
            uint b = 20;
            return a - b;
        }

        // Amgiguous operator
        //public static object TestByteSubtractUlong()
        //{
        //    sbyte a = 10;
        //    ulong b = 20;
        //    return a - b;
        //}

        public static object TestByteSubtractFloat()
        {
            byte a = 10;
            float b = 20;
            return a - b;
        }

        public static object TestByteSubtractDouble()
        {
            byte a = 10;
            double b = 20;
            return a - b;
        }
        #endregion

        #region DifferentType Ushort

        public static object TestUshortSubtractShort()
        {
            ushort a = 10;
            short b = 20;
            return a - b;
        }

        public static object TestUshortSubtractInt()
        {
            ushort a = 10;
            int b = 20;
            return a - b;
        }

        public static object TestUshortSubtractLong()
        {
            ushort a = 10;
            long b = 20;
            return a - b;
        }

        public static object TestUshortSubtractSbyte()
        {
            ushort a = 10;
            sbyte b = 20;
            return a - b;
        }

        public static object TestUshortSubtractByte()
        {
            ushort a = 10;
            byte b = 20;
            return a - b;
        }

        public static object TestUshortSubtractUint()
        {
            ushort a = 10;
            uint b = 20;
            return a - b;
        }

        public static object TestUshortSubtractUlong()
        {
            ushort a = 10;
            ulong b = 20;
            return a - b;
        }

        public static object TestUshortSubtractFloat()
        {
            ushort a = 10;
            float b = 20;
            return a - b;
        }

        public static object TestUshortSubtractDouble()
        {
            ushort a = 10;
            double b = 20;
            return a - b;
        }
        #endregion

        #region DifferentType Uint

        public static object TestUintSubtractShort()
        {
            uint a = 10;
            short b = 20;
            return a - b;
        }

        public static object TestUintSubtractInt()
        {
            uint a = 10;
            int b = 20;
            return a - b;
        }

        public static object TestUintSubtractLong()
        {
            uint a = 10;
            long b = 20;
            return a - b;
        }

        public static object TestUintSubtractSbyte()
        {
            uint a = 10;
            sbyte b = 20;
            return a - b;
        }

        public static object TestUintSubtractByte()
        {
            uint a = 10;
            byte b = 20;
            return a - b;
        }

        public static object TestUintSubtractUshort()
        {
            uint a = 10;
            ushort b = 20;
            return a - b;
        }

        public static object TestUintSubtractUlong()
        {
            uint a = 10;
            ulong b = 20;
            return a - b;
        }

        public static object TestUintSubtractFloat()
        {
            uint a = 10;
            float b = 20;
            return a - b;
        }

        public static object TestUintSubtractDouble()
        {
            uint a = 10;
            double b = 20;
            return a - b;
        }
        #endregion

        #region DifferentType Ulong
        // Amgiguous operator
        //public static object TestUlongSubtractShort()
        //{
        //    ulong a = 10;
        //    short b = 20;
        //    return a - b;
        //}

        // Amgiguous operator
        //public static object TestUlongSubtractInt()
        //{
        //    ulong a = 10;
        //    int b = 20;
        //    return a - b;
        //}

        // Amgiguous operator
        //public static object TestUlongSubtractLong()
        //{
        //    ulong a = 10;
        //    long b = 20;
        //    return a - b;
        //}

        // Amgiguous operator
        //public static object TestUlongSubtractSbyte()
        //{
        //    ulong a = 10;
        //    sbyte b = 20;
        //    return a - b;
        //}

        public static object TestUlongSubtractByte()
        {
            ulong a = 10;
            byte b = 20;
            return a - b;
        }

        public static object TestUlongSubtractUshort()
        {
            ulong a = 10;
            ushort b = 20;
            return a - b;
        }

        public static object TestUlongSubtractUint()
        {
            ulong a = 10;
            ulong b = 20;
            return a - b;
        }

        public static object TestUlongSubtractFloat()
        {
            ulong a = 10;
            float b = 20;
            return a - b;
        }

        public static object TestUlongSubtractDouble()
        {
            ulong a = 10;
            double b = 20;
            return a - b;
        }
        #endregion
    }
}
