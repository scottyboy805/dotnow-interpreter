using Microsoft.VisualStudio.TestTools.UnitTesting;
using dotnow.Reflection;

namespace dotnow.Tests
{
    [TestClass]
    public class UnitTestTypes
    {
        [TestMethod]
        public void TestTypeof1()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestTypes", "TestTypeof1");

            Assert.AreEqual(typeof(bool), method.Invoke(null, null));
        }

        [TestMethod]
        public void TestTypeof2()
        {
            //AppDomain domain;

            //CLRMethod method = TestUtils.LoadTestMethod("TestTypes", "TestTypeof2", out domain);

            //Assert.AreEqual(domain.GetRuntimeType("TestTypes").GetNestedType("TestType", System.Reflection.BindingFlags.NonPublic), method.Invoke(null, null));
        }

        [TestMethod]
        public void TestAs1()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestTypes", "TestAs1");

            Assert.IsNotNull(method.Invoke(null, null));
        }

        [TestMethod]
        public void TestAs2()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestTypes", "TestAs2");

            Assert.IsNotNull(method.Invoke(null, null));
        }
    }
}
