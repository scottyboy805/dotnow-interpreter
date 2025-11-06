
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
    }
}
