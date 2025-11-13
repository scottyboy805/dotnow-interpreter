using System.Collections.Generic;

namespace TestAssembly
{
    public class TestClass
    {
        public bool fieldBool = true;
        public char fieldChar = 'a';

        public sbyte fieldI8 = -2;
        public byte fieldU8 = 4;
        public short fieldI16 = -6;
        public ushort fieldU16 = 8;
        public int fieldI32 = -10;
        public uint fieldU32 = 12;
        public long fieldI64 = -14;
        public ulong fieldU64 = 16;
        public float fieldFloat = -18.25f;
        public double fieldDouble = 20.55d;

        public string fieldString = "hello world";
        //public object fieldObject = null;


        public static TestClass TestCreateClass()
        {
            return new TestClass();
        }
    }
}
