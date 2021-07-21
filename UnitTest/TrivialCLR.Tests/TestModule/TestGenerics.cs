using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestModule
{
    public class TestGenerics
    {
        public class TestInstance<T>
        {
            public T value;

            public T GetValue()
            {
                return value;
            }
        }

        public class TestInstance2<T, J>
        {
            public T tValue;
            public J jValue;
        }

        public class TestType
        {
            public int a = 0;
        }

        public static int TestGenerics1()
        {
            List<int> testList = new List<int>();

            testList.Add(10);

            return testList[0];
        }

        public static int TestGenerics2()
        {
            TestInstance<int> test = new TestInstance<int>();

            test.value = 10;

            return test.value;
        }

        public static string TestGenerics3()
        {
            TestInstance<string> test = new TestInstance<string>();

            test.value = "Hello World";

            return test.value;
        }

        public static int TestGenerics4()
        {
            TestInstance<TestType> test = new TestInstance<TestType>();

            test.value = new TestType();
            test.value.a = 10;

            return test.value.a;
        }

        public static object TestGenerics5(bool a)
        {
            TestInstance2<int, float> test = new TestInstance2<int, float>();

            test.tValue = 10;
            test.jValue = 5f;

            if (a == true)
                return test.tValue;

            return test.jValue;
        }

        public static int TestGenericMethods1()
        {
            TestInstance<int> test = new TestInstance<int>();

            test.value = 10;

            return test.GetValue();
        }
    }
}
