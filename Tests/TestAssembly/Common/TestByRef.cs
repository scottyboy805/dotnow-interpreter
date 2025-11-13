namespace TestAssembly
{
    public class TestByRef
    {
        public bool boolVal = true;
        public char charVal = 'a';
        public sbyte sbyteVal = -2;
        public byte byteVal = 4;
        public short shortVal = -6;
        public ushort ushortVal = 8;
        public int intVal = -10;
        public uint uintVal = 12;
        public long longVal = -14;
        public ulong ulongVal = 16;
        public float floatVal = -18.25f;
        public double doubleVal = 20.55d;
        public decimal decimalVal = 76.234m;
        public string stringVal = "hello world";

        // OUT parameter test methods for each type
        private static void TestOutBool(out bool val) => val = true;
        private static void TestOutChar(out char val) => val = 'b';
        private static void TestOutByte(out byte val) => val = 1;
        private static void TestOutSByte(out sbyte val) => val = -1;
        private static void TestOutShort(out short val) => val = -1;
        private static void TestOutUShort(out ushort val) => val = 1;
        private static void TestOutInt(out int val) => val = -1;
        private static void TestOutUInt(out uint val) => val = 1;
        private static void TestOutLong(out long val) => val = -1L;
        private static void TestOutULong(out ulong val) => val = 1UL;
        private static void TestOutFloat(out float val) => val = 1.1f;
        private static void TestOutDouble(out double val) => val = 1.1;
        private static void TestOutDecimal(out decimal val) => val = 1.1m;
        private static void TestOutString(out string val) => val = "goodbye";

        // REF parameter test methods for each type
        private static void TestRefBool(ref bool val) => val = !val;
        private static void TestRefChar(ref char val) => val = (char)(val + 1);
        private static void TestRefByte(ref byte val) => val = (byte)(val * 2);
        private static void TestRefSByte(ref sbyte val) => val = (sbyte)(val * -2);
        private static void TestRefShort(ref short val) => val = (short)(val * -2);
        private static void TestRefUShort(ref ushort val) => val = (ushort)(val * 2);
        private static void TestRefInt(ref int val) => val = val * -2;
        private static void TestRefUInt(ref uint val) => val = val * 2;
        private static void TestRefLong(ref long val) => val = val * -2L;
        private static void TestRefULong(ref ulong val) => val = val * 2UL;
        private static void TestRefFloat(ref float val) => val = val * -2.0f;
        private static void TestRefDouble(ref double val) => val = val * -2.0;
        private static void TestRefDecimal(ref decimal val) => val = val * -2.0m;
        private static void TestRefString(ref string val) => val = val + " modified";

        // IN parameter test methods for each type
        private static bool TestInBool(in bool val) => val;
        private static char TestInChar(in char val) => val;
        private static byte TestInByte(in byte val) => val;
        private static sbyte TestInSByte(in sbyte val) => val;
        private static short TestInShort(in short val) => val;
        private static ushort TestInUShort(in ushort val) => val;
        private static int TestInInt(in int val) => val;
        private static uint TestInUInt(in uint val) => val;
        private static long TestInLong(in long val) => val;
        private static ulong TestInULong(in ulong val) => val;
        private static float TestInFloat(in float val) => val;
        private static double TestInDouble(in double val) => val;
        private static decimal TestInDecimal(in decimal val) => val;
        private static string TestInString(in string val) => val;

        // ===== OUT PARAMETER TESTS =====

        public static object[] TestOutBool()
        {
            TestOutBool(out bool boolVal);
            return new object[] { boolVal };
        }

        public static object[] TestOutChar()
        {
            TestOutChar(out char charVal);
            return new object[] { charVal };
        }

        public static object[] TestOutByte()
        {
            TestOutByte(out byte byteVal);
            return new object[] { byteVal };
        }

        public static object[] TestOutSByte()
        {
            TestOutSByte(out sbyte sbyteVal);
            return new object[] { sbyteVal };
        }

        public static object[] TestOutShort()
        {
            TestOutShort(out short shortVal);
            return new object[] { shortVal };
        }

        public static object[] TestOutUShort()
        {
            TestOutUShort(out ushort ushortVal);
            return new object[] { ushortVal };
        }

        public static object[] TestOutInt()
        {
            TestOutInt(out int intVal);
            return new object[] { intVal };
        }

        public static object[] TestOutUInt()
        {
            TestOutUInt(out uint uintVal);
            return new object[] { uintVal };
        }

        public static object[] TestOutLong()
        {
            TestOutLong(out long longVal);
            return new object[] { longVal };
        }

        public static object[] TestOutULong()
        {
            TestOutULong(out ulong ulongVal);
            return new object[] { ulongVal };
        }

        public static object[] TestOutFloat()
        {
            TestOutFloat(out float floatVal);
            return new object[] { floatVal };
        }

        public static object[] TestOutDouble()
        {
            TestOutDouble(out double doubleVal);
            return new object[] { doubleVal };
        }

        public static object[] TestOutDecimal()
        {
            TestOutDecimal(out decimal decimalVal);
            return new object[] { decimalVal };
        }

        public static object[] TestOutString()
        {
            TestOutString(out string stringVal);
            return new object[] { stringVal };
        }

        // ===== REF PARAMETER TESTS =====

        public static object[] TestRefBool()
        {
            bool val = false;
            TestRefBool(ref val);
            return new object[] { val };
        }

        public static object[] TestRefChar()
        {
            char val = 'a';
            TestRefChar(ref val);
            return new object[] { val };
        }

        public static object[] TestRefByte()
        {
            byte val = 5;
            TestRefByte(ref val);
            return new object[] { val };
        }

        public static object[] TestRefSByte()
        {
            sbyte val = -5;
            TestRefSByte(ref val);
            return new object[] { val };
        }

        public static object[] TestRefShort()
        {
            short val = -10;
            TestRefShort(ref val);
            return new object[] { val };
        }

        public static object[] TestRefUShort()
        {
            ushort val = 10;
            TestRefUShort(ref val);
            return new object[] { val };
        }

        public static object[] TestRefInt()
        {
            int val = 42;
            TestRefInt(ref val);
            return new object[] { val };
        }

        public static object[] TestRefUInt()
        {
            uint val = 42;
            TestRefUInt(ref val);
            return new object[] { val };
        }

        public static object[] TestRefLong()
        {
            long val = 100L;
            TestRefLong(ref val);
            return new object[] { val };
        }

        public static object[] TestRefULong()
        {
            ulong val = 100UL;
            TestRefULong(ref val);
            return new object[] { val };
        }

        public static object[] TestRefFloat()
        {
            float val = 3.14f;
            TestRefFloat(ref val);
            return new object[] { val };
        }

        public static object[] TestRefDouble()
        {
            double val = 2.718;
            TestRefDouble(ref val);
            return new object[] { val };
        }

        public static object[] TestRefDecimal()
        {
            decimal val = 123.456m;
            TestRefDecimal(ref val);
            return new object[] { val };
        }

        public static object[] TestRefString()
        {
            string val = "original";
            TestRefString(ref val);
            return new object[] { val };
        }

        // ===== IN PARAMETER TESTS =====

        public static object[] TestInBool()
        {
            bool val = true;
            bool result = TestInBool(in val);
            return new object[] { result };
        }

        public static object[] TestInChar()
        {
            char val = 'x';
            char result = TestInChar(in val);
            return new object[] { result };
        }

        public static object[] TestInByte()
        {
            byte val = 200;
            byte result = TestInByte(in val);
            return new object[] { result };
        }

        public static object[] TestInSByte()
        {
            sbyte val = -100;
            sbyte result = TestInSByte(in val);
            return new object[] { result };
        }

        public static object[] TestInShort()
        {
            short val = -1000;
            short result = TestInShort(in val);
            return new object[] { result };
        }

        public static object[] TestInUShort()
        {
            ushort val = 1000;
            ushort result = TestInUShort(in val);
            return new object[] { result };
        }

        public static object[] TestInInt()
        {
            int val = -50000;
            int result = TestInInt(in val);
            return new object[] { result };
        }

        public static object[] TestInUInt()
        {
            uint val = 50000;
            uint result = TestInUInt(in val);
            return new object[] { result };
        }

        public static object[] TestInLong()
        {
            long val = -1000000L;
            long result = TestInLong(in val);
            return new object[] { result };
        }

        public static object[] TestInULong()
        {
            ulong val = 1000000UL;
            ulong result = TestInULong(in val);
            return new object[] { result };
        }

        public static object[] TestInFloat()
        {
            float val = -99.99f;
            float result = TestInFloat(in val);
            return new object[] { result };
        }

        public static object[] TestInDouble()
        {
            double val = 123.456789;
            double result = TestInDouble(in val);
            return new object[] { result };
        }

        public static object[] TestInDecimal()
        {
            decimal val = -987.654321m;
            decimal result = TestInDecimal(in val);
            return new object[] { result };
        }

        public static object[] TestInString()
        {
            string val = "test string";
            string result = TestInString(in val);
            return new object[] { result };
        }

        // ===== FIELD BYREF TESTS =====

        public static object[] TestOutFieldBool()
        {
            TestByRef inst = new TestByRef();
            TestOutBool(out inst.boolVal);
            return new object[] { inst.boolVal };
        }

        public static object[] TestOutFieldChar()
        {
            TestByRef inst = new TestByRef();
            TestOutChar(out inst.charVal);
            return new object[] { inst.charVal };
        }

        public static object[] TestOutFieldByte()
        {
            TestByRef inst = new TestByRef();
            TestOutByte(out inst.byteVal);
            return new object[] { inst.byteVal };
        }

        public static object[] TestOutFieldSByte()
        {
            TestByRef inst = new TestByRef();
            TestOutSByte(out inst.sbyteVal);
            return new object[] { inst.sbyteVal };
        }

        public static object[] TestOutFieldShort()
        {
            TestByRef inst = new TestByRef();
            TestOutShort(out inst.shortVal);
            return new object[] { inst.shortVal };
        }

        public static object[] TestOutFieldUShort()
        {
            TestByRef inst = new TestByRef();
            TestOutUShort(out inst.ushortVal);
            return new object[] { inst.ushortVal };
        }

        public static object[] TestOutFieldInt()
        {
            TestByRef inst = new TestByRef();
            TestOutInt(out inst.intVal);
            return new object[] { inst.intVal };
        }

        public static object[] TestOutFieldUInt()
        {
            TestByRef inst = new TestByRef();
            TestOutUInt(out inst.uintVal);
            return new object[] { inst.uintVal };
        }

        public static object[] TestOutFieldLong()
        {
            TestByRef inst = new TestByRef();
            TestOutLong(out inst.longVal);
            return new object[] { inst.longVal };
        }

        public static object[] TestOutFieldULong()
        {
            TestByRef inst = new TestByRef();
            TestOutULong(out inst.ulongVal);
            return new object[] { inst.ulongVal };
        }

        public static object[] TestOutFieldFloat()
        {
            TestByRef inst = new TestByRef();
            TestOutFloat(out inst.floatVal);
            return new object[] { inst.floatVal };
        }

        public static object[] TestOutFieldDouble()
        {
            TestByRef inst = new TestByRef();
            TestOutDouble(out inst.doubleVal);
            return new object[] { inst.doubleVal };
        }

        public static object[] TestOutFieldDecimal()
        {
            TestByRef inst = new TestByRef();
            TestOutDecimal(out inst.decimalVal);
            return new object[] { inst.decimalVal };
        }

        public static object[] TestOutFieldString()
        {
            TestByRef inst = new TestByRef();
            TestOutString(out inst.stringVal);
            return new object[] { inst.stringVal };
        }

        public static object[] TestRefFieldBool()
        {
            TestByRef inst = new TestByRef();
            inst.boolVal = false;
            TestRefBool(ref inst.boolVal);
            return new object[] { inst.boolVal };
        }

        public static object[] TestRefFieldChar()
        {
            TestByRef inst = new TestByRef();
            inst.charVal = 'z';
            TestRefChar(ref inst.charVal);
            return new object[] { inst.charVal };
        }

        public static object[] TestRefFieldByte()
        {
            TestByRef inst = new TestByRef();
            inst.byteVal = 50;
            TestRefByte(ref inst.byteVal);
            return new object[] { inst.byteVal };
        }

        public static object[] TestRefFieldSByte()
        {
            TestByRef inst = new TestByRef();
            inst.sbyteVal = -50;
            TestRefSByte(ref inst.sbyteVal);
            return new object[] { inst.sbyteVal };
        }

        public static object[] TestRefFieldShort()
        {
            TestByRef inst = new TestByRef();
            inst.shortVal = -500;
            TestRefShort(ref inst.shortVal);
            return new object[] { inst.shortVal };
        }

        public static object[] TestRefFieldUShort()
        {
            TestByRef inst = new TestByRef();
            inst.ushortVal = 500;
            TestRefUShort(ref inst.ushortVal);
            return new object[] { inst.ushortVal };
        }

        public static object[] TestRefFieldInt()
        {
            TestByRef inst = new TestByRef();
            inst.intVal = 12345;
            TestRefInt(ref inst.intVal);
            return new object[] { inst.intVal };
        }

        public static object[] TestRefFieldUInt()
        {
            TestByRef inst = new TestByRef();
            inst.uintVal = 12345;
            TestRefUInt(ref inst.uintVal);
            return new object[] { inst.uintVal };
        }

        public static object[] TestRefFieldLong()
        {
            TestByRef inst = new TestByRef();
            inst.longVal = 123456789L;
            TestRefLong(ref inst.longVal);
            return new object[] { inst.longVal };
        }

        public static object[] TestRefFieldULong()
        {
            TestByRef inst = new TestByRef();
            inst.ulongVal = 123456789UL;
            TestRefULong(ref inst.ulongVal);
            return new object[] { inst.ulongVal };
        }

        public static object[] TestRefFieldFloat()
        {
            TestByRef inst = new TestByRef();
            inst.floatVal = 12.34f;
            TestRefFloat(ref inst.floatVal);
            return new object[] { inst.floatVal };
        }

        public static object[] TestRefFieldDouble()
        {
            TestByRef inst = new TestByRef();
            inst.doubleVal = 56.789;
            TestRefDouble(ref inst.doubleVal);
            return new object[] { inst.doubleVal };
        }

        public static object[] TestRefFieldDecimal()
        {
            TestByRef inst = new TestByRef();
            inst.decimalVal = 999.888m;
            TestRefDecimal(ref inst.decimalVal);
            return new object[] { inst.decimalVal };
        }

        public static object[] TestRefFieldString()
        {
            TestByRef inst = new TestByRef();
            inst.stringVal = "field test";
            TestRefString(ref inst.stringVal);
            return new object[] { inst.stringVal };
        }

        // ===== ARRAY ELEMENT BYREF TESTS =====

        public static object[] TestOutArrayElementInt()
        {
            int[] arr = new int[3];
            TestOutInt(out arr[1]);
            return new object[] { arr[1] };
        }

        public static object[] TestRefArrayElementInt()
        {
            int[] arr = { 10, 20, 30 };
            TestRefInt(ref arr[1]);
            return new object[] { arr[1] };
        }

        public static object[] TestOutArrayElementFloat()
        {
            float[] arr = new float[3];
            TestOutFloat(out arr[2]);
            return new object[] { arr[2] };
        }

        public static object[] TestRefArrayElementFloat()
        {
            float[] arr = { 1.1f, 2.2f, 3.3f };
            TestRefFloat(ref arr[0]);
            return new object[] { arr[0] };
        }

        public static object[] TestOutArrayElementString()
        {
            string[] arr = new string[2];
            TestOutString(out arr[1]);
            return new object[] { arr[1] };
        }

        public static object[] TestRefArrayElementString()
        {
            string[] arr = { "first", "second" };
            TestRefString(ref arr[0]);
            return new object[] { arr[0] };
        }
    }
}
