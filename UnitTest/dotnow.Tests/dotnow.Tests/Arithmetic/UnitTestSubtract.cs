using Microsoft.VisualStudio.TestTools.UnitTesting;
using dotnow.Reflection;

namespace dotnow.Tests.Arithmetic
{
    [TestClass]
    public class UnitTestSubtract
    {
        #region SameType
        [TestMethod]
        public void TestSByteSubtractSByte()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestSubtract", "TestSByteSubtractSByte");

            // Sbyte - Sbyte = Int32
            Assert.AreEqual((int)-10, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestShortSubtractShort()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestSubtract", "TestShortSubtractShort");

            // Short - Short = Int32
            Assert.AreEqual((int)-10, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestIntSubtractInt()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestSubtract", "TestIntSubtractInt");

            // Int - Int = Int32
            Assert.AreEqual((int)-10, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestLongSubtractLong()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestSubtract", "TestLongSubtractLong");

            // Long - Long = Int64
            Assert.AreEqual((long)-10, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestByteSubtractByte()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestSubtract", "TestByteSubtractByte");

            // Byte - Byte = Int32
            Assert.AreEqual((int)-10, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestUshortSubtractUshort()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestSubtract", "TestUshortSubtractUshort");

            // UShort - UShort = Int32
            Assert.AreEqual((int)-10, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestUintSubtractUint()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestSubtract", "TestUintSubtractUint");

            // Uint - Uint = Uint
            Assert.AreEqual((uint)4294967286, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestUlongSubtractUlong()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestSubtract", "TestUlongSubtractUlong");

            // Ulong - Ulong = Ulong
            Assert.AreEqual((ulong)18446744073709551606, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestFloatSubtractFloat()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestSubtract", "TestFloatSubtractFloat");

            Assert.AreEqual((float)-10f, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestDoubleSubtractDouble()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestSubtract", "TestDoubleSubtractDouble");

            Assert.AreEqual((double)-10, method.Invoke(null, null));
        }
        #endregion

        #region DifferentType Sbyte
        [TestMethod]
        public void TestSByteSubtractShort()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestSubtract", "TestSByteSubtractShort");

            // Sbyte - Short = Int32
            Assert.AreEqual((int)-10, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestSByteSubtractInt()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestSubtract", "TestSByteSubtractInt");

            // Sbyte - Int = Int32
            Assert.AreEqual((int)-10, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestSByteSubtractLong()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestSubtract", "TestSByteSubtractLong");

            // Sbyte - Long = Int64
            Assert.AreEqual((long)-10, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestSByteSubtractByte()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestSubtract", "TestSByteSubtractByte");

            // Sbyte - Byte = Int32
            Assert.AreEqual((int)-10, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestSByteSubtractUshort()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestSubtract", "TestSByteSubtractUshort");

            // Sbyte - Ushort = Int32
            Assert.AreEqual((int)-10, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestSByteSubtractUint()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestSubtract", "TestSByteSubtractUint");

            // Sbyte - Uint = Int64
            Assert.AreEqual((long)-10, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestSByteSubtractFloat()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestSubtract", "TestSByteSubtractFloat");

            // Sbyte - Float = Float
            Assert.AreEqual((float)-10, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestSByteSubtractDouble()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestSubtract", "TestSByteSubtractDouble");

            // Sbyte - Double = Double
            Assert.AreEqual((double)-10, method.Invoke(null, null));
        }
        #endregion

        #region DifferentType Short
        [TestMethod]
        public void TestShortSubtractSbyte()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestSubtract", "TestShortSubtractSbyte");

            // Short - Sbyte = Int32
            Assert.AreEqual((int)-10, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestShortSubtractInt()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestSubtract", "TestShortSubtractInt");

            // Short - Int = Int32
            Assert.AreEqual((int)-10, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestShortSubtractLong()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestSubtract", "TestShortSubtractLong");

            // Short - Long = Int64
            Assert.AreEqual((long)-10, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestShortSubtractByte()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestSubtract", "TestShortSubtractByte");

            // Short - Byte = Int32
            Assert.AreEqual((int)-10, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestShortSubtractUshort()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestSubtract", "TestShortSubtractUshort");

            // Short - Ushort = Int32
            Assert.AreEqual((int)-10, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestShortSubtractUint()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestSubtract", "TestShortSubtractUint");

            // Short - Uint = Int64
            Assert.AreEqual((long)-10, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestShortSubtractFloat()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestSubtract", "TestShortSubtractFloat");

            // Short - Float = Float
            Assert.AreEqual((float)-10, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestShortSubtractDouble()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestSubtract", "TestShortSubtractDouble");

            // Short - Double = Double
            Assert.AreEqual((double)-10, method.Invoke(null, null));
        }
        #endregion

        #region DifferentType Int
        [TestMethod]
        public void TestIntSubtractSbyte()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestSubtract", "TestIntSubtractSbyte");

            // Int - Sbyte = Int32
            Assert.AreEqual((int)-10, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestIntSubtractShort()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestSubtract", "TestIntSubtractShort");

            // Int - Int = Int32
            Assert.AreEqual((int)-10, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestIntSubtractLong()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestSubtract", "TestIntSubtractLong");

            // Int - Long = Int64
            Assert.AreEqual((long)-10, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestIntSubtractByte()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestSubtract", "TestIntSubtractByte");

            // Int - Byte = Int32
            Assert.AreEqual((int)-10, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestIntSubtractUshort()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestSubtract", "TestIntSubtractUshort");

            // Int - Ushort = Int32
            Assert.AreEqual((int)-10, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestIntSubtractUint()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestSubtract", "TestIntSubtractUint");

            // Int - Uint = Int64
            Assert.AreEqual((long)-10, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestIntSubtractFloat()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestSubtract", "TestIntSubtractFloat");

            // Int - Float = Float
            Assert.AreEqual((float)-10, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestIntSubtractDouble()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestSubtract", "TestIntSubtractDouble");

            // Int - Double = Double
            Assert.AreEqual((double)-10, method.Invoke(null, null));
        }
        #endregion

        #region DifferentType Long
        [TestMethod]
        public void TestLongSubtractSbyte()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestSubtract", "TestLongSubtractSbyte");

            // Long - Sbyte = Int64
            Assert.AreEqual((long)-10, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestLongSubtractShort()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestSubtract", "TestLongSubtractShort");

            // Long - Short = Int64
            Assert.AreEqual((long)-10, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestLongSubtractInt()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestSubtract", "TestLongSubtractInt");

            // Long - Int = Int64
            Assert.AreEqual((long)-10, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestLongSubtractByte()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestSubtract", "TestLongSubtractByte");

            // Long - Byte = Int64
            Assert.AreEqual((long)-10, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestLongSubtractUshort()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestSubtract", "TestLongSubtractUshort");

            // Long - Ushort = Int64
            Assert.AreEqual((long)-10, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestLongSubtractUint()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestSubtract", "TestLongSubtractUint");

            // Long - Uint = Int64
            Assert.AreEqual((long)-10, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestLongSubtractFloat()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestSubtract", "TestLongSubtractFloat");

            // Long - Float = Float
            Assert.AreEqual((float)-10, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestLongSubtractDouble()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestSubtract", "TestLongSubtractDouble");

            // Long - Double = Double
            Assert.AreEqual((double)-10, method.Invoke(null, null));
        }
        #endregion

        #region DifferentType Byte
        [TestMethod]
        public void TestByteSubtractShort()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestSubtract", "TestByteSubtractShort");

            // Byte - Short = Int32
            Assert.AreEqual((int)-10, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestByteSubtractInt()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestSubtract", "TestByteSubtractInt");

            // Byte - Int = Int32
            Assert.AreEqual((int)-10, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestByteSubtractLong()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestSubtract", "TestByteSubtractLong");

            // Byte - Long = Int64
            Assert.AreEqual((long)-10, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestByteSubtractSbyte()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestSubtract", "TestByteSubtractSbyte");

            // Byte - Sbyte = Int32
            Assert.AreEqual((int)-10, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestByteSubtractUshort()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestSubtract", "TestByteSubtractUshort");

            // Byte - Ushort = Int32
            Assert.AreEqual((int)-10, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestByteSubtractUint()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestSubtract", "TestByteSubtractUint");

            // Byte - Uint = UInt32
            Assert.AreEqual((uint)4294967286, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestByteSubtractFloat()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestSubtract", "TestByteSubtractFloat");

            // Byte - Float = Float
            Assert.AreEqual((float)-10, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestByteSubtractDouble()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestSubtract", "TestByteSubtractDouble");

            // Byte - Double = Double
            Assert.AreEqual((double)-10, method.Invoke(null, null));
        }
        #endregion

        #region DifferentType Ushort
        [TestMethod]
        public void TestUshortSubtractShort()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestSubtract", "TestUshortSubtractShort");

            // Ushort - Short = Int32
            Assert.AreEqual((int)-10, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestUshortSubtractInt()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestSubtract", "TestUshortSubtractInt");

            // Ushort - Int = Int32
            Assert.AreEqual((int)-10, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestUshortSubtractLong()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestSubtract", "TestUshortSubtractLong");

            // Ushort - Long = Int64
            Assert.AreEqual((long)-10, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestUshortSubtractSbyte()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestSubtract", "TestUshortSubtractSbyte");

            // Ushort - Sbyte = Int32
            Assert.AreEqual((int)-10, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestUshortSubtractByte()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestSubtract", "TestUshortSubtractByte");

            // Ushort - Byte = Int32
            Assert.AreEqual((int)-10, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestUshortSubtractUint()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestSubtract", "TestUshortSubtractUint");

            // Ushort - Uint = UInt32
            Assert.AreEqual((uint)4294967286, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestUshortSubtractUlong()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestSubtract", "TestUshortSubtractUlong");

            // Ushort - ULong = UInt64
            Assert.AreEqual((ulong)18446744073709551606, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestUshortSubtractFloat()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestSubtract", "TestUshortSubtractFloat");

            // Ushort - Float = Float
            Assert.AreEqual((float)-10, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestUshortSubtractDouble()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestSubtract", "TestUshortSubtractDouble");

            // Ushort - Double = Double
            Assert.AreEqual((double)-10, method.Invoke(null, null));
        }
        #endregion

        #region DifferentType Uint
        [TestMethod]
        public void TestUintSubtractShort()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestSubtract", "TestUintSubtractShort");

            // Uint - Short = Int64
            Assert.AreEqual((long)-10, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestUintSubtractInt()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestSubtract", "TestUintSubtractInt");

            // Uint - Int = Int64
            Assert.AreEqual((long)-10, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestUintSubtractLong()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestSubtract", "TestUintSubtractLong");

            // Uint - Long = Int64
            Assert.AreEqual((long)-10, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestUintSubtractSbyte()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestSubtract", "TestUintSubtractSbyte");

            // Uint - Sbyte = Int64
            Assert.AreEqual((long)-10, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestUintSubtractByte()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestSubtract", "TestUintSubtractByte");

            // Uint - Byte = UInt32
            Assert.AreEqual((uint)4294967286, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestUintSubtractUshort()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestSubtract", "TestUintSubtractUshort");

            // Uint - Ushort = UInt32
            Assert.AreEqual((uint)4294967286, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestUintSubtractUlong()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestSubtract", "TestUintSubtractUlong");

            // Uint - ULong = UInt64
            Assert.AreEqual((ulong)18446744073709551606, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestUintSubtractFloat()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestSubtract", "TestUintSubtractFloat");

            // Uint - Float = Float
            Assert.AreEqual((float)-10, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestUintSubtractDouble()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestSubtract", "TestUintSubtractDouble");

            // Uint - Double = Double
            Assert.AreEqual((double)-10, method.Invoke(null, null));
        }
        #endregion

        #region DifferentType Ulong
        [TestMethod]
        public void TestUlongSubtractByte()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestSubtract", "TestUlongSubtractByte");

            // Ulong - Byte = UInt64
            Assert.AreEqual((ulong)18446744073709551606, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestUlongSubtractUshort()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestSubtract", "TestUlongSubtractUshort");

            // Ulong - Ushort = UInt64
            Assert.AreEqual((ulong)18446744073709551606, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestUlongSubtractUint()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestSubtract", "TestUlongSubtractUint");

            // Ulong - Uint = UInt64
            Assert.AreEqual((ulong)18446744073709551606, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestUlongSubtractFloat()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestSubtract", "TestUlongSubtractFloat");

            // Ulong - Float = Float
            Assert.AreEqual((float)-10, method.Invoke(null, null));
        }

        [TestMethod]
        public void TestUlongSubtractDouble()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestSubtract", "TestUlongSubtractDouble");

            // Ulong - Double = Double
            Assert.AreEqual((double)-10, method.Invoke(null, null));
        }
        #endregion
    }
}
