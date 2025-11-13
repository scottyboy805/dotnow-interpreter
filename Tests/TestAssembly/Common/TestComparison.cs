namespace TestAssembly
{
    public static class TestComparison
    {
        // Equality tests
        public static object[] TestEquality()
        {
            int a = 10;
            int b = 10;
            int c = 20;
            float d = 10.0f;
            double e = 10.0;
            string s1 = "hello";
            string s2 = "hello";
            string s3 = "world";
            bool t1 = true;
            bool t2 = true;
            bool f1 = false;

            return new object[]
   {
        a == b,      // int == int (true)
 a == c,         // int == int (false)
       a == d,         // int == float
    a == e, // int == double
       s1 == s2,   // string == string (true)
      s1 == s3,       // string == string (false)
      t1 == t2,       // bool == bool (true)
   t1 == f1, // bool == bool (false)
    null == null,   // null == null
    s1 == null,     // string == null
  };
        }

        // Inequality tests  
        public static object[] TestInequality()
        {
            int a = 10;
            int b = 10;
            int c = 20;
            float d = 10.0f;
            string s1 = "hello";
            string s2 = "hello";
            string s3 = "world";

            return new object[]
       {
        a != b,// int != int (false)
     a != c,// int != int (true)
        a != d,     // int != float
   s1 != s2,// string != string (false)
  s1 != s3,       // string != string (true)
  s1 != null,  // string != null
         };
        }

        // Less than tests
        public static object[] TestLessThan()
        {
            int a = 10;
            int b = 20;
            int c = 5;
            uint ua = 10U;
            uint ub = 20U;
            float fa = 10.5f;
            float fb = 20.5f;
            double da = 10.5;
            double db = 20.5;

            return new object[]
                {
 a < b,  // int < int (true)
  b < a,          // int < int (false)
      a < c,          // int < int (false)
     ua < ub,        // uint < uint
      fa < fb, // float < float
  da < db,        // double < double
                a < fa,    // int < float
        a < da,         // int < double
                        };
        }

        // Greater than tests
        public static object[] TestGreaterThan()
        {
            int a = 10;
            int b = 20;
            int c = 5;
            uint ua = 10U;
            uint ub = 20U;
            float fa = 10.5f;
            float fb = 20.5f;
            double da = 10.5;
            double db = 20.5;

            return new object[]
         {
     a > b,  // int > int (false)
      b > a,        // int > int (true)
      a > c,          // int > int (true)
    ua > ub,        // uint > uint
          fa > fb,        // float > float
     da > db,     // double > double
 a > fa,       // int > float
  a > da,    // int > double
       };
        }

        // Less than or equal tests
        public static object[] TestLessThanOrEqual()
        {
            int a = 10;
            int b = 20;
            int c = 10;
            float fa = 10.0f;

            return new object[]
        {
    a <= b,         // int <= int (true)
        b <= a,    // int <= int (false)
  a <= c,     // int <= int (true)
   a <= fa,        // int <= float
                  };
        }

        // Greater than or equal tests
        public static object[] TestGreaterThanOrEqual()
        {
            int a = 10;
            int b = 20;
            int c = 10;
            float fa = 10.0f;

            return new object[]
         {
       a >= b, // int >= int (false)
        b >= a,         // int >= int (true)
         a >= c,         // int >= int (true)
      a >= fa,    // int >= float
        };
        }

        // Branch-specific equality comparisons
        public static object[] TestBranchEqualityComparisons()
        {
            object[] results = new object[20];
            int index = 0;

            int a = 42, b = 42, c = 24;
            float f1 = 3.14f, f2 = 3.14f, f3 = 2.71f;
            string str1 = "test", str2 = "test", str3 = "other";

            // beq instruction tests (branch if equal)
            if (a == b) results[index++] = "beq_int_true";
            else results[index++] = "beq_int_false";

            if (a == c) results[index++] = "beq_int_false_true";
            else results[index++] = "beq_int_false_false";

            if (f1 == f2) results[index++] = "beq_float_true";
            else results[index++] = "beq_float_false";

            if (f1 == f3) results[index++] = "beq_float_false_true";
            else results[index++] = "beq_float_false_false";

            if (str1 == str2) results[index++] = "beq_string_true";
            else results[index++] = "beq_string_false";

            if (str1 == str3) results[index++] = "beq_string_false_true";
            else results[index++] = "beq_string_false_false";

            // bne instruction tests (branch if not equal)
            if (a != c) results[index++] = "bne_int_true";
            else results[index++] = "bne_int_false";

            if (a != b) results[index++] = "bne_int_false_true";
            else results[index++] = "bne_int_false_false";

            if (f1 != f3) results[index++] = "bne_float_true";
            else results[index++] = "bne_float_false";

            if (f1 != f2) results[index++] = "bne_float_false_true";
            else results[index++] = "bne_float_false_false";

            if (str1 != str3) results[index++] = "bne_string_true";
            else results[index++] = "bne_string_false";

            if (str1 != str2) results[index++] = "bne_string_false_true";
            else results[index++] = "bne_string_false_false";

            // Null equality tests
            string nullStr = null;
            object nullObj = null;
            object nonNullObj = new object();

            if (nullStr == null) results[index++] = "beq_null_true";
            else results[index++] = "beq_null_false";

            if (nullObj == nonNullObj) results[index++] = "beq_obj_null_false_true";
            else results[index++] = "beq_obj_null_false_false";

            if (nullStr != null) results[index++] = "bne_null_false_true";
            else results[index++] = "bne_null_false_false";

            if (nonNullObj != null) results[index++] = "bne_obj_null_true";
            else results[index++] = "bne_obj_null_false";

            return results;
        }

        // Branch-specific comparison tests for signed integers
        public static object[] TestBranchSignedComparisons()
        {
            object[] results = new object[24];
            int index = 0;

            int a = 10, b = 5, c = 15, d = 10;
            sbyte sb1 = 10, sb2 = 5;
            short s1 = 100, s2 = 50;
            long l1 = 1000L, l2 = 500L;

            // blt instruction tests (branch if less than)
            if (b < a) results[index++] = "blt_int_true";
            else results[index++] = "blt_int_false";

            if (a < b) results[index++] = "blt_int_false_true";
            else results[index++] = "blt_int_false_false";

            if (sb2 < sb1) results[index++] = "blt_sbyte_true";
            else results[index++] = "blt_sbyte_false";

            if (s2 < s1) results[index++] = "blt_short_true";
            else results[index++] = "blt_short_false";

            if (l2 < l1) results[index++] = "blt_long_true";
            else results[index++] = "blt_long_false";

            // bgt instruction tests (branch if greater than)
            if (a > b) results[index++] = "bgt_int_true";
            else results[index++] = "bgt_int_false";

            if (b > a) results[index++] = "bgt_int_false_true";
            else results[index++] = "bgt_int_false_false";

            if (sb1 > sb2) results[index++] = "bgt_sbyte_true";
            else results[index++] = "bgt_sbyte_false";

            if (s1 > s2) results[index++] = "bgt_short_true";
            else results[index++] = "bgt_short_false";

            if (l1 > l2) results[index++] = "bgt_long_true";
            else results[index++] = "bgt_long_false";

            // ble instruction tests (branch if less than or equal)
            if (b <= a) results[index++] = "ble_int_less_true";
            else results[index++] = "ble_int_less_false";

            if (a <= d) results[index++] = "ble_int_equal_true";
            else results[index++] = "ble_int_equal_false";

            if (a <= b) results[index++] = "ble_int_false_true";
            else results[index++] = "ble_int_false_false";

            // bge instruction tests (branch if greater than or equal)
            if (a >= b) results[index++] = "bge_int_greater_true";
            else results[index++] = "bge_int_greater_false";

            if (a >= d) results[index++] = "bge_int_equal_true";
            else results[index++] = "bge_int_equal_false";

            if (b >= a) results[index++] = "bge_int_false_true";
            else results[index++] = "bge_int_false_false";

            // Test with negative numbers
            int neg1 = -10, neg2 = -5, neg3 = -15;

            if (neg1 < neg2) results[index++] = "blt_negative_true";
            else results[index++] = "blt_negative_false";

            if (neg2 > neg1) results[index++] = "bgt_negative_true";
            else results[index++] = "bgt_negative_false";

            if (neg3 <= neg1) results[index++] = "ble_negative_true";
            else results[index++] = "ble_negative_false";

            if (neg2 >= neg3) results[index++] = "bge_negative_true";
            else results[index++] = "bge_negative_false";

            return results;
        }

        // Branch-specific comparison tests for unsigned integers
        public static object[] TestBranchUnsignedComparisons()
        {
            object[] results = new object[20];
            int index = 0;

            uint ua = 10U, ub = 5U, uc = 15U, ud = 10U;
            byte b1 = 200, b2 = 100;
            ushort us1 = 50000, us2 = 25000;
            ulong ul1 = 1000000UL, ul2 = 500000UL;

            // blt.un instruction tests (branch if less than unsigned)
            if (ub < ua) results[index++] = "blt_un_uint_true";
            else results[index++] = "blt_un_uint_false";

            if (ua < ub) results[index++] = "blt_un_uint_false_true";
            else results[index++] = "blt_un_uint_false_false";

            if (b2 < b1) results[index++] = "blt_un_byte_true";
            else results[index++] = "blt_un_byte_false";

            if (us2 < us1) results[index++] = "blt_un_ushort_true";
            else results[index++] = "blt_un_ushort_false";

            if (ul2 < ul1) results[index++] = "blt_un_ulong_true";
            else results[index++] = "blt_un_ulong_false";

            // bgt.un instruction tests (branch if greater than unsigned)
            if (ua > ub) results[index++] = "bgt_un_uint_true";
            else results[index++] = "bgt_un_uint_false";

            if (ub > ua) results[index++] = "bgt_un_uint_false_true";
            else results[index++] = "bgt_un_uint_false_false";

            if (b1 > b2) results[index++] = "bgt_un_byte_true";
            else results[index++] = "bgt_un_byte_false";

            // ble.un instruction tests (branch if less than or equal unsigned)
            if (ub <= ua) results[index++] = "ble_un_uint_less_true";
            else results[index++] = "ble_un_uint_less_false";

            if (ua <= ud) results[index++] = "ble_un_uint_equal_true";
            else results[index++] = "ble_un_uint_equal_false";

            if (ua <= ub) results[index++] = "ble_un_uint_false_true";
            else results[index++] = "ble_un_uint_false_false";

            // bge.un instruction tests (branch if greater than or equal unsigned)
            if (ua >= ub) results[index++] = "bge_un_uint_greater_true";
            else results[index++] = "bge_un_uint_greater_false";

            if (ua >= ud) results[index++] = "bge_un_uint_equal_true";
            else results[index++] = "bge_un_uint_equal_false";

            if (ub >= ua) results[index++] = "bge_un_uint_false_true";
            else results[index++] = "bge_un_uint_false_false";

            // Test with maximum values
            uint maxUint = uint.MaxValue;

            if (ua < maxUint) results[index++] = "blt_un_max_true";
            else results[index++] = "blt_un_max_false";

            if (maxUint > ua) results[index++] = "bgt_un_max_true";
            else results[index++] = "bgt_un_max_false";

            if (maxUint >= maxUint) results[index++] = "bge_un_max_equal_true";
            else results[index++] = "bge_un_max_equal_false";

            if (maxUint <= maxUint) results[index++] = "ble_un_max_equal_true";
            else results[index++] = "ble_un_max_equal_false";

            return results;
        }

        // Branch-specific comparison tests for floating-point numbers
        public static object[] TestBranchFloatingPointComparisons()
        {
            object[] results = new object[24];
            int index = 0;

            float f1 = 10.5f, f2 = 5.25f, f3 = 15.75f, f4 = 10.5f;
            double d1 = 100.125, d2 = 50.625, d3 = 150.875, d4 = 100.125;

            // Floating-point less than tests
            if (f2 < f1) results[index++] = "blt_float_true";
            else results[index++] = "blt_float_false";

            if (f1 < f2) results[index++] = "blt_float_false_true";
            else results[index++] = "blt_float_false_false";

            if (d2 < d1) results[index++] = "blt_double_true";
            else results[index++] = "blt_double_false";

            // Floating-point greater than tests
            if (f1 > f2) results[index++] = "bgt_float_true";
            else results[index++] = "bgt_float_false";

            if (f2 > f1) results[index++] = "bgt_float_false_true";
            else results[index++] = "bgt_float_false_false";

            if (d1 > d2) results[index++] = "bgt_double_true";
            else results[index++] = "bgt_double_false";

            // Floating-point less than or equal tests
            if (f2 <= f1) results[index++] = "ble_float_less_true";
            else results[index++] = "ble_float_less_false";

            if (f1 <= f4) results[index++] = "ble_float_equal_true";
            else results[index++] = "ble_float_equal_false";

            if (f1 <= f2) results[index++] = "ble_float_false_true";
            else results[index++] = "ble_float_false_false";

            // Floating-point greater than or equal tests
            if (f1 >= f2) results[index++] = "bge_float_greater_true";
            else results[index++] = "bge_float_greater_false";

            if (f1 >= f4) results[index++] = "bge_float_equal_true";
            else results[index++] = "bge_float_equal_false";

            if (f2 >= f1) results[index++] = "bge_float_false_true";
            else results[index++] = "bge_float_false_false";

            // Test with special floating-point values
            float positiveInfinity = float.PositiveInfinity;
            float negativeInfinity = float.NegativeInfinity;
            float nan = float.NaN;

            if (f1 < positiveInfinity) results[index++] = "blt_positive_infinity_true";
            else results[index++] = "blt_positive_infinity_false";

            if (f1 > negativeInfinity) results[index++] = "bgt_negative_infinity_true";
            else results[index++] = "bgt_negative_infinity_false";

            // NaN comparisons always return false
            if (nan == nan) results[index++] = "beq_nan_true";
            else results[index++] = "beq_nan_false";

            if (nan < f1) results[index++] = "blt_nan_true";
            else results[index++] = "blt_nan_false";

            if (nan > f1) results[index++] = "bgt_nan_true";
            else results[index++] = "bgt_nan_false";

            if (nan <= f1) results[index++] = "ble_nan_true";
            else results[index++] = "ble_nan_false";

            if (nan >= f1) results[index++] = "bge_nan_true";
            else results[index++] = "bge_nan_false";

            // Test zero comparisons
            float zero = 0.0f;
            float negativeZero = -0.0f;

            if (zero == negativeZero) results[index++] = "beq_zero_true";
            else results[index++] = "beq_zero_false";

            if (f1 > zero) results[index++] = "bgt_zero_true";
            else results[index++] = "bgt_zero_false";

            if (zero >= negativeZero) results[index++] = "bge_negative_zero_true";
            else results[index++] = "bge_negative_zero_false";

            return results;
        }

        // Mixed-type comparison branch tests
        public static object[] TestBranchMixedTypeComparisons()
        {
            object[] results = new object[16];
            int index = 0;

            int intVal = 10;
            float floatVal = 10.0f;
            double doubleVal = 10.0;
            decimal decimalVal = 10.0m;

            // int to float comparisons
            if (intVal == floatVal) results[index++] = "beq_int_float_true";
            else results[index++] = "beq_int_float_false";

            if (intVal < (floatVal + 1.0f)) results[index++] = "blt_int_float_true";
            else results[index++] = "blt_int_float_false";

            if (intVal > (floatVal - 1.0f)) results[index++] = "bgt_int_float_true";
            else results[index++] = "bgt_int_float_false";

            // int to double comparisons
            if (intVal == doubleVal) results[index++] = "beq_int_double_true";
            else results[index++] = "beq_int_double_false";

            if (intVal <= doubleVal) results[index++] = "ble_int_double_true";
            else results[index++] = "ble_int_double_false";

            if (intVal >= doubleVal) results[index++] = "bge_int_double_true";
            else results[index++] = "bge_int_double_false";

            // float to double comparisons
            if (floatVal == (float)doubleVal) results[index++] = "beq_float_double_true";
            else results[index++] = "beq_float_double_false";

            if (floatVal != (floatVal + 0.1f)) results[index++] = "bne_float_modified_true";
            else results[index++] = "bne_float_modified_false";

            // Precision comparison tests
            float precisionFloat = 0.1f + 0.2f;
            float expectedFloat = 0.3f;

            if (precisionFloat == expectedFloat) results[index++] = "beq_precision_true";
            else results[index++] = "beq_precision_false";

            if (System.Math.Abs(precisionFloat - expectedFloat) < 0.0001f) results[index++] = "precision_tolerance_true";
            else results[index++] = "precision_tolerance_false";

            // char comparisons
            char ch1 = 'A', ch2 = 'B', ch3 = 'A';

            if (ch1 == ch3) results[index++] = "beq_char_true";
            else results[index++] = "beq_char_false";

            if (ch1 < ch2) results[index++] = "blt_char_true";
            else results[index++] = "blt_char_false";

            if (ch2 > ch1) results[index++] = "bgt_char_true";
            else results[index++] = "bgt_char_false";

            // char to int comparison
            int charAsInt = (int)ch1;
            if (charAsInt == 65) results[index++] = "beq_char_int_true";
            else results[index++] = "beq_char_int_false";

            if ((char)66 == 'B') results[index++] = "beq_int_char_true";
            else results[index++] = "beq_int_char_false";

            if ((int)ch2 > (int)ch1) results[index++] = "bgt_char_as_int_true";
            else results[index++] = "bgt_char_as_int_false";

            return results;
        }
    }
}