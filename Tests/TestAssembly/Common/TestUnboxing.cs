namespace TestAssembly
{
    public static class TestUnboxing
    {
        public static object TestUnboxingInt()
        {
            object boxed = 42;
            int unboxed = (int)boxed;  // Unboxing operation
            return unboxed;
        }

        public static object TestUnboxingUInt()
        {
            object boxed = 42U;
            uint unboxed = (uint)boxed;  // Unboxing operation
            return unboxed;
        }

        public static object TestUnboxingLong()
        {
            object boxed = 9223372036854775807L;
            long unboxed = (long)boxed;  // Unboxing operation
            return unboxed;
        }

        public static object TestUnboxingULong()
        {
            object boxed = 18446744073709551615UL;
            ulong unboxed = (ulong)boxed;  // Unboxing operation
            return unboxed;
        }

        public static object TestUnboxingShort()
        {
            object boxed = (short)32767;
            short unboxed = (short)boxed;  // Unboxing operation
            return unboxed;
        }

        public static object TestUnboxingUShort()
        {
            object boxed = (ushort)65535;
            ushort unboxed = (ushort)boxed;  // Unboxing operation
            return unboxed;
        }

        public static object TestUnboxingByte()
        {
            object boxed = (byte)255;
            byte unboxed = (byte)boxed;  // Unboxing operation
            return unboxed;
        }

        public static object TestUnboxingSByte()
        {
            object boxed = (sbyte)-128;
            sbyte unboxed = (sbyte)boxed;  // Unboxing operation
            return unboxed;
        }

        public static object TestUnboxingChar()
        {
            object boxed = 'Z';
            char unboxed = (char)boxed;  // Unboxing operation
            return unboxed;
        }

        public static object TestUnboxingFloat()
        {
            object boxed = 3.14159f;
            float unboxed = (float)boxed;  // Unboxing operation
            return unboxed;
        }

        public static object TestUnboxingDouble()
        {
            object boxed = 3.141592653589793;
            double unboxed = (double)boxed;  // Unboxing operation
            return unboxed;
        }

        public static object TestUnboxingDecimal()
        {
            object boxed = 79228162514264337593543950335m;
            decimal unboxed = (decimal)boxed;  // Unboxing operation
            return unboxed;
        }

        public static object TestUnboxingBool()
        {
            object boxed = true;
            bool unboxed = (bool)boxed;  // Unboxing operation
            return unboxed;
        }

        // Unboxing with edge values
        public static object TestUnboxingIntMin()
        {
            object boxed = int.MinValue;
            int unboxed = (int)boxed;
            return unboxed;
        }

        public static object TestUnboxingIntMax()
        {
            object boxed = int.MaxValue;
            int unboxed = (int)boxed;
            return unboxed;
        }

        public static object TestUnboxingFloatNaN()
        {
            object boxed = float.NaN;
            float unboxed = (float)boxed;
            return unboxed;
        }

        public static object TestUnboxingFloatInfinity()
        {
            object boxed = float.PositiveInfinity;
            float unboxed = (float)boxed;
            return unboxed;
        }

        public static object TestUnboxingDoubleNegativeInfinity()
        {
            object boxed = double.NegativeInfinity;
            double unboxed = (double)boxed;
            return unboxed;
        }

        // Unboxing from arrays
        public static object[] TestUnboxingFromArrays()
        {
            // Test unboxing from object arrays
            object[] boxedArray = new object[]
            {
     42,  // int
             42U,           // uint
      123456789L,  // long
     123456789UL,            // ulong
  (short)12345,         // short
         (ushort)12345,   // ushort
          (byte)200,    // byte
   (sbyte)-100,  // sbyte
      'A',  // char
         1.5f,  // float
        2.5,   // double
           3.75m,          // decimal
    true      // bool
          };

            object[] results = new object[]
          {
    (int)boxedArray[0],
                (uint)boxedArray[1],
     (long)boxedArray[2],
          (ulong)boxedArray[3],
  (short)boxedArray[4],
    (ushort)boxedArray[5],
     (byte)boxedArray[6],
     (sbyte)boxedArray[7],
       (char)boxedArray[8],
            (float)boxedArray[9],
                (double)boxedArray[10],
      (decimal)boxedArray[11],
      (bool)boxedArray[12]
           };

            return results;
        }

        // Test round-trip boxing and unboxing
        public static object[] TestBoxingUnboxingRoundTrip()
        {
            // Test round-trip boxing and unboxing for all types
            int intValue = 42;
            uint uintValue = 42U;
            long longValue = 123456789L;
            ulong ulongValue = 123456789UL;
            short shortValue = 12345;
            ushort ushortValue = 12345;
            byte byteValue = 200;
            sbyte sbyteValue = -100;
            char charValue = 'A';
            float floatValue = 1.5f;
            double doubleValue = 2.5;
            decimal decimalValue = 3.75m;
            bool boolValue = false;

            // Box all values
            object[] boxed = new object[]
 {
      intValue,
  uintValue,
   longValue,
       ulongValue,
     shortValue,
        ushortValue,
           byteValue,
  sbyteValue,
              charValue,
     floatValue,
    doubleValue,
          decimalValue,
    boolValue
  };

            // Unbox all values and verify they match original
            object[] results = new object[]
        {
 (int)boxed[0] == intValue,
         (uint)boxed[1] == uintValue,
   (long)boxed[2] == longValue,
    (ulong)boxed[3] == ulongValue,
(short)boxed[4] == shortValue,
      (ushort)boxed[5] == ushortValue,
           (byte)boxed[6] == byteValue,
 (sbyte)boxed[7] == sbyteValue,
           (char)boxed[8] == charValue,
  (float)boxed[9] == floatValue,
            (double)boxed[10] == doubleValue,
   (decimal)boxed[11] == decimalValue,
                (bool)boxed[12] == boolValue
               };

            return results;
        }

        // Test unboxing with wrong type (should throw InvalidCastException)
        public static object TestUnboxingWrongType()
        {
            try
            {
                object boxed = 42;  // int
                long wrongUnbox = (long)boxed;  // Try to unbox as long - should fail
                return false;  // Should not reach here
            }
            catch (System.InvalidCastException)
            {
                return true;  // Expected behavior
            }
            catch
            {
                return false;  // Unexpected exception type
            }
        }

        // Test unboxing null reference (should throw NullReferenceException)
        public static object TestUnboxingNull()
        {
            try
            {
                object boxed = null;
                int unboxed = (int)boxed;  // Should throw
                return false;  // Should not reach here
            }
            catch (System.NullReferenceException)
            {
                return true;  // Expected behavior
            }
            catch
            {
                return false;  // Unexpected exception type
            }
        }

        // Test unboxing with type compatibility
        public static object[] TestUnboxingCompatibility()
        {
            object[] results = new object[6];

            // These should work - exact type matches
            object intBoxed = 42;
            object floatBoxed = 3.14f;
            object boolBoxed = true;

            results[0] = (int)intBoxed;
            results[1] = (float)floatBoxed;
            results[2] = (bool)boolBoxed;

            // Test that implicit conversions don't work with unboxing
            try
            {
                object shortBoxed = (short)100;
                int intFromShort = (int)shortBoxed;  // Should fail - no implicit unboxing
                results[3] = false;
            }
            catch (System.InvalidCastException)
            {
                results[3] = true;  // Expected
            }

            try
            {
                object intBoxed2 = 42;
                long longFromInt = (long)intBoxed2;  // Should fail - no implicit unboxing
                results[4] = false;
            }
            catch (System.InvalidCastException)
            {
                results[4] = true;  // Expected
            }

            try
            {
                object floatBoxed2 = 3.14f;
                double doubleFromFloat = (double)floatBoxed2;  // Should fail
                results[5] = false;
            }
            catch (System.InvalidCastException)
            {
                results[5] = true;  // Expected
            }

            return results;
        }

        // Test unboxing in method return values
        public static object TestUnboxingInReturn()
        {
            object boxed = 123;
            return UnboxingHelper(boxed);
        }

        private static int UnboxingHelper(object boxed)
        {
            return (int)boxed;  // Unboxing in parameter
        }

        // Test unboxing with nullable result
        public static object[] TestUnboxingToNullable()
        {
            object intBoxed = 42;
            object nullBoxed = null;

            int? nullable1 = (int?)intBoxed;    // Should work
            int? nullable2 = (int?)nullBoxed;   // Should be null

            return new object[] { nullable1, nullable2 };
        }
    }
}