namespace TestAssembly
{
    public static class TestModuloAndEdgeCases
    {
        // Modulo operation tests
        public static object[] TestModulo()
        {
            int a = 10;
            int b = 3;
            int c = -10;
            int d = -3;
            uint ua = 10U;
            uint ub = 3U;
            float fa = 10.5f;
            float fb = 3.2f;
            double da = 10.7;
            double db = 3.1;

            return new object[]
           {
       a % b,      // 10 % 3 = 1
     c % b,      // -10 % 3 = -1
     a % d,      // 10 % -3 = 1  
        c % d,      // -10 % -3 = -1
    ua % ub,    // uint % uint
     fa % fb,  // float % float
    da % db,    // double % double
      };
        }

        // Division by zero and edge cases
        public static object[] TestDivisionEdgeCases()
        {
            int maxInt = int.MaxValue;
            int minInt = int.MinValue;
            long maxLong = long.MaxValue;
            long minLong = long.MinValue;

            return new object[]
             {
      maxInt / 2,             // Large positive / 2
     minInt / 2,       // Large negative / 2  
      maxLong / 2L,        // Large long / 2
     minLong / 2L,         // Large negative long / 2
     maxInt + 1L,            // int overflow to long
  (long)maxInt + 1L,      // explicit cast overflow
             };
        }

        // Overflow tests (unchecked)
        public static object[] TestUncheckedOverflow()
        {
            unchecked
            {
                int maxInt = int.MaxValue;
                int result1 = maxInt + 1;      // Should wrap to MinValue

                byte maxByte = byte.MaxValue;
                byte result2 = (byte)(maxByte + 1); // Should wrap to 0

                short maxShort = short.MaxValue;
                short result3 = (short)(maxShort + 1); // Should wrap to MinValue

                return new object[] { result1, result2, result3 };
            }
        }

        // Floating point special values
        public static object[] TestFloatingPointSpecialValues()
        {
            float positiveInfinity = float.PositiveInfinity;
            float negativeInfinity = float.NegativeInfinity;
            float nan = float.NaN;

            double dPositiveInfinity = double.PositiveInfinity;
            double dNegativeInfinity = double.NegativeInfinity;
            double dNan = double.NaN;

            return new object[]
            {
     float.IsInfinity(positiveInfinity),
     float.IsInfinity(negativeInfinity),
    float.IsNaN(nan),
       double.IsInfinity(dPositiveInfinity),
      double.IsInfinity(dNegativeInfinity),
 double.IsNaN(dNan),
   1.0f / 0.0f == positiveInfinity,   // Division resulting in infinity
     -1.0f / 0.0f == negativeInfinity,  // Division resulting in negative infinity
    0.0f / 0.0f != 0.0f / 0.0f,      // NaN != NaN is true
          };
        }

        // Precision tests
        public static object[] TestFloatingPointPrecision()
        {
            float f1 = 0.1f + 0.2f;
            float f2 = 0.3f;
            bool floatEqual = f1 == f2;   // May be false due to precision

            double d1 = 0.1 + 0.2;
            double d2 = 0.3;
            bool doubleEqual = d1 == d2;  // May be false due to precision

            float verySmall = 1e-10f;
            bool isZero = verySmall == 0.0f;

            return new object[] { floatEqual, doubleEqual, isZero, f1, d1 };
        }

        // Null reference tests  
        public static object[] TestNullReferences()
        {
            string nullString = null;
            object nullObject = null;

            bool isNullString = nullString == null;
            bool isNullObject = nullObject == null;
            bool stringEqualsNull = nullString == nullObject;

            return new object[] { isNullString, isNullObject, stringEqualsNull };
        }

        // Default value tests
        public static object[] TestDefaultValues()
        {
            int defaultInt = default(int);
            bool defaultBool = default(bool);
            char defaultChar = default(char);
            string defaultString = default(string);
            object defaultObject = default(object);

            return new object[] {
       defaultInt,
      defaultBool,
      (int)defaultChar,  // char default is '\0' = 0
      defaultString == null,
     defaultObject == null
       };
        }

        // String comparison edge cases
        public static object[] TestStringComparisons()
        {
            string empty = "";
            string space = " ";
            string nullStr = null;

            return new object[]
            {
   empty == "",
      empty.Length,
      space == " ",
     space.Length,
      nullStr == null,
     empty == nullStr,
       ReferenceEquals(empty, ""),  // String interning
              };
        }
    }
}