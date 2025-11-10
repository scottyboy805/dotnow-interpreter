using NUnit.Framework;
using System.Reflection;
using TestAssembly;

namespace dotnow.Common
{
    [TestFixture]
    [Category("Unit Test")]
    public class Bitwise
    {
        // ===== GRANULAR BITWISE AND TESTS BY TYPE =====

        [Test]
        public void TestBitwiseAndInt()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestBitwise), nameof(TestBitwise.TestBitwiseAndInt));
            object expected = TestBitwise.TestBitwiseAndInt();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestBitwiseAndUInt()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestBitwise), nameof(TestBitwise.TestBitwiseAndUInt));
            object expected = TestBitwise.TestBitwiseAndUInt();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestBitwiseAndLong()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestBitwise), nameof(TestBitwise.TestBitwiseAndLong));
            object expected = TestBitwise.TestBitwiseAndLong();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestBitwiseAndShort()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestBitwise), nameof(TestBitwise.TestBitwiseAndShort));
            object expected = TestBitwise.TestBitwiseAndShort();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestBitwiseAndUShort()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestBitwise), nameof(TestBitwise.TestBitwiseAndUShort));
            object expected = TestBitwise.TestBitwiseAndUShort();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestBitwiseAndByte()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestBitwise), nameof(TestBitwise.TestBitwiseAndByte));
            object expected = TestBitwise.TestBitwiseAndByte();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestBitwiseAndSByte()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestBitwise), nameof(TestBitwise.TestBitwiseAndSByte));
            object expected = TestBitwise.TestBitwiseAndSByte();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        // ===== GRANULAR BITWISE OR TESTS BY TYPE =====

        [Test]
        public void TestBitwiseOrInt()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestBitwise), nameof(TestBitwise.TestBitwiseOrInt));
            object expected = TestBitwise.TestBitwiseOrInt();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestBitwiseOrUInt()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestBitwise), nameof(TestBitwise.TestBitwiseOrUInt));
            object expected = TestBitwise.TestBitwiseOrUInt();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestBitwiseOrLong()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestBitwise), nameof(TestBitwise.TestBitwiseOrLong));
            object expected = TestBitwise.TestBitwiseOrLong();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestBitwiseOrShort()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestBitwise), nameof(TestBitwise.TestBitwiseOrShort));
            object expected = TestBitwise.TestBitwiseOrShort();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestBitwiseOrUShort()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestBitwise), nameof(TestBitwise.TestBitwiseOrUShort));
            object expected = TestBitwise.TestBitwiseOrUShort();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestBitwiseOrByte()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestBitwise), nameof(TestBitwise.TestBitwiseOrByte));
            object expected = TestBitwise.TestBitwiseOrByte();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestBitwiseOrSByte()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestBitwise), nameof(TestBitwise.TestBitwiseOrSByte));
            object expected = TestBitwise.TestBitwiseOrSByte();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        // ===== GRANULAR BITWISE XOR TESTS BY TYPE =====

        [Test]
        public void TestBitwiseXorInt()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestBitwise), nameof(TestBitwise.TestBitwiseXorInt));
            object expected = TestBitwise.TestBitwiseXorInt();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestBitwiseXorUInt()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestBitwise), nameof(TestBitwise.TestBitwiseXorUInt));
            object expected = TestBitwise.TestBitwiseXorUInt();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestBitwiseXorLong()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestBitwise), nameof(TestBitwise.TestBitwiseXorLong));
            object expected = TestBitwise.TestBitwiseXorLong();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestBitwiseXorShort()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestBitwise), nameof(TestBitwise.TestBitwiseXorShort));
            object expected = TestBitwise.TestBitwiseXorShort();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestBitwiseXorUShort()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestBitwise), nameof(TestBitwise.TestBitwiseXorUShort));
            object expected = TestBitwise.TestBitwiseXorUShort();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestBitwiseXorByte()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestBitwise), nameof(TestBitwise.TestBitwiseXorByte));
            object expected = TestBitwise.TestBitwiseXorByte();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestBitwiseXorSByte()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestBitwise), nameof(TestBitwise.TestBitwiseXorSByte));
            object expected = TestBitwise.TestBitwiseXorSByte();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        // ===== GRANULAR BITWISE NOT TESTS BY TYPE =====

        [Test]
        public void TestBitwiseNotInt()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestBitwise), nameof(TestBitwise.TestBitwiseNotInt));
            object expected = TestBitwise.TestBitwiseNotInt();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestBitwiseNotUInt()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestBitwise), nameof(TestBitwise.TestBitwiseNotUInt));
            object expected = TestBitwise.TestBitwiseNotUInt();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestBitwiseNotLong()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestBitwise), nameof(TestBitwise.TestBitwiseNotLong));
            object expected = TestBitwise.TestBitwiseNotLong();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestBitwiseNotShort()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestBitwise), nameof(TestBitwise.TestBitwiseNotShort));
            object expected = TestBitwise.TestBitwiseNotShort();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestBitwiseNotUShort()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestBitwise), nameof(TestBitwise.TestBitwiseNotUShort));
            object expected = TestBitwise.TestBitwiseNotUShort();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestBitwiseNotByte()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestBitwise), nameof(TestBitwise.TestBitwiseNotByte));
            object expected = TestBitwise.TestBitwiseNotByte();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        // ===== GRANULAR LEFT SHIFT TESTS BY TYPE =====

        [Test]
        public void TestLeftShiftInt()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestBitwise), nameof(TestBitwise.TestLeftShiftInt));
            object expected = TestBitwise.TestLeftShiftInt();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestLeftShiftUInt()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestBitwise), nameof(TestBitwise.TestLeftShiftUInt));
            object expected = TestBitwise.TestLeftShiftUInt();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestLeftShiftLong()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestBitwise), nameof(TestBitwise.TestLeftShiftLong));
            object expected = TestBitwise.TestLeftShiftLong();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestLeftShiftShort()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestBitwise), nameof(TestBitwise.TestLeftShiftShort));
            object expected = TestBitwise.TestLeftShiftShort();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestLeftShiftUShort()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestBitwise), nameof(TestBitwise.TestLeftShiftUShort));
            object expected = TestBitwise.TestLeftShiftUShort();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestLeftShiftByte()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestBitwise), nameof(TestBitwise.TestLeftShiftByte));
            object expected = TestBitwise.TestLeftShiftByte();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestLeftShiftSByte()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestBitwise), nameof(TestBitwise.TestLeftShiftSByte));
            object expected = TestBitwise.TestLeftShiftSByte();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        // ===== GRANULAR RIGHT SHIFT TESTS BY TYPE =====

        [Test]
        public void TestRightShiftInt()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestBitwise), nameof(TestBitwise.TestRightShiftInt));
            object expected = TestBitwise.TestRightShiftInt();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestRightShiftUInt()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestBitwise), nameof(TestBitwise.TestRightShiftUInt));
            object expected = TestBitwise.TestRightShiftUInt();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestRightShiftLong()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestBitwise), nameof(TestBitwise.TestRightShiftLong));
            object expected = TestBitwise.TestRightShiftLong();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestRightShiftShort()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestBitwise), nameof(TestBitwise.TestRightShiftShort));
            object expected = TestBitwise.TestRightShiftShort();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestRightShiftUShort()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestBitwise), nameof(TestBitwise.TestRightShiftUShort));
            object expected = TestBitwise.TestRightShiftUShort();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestRightShiftByte()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestBitwise), nameof(TestBitwise.TestRightShiftByte));
            object expected = TestBitwise.TestRightShiftByte();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestRightShiftSByte()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestBitwise), nameof(TestBitwise.TestRightShiftSByte));
            object expected = TestBitwise.TestRightShiftSByte();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }
    }
}