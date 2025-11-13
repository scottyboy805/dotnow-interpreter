using NUnit.Framework;
using System.Reflection;
using TestAssembly;

namespace dotnow.Common
{
    [TestFixture]
    [Category("Unit Test")]
    public class Interop
    {
        [Test]
        public void TestUnmanagedPrimitive_Argument()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestInterop), nameof(TestInterop.UnmanagedPrimitive_Argument));

            Assert.AreEqual(
                TestInterop.UnmanagedPrimitive_Argument(),
                method.Invoke(null, null));
        }

        [Test]
        public void TestUnmanagedValueTypeInstance_Argument()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestInterop), nameof(TestInterop.UnmanagedStruct_Argument));

            Assert.AreEqual(
                TestInterop.UnmanagedStruct_Argument(),
                method.Invoke(null, null));
        }
    }
}
