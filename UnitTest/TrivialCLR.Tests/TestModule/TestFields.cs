using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestModule
{
    public class TestFields
    {
        private class TestInstance
        {
            public int a = 10;
        }

        private class TestInstanceSub : TestInstance
        {
        }

        static int a = 10;

        public static bool TestStaticField1()
        {
            return a == 10;
        }

        public static bool TestInstanceField1()
        {
            TestInstance instance = new TestInstance();

            return instance.a == 10;
        }

        public static bool TestInstanceField2()
        {
            TestInstance instance = new TestInstanceSub();

            return instance.a == 10;
        }
    }
}
