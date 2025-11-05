
namespace TestAssembly
{
    public class TestField
    {
        // Instance Fields (All Built-in Types)
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

        // Static Fields (Shared among all instances)
        public static int staticInt = 100;
        public static string staticString = "Static Field";
        public static bool staticBool = false;

        // Constant Fields (Must be initialized at declaration, cannot be changed)
        public const float Pi = 3.1415927f;
        public const int MaxValue = int.MaxValue;
        public const string Greeting = "Welcome to C# Fields";

        // Readonly Fields (Can only be assigned in constructor or at declaration)
        public readonly int readOnlyInt = 99;
        public readonly double readOnlyDouble = 199.99;
        public readonly string readOnlyString = "Recently Updated";

        // Volatile Fields (For Multi-threading)
#pragma warning disable 414
        private volatile bool isRunning = false;
#pragma warning restore 414

        // Methods
        public static TestField TestFieldsAssign()
        {
            // Create an instance of FieldTest
            TestField instance = new TestField();

            // Modify instance fields
            instance.instanceInt = 42;
            instance.instanceFloat = 20.5f;
            instance.instanceDouble = 50.55;
            instance.instanceDecimal = 999.99m;
            instance.instanceChar = 'Z';
            instance.instanceString = "Updated Instance String";
            instance.instanceBool = false;
            instance.instanceByte = 100;
            instance.instanceSByte = -64;
            instance.instanceShort = -12345;
            instance.instanceUShort = 54321;
            instance.instanceUInt = 123456789;
            instance.instanceLong = -9999999999;
            instance.instanceULong = 9999999999;

            instance.isRunning = true;

            // Modify static fields
            staticInt = 999;
            staticString = "Updated Static String";
            staticBool = true;

            // Return the modified instance
            return instance;
        }

        public static TestField TestFieldReadWrite()
        {
            TestField instance = new TestField();

            return instance;
        }
    }
}
