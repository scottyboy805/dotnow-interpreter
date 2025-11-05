using System;

namespace dotnow.Interop
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class CLRProxyBindingAttribute : Attribute
    {
        // Private
        private readonly Type forType;

        // Properties
        public Type ForType => forType;

        // Constructor
        public CLRProxyBindingAttribute(Type forType)
        {
            this.forType = forType;
        }
    }
}
