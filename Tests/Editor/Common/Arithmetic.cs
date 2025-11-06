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
    }
}
