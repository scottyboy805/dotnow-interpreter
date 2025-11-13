#if DOTNOW_ENABLE_TESTS
using NUnit.Framework;
using System.Collections;
using System.Reflection;
using TestAssembly;

namespace dotnow.Common
{
    [TestFixture]
    [Category("Unit Test")]
    public class Enum
    {
        private sealed class EnumComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                return x.ToString().CompareTo(y.ToString());
            }
        }

        private static readonly EnumComparer comparer = new EnumComparer();

        [Test]
        public void TestEnumReturn()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestEnum), nameof(TestEnum.TestReturnEnum));

            Assert.AreEqual(
                TestEnum.TestReturnEnum().ToString(),
                method.Invoke(null, null).ToString());
        }

        [Test]
        public void TestEnumReturnName()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestEnum), nameof(TestEnum.TestReturnEnumName));

            Assert.AreEqual(
                TestEnum.TestReturnEnumName(),
                method.Invoke(null, null));
        }

        [Test]
        public void TestGetEnumNames()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestEnum), nameof(TestEnum.TestGetEnumNames));

            Assert.AreEqual(
                TestEnum.TestGetEnumNames(),
                method.Invoke(null, null));
        }

        [Test]
        public void TestParseEnum()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestEnum), nameof(TestEnum.TestParseEnum));

            Assert.AreEqual(
                TestEnum.TestParseEnum().ToString(),
                method.Invoke(null, null).ToString());
        }

        [Test]
        public void TestParseGenericEnum()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestEnum), nameof(TestEnum.TestParseGenericEnum));

            Assert.AreEqual(
                TestEnum.TestParseGenericEnum().ToString(),
                method.Invoke(null, null).ToString());
        }

        [Test]
        public void TestTryParseEnum()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestEnum), nameof(TestEnum.TestTryParseEnum));

            Assert.AreEqual(
                TestEnum.TestTryParseEnum().ToString(),
                method.Invoke(null, null)?.ToString());
        }

        [Test]
        public void TestTryParseGenericEnum()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestEnum), nameof(TestEnum.TestTryParseGenericEnum));

            Assert.AreEqual(
                TestEnum.TestTryParseGenericEnum().ToString(),
                method.Invoke(null, null)?.ToString());
        }

        [Test]
        public void TestGetUnderlyingEnumType()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestEnum), nameof(TestEnum.TestGetUnderlyingEnumType));

            Assert.AreEqual(
                TestEnum.TestGetUnderlyingEnumType().ToString(),
                method.Invoke(null, null).ToString());
        }

        [Test]
        public void TestGetEnumValues()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestEnum), nameof(TestEnum.TestGetEnumValues));

            CollectionAssert.AreEqual(
                (System.Array)TestEnum.TestGetEnumValues(),
                (System.Array)method.Invoke(null, null),
                comparer);
        }

        [Test]
        public void TestFormatEnum()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestEnum), nameof(TestEnum.TestFormatEnum));

            Assert.AreEqual(
                TestEnum.TestFormatEnum(),
                method.Invoke(null, null));
        }
    }
}
#endif