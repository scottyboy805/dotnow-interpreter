
using System;
using TestInterop;

namespace TestModule
{
    public class TestFunc
    {
        public static int TestFuncPrimitive_T0()
        {
            Func<int> call = () => { return 10; };
            return call();
        }

        public static string TestFuncString_T0()
        {
            Func<string> call = () => { return "Hello World"; };
            return call();
        }

        public static object TestFuncBoxedObject_T0()
        {
            Func<object> call = () => { return 10; };
            return call();
        }

        public static object TestActionInteropObject_T0()
        {
            Func<InteropFunc.TestClass> call = () => { return new InteropFunc.TestClass { val = 10 }; };
            return call().val;
        }

        public static object TestActionInteropStruct_T0()
        {
            Func<InteropFunc.TestStruct> call = () => { return new InteropFunc.TestStruct { val = 10 }; };
            return call().val;
        }


        public static int TestFuncPrimitivePrimitive_T1()
        {
            Func<int, int> call = (int a) => { return a + 5; };
            return call(5);
        }

        public static string TestFuncStringString_T1()
        {
            Func<string, string> call = (string a) => { return "Hello" + a; };
            return call(" World");
        }

        public static string TestFuncPrimitiveString_T1()
        {
            Func<int, string> call = (int a) => { return a.ToString(); };
            return call(1234);
        }

        public static object TestFuncInteropPrimitiveObject_T1()
        {
            Func<int, InteropFunc.TestClass> call = (int a) => { return new InteropFunc.TestClass { val = a }; };
            return call(10).val;
        }

        public static object TestFuncInteropPrimitiveStruct_T1()
        {
            Func<int, InteropFunc.TestStruct> call = (int a) => { return new InteropFunc.TestStruct { val = a }; };
            return call(10).val;
        }
    }
}
