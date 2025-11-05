using System;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace dotnow.Reflection.Internal
{
    /// <summary>
    /// Represents a method that is injected by the runtime.
    /// In some cases such as multi-dimensional arrays, the Get and Set methods do not exist but are instead implicitly available in the runtime.
    /// Provides no functionality, only limited metadata. The actual implementation is handled by a direct call.
    /// </summary>
    internal sealed class CLRInternalMethodInfo : MethodInfo
    {
        // Private
        private readonly string name;
        private readonly Type declaringType;
        private readonly ParameterInfo returnParameter;
        private readonly ParameterInfo[] parameters;
        private readonly MethodAttributes attributes;
        private readonly DirectCall directInternalCall;

        // Properties
        public override string Name => name;
        public override Type DeclaringType => declaringType;
        public override MethodAttributes Attributes => attributes;
        public override RuntimeMethodHandle MethodHandle => throw new NotSupportedException();
        public override Type ReflectedType => typeof(CLRInternalConstructorInfo);
        public override ICustomAttributeProvider ReturnTypeCustomAttributes => throw new NotImplementedException();
        public override ParameterInfo ReturnParameter => returnParameter;
        public override Type ReturnType => returnParameter.ParameterType;

        public DirectCall DirectInternalCall => directInternalCall;

        // Constructor
        public CLRInternalMethodInfo(string name, Type declaringType, Type returnType, Type[] parameterTypes, MethodAttributes attributes, DirectCall directInternalCall)
        {
            // Check for null
            if(directInternalCall == null)
                throw new ArgumentNullException(nameof(directInternalCall));

            this.name = name;
            this.declaringType = declaringType;
            this.returnParameter = new CLRParameterInfo(this, returnType);
            this.parameters = parameterTypes
                .Select(p => new CLRParameterInfo(this, p))
                .ToArray();
            this.attributes = attributes;
            this.directInternalCall = directInternalCall;
        }

        // Methods
        public override ParameterInfo[] GetParameters() => parameters;
        public override MethodImplAttributes GetMethodImplementationFlags() => MethodImplAttributes.InternalCall;
        public override MethodBody GetMethodBody() => null;

        public override MethodInfo GetBaseDefinition() => throw new NotSupportedException();
        public override object[] GetCustomAttributes(bool inherit) => throw new NotSupportedException();
        public override object[] GetCustomAttributes(Type attributeType, bool inherit) => throw new NotSupportedException();
        public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture) => throw new NotSupportedException("Internal method cannot be called via reflection");
        public override bool IsDefined(Type attributeType, bool inherit) => throw new NotSupportedException();
    }
}
