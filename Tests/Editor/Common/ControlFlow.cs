#if DOTNOW_ENABLE_TESTS
using NUnit.Framework;
using System.Reflection;
using TestAssembly;

namespace dotnow.Common
{
    [TestFixture]
    [Category("Unit Test")]
    public class ControlFlow
    {
        [Test]
        public void TestIfElse()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestControlFlow), nameof(TestControlFlow.TestIfElse));

            // Call original
            object[] expected = TestControlFlow.TestIfElse();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestSwitch()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestControlFlow), nameof(TestControlFlow.TestSwitch));

            // Call original
            object[] expected = TestControlFlow.TestSwitch();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestForLoop()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestControlFlow), nameof(TestControlFlow.TestForLoop));

            // Call original
            object[] expected = TestControlFlow.TestForLoop();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestWhileLoop()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestControlFlow), nameof(TestControlFlow.TestWhileLoop));

            // Call original
            object[] expected = TestControlFlow.TestWhileLoop();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestDoWhileLoop()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestControlFlow), nameof(TestControlFlow.TestDoWhileLoop));

            // Call original
            object[] expected = TestControlFlow.TestDoWhileLoop();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestBreakContinue()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestControlFlow), nameof(TestControlFlow.TestBreakContinue));

            // Call original
            object[] expected = TestControlFlow.TestBreakContinue();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestNestedLoops()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestControlFlow), nameof(TestControlFlow.TestNestedLoops));

            // Call original
            object[] expected = TestControlFlow.TestNestedLoops();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestBranchEquality()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestControlFlow), nameof(TestControlFlow.TestBranchEquality));

            // Call original
            object[] expected = TestControlFlow.TestBranchEquality();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestBranchGreaterThan()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestControlFlow), nameof(TestControlFlow.TestBranchGreaterThan));

            // Call original
            object[] expected = TestControlFlow.TestBranchGreaterThan();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestBranchLessThan()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestControlFlow), nameof(TestControlFlow.TestBranchLessThan));

            // Call original
            object[] expected = TestControlFlow.TestBranchLessThan();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestDirectBranchComparisons()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestControlFlow), nameof(TestControlFlow.TestDirectBranchComparisons));

            // Call original
            object[] expected = TestControlFlow.TestDirectBranchComparisons();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestUnsignedBranchComparisons()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestControlFlow), nameof(TestControlFlow.TestUnsignedBranchComparisons));

            // Call original
            object[] expected = TestControlFlow.TestUnsignedBranchComparisons();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestNullReferenceBranches()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestControlFlow), nameof(TestControlFlow.TestNullReferenceBranches));

            // Call original
            object[] expected = TestControlFlow.TestNullReferenceBranches();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestBooleanBranches()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestControlFlow), nameof(TestControlFlow.TestBooleanBranches));

            // Call original
            object[] expected = TestControlFlow.TestBooleanBranches();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestExceptionHandlingBranches()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestControlFlow), nameof(TestControlFlow.TestExceptionHandlingBranches));

            // Call original
            object[] expected = TestControlFlow.TestExceptionHandlingBranches();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestNestedBranching()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestControlFlow), nameof(TestControlFlow.TestNestedBranching));

            // Call original
            object[] expected = TestControlFlow.TestNestedBranching();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestUnconditionalBranches()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestControlFlow), nameof(TestControlFlow.TestUnconditionalBranches));

            // Call original
            object[] expected = TestControlFlow.TestUnconditionalBranches();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestSwitchJumpTable()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestControlFlow), nameof(TestControlFlow.TestSwitchJumpTable));

            // Call original
            object[] expected = TestControlFlow.TestSwitchJumpTable();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
#endif