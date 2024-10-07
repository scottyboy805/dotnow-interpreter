using System;
using System.Globalization;
using System.Reflection;

namespace dotnow.Interop
{
    internal class CLRGenericMethodBindingCallSite : CLRMethodBindingCallSite
    {
        // Protected
        protected Type[] genericArguments = null;

        // Constructor
        internal CLRGenericMethodBindingCallSite(AppDomain domain, MethodBase originalMethod, MethodBase target, Type[] genericArguments)
            : base(domain, originalMethod, target)
        {
            this.genericArguments = genericArguments;
        }

        // Methods
        public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
        {
            // Create cached args
            if (args == null)
            {
                args = new object[5];

                // Fill out persistent args
                args[0] = domain;
                args[1] = originalMethod;
            }

            // Fill out ordered parameters
            args[2] = obj;
            args[3] = parameters;
            args[4] = genericArguments;

            // Invoke with mapped configuration
            return target.Invoke(null, args);
        }
    }
}
