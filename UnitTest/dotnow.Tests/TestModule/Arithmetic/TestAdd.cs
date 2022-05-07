
namespace TestModule
{
    public class TestAdd
    {
        #region SameType
        public static object TestSByteAddSByte()
        {
            sbyte a = 10;
            sbyte b = 20;
            return a + b;
        }

        public static object TestShortAddShort()
        {
            short a = 10;
            short b = 20;
            return a + b;
        }

        public static object TestIntAddInt()
        {
            int a = 10;
            int b = 20;
            return a + b;
        }

        public static object TestLongAddLong()
        {
            long a = 10;
            long b = 20;
            return a + b;
        }

        public static object TestByteAddByte()
        {
            byte a = 10;
            byte b = 20;
            return a + b;
        }

        public static object TestUshortAddUshort()
        {
            ushort a = 10;
            ushort b = 20;
            return a + b;
        }

        public static object TestUintAddUint()
        {
            uint a = 10;
            uint b = 20;
            return a + b;
        }

        public static object TestUlongAddUlong()
        {
            ulong a = 10;
            ulong b = 20;
            return a + b;
        }

        public static object TestFloatAddFloat()
        {
            float a = 10;
            float b = 20;
            return a + b;
        }

        public static object TestDoubleAddDouble()
        {
            double a = 10;
            double b = 20;
            return a + b;
        }
        #endregion

        #region DifferentType Sbyte

        public static object TestSByteAddShort()
        {
            sbyte a = 10;
            short b = 20;
            return a + b;
        }

        public static object TestSByteAddInt()
        {
            sbyte a = 10;
            int b = 20;
            return a + b;
        }

        public static object TestSByteAddLong()
        {
            sbyte a = 10;
            long b = 20;
            return a + b;
        }

        public static object TestSByteAddByte()
        {
            sbyte a = 10;
            byte b = 20;
            return a + b;
        }

        public static object TestSByteAddUshort()
        {
            sbyte a = 10;
            ushort b = 20;
            return a + b;
        }

        public static object TestSByteAddUint()
        {
            sbyte a = 10;
            uint b = 20;
            return a + b;
        }

        // Amgiguous operator
        //public static object TestSByteAddUlong()
        //{
        //    sbyte a = 10;
        //    ulong b = 20;
        //    return a + b;
        //}

        public static object TestSByteAddFloat()
        {
            sbyte a = 10;
            float b = 20;
            return a + b;
        }

        public static object TestSByteAddDouble()
        {
            sbyte a = 10;
            double b = 20;
            return a + b;
        }
        #endregion

        #region DifferentType Short
        public static object TestShortAddSbyte()
        {
            short a = 10;
            sbyte b = 20;
            return a + b;
        }

        public static object TestShortAddInt()
        {
            short a = 10;
            int b = 20;
            return a + b;
        }

        public static object TestShortAddLong()
        {
            short a = 10;
            long b = 20;
            return a + b;
        }

        public static object TestShortAddByte()
        {
            short a = 10;
            byte b = 20;
            return a + b;
        }

        public static object TestShortAddUshort()
        {
            short a = 10;
            ushort b = 20;
            return a + b;
        }

        public static object TestShortAddUint()
        {
            short a = 10;
            uint b = 20;
            return a + b;
        }

        // Amgiguous operator
        //public static object TestShortAddUlong()
        //{
        //    short a = 10;
        //    ulong b = 20;
        //    return a + b;
        //}

        public static object TestShortAddFloat()
        {
            short a = 10;
            float b = 20;
            return a + b;
        }

        public static object TestShortAddDouble()
        {
            short a = 10;
            double b = 20;
            return a + b;
        }
        #endregion

        #region DifferentType Int
        public static object TestIntAddSbyte()
        {
            int a = 10;
            sbyte b = 20;
            return a + b;
        }

        public static object TestIntAddShort()
        {
            int a = 10;
            short b = 20;
            return a + b;
        }

        public static object TestIntAddLong()
        {
            int a = 10;
            long b = 20;
            return a + b;
        }

        public static object TestIntAddByte()
        {
            int a = 10;
            byte b = 20;
            return a + b;
        }

        public static object TestIntAddUshort()
        {
            int a = 10;
            ushort b = 20;
            return a + b;
        }

        public static object TestIntAddUint()
        {
            int a = 10;
            uint b = 20;
            return a + b;
        }

        // Amgiguous operator
        //public static object TestIntAddUlong()
        //{
        //    int a = 10;
        //    ulong b = 20;
        //    return a + b;
        //}

        public static object TestIntAddFloat()
        {
            int a = 10;
            float b = 20;
            return a + b;
        }

        public static object TestIntAddDouble()
        {
            int a = 10;
            double b = 20;
            return a + b;
        }
        #endregion

        #region DifferentType Long
        public static object TestLongAddSbyte()
        {
            long a = 10;
            sbyte b = 20;
            return a + b;
        }

        public static object TestLongAddShort()
        {
            long a = 10;
            short b = 20;
            return a + b;
        }

        public static object TestLongAddInt()
        {
            long a = 10;
            int b = 20;
            return a + b;
        }

        public static object TestLongAddByte()
        {
            long a = 10;
            byte b = 20;
            return a + b;
        }

