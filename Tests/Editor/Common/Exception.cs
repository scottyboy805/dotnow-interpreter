using NUnit.Framework;
using System.Reflection;
using TestAssembly;

namespace dotnow.Common
{
    [TestFixture]
    [Category("Unit Test")]
    public class Exception
    {
        [Test]
        public void TestThrowException()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestException), nameof(TestException.ThrowException));

            Assert.Throws(typeof(System.Exception), () => method.Invoke(null, null), "");

            try
            {
                method.Invoke(null, null);
            }
            catch (System.Exception e)
            {
                TestContext.WriteLine("Exception: " + e.Message);
                TestContext.WriteLine("Stack Trace: " + e.StackTrace);
            }
        }
    }
}
