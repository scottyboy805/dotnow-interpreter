using NUnit.Framework;
using System.Reflection;
using TestAssembly;

namespace dotnow.Common
{
    [TestFixture]
    [Category("Unit Test")]
    public class Comparison
    {
        [Test]
        public void TestEquality()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestComparison), nameof(TestComparison.TestEquality));

            // Call original
            object[] expected = TestComparison.TestEquality();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestInequality()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestComparison), nameof(TestComparison.TestInequality));

            // Call original
            object[] expected = TestComparison.TestInequality();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestLessThan()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestComparison), nameof(TestComparison.TestLessThan));

            // Call original
            object[] expected = TestComparison.TestLessThan();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestGreaterThan()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestComparison), nameof(TestComparison.TestGreaterThan));

            // Call original
            object[] expected = TestComparison.TestGreaterThan();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestLessThanOrEqual()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestComparison), nameof(TestComparison.TestLessThanOrEqual));

            // Call original
            object[] expected = TestComparison.TestLessThanOrEqual();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestGreaterThanOrEqual()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestComparison), nameof(TestComparison.TestGreaterThanOrEqual));

            // Call original
            object[] expected = TestComparison.TestGreaterThanOrEqual();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}