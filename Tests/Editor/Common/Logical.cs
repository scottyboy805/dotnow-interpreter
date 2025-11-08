//using NUnit.Framework;
//using System.Reflection;
//using TestAssembly;

//namespace dotnow.Common
//{
//    [TestFixture]
//    [Category("Unit Test")]
//    public class Logical
//    {
//        [Test]
//  public void TestLogicalAnd()
//  {
//            // Try to load method
//          MethodInfo method = TestUtils.LoadTestMethod(nameof(TestLogical), nameof(TestLogical.TestLogicalAnd));

//          // Call original
//          object[] expected = TestLogical.TestLogicalAnd();
//        object[] actual = (object[])method.Invoke(null, null);

//    // Check for equal elements
//            CollectionAssert.AreEqual(expected, actual);
//   }

//        [Test]
//  public void TestLogicalOr()
//        {
// // Try to load method
//      MethodInfo method = TestUtils.LoadTestMethod(nameof(TestLogical), nameof(TestLogical.TestLogicalOr));

//       // Call original
//         object[] expected = TestLogical.TestLogicalOr();
//            object[] actual = (object[])method.Invoke(null, null);

//            // Check for equal elements
//      CollectionAssert.AreEqual(expected, actual);
//        }

//        [Test]
//        public void TestLogicalNot()
// {
//            // Try to load method
//            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestLogical), nameof(TestLogical.TestLogicalNot));

//        // Call original
//            object[] expected = TestLogical.TestLogicalNot();
//            object[] actual = (object[])method.Invoke(null, null);

//            // Check for equal elements
// CollectionAssert.AreEqual(expected, actual);
//      }

//        [Test]
//        public void TestShortCircuitAnd()
// {
//     // Try to load method
//       MethodInfo method = TestUtils.LoadTestMethod(nameof(TestLogical), nameof(TestLogical.TestShortCircuitAnd));

//  // Call original
//            object[] expected = TestLogical.TestShortCircuitAnd();
//   object[] actual = (object[])method.Invoke(null, null);

//            // Check for equal elements
//            CollectionAssert.AreEqual(expected, actual);
//        }

//    [Test]
//   public void TestShortCircuitOr()
//        {
//            // Try to load method
//    MethodInfo method = TestUtils.LoadTestMethod(nameof(TestLogical), nameof(TestLogical.TestShortCircuitOr));

//      // Call original
//      object[] expected = TestLogical.TestShortCircuitOr();
//            object[] actual = (object[])method.Invoke(null, null);

//  // Check for equal elements
//            CollectionAssert.AreEqual(expected, actual);
//        }

//     [Test]
// public void TestConditional()
//        {
//            // Try to load method
//   MethodInfo method = TestUtils.LoadTestMethod(nameof(TestLogical), nameof(TestLogical.TestConditional));

// // Call original
//     object[] expected = TestLogical.TestConditional();
//      object[] actual = (object[])method.Invoke(null, null);

//     // Check for equal elements
//   CollectionAssert.AreEqual(expected, actual);
//        }

//        [Test]
// public void TestComplexLogicalBranches()
//        {
//            // Try to load method
//            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestLogical), nameof(TestLogical.TestComplexLogicalBranches));

//  // Call original
//          object[] expected = TestLogical.TestComplexLogicalBranches();
//            object[] actual = (object[])method.Invoke(null, null);

//            // Check for equal elements
//         CollectionAssert.AreEqual(expected, actual);
//}

//        [Test]
//        public void TestLogicalBranchInstructions()
//        {
//          // Try to load method
//            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestLogical), nameof(TestLogical.TestLogicalBranchInstructions));

//        // Call original
//    object[] expected = TestLogical.TestLogicalBranchInstructions();
//            object[] actual = (object[])method.Invoke(null, null);

//            // Check for equal elements
//            CollectionAssert.AreEqual(expected, actual);
//        }

//        [Test]
//        public void TestLogicalWithDataTypes()
//        {
//      // Try to load method
//   MethodInfo method = TestUtils.LoadTestMethod(nameof(TestLogical), nameof(TestLogical.TestLogicalWithDataTypes));

//        // Call original
//            object[] expected = TestLogical.TestLogicalWithDataTypes();
//        object[] actual = (object[])method.Invoke(null, null);

//            // Check for equal elements
// CollectionAssert.AreEqual(expected, actual);
//     }
//    }
//}