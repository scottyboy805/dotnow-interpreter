using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrivialCLR.Reflection;

namespace TrivialCLR.Tests
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
