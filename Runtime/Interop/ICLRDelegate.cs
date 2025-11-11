using System;
using System.Reflection;

namespace dotnow.Interop
{
    public interface ICLRDelegate
    {
        // Properties
        Delegate Delegate { get; }
        
        // Methods
        void Initialize(AppDomain appDomain, Type type, MethodInfo target);
    }
}
