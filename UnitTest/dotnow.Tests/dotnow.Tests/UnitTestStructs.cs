using Microsoft.VisualStudio.TestTools.UnitTesting;
using dotnow.Reflection;

namespace dotnow.Tests
{
    [TestClass]
    public class UnitTestStructs
    {
        [TestMethod]
        public void TestSetStructField()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestStructs", "TestSetStructField");

            Assert.AreEqual(1234, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestGetStructField()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestStructs", "TestGetStructField");

            Assert.AreEqual(1234, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestPassStruct()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestStructs", "TestPassStruct");

            Assert.AreEqual(5, method.Invoke(null, null));
        }
    }
}
