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

                // Check equality
                Assert.AreEqual(valueA, valueB, message: typeAField.Name);
            }
        }
    }
}
