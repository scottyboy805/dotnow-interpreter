using Microsoft.VisualStudio.TestTools.UnitTesting;
using dotnow.Reflection;

namespace dotnow.Tests.Arithmetic
{
    [TestClass]
    public class UnitTestAdd
    {
        #region SameType
        [TestMethod]
        public void TestSByteAddSByte()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestAdd", "TestSByteAddSByte");

            // Sbyte + Sbyte = Int32
            Assert.AreEqual((int)30, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestShortAddShort()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestAdd", "TestShortAddShort");

            // Short + Short = Int32
            Assert.AreEqual((int)30, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestIntAddInt()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestAdd", "TestIntAddInt");

            // Int + Int = Int32
            Assert.AreEqual((int)30, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestLongAddLong()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestAdd", "TestLongAddLong");

            // Long + Long = Int64
            Assert.AreEqual((long)30, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestByteAddByte()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestAdd", "TestByteAddByte");

            // Byte + Byte = Int32
            Assert.AreEqual((int)30, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestUshortAddUshort()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestAdd", "TestUshortAddUshort");

            // Short + Short = Int32
            Assert.AreEqual((int)30, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestUintAddUint()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestAdd", "TestUintAddUint");

            // Uint + Uint = Uint
            Assert.AreEqual((uint)30, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestUlongAddUlong()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestAdd", "TestUlongAddUlong");

            // Ulong + Ulong = Ulong
            Assert.AreEqual((ulong)30, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestFloatAddFloat()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestAdd", "TestFloatAddFloat");

            Assert.AreEqual((float)30, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestDoubleAddDouble()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestAdd", "TestDoubleAddDouble");

            Assert.AreEqual((double)30, method.Invoke(null, null));
        }
        #endregion

        #region DifferentType Sbyte
        [TestMethod]
        public void TestSByteAddShort()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestAdd", "TestSByteAddShort");

            // Sbyte + Short = Int32
            Assert.AreEqual((int)30, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestSByteAddInt()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestAdd", "TestSByteAddInt");

            // Sbyte + Int = Int32
            Assert.AreEqual((int)30, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestSByteAddLong()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestAdd", "TestSByteAddLong");

            // Sbyte + Long = Int64
            Assert.AreEqual((long)30, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestSByteAddByte()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestAdd", "TestSByteAddByte");

            // Sbyte + Byte = Int32
            Assert.AreEqual((int)30, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestSByteAddUshort()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestAdd", "TestSByteAddUshort");

            // Sbyte + Ushort = Int32
            Assert.AreEqual((int)30, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestSByteAddUint()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestAdd", "TestSByteAddUint");

            // Sbyte + Uint = Int64
            Assert.AreEqual((long)30, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestSByteAddFloat()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestAdd", "TestSByteAddFloat");

            // Sbyte + Float = Float
            Assert.AreEqual((float)30, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestSByteAddDouble()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestAdd", "TestSByteAddDouble");

            // Sbyte + Double = Double
            Assert.AreEqual((double)30, method.Invoke(null, null));
        }
        #endregion

        #region DifferentType Short
        [TestMethod]
        public void TestShortAddSbyte()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestAdd", "TestShortAddSbyte");

            // Short + Sbyte = Int32
            Assert.AreEqual((int)30, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestShortAddInt()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestAdd", "TestShortAddInt");

            // Short + Int = Int32
            Assert.AreEqual((int)30, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestShortAddLong()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestAdd", "TestShortAddLong");

            // Short + Long = Int64
            Assert.AreEqual((long)30, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestShortAddByte()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestAdd", "TestShortAddByte");

            // Short + Byte = Int32
            Assert.AreEqual((int)30, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestShortAddUshort()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestAdd", "TestShortAddUshort");

            // Short + Ushort = Int32
            Assert.AreEqual((int)30, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestShortAddUint()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestAdd", "TestShortAddUint");

            // Short + Uint = Int64
            Assert.AreEqual((long)30, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestShortAddFloat()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestAdd", "TestShortAddFloat");

            // Short + Float = Float
            Assert.AreEqual((float)30, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestShortAddDouble()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestAdd", "TestShortAddDouble");

            // Short + Double = Double
            Assert.AreEqual((double)30, method.Invoke(null, null));
        }
        #endregion

        #region DifferentType Int
        [TestMethod]
        public void TestIntAddSbyte()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestAdd", "TestIntAddSbyte");

            // Int + Sbyte = Int32
            Assert.AreEqual((int)30, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestIntAddShort()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestAdd", "TestIntAddShort");

            // Int + Int = Int32
            Assert.AreEqual((int)30, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestIntAddLong()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestAdd", "TestIntAddLong");

            // Int + Long = Int64
            Assert.AreEqual((long)30, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestIntAddByte()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestAdd", "TestIntAddByte");

            // Int + Byte = Int32
            Assert.AreEqual((int)30, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestIntAddUshort()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestAdd", "TestIntAddUshort");

            // Int + Ushort = Int32
            Assert.AreEqual((int)30, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestIntAddUint()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestAdd", "TestIntAddUint");

            // Int + Uint = Int64
            Assert.AreEqual((long)30, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestIntAddFloat()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestAdd", "TestIntAddFloat");

            // Int + Float = Float
            Assert.AreEqual((float)30, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestIntAddDouble()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestAdd", "TestIntAddDouble");

            // Int + Double = Double
            Assert.AreEqual((double)30, method.Invoke(null, null));
        }
        #endregion

        #region DifferentType Long
        [TestMethod]
        public void TestLongAddSbyte()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestAdd", "TestLongAddSbyte");

            // Long + Sbyte = Int64
            Assert.AreEqual((long)30, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestLongAddShort()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestAdd", "TestLongAddShort");

            // Long + Short = Int64
            Assert.AreEqual((long)30, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestLongAddInt()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestAdd", "TestLongAddInt");

            // Long + Int = Int64
            Assert.AreEqual((long)30, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestLongAddByte()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestAdd", "TestLongAddByte");

            // Long + Byte = Int64
            Assert.AreEqual((long)30, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestLongAddUshort()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestAdd", "TestLongAddUshort");

            // Long + Ushort = Int64
            Assert.AreEqual((long)30, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestLongAddUint()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestAdd", "TestLongAddUint");

            // Long + Uint = Int64
            Assert.AreEqual((long)30, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestLongAddFloat()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestAdd", "TestLongAddFloat");

            // Long + Float = Float
            Assert.AreEqual((float)30, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestLongAddDouble()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestAdd", "TestLongAddDouble");

            // Long + Double = Double
            Assert.AreEqual((double)30, method.Invoke(null, null));
        }
        #endregion

        #region DifferentType Byte
        [TestMethod]
        public void TestByteAddShort()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestAdd", "TestByteAddShort");

            // Byte + Short = Int32
            Assert.AreEqual((int)30, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestByteAddInt()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestAdd", "TestByteAddInt");

            // Byte + Int = Int32
            Assert.AreEqual((int)30, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestByteAddLong()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestAdd", "TestByteAddLong");

            // Byte + Long = Int64
            Assert.AreEqual((long)30, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestByteAddSbyte()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestAdd", "TestByteAddSbyte");

            // Byte + Sbyte = Int32
            Assert.AreEqual((int)30, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestByteAddUshort()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestAdd", "TestByteAddUshort");

            // Byte + Ushort = Int32
            Assert.AreEqual((int)30, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestByteAddUint()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestAdd", "TestByteAddUint");

            // Byte + Uint = UInt32
            Assert.AreEqual((uint)30, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestByteAddFloat()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestAdd", "TestByteAddFloat");

            // Byte + Float = Float
            Assert.AreEqual((float)30, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestByteAddDouble()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestAdd", "TestByteAddDouble");

            // Byte + Double = Double
            Assert.AreEqual((double)30, method.Invoke(null, null));
        }
        #endregion

        #region DifferentType Ushort
        [TestMethod]
        public void TestUshortAddShort()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestAdd", "TestUshortAddShort");

            // Ushort + Short = Int32
            Assert.AreEqual((int)30, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestUshortAddInt()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestAdd", "TestUshortAddInt");

            // Ushort + Int = Int32
            Assert.AreEqual((int)30, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestUshortAddLong()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestAdd", "TestUshortAddLong");

            // Ushort + Long = Int64
            Assert.AreEqual((long)30, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestUshortAddSbyte()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestAdd", "TestUshortAddSbyte");

            // Ushort + Sbyte = Int32
            Assert.AreEqual((int)30, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestUshortAddByte()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestAdd", "TestUshortAddByte");

            // Ushort + Byte = Int32
            Assert.AreEqual((int)30, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestUshortAddUint()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestAdd", "TestUshortAddUint");

            // Ushort + Uint = UInt32
            Assert.AreEqual((uint)30, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestUshortAddUlong()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestAdd", "TestUshortAddUlong");

            // Ushort + ULong = UInt64
            Assert.AreEqual((ulong)30, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestUshortAddFloat()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestAdd", "TestUshortAddFloat");

            // Ushort + Float = Float
            Assert.AreEqual((float)30, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestUshortAddDouble()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestAdd", "TestUshortAddDouble");

            // Ushort + Double = Double
            Assert.AreEqual((double)30, method.Invoke(null, null));
        }
        #endregion

        #region DifferentType Uint
        [TestMethod]
        public void TestUintAddShort()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestAdd", "TestUintAddShort");

            // Uint + Short = Int64
            Assert.AreEqual((long)30, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestUintAddInt()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestAdd", "TestUintAddInt");

            // Uint + Int = Int64
            Assert.AreEqual((long)30, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestUintAddLong()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestAdd", "TestUintAddLong");

            // Uint + Long = Int64
            Assert.AreEqual((long)30, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestUintAddSbyte()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestAdd", "TestUintAddSbyte");

            // Uint + Sbyte = Int64
            Assert.AreEqual((long)30, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestUintAddByte()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestAdd", "TestUintAddByte");

            // Uint + Byte = UInt32
            Assert.AreEqual((uint)30, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestUintAddUshort()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestAdd", "TestUintAddUshort");

            // Uint + Ushort = UInt32
            Assert.AreEqual((uint)30, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestUintAddUlong()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestAdd", "TestUintAddUlong");

            // Uint + ULong = UInt64
            Assert.AreEqual((ulong)30, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestUintAddFloat()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestAdd", "TestUintAddFloat");

            // Uint + Float = Float
            Assert.AreEqual((float)30, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestUintAddDouble()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestAdd", "TestUintAddDouble");

            // Uint + Double = Double
            Assert.AreEqual((double)30, method.Invoke(null, null));
        }
        #endregion

        #region DifferentType Ulong
        [TestMethod]
        public void TestUlongAddByte()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestAdd", "TestUlongAddByte");

            // Ulong + Byte = UInt64
            Assert.AreEqual((ulong)30, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestUlongAddUshort()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestAdd", "TestUlongAddUshort");

            // Ulong + Ushort = UInt64
            Assert.AreEqual((ulong)30, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestUlongAddUint()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestAdd", "TestUlongAddUint");

            // Ulong + Uint = UInt64
            Assert.AreEqual((ulong)30, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestUlongAddFloat()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestAdd", "TestUlongAddFloat");

            // Ulong + Float = Float
            Assert.AreEqual((float)30, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestUlongAddDouble()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestAdd", "TestUlongAddDouble");

            // Ulong + Double = Double
            Assert.AreEqual((double)30, method.Invoke(null, null));
        }
        #endregion
    }
}
