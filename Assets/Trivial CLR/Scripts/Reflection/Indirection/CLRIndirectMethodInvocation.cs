using System;
using System.Globalization;
using System.Reflection;

namespace TrivialCLR.Reflection.Indirection
{
    internal sealed class CLRIndirectMethodInvocation : MethodInfo
    {
        // Private
        private MethodInfo indirectMethod = null;

        // Properties
        public override ICustomAttributeProvider ReturnTypeCustomAttributes
        {
            get { return indirectMethod.ReturnTypeCustomAttributes; }
        }

        public override MethodAttributes Attributes
        {
            get { return indirectMethod.Attributes; }
        }

        public override RuntimeMethodHandle MethodHandle
        {
            get { return indirectMethod.MethodHandle; }
        }

        public override Type DeclaringType
        {
            get { return indirectMethod.DeclaringType; }
        }

        public override string Name
        {
            get { return indirectMethod.Name; }
        }

        public override Type ReflectedType
        {
            get { return indirectMethod.ReflectedType; }
        }

        // Constructor
        public CLRIndirectMethodInvocation(MethodInfo indirectMethod)
        {
            if (indirectMethod == null)
                throw new ArgumentNullException(nameof(indirectMethod));

            this.indirectMethod = indirectMethod;
        }

        // Methods
        public override MethodInfo GetBaseDefinition()
        {
            return indirectMethod.GetBaseDefinition();
        }

        public override object[] GetCustomAttributes(bool inherit)
        {
            return indirectMethod.GetCustomAttributes(inherit);
        }

        public override object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            return indirectMethod.GetCustomAttributes(attributeType, inherit);
        }

        public override MethodImplAttributes GetMethodImplementationFlags()
        {
            return indirectMethod.GetMethodImplementationFlags();
        }

        public override ParameterInfo[] GetParameters()
        {
            return indirectMethod.GetParameters();
        }

        public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
        {
            // Check for clr instance
            if (obj.IsCLRInstance() == true && (indirectMethod is CLRMethod) == false)
                obj = obj.Unwrap();

            // Invoke the method
            return indirectMethod.Invoke(obj, invokeAttr, binder, parameters, culture);
        }

        public override bool IsDefined(Type attributeType, bool inherit)
        {
            return indirectMethod.IsDefined(attributeType, inherit);
        }
    }
}
