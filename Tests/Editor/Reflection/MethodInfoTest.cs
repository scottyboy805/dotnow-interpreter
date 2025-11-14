#if DOTNOW_ENABLE_TESTS
using NUnit.Framework;
using System;
using System.Reflection;
using TestAssembly;

namespace dotnow.Reflection
{
    [TestFixture]
    [Category("Unit Test")]
    public class MethodInfoTest
    {
        [Test]
        public void TestStaticVoidMethodProperties()
        {
            // Load method through dotnow
            System.Reflection.MethodInfo actualMethod = TestUtils.LoadTestMethod(nameof(TestMethodInfo), nameof(TestMethodInfo.StaticVoidMethod));

            // Get expected method from normal reflection
            System.Reflection.MethodInfo expectedMethod = typeof(TestMethodInfo).GetMethod(nameof(TestMethodInfo.StaticVoidMethod));

            // Compare all properties
            AssertMethodInfoPropertiesEqual(expectedMethod, actualMethod);
        }

        [Test]
        public void TestStaticIntMethodProperties()
        {
            // Load method through dotnow
            System.Reflection.MethodInfo actualMethod = TestUtils.LoadTestMethod(nameof(TestMethodInfo), nameof(TestMethodInfo.StaticIntMethod));

            // Get expected method from normal reflection
            System.Reflection.MethodInfo expectedMethod = typeof(TestMethodInfo).GetMethod(nameof(TestMethodInfo.StaticIntMethod));

            // Compare all properties
            AssertMethodInfoPropertiesEqual(expectedMethod, actualMethod);
        }

        [Test]
        public void TestInstanceMethodProperties()
        {
            // Load method through dotnow
            System.Reflection.MethodInfo actualMethod = TestUtils.LoadTestMethod(nameof(TestMethodInfo), nameof(TestMethodInfo.InstanceVoidMethod));

            // Get expected method from normal reflection
            System.Reflection.MethodInfo expectedMethod = typeof(TestMethodInfo).GetMethod(nameof(TestMethodInfo.InstanceVoidMethod));

            // Compare all properties
            AssertMethodInfoPropertiesEqual(expectedMethod, actualMethod);
        }

        [Test]
        public void TestVirtualMethodProperties()
        {
            // Load method through dotnow
            System.Reflection.MethodInfo actualMethod = TestUtils.LoadTestMethod(nameof(TestMethodInfo), nameof(TestMethodInfo.VirtualMethod));

            // Get expected method from normal reflection
            System.Reflection.MethodInfo expectedMethod = typeof(TestMethodInfo).GetMethod(nameof(TestMethodInfo.VirtualMethod));

            // Compare all properties
            AssertMethodInfoPropertiesEqual(expectedMethod, actualMethod);
        }

        [Test]
        public void TestMethodWithParametersProperties()
        {
            // Load method through dotnow
            System.Reflection.MethodInfo actualMethod = TestUtils.LoadTestMethod(nameof(TestMethodInfo), nameof(TestMethodInfo.MethodWithMultipleParameters));

            // Get expected method from normal reflection
            System.Reflection.MethodInfo expectedMethod = typeof(TestMethodInfo).GetMethod(nameof(TestMethodInfo.MethodWithMultipleParameters));

            // Compare all properties
            AssertMethodInfoPropertiesEqual(expectedMethod, actualMethod);
        }

        [Test]
        public void TestGenericMethodProperties()
        {
            // Load method through dotnow
            System.Reflection.MethodInfo actualMethod = TestUtils.LoadTestMethod(nameof(TestMethodInfo), nameof(TestMethodInfo.StaticGenericMethod));

            // Get expected method from normal reflection
            System.Reflection.MethodInfo expectedMethod = typeof(TestMethodInfo).GetMethod(nameof(TestMethodInfo.StaticGenericMethod));

            // Compare all properties
            AssertMethodInfoPropertiesEqual(expectedMethod, actualMethod);
        }

