using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using dotnow.Reflection;

namespace dotnow.Tests
{
    [TestClass]
    public class UnitTestExceptions
    {
        [TestMethod]
        public void TestThrowException()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestExceptions", "TestThrowException");

            Assert.ThrowsException<Exception>(() => method.Invoke(null, null));
        }
    }
}
