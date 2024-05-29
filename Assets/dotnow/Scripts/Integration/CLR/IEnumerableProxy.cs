using dotnow;
using dotnow.Interop;
using System.Collections;

namespace System
{
    [CLRProxyBinding(typeof(IEnumerable))]
    public class IEnumerableProxy : ICLRProxy, IEnumerable
    {
        // Private
        private CLRInstance instance;

        public CLRInstance Instance
        {
            get { return instance; }
        }

        public IEnumerator GetEnumerator()
        {
            return (IEnumerator)instance.Type.GetMethod("GetEnumerator")?.Invoke(instance, null);
        }

        public void InitializeProxy(dotnow.AppDomain domain, CLRInstance instance)
        {
            this.instance = instance;
        }
    }
}
