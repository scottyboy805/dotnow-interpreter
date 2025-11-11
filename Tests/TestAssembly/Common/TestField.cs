namespace TestAssembly
{
    // Base class for inheritance testing
    public class TestFieldBase
    {
        public int basePublicField = 100;
        protected int baseProtectedField = 200;
        private int basePrivateField = 300;

        // Virtual property for inheritance testing
        private int _virtualPropertyValue = 400;
        public virtual int VirtualProperty
        {
            get { return _virtualPropertyValue; }
            set { _virtualPropertyValue = value; }
        }

        public int virtualFieldValue = 400; // Regular field

        public static int baseStaticField = 500;
    }

    // Derived class for inheritance testing
    public class TestFieldDerived : TestFieldBase
    {
        public int derivedPublicField = 1000;
        protected int derivedProtectedField = 2000;
        private int derivedPrivateField = 3000;

        public override int VirtualProperty
        {
            get { return 4000; }
        }

        public new int virtualFieldValue = 4000; // Hiding base field
        public new int basePublicField = 9999; // Hiding base field

        public static int derivedStaticField = 5000;
    }

    public class TestField
    {
        // Instance Fields (All Built-in Types) - READ TEST FIELDS (never modified)
        public int instanceInt = 10;
        public float instanceFloat = 10.5f;
        public double instanceDouble = 20.99;
        public decimal instanceDecimal = 100.99m;
        public char instanceChar = 'A';
        public string instanceString = "Hello, Fields!";
        public bool instanceBool = true;
        public byte instanceByte = 255;
        public sbyte instanceSByte = -128;
        public short instanceShort = -32768;
        public ushort instanceUShort = 65535;
        public uint instanceUInt = 4294967295;
        public long instanceLong = -9223372036854775808;
        public ulong instanceULong = 18446744073709551615;

        // WRITE TEST FIELDS (dedicated for write tests to avoid cross-contamination)
        public int writeTestInt = 10;
        public float writeTestFloat = 10.5f;
        public double writeTestDouble = 20.99;
        public decimal writeTestDecimal = 100.99m;
        public char writeTestChar = 'A';
        public string writeTestString = "Hello, Fields!";
        public bool writeTestBool = true;
        public byte writeTestByte = 255;
        public sbyte writeTestSByte = -128;
        public short writeTestShort = -32768;
        public ushort writeTestUShort = 65535;
        public uint writeTestUInt = 4294967295;
        public long writeTestLong = -9223372036854775808;
        public ulong writeTestULong = 18446744073709551615;

        // Dedicated fields for boundary value tests to avoid cross-test contamination
        public int boundaryTestInt = 0;
        public byte boundaryTestByte = 0;
        public long boundaryTestLong = 0;

        // Static Fields (Shared among all instances)
        public static int staticInt = 100;
        public static string staticString = "Static Field";
        public static bool staticBool = false;

        // Dedicated static fields for write tests to avoid cross-contamination
        public static int staticWriteTestInt = 100;
        public static string staticWriteTestString = "Static Field";
        public static bool staticWriteTestBool = false;

        // Constant Fields (Must be initialized at declaration, cannot be changed)
        public const float Pi = 3.1415927f;
        public const int MaxValue = int.MaxValue;
        public const string Greeting = "Welcome to C# Fields";

        // Readonly Fields (Can only be assigned in constructor or at declaration)
        public readonly int readOnlyInt = 99;
        public readonly double readOnlyDouble = 199.99;
        public readonly string readOnlyString = "Recently Updated";

        // Access modifier fields
        public int publicField = 1;
        internal int internalField = 2;
        protected int protectedField = 3;
        private int privateField = 4;

        // Volatile Fields (For Multi-threading)
#pragma warning disable 414
        private volatile bool isRunning = false;
#pragma warning restore 414

        // Reference type fields (read-only)
        public object objectField = new object();
        public TestField selfReference;

        // Dedicated object test fields for write tests
        public object writeTestObjectField = new object();

        // Null fields
        public string nullStringField = null;
        public object nullObjectField = null;

        // Constructor for readonly field testing
        public TestField()
        {
            selfReference = this;
        }

        public TestField(int readOnlyValue)
        {
            readOnlyInt = readOnlyValue;
            selfReference = this;
        }

        // Methods
        public static TestField TestFieldsAssign()
        {
            // Create an instance of FieldTest
            TestField instance = new TestField();

            // Modify ONLY the dedicated assignment test fields to avoid cross-test contamination
            instance.writeTestInt = 42;
            instance.writeTestFloat = 20.5f;
            instance.writeTestDouble = 50.55;
            instance.writeTestDecimal = 999.99m;
            instance.writeTestChar = 'Z';
            instance.writeTestString = "Updated Instance String";
            instance.writeTestBool = false;
            instance.writeTestByte = 100;
            instance.writeTestSByte = -64;
            instance.writeTestShort = -12345;
            instance.writeTestUShort = 54321;
            instance.writeTestUInt = 123456789;
            instance.writeTestLong = -9999999999;
            instance.writeTestULong = 9999999999;

            instance.isRunning = true;

            // Modify ONLY the dedicated static assignment test fields
            staticWriteTestInt = 999;
            staticWriteTestString = "Updated Static String";
            staticWriteTestBool = true;

            // Return the modified instance
            return instance;
        }

        public static TestField TestFieldReadWrite()
        {
            TestField instance = new TestField();
            return instance;
        }

        // Individual field read tests - dedicated fields for isolation
        public static int TestReadInstanceInt()
        {
            TestField instance = new TestField();
            return instance.instanceInt; // Uses original field - never modified
        }

        public static float TestReadInstanceFloat()
        {
            TestField instance = new TestField();
            return instance.instanceFloat; // Uses original field - never modified
        }

        public static bool TestReadInstanceBool()
        {
            TestField instance = new TestField();
            return instance.instanceBool; // Uses original field - never modified
        }

        public static string TestReadInstanceString()
        {
            TestField instance = new TestField();
            return instance.instanceString; // Uses original field - never modified
        }

        // Additional individual field read tests for all types (using dedicated read fields)
        public static double TestReadInstanceDouble()
        {
            TestField instance = new TestField();
            return instance.instanceDouble;
        }

        public static decimal TestReadInstanceDecimal()
        {
            TestField instance = new TestField();
            return instance.instanceDecimal;
        }

        public static char TestReadInstanceChar()
        {
            TestField instance = new TestField();
            return instance.instanceChar;
        }

        public static byte TestReadInstanceByte()
        {
            TestField instance = new TestField();
            return instance.instanceByte;
        }

        public static sbyte TestReadInstanceSByte()
        {
            TestField instance = new TestField();
            return instance.instanceSByte;
        }

        public static short TestReadInstanceShort()
        {
            TestField instance = new TestField();
            return instance.instanceShort;
        }

        public static ushort TestReadInstanceUShort()
        {
            TestField instance = new TestField();
            return instance.instanceUShort;
        }

        public static uint TestReadInstanceUInt()
        {
            TestField instance = new TestField();
            return instance.instanceUInt;
        }

        public static long TestReadInstanceLong()
        {
            TestField instance = new TestField();
            return instance.instanceLong;
        }

        public static ulong TestReadInstanceULong()
        {
            TestField instance = new TestField();
            return instance.instanceULong;
        }

        // Additional individual field write tests for all types (using dedicated write fields)
        public static double TestWriteInstanceDouble()
        {
            TestField instance = new TestField();
            instance.writeTestDouble = 3.14159; // Use dedicated write field
            return instance.writeTestDouble;
        }

        public static decimal TestWriteInstanceDecimal()
        {
            TestField instance = new TestField();
            instance.writeTestDecimal = 12345.6789m; // Use dedicated write field
            return instance.writeTestDecimal;
        }

        public static char TestWriteInstanceChar()
        {
            TestField instance = new TestField();
            instance.writeTestChar = 'X'; // Use dedicated write field
            return instance.writeTestChar;
        }

        public static byte TestWriteInstanceByte()
        {
            TestField instance = new TestField();
            instance.writeTestByte = 128; // Use dedicated write field
            return instance.writeTestByte;
        }

        public static sbyte TestWriteInstanceSByte()
        {
            TestField instance = new TestField();
            instance.writeTestSByte = -100; // Use dedicated write field
            return instance.writeTestSByte;
        }

        public static short TestWriteInstanceShort()
        {
            TestField instance = new TestField();
            instance.writeTestShort = 1000; // Use dedicated write field
            return instance.writeTestShort;
        }

        public static ushort TestWriteInstanceUShort()
        {
            TestField instance = new TestField();
            instance.writeTestUShort = 2000; // Use dedicated write field
            return instance.writeTestUShort;
        }

        public static uint TestWriteInstanceUInt()
        {
            TestField instance = new TestField();
            instance.writeTestUInt = 3000000; // Use dedicated write field
            return instance.writeTestUInt;
        }

        public static long TestWriteInstanceLong()
        {
            TestField instance = new TestField();
            instance.writeTestLong = 4000000000L; // Use dedicated write field
            return instance.writeTestLong;
        }

        public static ulong TestWriteInstanceULong()
        {
            TestField instance = new TestField();
            instance.writeTestULong = 5000000000UL; // Use dedicated write field
            return instance.writeTestULong;
        }

        // Static field tests
        public static int TestReadStaticInt()
        {
            return staticInt; // Use read-only static field
        }

        public static string TestReadStaticString()
        {
            return staticString; // Use read-only static field
        }

        public static bool TestReadStaticBool()
        {
            return staticBool; // Use read-only static field
        }

        // Static field write tests (using dedicated write static fields)
        public static int TestWriteStaticInt()
        {
            staticWriteTestInt = 12345; // Use dedicated write static field
            return staticWriteTestInt;
        }

        public static string TestWriteStaticString()
        {
            staticWriteTestString = "Modified Static String"; // Use dedicated write static field
            return staticWriteTestString;
        }

        public static bool TestWriteStaticBool()
        {
            staticWriteTestBool = !staticWriteTestBool; // Use dedicated write static field
            return staticWriteTestBool;
        }

        // Constant field tests
        public static float TestReadConstantPi()
        {
            return Pi;
        }

        public static int TestReadConstantMaxValue()
        {
            return MaxValue;
        }

        public static string TestReadConstantGreeting()
        {
            return Greeting;
        }

        // Object field tests (using dedicated fields)
        public static object TestReadObjectField()
        {
            TestField instance = new TestField();
            return instance.objectField; // Use read-only object field
        }

        public static object TestWriteObjectField()
        {
            TestField instance = new TestField();
            instance.writeTestObjectField = "New Object Value"; // Use dedicated write field
            return instance.writeTestObjectField;
        }

        // Boundary value tests (using dedicated boundary test fields)
        public static int TestMinMaxInt()
        {
            TestField instance = new TestField();
            instance.boundaryTestInt = int.MinValue; // Use dedicated boundary field
            int min = instance.boundaryTestInt;
            instance.boundaryTestInt = int.MaxValue;
            int max = instance.boundaryTestInt;
            return max - min != -1 ? max : min; // Return max if calculation doesn't overflow
        }

        public static byte TestMinMaxByte()
        {
            TestField instance = new TestField();
            instance.boundaryTestByte = byte.MinValue; // Use dedicated boundary field
            byte min = instance.boundaryTestByte;
            instance.boundaryTestByte = byte.MaxValue;
            byte max = instance.boundaryTestByte;
            return max; // Return max value
        }

        public static long TestMinMaxLong()
        {
            TestField instance = new TestField();
            instance.boundaryTestLong = long.MinValue; // Use dedicated boundary field
            long min = instance.boundaryTestLong;
            instance.boundaryTestLong = long.MaxValue;
            long max = instance.boundaryTestLong;
            return max; // Return max value
        }

        // Derived class field tests
        public static TestFieldDerived TestDerivedFields()
        {
            TestFieldDerived instance = new TestFieldDerived();

            // Access base fields through derived instance
            instance.basePublicField = 777; // This will access the hidden field
            instance.derivedPublicField = 888;

            // Access static fields
            TestFieldBase.baseStaticField = 111;
            TestFieldDerived.derivedStaticField = 222;

            return instance;
        }

        // Null field tests
        public static object[] TestNullFields()
        {
            TestField instance = new TestField();
            return new object[] { instance.nullStringField, instance.nullObjectField };
        }

        // Reference field tests
        public static bool TestSelfReference()
        {
            TestField instance = new TestField();
            return ReferenceEquals(instance, instance.selfReference);
        }

        // Private field access (for reflection testing)
        public int GetPrivateField()
        {
            return privateField;
        }

        public void SetPrivateField(int value)
        {
            privateField = value;
        }

        // Individual field write tests - use dedicated write fields to avoid cross-test contamination
        public static int TestWriteInstanceInt()
        {
            TestField instance = new TestField();
            instance.writeTestInt = 999; // Use dedicated write field
            return instance.writeTestInt;
        }

        public static float TestWriteInstanceFloat()
        {
            TestField instance = new TestField();
            instance.writeTestFloat = 888.888f; // Use dedicated write field
            return instance.writeTestFloat;
        }

        public static bool TestWriteInstanceBool()
        {
            TestField instance = new TestField();
            instance.writeTestBool = !instance.writeTestBool; // Use dedicated write field
            return instance.writeTestBool;
        }

        public static string TestWriteInstanceString()
        {
            TestField instance = new TestField();
            instance.writeTestString = "New Value"; // Use dedicated write field
            return instance.writeTestString;
        }
    }
}
