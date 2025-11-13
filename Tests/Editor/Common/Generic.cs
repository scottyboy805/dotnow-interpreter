#if DOTNOW_ENABLE_TESTS
using NUnit.Framework;
using System.Collections;
using System.Reflection;
using TestAssembly;

namespace dotnow.Common
{
    [TestFixture]
    [Category("Unit Test")]
    public class Generic
    {
        [Test]
        public void TestInteropGenericType()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestGeneric), nameof(TestGeneric.TestInteropGenericType));

            IEnumerable expected = (IEnumerable)TestGeneric.TestInteropGenericType();
            IEnumerable actual = (IEnumerable)method.Invoke(null, null);


            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestInteropGenericMethod()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestGeneric), nameof(TestGeneric.TestInteropGenericMethod));

            IEnumerable expected = (IEnumerable)TestGeneric.TestInteropGenericMethod();
            IEnumerable actual = (IEnumerable)method.Invoke(null, null);


            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
#endif