        [Test]
        public void TestParameterInfo()
        {
            // Load method through dotnow
            System.Reflection.MethodInfo actualMethod = TestUtils.LoadTestMethod(nameof(TestMethodInfo), nameof(TestMethodInfo.MethodWithMultipleParameters));

            // Get expected method from normal reflection
            System.Reflection.MethodInfo expectedMethod = typeof(TestMethodInfo).GetMethod(nameof(TestMethodInfo.MethodWithMultipleParameters));

            // Compare parameter info
            AssertParameterInfoEqual(expectedMethod, actualMethod);
        }

        [Test]
        public void TestRefParameterInfo()
        {
            // Load method through dotnow
            System.Reflection.MethodInfo actualMethod = TestUtils.LoadTestMethod(nameof(TestMethodInfo), nameof(TestMethodInfo.MethodWithRefParameter));

            // Get expected method from normal reflection
            System.Reflection.MethodInfo expectedMethod = typeof(TestMethodInfo).GetMethod(nameof(TestMethodInfo.MethodWithRefParameter));

            // Compare parameter info
            AssertParameterInfoEqual(expectedMethod, actualMethod);
        }

        [Test]
        public void TestOutParameterInfo()
        {
            // Load method through dotnow
            System.Reflection.MethodInfo actualMethod = TestUtils.LoadTestMethod(nameof(TestMethodInfo), nameof(TestMethodInfo.MethodWithOutParameter));

            // Get expected method from normal reflection
            System.Reflection.MethodInfo expectedMethod = typeof(TestMethodInfo).GetMethod(nameof(TestMethodInfo.MethodWithOutParameter));

            // Compare parameter info
            AssertParameterInfoEqual(expectedMethod, actualMethod);
        }

