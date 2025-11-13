using System;

namespace dotnow.Interop
{
    public interface ICLRProxy
    {
        // Properties
        ICLRInstance Instance { get; }

        // Methods
        void Initialize(AppDomain appDomain, Type type, ICLRInstance instance);
    }
}
