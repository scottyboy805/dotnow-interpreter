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
    public class UnitTestFields
    {
        [TestMethod]
        public void TestStaticField1()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestFields", "TestStaticField1");

            Assert.AreEqual(true, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestInstanceField1()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestFields", "TestInstanceField1");

            Assert.AreEqual(true, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestInstanceField2()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestFields", "TestInstanceField2");

            Assert.AreEqual(true, method.Invoke(null, null));
        }
    }
}
