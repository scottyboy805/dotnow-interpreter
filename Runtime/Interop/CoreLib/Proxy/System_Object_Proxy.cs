using System;
using System.Reflection;

namespace dotnow.Interop.CoreLib.Proxy
{
    [Preserve]
    [CLRProxyBinding(typeof(object))]
    internal sealed class System_Object_Proxy : object, ICLRProxy
    {
        // Private
        private ICLRInstance instance;

        private MethodInfo toStringOverride;
        private MethodInfo getHashCodeOverride;
        private MethodInfo equalsOverride;

        // Properties
        public ICLRInstance Instance => instance;

        // Methods
        public void Initialize(AppDomain appDomain, Type type, ICLRInstance instance)
        {
            this.instance = instance;

            toStringOverride = type.GetMethod(nameof(ToString), BindingFlags.Instance | BindingFlags.Public);
            getHashCodeOverride = type.GetMethod(nameof(GetHashCode), BindingFlags.Instance | BindingFlags.Public);
            equalsOverride = type.GetMethod(nameof(Equals), BindingFlags.Instance | BindingFlags.Public, Type.DefaultBinder, new[] { typeof(object) }, null);
        }

        public override string ToString()
        {
            // Check for override
            if(toStringOverride != null)
            {
                return (string)toStringOverride.Invoke(instance, null);
            }

            // Call default implementation
            return instance.ToString();
        }

        public override int GetHashCode()
        {
            // Check for override
            if(getHashCodeOverride != null)
            {
                return (int)getHashCodeOverride.Invoke(instance, null);
            }

            // Call default implementation
            return instance.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            // Check for override
            if(equalsOverride != null)
            {
                return (bool)equalsOverride.Invoke(instance, new[] { obj });
            }

            // Check for interpreted instance
            if (obj is ICLRInstance clrObject)
                return instance.Equals(clrObject);

            // Not equal to
            return false;
        }
    }
}
