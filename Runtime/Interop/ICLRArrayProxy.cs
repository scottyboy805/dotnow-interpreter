using System;

namespace dotnow.Interop
{
    public interface ICLRArrayProxy
    {
        // Properties
        Array Instance { get; }

        // Methods
        void Initialize(AppDomain appDomain, Type elementType, Array instance);

        void GetValueDirect(StackContext context);

        void SetValueDirect(StackContext context);
    }
}
