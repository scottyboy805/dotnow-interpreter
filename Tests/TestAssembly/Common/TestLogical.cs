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
bool result2 = true && ReturnsFalse();  // Should call second method
    
     return new object[]
        {
  result1, // false
      result2// false
    };
  }

      // Short-circuit OR test
       public static object[] TestShortCircuitOr()
    {
         bool result1 = true || ThrowsException();  // Should not throw
 bool result2 = false || ReturnsTrue(); // Should call second method
    
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

        // Complex logical branch patterns
   public static object[] TestComplexLogicalBranches()
        {
      object[] results = new object[16];
          int index = 0;
    
            bool a = true, b = false, c = true, d = false;
            
    // Test complex AND operations with branching
     if (a && b && c)
             results[index++] = "and_false_path";
        else
      results[index++] = "and_true_path";
            
   if (a && c && !b)
         results[index++] = "and_complex_true";
            else
      results[index++] = "and_complex_false";
      
  // Test complex OR operations with branching  
    if (b || d || !a)
             results[index++] = "or_false_path";
   else
    results[index++] = "or_true_path";
    
 if (a || c || d)
      results[index++] = "or_complex_true";
      else
        results[index++] = "or_complex_false";
       
  // Test mixed logical operations
       if ((a && c) || (b && d))
       results[index++] = "mixed_true";
         else
  results[index++] = "mixed_false";
   
            if ((a || b) && (c || d))
      results[index++] = "mixed2_true";
            else
         results[index++] = "mixed2_false";
      
            // Test negated complex expressions
     if (!(a && b))
        results[index++] = "negated_and_true";
          else
    results[index++] = "negated_and_false";
     
       if (!(a || b))
     results[index++] = "negated_or_true";
         else
                results[index++] = "negated_or_false";
           
          // Test with comparison results in logical operations
            int x = 10, y = 5, z = 15;
      if ((x > y) && (z > x))
                results[index++] = "comparison_and_true";
     else
              results[index++] = "comparison_and_false";
       
            if ((x < y) || (x < z))
         results[index++] = "comparison_or_true";
  else
       results[index++] = "comparison_or_false";
     
        // Test logical operations with method calls
 if (ReturnsTrue() && ReturnsFalse())
            results[index++] = "method_and_false";
       else
           results[index++] = "method_and_true";
    
      if (ReturnsFalse() || ReturnsTrue())
   results[index++] = "method_or_true";
         else
          results[index++] = "method_or_false";
     
         // Test nested ternary operations
            string ternary1 = a ? (b ? "a_b" : "a_not_b") : (b ? "not_a_b" : "not_a_not_b");
            results[index++] = ternary1;
            
         string ternary2 = (x > y) ? ((z > x) ? "gt_gt" : "gt_le") : ((z < y) ? "le_lt" : "le_ge");
   results[index++] = ternary2;
            
            // Test logical operations with null checks
            string nullStr = null;
          string validStr = "test";
     
         if ((nullStr != null) && (validStr != null))
     results[index++] = "null_check_false";
       else
        results[index++] = "null_check_true";
     
     if ((nullStr == null) || (validStr == null))
     results[index++] = "null_or_true";
else
       results[index++] = "null_or_false";
 
     return results;
}

     // Test logical operations that generate specific CIL branch instructions
public static object[] TestLogicalBranchInstructions()
        {
    object[] results = new object[20];
            int index = 0;
        
            bool t = true, f = false;
            int a = 10, b = 5;
    
            // Test brtrue instruction patterns
      if (t)
                results[index++] = "brtrue_literal";
  else
      results[index++] = "brtrue_literal_false";
     
         if (a > b)
     results[index++] = "brtrue_comparison";
  else
                results[index++] = "brtrue_comparison_false";
            
         // Test brfalse instruction patterns
      if (!f)
 results[index++] = "brfalse_negated";
         else
             results[index++] = "brfalse_negated_false";
      
   if (!(a < b))
            results[index++] = "brfalse_negated_comparison";
            else
   results[index++] = "brfalse_negated_comparison_false";
     
        // Test with method calls that return bool
            bool methodResult = ReturnsTrue();
          if (methodResult)
                results[index++] = "method_result_true";
            else
           results[index++] = "method_result_false";
   
       methodResult = ReturnsFalse();
      if (methodResult)
          results[index++] = "method_result_false_true";
            else
       results[index++] = "method_result_false_false";
    
  // Test logical AND with short-circuit evaluation
            bool andResult1 = t && ReturnsTrue();
            if (andResult1)
     results[index++] = "and_short_circuit_true";
            else
        results[index++] = "and_short_circuit_false";
            
       bool andResult2 = f && ThrowsExceptionBool(); // Should short-circuit
   if (andResult2)
          results[index++] = "and_short_circuit_no_throw_true";
   else
                results[index++] = "and_short_circuit_no_throw_false";
           
      // Test logical OR with short-circuit evaluation
      bool orResult1 = t || ThrowsExceptionBool(); // Should short-circuit
   if (orResult1)
  results[index++] = "or_short_circuit_true";
          else
           results[index++] = "or_short_circuit_false";
   
     bool orResult2 = f || ReturnsTrue();
        if (orResult2)
     results[index++] = "or_no_short_circuit_true";
            else
    results[index++] = "or_no_short_circuit_false";
         
  // Test compound logical expressions
        if ((t && !f) || (f && !t))
    results[index++] = "compound_true";
   else
  results[index++] = "compound_false";
      
            if ((a > b && a > 0) || (b > a && b > 0))
  results[index++] = "compound_comparison_true";
            else
                results[index++] = "compound_comparison_false";
     
            // Test ternary operator branches
     string ternaryResult1 = t ? "ternary_true" : "ternary_false";
     results[index++] = ternaryResult1;
   
string ternaryResult2 = f ? "ternary_false_true" : "ternary_false_false";
            results[index++] = ternaryResult2;
            
  int ternaryInt1 = (a > b) ? a : b;
      results[index++] = $"ternary_int_{ternaryInt1}";
 
   int ternaryInt2 = (a < b) ? a : b;
  results[index++] = $"ternary_int2_{ternaryInt2}";
            
 // Test with nullable bool
            bool? nullableBool1 = true;
     bool? nullableBool2 = false;
    bool? nullableBool3 = null;
            
     if (nullableBool1 == true)
          results[index++] = "nullable_true";
            else
   results[index++] = "nullable_not_true";
                
            if (nullableBool2 == false)
       results[index++] = "nullable_false";
        else
    results[index++] = "nullable_not_false";
          
    if (nullableBool3 == null)
      results[index++] = "nullable_null";
 else
           results[index++] = "nullable_not_null";
        
         return results;
        }

        // Test logical operators with different data types
     public static object[] TestLogicalWithDataTypes()
        {
  object[] results = new object[12];
       int index = 0;
          
            // Test with integers (non-zero is true, zero is false in C#)
     int zero = 0;
 int nonZero = 42;
            
  if (nonZero != 0)
  results[index++] = "int_nonzero_true";
            else
     results[index++] = "int_nonzero_false";
     
          if (zero == 0)
         results[index++] = "int_zero_true";
        else
  results[index++] = "int_zero_false";
     
         // Test with object references
    object obj1 = new object();
          object obj2 = null;
            
            if (obj1 != null)
   results[index++] = "object_notnull_true";
          else
        results[index++] = "object_notnull_false";
        
   if (obj2 == null)
      results[index++] = "object_null_true";
            else
      results[index++] = "object_null_false";
     
       // Test with strings
            string str1 = "test";
    string str2 = "";
            string str3 = null;
 
            if (!string.IsNullOrEmpty(str1))
                results[index++] = "string_notempty_true";
    else
      results[index++] = "string_notempty_false";
                
    if (string.IsNullOrEmpty(str2))
   results[index++] = "string_empty_true";
            else
   results[index++] = "string_empty_false";
                
     if (string.IsNullOrEmpty(str3))
         results[index++] = "string_null_true";
            else
         results[index++] = "string_null_false";
     
        // Test with enums
  System.StringComparison comp = System.StringComparison.Ordinal;
    if (comp == System.StringComparison.Ordinal)
        results[index++] = "enum_equal_true";
            else
        results[index++] = "enum_equal_false";
      
            if (comp != System.StringComparison.OrdinalIgnoreCase)
        results[index++] = "enum_notequal_true";
            else
              results[index++] = "enum_notequal_false";
              
         // Test with floating point comparisons
       double d1 = 1.0, d2 = 1.0, d3 = 1.1;
            
          if (d1 == d2)
                results[index++] = "double_equal_true";
     else
     results[index++] = "double_equal_false";
                
   if (d1 != d3)
       results[index++] = "double_notequal_true";
        else
  results[index++] = "double_notequal_false";
    
            if (System.Math.Abs(d1 - d2) < 0.001)
         results[index++] = "double_approximate_true";
  else
     results[index++] = "double_approximate_false";
        
        return results;
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
     
        private static bool ThrowsExceptionBool()
        {
        throw new System.Exception("Should not be called in short-circuit");
        }
    }
}