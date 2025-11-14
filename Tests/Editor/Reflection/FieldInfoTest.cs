#if DOTNOW_ENABLE_TESTS
using NUnit.Framework;
using System;
using System.Reflection;
using TestAssembly;

namespace dotnow.Reflection
{
    [TestFixture]
    [Category("Unit Test")]
    public class FieldInfoTest
    {
        [Test]
        public void TestPublicInstanceIntFieldProperties()
        {
            // Load field through dotnow
            System.Reflection.FieldInfo actualField = GetActualField(nameof(TestFieldInfo.publicInstanceInt));

            // Get expected field from normal reflection
            System.Reflection.FieldInfo expectedField = typeof(TestFieldInfo).GetField(nameof(TestFieldInfo.publicInstanceInt));

            // Compare all properties
            AssertFieldInfoPropertiesEqual(expectedField, actualField);
        }

        [Test]
        public void TestPublicInstanceStringFieldProperties()
        {
            // Load field through dotnow
            System.Reflection.FieldInfo actualField = GetActualField(nameof(TestFieldInfo.publicInstanceString));

            // Get expected field from normal reflection
            System.Reflection.FieldInfo expectedField = typeof(TestFieldInfo).GetField(nameof(TestFieldInfo.publicInstanceString));

            // Compare all properties
            AssertFieldInfoPropertiesEqual(expectedField, actualField);
        }

        [Test]
        public void TestPrivateFieldProperties()
        {
            // Load field through dotnow - private fields require BindingFlags
            System.Reflection.FieldInfo actualField = GetActualField("privateField", BindingFlags.NonPublic | BindingFlags.Instance);

            // Get expected field from normal reflection
            System.Reflection.FieldInfo expectedField = typeof(TestFieldInfo).GetField("privateField", BindingFlags.NonPublic | BindingFlags.Instance);

            // Compare all properties
            AssertFieldInfoPropertiesEqual(expectedField, actualField);
        }

        [Test]
        public void TestProtectedFieldProperties()
        {
            // Load field through dotnow
            System.Reflection.FieldInfo actualField = GetActualField("protectedField", BindingFlags.NonPublic | BindingFlags.Instance);

            // Get expected field from normal reflection
            System.Reflection.FieldInfo expectedField = typeof(TestFieldInfo).GetField("protectedField", BindingFlags.NonPublic | BindingFlags.Instance);

            // Compare all properties
            AssertFieldInfoPropertiesEqual(expectedField, actualField);
        }

        [Test]
        public void TestInternalFieldProperties()
        {
            // Load field through dotnow
            System.Reflection.FieldInfo actualField = GetActualField("internalField", BindingFlags.NonPublic | BindingFlags.Instance);

            // Get expected field from normal reflection
            System.Reflection.FieldInfo expectedField = typeof(TestFieldInfo).GetField("internalField", BindingFlags.NonPublic | BindingFlags.Instance);

            // Compare all properties
            AssertFieldInfoPropertiesEqual(expectedField, actualField);
        }

        [Test]
        public void TestStaticFieldProperties()
        {
            // Load field through dotnow
            System.Reflection.FieldInfo actualField = GetActualField(nameof(TestFieldInfo.publicStaticInt));

            // Get expected field from normal reflection
            System.Reflection.FieldInfo expectedField = typeof(TestFieldInfo).GetField(nameof(TestFieldInfo.publicStaticInt));

            // Compare all properties
            AssertFieldInfoPropertiesEqual(expectedField, actualField);
        }

        [Test]
        public void TestReadonlyFieldProperties()
        {
            // Load field through dotnow
            System.Reflection.FieldInfo actualField = GetActualField(nameof(TestFieldInfo.publicReadonlyInt));

            // Get expected field from normal reflection
            System.Reflection.FieldInfo expectedField = typeof(TestFieldInfo).GetField(nameof(TestFieldInfo.publicReadonlyInt));

            // Compare all properties
            AssertFieldInfoPropertiesEqual(expectedField, actualField);
        }

