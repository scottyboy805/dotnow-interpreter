using NUnit.Framework;
using System;
using System.Reflection;
using TestAssembly;

namespace dotnow.Common
{
    [TestFixture]
    [Category("Unit Test")]
    public class Struct
    {
        [Test]
        public void TestStructCreation()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestStructMethods), nameof(TestStructMethods.TestStructCreation));

            object[] expected = TestStructMethods.TestStructCreation();
            object[] actual = (object[])method.Invoke(null, null);

            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestStructFieldModification()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestStructMethods), nameof(TestStructMethods.TestStructFieldModification));

            object[] expected = TestStructMethods.TestStructFieldModification();
            object[] actual = (object[])method.Invoke(null, null);

            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestStructMethodCall()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestStructMethods), nameof(TestStructMethods.TestStructMethodCall));

            int expected = TestStructMethods.TestStructMethodCall();
            int actual = (int)method.Invoke(null, null);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestStructMethodModification()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestStructMethods), nameof(TestStructMethods.TestStructMethodModification));

            object[] expected = TestStructMethods.TestStructMethodModification();
            object[] actual = (object[])method.Invoke(null, null);

            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestStructPassByValue()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestStructMethods), nameof(TestStructMethods.TestStructPassByValue));

            object[] expected = TestStructMethods.TestStructPassByValue();
            object[] actual = (object[])method.Invoke(null, null);

            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestStructPassByRef()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestStructMethods), nameof(TestStructMethods.TestStructPassByRef));

            object[] expected = TestStructMethods.TestStructPassByRef();
            object[] actual = (object[])method.Invoke(null, null);

            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestStructOutParameter()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestStructMethods), nameof(TestStructMethods.TestStructOutParameter));

            object[] expected = TestStructMethods.TestStructOutParameter();
            object[] actual = (object[])method.Invoke(null, null);

            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestStructInParameter()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestStructMethods), nameof(TestStructMethods.TestStructInParameter));

            int expected = TestStructMethods.TestStructInParameter();
            int actual = (int)method.Invoke(null, null);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestStructBoxing()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestStructMethods), nameof(TestStructMethods.TestStructBoxing));

            object[] expected = TestStructMethods.TestStructBoxing();
            object[] actual = (object[])method.Invoke(null, null);

            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestStructInArrays()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestStructMethods), nameof(TestStructMethods.TestStructInArrays));

            object[] expected = TestStructMethods.TestStructInArrays();
            object[] actual = (object[])method.Invoke(null, null);

            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestNestedStructs()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestStructMethods), nameof(TestStructMethods.TestNestedStructs));

            object[] expected = TestStructMethods.TestNestedStructs();
            object[] actual = (object[])method.Invoke(null, null);

            // Compare with tolerance for floating point values
            Assert.AreEqual(expected.Length, actual.Length);
            for (int i = 0; i < expected.Length; i++)
            {
                if (expected[i] is float expectedFloat && actual[i] is float actualFloat)
                {
                    Assert.AreEqual(expectedFloat, actualFloat, 0.001f, $"Mismatch at index {i}");
                }
                else
                {
                    Assert.AreEqual(expected[i], actual[i], $"Mismatch at index {i}");
                }
            }
        }

        [Test]
        public void TestStructProperties()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestStructMethods), nameof(TestStructMethods.TestStructProperties));

            float expected = TestStructMethods.TestStructProperties();
            float actual = (float)method.Invoke(null, null);

            Assert.AreEqual(expected, actual, 0.001f);
        }

        [Test]
        public void TestStructDefaultConstructor()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestStructMethods), nameof(TestStructMethods.TestStructDefaultConstructor));

            object[] expected = TestStructMethods.TestStructDefaultConstructor();
            object[] actual = (object[])method.Invoke(null, null);

            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestStructOverrides()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestStructMethods), nameof(TestStructMethods.TestStructOverrides));

            object[] expected = TestStructMethods.TestStructOverrides();
            object[] actual = (object[])method.Invoke(null, null);

            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestStructMutation()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestStructMethods), nameof(TestStructMethods.TestStructMutation));

            object[] expected = TestStructMethods.TestStructMutation();
            object[] actual = (object[])method.Invoke(null, null);

            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestStructAssignment()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestStructMethods), nameof(TestStructMethods.TestStructAssignment));

            object[] expected = TestStructMethods.TestStructAssignment();
            object[] actual = (object[])method.Invoke(null, null);

            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestStructEquality()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestStructMethods), nameof(TestStructMethods.TestStructEquality));

            bool[] expected = TestStructMethods.TestStructEquality();
            bool[] actual = (bool[])method.Invoke(null, null);

            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestStructAsInterface()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestStructMethods), nameof(TestStructMethods.TestStructAsInterface));

            string expected = TestStructMethods.TestStructAsInterface();
            string actual = (string)method.Invoke(null, null);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestStructWithReadonlyBehavior()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestStructMethods), nameof(TestStructMethods.TestStructWithReadonlyBehavior));

            object[] expected = TestStructMethods.TestStructWithReadonlyBehavior();
            object[] actual = (object[])method.Invoke(null, null);

            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestLargeStructHandling()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestStructMethods), nameof(TestStructMethods.TestLargeStructHandling));

            bool expected = TestStructMethods.TestLargeStructHandling();
            bool actual = (bool)method.Invoke(null, null);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestStructInstanceFieldComparison()
        {
            // This test ensures that struct field access works the same way
            // between native and interpreted code
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestStructMethods), nameof(TestStructMethods.TestStructCreation));

            object[] nativeResult = TestStructMethods.TestStructCreation();
            object[] interpretedResult = (object[])method.Invoke(null, null);

            // Verify that the field access results are identical
            Assert.AreEqual(nativeResult.Length, interpretedResult.Length);
            for (int i = 0; i < nativeResult.Length; i++)
            {
                Assert.AreEqual(nativeResult[i], interpretedResult[i], $"Field value mismatch at index {i}");
            }
        }
    }
}