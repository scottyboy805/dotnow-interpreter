//using dotnow;
//using dotnow.Interop;

//namespace System
//{
//    [CLRProxyBinding(typeof(IDisposable))]
//    public class IDisposableProxy : ICLRProxy, IDisposable
//    {
//        // Private
//        private CLRInstance instance;

//        public CLRInstance Instance
//        {
//            get { return instance; }
//        }

//        public void Dispose()
//        {
//            instance.Type.GetMethod("Dispose")?.Invoke(instance, null);
//        }

//        public void InitializeProxy(dotnow.AppDomain domain, CLRInstance instance)
//        {
//            this.instance = instance;
//        }
//    }
//}
