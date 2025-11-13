#if DOTNOW_ENABLE_TESTS
using NUnit.Framework;
using System.Reflection;
using TestAssembly;

namespace dotnow.Complete
{
    [TestFixture]
    [Category("Complete Test")]
    public class Fibonacci
    {
        [TestCase(5, TestName = "Fib(5)")]
        [TestCase(9, TestName = "Fib(9)")]
        [TestCase(13, TestName = "Fib(13)")]
        [TestCase(18, TestName = "Fib(18)")]
        [TestCase(40, TestName = "Fib(40)")]
        public void TestIterative(int value)
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestFibonacci), nameof(TestFibonacci.Fibonacci_Iterative));

            // Run the test against the clr
            Assert.AreEqual(
                TestFibonacci.Fibonacci_Iterative(value),
                method.Invoke(null, new object[] { value }));
        }

        [TestCase(0, TestName = "Fib(0)")]
        [TestCase(1, TestName = "Fib(1)")]
        [TestCase(2, TestName = "Fib(2)")]
        [TestCase(5, TestName = "Fib(5)")]
        [TestCase(9, TestName = "Fib(9)")]
        [TestCase(13, TestName = "Fib(13)")]
        [TestCase(18, TestName = "Fib(18)")]
        [TestCase(18, TestName = "Fib(40)")]
        public void TestRecursive(int value)
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestFibonacci), nameof(TestFibonacci.Fibonacci_Recursive));

            Assert.AreEqual(
                TestFibonacci.Fibonacci_Recursive(value),
                method.Invoke(null, new object[] { value }));
        }
    }
}
#endif