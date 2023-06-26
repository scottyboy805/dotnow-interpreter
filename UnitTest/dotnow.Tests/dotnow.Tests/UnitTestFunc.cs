using dotnow.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace dotnow.Tests
{
    [TestClass]
    public class UnitTestFunc
    {
        [TestMethod]
        public void TestFuncPrimitive_T0()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestFunc", "TestFuncPrimitive_T0");

            Assert.AreEqual(10, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestFuncString_T0()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestFunc", "TestFuncString_T0");

            Assert.AreEqual("Hello World", method.Invoke(null, null));
        }

        [TestMethod]
        public void TestFuncBoxedObject_T0()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestFunc", "TestFuncBoxedObject_T0");

            Assert.AreEqual(10, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestFuncInteropObject_T0()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestFunc", "TestActionInteropObject_T0");

            Assert.AreEqual(10, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestFuncInteropStruct_T0()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestFunc", "TestActionInteropStruct_T0");

            Assert.AreEqual(10, method.Invoke(null, null));
        }



        [TestMethod]
        public void TestFuncPrimitivePrimitive_T1()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestFunc", "TestFuncPrimitivePrimitive_T1");

            Assert.AreEqual(10, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestFuncStringString_T1()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestFunc", "TestFuncStringString_T1");

            Assert.AreEqual("Hello World", method.Invoke(null, null));
        }

        [TestMethod]
        public void TestFuncPrimitiveString_T1()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestFunc", "TestFuncPrimitiveString_T1");

            Assert.AreEqual("1234", method.Invoke(null, null));
        }

        [TestMethod]
        public void TestFuncInteropPrimitiveObject_T1()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestFunc", "TestFuncInteropPrimitiveObject_T1");

            Assert.AreEqual(10, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestFuncInteropPrimitiveStruct_T1()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestFunc", "TestFuncInteropPrimitiveStruct_T1");

            Assert.AreEqual(10, method.Invoke(null, null));
        }
    }
}
