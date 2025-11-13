namespace TestAssembly
{
    public static class TestBoxing
    {
        public static object TestBoxingInt()
        {
            int value = 42;
            object boxed = value;  // Boxing operation
            return boxed;
        }

        public static object TestBoxingUInt()
        {
            uint value = 42U;
            object boxed = value;  // Boxing operation
            return boxed;
        }

        public static object TestBoxingLong()
        {
            long value = 9223372036854775807L;
            object boxed = value;  // Boxing operation
            return boxed;
        }

        public static object TestBoxingULong()
        {
            ulong value = 18446744073709551615UL;
            object boxed = value;  // Boxing operation
            return boxed;
        }

        public static object TestBoxingShort()
        {
            short value = 32767;
            object boxed = value;  // Boxing operation
            return boxed;
        }

        public static object TestBoxingUShort()
        {
            ushort value = 65535;
            object boxed = value;  // Boxing operation
            return boxed;
        }

        public static object TestBoxingByte()
        {
            byte value = 255;
            object boxed = value;  // Boxing operation
            return boxed;
        }

        public static object TestBoxingSByte()
        {
            sbyte value = -128;
            object boxed = value;  // Boxing operation
            return boxed;
        }

        public static object TestBoxingChar()
        {
            char value = 'Z';
            object boxed = value;  // Boxing operation
            return boxed;
        }

        public static object TestBoxingFloat()
        {
            float value = 3.14159f;
            object boxed = value;  // Boxing operation
            return boxed;
        }

        public static object TestBoxingDouble()
        {
            double value = 3.141592653589793;
            object boxed = value;  // Boxing operation
            return boxed;
        }

        public static object TestBoxingDecimal()
        {
            decimal value = 79228162514264337593543950335m;
            object boxed = value;  // Boxing operation
            return boxed;
        }

        public static object TestBoxingBool()
        {
            bool value = true;
            object boxed = value;  // Boxing operation
            return boxed;
        }

        // Boxing with edge values
        public static object TestBoxingIntMin()
        {
            int value = int.MinValue;
            object boxed = value;
            return boxed;
        }

        public static object TestBoxingIntMax()
        {
            int value = int.MaxValue;
            object boxed = value;
            return boxed;
        }

        public static object TestBoxingIntZero()
        {
            int value = 0;
            object boxed = value;
            return boxed;
        }

        public static object TestBoxingFloatNaN()
        {
            float value = float.NaN;
            object boxed = value;
            return boxed;
        }

        public static object TestBoxingFloatInfinity()
        {
            float value = float.PositiveInfinity;
            object boxed = value;
            return boxed;
        }

        public static object TestBoxingDoubleNegativeInfinity()
        {
            double value = double.NegativeInfinity;
            object boxed = value;
            return boxed;
        }

        // Boxing in arrays
        public static object[] TestBoxingInArrays()
        {
            // Test boxing when storing value types in object arrays
            object[] array = new object[13];

            array[0] = 42;   // int
            array[1] = 42U;          // uint
            array[2] = 123456789L;    // long
            array[3] = 123456789UL;             // ulong
            array[4] = (short)12345;// short
            array[5] = (ushort)12345;           // ushort
            array[6] = (byte)200;   // byte
            array[7] = (sbyte)-100;// sbyte
            array[8] = 'A';            // char
            array[9] = 1.5f;             // float
            array[10] = 2.5;          // double
            array[11] = 3.75m;   // decimal
            array[12] = true; // bool

            return array;
        }

        // Boxing with implicit conversions
        public static object[] TestBoxingWithConversions()
        {
            object[] results = new object[8];

            // These should box as their original types, not promote
            short shortVal = 100;
            byte byteVal = 200;
            sbyte sbyteVal = -50;
            ushort ushortVal = 300;

            results[0] = shortVal;      // Should box as short, not int
            results[1] = byteVal;       // Should box as byte, not int
            results[2] = sbyteVal;  // Should box as sbyte, not int
            results[3] = ushortVal;     // Should box as ushort, not int

            // Arithmetic results should box as their computed types
            results[4] = shortVal + shortVal;   // Should be int (arithmetic promotion)
            results[5] = byteVal + byteVal;   // Should be int (arithmetic promotion)
            results[6] = sbyteVal + sbyteVal;   // Should be int (arithmetic promotion)
            results[7] = ushortVal + ushortVal; // Should be int (arithmetic promotion)

            return results;
        }

        // Boxing with nullable types
        public static object[] TestBoxingNullables()
        {
            int? nullableInt = 42;
            int? nullInt = null;
            bool? nullableBool = true;
            bool? nullBool = null;

            object[] results = new object[4];
            results[0] = nullableInt;   // Should box the int value, not the nullable
            results[1] = nullInt;   // Should be null
            results[2] = nullableBool;  // Should box the bool value
            results[3] = nullBool;      // Should be null

            return results;
        }

        // Test boxing in method parameters
        public static object TestBoxingInMethodCall()
        {
            return BoxingHelper(42, 3.14f, true, 'X');
        }

        private static object[] BoxingHelper(object a, object b, object c, object d)
        {
            return new object[] { a, b, c, d };
        }
    }
}