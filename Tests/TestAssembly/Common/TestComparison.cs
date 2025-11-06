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
                a == e,         // int == double
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
        a != d,         // int != float
   s1 != s2,  // string != string (false)
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
      fa < fb,        // float < float
             da < db,        // double < double
                a < fa,      // int < float
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
      b > a,          // int > int (true)
      a > c,          // int > int (true)
    ua > ub,        // uint > uint
          fa > fb,        // float > float
     da > db,     // double > double
 a > fa,       // int > float
             a > da,         // int > double
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
    }
}