using NUnit.Framework;
using System.Reflection;
using TestAssembly;

namespace dotnow.Common
{
    [TestFixture]
    [Category("Unit Test")]
    public class ByRef
    {
        [Test]
        public void TestOut()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestByRef), nameof(TestByRef.TestOut));

            object[] expected = TestByRef.TestOut();
            object[] actual = (object[])method.Invoke(null, null);

            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void TestOutField()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestByRef), nameof(TestByRef.TestOutField));

            object[] expected = TestByRef.TestOutField();
            object[] actual = (object[])method.Invoke(null, null);

            CollectionAssert.AreEquivalent(expected, actual);
        }
    }
}
