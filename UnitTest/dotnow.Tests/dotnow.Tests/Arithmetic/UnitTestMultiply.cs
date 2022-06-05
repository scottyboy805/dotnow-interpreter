using dotnow.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace dotnow.Tests.Arithmetic
{
    [TestClass]
    public class UnitTestMultiply
    {
        #region SameType
        [TestMethod]
        public void TestSByteMultiplySByte()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestMultiply", "TestSByteMultiplySByte");

            // Sbyte * Sbyte = Int32
            Assert.AreEqual((int)200, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestShortMultiplyShort()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestMultiply", "TestShortMultiplyShort");

            // Short * Short = Int32
            Assert.AreEqual((int)200, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestIntMultiplyInt()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestMultiply", "TestIntMultiplyInt");

            // Int * Int = Int32
            Assert.AreEqual((int)200, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestLongMultiplyLong()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestMultiply", "TestLongMultiplyLong");

            // Long * Long = Int64
            Assert.AreEqual((long)200, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestByteMultiplyByte()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestMultiply", "TestByteMultiplyByte");

            // Byte * Byte = Int32
            Assert.AreEqual((int)200, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestUshortMultiplyUshort()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestMultiply", "TestUshortMultiplyUshort");

            // Short * Short = Int32
            Assert.AreEqual((int)200, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestUintMultiplyUint()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestMultiply", "TestUintMultiplyUint");

            // Uint * Uint = Uint
            Assert.AreEqual((uint)200, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestUlongMultiplyUlong()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestMultiply", "TestUlongMultiplyUlong");

            // Ulong * Ulong = Ulong
            Assert.AreEqual((ulong)200, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestFloatMultiplyFloat()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestMultiply", "TestFloatMultiplyFloat");

            Assert.AreEqual((float)200, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestDoubleMultiplyDouble()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestMultiply", "TestDoubleMultiplyDouble");

            Assert.AreEqual((double)200, method.Invoke(null, null));
        }
        #endregion

        #region DifferentType Sbyte
        [TestMethod]
        public void TestSByteMultiplyShort()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestMultiply", "TestSByteMultiplyShort");

            // Sbyte * Short = Int32
            Assert.AreEqual((int)200, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestSByteMultiplyInt()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestMultiply", "TestSByteMultiplyInt");

            // Sbyte * Int = Int32
            Assert.AreEqual((int)200, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestSByteMultiplyLong()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestMultiply", "TestSByteMultiplyLong");

            // Sbyte * Long = Int64
            Assert.AreEqual((long)200, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestSByteMultiplyByte()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestMultiply", "TestSByteMultiplyByte");

            // Sbyte * Byte = Int32
            Assert.AreEqual((int)200, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestSByteMultiplyUshort()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestMultiply", "TestSByteMultiplyUshort");

            // Sbyte * Ushort = Int32
            Assert.AreEqual((int)200, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestSByteMultiplyUint()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestMultiply", "TestSByteMultiplyUint");

            // Sbyte * Uint = Int64
            Assert.AreEqual((long)200, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestSByteMultiplyFloat()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestMultiply", "TestSByteMultiplyFloat");

            // Sbyte * Float = Float
            Assert.AreEqual((float)200, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestSByteMultiplyDouble()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestMultiply", "TestSByteMultiplyDouble");

            // Sbyte * Double = Double
            Assert.AreEqual((double)200, method.Invoke(null, null));
        }
        #endregion

        #region DifferentType Short
        [TestMethod]
        public void TestShortMultiplySbyte()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestMultiply", "TestShortMultiplySbyte");

            // Short * Sbyte = Int32
            Assert.AreEqual((int)200, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestShortMultiplyInt()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestMultiply", "TestShortMultiplyInt");

            // Short * Int = Int32
            Assert.AreEqual((int)200, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestShortMultiplyLong()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestMultiply", "TestShortMultiplyLong");

            // Short * Long = Int64
            Assert.AreEqual((long)200, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestShortMultiplyByte()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestMultiply", "TestShortMultiplyByte");

            // Short * Byte = Int32
            Assert.AreEqual((int)200, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestShortMultiplyUshort()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestMultiply", "TestShortMultiplyUshort");

            // Short * Ushort = Int32
            Assert.AreEqual((int)200, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestShortMultiplyUint()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestMultiply", "TestShortMultiplyUint");

            // Short * Uint = Int64
            Assert.AreEqual((long)200, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestShortMultiplyFloat()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestMultiply", "TestShortMultiplyFloat");

            // Short * Float = Float
            Assert.AreEqual((float)200, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestShortMultiplyDouble()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestMultiply", "TestShortMultiplyDouble");

            // Short * Double = Double
            Assert.AreEqual((double)200, method.Invoke(null, null));
        }
        #endregion

        #region DifferentType Int
        [TestMethod]
        public void TestIntMultiplySbyte()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestMultiply", "TestIntMultiplySbyte");

            // Int * Sbyte = Int32
            Assert.AreEqual((int)200, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestIntMultiplyShort()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestMultiply", "TestIntMultiplyShort");

            // Int * Int = Int32
            Assert.AreEqual((int)200, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestIntMultiplyLong()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestMultiply", "TestIntMultiplyLong");

            // Int * Long = Int64
            Assert.AreEqual((long)200, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestIntMultiplyByte()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestMultiply", "TestIntMultiplyByte");

            // Int * Byte = Int32
            Assert.AreEqual((int)200, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestIntMultiplyUshort()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestMultiply", "TestIntMultiplyUshort");

            // Int * Ushort = Int32
            Assert.AreEqual((int)200, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestIntMultiplyUint()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestMultiply", "TestIntMultiplyUint");

            // Int * Uint = Int64
            Assert.AreEqual((long)200, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestIntMultiplyFloat()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestMultiply", "TestIntMultiplyFloat");

            // Int * Float = Float
            Assert.AreEqual((float)200, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestIntMultiplyDouble()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestMultiply", "TestIntMultiplyDouble");

            // Int * Double = Double
            Assert.AreEqual((double)200, method.Invoke(null, null));
        }
        #endregion

        #region DifferentType Long
        [TestMethod]
        public void TestLongMultiplySbyte()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestMultiply", "TestLongMultiplySbyte");

            // Long * Sbyte = Int64
            Assert.AreEqual((long)200, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestLongMultiplyShort()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestMultiply", "TestLongMultiplyShort");

            // Long * Short = Int64
            Assert.AreEqual((long)200, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestLongMultiplyInt()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestMultiply", "TestLongMultiplyInt");

            // Long * Int = Int64
            Assert.AreEqual((long)200, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestLongMultiplyByte()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestMultiply", "TestLongMultiplyByte");

            // Long * Byte = Int64
            Assert.AreEqual((long)200, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestLongMultiplyUshort()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestMultiply", "TestLongMultiplyUshort");

            // Long * Ushort = Int64
            Assert.AreEqual((long)200, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestLongMultiplyUint()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestMultiply", "TestLongMultiplyUint");

            // Long * Uint = Int64
            Assert.AreEqual((long)200, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestLongMultiplyFloat()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestMultiply", "TestLongMultiplyFloat");

            // Long * Float = Float
            Assert.AreEqual((float)200, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestLongMultiplyDouble()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestMultiply", "TestLongMultiplyDouble");

            // Long * Double = Double
            Assert.AreEqual((double)200, method.Invoke(null, null));
        }
        #endregion

        #region DifferentType Byte
        [TestMethod]
        public void TestByteMultiplyShort()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestMultiply", "TestByteMultiplyShort");

            // Byte * Short = Int32
            Assert.AreEqual((int)200, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestByteMultiplyInt()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestMultiply", "TestByteMultiplyInt");

            // Byte * Int = Int32
            Assert.AreEqual((int)200, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestByteMultiplyLong()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestMultiply", "TestByteMultiplyLong");

            // Byte * Long = Int64
            Assert.AreEqual((long)200, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestByteMultiplySbyte()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestMultiply", "TestByteMultiplySbyte");

            // Byte * Sbyte = Int32
            Assert.AreEqual((int)200, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestByteMultiplyUshort()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestMultiply", "TestByteMultiplyUshort");

            // Byte * Ushort = Int32
            Assert.AreEqual((int)200, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestByteMultiplyUint()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestMultiply", "TestByteMultiplyUint");

            // Byte * Uint = UInt32
            Assert.AreEqual((uint)200, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestByteMultiplyFloat()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestMultiply", "TestByteMultiplyFloat");

            // Byte * Float = Float
            Assert.AreEqual((float)200, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestByteMultiplyDouble()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestMultiply", "TestByteMultiplyDouble");

            // Byte * Double = Double
            Assert.AreEqual((double)200, method.Invoke(null, null));
        }
        #endregion

        #region DifferentType Ushort
        [TestMethod]
        public void TestUshortMultiplyShort()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestMultiply", "TestUshortMultiplyShort");

            // Ushort * Short = Int32
            Assert.AreEqual((int)200, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestUshortMultiplyInt()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestMultiply", "TestUshortMultiplyInt");

            // Ushort * Int = Int32
            Assert.AreEqual((int)200, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestUshortMultiplyLong()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestMultiply", "TestUshortMultiplyLong");

            // Ushort * Long = Int64
            Assert.AreEqual((long)200, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestUshortMultiplySbyte()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestMultiply", "TestUshortMultiplySbyte");

            // Ushort * Sbyte = Int32
            Assert.AreEqual((int)200, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestUshortMultiplyByte()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestMultiply", "TestUshortMultiplyByte");

            // Ushort * Byte = Int32
            Assert.AreEqual((int)200, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestUshortMultiplyUint()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestMultiply", "TestUshortMultiplyUint");

            // Ushort * Uint = UInt32
            Assert.AreEqual((uint)200, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestUshortMultiplyUlong()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestMultiply", "TestUshortMultiplyUlong");

            // Ushort * ULong = UInt64
            Assert.AreEqual((ulong)200, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestUshortMultiplyFloat()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestMultiply", "TestUshortMultiplyFloat");

            // Ushort * Float = Float
            Assert.AreEqual((float)200, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestUshortMultiplyDouble()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestMultiply", "TestUshortMultiplyDouble");

            // Ushort * Double = Double
            Assert.AreEqual((double)200, method.Invoke(null, null));
        }
        #endregion

        #region DifferentType Uint
        [TestMethod]
        public void TestUintMultiplyShort()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestMultiply", "TestUintMultiplyShort");

            // Uint * Short = Int64
            Assert.AreEqual((long)200, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestUintMultiplyInt()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestMultiply", "TestUintMultiplyInt");

            // Uint * Int = Int64
            Assert.AreEqual((long)200, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestUintMultiplyLong()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestMultiply", "TestUintMultiplyLong");

            // Uint * Long = Int64
            Assert.AreEqual((long)200, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestUintMultiplySbyte()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestMultiply", "TestUintMultiplySbyte");

            // Uint * Sbyte = Int64
            Assert.AreEqual((long)200, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestUintMultiplyByte()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestMultiply", "TestUintMultiplyByte");

            // Uint * Byte = UInt32
            Assert.AreEqual((uint)200, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestUintMultiplyUshort()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestMultiply", "TestUintMultiplyUshort");

            // Uint * Ushort = UInt32
            Assert.AreEqual((uint)200, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestUintMultiplyUlong()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestMultiply", "TestUintMultiplyUlong");

            // Uint * ULong = UInt64
            Assert.AreEqual((ulong)200, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestUintMultiplyFloat()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestMultiply", "TestUintMultiplyFloat");

            // Uint * Float = Float
            Assert.AreEqual((float)200, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestUintMultiplyDouble()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestMultiply", "TestUintMultiplyDouble");

            // Uint * Double = Double
            Assert.AreEqual((double)200, method.Invoke(null, null));
        }
        #endregion

        #region DifferentType Ulong
        [TestMethod]
        public void TestUlongMultiplyByte()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestMultiply", "TestUlongMultiplyByte");

            // Ulong * Byte = UInt64
            Assert.AreEqual((ulong)200, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestUlongMultiplyUshort()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestMultiply", "TestUlongMultiplyUshort");

            // Ulong * Ushort = UInt64
            Assert.AreEqual((ulong)200, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestUlongMultiplyUint()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestMultiply", "TestUlongMultiplyUint");

            // Ulong * Uint = UInt64
            Assert.AreEqual((ulong)200, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestUlongMultiplyFloat()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestMultiply", "TestUlongMultiplyFloat");

            // Ulong * Float = Float
            Assert.AreEqual((float)200, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestUlongMultiplyDouble()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestMultiply", "TestUlongMultiplyDouble");

            // Ulong * Double = Double
            Assert.AreEqual((double)200, method.Invoke(null, null));
        }
        #endregion
    }
}
