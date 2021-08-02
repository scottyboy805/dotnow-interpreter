using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestModule
{
    public class TestObject
    {
        private class TestInstance
        {
            public override string ToString()
            {
                return "Test String";
            }

            public override int GetHashCode()
            {
                return 12345;
            }

            public override bool Equals(object obj)
            {
                return true;
            }
        }

        private class TestDefaultInstance
        {
            public override bool Equals(object obj)
            {
                return false;
            }
        }

        public static string TestInstanceToString1()
        {
            TestInstance instance = new TestInstance();
            return instance.ToString();
        }

        public static string TestInstanceToString2()
        {
            TestDefaultInstance instance = new TestDefaultInstance();
            return instance.ToString();
        }

        public static int TestInstanceGetHashCode1()
        {
            TestInstance instance = new TestInstance();
            return instance.GetHashCode();
        }

        public static bool TestInstanceEquals1()
        {
            TestInstance instance = new TestInstance();
            return instance.Equals(null);
        }

        public static bool TestInstanceEquals2()
        {
            TestDefaultInstance instance = new TestDefaultInstance();
            return instance.Equals(null);
        }
    }
}
