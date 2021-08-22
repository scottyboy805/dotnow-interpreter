using System;
using System.Globalization;
using System.Reflection;

namespace dotnow.Reflection.Indirection
{
    internal sealed class CLRIndirectPropertyInvocation : PropertyInfo
    {
        // Private
        private PropertyInfo indirectProperty = null;

        // Properties
        public override PropertyAttributes Attributes
        {
            get { return indirectProperty.Attributes; }
        }

        public override bool CanRead
        {
            get { return indirectProperty.CanRead; }
        }

        public override bool CanWrite
        {
            get { return indirectProperty.CanWrite; }
        }

        public override Type PropertyType
        {
            get { return indirectProperty.PropertyType; }
        }

        public override Type DeclaringType
        {
            get { return indirectProperty.DeclaringType; }
        }

        public override string Name
        {
            get { return indirectProperty.Name; }
        }

        public override Type ReflectedType
        {
            get { return indirectProperty.ReflectedType; }
        }

        // Constructor
        public CLRIndirectPropertyInvocation(PropertyInfo indirectProperty)
        {
            if (indirectProperty == null)
                throw new ArgumentNullException(nameof(indirectProperty));

            this.indirectProperty = indirectProperty;
        }

        // Methods
        public override MethodInfo[] GetAccessors(bool nonPublic)
        {
            return indirectProperty.GetAccessors(nonPublic);
        }

        public override object[] GetCustomAttributes(bool inherit)
        {
            return indirectProperty.GetCustomAttributes(inherit);
        }

        public override object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            return indirectProperty.GetCustomAttributes(attributeType, inherit);
        }

        public override MethodInfo GetGetMethod(bool nonPublic)
        {
            return indirectProperty.GetGetMethod(nonPublic);
        }

        public override ParameterInfo[] GetIndexParameters()
        {
            return indirectProperty.GetIndexParameters();
        }

        public override MethodInfo GetSetMethod(bool nonPublic)
        {
            return indirectProperty.GetSetMethod(nonPublic);
        }

        public override object GetValue(object obj, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture)
        {
            // Check for clr instance
            if (obj.IsCLRInstance() == true && (indirectProperty is CLRProperty) == false)
                obj = obj.Unwrap();

            // Get the property value
            return indirectProperty.GetValue(obj, invokeAttr, binder, index, culture);
        }

        public override bool IsDefined(Type attributeType, bool inherit)
        {
            return indirectProperty.IsDefined(attributeType, inherit);
        }

        public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture)
        {
            // Check for clr instance
            if (obj.IsCLRInstance() == true && (indirectProperty is CLRProperty) == false)
                obj = obj.Unwrap();

            // Set the property value
            indirectProperty.SetValue(obj, value, invokeAttr, binder, index, culture);
        }
    }
}
