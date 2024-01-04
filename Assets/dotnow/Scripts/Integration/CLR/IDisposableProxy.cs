using dotnow;
using dotnow.Interop;

namespace System
{
    [CLRProxyBinding(typeof(IDisposable))]
    public class IDisposableProxy : ICLRProxy, IDisposable
    {
        // Private
        private CLRInstanceOld instance;

        public CLRInstanceOld Instance
        {
            get { return instance; }
        }

        public void Dispose()
        {
            instance.Type.GetMethod("Dispose")?.Invoke(instance, null);
        }

        public void InitializeProxy(dotnow.AppDomain domain, CLRInstanceOld instance)
        {
            this.instance = instance;
        }
    }
}
