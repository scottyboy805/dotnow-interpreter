using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using dotnow.Reflection;

namespace dotnow.Tests
{
    [TestClass]
    public class UnitTestInheritance
    {
        [TestMethod]
        public void TestAbstractMethod1()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestInheritance", "TestAbstractMethod1");

            Assert.AreEqual("Success", method.Invoke(null, null));
        }

        [TestMethod]
        public void TestAbstractMethod2()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestInheritance", "TestAbstractMethod2");

            Assert.AreEqual("Success Sub", method.Invoke(null, null));
        }

        [TestMethod]
        public void TestVirtualMethod1()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestInheritance", "TestVirtualMethod1");

            Assert.AreEqual("Success", method.Invoke(null, null));
        }

        [TestMethod]
        public void TestVirtualMethod2()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestInheritance", "TestVirtualMethod2");

            Assert.AreEqual("Success Sub", method.Invoke(null, null));
        }

        [TestMethod]
        public void TestNullReferenceCall()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestInheritance", "TestNullReferenceCall");

            Assert.ThrowsException<NullReferenceException>(() => method.Invoke(null, null));
        }
    }
}
