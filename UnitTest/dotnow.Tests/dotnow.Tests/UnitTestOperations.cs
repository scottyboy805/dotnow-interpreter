using Microsoft.VisualStudio.TestTools.UnitTesting;
using dotnow.Reflection;

namespace dotnow.Tests
{
    [TestClass]
    public class UnitTestOperations
    {
        #region Add
        [TestMethod]
        public void TestAddSByte()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestOperations", "TestAdditionSByte");

            Assert.AreEqual(5, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestAddInt16()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestOperations", "TestAdditionInt16");

            Assert.AreEqual(5, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestAddInt32()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestOperations", "TestAdditionInt32");

            Assert.AreEqual(5, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestAddInt64()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestOperations", "TestAdditionInt64");

            Assert.AreEqual(5, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestAddByte()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestOperations", "TestAdditionByte");

            Assert.AreEqual(5, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestAddUInt16()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestOperations", "TestAdditionUInt16");

            Assert.AreEqual(5, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestAddUInt32()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestOperations", "TestAdditionUInt32");

            Assert.AreEqual(5, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestAddUInt64()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestOperations", "TestAdditionUInt64");

            Assert.AreEqual(5, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestAddSingle()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestOperations", "TestAdditionSingle");

            Assert.AreEqual(5f, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestAddDouble()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestOperations", "TestAdditionDouble");

            Assert.AreEqual(5.0, method.Invoke(null, null));
        }
        #endregion

        [TestMethod]
        public void TestSubtract()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestOperations", "TestSubtraction");

            Assert.AreEqual(2, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestMultiplication()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestOperations", "TestMultiplication");

            Assert.AreEqual(6, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestDivide()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestOperations", "TestDivide");

            Assert.AreEqual(2, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestBitshiftLeft()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestOperations", "TestBitshiftLeft");

            Assert.AreEqual(4 << 1, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestBitshiftRight()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestOperations", "TestBitshiftRight");

            Assert.AreEqual(4 >> 1, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestNot()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestOperations", "TestNot");

            Assert.AreEqual(true, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestLessThan1()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestOperations", "TestLessThan1");

            Assert.AreEqual(false, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestLessThan2()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestOperations", "TestLessThan2");

            Assert.AreEqual(true, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestLessThanEqual1()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestOperations", "TestLessThanEqual1");

            Assert.AreEqual(false, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestLessThanEqual2()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestOperations", "TestLessThanEqual2");

            Assert.AreEqual(true, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestLessThanEqual3()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestOperations", "TestLessThanEqual3");

            Assert.AreEqual(true, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestGreaterThan1()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestOperations", "TestGreaterThan1");

            Assert.AreEqual(true, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestGreaterThan2()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestOperations", "TestGreaterThan2");

            Assert.AreEqual(false, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestGreaterThanEqual1()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestOperations", "TestGreaterThanEqual1");

            Assert.AreEqual(true, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestGreaterThanEqual2()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestOperations", "TestGreaterThanEqual2");

            Assert.AreEqual(false, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestGreaterThanEqual3()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestOperations", "TestGreaterThanEqual3");

            Assert.AreEqual(true, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestEqual1()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestOperations", "TestEqual1");

            Assert.AreEqual(true, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestEqual2()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestOperations", "TestEqual2");

            Assert.AreEqual(true, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestEqual3()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestOperations", "TestEqual3");

            Assert.AreEqual(true, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestNotEqual1()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestOperations", "TestNotEqual1");

            Assert.AreEqual(false, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestNotEqual2()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestOperations", "TestNotEqual2");

            Assert.AreEqual(false, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestNotEqual3()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestOperations", "TestNotEqual3");

            Assert.AreEqual(false, method.Invoke(null, null));
        }
    }
}
