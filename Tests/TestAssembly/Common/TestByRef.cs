
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

        public static object[] TestOut()
        {
            TestOutBool(out bool boolVal);
            TestOutChar(out char charVal);
            TestOutByte(out byte byteVal);
            TestOutSByte(out sbyte sbyteVal);
            TestOutShort(out short shortVal);
            TestOutUShort(out ushort ushortVal);
            TestOutInt(out int intVal);
            TestOutUInt(out uint uintVal);
            TestOutLong(out long longVal);
            TestOutULong(out ulong ulongVal);
            TestOutFloat(out float floatVal);
            TestOutDouble(out double doubleVal);
            TestOutDecimal(out decimal decimalVal);
            TestOutString(out string stringVal);

            return new object[]
            {
                boolVal,
                charVal,
                byteVal,
                sbyteVal,
                shortVal,
                ushortVal,
                intVal,
                uintVal,
                longVal,
                ulongVal,
                floatVal,
                doubleVal,
                decimalVal,
                stringVal,
            };
        }

        public static object[] TestOutField()
        {
            TestByRef inst = new TestByRef();

            TestOutBool(out inst.boolVal);
            TestOutChar(out inst.charVal);
            TestOutByte(out inst.byteVal);
            TestOutSByte(out inst.sbyteVal);
            TestOutShort(out inst.shortVal);
            TestOutUShort(out inst.ushortVal);
            TestOutInt(out inst.intVal);
            TestOutUInt(out inst.uintVal);
            TestOutLong(out inst.longVal);
            TestOutULong(out inst.ulongVal);
            TestOutFloat(out inst.floatVal);
            TestOutDouble(out inst.doubleVal);
            TestOutDecimal(out inst.decimalVal);
            TestOutString(out inst.stringVal);

            return new object[]
            {
                inst.boolVal,
                inst.charVal,
                inst.byteVal,
                inst.sbyteVal,
                inst.shortVal,
                inst.ushortVal,
                inst.intVal,
                inst.uintVal,
                inst.longVal,
                inst.ulongVal,
                inst.floatVal,
                inst.doubleVal,
                inst.decimalVal,
                inst.stringVal,
            };
        }
    }
}
