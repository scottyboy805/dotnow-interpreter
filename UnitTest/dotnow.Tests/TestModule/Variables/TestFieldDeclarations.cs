
namespace TestModule
{
    public class TestFieldDeclarations
    {
        // Instance
        sbyte sbyteInstance = 10;
        short shortInstance = 20;
        int intInstance = 30;
        long longInstance = 40;
        byte byteInstance = 50;
        ushort ushortInstance = 60;
        uint uintInstance = 70;
        ulong ulongInstance = 80;

        float floatInstance = 90.25f;
        double doubleInstance = 100.25d;

        string stringInstance = "Hello World";

        // Static
        static sbyte sbyteStatic = 10;
        static short shortStatic = 20;
        static int intStatic = 30;
        static long longStatic = 40;
        static byte byteStatic = 50;
        static ushort ushortStatic = 60;
        static uint uintStatic = 70;
        static ulong ulongStatic = 80;

        static float floatStatic = 90.25f;
        static double doubleStatic = 100.25d;

        static string stringStatic = "Hello World";

        // Methods
        public static object TestSByteInstance()
        {
            return new TestFieldDeclarations().sbyteInstance;
        }
        public static object TestShortInstance()
        {
            return new TestFieldDeclarations().shortInstance;
        }
        public static object TestIntInstance()
        {
            return new TestFieldDeclarations().intInstance;
        }
        public static object TestLongInstance()
        {
            return new TestFieldDeclarations().longInstance;
        }
        public static object TestByteInstance()
        {
            return new TestFieldDeclarations().byteInstance;
        }
        public static object TestUshortInstance()
        {
            return new TestFieldDeclarations().ushortInstance;
        }
        public static object TestUintInstance()
        {
            return new TestFieldDeclarations().uintInstance;
        }
        public static object TestUlongInstance()
        {
            return new TestFieldDeclarations().ulongInstance;
        }
        public static object TestFloatInstance()
        {
            return new TestFieldDeclarations().floatInstance;
        }
        public static object TestDoubleInstance()
        {
            return new TestFieldDeclarations().doubleInstance;
        }
        public static object TestStringInstance()
        {
            return new TestFieldDeclarations().stringInstance;
        }



        public static object TestSByteStatic()
        {
            return sbyteStatic; ;
        }
        public static object TestShortStatic()
        {
            return shortStatic;
        }
        public static object TestIntStatic()
        {
            return intStatic;
        }
        public static object TestLongStatic()
        {
            return longStatic;
        }
        public static object TestByteStatic()
        {
            return byteStatic;
        }
        public static object TestUshortStatic()
        {
            return ushortStatic;
        }
        public static object TestUintStatic()
        {
            return uintStatic;
        }
        public static object TestUlongStatic()
        {
            return ulongStatic;
        }
        public static object TestFloatStatic()
        {
            return floatStatic;
        }
        public static object TestDoubleStatic()
        {
            return doubleStatic;
        }
        public static object TestStringStatic()
        {
            return stringStatic;
        }
    }
}
