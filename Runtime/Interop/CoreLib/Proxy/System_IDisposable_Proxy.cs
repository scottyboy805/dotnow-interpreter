using System;
using System.Reflection;

namespace dotnow.Interop.CoreLib.Proxy
{
    [Preserve]
    [CLRProxyBinding(typeof(IDisposable))]
    internal sealed class System_IDisposable_Proxy : IDisposable, ICLRProxy
    {
        // Private
        private ICLRInstance instance;

        private MethodInfo disposeMethod;

        // Properties
        public ICLRInstance Instance => instance;

        // Methods
        public void Initialize(AppDomain appDomain, Type type, ICLRInstance instance)
        {
            this.instance = instance;

            disposeMethod = type.GetMethod(nameof(Dispose), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        }

        public void Dispose()
        {
            // Call dispose
            disposeMethod.Invoke(instance, null);
        }
    }
}
