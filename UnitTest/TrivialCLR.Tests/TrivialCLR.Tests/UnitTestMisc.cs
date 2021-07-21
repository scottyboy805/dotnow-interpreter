using Microsoft.VisualStudio.TestTools.UnitTesting;
using TrivialCLR.Reflection;

namespace TrivialCLR.Tests
{
    [TestClass]
    public class UnitTestMisc
    {
        [TestMethod]
        public void TestNameof()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestMisc", "TestNameof");

            Assert.AreEqual("variable", method.Invoke(null, null));
        }

        [TestMethod]
        public void TestSizeof()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestMisc", "TestSizeof");

            Assert.AreEqual(4, method.Invoke(null, null));
        }
    }
}