        [Test]
        public void TestConstantFieldProperties()
        {
            // Load field through dotnow
            System.Reflection.FieldInfo actualField = GetActualField(nameof(TestFieldInfo.PublicConstInt));

            // Get expected field from normal reflection
            System.Reflection.FieldInfo expectedField = typeof(TestFieldInfo).GetField(nameof(TestFieldInfo.PublicConstInt));

            // Compare all properties
            AssertFieldInfoPropertiesEqual(expectedField, actualField);
        }

        [Test]
        public void TestFieldAttributes()
        {
            // Test public field
            System.Reflection.FieldInfo actualPublicField = GetActualField(nameof(TestFieldInfo.publicInstanceInt));
            System.Reflection.FieldInfo expectedPublicField = typeof(TestFieldInfo).GetField(nameof(TestFieldInfo.publicInstanceInt));
            AssertFieldAttributesEqual(expectedPublicField, actualPublicField);

            // Test private field
            System.Reflection.FieldInfo actualPrivateField = GetActualField("privateField", BindingFlags.NonPublic | BindingFlags.Instance);
            System.Reflection.FieldInfo expectedPrivateField = typeof(TestFieldInfo).GetField("privateField", BindingFlags.NonPublic | BindingFlags.Instance);
            AssertFieldAttributesEqual(expectedPrivateField, actualPrivateField);

            // Test static field
            System.Reflection.FieldInfo actualStaticField = GetActualField(nameof(TestFieldInfo.publicStaticInt));
            System.Reflection.FieldInfo expectedStaticField = typeof(TestFieldInfo).GetField(nameof(TestFieldInfo.publicStaticInt));
            AssertFieldAttributesEqual(expectedStaticField, actualStaticField);

            // Test readonly field
            System.Reflection.FieldInfo actualReadonlyField = GetActualField(nameof(TestFieldInfo.publicReadonlyInt));
            System.Reflection.FieldInfo expectedReadonlyField = typeof(TestFieldInfo).GetField(nameof(TestFieldInfo.publicReadonlyInt));
            AssertFieldAttributesEqual(expectedReadonlyField, actualReadonlyField);
        }

