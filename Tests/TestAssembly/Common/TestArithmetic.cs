namespace TestAssembly
{
    public static class TestArithmetic
    {
        // Methods
        public static object[] TestAddition()
        {
            var a = 10;                         // int + int
            var b = 30U;                        // uint + uint
            var c = 10.5;                       // double + double
            var d = 5.5f;                       // float + float
            var e = 100.25m;                    // decimal + decimal
            var f = 100000L;                    // long + long
            var g = 300000UL;                   // ulong + ulong
            var h = (short)100;                 // short + short
            var i = (ushort)300;                // ushort + ushort
            var j = (byte)50;                   // byte + byte
            var k = 'A';                        // char + char (ASCII addition)
            var l = (sbyte)-50;                 // sbyte + sbyte
            var m = 10;                         // int + double
            var n = 20.3;                       // double + float
            var o = 100000L;                    // long + int
            var p = 100.25m;                    // decimal + double (casted to decimal)

            object[] results = new object[]
            {
                a + 20,                         // int + int
                b + 20U,                        // uint + uint
                c + 20.3,                       // double + double
                d + 2.2f,                       // float + float
                e + 200.75m,                    // decimal + decimal
                f + 200000L,                    // long + long
                g + 400000UL,                   // ulong + ulong
                h + (short)200,                 // short + short
                i + (ushort)400,                // ushort + ushort
                j + (byte)60,                   // byte + byte
                k + 'B',                        // char + char (ASCII addition)
                l + (sbyte)60,                  // sbyte + sbyte
                m + 10.5,                       // int + double
                n + 5.5f,                       // double + float
                o + 10,                         // long + int
                p + (decimal)10.5,              // decimal + double (casted to decimal)
            };

            return results;
        }

        public static object[] TestSubtraction()
        {
            var a = 10;                         // int - int
            var b = 30U;                        // uint - uint
            var c = 10.5;                       // double - double
            var d = 5.5f;                       // float - float
            var e = 100.25m;                    // decimal - decimal
            var f = 100000L;                    // long - long
            var g = 300000UL;                   // ulong - ulong
            var h = (short)100;                 // short - short
            var i = (ushort)300;                // ushort - ushort
            var j = (byte)50;                   // byte - byte
            var k = 'A';                        // char - char (ASCII addition)
            var l = (sbyte)-50;                 // sbyte - sbyte
            var m = 10;                         // int - double
            var n = 20.3;                       // double - float
            var o = 100000L;                    // long - int
            var p = 100.25m;                    // decimal - double (casted to decimal)

            object[] results = new object[]
            {
                a - 20,                         // int - int
                b - 20U,                        // uint - uint
                c - 20.3,                       // double - double
                d - 2.2f,                       // float - float
                e - 200.75m,                    // decimal - decimal
                f - 200000L,                    // long - long
                g - 400000UL,                   // ulong - ulong
                h - (short)200,                 // short - short
                i - (ushort)400,                // ushort - ushort
                j - (byte)60,                   // byte - byte
                k - 'B',                        // char - char (ASCII addition)
                l - (sbyte)60,                  // sbyte - sbyte
                m - 10.5,                       // int - double
                n - 5.5f,                       // double - float
                o - 10,                         // long - int
                p - (decimal)10.5,              // decimal - double (casted to decimal)
            };

            return results;
        }

        public static object[] TestMultiplication()
        {
            var a = 10;                         // int * int
            var b = 30U;                        // uint * uint
            var c = 10.5;                       // double * double
            var d = 5.5f;                       // float * float
            var e = 100.25m;                    // decimal * decimal
            var f = 100000L;                    // long * long
            var g = 300000UL;                   // ulong * ulong
            var h = (short)100;                 // short * short
            var i = (ushort)300;                // ushort * ushort
            var j = (byte)50;                   // byte * byte
            var k = 'A';                        // char * char (ASCII addition)
            var l = (sbyte)-50;                 // sbyte * sbyte
            var m = 10;                         // int * double
            var n = 20.3;                       // double * float
            var o = 100000L;                    // long * int
            var p = 100.25m;                    // decimal * double (casted to decimal)

            object[] results = new object[]
            {
                a * 20,                         // int * int
                b * 20U,                        // uint * uint
                c * 20.3,                       // double * double
                d * 2.2f,                       // float * float
                e * 200.75m,                    // decimal * decimal
                f * 200000L,                    // long * long
                g * 400000UL,                   // ulong * ulong
                h * (short)200,                 // short * short
                i * (ushort)400,                // ushort * ushort
                j * (byte)60,                   // byte * byte
                k * 'B',                        // char * char (ASCII addition)
                l * (sbyte)60,                  // sbyte * sbyte
                m * 10.5,                       // int * double
                n * 5.5f,                       // double * float
                o * 10,                         // long * int
                p * (decimal)10.5,              // decimal * double (casted to decimal)
            };

            return results;
        }

        public static object[] TestDivide()
        {
            var a = 10;                         // int / int
            var b = 30U;                        // uint / uint
            var c = 10.5;                       // double / double
            var d = 5.5f;                       // float / float
            var e = 100.25m;                    // decimal / decimal
            var f = 100000L;                    // long / long
            var g = 300000UL;                   // ulong / ulong
            var h = (short)100;                 // short / short
            var i = (ushort)300;                // ushort / ushort
            var j = (byte)50;                   // byte / byte
            var k = 'A';                        // char / char (ASCII addition)
            var l = (sbyte)-50;                 // sbyte / sbyte
            var m = 10;                         // int / double
            var n = 20.3;                       // double / float
            var o = 100000L;                    // long / int
            var p = 100.25m;                    // decimal / double (casted to decimal)

            object[] results = new object[]
            {
                a / 20,                         // int / int
                b / 20U,                        // uint / uint
                c / 20.3,                       // double / double
                d / 2.2f,                       // float / float
                e / 200.75m,                    // decimal / decimal
                f / 200000L,                    // long / long
                g / 400000UL,                   // ulong / ulong
                h / (short)200,                 // short / short
                i / (ushort)400,                // ushort / ushort
                j / (byte)60,                   // byte / byte
                k / 'B',                        // char / char (ASCII addition)
                l / (sbyte)60,                  // sbyte / sbyte
                m / 10.5,                       // int / double
                n / 5.5f,                       // double / float
                o / 10,                         // long / int
                p / (decimal)10.5,              // decimal / double (casted to decimal)
            };

            return results;
        }

        public static object TestDecimal()
        {
            decimal a = 50.5m;
            decimal b = 73.45m;

            return a + b * 120 / 3;
        }

        // Modulo/Remainder operation tests
        public static object[] TestModulo()
        {
            var a = 17;    // int % int
            var b = 30U;      // uint % uint
            var c = 17.5;           // double % double
            var d = 17.5f;   // float % float
            var e = 17.25m;  // decimal % decimal
            var f = 17L;               // long % long
            var g = 30UL;      // ulong % ulong
            var h = (short)17;             // short % short
            var i = (ushort)30;  // ushort % ushort
            var j = (byte)17;   // byte % byte
            var k = (sbyte)17;   // sbyte % sbyte
            var l = 17;    // int % double
            var m = 17.5;         // double % float
            var n = 17L;    // long % int
            var o = 17.25m;          // decimal % int

            object[] results = new object[]
   {
   a % 5,      // int % int
    b % 7U,// uint % uint
     c % 5.0,    // double % double
                d % 5.0f,    // float % float
  e % 5.0m,             // decimal % decimal
            f % 5L,      // long % long
     g % 7UL,   // ulong % ulong
          h % (short)5,          // short % short
            i % (ushort)7,      // ushort % ushort
   j % (byte)5,      // byte % byte
   k % (sbyte)5,             // sbyte % sbyte
 l % 5.0,// int % double
            m % 5.0f,        // double % float
         n % 5,     // long % int
    o % 5m,        // decimal % decimal (converted)
       };

            return results;
        }

        // Unary minus operation tests
        public static object[] TestUnaryMinus()
        {
            var a = 10;        // -int
            var b = 10U;     // Cannot negate uint directly
            var c = 10.5;   // -double
            var d = 10.5f;         // -float
            var e = 10.25m;        // -decimal
            var f = 10L;             // -long
            var g = (short)10;   // -short
            var h = (sbyte)10;      // -sbyte
            var i = -5;// -(-int)
            var j = -5.5;      // -(-double)

            object[] results = new object[]
               {
          -a,  // -int
    -c,        // -double
              -d,    // -float
         -e,          // -decimal
         -f,      // -long
     -g,    // -short
              -h,      // -sbyte
      -i,             // -(-int) = positive
     -j,           // -(-double) = positive
                      };

            return results;
        }

        // Increment and decrement operations
        public static object[] TestIncrementDecrement()
        {
            var a = 10;
            var b = 10U;
            var c = 10.5;
            var d = 10.5f;
            var e = 10.25m;
            var f = 10L;
            var g = 10UL;
            var h = (short)10;
            var i = (ushort)10;
            var j = (byte)10;
            var k = (sbyte)10;
            var l = 'A';             // char

            // Pre-increment
            var a_pre = ++a;
            var b_pre = ++b;
            var c_pre = ++c;
            var d_pre = ++d;
            var e_pre = ++e;
            var f_pre = ++f;
            var g_pre = ++g;
            var h_pre = ++h;
            var i_pre = ++i;
            var j_pre = ++j;
            var k_pre = ++k;
            var l_pre = ++l;

            // Reset values for post-increment
            a = 10; b = 10U; c = 10.5; d = 10.5f; e = 10.25m;
            f = 10L; g = 10UL; h = 10; i = 10; j = 10; k = 10; l = 'A';

            // Post-increment (capture value before increment)
            var a_post = a++;
            var b_post = b++;
            var c_post = c++;
            var d_post = d++;
            var e_post = e++;
            var f_post = f++;
            var g_post = g++;
            var h_post = h++;
            var i_post = i++;
            var j_post = j++;
            var k_post = k++;
            var l_post = l++;

            // Reset values for decrement
            a = 10; b = 10U; c = 10.5; d = 10.5f; e = 10.25m;
            f = 10L; g = 10UL; h = 10; i = 10; j = 10; k = 10; l = 'A';

            // Pre-decrement
            var a_predec = --a;
            var b_predec = --b;
            var c_predec = --c;
            var d_predec = --d;
            var e_predec = --e;
            var f_predec = --f;
            var g_predec = --g;
            var h_predec = --h;
            var i_predec = --i;
            var j_predec = --j;
            var k_predec = --k;
            var l_predec = --l;

            object[] results = new object[]
           {
                // Pre-increment results
                a_pre, b_pre, c_pre, d_pre, e_pre, f_pre, g_pre, h_pre, i_pre, j_pre, k_pre, l_pre,
                // Post-increment results (original values)
 a_post, b_post, c_post, d_post, e_post, f_post, g_post, h_post, i_post, j_post, k_post, l_post,
          // Pre-decrement results  
       a_predec, b_predec, c_predec, d_predec, e_predec, f_predec, g_predec, h_predec, i_predec, j_predec, k_predec, l_predec,
   };

            return results;
        }

        // Bitwise arithmetic operations (complement to bitwise tests)
        public static object[] TestBitwiseArithmetic()
        {
            int a = 0xFF;             // 255
            int b = 0xF0;     // 240
            uint ua = 0xFFU;
            uint ub = 0xF0U;
            long la = 0xFFL;
            long lb = 0xF0L;

            object[] results = new object[]
                {
       // AND operations
                a & b,        // int & int
     ua & ub,   // uint & uint
          la & lb,                 // long & long
     
                // OR operations
       a | b,              // int | int
   ua | ub, // uint | uint
         la | lb,             // long | long
        
    // XOR operations
      a ^ b,                // int ^ int
    ua ^ ub,       // uint ^ uint
 la ^ lb,        // long ^ long

// NOT operations
          ~a,     // ~int
            ~ua,          // ~uint
           ~la,              // ~long
                
     // Shift operations
                a << 2,    // int << int
       ua << 2,            // uint << int
la << 2,// long << int
   a >> 2,  // int >> int
           ua >> 2,         // uint >> int
                la >> 2,       // long >> int
       };

            return results;
        }

        // Overflow and edge case arithmetic tests
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

        // Complex arithmetic expressions
        public static object[] TestComplexExpressions()
        {
            // Mix of operations testing operator precedence
            var expr1 = 2 + 3 * 4;      // Should be 14, not 20
            var expr2 = (2 + 3) * 4;         // Should be 20
            var expr3 = 10 - 3 * 2;   // Should be 4, not 14
            var expr4 = (10 - 3) * 2;    // Should be 14
            var expr5 = 20 / 4 + 2;         // Should be 7
            var expr6 = 20 / (4 + 2);            // Should be approximately 3.33

            // Floating point complex expressions
            var expr7 = 3.5 + 2.5 * 1.5;       // Should be 7.25
            var expr8 = (3.5 + 2.5) * 1.5;       // Should be 9.0

            // Mixed type expressions
            var expr9 = 5 + 2.5;      // int + double
            var expr10 = 10L * 3;         // long * int
            var expr11 = 5.0f + 3.0;      // float + double

            // Modulo in complex expressions
            var expr12 = 15 % 4 + 2;      // Should be 5 (3 + 2)
            var expr13 = 15 % (4 + 2);    // Should be 3 (15 % 6)

            object[] results = new object[]
                    {
    expr1, expr2, expr3, expr4, expr5, expr6,
     expr7, expr8, expr9, expr10, expr11,
                expr12, expr13
        };

            return results;
        }

        // Power operations (using manual multiplication since Math.Pow might not be available)
        public static object[] TestPowerOperations()
        {
            // Manual power calculations
            var square1 = 5 * 5;        // 5^2
            var square2 = 7 * 7;        // 7^2
            var cube1 = 3 * 3 * 3;   // 3^3
            var cube2 = 4 * 4 * 4;         // 4^3

            // Power of decimals
            var squareDecimal = 2.5m * 2.5m;     // 2.5^2
            var squareDouble = 3.5 * 3.5;     // 3.5^2
            var squareFloat = 2.5f * 2.5f;       // 2.5f^2

            // Power operations with different types
            var mixedPower = 2L * 2L * 2L;       // 2L^3

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
