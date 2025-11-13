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
    public class Class
    {
        [Test]
        public void TestCreateClass()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestClass), nameof(TestClass.TestCreateClass));

            object instA = TestClass.TestCreateClass();
            object instB = method.Invoke(null, null);


            //Assert.AreEqual(instA, instB);
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
#endif