using System;
using System.Globalization;
using System.Reflection;

namespace dotnow.Interop
{
    internal class CLRMethodBindingCallSite : MethodInfo
    {
        // Protected
        protected AppDomain domain = null;
        protected MethodBase originalMethod = null;
        protected MethodBase target = null;
        protected object[] args = null;

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

        public MethodBase OriginalMethod
        {
            get { return originalMethod; }
        }

        public MethodBase TargetMethod
        {
            get { return target; }
        }

        // Constructor
        internal CLRMethodBindingCallSite(AppDomain domain, MethodBase originalMethod, MethodBase target)
        {
            this.domain = domain;
            this.originalMethod = originalMethod;
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
            return target.GetMethodImplementationFlags();
        }

        public override ParameterInfo[] GetParameters()
        {
            return originalMethod.GetParameters();
        }

        public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
        {
            // Create cached args
            if(args == null)
            {
                args = new object[4];

                // Fill out persistent args
                args[0] = domain;
                args[1] = originalMethod;
            }

            // Fill out ordered parameters
            args[2] = obj;
            args[3] = parameters;

            // Invoke with mapped configuration
            return target.Invoke(null, args);
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
