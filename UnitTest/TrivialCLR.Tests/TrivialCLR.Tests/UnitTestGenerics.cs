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
    public class UnitTestGenerics
    {
        [TestMethod]
        public void TestGenerics1()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestGenerics", "TestGenerics1");

            Assert.AreEqual(10, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestGenerics2()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestGenerics", "TestGenerics2");

            Assert.AreEqual(10, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestGenerics3()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestGenerics", "TestGenerics3");

            Assert.AreEqual("Hello World", method.Invoke(null, null));
        }

        [TestMethod]
        public void TestGenerics4()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestGenerics", "TestGenerics4");

            Assert.AreEqual(10, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestGenerics5()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestGenerics", "TestGenerics5");

            Assert.AreEqual(10, method.Invoke(null, new object[] { true }));
            Assert.AreEqual(5f, method.Invoke(null, new object[] { false }));
        }

        [TestMethod]
        public void TestGenericsMethods1()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestGenerics", "TestGenericMethods1");

            Assert.AreEqual(10, method.Invoke(null, null));
        }
    }
}
