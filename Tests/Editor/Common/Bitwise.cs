using NUnit.Framework;
using System.Reflection;
using TestAssembly;

namespace dotnow.Common
{
    [TestFixture]
    [Category("Unit Test")]
    public class Bitwise
    {
        [Test]
        public void TestBitwiseAnd()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestBitwise), nameof(TestBitwise.TestBitwiseAnd));

            // Call original
            object[] expected = TestBitwise.TestBitwiseAnd();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestBitwiseOr()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestBitwise), nameof(TestBitwise.TestBitwiseOr));

            // Call original
            object[] expected = TestBitwise.TestBitwiseOr();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestBitwiseXor()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestBitwise), nameof(TestBitwise.TestBitwiseXor));

            // Call original
            object[] expected = TestBitwise.TestBitwiseXor();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestBitwiseNot()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestBitwise), nameof(TestBitwise.TestBitwiseNot));

            // Call original
            object[] expected = TestBitwise.TestBitwiseNot();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestLeftShift()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestBitwise), nameof(TestBitwise.TestLeftShift));

            // Call original
            object[] expected = TestBitwise.TestLeftShift();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestRightShift()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestBitwise), nameof(TestBitwise.TestRightShift));

            // Call original
            object[] expected = TestBitwise.TestRightShift();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}