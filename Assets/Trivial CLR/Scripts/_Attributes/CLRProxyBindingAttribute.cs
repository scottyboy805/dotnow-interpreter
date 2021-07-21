using System;

namespace TrivialCLR
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class CLRProxyBindingAttribute : Attribute
    {
        // Private
        private Type baseProxyType = null;

        // Properties
        public Type BaseProxyType
        {
            get { return baseProxyType; }
        }

        // Constructor
        public CLRProxyBindingAttribute(Type baseProxyType)
        {
            this.baseProxyType = baseProxyType;
        }
    }
}
