using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestModule
{
    public class TestDelegates
    {
        public delegate void ExampleDelegate(int value);

        static int a = 0;
        static float b = 0;

        public static void DelegateMethod()
        {
            a = 10;
        }

        public static void DelegateMethod2(int value)
        {
            a = value;
        }

        public static void DelegateMethod3(float fVal, int iVal)
        {
            a = iVal;
            b = fVal;
        }

        public static int ReturnDelegateMethod()
        {
            return 10;
        }

        public static int ReturnDelegateMethod2(int value)
        {
            return value;
        }

        public static int TestDelegate1()
        {
            Action call = DelegateMethod;
            
            call();

            return a;
        }

        public static int TestDelegate2()
        {
            Action<int> call = DelegateMethod2;

            call(10);

            return a;
        }

        public static float TestDelegate3()
        {
            Action<float, int> call = DelegateMethod3;

            call(5f, 10);

            return b;
        }

        public static int TestReturnDelegate1()
        {
            Func<int> call = ReturnDelegateMethod;

            return call();
        }

        public static int TestReturnDelegate2()
        {
            Func<int, int> call = ReturnDelegateMethod2;

            return call(10);
        }

        public static int TestCustomDelegate1()
        {
            ExampleDelegate call = DelegateMethod2;

            call(10);

            return a;
        }
    }
}
