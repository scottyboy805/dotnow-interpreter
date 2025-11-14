#if DOTNOW_ENABLE_TESTS
using NUnit.Framework;
using System.Reflection;
using TestAssembly;

namespace dotnow.Common
{
    [TestFixture]
    [Category("Unit Test")]
    public class String
    {
        [Test]
        public void TestEmpty()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestString), nameof(TestString.TestEmpty));

            // Call original
            object expected = TestString.TestEmpty();
            object actual = method.Invoke(null, null);

            // Check for equal elements
            Assert.AreEqual(expected, actual);
        }
    }
}
#endif