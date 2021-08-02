using System;
using System.Globalization;
using System.Reflection;

namespace dotnow.Interop
{
    internal sealed class CLRAbstractMethodInfo : MethodInfo
    {
        // Private
        private string methodName = "";
        private Type returnType = typeof(void);
        private ParameterInfo[] parameters = Array.Empty<ParameterInfo>();
        private MethodAttributes attributes = 0;

        // Propertie
        public override string Name
        {
            get { return methodName; }
        }

        public override Type ReturnType
        {
            get { return returnType; }
        }

        public override Type ReflectedType => throw new NotImplementedException();

        public override ICustomAttributeProvider ReturnTypeCustomAttributes => throw new NotImplementedException();

        public override MethodAttributes Attributes
        {
            get { return attributes; }
        }

        public override RuntimeMethodHandle MethodHandle => throw new NotImplementedException();

        public override Type DeclaringType => throw new NotImplementedException();

        // Constructor
        public CLRAbstractMethodInfo(string methodName, Type returnType, Type[] parameterTypes, MethodAttributes attributes)
        {
            this.methodName = methodName;
            this.returnType = returnType;
            this.parameters = new ParameterInfo[parameterTypes.Length];
            this.attributes = attributes;

            for (int i = 0; i < parameters.Length; i++)
                parameters[i] = new CLRAbstractParameterInfo(parameterTypes[i]);
        }

        // Methods
        public override MethodInfo GetBaseDefinition()
        {
            throw new NotImplementedException();
        }

        public override object[] GetCustomAttributes(bool inherit)
        {
            throw new NotImplementedException();
        }

        public override object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            throw new NotImplementedException();
        }

        public override MethodImplAttributes GetMethodImplementationFlags()
        {
            throw new NotImplementedException();
        }

        public override ParameterInfo[] GetParameters()
        {
            return parameters;
        }

        public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override bool IsDefined(Type attributeType, bool inherit)
        {
            throw new NotImplementedException();
        }
    }
}
