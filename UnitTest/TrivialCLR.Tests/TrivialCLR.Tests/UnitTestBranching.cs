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
    public class UnitTestBranching
    {
        [TestMethod]
        public void TestGoto()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestBranching", "TestGoto");

            Assert.AreEqual(false, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestBranch1()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestBranching", "TestBranch1");

            Assert.AreEqual(true, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestBranch2()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestBranching", "TestBranch2");

            Assert.AreEqual(true, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestBranch3()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestBranching", "TestBranch3");

            Assert.AreEqual(true, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestBranch4()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestBranching", "TestBranch4");

            Assert.AreEqual(true, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestBranch5()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestBranching", "TestBranch5");

            Assert.AreEqual(true, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestBranch6()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestBranching", "TestBranch6");

            Assert.AreEqual(true, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestBranch7()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestBranching", "TestBranch7");

            Assert.AreEqual(true, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestBranch8()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestBranching", "TestBranch8");

            Assert.AreEqual(true, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestBranch9()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestBranching", "TestBranch9");

            Assert.AreEqual(true, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestSwitch1()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestBranching", "TestSwitch1");

            Assert.AreEqual(10, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestSwitch2()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestBranching", "TestSwitch2");

            Assert.AreEqual(true, method.Invoke(null, null));
        }
    }
}
