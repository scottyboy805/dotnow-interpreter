using dotnow;
using dotnow.Interop;

namespace System
{
    [CLRProxyBinding(typeof(Attribute))]
    public class AttributeProxy : Attribute, ICLRProxy
    {
        // Private
        private CLRInstance instance;

        // Properties
        public CLRInstance Instance
        {
            get { return instance; }
        }

        // Methods
        public void InitializeProxy(dotnow.AppDomain domain, CLRInstance instance)
        {
            this.instance = instance;
        }

        public override bool IsDefaultAttribute()
        {
            return base.IsDefaultAttribute();
        }
    }
}
