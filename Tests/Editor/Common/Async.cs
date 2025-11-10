using NUnit.Framework;
using System.Reflection;
using TestAssembly;

namespace dotnow.Common
{
    [TestFixture]
    [Category("Unit Test")]
    public class Async
    {
        [Test]
        public void TestAsyncDelay()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestAsync), nameof(TestAsync.TestDelay));

            // Call original - Fix: TestAddition doesn't exist, using TestAdditionInt instead
            object expected = TestArithmetic.TestAdditionInt();
            object actual = method.Invoke(null, null);

            // Check for equal elements
            Assert.AreEqual(expected, actual);
        }
    }
}
