namespace TestAssembly
{
    public static class TestConversion
    {
        // === IMPLICIT CONVERSION TESTS ===

        // Byte implicit conversions
        public static object[] TestByteImplicitConversions()
        {
            byte b = 100;
            return new object[]
                   {
                (short)b,   // byte -> short
      (int)b,   // byte -> int
(long)b,    // byte -> long
             (float)b,   // byte -> float
         (double)b,  // byte -> double
             };
        }

        // SByte implicit conversions
        public static object[] TestSByteImplicitConversions()
        {
            sbyte sb = 50;
            return new object[]
          {
       (short)sb,  // sbyte -> short
           (int)sb,    // sbyte -> int
  (long)sb,   // sbyte -> long
      (float)sb,  // sbyte -> float
       (double)sb, // sbyte -> double
                };
        }

        // Short implicit conversions
        public static object[] TestShortImplicitConversions()
        {
            short s = 1000;
            return new object[]
                {
        (int)s,   // short -> int
     (long)s,    // short -> long
    (float)s,   // short -> float
          (double)s,  // short -> double
                  };
        }

        // UShort implicit conversions
        public static object[] TestUShortImplicitConversions()
        {
            ushort us = 2000;
            return new object[]
             {
    (int)us,    // ushort -> int
     (uint)us,   // ushort -> uint
           (long)us,   // ushort -> long
        (ulong)us,  // ushort -> ulong
     (float)us,  // ushort -> float
     (double)us, // ushort -> double
              };
        }

        // Int implicit conversions
        public static object[] TestIntImplicitConversions()
        {
            int i = 100000;
            return new object[]
                        {
       (long)i,    // int -> long
  (float)i,   // int -> float
                (double)i,  // int -> double
                  };
        }

        // UInt implicit conversions
        public static object[] TestUIntImplicitConversions()
        {
            uint ui = 200000U;
            return new object[]
    {
                (long)ui,   // uint -> long
    (ulong)ui,  // uint -> ulong
        (float)ui,  // uint -> float
                (double)ui, // uint -> double
   };
        }

        // Long implicit conversions
        public static object[] TestLongImplicitConversions()
        {
            long l = 1000000L;
            return new object[]
     {
    (float)l,   // long -> float
                (double)l,  // long -> double
          };
        }

        // ULong implicit conversions
        public static object[] TestULongImplicitConversions()
        {
            ulong ul = 2000000UL;
            return new object[]
         {
      (float)ul,  // ulong -> float
          (double)ul, // ulong -> double
               };
        }

        // Float implicit conversions
        public static object[] TestFloatImplicitConversions()
        {
            float f = 10.5f;
            return new object[]
                 {
        (double)f,  // float -> double
              };
        }

        // === EXPLICIT CONVERSION TESTS ===

        // Int explicit conversions
        public static object[] TestIntExplicitConversions()
        {
            int i = 300;
            return new object[]
       {
        (byte)i,    // int -> byte (may truncate)
    (sbyte)i,   // int -> sbyte (may truncate)
                (short)i, // int -> short
     (ushort)i,  // int -> ushort
           (uint)i,    // int -> uint
   };
        }

        // Long explicit conversions
        public static object[] TestLongExplicitConversions()
        {
            long l = 100000L;
            return new object[]
          {
      (byte)l,    // long -> byte (may truncate)
                (sbyte)l,   // long -> sbyte (may truncate)
     (short)l,   // long -> short (may truncate)
                (ushort)l,  // long -> ushort (may truncate)
      (int)l,     // long -> int (may truncate)
        (uint)l,    // long -> uint (may truncate)
                (ulong)l,   // long -> ulong
             };
        }

        // Float explicit conversions
        public static object[] TestFloatExplicitConversions()
        {
            float f = 123.75f;
            return new object[]
            {
       (byte)f,    // float -> byte (truncate)
      (sbyte)f,   // float -> sbyte (truncate)
       (short)f,   // float -> short (truncate)
  (ushort)f,  // float -> ushort (truncate)
                (int)f,     // float -> int (truncate)
                (uint)f,    // float -> uint (truncate)
  (long)f,    // float -> long (truncate)
      (ulong)f,   // float -> ulong (truncate)
          };
        }

