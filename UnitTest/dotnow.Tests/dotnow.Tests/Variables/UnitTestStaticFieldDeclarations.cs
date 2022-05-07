using Microsoft.VisualStudio.TestTools.UnitTesting;
using dotnow.Reflection;

namespace dotnow.Tests.Variables
{
    [TestClass]
    public class UnitTestStaticFieldDeclarations
    {
        [TestMethod]
        public void TestSByteInstanceDeclaration()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestFieldDeclarations", "TestSByteStatic");

            Assert.AreEqual((sbyte)10, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestShortInstanceDeclaration()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestFieldDeclarations", "TestShortStatic");

            Assert.AreEqual((short)20, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestIntInstanceDeclaration()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestFieldDeclarations", "TestIntStatic");

            Assert.AreEqual((int)30, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestLongInstanceDeclaration()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestFieldDeclarations", "TestLongStatic");

            Assert.AreEqual((long)40, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestByteInstanceDeclaration()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestFieldDeclarations", "TestByteStatic");

            Assert.AreEqual((byte)50, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestUshortInstanceDeclaration()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestFieldDeclarations", "TestUshortStatic");

            Assert.AreEqual((ushort)60, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestUintInstanceDeclaration()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestFieldDeclarations", "TestUintStatic");

            Assert.AreEqual((uint)70, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestUlongInstanceDeclaration()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestFieldDeclarations", "TestUlongStatic");

            Assert.AreEqual((ulong)80, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestFloatInstanceDeclaration()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestFieldDeclarations", "TestFloatStatic");

            Assert.AreEqual((float)90.25f, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestDoubleInstanceDeclaration()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestFieldDeclarations", "TestDoubleStatic");

            Assert.AreEqual((double)100.25d, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestStringInstanceDeclaration()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestFieldDeclarations", "TestStringStatic");

            Assert.AreEqual("Hello World", method.Invoke(null, null));
        }
    }
}
