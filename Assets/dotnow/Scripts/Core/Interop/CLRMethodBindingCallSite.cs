using System;
using System.Globalization;
using System.Reflection;

namespace dotnow.Interop
{
    internal class CLRMethodBindingCallSite : MethodInfo
    {
        // Private
        private AppDomain domain = null;
        private MethodBase originalMethod = null;
        private MethodBase target = null;
        private bool isGeneric;
        private Type[] genericTypes;
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
            get { throw new NotSupportedException("A RuntimeMethod has no obtainable method hamdle"); }
        }

        public override MethodAttributes Attributes
        {
            get { return originalMethod.Attributes; }
        }

        public override MemberTypes MemberType
        {
            get { return MemberTypes.Method; }
        }

        public override Type ReflectedType
        {
            get { return target.ReflectedType; }
        }

        public override Type ReturnType
        {
            get
            {
                if (originalMethod is MethodInfo)
                    return (originalMethod as MethodInfo).ReturnType;

                return typeof(void);
            }
        }

        public override ICustomAttributeProvider ReturnTypeCustomAttributes
        {
            get { throw new NotImplementedException(); }
        }

        // Constructor
        internal CLRMethodBindingCallSite(AppDomain domain, MethodBase originalMethod, MethodBase target)
        {
            this.domain = domain;
            this.originalMethod = originalMethod;
            this.target = target;
        }
        CLRMethodBindingCallSite(AppDomain domain, MethodBase originalMethod, MethodBase target, Type[] genericTypes)
        {
            this.domain = domain;
            this.originalMethod = originalMethod;
            this.target = target;

            isGeneric = true;
            this.genericTypes = genericTypes;
        }

        public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
        {
            if (!isGeneric)
            {
                object[] args = new object[4];

                // Fill out ordered parameters
                args[0] = domain;
                args[1] = originalMethod;
                args[2] = obj;
                args[3] = parameters;

                // Invoke with mapped configuration
                return target.Invoke(null, args);
            }
            else
            {
                object[] args = new object[5];

                // Fill out ordered parameters
                args[0] = domain;
                args[1] = originalMethod;
                args[2] = obj;
                args[3] = parameters;
                args[4] = genericTypes;

                // Invoke with mapped configuration
                return target.Invoke(null, args);
            }
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
            return target.GetMethodImplementationFlags();
        }

        public override ParameterInfo[] GetParameters()
        {
            return originalMethod.GetParameters();
        }

        public override bool IsDefined(Type attributeType, bool inherit)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return target.ToString();
        }
    }
}
