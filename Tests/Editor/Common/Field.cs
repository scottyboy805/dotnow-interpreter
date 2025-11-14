#if DOTNOW_ENABLE_TESTS
using NUnit.Framework;
using System;
using System.Linq;
using System.Reflection;
using TestAssembly;

namespace dotnow.Common
{
    [TestFixture]
    [Category("Unit Test")]
    public class Field
    {
        [Test]
        public void TestAllFields()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestField), nameof(TestField.TestFieldsAssign));

            object instA = TestField.TestFieldsAssign();
            object instB = method.Invoke(null, null);

            CheckInstanceFields(instA, instB);
        }

        [Test]
        public void TestInstanceIntField()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestField), nameof(TestField.TestReadInstanceInt));

            object expected = TestField.TestReadInstanceInt();
            object actual = method.Invoke(null, null);

            Assert.AreEqual(expected, actual, "Instance int field read mismatch");
        }

        [Test]
        public void TestInstanceFloatField()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestField), nameof(TestField.TestReadInstanceFloat));

            object expected = TestField.TestReadInstanceFloat();
            object actual = method.Invoke(null, null);

            Assert.AreEqual(expected, actual, "Instance float field read mismatch");
        }

        [Test]
        public void TestInstanceStringField()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestField), nameof(TestField.TestReadInstanceString));

            object expected = TestField.TestReadInstanceString();
            object actual = method.Invoke(null, null);

            Assert.AreEqual(expected, actual, "Instance string field read mismatch");
        }

        [Test]
        public void TestInstanceBoolField()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestField), nameof(TestField.TestReadInstanceBool));

            object expected = TestField.TestReadInstanceBool();
            object actual = method.Invoke(null, null);

            Assert.AreEqual(expected, actual, "Instance bool field read mismatch");
        }

        [Test]
        public void TestInstanceDoubleField()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestField), nameof(TestField.TestReadInstanceDouble));

            object expected = TestField.TestReadInstanceDouble();
            object actual = method.Invoke(null, null);

            Assert.AreEqual(expected, actual, "Instance double field read mismatch");
        }

        [Test]
        public void TestInstanceDecimalField()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestField), nameof(TestField.TestReadInstanceDecimal));

            object expected = TestField.TestReadInstanceDecimal();
            object actual = method.Invoke(null, null);

            Assert.AreEqual(expected, actual, "Instance decimal field read mismatch");
        }

        [Test]
        public void TestInstanceCharField()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestField), nameof(TestField.TestReadInstanceChar));

            object expected = TestField.TestReadInstanceChar();
            object actual = method.Invoke(null, null);

            Assert.AreEqual(expected, actual, "Instance char field read mismatch");
        }

        [Test]
        public void TestInstanceByteField()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestField), nameof(TestField.TestReadInstanceByte));

            object expected = TestField.TestReadInstanceByte();
            object actual = method.Invoke(null, null);

            Assert.AreEqual(expected, actual, "Instance byte field read mismatch");
        }

        [Test]
        public void TestInstanceSByteField()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestField), nameof(TestField.TestReadInstanceSByte));

            object expected = TestField.TestReadInstanceSByte();
            object actual = method.Invoke(null, null);

            Assert.AreEqual(expected, actual, "Instance sbyte field read mismatch");
        }

        [Test]
        public void TestInstanceShortField()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestField), nameof(TestField.TestReadInstanceShort));

            object expected = TestField.TestReadInstanceShort();
            object actual = method.Invoke(null, null);

            Assert.AreEqual(expected, actual, "Instance short field read mismatch");
        }

        [Test]
        public void TestInstanceUShortField()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestField), nameof(TestField.TestReadInstanceUShort));

            object expected = TestField.TestReadInstanceUShort();
            object actual = method.Invoke(null, null);

            Assert.AreEqual(expected, actual, "Instance ushort field read mismatch");
        }

        [Test]
        public void TestInstanceUIntField()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestField), nameof(TestField.TestReadInstanceUInt));

            object expected = TestField.TestReadInstanceUInt();
            object actual = method.Invoke(null, null);

            Assert.AreEqual(expected, actual, "Instance uint field read mismatch");
        }

        [Test]
        public void TestInstanceLongField()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestField), nameof(TestField.TestReadInstanceLong));

            object expected = TestField.TestReadInstanceLong();
            object actual = method.Invoke(null, null);

            Assert.AreEqual(expected, actual, "Instance long field read mismatch");
        }

        [Test]
        public void TestInstanceULongField()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestField), nameof(TestField.TestReadInstanceULong));

            object expected = TestField.TestReadInstanceULong();
            object actual = method.Invoke(null, null);

            Assert.AreEqual(expected, actual, "Instance ulong field read mismatch");
        }

        [Test]
        public void TestWriteInstanceIntField()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestField), nameof(TestField.TestWriteInstanceInt));

            object expected = TestField.TestWriteInstanceInt();
            object actual = method.Invoke(null, null);

            Assert.AreEqual(expected, actual, "Instance int field write mismatch");
        }

        [Test]
        public void TestWriteInstanceFloatField()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestField), nameof(TestField.TestWriteInstanceFloat));

            object expected = TestField.TestWriteInstanceFloat();
            object actual = method.Invoke(null, null);

            Assert.AreEqual(expected, actual, "Instance float field write mismatch");
        }

        [Test]
        public void TestWriteInstanceBoolField()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestField), nameof(TestField.TestWriteInstanceBool));

            object expected = TestField.TestWriteInstanceBool();
            object actual = method.Invoke(null, null);

            Assert.AreEqual(expected, actual, "Instance bool field write mismatch");
        }

        [Test]
        public void TestWriteInstanceStringField()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestField), nameof(TestField.TestWriteInstanceString));

            object expected = TestField.TestWriteInstanceString();
            object actual = method.Invoke(null, null);

            Assert.AreEqual(expected, actual, "Instance string field write mismatch");
        }

        [Test]
        public void TestWriteInstanceDoubleField()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestField), nameof(TestField.TestWriteInstanceDouble));

            object expected = TestField.TestWriteInstanceDouble();
            object actual = method.Invoke(null, null);

            Assert.AreEqual(expected, actual, "Instance double field write mismatch");
        }

        [Test]
        public void TestWriteInstanceDecimalField()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestField), nameof(TestField.TestWriteInstanceDecimal));

            object expected = TestField.TestWriteInstanceDecimal();
            object actual = method.Invoke(null, null);

            Assert.AreEqual(expected, actual, "Instance decimal field write mismatch");
        }

        [Test]
        public void TestWriteInstanceCharField()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestField), nameof(TestField.TestWriteInstanceChar));

            object expected = TestField.TestWriteInstanceChar();
            object actual = method.Invoke(null, null);

            Assert.AreEqual(expected, actual, "Instance char field write mismatch");
        }

        [Test]
        public void TestWriteInstanceByteField()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestField), nameof(TestField.TestWriteInstanceByte));

            object expected = TestField.TestWriteInstanceByte();
            object actual = method.Invoke(null, null);

            Assert.AreEqual(expected, actual, "Instance byte field write mismatch");
        }

        [Test]
        public void TestWriteInstanceSByteField()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestField), nameof(TestField.TestWriteInstanceSByte));

            object expected = TestField.TestWriteInstanceSByte();
            object actual = method.Invoke(null, null);

            Assert.AreEqual(expected, actual, "Instance sbyte field write mismatch");
        }

        [Test]
        public void TestWriteInstanceShortField()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestField), nameof(TestField.TestWriteInstanceShort));

            object expected = TestField.TestWriteInstanceShort();
            object actual = method.Invoke(null, null);

            Assert.AreEqual(expected, actual, "Instance short field write mismatch");
        }

        [Test]
        public void TestWriteInstanceUShortField()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestField), nameof(TestField.TestWriteInstanceUShort));

            object expected = TestField.TestWriteInstanceUShort();
            object actual = method.Invoke(null, null);

            Assert.AreEqual(expected, actual, "Instance ushort field write mismatch");
        }

        [Test]
        public void TestWriteInstanceUIntField()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestField), nameof(TestField.TestWriteInstanceUInt));

            object expected = TestField.TestWriteInstanceUInt();
            object actual = method.Invoke(null, null);

            Assert.AreEqual(expected, actual, "Instance uint field write mismatch");
        }

        [Test]
        public void TestWriteInstanceLongField()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestField), nameof(TestField.TestWriteInstanceLong));

            object expected = TestField.TestWriteInstanceLong();
            object actual = method.Invoke(null, null);

            Assert.AreEqual(expected, actual, "Instance long field write mismatch");
        }

        [Test]
        public void TestWriteInstanceULongField()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestField), nameof(TestField.TestWriteInstanceULong));

            object expected = TestField.TestWriteInstanceULong();
            object actual = method.Invoke(null, null);

            Assert.AreEqual(expected, actual, "Instance ulong field write mismatch");
        }

        [Test]
        public void TestStaticIntFieldRead()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestField), nameof(TestField.TestReadStaticInt));

            object expected = TestField.TestReadStaticInt();
            object actual = method.Invoke(null, null);

            Assert.AreEqual(expected, actual, "Static int field read mismatch");
        }

        [Test]
        public void TestStaticIntFieldWrite()
        {
            // Reset dedicated static write field first
            TestField.staticWriteTestInt = 100;

            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestField), nameof(TestField.TestWriteStaticInt));

            object expected = TestField.TestWriteStaticInt();
            object actual = method.Invoke(null, null);

            Assert.AreEqual(expected, actual, "Static int field write mismatch");
        }

        [Test]
        public void TestStaticStringFieldRead()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestField), nameof(TestField.TestReadStaticString));

            object expected = TestField.TestReadStaticString();
            object actual = method.Invoke(null, null);

            Assert.AreEqual(expected, actual, "Static string field read mismatch");
        }

        [Test]
        public void TestStaticBoolFieldRead()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestField), nameof(TestField.TestReadStaticBool));

            object expected = TestField.TestReadStaticBool();
            object actual = method.Invoke(null, null);

            Assert.AreEqual(expected, actual, "Static bool field read mismatch");
        }

        [Test]
        public void TestStaticStringFieldWrite()
        {
            // Reset dedicated static write field first
            TestField.staticWriteTestString = "Static Field";

            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestField), nameof(TestField.TestWriteStaticString));

            object expected = TestField.TestWriteStaticString();
            object actual = method.Invoke(null, null);

            Assert.AreEqual(expected, actual, "Static string field write mismatch");
        }

        [Test]
        public void TestStaticBoolFieldWrite()
        {
            // Reset dedicated static write field first
            TestField.staticWriteTestBool = false;

            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestField), nameof(TestField.TestWriteStaticBool));

            object expected = TestField.TestWriteStaticBool();
            object actual = method.Invoke(null, null);

            Assert.AreEqual(expected, actual, "Static bool field write mismatch");
        }

        [Test]
        public void TestConstantPiField()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestField), nameof(TestField.TestReadConstantPi));

            object expected = TestField.TestReadConstantPi();
            object actual = method.Invoke(null, null);

            Assert.AreEqual(expected, actual, "Constant Pi field read mismatch");
        }

        [Test]
        public void TestConstantMaxValueField()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestField), nameof(TestField.TestReadConstantMaxValue));

            object expected = TestField.TestReadConstantMaxValue();
            object actual = method.Invoke(null, null);

            Assert.AreEqual(expected, actual, "Constant MaxValue field read mismatch");
        }

        [Test]
        public void TestConstantGreetingField()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestField), nameof(TestField.TestReadConstantGreeting));

            object expected = TestField.TestReadConstantGreeting();
            object actual = method.Invoke(null, null);

            Assert.AreEqual(expected, actual, "Constant Greeting field read mismatch");
        }

        [Test]
        public void TestObjectFieldRead()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestField), nameof(TestField.TestReadObjectField));

            object expected = TestField.TestReadObjectField();
            object actual = method.Invoke(null, null);

            Assert.IsNotNull(expected, "Expected object field should not be null");
            Assert.IsNotNull(actual, "Actual object field should not be null");
            Assert.AreEqual(expected.GetType(), actual.GetType(), "Object field types should match");
        }

        [Test]
        public void TestObjectFieldWrite()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestField), nameof(TestField.TestWriteObjectField));

            object expected = TestField.TestWriteObjectField();
            object actual = method.Invoke(null, null);

            Assert.AreEqual(expected, actual, "Object field write mismatch");
        }

        [Test]
        public void TestMinMaxIntField()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestField), nameof(TestField.TestMinMaxInt));

            object expected = TestField.TestMinMaxInt();
            object actual = method.Invoke(null, null);

            Assert.AreEqual(expected, actual, "Min/Max int field test mismatch");
        }

        [Test]
        public void TestMinMaxByteField()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestField), nameof(TestField.TestMinMaxByte));

            object expected = TestField.TestMinMaxByte();
            object actual = method.Invoke(null, null);

            Assert.AreEqual(expected, actual, "Min/Max byte field test mismatch");
        }

        [Test]
        public void TestMinMaxLongField()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestField), nameof(TestField.TestMinMaxLong));

            object expected = TestField.TestMinMaxLong();
            object actual = method.Invoke(null, null);

            Assert.AreEqual(expected, actual, "Min/Max long field test mismatch");
        }

        [Test]
        public void TestReadonlyFields()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestField), nameof(TestField.TestFieldReadWrite));

            object instA = TestField.TestFieldReadWrite();
            object instB = method.Invoke(null, null);

            CheckIndividualField(instA, instB, "readOnlyInt");
            CheckIndividualField(instA, instB, "readOnlyDouble");
            CheckIndividualField(instA, instB, "readOnlyString");
        }

        [Test]
        public void TestDerivedClassFields()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestField), nameof(TestField.TestDerivedFields));

            object instA = TestField.TestDerivedFields();
            object instB = method.Invoke(null, null);

            CheckInstanceFields(instA, instB);
        }

        [Test]
        public void TestNullFields()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestField), nameof(TestField.TestNullFields));

            object[] expected = TestField.TestNullFields();
            object[] actual = (object[])method.Invoke(null, null);

            Assert.AreEqual(expected.Length, actual.Length, "Null fields array length mismatch");
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], actual[i], $"Null field {i} mismatch");
            }
        }

        [Test]
        public void TestSelfReferenceField()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestField), nameof(TestField.TestSelfReference));

            bool expected = TestField.TestSelfReference();
            bool actual = (bool)method.Invoke(null, null);

            Assert.AreEqual(expected, actual, "Self reference field test mismatch");
        }

        [Test]
        public void TestPrivateFieldThroughMethods()
        {
            Type testType = TestUtils.LoadTestType(nameof(TestField));
            object instance = Activator.CreateInstance(testType);

            MethodInfo getter = testType.GetMethod("GetPrivateField");
            MethodInfo setter = testType.GetMethod("SetPrivateField");

            // Get initial value
            int initialValue = (int)getter.Invoke(instance, null);
            Assert.AreEqual(4, initialValue, "Private field initial value should be 4");

            // Set new value
            setter.Invoke(instance, new object[] { 999 });

            // Get modified value
            int modifiedValue = (int)getter.Invoke(instance, null);
            Assert.AreEqual(999, modifiedValue, "Private field should be modified to 999");
        }

        private void CheckIndividualField(object instA, object instB, string fieldName)
        {
            Type typeA = instA.GetInterpretedType();
            Type typeB = instB.GetInterpretedType();

            FieldInfo fieldA = typeA.GetField(fieldName);
            FieldInfo fieldB = typeB.GetField(fieldName);

            Assert.IsNotNull(fieldA, $"Field {fieldName} should exist in direct execution");
            Assert.IsNotNull(fieldB, $"Field {fieldName} should exist in interpreted execution");

            object valueA = fieldA.GetValue(instA);
            object valueB = fieldB.GetValue(instB);

            Assert.AreEqual(valueA, valueB, $"Field {fieldName} values should match");
        }

        private void CheckInstanceFields(object instA, object instB)
        {
            // Get types
            Type typeA = instA.GetInterpretedType();
            Type typeB = instB.GetInterpretedType();

            // Get fields
            FieldInfo[] typeAFields = typeA.GetFields();
            FieldInfo[] typeBFields = typeB.GetFields();

            // Check all fields
            foreach (FieldInfo typeAField in typeAFields)
            {
                // Get value
                object valueA = typeAField.GetValue(instA);

                // Find matching field
                FieldInfo typeBField = typeBFields.FirstOrDefault(f => f.Name == typeAField.Name);

                // Get other value
                object valueB = typeBField.GetValue(instB);

                if (typeAField.FieldType.IsClass == true || typeAField.FieldType.IsInterface == true)
                {
                    // Check equality of string representation
                    Assert.AreEqual(valueA?.GetInterpretedType().ToString(), valueB?.GetInterpretedType().ToString(), message: typeAField.Name);
                }
                else
                {
                    // Check equality
                    Assert.AreEqual(valueA, valueB, message: typeAField.Name);
                }
            }
        }
    }
}
#endif