        // Double explicit conversions
        public static object[] TestDoubleExplicitConversions()
        {
            double d = 456.99;
            return new object[]
                   {
     (byte)d,    // double -> byte (truncate)
          (sbyte)d,   // double -> sbyte (truncate)
      (short)d,   // double -> short (truncate)
                (ushort)d,  // double -> ushort (truncate)
  (int)d,     // double -> int (truncate)
     (uint)d,    // double -> uint (truncate)
     (long)d,  // double -> long (truncate)
       (ulong)d,// double -> ulong (truncate)
       (float)d,   // double -> float
                     };
        }        

        // === STRING CONVERSION TESTS ===

        public static object[] TestIntToString()
        {
            int i = 123;
            return new object[] { i.ToString() };
        }

        public static object[] TestDoubleToString()
        {
            double d = 45.67;
            return new object[] { d.ToString() };
        }

        public static object[] TestBoolToString()
        {
            bool b = true;
            return new object[] { b.ToString() };
        }

        public static object[] TestStringToInt()
        {
            string str = "123";
            return new object[] { int.Parse(str) };
        }

        // === CHECKED/UNCHECKED CONVERSION TESTS ===

        public static object[] TestCheckedConversion()
        {
            int i = 300; // > byte.MaxValue (255)
            try
            {
                checked
                {
                    byte result = (byte)i;
                    return new object[] { result };
                }
            }
            catch (System.OverflowException)
            {
                return new object[] { "OverflowException" };
            }
        }

        public static object[] TestUncheckedConversion()
        {
            int i = 300; // > byte.MaxValue (255)
            unchecked
            {
                byte result = (byte)i; // Should wrap around to 44
                return new object[] { result };
            }
        }

        // === NULLABLE CONVERSION TESTS ===

        public static object[] TestNullableHasValue()
        {
            int? nullableInt = 42;
            return new object[] { nullableInt.HasValue };
        }

        public static object[] TestNullableValue()
        {
            int? nullableInt = 42;
            return new object[] { nullableInt.Value };
        }

        public static object[] TestNullableNoValue()
        {
            int? nullInt = null;
            return new object[] { nullInt.HasValue };
        }

        // === CASTING OPERATION TESTS ===

        public static object[] TestUnboxingCast()
        {
            object obj = 42;
            return new object[] { (int)obj };
        }

        public static object[] TestBoxingCast()
        {
            int i = 42;
            return new object[] { (object)i };
        }

        public static object[] TestAsOperatorSuccess()
        {
            object obj = "Hello";
            string result = obj as string;
            return new object[] { result };
        }

        public static object[] TestAsOperatorFail()
        {
            object obj = 42;
            string result = obj as string;
            return new object[] { result == null };
        }

        public static object[] TestIsOperatorTrue()
        {
            object obj = 42;
            return new object[] { obj is int };
        }

        public static object[] TestIsOperatorFalse()
        {
            object obj = 42;
            return new object[] { obj is string };
        }

        // === SIGNED/UNSIGNED CONVERSION TESTS ===

        public static object[] TestSignedToUnsignedInt()
        {
            int negative = -100;
            return new object[] { (uint)negative };
        }

        public static object[] TestUnsignedToSignedInt()
        {
            uint positive = 200U;
            return new object[] { (int)positive };
        }

        public static object[] TestSignedToUnsignedByte()
        {
            sbyte negative = -50;
            return new object[] { (byte)negative };
        }

        public static object[] TestUnsignedToSignedByte()
        {
            byte positive = 200;
            return new object[] { (sbyte)positive };
        }

        // === FLOATING-POINT CONVERSION TESTS ===

        public static object[] TestFloatToDoubleInfinity()
        {
            float posInf = float.PositiveInfinity;
            return new object[] { (double)posInf };
        }

        public static object[] TestFloatToDoubleNegInfinity()
        {
            float negInf = float.NegativeInfinity;
            return new object[] { (double)negInf };
        }

        public static object[] TestFloatToDoubleNaN()
        {
            float nan = float.NaN;
            return new object[] { double.IsNaN((double)nan) };
        }

        public static object[] TestDoubleToFloat()
        {
            double large = 1e20;
            return new object[] { (float)large };
        }

