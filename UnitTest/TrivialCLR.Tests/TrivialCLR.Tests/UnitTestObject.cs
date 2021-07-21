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
    public class UnitTestObject
    {
        [TestMethod]
        public void TestObjectToString1()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestObject", "TestInstanceToString1");

            Assert.AreEqual("Test String", method.Invoke(null, null));
        }

        [TestMethod]
        public void TestObjectToString2()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestObject", "TestInstanceToString2");

            Assert.AreEqual("TestModule.TestObject/TestDefaultInstance", method.Invoke(null, null));
        }

        [TestMethod]
        public void TestObjectGetHashCode1()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestObject", "TestInstanceGetHashCode1");

            Assert.AreEqual(12345, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestObjectEquals1()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestObject", "TestInstanceEquals1");

            Assert.AreEqual(true, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestObjectEquals2()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestObject", "TestInstanceEquals2");

            Assert.AreEqual(false, method.Invoke(null, null));
        }
    }
}
