using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestModule
{
    public class TestTypes
    {
        public class TestType
        {
        }

        public interface ITestType
        {
        }

        public class TestTypeSub : TestType, ITestType
        {
        }

        public static object TestTypeof1()
        {
            return typeof(bool);
        }

        public static object TestTypeof2()
        {
            return typeof(TestType);
        }

        public static TestType TestAs1()
        {
            TestType baseType = new TestTypeSub();

            TestTypeSub subType = baseType as TestTypeSub;

            return subType;
        }

        public static TestType TestAs2()
        {
            ITestType baseType = new TestTypeSub();

            TestTypeSub subType = baseType as TestTypeSub;

            return subType;
        }
    }
}
