using System;
using System.Reflection;
using System.Globalization;

namespace TrivialCLR.Reflection.Indirection
{
    internal sealed class CLRIndirectFieldInvocation : FieldInfo
    {
        // Private
        private FieldInfo indirectField = null;

        // Properties
        public override FieldAttributes Attributes
        {
            get { return indirectField.Attributes; }
        }

        public override RuntimeFieldHandle FieldHandle
        {
            get { return indirectField.FieldHandle; }
        }

        public override Type FieldType
        {
            get { return indirectField.FieldType; }
        }

        public override Type DeclaringType
        {
            get { return indirectField.DeclaringType; }
        }

        public override string Name
        {
            get { return indirectField.Name; }
        }

        public override Type ReflectedType
        {
            get { return indirectField.ReflectedType; }
        }

        // Constructor
        public CLRIndirectFieldInvocation(FieldInfo indirectField)
        {
            if (indirectField == null)
                throw new ArgumentNullException(nameof(indirectField));

            this.indirectField = indirectField;
        }

        // Methods
        public override object[] GetCustomAttributes(bool inherit)
        {
            return indirectField.GetCustomAttributes(inherit);
        }

        public override object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            return indirectField.GetCustomAttributes(attributeType, inherit);
        }

        public override object GetValue(object obj)
        {
            // Check for clr instance
            if (obj.IsCLRInstance() == true && (indirectField is CLRField) == false)
                obj = obj.Unwrap();

            // Get field value
            return indirectField.GetValue(obj);
        }

        public override bool IsDefined(Type attributeType, bool inherit)
        {
            return indirectField.IsDefined(attributeType, inherit);
        }

        public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, CultureInfo culture)
        {
            // Check for clr instance
            if (obj.IsCLRInstance() == true && (indirectField is CLRField) == false)
                obj = obj.Unwrap();

            // Set field value
            indirectField.SetValue(obj, value, invokeAttr, binder, culture);
        }
    }
}
