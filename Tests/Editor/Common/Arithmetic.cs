using NUnit.Framework;
using System.Reflection;
using TestAssembly;

namespace dotnow.Common
{
    [TestFixture]
    [Category("Unit Test")]
    public class Arithmetic
    {
        [Test]
        public void TestAddition()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestAddition));

            // Call original
            object[] expected = TestArithmetic.TestAddition();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestSubtraction()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestSubtraction));

            // Call original
            object[] expected = TestArithmetic.TestSubtraction();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestMultiplication()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestMultiplication));

            // Call original
            object[] expected = TestArithmetic.TestMultiplication();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestDivide()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestDivide));

            // Call original
            object[] expected = TestArithmetic.TestDivide();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestDecimal()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestDecimal));

            // Call original
            object expected = TestArithmetic.TestDecimal();
            object actual = method.Invoke(null, null);

            // Check for equal elements
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestModulo()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestModulo));

            // Call original
            object[] expected = TestArithmetic.TestModulo();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestUnaryMinus()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestUnaryMinus));

            // Call original
            object[] expected = TestArithmetic.TestUnaryMinus();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestIncrementDecrement()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestIncrementDecrement));

            // Call original
            object[] expected = TestArithmetic.TestIncrementDecrement();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestBitwiseArithmetic()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestBitwiseArithmetic));

            // Call original
            object[] expected = TestArithmetic.TestBitwiseArithmetic();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestOverflowAndEdgeCases()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestOverflowAndEdgeCases));

            // Call original
            object[] expected = TestArithmetic.TestOverflowAndEdgeCases();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestComplexExpressions()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestComplexExpressions));

            // Call original
            object[] expected = TestArithmetic.TestComplexExpressions();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestPowerOperations()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestPowerOperations));

            // Call original
            object[] expected = TestArithmetic.TestPowerOperations();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
