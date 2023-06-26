using dotnow.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace dotnow.Tests
{
    [TestClass]
    public class UnitTestAction
    {
        [TestMethod]
        public void TestActionNoArguments()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestAction", "TestActionNoArguments");

            Assert.AreEqual(10, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestActionPrimitive_T0()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestAction", "TestActionPrimitive_T0");

            Assert.AreEqual(10, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestActionString_T0()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestAction", "TestActionString_T0");

            Assert.AreEqual("Hello World", method.Invoke(null, null));
        }

        [TestMethod]
        public void TestActionBoxedObject_T0()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestAction", "TestActionBoxedObject_T0");

            Assert.AreEqual(10, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestActionObject_T0()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestAction", "TestActionObject_T0");

            Assert.AreEqual(10, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestActionStruct_T0()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestAction", "TestActionStruct_T0");

            Assert.AreEqual(10, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestActionInteropObject_T0()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestAction", "TestActionInteropObject_T0");

            Assert.AreEqual(10, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestActionInteropStruct_T0()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestAction", "TestActionInteropStruct_T0");

            Assert.AreEqual(10, method.Invoke(null, null));
        }



        [TestMethod]
        public void Interop_TestActionNoArguments()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestAction", "Interop_TestActionNoArguments");

            Assert.AreEqual(10, method.Invoke(null, null));
        }

        [TestMethod]
        public void Interop_TestActionPrimitive_T0()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestAction", "Interop_TestActionPrimitive_T0");

            Assert.AreEqual(10, method.Invoke(null, null));
        }

        [TestMethod]
        public void Interop_TestActionString_T0()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestAction", "Interop_TestActionString_T0");

            Assert.AreEqual("Hello World", method.Invoke(null, null));
        }

        [TestMethod]
        public void Interop_TestActionBoxedObject_T0()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestAction", "Interop_TestActionBoxedObject_T0");

            Assert.AreEqual(10, method.Invoke(null, null));
        }

        [TestMethod]
        public void Interop_TestActionInteropObject_T0()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestAction", "Interop_TestActionInteropObject_T0");

            Assert.AreEqual(10, method.Invoke(null, null));
        }

        [TestMethod]
        public void Interop_TestActionInteropStruct_T0()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestAction", "Interop_TestActionInteropStruct_T0");

            Assert.AreEqual(10, method.Invoke(null, null));
        }
    }
}
