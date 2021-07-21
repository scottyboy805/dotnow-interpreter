using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TrivialCLR.Interop
{
    internal class CLRConstructorBindingCallSite : ConstructorInfo
    {
        // Private
        private AppDomain domain = null;
        private MethodBase originalMethod = null;
        private MethodBase target = null;

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
        public override Type ReflectedType
        {
            get { return target.ReflectedType; }
        }

        // Constructor
        internal CLRConstructorBindingCallSite(AppDomain domain, MethodBase originalMethod, MethodBase target)
        {
            this.domain = domain;
            this.originalMethod = originalMethod;
            this.target = target;
        }

        // Methods
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

        public override object Invoke(BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
        {
            object[] args = new object[4];

            // Fill out ordered parameters
            args[0] = domain;
            args[1] = originalMethod;
            args[2] = null;
            args[3] = parameters;

            // Invoke with mapped configuration
            return target.Invoke(null, args);
        }

        public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
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