        public static object TestLongAddUshort()
        {
            long a = 10;
            ushort b = 20;
            return a + b;
        }

        public static object TestLongAddUint()
        {
            long a = 10;
            uint b = 20;
            return a + b;
        }

        // Amgiguous operator
        //public static object TestLongAddUlong()
        //{
        //    long a = 10;
        //    ulong b = 20;
        //    return a + b;
        //}

        public static object TestLongAddFloat()
        {
            long a = 10;
            float b = 20;
            return a + b;
        }

        public static object TestLongAddDouble()
        {
            long a = 10;
            double b = 20;
            return a + b;
        }
        #endregion

        #region DifferentType Byte

        public static object TestByteAddShort()
        {
            byte a = 10;
            short b = 20;
            return a + b;
        }

        public static object TestByteAddInt()
        {
            byte a = 10;
            int b = 20;
            return a + b;
        }

        public static object TestByteAddLong()
        {
            byte a = 10;
            long b = 20;
            return a + b;
        }

        public static object TestByteAddSbyte()
        {
            byte a = 10;
            sbyte b = 20;
            return a + b;
        }

        public static object TestByteAddUshort()
        {
            byte a = 10;
            ushort b = 20;
            return a + b;
        }

        public static object TestByteAddUint()
        {
            byte a = 10;
            uint b = 20;
            return a + b;
        }

        // Amgiguous operator
        //public static object TestByteAddUlong()
        //{
        //    sbyte a = 10;
        //    ulong b = 20;
        //    return a + b;
        //}

        public static object TestByteAddFloat()
        {
            byte a = 10;
            float b = 20;
            return a + b;
        }

        public static object TestByteAddDouble()
        {
            byte a = 10;
            double b = 20;
            return a + b;
        }
        #endregion

        #region DifferentType Ushort

        public static object TestUshortAddShort()
        {
            ushort a = 10;
            short b = 20;
            return a + b;
        }

        public static object TestUshortAddInt()
        {
            ushort a = 10;
            int b = 20;
            return a + b;
        }

        public static object TestUshortAddLong()
        {
            ushort a = 10;
            long b = 20;
            return a + b;
        }

        public static object TestUshortAddSbyte()
        {
            ushort a = 10;
            sbyte b = 20;
            return a + b;
        }

        public static object TestUshortAddByte()
        {
            ushort a = 10;
            byte b = 20;
            return a + b;
        }

        public static object TestUshortAddUint()
        {
            ushort a = 10;
            uint b = 20;
            return a + b;
        }

        public static object TestUshortAddUlong()
        {
            ushort a = 10;
            ulong b = 20;
            return a + b;
        }

        public static object TestUshortAddFloat()
        {
            ushort a = 10;
            float b = 20;
            return a + b;
        }

        public static object TestUshortAddDouble()
        {
            ushort a = 10;
            double b = 20;
            return a + b;
        }
        #endregion

        #region DifferentType Uint

        public static object TestUintAddShort()
        {
            uint a = 10;
            short b = 20;
            return a + b;
        }

        public static object TestUintAddInt()
        {
            uint a = 10;
            int b = 20;
            return a + b;
        }

        public static object TestUintAddLong()
        {
            uint a = 10;
            long b = 20;
            return a + b;
        }

        public static object TestUintAddSbyte()
        {
            uint a = 10;
            sbyte b = 20;
            return a + b;
        }

        public static object TestUintAddByte()
        {
            uint a = 10;
            byte b = 20;
            return a + b;
        }

        public static object TestUintAddUshort()
        {
            uint a = 10;
            ushort b = 20;
            return a + b;
        }

        public static object TestUintAddUlong()
        {
            uint a = 10;
            ulong b = 20;
            return a + b;
        }

        public static object TestUintAddFloat()
        {
            uint a = 10;
            float b = 20;
            return a + b;
        }

        public static object TestUintAddDouble()
        {
            uint a = 10;
            double b = 20;
            return a + b;
        }
        #endregion

        #region DifferentType Ulong
        // Amgiguous operator
        //public static object TestUlongAddShort()
        //{
        //    ulong a = 10;
        //    short b = 20;
        //    return a + b;
        //}

        // Amgiguous operator
        //public static object TestUlongAddInt()
        //{
        //    ulong a = 10;
        //    int b = 20;
        //    return a + b;
        //}

        // Amgiguous operator
        //public static object TestUlongAddLong()
        //{
        //    ulong a = 10;
        //    long b = 20;
        //    return a + b;
        //}

        // Amgiguous operator
        //public static object TestUlongAddSbyte()
        //{
        //    ulong a = 10;
        //    sbyte b = 20;
        //    return a + b;
        //}

        public static object TestUlongAddByte()
        {
            ulong a = 10;
            byte b = 20;
            return a + b;
        }

        public static object TestUlongAddUshort()
        {
            ulong a = 10;
            ushort b = 20;
            return a + b;
        }

        public static object TestUlongAddUint()
        {
            ulong a = 10;
            ulong b = 20;
            return a + b;
        }

        public static object TestUlongAddFloat()
        {
            ulong a = 10;
            float b = 20;
            return a + b;
        }

        public static object TestUlongAddDouble()
        {
            ulong a = 10;
            double b = 20;
            return a + b;
        }
        #endregion
    }
}