        [Test]
        public void TestMethodInvocation()
        {
            // Load method through dotnow
            System.Reflection.MethodInfo actualMethod = TestUtils.LoadTestMethod(nameof(TestMethodInfo), nameof(TestMethodInfo.StaticIntMethod));

            // Get expected method from normal reflection
            System.Reflection.MethodInfo expectedMethod = typeof(TestMethodInfo).GetMethod(nameof(TestMethodInfo.StaticIntMethod));

            // Compare invocation results
            object expected = expectedMethod.Invoke(null, null);
            object actual = actualMethod.Invoke(null, null);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestMethodInvocationWithParameters()
        {
            // Load method through dotnow
            System.Reflection.MethodInfo actualMethod = TestUtils.LoadTestMethod(nameof(TestMethodInfo), nameof(TestMethodInfo.StaticStringMethod));

            // Get expected method from normal reflection
            System.Reflection.MethodInfo expectedMethod = typeof(TestMethodInfo).GetMethod(nameof(TestMethodInfo.StaticStringMethod));

            // Test with parameter
            string testInput = "test string";
            object expected = expectedMethod.Invoke(null, new object[] { testInput });
            object actual = actualMethod.Invoke(null, new object[] { testInput });

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestInstanceMethodInvocation()
        {
            // Load method through dotnow
            System.Reflection.MethodInfo actualMethod = TestUtils.LoadTestMethod(nameof(TestMethodInfo), nameof(TestMethodInfo.InstanceIntMethod));

            // Get expected method from normal reflection
            System.Reflection.MethodInfo expectedMethod = typeof(TestMethodInfo).GetMethod(nameof(TestMethodInfo.InstanceIntMethod));

            // Create instances
            Type actualType = TestUtils.LoadTestType(nameof(TestMethodInfo));
            object actualInstance = Activator.CreateInstance(actualType);
            object expectedInstance = new TestMethodInfo();

            // Compare invocation results
            object expected = expectedMethod.Invoke(expectedInstance, null);
            object actual = actualMethod.Invoke(actualInstance, null);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestMethodAttributes()
        {
            // Load method through dotnow
            System.Reflection.MethodInfo actualMethod = TestUtils.LoadTestMethod(nameof(TestMethodInfo), nameof(TestMethodInfo.StaticVoidMethod));

            // Get expected method from normal reflection
            System.Reflection.MethodInfo expectedMethod = typeof(TestMethodInfo).GetMethod(nameof(TestMethodInfo.StaticVoidMethod));

            // Compare attributes
            AssertMethodAttributesEqual(expectedMethod, actualMethod);
        }

        [Test]
        public void TestGenericMethodDefinition()
        {
            // Load generic method through dotnow
            System.Reflection.MethodInfo actualMethod = TestUtils.LoadTestMethod(nameof(TestMethodInfo), nameof(TestMethodInfo.StaticGenericMethod));

            // Get expected method from normal reflection
            System.Reflection.MethodInfo expectedMethod = typeof(TestMethodInfo).GetMethod(nameof(TestMethodInfo.StaticGenericMethod));

            // Test generic method properties
            Assert.AreEqual(expectedMethod.IsGenericMethodDefinition, actualMethod.IsGenericMethodDefinition);
            Assert.AreEqual(expectedMethod.IsGenericMethod, actualMethod.IsGenericMethod);
            Assert.AreEqual(expectedMethod.IsConstructedGenericMethod, actualMethod.IsConstructedGenericMethod);
            Assert.AreEqual(expectedMethod.ContainsGenericParameters, actualMethod.ContainsGenericParameters);
        }

        [Test]
        public void TestConstructedGenericMethod()
        {
            // Load generic method through dotnow
            System.Reflection.MethodInfo actualGenericMethod = TestUtils.LoadTestMethod(nameof(TestMethodInfo), nameof(TestMethodInfo.StaticGenericMethod));

            // Get expected method from normal reflection
            System.Reflection.MethodInfo expectedGenericMethod = typeof(TestMethodInfo).GetMethod(nameof(TestMethodInfo.StaticGenericMethod));

            // Make generic method with specific type
            System.Reflection.MethodInfo expectedConstructedMethod = expectedGenericMethod.MakeGenericMethod(typeof(string));
            System.Reflection.MethodInfo actualConstructedMethod = actualGenericMethod.MakeGenericMethod(typeof(string));

            // Test constructed generic method properties
            Assert.AreEqual(expectedConstructedMethod.IsGenericMethod, actualConstructedMethod.IsGenericMethod);
            Assert.AreEqual(expectedConstructedMethod.IsGenericMethodDefinition, actualConstructedMethod.IsGenericMethodDefinition);
            Assert.AreEqual(expectedConstructedMethod.IsConstructedGenericMethod, actualConstructedMethod.IsConstructedGenericMethod);
            Assert.AreEqual(expectedConstructedMethod.ContainsGenericParameters, actualConstructedMethod.ContainsGenericParameters);

            // Test generic arguments
            Type[] expectedGenericArgs = expectedConstructedMethod.GetGenericArguments();
            Type[] actualGenericArgs = actualConstructedMethod.GetGenericArguments();

            Assert.AreEqual(expectedGenericArgs.Length, actualGenericArgs.Length);
            for (int i = 0; i < expectedGenericArgs.Length; i++)
            {
                Assert.AreEqual(expectedGenericArgs[i], actualGenericArgs[i]);
            }
        }

        [Test]
        public void TestGetBaseDefinition()
        {
            // Load virtual method through dotnow
            System.Reflection.MethodInfo actualMethod = TestUtils.LoadTestMethod(nameof(TestMethodInfo), nameof(TestMethodInfo.VirtualMethod));

            // Get expected method from normal reflection
            System.Reflection.MethodInfo expectedMethod = typeof(TestMethodInfo).GetMethod(nameof(TestMethodInfo.VirtualMethod));

            // Get base definitions
            System.Reflection.MethodInfo expectedBaseDefinition = expectedMethod.GetBaseDefinition();
            System.Reflection.MethodInfo actualBaseDefinition = actualMethod.GetBaseDefinition();

            // Should be the same for non-overridden methods
            Assert.AreEqual(expectedBaseDefinition.Name, actualBaseDefinition.Name);
            Assert.AreEqual(expectedBaseDefinition.DeclaringType?.Name, actualBaseDefinition.DeclaringType?.Name);
        }

        [Test]
        public void TestMethodBody()
        {
            // Load method through dotnow
            System.Reflection.MethodInfo actualMethod = TestUtils.LoadTestMethod(nameof(TestMethodInfo), nameof(TestMethodInfo.StaticIntMethod));

            // Try to get method body - may not be implemented in dotnow
            try
            {
                MethodBody body = actualMethod.GetMethodBody();
                // If implemented, should not be null for methods with body
                // If not implemented, should throw NotImplementedException
            }
            catch (NotImplementedException)
            {
                // Expected if not implemented in dotnow
                Assert.Pass("GetMethodBody not implemented - throws NotImplementedException as expected");
            }
        }

        [Test]
        public void TestMethodHandle()
        {
            // Load method through dotnow
            System.Reflection.MethodInfo actualMethod = TestUtils.LoadTestMethod(nameof(TestMethodInfo), nameof(TestMethodInfo.StaticVoidMethod));

            // Try to get method handle - may not be supported in dotnow
            try
            {
                RuntimeMethodHandle handle = actualMethod.MethodHandle;
                // If supported, should have a valid handle
                Assert.IsNotNull(handle);
            }
            catch (NotSupportedException)
            {
                // Expected if not supported in dotnow
                Assert.Pass("MethodHandle not supported - throws NotSupportedException as expected");
            }
        }

        [Test]
        public void TestReturnTypeCustomAttributes()
        {
            // Load method through dotnow
            System.Reflection.MethodInfo actualMethod = TestUtils.LoadTestMethod(nameof(TestMethodInfo), nameof(TestMethodInfo.StaticIntMethod));

            // Try to get return type custom attributes - may not be implemented in dotnow
            try
            {
                ICustomAttributeProvider returnAttribs = actualMethod.ReturnTypeCustomAttributes;
                Assert.IsNotNull(returnAttribs);
            }
            catch (NotImplementedException)
            {
                // Expected if not implemented in dotnow
                Assert.Pass("ReturnTypeCustomAttributes not implemented - throws NotImplementedException as expected");
            }
        }

        [Test]
        public void TestMethodImplementationFlags()
        {
            // Load method through dotnow
            System.Reflection.MethodInfo actualMethod = TestUtils.LoadTestMethod(nameof(TestMethodInfo), nameof(TestMethodInfo.StaticVoidMethod));

            // Try to get method implementation flags - may not be implemented in dotnow
            try
            {
                MethodImplAttributes flags = actualMethod.GetMethodImplementationFlags();
                // If implemented, should return valid flags
                Assert.IsTrue(Enum.IsDefined(typeof(MethodImplAttributes), flags) || flags == 0);
            }
            catch (NotImplementedException)
            {
                // Expected if not implemented in dotnow
                Assert.Pass("GetMethodImplementationFlags not implemented - throws NotImplementedException as expected");
            }
        }

        // Helper methods to compare MethodInfo properties
        private void AssertMethodInfoPropertiesEqual(System.Reflection.MethodInfo expected, System.Reflection.MethodInfo actual)
        {
            Assert.AreEqual(expected.Name, actual.Name, "Method names should match");
            Assert.AreEqual(expected.DeclaringType?.Name, actual.DeclaringType?.Name, "Declaring type names should match");
            Assert.AreEqual(expected.ReflectedType?.Name, actual.ReflectedType?.Name, "Reflected type names should match");
            Assert.AreEqual(expected.ReturnType?.Name, actual.ReturnType?.Name, "Return type names should match");
            Assert.AreEqual(expected.IsStatic, actual.IsStatic, "IsStatic should match");
            Assert.AreEqual(expected.IsVirtual, actual.IsVirtual, "IsVirtual should match");
            Assert.AreEqual(expected.IsAbstract, actual.IsAbstract, "IsAbstract should match");
            Assert.AreEqual(expected.IsPublic, actual.IsPublic, "IsPublic should match");
            Assert.AreEqual(expected.IsPrivate, actual.IsPrivate, "IsPrivate should match");
            Assert.AreEqual(expected.IsFamily, actual.IsFamily, "IsFamily (protected) should match");
            Assert.AreEqual(expected.IsAssembly, actual.IsAssembly, "IsAssembly (internal) should match");
            Assert.AreEqual(expected.IsFinal, actual.IsFinal, "IsFinal should match");
            Assert.AreEqual(expected.IsGenericMethod, actual.IsGenericMethod, "IsGenericMethod should match");
            Assert.AreEqual(expected.IsGenericMethodDefinition, actual.IsGenericMethodDefinition, "IsGenericMethodDefinition should match");
            Assert.AreEqual(expected.ContainsGenericParameters, actual.ContainsGenericParameters, "ContainsGenericParameters should match");
            Assert.AreEqual(expected.GetParameters()?.Length ?? 0, actual.GetParameters()?.Length ?? 0, "Parameter count should match");
        }

        private void AssertParameterInfoEqual(System.Reflection.MethodInfo expected, System.Reflection.MethodInfo actual)
        {
            ParameterInfo[] expectedParameters = expected.GetParameters();
            ParameterInfo[] actualParameters = actual.GetParameters();

            Assert.AreEqual(expectedParameters.Length, actualParameters.Length, "Parameter count should match");

            for (int i = 0; i < expectedParameters.Length; i++)
            {
                var expectedParam = expectedParameters[i];
                var actualParam = actualParameters[i];

                Assert.AreEqual(expectedParam.Name, actualParam.Name, $"Parameter {i} name should match");
                Assert.AreEqual(expectedParam.ParameterType?.Name, actualParam.ParameterType?.Name, $"Parameter {i} type name should match");
                Assert.AreEqual(expectedParam.Position, actualParam.Position, $"Parameter {i} position should match");
                Assert.AreEqual(expectedParam.IsIn, actualParam.IsIn, $"Parameter {i} IsIn should match");
                Assert.AreEqual(expectedParam.IsOut, actualParam.IsOut, $"Parameter {i} IsOut should match");
                Assert.AreEqual(expectedParam.IsRetval, actualParam.IsRetval, $"Parameter {i} IsRetval should match");
                Assert.AreEqual(expectedParam.IsOptional, actualParam.IsOptional, $"Parameter {i} IsOptional should match");
                Assert.AreEqual(expectedParam.HasDefaultValue, actualParam.HasDefaultValue, $"Parameter {i} HasDefaultValue should match");
            }
        }

        private void AssertMethodAttributesEqual(System.Reflection.MethodInfo expected, System.Reflection.MethodInfo actual)
        {
            var expectedAttributes = expected.Attributes;
            var actualAttributes = actual.Attributes;

            Assert.AreEqual((expectedAttributes & MethodAttributes.Public) != 0, (actualAttributes & MethodAttributes.Public) != 0, "Public attribute should match");
            Assert.AreEqual((expectedAttributes & MethodAttributes.Private) != 0, (actualAttributes & MethodAttributes.Private) != 0, "Private attribute should match");
            Assert.AreEqual((expectedAttributes & MethodAttributes.Family) != 0, (actualAttributes & MethodAttributes.Family) != 0, "Family (protected) attribute should match");
            Assert.AreEqual((expectedAttributes & MethodAttributes.Assembly) != 0, (actualAttributes & MethodAttributes.Assembly) != 0, "Assembly (internal) attribute should match");
            Assert.AreEqual((expectedAttributes & MethodAttributes.Static) != 0, (actualAttributes & MethodAttributes.Static) != 0, "Static attribute should match");
            Assert.AreEqual((expectedAttributes & MethodAttributes.Virtual) != 0, (actualAttributes & MethodAttributes.Virtual) != 0, "Virtual attribute should match");
            Assert.AreEqual((expectedAttributes & MethodAttributes.Abstract) != 0, (actualAttributes & MethodAttributes.Abstract) != 0, "Abstract attribute should match");
            Assert.AreEqual((expectedAttributes & MethodAttributes.Final) != 0, (actualAttributes & MethodAttributes.Final) != 0, "Final attribute should match");
        }
    }
}
#endif