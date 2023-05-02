using System;
using System.Globalization;
using System.Reflection;

namespace dotnow.Interop
{
    internal class CLRCreateInstanceBindingCallSite : MethodInfo
    {
        // Private
        private AppDomain domain = null;
        private Type originalType = null;
        private ConstructorInfo originalConstructor = null;
        private MethodBase target = null;

        private Type dynamicOriginalType = null;
        private ConstructorInfo dynamicOriginalConstructor = null;

        // Properties
        public override string Name
        {
            get { return target.Name; }
        }

        public override Type DeclaringType
        {
            get { return target.DeclaringType; }
        }

        public override RuntimeMethodHandle MethodHandle
        {
            get { throw new NotSupportedException("A RuntimeMethod has no obtainble method handle"); }
        }

        public override MethodAttributes Attributes
        {
            get { return target.Attributes; }
        }

        public override MemberTypes MemberType
        {
            get { return MemberTypes.Method; }
        }

        public override Type ReflectedType
        {
            get { return target.ReflectedType; }
        }

        public override ICustomAttributeProvider ReturnTypeCustomAttributes
        {
            get { throw new NotImplementedException(); }
        }

        // Constructor
        internal CLRCreateInstanceBindingCallSite(AppDomain domain, Type originalType, ConstructorInfo originalConstructor, MethodBase target)
        {
            this.domain = domain;
            this.originalType = originalType;
            this.originalConstructor = originalConstructor;
            this.target = target;
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
            throw new NotImplementedException();
        }

        public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
        {
            object[] args = new object[4];

            Type typeInfo = originalType;
            ConstructorInfo ctorInfo = originalConstructor;

            if (ctorInfo == null && dynamicOriginalConstructor != null)
                ctorInfo = dynamicOriginalConstructor;

            if (dynamicOriginalType != null)
                typeInfo = dynamicOriginalType;

            // Fill out parameters
            args[0] = domain;
            args[1] = typeInfo;
            args[2] = ctorInfo;
            args[3] = parameters;

            // Reset state
            dynamicOriginalType = null;
            dynamicOriginalConstructor = null;

            // Invoke with mapped configuration
            return target.Invoke(null, args);
        }

        public override bool IsDefined(Type attributeType, bool inherit)
        {
            throw new NotImplementedException();
        }

        internal void DynamicOriginalConstructorCall(MethodBase dynamicCtor, Type dynamicType = null)
        {
            if (dynamicCtor == null || (dynamicCtor.DeclaringType.IsGenericType == false && dynamicCtor.DeclaringType != originalType))
                return;

            this.dynamicOriginalType = dynamicType;
            this.dynamicOriginalConstructor = dynamicCtor as ConstructorInfo;
        }
    }
}
