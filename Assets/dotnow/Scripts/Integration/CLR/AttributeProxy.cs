using dotnow;
using dotnow.Interop;

namespace System
{
    [CLRProxyBinding(typeof(Attribute))]
    public class AttributeProxy : Attribute, ICLRProxy
    {
        // Private
        private CLRInstanceOld instance;

        // Properties
        public CLRInstanceOld Instance
        {
            get { return instance; }
        }

        // Methods
        public void InitializeProxy(dotnow.AppDomain domain, CLRInstanceOld instance)
        {
            this.instance = instance;
        }

        public override bool IsDefaultAttribute()
        {
            return base.IsDefaultAttribute();
        }
    }
}
