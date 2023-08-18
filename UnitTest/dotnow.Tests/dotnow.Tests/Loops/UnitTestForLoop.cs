using dotnow.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace dotnow.Tests.Loops
{
    [TestClass]
    public class UnitTestForLoop
    {
        [TestMethod]
        public void TestForLoopInt32()
        {
            CLRMethod method = TestUtils.LoadTestMethod("TestForLoop", "TestForLoopInt32");

            // Try to invoke
            method.Invoke(null, null);
        }
    }
}
