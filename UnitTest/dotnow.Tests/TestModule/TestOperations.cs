using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TestModule
{
    public class TestOperations
    {
        #region Add
        public static int TestAdditionSByte()
        {
            sbyte a = 2;
            sbyte b = 3;

            return a + b;
        }

        public static int TestAdditionInt16()
        {
            short a = 2;
            short b = 3;

            return a + b;
        }

        public static int TestAdditionInt32()
        {
            int a = 2;
            int b = 3;

            return a + b;
        }

        public static int TestAdditionInt64()
        {
            long a = 2;
            long b = 3;

            return (int)(a + b);
        }

        public static int TestAdditionByte()
        {
            byte a = 2;
            byte b = 3;

            return a + b;
        }

        public static int TestAdditionUInt16()
        {
            ushort a = 2;
            ushort b = 3;

            return a + b;
        }

        public static int TestAdditionUInt32()
        {
            uint a = 2;
            uint b = 3;

            return (int)(a + b);
        }

        public static int TestAdditionUInt64()
        {
            ulong a = 2;
            ulong b = 3;

            return (int)(a + b);
        }

        public static float TestAdditionSingle()
        {
            float a = 2;
            float b = 3;

            return a + b;
        }

        public static double TestAdditionDouble()
        {
            double a = 2;
            double b = 3;

            return a + b;
        }
        #endregion

        public static int TestSubtraction()
        {
            int a = 4;
            int b = 2;

            return a - b;
        }

        public static int TestMultiplication()
        {
            int a = 2;
            int b = 3;

            return a * b;
        }

        public static int TestDivide()
        {
            int a = 10;
            int b = 5;

            return a / b;
        }

        public static int TestBitshiftLeft()
        {
            int a = 4;

            a = a << 1;

            return a;
        }

        public static int TestBitshiftRight()
        {
            int a = 4;

            a = a >> 1;

            return a;
        }

        public static bool TestNot()
        {
            bool a = false;

            return !a;
        }

        public static bool TestLessThan1()
        {
            int a = 10;
            int b = 5;

            return a < b;
        }

        public static bool TestLessThan2()
        {
            int a = 5;
            int b = 10;

            return a < b;
        }

        public static bool TestLessThanEqual1()
        {
            int a = 10;
            int b = 5;

            return a <= b;
        }

        public static bool TestLessThanEqual2()
        {
            int a = 5;
            int b = 10;

            return a <= b;
        }

        public static bool TestLessThanEqual3()
        {
            int a = 5;
            int b = 5;

            return a <= b;
        }

        public static bool TestGreaterThan1()
        {
            int a = 10;
            int b = 5;

            return a > b;
        }

        public static bool TestGreaterThan2()
        {
            int a = 5;
            int b = 10;

            return a > b;
        }

        public static bool TestGreaterThanEqual1()
        {
            int a = 10;
            int b = 5;

            return a >= b;
        }

        public static bool TestGreaterThanEqual2()
        {
            int a = 5;
            int b = 10;

            return a >= b;
        }

        public static bool TestGreaterThanEqual3()
        {
            int a = 5;
            int b = 5;

            return a >= b;
        }

        public static bool TestEqual1()
        {
            int a = 5;
            int b = 5;

            return a == b;
        }

        public static bool TestEqual2()
        {
            object a = 5;
            object b = 5;

            return a == b;
        }

        public static bool TestEqual3()
        {
            object a = new object();
            object b = a;

            return a == b;
        }

        public static bool TestNotEqual1()
        {
            int a = 5;
            int b = 5;

            return a != b;
        }

        public static bool TestNotEqual2()
        {
            object a = 5;
            object b = 5;

            return a != b;
        }

        public static bool TestNotEqual3()
        {
            object a = new object();
            object b = a;

            return a != b;
        }
    }
}
