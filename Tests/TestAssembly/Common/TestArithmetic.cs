namespace TestAssembly
{
    public static class TestArithmetic
    {
        // ===== GRANULAR ADDITION TESTS BY TYPE =====

        public static object TestAdditionInt()
        {
            return 10 + 20;  // int + int
        }

        public static object TestAdditionUInt()
        {
            return 30U + 20U;  // uint + uint
        }

        public static object TestAdditionDouble()
        {
            return 10.5 + 20.3;  // double + double
        }

        public static object TestAdditionFloat()
        {
            return 5.5f + 2.2f;  // float + float
        }

        public static object TestAdditionDecimal()
        {
            return 100.25m + 200.75m;  // decimal + decimal
        }

        public static object TestAdditionLong()
        {
            return 100000L + 200000L;  // long + long
        }

        public static object TestAdditionULong()
        {
            return 300000UL + 400000UL;  // ulong + ulong
        }

        public static object TestAdditionShort()
        {
            return (short)((short)100 + (short)200);  // short + short
        }

        public static object TestAdditionUShort()
        {
            return (ushort)((ushort)300 + (ushort)400);  // ushort + ushort
        }

        public static object TestAdditionByte()
        {
            return (byte)((byte)50 + (byte)60);  // byte + byte
        }

        public static object TestAdditionSByte()
        {
            return (sbyte)((sbyte)-50 + (sbyte)60);  // sbyte + sbyte
        }

        public static object TestAdditionChar()
        {
            return 'A' + 'B';  // char + char
        }

        // ===== GRANULAR SUBTRACTION TESTS BY TYPE =====

        public static object TestSubtractionInt()
        {
            return 30 - 20;  // int - int
        }

        public static object TestSubtractionUInt()
        {
            return 50U - 20U;  // uint - uint
        }

        public static object TestSubtractionDouble()
        {
            return 30.8 - 20.3;  // double - double
        }

        public static object TestSubtractionFloat()
        {
            return 7.7f - 2.2f;  // float - float
        }

        public static object TestSubtractionDecimal()
        {
            return 301.0m - 200.75m;  // decimal - decimal
        }

        public static object TestSubtractionLong()
        {
            return 300000L - 200000L;  // long - long
        }

        public static object TestSubtractionULong()
        {
            return 700000UL - 400000UL;  // ulong - ulong
        }

        public static object TestSubtractionShort()
        {
            return (short)((short)300 - (short)200);  // short - short
        }

        public static object TestSubtractionUShort()
        {
            return (ushort)((ushort)700 - (ushort)400);  // ushort - ushort
        }

        public static object TestSubtractionByte()
        {
            return (byte)((byte)110 - (byte)60);  // byte - byte
        }

        public static object TestSubtractionSByte()
        {
            return (sbyte)((sbyte)10 - (sbyte)60);  // sbyte - sbyte
        }

        public static object TestSubtractionChar()
        {
            return 'Z' - 'A';  // char - char
        }

        // ===== GRANULAR MULTIPLICATION TESTS BY TYPE =====

        public static object TestMultiplicationInt()
        {
            return 10 * 5;  // int * int
        }

        public static object TestMultiplicationUInt()
        {
            return 30U * 3U;  // uint * uint
        }

        public static object TestMultiplicationDouble()
        {
            return 10.5 * 2.0;  // double * double
        }

        public static object TestMultiplicationFloat()
        {
            return 5.5f * 2.0f;  // float * float
        }

        public static object TestMultiplicationDecimal()
        {
            return 100.25m * 2.0m;  // decimal * decimal
        }

        public static object TestMultiplicationLong()
        {
            return 100000L * 2L;  // long * long
        }

        public static object TestMultiplicationULong()
        {
            return 300000UL * 2UL;  // ulong * ulong
        }

        public static object TestMultiplicationShort()
        {
            return (short)((short)100 * (short)2);  // short * short
        }

        public static object TestMultiplicationUShort()
        {
            return (ushort)((ushort)300 * (ushort)2);  // ushort * ushort
        }

        public static object TestMultiplicationByte()
        {
            return (byte)((byte)50 * (byte)2);  // byte * byte
        }

        public static object TestMultiplicationSByte()
        {
            return (sbyte)((sbyte)10 * (sbyte)3);  // sbyte * sbyte
        }

        public static object TestMultiplicationChar()
        {
            return 'A' * 2;  // char * int
        }

        // ===== GRANULAR DIVISION TESTS BY TYPE =====

        public static object TestDivisionInt()
        {
            return 20 / 4;  // int / int
        }

        public static object TestDivisionUInt()
        {
            return 60U / 3U;  // uint / uint
        }

        public static object TestDivisionDouble()
        {
            return 21.0 / 2.0;  // double / double
        }

        public static object TestDivisionFloat()
        {
            return 11.0f / 2.0f;  // float / float
        }

        public static object TestDivisionDecimal()
        {
            return 201.0m / 2.0m;  // decimal / decimal
        }

        public static object TestDivisionLong()
        {
            return 200000L / 2L;  // long / long
        }

        public static object TestDivisionULong()
        {
            return 600000UL / 2UL;  // ulong / ulong
        }

        public static object TestDivisionShort()
        {
            return (short)((short)200 / (short)2);  // short / short
        }

        public static object TestDivisionUShort()
        {
            return (ushort)((ushort)600 / (ushort)2);  // ushort / ushort
        }

        public static object TestDivisionByte()
        {
            return (byte)((byte)100 / (byte)2);  // byte / byte
        }

        public static object TestDivisionSByte()
        {
            return (sbyte)((sbyte)60 / (sbyte)2);  // sbyte / sbyte
        }

        public static object TestDivisionChar()
        {
            return (char)('Z' / 2);  // char / int
        }

        // ===== GRANULAR MODULO TESTS BY TYPE =====

        public static object TestModuloInt()
        {
            return 17 % 5;  // int % int
        }

        public static object TestModuloUInt()
        {
            return 30U % 7U;  // uint % uint
        }

        public static object TestModuloDouble()
        {
            return 17.5 % 5.0;  // double % double
        }

        public static object TestModuloFloat()
        {
            return 17.5f % 5.0f;  // float % float
        }

        public static object TestModuloDecimal()
        {
            return 17.25m % 5.0m;  // decimal % decimal
        }

        public static object TestModuloLong()
        {
            return 17L % 5L;  // long % long
        }

        public static object TestModuloULong()
        {
            return 30UL % 7UL;  // ulong % ulong
        }

        public static object TestModuloShort()
        {
            return (short)((short)17 % (short)5);  // short % short
        }

        public static object TestModuloUShort()
        {
            return (ushort)((ushort)30 % (ushort)7);  // ushort % ushort
        }

        public static object TestModuloByte()
        {
            return (byte)((byte)17 % (byte)5);  // byte % byte
        }

        public static object TestModuloSByte()
        {
            return (sbyte)((sbyte)17 % (sbyte)5);  // sbyte % sbyte
        }

        // ===== GRANULAR UNARY MINUS TESTS BY TYPE =====

        public static object TestUnaryMinusInt()
        {
            return -10;  // -int
        }

        public static object TestUnaryMinusDouble()
        {
            return -10.5;  // -double
        }

        public static object TestUnaryMinusFloat()
        {
            return -10.5f;  // -float
        }

        public static object TestUnaryMinusDecimal()
        {
            return -10.25m;  // -decimal
        }

        public static object TestUnaryMinusLong()
        {
            return -10L;  // -long
        }

        public static object TestUnaryMinusShort()
        {
            return -(short)10;  // -short
        }

        public static object TestUnaryMinusSByte()
        {
            return -(sbyte)10;  // -sbyte
        }

        // ===== GRANULAR INCREMENT TESTS BY TYPE =====

        public static object TestIncrementInt()
        {
            var a = 10;
            return ++a;  // pre-increment int
        }

        public static object TestIncrementUInt()
        {
            var a = 10U;
            return ++a;  // pre-increment uint
        }

        public static object TestIncrementDouble()
        {
            var a = 10.5;
            return ++a;  // pre-increment double
        }

        public static object TestIncrementFloat()
        {
            var a = 10.5f;
            return ++a;  // pre-increment float
        }

        public static object TestIncrementDecimal()
        {
            var a = 10.25m;
            return ++a;  // pre-increment decimal
        }

        public static object TestIncrementLong()
        {
            var a = 10L;
            return ++a;  // pre-increment long
        }

        public static object TestIncrementULong()
        {
            var a = 10UL;
            return ++a;  // pre-increment ulong
        }

        public static object TestIncrementShort()
        {
            var a = (short)10;
            return ++a;  // pre-increment short
        }

        public static object TestIncrementUShort()
        {
            var a = (ushort)10;
            return ++a;  // pre-increment ushort
        }

        public static object TestIncrementByte()
        {
            var a = (byte)10;
            return ++a;  // pre-increment byte
        }

        public static object TestIncrementSByte()
        {
            var a = (sbyte)10;
            return ++a;  // pre-increment sbyte
        }

        public static object TestIncrementChar()
        {
            var a = 'A';
            return ++a;  // pre-increment char
        }

        // ===== GRANULAR DECREMENT TESTS BY TYPE =====

        public static object TestDecrementInt()
        {
            var a = 10;
            return --a;  // pre-decrement int
        }

        public static object TestDecrementUInt()
        {
            var a = 10U;
            return --a;  // pre-decrement uint
        }

        public static object TestDecrementDouble()
        {
            var a = 10.5;
            return --a;  // pre-decrement double
        }

        public static object TestDecrementFloat()
        {
            var a = 10.5f;
            return --a;  // pre-decrement float
        }

        public static object TestDecrementDecimal()
        {
            var a = 10.25m;
            return --a;  // pre-decrement decimal
        }

        public static object TestDecrementLong()
        {
            var a = 10L;
            return --a;  // pre-decrement long
        }

        public static object TestDecrementULong()
        {
            var a = 10UL;
            return --a;  // pre-decrement ulong
        }

        public static object TestDecrementShort()
        {
            var a = (short)10;
            return --a;  // pre-decrement short
        }

        public static object TestDecrementUShort()
        {
            var a = (ushort)10;
            return --a;  // pre-decrement ushort
        }

        public static object TestDecrementByte()
        {
            var a = (byte)10;
            return --a;  // pre-decrement byte
        }

        public static object TestDecrementSByte()
        {
            var a = (sbyte)10;
            return --a;  // pre-decrement sbyte
        }

        public static object TestDecrementChar()
        {
            var a = 'B';
            return --a;  // pre-decrement char
        }

        // ===== POST-INCREMENT AND POST-DECREMENT TESTS =====

        public static object TestPostIncrementInt()
        {
            var a = 10;
            return a++;  // post-increment int
        }

        public static object TestPostIncrementUInt()
        {
            var a = 10U;
            return a++;  // post-increment uint
        }

        public static object TestPostIncrementDouble()
        {
            var a = 10.5;
            return a++;  // post-increment double
        }

        public static object TestPostIncrementFloat()
        {
            var a = 10.5f;
            return a++;  // post-increment float
        }

        public static object TestPostIncrementDecimal()
        {
            var a = 10.25m;
            return a++;  // post-increment decimal
        }

        public static object TestPostIncrementLong()
        {
            var a = 10L;
            return a++;  // post-increment long
        }

        public static object TestPostIncrementULong()
        {
            var a = 10UL;
            return a++;  // post-increment ulong
        }

        public static object TestPostIncrementShort()
        {
            var a = (short)10;
            return a++;  // post-increment short
        }

        public static object TestPostIncrementUShort()
        {
            var a = (ushort)10;
            return a++;  // post-increment ushort
        }

        public static object TestPostIncrementByte()
        {
            var a = (byte)10;
            return a++;  // post-increment byte
        }

        public static object TestPostIncrementSByte()
        {
            var a = (sbyte)10;
            return a++;  // post-increment sbyte
        }

        public static object TestPostIncrementChar()
        {
            var a = 'A';
            return a++;  // post-increment char
        }

        public static object TestPostDecrementInt()
        {
            var a = 10;
            return a--;  // post-decrement int
        }

        public static object TestPostDecrementUInt()
        {
            var a = 10U;
            return a--;  // post-decrement uint
        }

        public static object TestPostDecrementDouble()
        {
            var a = 10.5;
            return a--;  // post-decrement double
        }

        public static object TestPostDecrementFloat()
        {
            var a = 10.5f;
            return a--;  // post-decrement float
        }

        public static object TestPostDecrementDecimal()
        {
            var a = 10.25m;
            return a--;  // post-decrement decimal
        }

        public static object TestPostDecrementLong()
        {
            var a = 10L;
            return a--;  // post-decrement long
        }

        public static object TestPostDecrementULong()
        {
            var a = 10UL;
            return a--;  // post-decrement ulong
        }

        public static object TestPostDecrementShort()
        {
            var a = (short)10;
            return a--;  // post-decrement short
        }

        public static object TestPostDecrementUShort()
        {
            var a = (ushort)10;
            return a--;  // post-decrement ushort
        }

        public static object TestPostDecrementByte()
        {
            var a = (byte)10;
            return a--;  // post-decrement byte
        }

        public static object TestPostDecrementSByte()
        {
            var a = (sbyte)10;
            return a--;  // post-decrement sbyte
        }

        public static object TestPostDecrementChar()
        {
            var a = 'B';
            return a--;  // post-decrement char
        }

        // ===== SPECIAL EDGE CASE TESTS =====

        public static object TestDecimal()
        {
            decimal a = 50.5m;
            decimal b = 73.45m;
            return a + b * 120 / 3;
        }

        public static object[] TestOverflowAndEdgeCases()
        {
            // Integer overflow/underflow scenarios
            int maxInt = int.MaxValue;
            int minInt = int.MinValue;
            uint maxUint = uint.MaxValue;
            long maxLong = long.MaxValue;
            long minLong = long.MinValue;

            // Division by zero handling (these would throw exceptions in normal execution)
            // We'll test safe division operations
            int safeDiv1 = 100 / 2;
            double safeDiv2 = 100.0 / 2.0;
            float safeDiv3 = 100.0f / 2.0f;
            decimal safeDiv4 = 100.0m / 2.0m;

            // Zero operations
            int zeroAdd = 0 + 5;
            int zeroMul = 0 * 999;
            int zeroSub = 0 - 5;

            // Identity operations
            int identity1 = 42 * 1;
            double identity2 = 42.5 * 1.0;
            int identity3 = 42 + 0;

            // Negative zero handling
            double negZero = 0.0 * -1.0;
            float negZeroF = 0.0f * -1.0f;

            object[] results = new object[]
   {
     // Basic edge values
       maxInt,
          minInt,
maxUint,
  maxLong,
 minLong,
     
  // Safe operations
            safeDiv1,
      safeDiv2,
        safeDiv3,
        safeDiv4,
     
       // Zero operations
      zeroAdd,
            zeroMul,
             zeroSub,

    // Identity operations
         identity1,
     identity2,
          identity3,
       
          // Negative zero
    negZero,
    negZeroF,
          };

            return results;
        }

        public static object[] TestComplexExpressions()
        {
            // Mix of operations testing operator precedence
            var expr1 = 2 + 3 * 4;       // Should be 14, not 20
            var expr2 = (2 + 3) * 4;     // Should be 20
            var expr3 = 10 - 3 * 2;   // Should be 4, not 14
            var expr4 = (10 - 3) * 2;    // Should be 14
            var expr5 = 20 / 4 + 2;      // Should be 7
            var expr6 = 20 / (4 + 2);    // Should be approximately 3.33

            // Floating point complex expressions
            var expr7 = 3.5 + 2.5 * 1.5;    // Should be 7.25
            var expr8 = (3.5 + 2.5) * 1.5;  // Should be 9.0

            // Mixed type expressions
            var expr9 = 5 + 2.5;  // int + double
            var expr10 = 10L * 3;       // long * int
            var expr11 = 5.0f + 3.0;    // float + double

            // Modulo in complex expressions
            var expr12 = 15 % 4 + 2;// Should be 5 (3 + 2)
            var expr13 = 15 % (4 + 2);   // Should be 3 (15 % 6)

            object[] results = new object[]
                   {
         expr1, expr2, expr3, expr4, expr5, expr6,
                expr7, expr8, expr9, expr10, expr11,
    expr12, expr13
                   };

            return results;
        }

        public static object[] TestPowerOperations()
        {
            // Manual power calculations
            var square1 = 5 * 5;      // 5^2
            var square2 = 7 * 7;    // 7^2
            var cube1 = 3 * 3 * 3;      // 3^3
            var cube2 = 4 * 4 * 4;         // 4^3

            // Power of decimals
            var squareDecimal = 2.5m * 2.5m;     // 2.5^2
            var squareDouble = 3.5 * 3.5;   // 3.5^2
            var squareFloat = 2.5f * 2.5f;    // 2.5f^2

            // Power operations with different types
            var mixedPower = 2L * 2L * 2L;        // 2L^3

            object[] results = new object[]
          {
    square1, square2, cube1, cube2,
            squareDecimal, squareDouble, squareFloat,
   mixedPower
};

            return results;
        }
    }
}
