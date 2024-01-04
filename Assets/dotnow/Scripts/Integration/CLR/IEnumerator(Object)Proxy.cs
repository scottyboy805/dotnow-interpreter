using dotnow;
using dotnow.Interop;

namespace System.Collections.Generic
{
    [CLRProxyBinding(typeof(IEnumerator<object>))]
    public class IEnumerator_Object_Proxy : ICLRProxy, IEnumerator<object>
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
                return instance.Type.GetMethod("System.Collections.Generic.IEnumerator<System.Object>.get_Current")?.Invoke(instance, null);
            }
        }

        public void Dispose()
        {
            instance.Type.GetMethod("Dispose")?.Invoke(instance, null);
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
