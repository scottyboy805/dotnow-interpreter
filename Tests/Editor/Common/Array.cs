#if DOTNOW_ENABLE_TESTS
using dotnow;
using dotnow.Interop;
using NUnit.Framework;
using System.Reflection;
using TestAssembly;

namespace dotnow.Common
{
    [TestFixture]
    [Category("Unit Test")]
    public class Array
    {
        [Test]
        public void TestUnmanagedValueType()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArray), nameof(TestArray.TestUnmanagedValueTypeArray));

            // Call original
            object expected = TestArray.TestUnmanagedValueTypeArray();
            object actual = method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual((decimal[])expected, (decimal[])actual.UnwrapAsType<decimal[]>());
        }

        [Test]
        public void TestPrimitiveType()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArray), nameof(TestArray.TestPrimitiveTypeArray));

            // Call original
            object expected = TestArray.TestPrimitiveTypeArray();
            object actual = method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual((int[])expected, actual.UnwrapAsType<int[]>());
        }

        [Test]
        public void TestMultidimensionalPrimitiveType()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArray), nameof(TestArray.TestMultidimensionalPrimitiveTypeArray));

            // Call original
            object expected = TestArray.TestMultidimensionalPrimitiveTypeArray();
            object actual = method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual((int[,])expected, actual.UnwrapAsType<int[,]>());
        }
    }
}
#endif