        public static object[] TestFloatPrecision()
        {
            float precision = 0.1f + 0.2f;
            return new object[] { precision.ToString() };
        }

        // === CHARACTER CONVERSION TESTS ===

        public static object[] TestCharToInt()
        {
            char ch = 'A';
            return new object[] { (int)ch };
        }

        public static object[] TestIntToChar()
        {
            int ascii = 65;
            return new object[] { (char)ascii };
        }

        public static object[] TestByteToChar()
        {
            byte b = 66;
            return new object[] { (char)b };
        }

        public static object[] TestUShortToChar()
        {
            ushort us = 67;
            return new object[] { (char)us };
        }

        public static object[] TestCharToUShort()
        {
            char ch = 'A';
            return new object[] { (ushort)ch };
        }

        public static object[] TestCharIsLetter()
        {
            char ch = 'A';
            return new object[] { char.IsLetter(ch) };
        }

        public static object[] TestCharIsDigit()
        {
            char ch = '5';
            return new object[] { char.IsDigit(ch) };
        }

        public static object[] TestCharToUpper()
        {
            char ch = 'a';
            return new object[] { char.ToUpper(ch) };
        }

        // === ENUM CONVERSION TESTS ===

        public static object[] TestEnumToInt()
        {
            TestConversionEnum enumVal = TestConversionEnum.Second;
            return new object[] { (int)enumVal };
        }

        public static object[] TestIntToEnum()
        {
            int intVal = 1;
            return new object[] { (TestConversionEnum)intVal };
        }

        public static object[] TestEnumToString()
        {
            TestConversionEnum enumVal = TestConversionEnum.Second;
            return new object[] { enumVal.ToString() };
        }

        public static object[] TestEnumConstantToInt()
        {
            return new object[] { (int)TestConversionEnum.First };
        }

        public static object[] TestZeroToEnum()
        {
            return new object[] { (TestConversionEnum)0 };
        }

        public static object[] TestEnumEquality()
        {
            TestConversionEnum enumVal = TestConversionEnum.Second;
            return new object[] { enumVal == TestConversionEnum.Second };
        }

        // === REFERENCE CONVERSION TESTS ===

        public static object[] TestDowncastWithAs()
        {
            object obj = "Hello";
            return new object[] { obj as string };
        }

        public static object[] TestDowncastSuccess()
        {
            object obj = "Hello";
            return new object[] { (obj as string) != null };
        }

        public static object[] TestDowncastFail()
        {
            object nullObj = null;
            return new object[] { (nullObj as string) == null };
        }

        public static object[] TestIsString()
        {
            object obj = "Hello";
            return new object[] { obj is string };
        }

        public static object[] TestIsObject()
        {
            string str = "World";
            return new object[] { str is object };
        }

        public static object[] TestReferenceEquals()
        {
            object obj = "Hello";
            return new object[] { ReferenceEquals(obj, obj) };
        }

        public static object[] TestReferenceEqualsNull()
        {
            string str = "Hello";
            return new object[] { ReferenceEquals(str, null) };
        }

        // === ARRAY CONVERSION TESTS ===

        public static object[] TestArrayAsObject()
        {
            int[] intArray = { 1, 2, 3 };
            return new object[] { intArray as object };
        }

        public static object[] TestArrayTypeCheck()
        {
            int[] intArray = { 1, 2, 3 };
            return new object[] { intArray.GetType().BaseType == typeof(System.Array) };
        }

        public static object[] TestArrayLength()
        {
            object[] objArray = { "a", "b", "c" };
            return new object[] { objArray.Length };
        }

        public static object[] TestArrayElementAccess()
        {
            int[] intArray = { 1, 2, 3 };
            return new object[] { intArray[0] };
        }

        public static object[] TestArrayBoxing()
        {
            int[] intArray = { 1, 2, 3 };
            return new object[] { (object)intArray };
        }

        public static object[] TestArrayIsArray()
        {
            int[] intArray = { 1, 2, 3 };
            return new object[] { intArray.GetType().IsArray };
        }
    }

    // Test enum for enum conversion tests
    public enum TestConversionEnum
    {
        First = 0,
        Second = 1,
        Third = 2
    }
}