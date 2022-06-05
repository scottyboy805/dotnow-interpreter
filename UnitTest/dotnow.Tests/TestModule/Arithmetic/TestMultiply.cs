
namespace TestModule.Arithmetic
{
    public class TestMultiply
    {
        #region SameType
        public static object TestSByteMultiplySByte()
        {
            sbyte a = 10;
            sbyte b = 20;
            return a * b;
        }

        public static object TestShortMultiplyShort()
        {
            short a = 10;
            short b = 20;
            return a * b;
        }

        public static object TestIntMultiplyInt()
        {
            int a = 10;
            int b = 20;
            return a * b;
        }

        public static object TestLongMultiplyLong()
        {
            long a = 10;
            long b = 20;
            return a * b;
        }

        public static object TestByteMultiplyByte()
        {
            byte a = 10;
            byte b = 20;
            return a * b;
        }

        public static object TestUshortMultiplyUshort()
        {
            ushort a = 10;
            ushort b = 20;
            return a * b;
        }

        public static object TestUintMultiplyUint()
        {
            uint a = 10;
            uint b = 20;
            return a * b;
        }

        public static object TestUlongMultiplyUlong()
        {
            ulong a = 10;
            ulong b = 20;
            return a * b;
        }

        public static object TestFloatMultiplyFloat()
        {
            float a = 10;
            float b = 20;
            return a * b;
        }

        public static object TestDoubleMultiplyDouble()
        {
            double a = 10;
            double b = 20;
            return a * b;
        }
        #endregion

        #region DifferentType Sbyte

        public static object TestSByteMultiplyShort()
        {
            sbyte a = 10;
            short b = 20;
            return a * b;
        }

        public static object TestSByteMultiplyInt()
        {
            sbyte a = 10;
            int b = 20;
            return a * b;
        }

        public static object TestSByteMultiplyLong()
        {
            sbyte a = 10;
            long b = 20;
            return a * b;
        }

        public static object TestSByteMultiplyByte()
        {
            sbyte a = 10;
            byte b = 20;
            return a * b;
        }

        public static object TestSByteMultiplyUshort()
        {
            sbyte a = 10;
            ushort b = 20;
            return a * b;
        }

        public static object TestSByteMultiplyUint()
        {
            sbyte a = 10;
            uint b = 20;
            return a * b;
        }

        // Amgiguous operator
        //public static object TestSByteMultiplyUlong()
        //{
        //    sbyte a = 10;
        //    ulong b = 20;
        //    return a * b;
        //}

        public static object TestSByteMultiplyFloat()
        {
            sbyte a = 10;
            float b = 20;
            return a * b;
        }

        public static object TestSByteMultiplyDouble()
        {
            sbyte a = 10;
            double b = 20;
            return a * b;
        }
        #endregion

        #region DifferentType Short
        public static object TestShortMultiplySbyte()
        {
            short a = 10;
            sbyte b = 20;
            return a * b;
        }

        public static object TestShortMultiplyInt()
        {
            short a = 10;
            int b = 20;
            return a * b;
        }

        public static object TestShortMultiplyLong()
        {
            short a = 10;
            long b = 20;
            return a * b;
        }

        public static object TestShortMultiplyByte()
        {
            short a = 10;
            byte b = 20;
            return a * b;
        }

        public static object TestShortMultiplyUshort()
        {
            short a = 10;
            ushort b = 20;
            return a * b;
        }

        public static object TestShortMultiplyUint()
        {
            short a = 10;
            uint b = 20;
            return a * b;
        }

        // Amgiguous operator
        //public static object TestShortMultiplyUlong()
        //{
        //    short a = 10;
        //    ulong b = 20;
        //    return a * b;
        //}

        public static object TestShortMultiplyFloat()
        {
            short a = 10;
            float b = 20;
            return a * b;
        }

        public static object TestShortMultiplyDouble()
        {
            short a = 10;
            double b = 20;
            return a * b;
        }
        #endregion

        #region DifferentType Int
        public static object TestIntMultiplySbyte()
        {
            int a = 10;
            sbyte b = 20;
            return a * b;
        }

        public static object TestIntMultiplyShort()
        {
            int a = 10;
            short b = 20;
            return a * b;
        }

        public static object TestIntMultiplyLong()
        {
            int a = 10;
            long b = 20;
            return a * b;
        }

        public static object TestIntMultiplyByte()
        {
            int a = 10;
            byte b = 20;
            return a * b;
        }

        public static object TestIntMultiplyUshort()
        {
            int a = 10;
            ushort b = 20;
            return a * b;
        }

        public static object TestIntMultiplyUint()
        {
            int a = 10;
            uint b = 20;
            return a * b;
        }

        // Amgiguous operator
        //public static object TestIntMultiplyUlong()
        //{
        //    int a = 10;
        //    ulong b = 20;
        //    return a * b;
        //}

        public static object TestIntMultiplyFloat()
        {
            int a = 10;
            float b = 20;
            return a * b;
        }

        public static object TestIntMultiplyDouble()
        {
            int a = 10;
            double b = 20;
            return a * b;
        }
        #endregion

        #region DifferentType Long
        public static object TestLongMultiplySbyte()
        {
            long a = 10;
            sbyte b = 20;
            return a * b;
        }

        public static object TestLongMultiplyShort()
        {
            long a = 10;
            short b = 20;
            return a * b;
        }

        public static object TestLongMultiplyInt()
        {
            long a = 10;
            int b = 20;
            return a * b;
        }

        public static object TestLongMultiplyByte()
        {
            long a = 10;
            byte b = 20;
            return a * b;
        }

