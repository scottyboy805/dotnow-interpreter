using System;

namespace TestAssembly
{
    public class TestFieldInfo
    {
        // Enum for testing
        public enum TestEnum
        {
            Value1,
            Value2 = 10,
            Value3
        }

        // Struct for testing
        public struct TestStruct
        {
            public int structField;
            public static readonly TestStruct Default = new TestStruct { structField = 42 };
        }

        // Generic class for testing
        public class GenericFieldTest<T>
        {
            public T genericField;
            public static T staticGenericField;
        }

        // Base class for inheritance testing
        public class BaseFieldTest
        {
            public int basePublicField = 1;
            protected int baseProtectedField = 2;
            private int basePrivateField = 3;
            public static int baseStaticField = 4;
        }

        // Derived class for inheritance testing
        public class DerivedFieldTest : BaseFieldTest
        {
            public int derivedPublicField = 10;
            protected int derivedProtectedField = 20;
            private int derivedPrivateField = 30;
            public static int derivedStaticField = 40;

            // Hiding base field
            public new int basePublicField = 100;
        }


        // Instance fields with different types
        public int publicInstanceInt = 42;
        public string publicInstanceString = "test string";
        public bool publicInstanceBool = true;
        public float publicInstanceFloat = 3.14f;
        public double publicInstanceDouble = 2.718;
        public char publicInstanceChar = 'A';
        public byte publicInstanceByte = 255;
        public sbyte publicInstanceSByte = -128;
        public short publicInstanceShort = 1000;
        public ushort publicInstanceUShort = 2000;
        public uint publicInstanceUInt = 3000;
        public long publicInstanceLong = 4000;
        public ulong publicInstanceULong = 5000;
        public decimal publicInstanceDecimal = 99.99m;

        // Fields with different access modifiers
        public int publicField = 100;
        private int privateField = 200;
        protected int protectedField = 300;
        internal int internalField = 400;

        // Static fields
        public static int publicStaticInt = 1000;
        public static string publicStaticString = "static string";
        private static bool privateStaticBool = false;
        protected static double protectedStaticDouble = 1.23;
        internal static float internalStaticFloat = 4.56f;

        // Readonly fields
        public readonly int publicReadonlyInt = 500;
        private readonly string privateReadonlyString = "readonly";
        public static readonly DateTime staticReadonlyDateTime = DateTime.MinValue;

        // Constant fields
        public const int PublicConstInt = 999;
        public const string PublicConstString = "constant";
        private const bool PrivateConstBool = true;
        internal const double InternalConstDouble = 3.14159;

        // Volatile field
        private volatile bool volatileField = false;

        // Object fields
        public object publicObjectField = new object();
        public Type publicTypeField = typeof(int);

        // Array fields
        public int[] publicIntArray = new int[] { 1, 2, 3 };
        public string[] publicStringArray = { "a", "b", "c" };

        // Null fields
        public string nullStringField = null;
        public object nullObjectField = null;

        // Self-reference
        public TestFieldInfo selfReference;

        // Generic type field
        public System.Collections.Generic.List<int> genericListField = new System.Collections.Generic.List<int>();

        // Constructor
        public TestFieldInfo()
        {
            selfReference = this;
        }

        public TestFieldInfo(int readonlyValue) : this()
        {
            publicReadonlyInt = readonlyValue;
        }

        // Methods to access private fields for testing
        public int GetPrivateField() => privateField;
        public void SetPrivateField(int value) => privateField = value;

        public bool GetPrivateStaticBool() => privateStaticBool;
        public void SetPrivateStaticBool(bool value) => privateStaticBool = value;

        public string GetPrivateReadonlyString() => privateReadonlyString;

        public bool GetVolatileField() => volatileField;
        public void SetVolatileField(bool value) => volatileField = value;
    }
}