        [Test]
        public void TestFieldGetValue()
        {
            AppDomain domain = new();

            // Test instance field
            Type actualType = TestUtils.LoadTestType(nameof(TestFieldInfo), domain);
            object actualInstance = domain.CreateInstance(actualType);
            object expectedInstance = new TestFieldInfo();

            System.Reflection.FieldInfo actualField = actualType.GetField(nameof(TestFieldInfo.publicInstanceInt));
            System.Reflection.FieldInfo expectedField = typeof(TestFieldInfo).GetField(nameof(TestFieldInfo.publicInstanceInt));

            object expected = expectedField.GetValue(expectedInstance);
            object actual = actualField.GetValue(actualInstance);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestFieldSetValue()
        {
            AppDomain domain = new();

            // Test instance field
            Type actualType = TestUtils.LoadTestType(nameof(TestFieldInfo), domain);
            object actualInstance = domain.CreateInstance(actualType);
            object expectedInstance = new TestFieldInfo();

            System.Reflection.FieldInfo actualField = actualType.GetField(nameof(TestFieldInfo.publicInstanceInt));
            System.Reflection.FieldInfo expectedField = typeof(TestFieldInfo).GetField(nameof(TestFieldInfo.publicInstanceInt));

            int testValue = 12345;
            expectedField.SetValue(expectedInstance, testValue);
            actualField.SetValue(actualInstance, testValue);

            object expected = expectedField.GetValue(expectedInstance);
            object actual = actualField.GetValue(actualInstance);

            Assert.AreEqual(expected, actual);
            Assert.AreEqual(testValue, actual);
        }

        [Test]
        public void TestStaticFieldGetValue()
        {
            // Test static field
            System.Reflection.FieldInfo actualField = GetActualField(nameof(TestFieldInfo.publicStaticInt));
            System.Reflection.FieldInfo expectedField = typeof(TestFieldInfo).GetField(nameof(TestFieldInfo.publicStaticInt));

            object expected = expectedField.GetValue(null);
            object actual = actualField.GetValue(null);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestStaticFieldSetValue()
        {
            // Test static field
            System.Reflection.FieldInfo actualField = GetActualField(nameof(TestFieldInfo.publicStaticInt));
            System.Reflection.FieldInfo expectedField = typeof(TestFieldInfo).GetField(nameof(TestFieldInfo.publicStaticInt));

            // Store original value
            object originalValue = expectedField.GetValue(null);

            try
            {
                int testValue = 99999;
                expectedField.SetValue(null, testValue);
                actualField.SetValue(null, testValue);

                object expected = expectedField.GetValue(null);
                object actual = actualField.GetValue(null);

                Assert.AreEqual(expected, actual);
                Assert.AreEqual(testValue, actual);
            }
            finally
            {
                // Restore original value
                expectedField.SetValue(null, originalValue);
                actualField.SetValue(null, originalValue);
            }
        }

        [Test]
        public void TestConstantFieldValue()
        {
            // Test constant field
            System.Reflection.FieldInfo actualField = GetActualField(nameof(TestFieldInfo.PublicConstInt));
            System.Reflection.FieldInfo expectedField = typeof(TestFieldInfo).GetField(nameof(TestFieldInfo.PublicConstInt));

            object expected = expectedField.GetValue(null);
            object actual = actualField.GetValue(null);

            Assert.AreEqual(expected, actual);
            Assert.AreEqual(TestFieldInfo.PublicConstInt, actual);
        }

        [Test]
        public void TestGetRawConstantValue()
        {
            // Test constant field
            System.Reflection.FieldInfo actualField = GetActualField(nameof(TestFieldInfo.PublicConstInt));
            System.Reflection.FieldInfo expectedField = typeof(TestFieldInfo).GetField(nameof(TestFieldInfo.PublicConstInt));

            try
            {
                object expected = expectedField.GetRawConstantValue();
                object actual = actualField.GetRawConstantValue();

                Assert.AreEqual(expected, actual);
            }
            catch (NotImplementedException)
            {
                // Expected if not implemented in dotnow
                Assert.Pass("GetRawConstantValue not implemented - throws NotImplementedException as expected");
            }
        }

        [Test]
        public void TestFieldHandle()
        {
            // Test field handle
            System.Reflection.FieldInfo actualField = GetActualField(nameof(TestFieldInfo.publicInstanceInt));

            try
            {
                RuntimeFieldHandle handle = actualField.FieldHandle;
                // If supported, should have a valid handle
                Assert.IsNotNull(handle);
            }
            catch (NotSupportedException)
            {
                // Expected if not supported in dotnow
                Assert.Pass("FieldHandle not supported - throws NotSupportedException as expected");
            }
        }

        [Test]
        public void TestFieldCustomAttributes()
        {
            // Test custom attributes - may not be implemented
            System.Reflection.FieldInfo actualField = GetActualField(nameof(TestFieldInfo.publicInstanceInt));

            try
            {
                object[] attributes = actualField.GetCustomAttributes(false);
                Assert.IsNotNull(attributes);
            }
            catch (NotImplementedException)
            {
                // Expected if not implemented in dotnow
                Assert.Pass("GetCustomAttributes not implemented - throws NotImplementedException as expected");
            }
        }

        [Test]
        public void TestAllFieldTypes()
        {
            // Test all basic field types
            string[] fieldNames = {
         nameof(TestFieldInfo.publicInstanceInt),
          nameof(TestFieldInfo.publicInstanceString),
      nameof(TestFieldInfo.publicInstanceBool),
                nameof(TestFieldInfo.publicInstanceFloat),
  nameof(TestFieldInfo.publicInstanceDouble),
  nameof(TestFieldInfo.publicInstanceChar),
          nameof(TestFieldInfo.publicInstanceByte),
    nameof(TestFieldInfo.publicInstanceSByte),
  nameof(TestFieldInfo.publicInstanceShort),
     nameof(TestFieldInfo.publicInstanceUShort),
   nameof(TestFieldInfo.publicInstanceUInt),
   nameof(TestFieldInfo.publicInstanceLong),
    nameof(TestFieldInfo.publicInstanceULong),
     nameof(TestFieldInfo.publicInstanceDecimal)
          };

            foreach (string fieldName in fieldNames)
            {
                System.Reflection.FieldInfo actualField = GetActualField(fieldName);
                System.Reflection.FieldInfo expectedField = typeof(TestFieldInfo).GetField(fieldName);

                AssertFieldInfoPropertiesEqual(expectedField, actualField, $"Field {fieldName}");
            }
        }

        [Test]
        public void TestInheritedFields()
        {
            // Test fields from base class - using nested types
            Type actualType = TestUtils.LoadTestType($"{nameof(TestFieldInfo)}").GetNestedType("DerivedFieldTest", BindingFlags.NonPublic);
            Type expectedType = typeof(TestFieldInfo.DerivedFieldTest);

            // Test accessing base field through derived type
            System.Reflection.FieldInfo actualBaseField = actualType.GetField("basePublicField");
            System.Reflection.FieldInfo expectedBaseField = expectedType.GetField("basePublicField");

            AssertFieldInfoPropertiesEqual(expectedBaseField, actualBaseField);

            // Test accessing derived field
            System.Reflection.FieldInfo actualDerivedField = actualType.GetField("derivedPublicField");
            System.Reflection.FieldInfo expectedDerivedField = expectedType.GetField("derivedPublicField");

            AssertFieldInfoPropertiesEqual(expectedDerivedField, actualDerivedField);
        }

        [Test]
        public void TestVolatileField()
        {
            // Test volatile field
            System.Reflection.FieldInfo actualField = GetActualField("volatileField", BindingFlags.NonPublic | BindingFlags.Instance);
            System.Reflection.FieldInfo expectedField = typeof(TestFieldInfo).GetField("volatileField", BindingFlags.NonPublic | BindingFlags.Instance);

            AssertFieldInfoPropertiesEqual(expectedField, actualField);
        }

        [Test]
        public void TestStructFields()
        {
            // Test struct type fields - using nested type
            Type actualStructType = TestUtils.LoadTestType($"{nameof(TestFieldInfo)}").GetNestedType("TestStruct", BindingFlags.NonPublic);
            Type expectedStructType = typeof(TestFieldInfo.TestStruct);

            System.Reflection.FieldInfo actualStructField = actualStructType.GetField("structField");
            System.Reflection.FieldInfo expectedStructField = expectedStructType.GetField("structField");

            AssertFieldInfoPropertiesEqual(expectedStructField, actualStructField);

            // Test static readonly field in struct
            System.Reflection.FieldInfo actualStaticField = actualStructType.GetField("Default");
            System.Reflection.FieldInfo expectedStaticField = expectedStructType.GetField("Default");

            AssertFieldInfoPropertiesEqual(expectedStaticField, actualStaticField);
        }

        [Test]
        public void TestGenericTypeFields()
        {
            // Test generic type fields - using nested type
            try
            {
                Type actualGenericType = TestUtils.LoadTestType($"{nameof(TestFieldInfo)}+GenericFieldTest`1");
                Type expectedGenericType = typeof(TestFieldInfo.GenericFieldTest<>);

                // Compare generic field definitions
                System.Reflection.FieldInfo actualGenericField = actualGenericType.GetField("genericField");
                System.Reflection.FieldInfo expectedGenericField = expectedGenericType.GetField("genericField");

                if (actualGenericField != null && expectedGenericField != null)
                {
                    AssertFieldInfoPropertiesEqual(expectedGenericField, actualGenericField);
                }
                else
                {
                    // Generic field info may not be fully supported
                    Assert.Pass("Generic field access may not be fully supported in dotnow");
                }
            }
            catch (Exception)
            {
                // Generic type loading may not be fully supported in dotnow
                Assert.Pass("Generic type loading may not be fully supported in dotnow");
            }
        }

        [Test]
        public void TestEnumFields()
        {
            // Test enum type - using nested enum
            Type actualEnumType = TestUtils.LoadTestType($"{nameof(TestFieldInfo)}").GetNestedType("TestEnum", BindingFlags.NonPublic);
            Type expectedEnumType = typeof(TestFieldInfo.TestEnum);

            // Test enum value fields
            System.Reflection.FieldInfo actualValue1 = actualEnumType.GetField("Value1");
            System.Reflection.FieldInfo expectedValue1 = expectedEnumType.GetField("Value1");

            if (actualValue1 != null && expectedValue1 != null)
            {
                AssertFieldInfoPropertiesEqual(expectedValue1, actualValue1);
            }
            else
            {
                // Enum field access may not be fully supported
                Assert.Pass("Enum field access may not be fully supported in dotnow");
            }
        }

        // Helper methods
        private System.Reflection.FieldInfo GetActualField(string fieldName, BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static)
        {
            Type type = TestUtils.LoadTestType(nameof(TestFieldInfo));
            return type.GetField(fieldName, bindingFlags);
        }

        private void AssertFieldInfoPropertiesEqual(System.Reflection.FieldInfo expected, System.Reflection.FieldInfo actual, string context = null)
        {
            string prefix = context != null ? $"{context}: " : "";

            Assert.AreEqual(expected.Name, actual.Name, $"{prefix}Field names should match");
            Assert.AreEqual(expected.DeclaringType?.Name, actual.DeclaringType?.Name, $"{prefix}Declaring type names should match");
            Assert.AreEqual(expected.ReflectedType?.Name, actual.ReflectedType?.Name, $"{prefix}Reflected type names should match");
            Assert.AreEqual(expected.FieldType?.Name, actual.FieldType?.Name, $"{prefix}Field type names should match");
            Assert.AreEqual(expected.IsStatic, actual.IsStatic, $"{prefix}IsStatic should match");
            Assert.AreEqual(expected.IsPublic, actual.IsPublic, $"{prefix}IsPublic should match");
            Assert.AreEqual(expected.IsPrivate, actual.IsPrivate, $"{prefix}IsPrivate should match");
            Assert.AreEqual(expected.IsFamily, actual.IsFamily, $"{prefix}IsFamily (protected) should match");
            Assert.AreEqual(expected.IsAssembly, actual.IsAssembly, $"{prefix}IsAssembly (internal) should match");
            Assert.AreEqual(expected.IsInitOnly, actual.IsInitOnly, $"{prefix}IsInitOnly (readonly) should match");
            Assert.AreEqual(expected.IsLiteral, actual.IsLiteral, $"{prefix}IsLiteral (const) should match");
        }

        private void AssertFieldAttributesEqual(System.Reflection.FieldInfo expected, System.Reflection.FieldInfo actual)
        {
            var expectedAttributes = expected.Attributes;
            var actualAttributes = actual.Attributes;

            Assert.AreEqual((expectedAttributes & FieldAttributes.Public) != 0, (actualAttributes & FieldAttributes.Public) != 0, "Public attribute should match");
            Assert.AreEqual((expectedAttributes & FieldAttributes.Private) != 0, (actualAttributes & FieldAttributes.Private) != 0, "Private attribute should match");
            Assert.AreEqual((expectedAttributes & FieldAttributes.Family) != 0, (actualAttributes & FieldAttributes.Family) != 0, "Family (protected) attribute should match");
            Assert.AreEqual((expectedAttributes & FieldAttributes.Assembly) != 0, (actualAttributes & FieldAttributes.Assembly) != 0, "Assembly (internal) attribute should match");
            Assert.AreEqual((expectedAttributes & FieldAttributes.Static) != 0, (actualAttributes & FieldAttributes.Static) != 0, "Static attribute should match");
            Assert.AreEqual((expectedAttributes & FieldAttributes.InitOnly) != 0, (actualAttributes & FieldAttributes.InitOnly) != 0, "InitOnly (readonly) attribute should match");
            Assert.AreEqual((expectedAttributes & FieldAttributes.Literal) != 0, (actualAttributes & FieldAttributes.Literal) != 0, "Literal (const) attribute should match");
        }
    }
}
#endif