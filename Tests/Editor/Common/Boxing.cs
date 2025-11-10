using NUnit.Framework;
using System.Reflection;
using TestAssembly;

namespace dotnow.Common
{
    [TestFixture]
    [Category("Unit Test")]
    public class Boxing
    {
        [Test]
        public void TestBoxingInt()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestBoxing), nameof(TestBoxing.TestBoxingInt));

            // Call original
            object expected = TestBoxing.TestBoxingInt();
            object actual = method.Invoke(null, null);

            // Check for equal elements
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestBoxingUInt()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestBoxing), nameof(TestBoxing.TestBoxingUInt));

            // Call original
            object expected = TestBoxing.TestBoxingUInt();
            object actual = method.Invoke(null, null);

            // Check for equal elements
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestBoxingLong()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestBoxing), nameof(TestBoxing.TestBoxingLong));

            // Call original
            object expected = TestBoxing.TestBoxingLong();
            object actual = method.Invoke(null, null);

            // Check for equal elements
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestBoxingULong()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestBoxing), nameof(TestBoxing.TestBoxingULong));

            // Call original
            object expected = TestBoxing.TestBoxingULong();
            object actual = method.Invoke(null, null);

            // Check for equal elements
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestBoxingShort()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestBoxing), nameof(TestBoxing.TestBoxingShort));

            // Call original
            object expected = TestBoxing.TestBoxingShort();
            object actual = method.Invoke(null, null);

            // Check for equal elements
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestBoxingUShort()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestBoxing), nameof(TestBoxing.TestBoxingUShort));

            // Call original
            object expected = TestBoxing.TestBoxingUShort();
            object actual = method.Invoke(null, null);

            // Check for equal elements
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestBoxingByte()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestBoxing), nameof(TestBoxing.TestBoxingByte));

            // Call original
            object expected = TestBoxing.TestBoxingByte();
            object actual = method.Invoke(null, null);

            // Check for equal elements
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestBoxingSByte()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestBoxing), nameof(TestBoxing.TestBoxingSByte));

            // Call original
            object expected = TestBoxing.TestBoxingSByte();
            object actual = method.Invoke(null, null);

            // Check for equal elements
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestBoxingChar()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestBoxing), nameof(TestBoxing.TestBoxingChar));

            // Call original
            object expected = TestBoxing.TestBoxingChar();
            object actual = method.Invoke(null, null);

            // Check for equal elements
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestBoxingFloat()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestBoxing), nameof(TestBoxing.TestBoxingFloat));

            // Call original
            object expected = TestBoxing.TestBoxingFloat();
            object actual = method.Invoke(null, null);

            // Check for equal elements
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestBoxingDouble()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestBoxing), nameof(TestBoxing.TestBoxingDouble));

            // Call original
            object expected = TestBoxing.TestBoxingDouble();
            object actual = method.Invoke(null, null);

            // Check for equal elements
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestBoxingDecimal()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestBoxing), nameof(TestBoxing.TestBoxingDecimal));

            // Call original
            object expected = TestBoxing.TestBoxingDecimal();
            object actual = method.Invoke(null, null);

            // Check for equal elements
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestBoxingBool()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestBoxing), nameof(TestBoxing.TestBoxingBool));

            // Call original
            object expected = TestBoxing.TestBoxingBool();
            object actual = method.Invoke(null, null);

            // Check for equal elements
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestBoxingIntMin()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestBoxing), nameof(TestBoxing.TestBoxingIntMin));

            // Call original
            object expected = TestBoxing.TestBoxingIntMin();
            object actual = method.Invoke(null, null);

            // Check for equal elements
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestBoxingIntMax()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestBoxing), nameof(TestBoxing.TestBoxingIntMax));

            // Call original
            object expected = TestBoxing.TestBoxingIntMax();
            object actual = method.Invoke(null, null);

            // Check for equal elements
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestBoxingIntZero()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestBoxing), nameof(TestBoxing.TestBoxingIntZero));

            // Call original
            object expected = TestBoxing.TestBoxingIntZero();
            object actual = method.Invoke(null, null);

            // Check for equal elements
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestBoxingFloatNaN()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestBoxing), nameof(TestBoxing.TestBoxingFloatNaN));

            // Call original
            object expected = TestBoxing.TestBoxingFloatNaN();
            object actual = method.Invoke(null, null);

            // Check for equal elements
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestBoxingFloatInfinity()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestBoxing), nameof(TestBoxing.TestBoxingFloatInfinity));

            // Call original
            object expected = TestBoxing.TestBoxingFloatInfinity();
            object actual = method.Invoke(null, null);

            // Check for equal elements
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestBoxingDoubleNegativeInfinity()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestBoxing), nameof(TestBoxing.TestBoxingDoubleNegativeInfinity));

            // Call original
            object expected = TestBoxing.TestBoxingDoubleNegativeInfinity();
            object actual = method.Invoke(null, null);

            // Check for equal elements
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestBoxingInArrays()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestBoxing), nameof(TestBoxing.TestBoxingInArrays));

            // Call original
            object[] expected = TestBoxing.TestBoxingInArrays();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestBoxingWithConversions()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestBoxing), nameof(TestBoxing.TestBoxingWithConversions));

            // Call original
            object[] expected = TestBoxing.TestBoxingWithConversions();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements - verify types are preserved correctly
            Assert.AreEqual(expected.Length, actual.Length);
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
                Assert.AreEqual(expected[i].GetType(), actual[i].GetType());
            }
        }

        [Test]
        public void TestBoxingNullables()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestBoxing), nameof(TestBoxing.TestBoxingNullables));

            // Call original
            object[] expected = TestBoxing.TestBoxingNullables();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestBoxingInMethodCall()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestBoxing), nameof(TestBoxing.TestBoxingInMethodCall));

            // Call original
            object expected = TestBoxing.TestBoxingInMethodCall();
            object actual = method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual((object[])expected, (object[])actual);
        }
    }
}