using Microsoft.VisualStudio.TestTools.UnitTesting;
using dotnow.Reflection;

namespace dotnow.Tests.Variables
{
    [TestClass]
    public class UnitTestLocalDeclarations
    {
        [TestMethod]
        public void TestSByteLocalDeclaration()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestLocalDeclarations", "TestSByteLocal");

            Assert.AreEqual((sbyte)10, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestShortLocalDeclaration()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestLocalDeclarations", "TestShortLocal");

            Assert.AreEqual((short)20, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestIntLocalDeclaration()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestLocalDeclarations", "TestIntLocal");

            Assert.AreEqual((int)30, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestLongLocalDeclaration()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestLocalDeclarations", "TestLongLocal");

            Assert.AreEqual((long)40, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestByteLocalDeclaration()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestLocalDeclarations", "TestByteLocal");

            Assert.AreEqual((byte)50, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestUshortLocalDeclaration()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestLocalDeclarations", "TestUshortLocal");

            Assert.AreEqual((ushort)60, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestUintLocalDeclaration()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestLocalDeclarations", "TestUintLocal");

            Assert.AreEqual((uint)70, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestUlongLocalDeclaration()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestLocalDeclarations", "TestUlongLocal");

            Assert.AreEqual((ulong)80, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestFloatLocalDeclaration()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestLocalDeclarations", "TestFloatLocal");

            Assert.AreEqual((float)90.25f, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestDoubleLocalDeclaration()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestLocalDeclarations", "TestDoubleLocal");

            Assert.AreEqual((double)100.25d, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestStringLocalDeclaration()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestLocalDeclarations", "TestStringLocal");

            Assert.AreEqual("Hello World", method.Invoke(null, null));
        }
    }
}
