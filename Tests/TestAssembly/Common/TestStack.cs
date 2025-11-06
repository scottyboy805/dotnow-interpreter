namespace TestAssembly
{
    public static class TestStack
    {
   // Local variable tests
 public static object[] TestLocalVariables()
     {
  int a = 10;
     int b = 20;
       int c = a + b;
   
    a = 30;
     int d = a + b + c;
      
  return new object[] { a, b, c, d };
   }

      // Parameter passing tests
  public static object[] TestParameters()
       {
  int result1 = AddTwo(5, 3);
      int result2 = MultiplyTwo(4, 6);
       string result3 = ConcatStrings("Hello", " World");
         
        return new object[] { result1, result2, result3 };
   }

  private static int AddTwo(int x, int y)
    {
   return x + y;
     }

     private static int MultiplyTwo(int x, int y)
      {
 return x * y;
        }

 private static string ConcatStrings(string a, string b)
   {
      return a + b;
  }

 // Return value tests
     public static object[] TestReturnValues()
    {
   int intResult = GetInteger();
      string stringResult = GetString();
    bool boolResult = GetBoolean();
  object nullResult = GetNull();
       
   return new object[] { intResult, stringResult, boolResult, nullResult };
   }

     private static int GetInteger()
    {
   return 42;
 }

     private static string GetString()
        {
    return "test";
    }

     private static bool GetBoolean()
    {
  return true;
  }

       private static object GetNull()
 {
      return null;
  }

// Multiple local variables of same type
      public static object[] TestMultipleLocals()
    {
       int var1 = 1;
int var2 = 2;
int var3 = 3;
       int var4 = 4;
   int var5 = 5;
   
        int sum = var1 + var2 + var3 + var4 + var5;
       int product = var1 * var2 * var3;
      
     return new object[] { sum, product };
        }

     // Mixed type local variables
 public static object[] TestMixedTypeLocals()
     {
   int intVar = 10;
      float floatVar = 3.14f;
   string stringVar = "test";
        bool boolVar = true;
   object objVar = new object();
 
  return new object[] { intVar, floatVar, stringVar, boolVar, objVar != null };
        }

    // Argument evaluation order test
     public static object[] TestArgumentOrder()
   {
       int counter = 0;
   int result = TestOrderMethod(++counter, ++counter, ++counter);
    
      return new object[] { result, counter };
    }

        private static int TestOrderMethod(int a, int b, int c)
    {
    return a * 100 + b * 10 + c;
      }

    // Nested method calls
     public static object[] TestNestedCalls()
     {
   int result1 = Add(Multiply(2, 3), Multiply(4, 5));
        int result2 = Multiply(Add(1, 2), Add(3, 4));
        
 return new object[] { result1, result2 };
  }

       private static int Add(int a, int b)
 {
     return a + b;
        }
 
     private static int Multiply(int a, int b)
   {
    return a * b;
       }
 }
}