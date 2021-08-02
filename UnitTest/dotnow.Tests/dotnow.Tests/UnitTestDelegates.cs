using Microsoft.VisualStudio.TestTools.UnitTesting;
using dotnow.Reflection;

namespace dotnow.Tests
{
    [TestClass]
    public class UnitTestDelegates
    {
        [TestMethod]
        public void TestDelegate1()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestDelegates", "TestDelegate1");

            Assert.AreEqual(10, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestDelegate2()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestDelegates", "TestDelegate2");

            Assert.AreEqual(10, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestDelegate3()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestDelegates", "TestDelegate3");

            Assert.AreEqual(5f, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestReturnDelegate1()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestDelegates", "TestReturnDelegate1");
            
            Assert.AreEqual(10, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestReturnDelegate2()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestDelegates", "TestReturnDelegate2");

            Assert.AreEqual(10, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestCustomDelegate1()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestDelegates", "TestCustomDelegate1");

            Assert.AreEqual(10, method.Invoke(null, null));
        }
    }
}
