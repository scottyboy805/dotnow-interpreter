using NUnit.Framework;
using System;
using System.Reflection;
using TestAssembly;

namespace dotnow.Common
{
    [TestFixture]
    [Category("Unit Test")]
    public class Call
    {
        [TestCase(128, TestName = "Simple Call (128)")]
        [TestCase(600, TestName = "Simple Call (600)")]
        [TestCase(-350, TestName = "Simple Call (-350)")]
        public void TestSimpleCall(int value)
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestCall), nameof(TestCall.CallSimple));

            Assert.AreEqual(
                TestCall.CallSimple(value),
                method.Invoke(null, new object[] { value }));
        }

        [TestCase("Hello World", TestName = "Object Call (String)")]
        public void TestObjectCall(object value)
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestCall), nameof(TestCall.CallObject));

            Assert.AreEqual(
                TestCall.CallObject(value),
                method.Invoke(null, new object[] { value }));
        }

        [Test]
        public void TestVirtualCall()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestCall), nameof(TestCall.CallVirtual));

            object[] expected = TestCall.CallVirtual();
            object[] actual = (object[])method.Invoke(null, null);

            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void TestStaticDelegate()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestCall), nameof(TestCall.TestStaticDelegate));

            Func<int> expectedDelegate = TestCall.TestStaticDelegate;
            Func<int> actualDelegate = (Func<int>)method.CreateDelegate(typeof(Func<int>));

            Assert.AreEqual(expectedDelegate(), actualDelegate());
        }
    }
}
