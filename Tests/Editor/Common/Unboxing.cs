#if DOTNOW_ENABLE_TESTS
using NUnit.Framework;
using System.Reflection;
using TestAssembly;

namespace dotnow.Common
{
    [TestFixture]
    [Category("Unit Test")]
    public class Unboxing
    {
        [Test]
        public void TestUnboxingInt()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestUnboxing), nameof(TestUnboxing.TestUnboxingInt));

            // Call original
            object expected = TestUnboxing.TestUnboxingInt();
            object actual = method.Invoke(null, null);

            // Check for equal elements
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestUnboxingUInt()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestUnboxing), nameof(TestUnboxing.TestUnboxingUInt));

            // Call original
            object expected = TestUnboxing.TestUnboxingUInt();
            object actual = method.Invoke(null, null);

            // Check for equal elements
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestUnboxingLong()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestUnboxing), nameof(TestUnboxing.TestUnboxingLong));

            // Call original
            object expected = TestUnboxing.TestUnboxingLong();
            object actual = method.Invoke(null, null);

            // Check for equal elements
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestUnboxingULong()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestUnboxing), nameof(TestUnboxing.TestUnboxingULong));

            // Call original
            object expected = TestUnboxing.TestUnboxingULong();
            object actual = method.Invoke(null, null);

            // Check for equal elements
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestUnboxingShort()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestUnboxing), nameof(TestUnboxing.TestUnboxingShort));

            // Call original
            object expected = TestUnboxing.TestUnboxingShort();
            object actual = method.Invoke(null, null);

            // Check for equal elements
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestUnboxingUShort()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestUnboxing), nameof(TestUnboxing.TestUnboxingUShort));

            // Call original
            object expected = TestUnboxing.TestUnboxingUShort();
            object actual = method.Invoke(null, null);

            // Check for equal elements
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestUnboxingByte()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestUnboxing), nameof(TestUnboxing.TestUnboxingByte));

            // Call original
            object expected = TestUnboxing.TestUnboxingByte();
            object actual = method.Invoke(null, null);

            // Check for equal elements
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestUnboxingSByte()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestUnboxing), nameof(TestUnboxing.TestUnboxingSByte));

            // Call original
            object expected = TestUnboxing.TestUnboxingSByte();
            object actual = method.Invoke(null, null);

            // Check for equal elements
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestUnboxingChar()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestUnboxing), nameof(TestUnboxing.TestUnboxingChar));

            // Call original
            object expected = TestUnboxing.TestUnboxingChar();
            object actual = method.Invoke(null, null);

            // Check for equal elements
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestUnboxingFloat()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestUnboxing), nameof(TestUnboxing.TestUnboxingFloat));

            // Call original
            object expected = TestUnboxing.TestUnboxingFloat();
            object actual = method.Invoke(null, null);

            // Check for equal elements
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestUnboxingDouble()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestUnboxing), nameof(TestUnboxing.TestUnboxingDouble));

            // Call original
            object expected = TestUnboxing.TestUnboxingDouble();
            object actual = method.Invoke(null, null);

            // Check for equal elements
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestUnboxingDecimal()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestUnboxing), nameof(TestUnboxing.TestUnboxingDecimal));

            // Call original
            object expected = TestUnboxing.TestUnboxingDecimal();
            object actual = method.Invoke(null, null);

            // Check for equal elements
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestUnboxingBool()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestUnboxing), nameof(TestUnboxing.TestUnboxingBool));

            // Call original
            object expected = TestUnboxing.TestUnboxingBool();
            object actual = method.Invoke(null, null);

            // Check for equal elements
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestUnboxingIntMin()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestUnboxing), nameof(TestUnboxing.TestUnboxingIntMin));

            // Call original
            object expected = TestUnboxing.TestUnboxingIntMin();
            object actual = method.Invoke(null, null);

            // Check for equal elements
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestUnboxingIntMax()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestUnboxing), nameof(TestUnboxing.TestUnboxingIntMax));

            // Call original
            object expected = TestUnboxing.TestUnboxingIntMax();
            object actual = method.Invoke(null, null);

            // Check for equal elements
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestUnboxingFloatNaN()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestUnboxing), nameof(TestUnboxing.TestUnboxingFloatNaN));

            // Call original
            object expected = TestUnboxing.TestUnboxingFloatNaN();
            object actual = method.Invoke(null, null);

            // Check for equal elements
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestUnboxingFloatInfinity()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestUnboxing), nameof(TestUnboxing.TestUnboxingFloatInfinity));

            // Call original
            object expected = TestUnboxing.TestUnboxingFloatInfinity();
            object actual = method.Invoke(null, null);

            // Check for equal elements
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestUnboxingDoubleNegativeInfinity()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestUnboxing), nameof(TestUnboxing.TestUnboxingDoubleNegativeInfinity));

            // Call original
            object expected = TestUnboxing.TestUnboxingDoubleNegativeInfinity();
            object actual = method.Invoke(null, null);

            // Check for equal elements
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestUnboxingFromArrays()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestUnboxing), nameof(TestUnboxing.TestUnboxingFromArrays));

            // Call original
            object[] expected = TestUnboxing.TestUnboxingFromArrays();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestBoxingUnboxingRoundTrip()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestUnboxing), nameof(TestUnboxing.TestBoxingUnboxingRoundTrip));

            // Call original
            object[] expected = TestUnboxing.TestBoxingUnboxingRoundTrip();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements - all should be true
            CollectionAssert.AreEqual(expected, actual);

            // Verify all elements are true (successful round-trip)
            foreach (object result in actual)
            {
                Assert.IsTrue((bool)result, "Round-trip boxing/unboxing failed for a value type");
            }
        }

        [Test]
        public void TestUnboxingWrongType()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestUnboxing), nameof(TestUnboxing.TestUnboxingWrongType));

            // Call original
            object expected = TestUnboxing.TestUnboxingWrongType();
            object actual = method.Invoke(null, null);

            // Should both return true (exception was caught correctly)
            Assert.AreEqual(expected, actual);
            Assert.IsTrue((bool)actual, "Wrong type unboxing should throw InvalidCastException");
        }

        [Test]
        public void TestUnboxingNull()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestUnboxing), nameof(TestUnboxing.TestUnboxingNull));

            // Call original
            object expected = TestUnboxing.TestUnboxingNull();
            object actual = method.Invoke(null, null);

            // Should both return true (exception was caught correctly)
            Assert.AreEqual(expected, actual);
            Assert.IsTrue((bool)actual, "Null unboxing should throw NullReferenceException");
        }

        [Test]
        public void TestUnboxingCompatibility()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestUnboxing), nameof(TestUnboxing.TestUnboxingCompatibility));

            // Call original
            object[] expected = TestUnboxing.TestUnboxingCompatibility();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);

            // Verify specific results
            Assert.AreEqual(42, actual[0]); // int unboxing should work
            Assert.AreEqual(3.14f, actual[1]); // float unboxing should work
            Assert.AreEqual(true, actual[2]); // bool unboxing should work
            Assert.IsTrue((bool)actual[3], "Short to int unboxing should fail"); // short->int should fail
            Assert.IsTrue((bool)actual[4], "Int to long unboxing should fail"); // int->long should fail
            Assert.IsTrue((bool)actual[5], "Float to double unboxing should fail"); // float->double should fail
        }

        [Test]
        public void TestUnboxingInReturn()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestUnboxing), nameof(TestUnboxing.TestUnboxingInReturn));

            // Call original
            object expected = TestUnboxing.TestUnboxingInReturn();
            object actual = method.Invoke(null, null);

            // Check for equal elements
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///  Currently causes crashes when attempting to pass a nullable object via reflection to an interop method.
        /// </summary>
        //[Test]
        //public void TestUnboxingToNullable()
        //{
        //    // Try to load method
        //    MethodInfo method = TestUtils.LoadTestMethod(nameof(TestUnboxing), nameof(TestUnboxing.TestUnboxingToNullable));

        //    // Call original
        //    object[] expected = TestUnboxing.TestUnboxingToNullable();
        //    object[] actual = (object[])method.Invoke(null, null);

        //    // Check for equal elements
        //    CollectionAssert.AreEqual(expected, actual);

        //    // Verify specific results
        //    Assert.AreEqual(42, actual[0]); // Should unbox to nullable int with value
        //    Assert.IsNull(actual[1]); // Should be null
        //}
    }
}
#endif