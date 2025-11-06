namespace TestAssembly
{
    public static class TestLogical
  {
        // Logical AND tests
        public static object[] TestLogicalAnd()
  {
            bool t = true;
  bool f = false;
    
     return new object[]
    {
       t && t,  // true && true
   t && f,  // true && false
       f && t,  // false && true
  f && f,  // false && false
      };
  }

     // Logical OR tests
        public static object[] TestLogicalOr()
     {
 bool t = true;
   bool f = false;
   
            return new object[]
  {
    t || t,  // true || true
 t || f,  // true || false
        f || t,  // false || true
   f || f,  // false || false
            };
        }

        // Logical NOT tests
        public static object[] TestLogicalNot()
  {
    bool t = true;
       bool f = false;
    
   return new object[]
   {
      !t,  // !true
    !f,  // !false
  };
 }

     // Short-circuit AND test
        public static object[] TestShortCircuitAnd()
        {
 bool result1 = false && ThrowsException(); // Should not throw
bool result2 = true && ReturnsFalse();     // Should call second method
    
     return new object[]
        {
      result1, // false
          result2  // false
    };
  }

        // Short-circuit OR test
       public static object[] TestShortCircuitOr()
    {
         bool result1 = true || ThrowsException();  // Should not throw
 bool result2 = false || ReturnsTrue();    // Should call second method
    
   return new object[]
      {
     result1, // true
     result2  // true
   };
        }

        // Conditional (ternary) operator tests
       public static object[] TestConditional()
     {
        bool condition1 = true;
         bool condition2 = false;
      int a = 10;
        int b = 20;
  
      return new object[]
        {
    condition1 ? a : b,  // true ? 10 : 20 = 10
  condition2 ? a : b,  // false ? 10 : 20 = 20
      condition1 ? "yes" : "no", // true ? "yes" : "no" = "yes"
 condition2 ? "yes" : "no", // false ? "yes" : "no" = "no"
  };
     }

        // Helper methods for short-circuit tests
        private static bool ThrowsException()
 {
   throw new System.Exception("Should not be called");
      }

      private static bool ReturnsFalse()
        {
   return false;
        }

      private static bool ReturnsTrue()
      {
       return true;
     }
    }
}