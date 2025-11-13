using System;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace dotnow.Reflection.Internal
{
    /// <summary>
    /// Represents an object initializer that is injected by the runtime.
    /// In some cases such as multi-dimensional arrays, the initializer does not exist but is instead implicitly available in the runtime.
    /// Provides no functionality, only limited metadata. The actual implementation is handled by a direct call.
    /// </summary>
    internal sealed class CLRInternalConstructorInfo : ConstructorInfo
    {
        // Private
        private readonly string name;
        private readonly Type declaringType;
        private readonly ParameterInfo[] parameters;
        private readonly MethodAttributes attributes;
        private readonly DirectInstance directInternalInstance;

        // Properties
        public override string Name => name;
        public override Type DeclaringType => declaringType;
        public override MethodAttributes Attributes => attributes;
        public override RuntimeMethodHandle MethodHandle => throw new NotSupportedException();
        public override Type ReflectedType => typeof(CLRInternalConstructorInfo);

        public DirectInstance DirectInternalInstance => directInternalInstance;

        // Constructor
        public CLRInternalConstructorInfo(string name, Type declaringType, Type[] parameterTypes, MethodAttributes attributes, DirectInstance directInternalInstance)
        {
            // Check for null
            if (directInternalInstance == null)
                throw new ArgumentNullException(nameof(directInternalInstance));

            this.name = name;
            this.declaringType = declaringType;
            this.parameters = parameterTypes
                .Select(p => new CLRParameterInfo(this, p))
                .ToArray();
            this.attributes = attributes;
            this.directInternalInstance = directInternalInstance;
        }

        // Methods
        public override ParameterInfo[] GetParameters() => parameters;
        public override MethodImplAttributes GetMethodImplementationFlags() => MethodImplAttributes.InternalCall;
        public override MethodBody GetMethodBody() => null;

        public override object[] GetCustomAttributes(bool inherit) => throw new NotSupportedException();
        public override object[] GetCustomAttributes(Type attributeType, bool inherit) => throw new NotSupportedException();        
        public override object Invoke(BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture) => throw new NotSupportedException("Internal initializer cannot be called via reflection");
        public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture) => throw new NotSupportedException("Internal initializer cannot be called via reflection");
        public override bool IsDefined(Type attributeType, bool inherit) => throw new NotSupportedException();
    }
}
