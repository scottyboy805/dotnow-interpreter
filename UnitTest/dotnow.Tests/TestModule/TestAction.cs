using System;
using System.Collections.Generic;
using TestInterop;

namespace TestModule
{
    public class TestAction
    {
        public class TestClass
        {
            public int val;
        }

        public struct TestStruct
        {
            public int val;
        }

        #region InterpretedAction
        public static int TestActionNoArguments()
        {
            int i = 0;
            Action call = () => { i = 10; };
            
            call();
            return i;
        }

        public static int TestActionPrimitive_T0()
        {
            int i = 5;
            Action<int> call = (int a) => { i += a; };

            call(5);
            return i;
        }

        public static string TestActionString_T0()
        {
            string i = "Hello";
            Action<string> call = (string a) => { i += a; };

            call(" World");
            return i;
        }

        public static object TestActionBoxedObject_T0()
        {
            object i = 5;
            Action<object> call = (object a) => { i = a; };

            call(10);
            return i;
        }

        public static object TestActionObject_T0()
        {
            TestClass i =  new TestClass { val = 5 };
            Action<TestClass> call = (TestClass a) => { a.val = 10; };

            call(i);
            return i.val;
        }

        public static object TestActionStruct_T0()
        {
            TestStruct i = new TestStruct { val = 5 };
            Action<TestStruct> call = (TestStruct a) => { a.val = 10; };

            call(i);
            return i.val;
        }

        public static object TestActionInteropObject_T0()
        {
            int result = 0;
            Tuple<int, int> i = new Tuple<int, int>(5, 5);
            Action<Tuple<int, int>> call = (Tuple<int, int> a) => { result = a.Item1 + a.Item2; };
            
            call(i);
            return result;
        }

        public static object TestActionInteropStruct_T0()
        {
            int result = 0;
            KeyValuePair<int, int> i = new KeyValuePair<int, int>(5, 5);
            Action<KeyValuePair<int, int>> call = (KeyValuePair<int, int> a) => { result = a.Key + a.Value; };
            
            call(i);
            return result;
        }
        #endregion

        #region InteropAction
        public static int Interop_TestActionNoArguments()
        {
            Action call = InteropAction.ActionNoArguments;

            call();
            return (int)InteropAction.value;
        }

        public static int Interop_TestActionPrimitive_T0()
        {
            Action<int> call = InteropAction.ActionPrimitive;

            call(10);
            return (int)InteropAction.value;
        }

        public static string Interop_TestActionString_T0()
        {
            Action<string> call = InteropAction.ActionString;

            call("Hello World");
            return (string)InteropAction.value;
        }

        public static object Interop_TestActionBoxedObject_T0()
        {
            Action<object> call = InteropAction.ActionBoxed;

            call(10);
            return (int)InteropAction.value;
        }

        public static object Interop_TestActionInteropObject_T0()
        {
            Tuple<int, int> i = new Tuple<int, int>(5, 5);
            Action<Tuple<int, int>> call = InteropAction.ActionInteropObject;

            call(i);
            return (int)InteropAction.value;
        }

        public static object Interop_TestActionInteropStruct_T0()
        {
            KeyValuePair<int, int> i = new KeyValuePair<int, int>(5, 5);
            Action<KeyValuePair<int, int>> call = InteropAction.ActionInteropStruct;

            call(i);
            return (int)InteropAction.value;
        }
        #endregion
    }
}
