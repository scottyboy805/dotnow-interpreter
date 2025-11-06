namespace TestAssembly
{
    public static class TestConversion
    {
        // Implicit numeric conversions
        public static object[] TestImplicitConversions()
        {
            byte b = 100;
            short s = 1000;
            int i = 100000;
            long l = 10000000000L;
            float f = 10.5f;
            double d = 20.5;

            return new object[]
        {
  (short)b,   // byte -> short
(int)b,     // byte -> int
  (long)b,// byte -> long
     (float)b,   // byte -> float
      (double)b,  // byte -> double
       (int)s,     // short -> int
   (long)s,    // short -> long
      (float)s, // short -> float
      (double)s,  // short -> double
      (long)i,    // int -> long
      (float)i,   // int -> float
      (double)i,  // int -> double
       (double)f,  // float -> double
              };
        }

        // Explicit numeric conversions
        public static object[] TestExplicitConversions()
        {
            int i = 300;
            long l = 100000L;
            float f = 10.75f;
            double d = 20.99;

            return new object[]
            {
    (byte)i,    // int -> byte (may truncate)
      (short)i,   // int -> short
  (int)l,     // long -> int (may truncate)
        (byte)l,    // long -> byte (may truncate)
      (int)f,     // float -> int (truncate)
     (byte)f,    // float -> byte (truncate)
  (int)d,     // double -> int (truncate)
            (float)d,   // double -> float
               };
        }

        // Boxing/Unboxing tests
        public static object[] TestBoxingUnboxing()
        {
            int i = 42;
            object boxedInt = i;     // Boxing
            int unboxedInt = (int)boxedInt; // Unboxing

            bool b = true;
            object boxedBool = b;       // Boxing
            bool unboxedBool = (bool)boxedBool; // Unboxing

            return new object[]
            {
     boxedInt,
     unboxedInt,
        boxedBool,
      unboxedBool
                  };
        }

        // String conversions
        public static object[] TestStringConversions()
        {
            int i = 123;
            double d = 45.67;
            bool b = true;

            return new object[]
          {
      i.ToString(),  // int -> string
       d.ToString(),  // double -> string
     b.ToString(),  // bool -> string
       };
        }

        // Checked/Unchecked conversions
        public static object[] TestCheckedConversions()
        {
            int i = 300; // > byte.MaxValue (255)

            try
            {
                checked
                {
                    byte result = (byte)i; // Should throw OverflowException
                    return new object[] { result };
                }
            }
            catch (System.OverflowException)
            {
                return new object[] { "OverflowException" };
            }
        }

        public static object[] TestUncheckedConversions()
        {
            int i = 300; // > byte.MaxValue (255)

            unchecked
            {
                byte result = (byte)i; // Should wrap around
                return new object[] { result }; // Should be 44 (300 - 256)
            }
        }

        // Nullable conversions
        public static object[] TestNullableConversions()
        {
            int? nullableInt = 42;
            int? nullInt = null;

            int valueFromNullable = nullableInt.Value;
            bool hasValue1 = nullableInt.HasValue;
            bool hasValue2 = nullInt.HasValue;

            return new object[]
           {
    valueFromNullable,
hasValue1,
hasValue2
            };
        }
    }
}