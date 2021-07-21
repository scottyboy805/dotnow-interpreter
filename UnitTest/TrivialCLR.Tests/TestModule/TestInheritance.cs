
namespace TestModule
{
    public class TestInheritance
    {
        public abstract class TestInstance
        {
            public virtual string GetVirtualString()
            {
                return "Failure";
            }

            public abstract string GetAbstractString();
        }

        public class TestInstanceSub : TestInstance
        {
            public override string GetVirtualString()
            {
                return "Success";
            }

            public override string GetAbstractString()
            {
                return "Success";
            }
        }

        public class TestInstanceSubMulti : TestInstanceSub
        {
            public override string GetVirtualString()
            {
                return "Success Sub";
            }

            public override string GetAbstractString()
            {
                return "Success Sub";
            }
        }

        public static string TestAbstractMethod1()
        {
            TestInstance instance = new TestInstanceSub();

            return instance.GetAbstractString();
        }

        public static string TestAbstractMethod2()
        {
            TestInstance instance = new TestInstanceSubMulti();

            return instance.GetAbstractString();
        }

        public static string TestVirtualMethod1()
        {
            TestInstance instance = new TestInstanceSub();

            return instance.GetVirtualString();
        }

        public static string TestVirtualMethod2()
        {
            TestInstance instance = new TestInstanceSubMulti();

            return instance.GetVirtualString();
        }

        public static void TestNullReferenceCall()
        {
            TestInstance instance = null;

            instance.GetVirtualString();
        }
    }
}
