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
    public class UnitTestArray
    {
        [TestMethod]
        public void TestArrayAllocation1()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestArray", "TestArrayAllocation1");

            Assert.AreEqual(10, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestArrayAllocation2()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestArray", "TestArrayAllocation2");

            Assert.AreEqual(5, method.Invoke(null, null));
        }
    }
}
