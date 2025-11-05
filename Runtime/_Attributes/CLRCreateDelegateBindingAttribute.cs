using System;

namespace dotnow
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class CLRCreateDelegateBindingAttribute : Attribute
    {
        // Private
        private Type delegateType;

        // Properties
        public Type DelegateType
        {
            get { return delegateType; }
        }

        // Constructor
        public CLRCreateDelegateBindingAttribute(Type delegateType)
        {
            this.delegateType = delegateType;
        }
    }
}
