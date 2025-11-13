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
        public static object TestFloatImplicitConversions()
        {
            float f = 10.5f;
            return (double)f;  // float -> double (single conversion)
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
  (long)f,  // float -> long (truncate)
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

        public static object TestIntToString()
        {
            int i = 123;
            return i.ToString(); // Single string conversion
        }

        public static object TestDoubleToString()
        {
            double d = 45.67;
            return d.ToString(); // Single string conversion
        }

        public static object TestBoolToString()
        {
            bool b = true;
            return b.ToString(); // Single string conversion
        }

        public static object TestStringToInt()
        {
            string str = "123";
            return int.Parse(str); // Single parsing operation
        }

        // === CHECKED/UNCHECKED CONVERSION TESTS ===

        public static object TestCheckedConversion()
        {
            int i = 300; // > byte.MaxValue (255)
            try
            {
                checked
                {
                    byte result = (byte)i;
                    return result;
                }
            }
            catch (System.OverflowException)
            {
                return "OverflowException";
            }
        }

        public static object TestUncheckedConversion()
        {
            int i = 300; // > byte.MaxValue (255)
            unchecked
            {
                return (byte)i; // Should wrap around to 44
            }
        }

        // === NULLABLE CONVERSION TESTS ===

        //  public static object TestNullableHasValue()
        //     {
        //     int? nullableInt = 42;
        //      return nullableInt.HasValue; // Single boolean result
        //       }

        //public static object TestNullableValue()
        //       {
        //int? nullableInt = 42;
        //           return nullableInt.Value; // Single value result
        //     }

        //       public static object TestNullableNoValue()
        //       {
        //    int? nullInt = null;
        //     return nullInt.HasValue; // Single boolean result
        //    }

        // === CASTING OPERATION TESTS ===

        public static object TestAsOperatorSuccess()
        {
            object obj = "Hello";
            return obj as string; // Single 'as' operation
        }

        public static object TestAsOperatorFail()
        {
            object obj = 42;
            string result = obj as string;
            return result == null; // Single boolean result
        }

        public static object TestIsOperatorTrue()
        {
            object obj = 42;
            return obj is int; // Single boolean result
        }

        public static object TestIsOperatorFalse()
        {
            object obj = 42;
            return obj is string; // Single boolean result
        }

        // === SIGNED/UNSIGNED CONVERSION TESTS ===

        public static object TestSignedToUnsignedInt()
        {
            int negative = -100;
            return (uint)negative; // Single conversion
        }

        public static object TestUnsignedToSignedInt()
        {
            uint positive = 200U;
            return (int)positive; // Single conversion
        }

        public static object TestSignedToUnsignedByte()
        {
            sbyte negative = -50;
            return (byte)negative; // Single conversion
        }

        public static object TestUnsignedToSignedByte()
        {
            byte positive = 200;
            return (sbyte)positive; // Single conversion
        }

        // === FLOATING-POINT CONVERSION TESTS ===

        public static object TestFloatToDoubleInfinity()
        {
            float posInf = float.PositiveInfinity;
            return (double)posInf; // Single conversion
        }

        public static object TestFloatToDoubleNegInfinity()
        {
            float negInf = float.NegativeInfinity;
            return (double)negInf; // Single conversion
        }

        public static object TestFloatToDoubleNaN()
        {
            float nan = float.NaN;
            return double.IsNaN((double)nan); // Single boolean result
        }

        public static object TestDoubleToFloat()
        {
            double large = 1e20;
            return (float)large; // Single conversion
        }

        public static object TestFloatPrecision()
        {
            float precision = 0.1f + 0.2f;
            return precision.ToString(); // Single string result
        }

        // === CHARACTER CONVERSION TESTS ===

        public static object TestCharToInt()
        {
            char ch = 'A';
            return (int)ch; // Single conversion
        }

        public static object TestIntToChar()
        {
            int ascii = 65;
            return (char)ascii; // Single conversion
        }

        public static object TestByteToChar()
        {
            byte b = 66;
            return (char)b; // Single conversion
        }

        public static object TestUShortToChar()
        {
            ushort us = 67;
            return (char)us; // Single conversion
        }

        public static object TestCharToUShort()
        {
            char ch = 'A';
            return (ushort)ch; // Single conversion
        }

        public static object TestCharIsLetter()
        {
            char ch = 'A';
            return char.IsLetter(ch); // Single boolean result
        }

        public static object TestCharIsDigit()
        {
            char ch = '5';
            return char.IsDigit(ch); // Single boolean result
        }

        public static object TestCharToUpper()
        {
            char ch = 'a';
            return char.ToUpper(ch); // Single conversion
        }

        // === ENUM CONVERSION TESTS ===

        public static object TestEnumToInt()
        {
            TestConversionEnum enumVal = TestConversionEnum.Second;
            return (int)enumVal; // Single conversion
        }

        public static object TestIntToEnum()
        {
            int intVal = 1;
            return (TestConversionEnum)intVal; // Single conversion
        }

        public static object TestEnumToString()
        {
            TestConversionEnum enumVal = TestConversionEnum.Second;
            return enumVal.ToString(); // Single string result
        }

        public static object TestEnumConstantToInt()
        {
            return (int)TestConversionEnum.First; // Single conversion
        }

        public static object TestZeroToEnum()
        {
            return (TestConversionEnum)0; // Single conversion
        }

        public static object TestEnumEquality()
        {
            TestConversionEnum enumVal = TestConversionEnum.Second;
            return enumVal == TestConversionEnum.Second; // Single boolean result
        }

        // === REFERENCE CONVERSION TESTS ===

        public static object TestDowncastWithAs()
        {
            object obj = "Hello";
            return obj as string; // Single 'as' operation
        }

        public static object TestDowncastSuccess()
        {
            object obj = "Hello";
            return (obj as string) != null; // Single boolean result
        }

        public static object TestDowncastFail()
        {
            object nullObj = null;
            return (nullObj as string) == null; // Single boolean result
        }

        public static object TestIsString()
        {
            object obj = "Hello";
            return obj is string; // Single boolean result
        }

        public static object TestIsObject()
        {
            string str = "World";
            return str is object; // Single boolean result
        }

        public static object TestReferenceEquals()
        {
            object obj = "Hello";
            return ReferenceEquals(obj, obj); // Single boolean result
        }

        public static object TestReferenceEqualsNull()
        {
            string str = "Hello";
            return ReferenceEquals(str, null); // Single boolean result
        }

        // === ARRAY CONVERSION TESTS ===

        public static object TestArrayAsObject()
        {
            int[] intArray = { 1, 2, 3 };
            return intArray as object; // Single 'as' operation
        }

        public static object TestArrayTypeCheck()
        {
            int[] intArray = { 1, 2, 3 };
            return intArray.GetType().BaseType == typeof(System.Array); // Single boolean result
        }

        public static object TestArrayLength()
        {
            object[] objArray = { "a", "b", "c" };
            return objArray.Length; // Single property access
        }

        public static object TestArrayElementAccess()
        {
            int[] intArray = { 1, 2, 3 };
            return intArray[0]; // Single element access
        }

        public static object TestArrayBoxing()
        {
            int[] intArray = { 1, 2, 3 };
            return (object)intArray; // Single boxing operation
        }

        public static object TestArrayIsArray()
        {
            int[] intArray = { 1, 2, 3 };
            return intArray.GetType().IsArray; // Single boolean result
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