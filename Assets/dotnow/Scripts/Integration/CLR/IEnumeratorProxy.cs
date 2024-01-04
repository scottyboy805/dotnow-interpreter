using dotnow;
using dotnow.Interop;

namespace System.Collections
{
    [CLRProxyBinding(typeof(IEnumerator))]
    public class IEnumeratorProxy : ICLRProxy, IEnumerator
    {
        // Private
        private CLRInstanceOld instance;

        public CLRInstanceOld Instance
        {
            get { return instance; }
        }

        public object Current
        {
            get
            {
                return instance.Type.GetMethod("System.Collections.Generic.IEnumerator.get_Current")?.Invoke(instance, null);
            }
        }

        public void InitializeProxy(dotnow.AppDomain domain, CLRInstanceOld instance)
        {
            this.instance = instance;
        }

        public bool MoveNext()
        {
            return (bool)instance.Type.GetMethod("MoveNext")?.Invoke(instance, null);
        }

        public void Reset()
        {
            instance.Type.GetMethod("Reset")?.Invoke(instance, null);
        }
    }
}
