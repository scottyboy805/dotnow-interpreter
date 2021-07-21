using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TrivialCLR.Reflection;
using TrivialCLR.Runtime;

namespace TrivialCLR.Tests
{
    [TestClass]
    public class UnitTestVariables
    {
        [TestMethod]
        public void TestValArgument()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestVariables", "TestValArgument");

            Assert.AreEqual(5, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestRefArgument()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestVariables", "TestRefArgument");

            Assert.AreEqual(10, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestRefArgumentMultiple()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestVariables", "TestRefArgumentMultiple");

            Assert.AreEqual(10, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestRefArgumentExternal1()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestVariables", "TestRefArgumentExternal");

            int testValue = 6;

            Assert.ThrowsException<TargetInvocationException>(() => method.Invoke(null, new object[] { testValue }));
        }

        [TestMethod]
        public void TestRefArgumentExternal2()
        {
            //CLRMethod method = TestUtils.LoadTestMethod("TestVariables", "TestRefArgumentExternal");

            //int testValue = 6;

            //ByRef byRef = new ByRef(testValue);

            //method.Invoke(null, new object[] { byRef });

            //Assert.AreEqual(10, byRef.value);
        }

        [TestMethod]
        public void TestRefField()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestVariables", "TestRefField");

            Assert.AreEqual(10, method.Invoke(null, null));
        }
    }
}