        public static object TestLongMultiplyUshort()
        {
            long a = 10;
            ushort b = 20;
            return a * b;
        }

        public static object TestLongMultiplyUint()
        {
            long a = 10;
            uint b = 20;
            return a * b;
        }

        // Amgiguous operator
        //public static object TestLongMultiplyUlong()
        //{
        //    long a = 10;
        //    ulong b = 20;
        //    return a * b;
        //}

        public static object TestLongMultiplyFloat()
        {
            long a = 10;
            float b = 20;
            return a * b;
        }

        public static object TestLongMultiplyDouble()
        {
            long a = 10;
            double b = 20;
            return a * b;
        }
        #endregion

        #region DifferentType Byte

        public static object TestByteMultiplyShort()
        {
            byte a = 10;
            short b = 20;
            return a * b;
        }

        public static object TestByteMultiplyInt()
        {
            byte a = 10;
            int b = 20;
            return a * b;
        }

        public static object TestByteMultiplyLong()
        {
            byte a = 10;
            long b = 20;
            return a * b;
        }

        public static object TestByteMultiplySbyte()
        {
            byte a = 10;
            sbyte b = 20;
            return a * b;
        }

        public static object TestByteMultiplyUshort()
        {
            byte a = 10;
            ushort b = 20;
            return a * b;
        }

        public static object TestByteMultiplyUint()
        {
            byte a = 10;
            uint b = 20;
            return a * b;
        }

        // Amgiguous operator
        //public static object TestByteMultiplyUlong()
        //{
        //    sbyte a = 10;
        //    ulong b = 20;
        //    return a * b;
        //}

        public static object TestByteMultiplyFloat()
        {
            byte a = 10;
            float b = 20;
            return a * b;
        }

        public static object TestByteMultiplyDouble()
        {
            byte a = 10;
            double b = 20;
            return a * b;
        }
        #endregion

        #region DifferentType Ushort

        public static object TestUshortMultiplyShort()
        {
            ushort a = 10;
            short b = 20;
            return a * b;
        }

        public static object TestUshortMultiplyInt()
        {
            ushort a = 10;
            int b = 20;
            return a * b;
        }

        public static object TestUshortMultiplyLong()
        {
            ushort a = 10;
            long b = 20;
            return a * b;
        }

        public static object TestUshortMultiplySbyte()
        {
            ushort a = 10;
            sbyte b = 20;
            return a * b;
        }

        public static object TestUshortMultiplyByte()
        {
            ushort a = 10;
            byte b = 20;
            return a * b;
        }

        public static object TestUshortMultiplyUint()
        {
            ushort a = 10;
            uint b = 20;
            return a * b;
        }

        public static object TestUshortMultiplyUlong()
        {
            ushort a = 10;
            ulong b = 20;
            return a * b;
        }

        public static object TestUshortMultiplyFloat()
        {
            ushort a = 10;
            float b = 20;
            return a * b;
        }

        public static object TestUshortMultiplyDouble()
        {
            ushort a = 10;
            double b = 20;
            return a * b;
        }
        #endregion

        #region DifferentType Uint

        public static object TestUintMultiplyShort()
        {
            uint a = 10;
            short b = 20;
            return a * b;
        }

        public static object TestUintMultiplyInt()
        {
            uint a = 10;
            int b = 20;
            return a * b;
        }

        public static object TestUintMultiplyLong()
        {
            uint a = 10;
            long b = 20;
            return a * b;
        }

        public static object TestUintMultiplySbyte()
        {
            uint a = 10;
            sbyte b = 20;
            return a * b;
        }

        public static object TestUintMultiplyByte()
        {
            uint a = 10;
            byte b = 20;
            return a * b;
        }

        public static object TestUintMultiplyUshort()
        {
            uint a = 10;
            ushort b = 20;
            return a * b;
        }

        public static object TestUintMultiplyUlong()
        {
            uint a = 10;
            ulong b = 20;
            return a * b;
        }

        public static object TestUintMultiplyFloat()
        {
            uint a = 10;
            float b = 20;
            return a * b;
        }

        public static object TestUintMultiplyDouble()
        {
            uint a = 10;
            double b = 20;
            return a * b;
        }
        #endregion

        #region DifferentType Ulong
        // Amgiguous operator
        //public static object TestUlongMultiplyShort()
        //{
        //    ulong a = 10;
        //    short b = 20;
        //    return a * b;
        //}

        // Amgiguous operator
        //public static object TestUlongMultiplyInt()
        //{
        //    ulong a = 10;
        //    int b = 20;
        //    return a * b;
        //}

        // Amgiguous operator
        //public static object TestUlongMultiplyLong()
        //{
        //    ulong a = 10;
        //    long b = 20;
        //    return a * b;
        //}

        // Amgiguous operator
        //public static object TestUlongMultiplySbyte()
        //{
        //    ulong a = 10;
        //    sbyte b = 20;
        //    return a * b;
        //}

        public static object TestUlongMultiplyByte()
        {
            ulong a = 10;
            byte b = 20;
            return a * b;
        }

        public static object TestUlongMultiplyUshort()
        {
            ulong a = 10;
            ushort b = 20;
            return a * b;
        }

        public static object TestUlongMultiplyUint()
        {
            ulong a = 10;
            ulong b = 20;
            return a * b;
        }

        public static object TestUlongMultiplyFloat()
        {
            ulong a = 10;
            float b = 20;
            return a * b;
        }

        public static object TestUlongMultiplyDouble()
        {
            ulong a = 10;
            double b = 20;
            return a * b;
        }
        #endregion
    }
}
