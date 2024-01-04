
namespace dotnow.Interop
{
    public interface ICLRProxy
    {
        // Properties
        CLRInstanceOld Instance { get; } 

        // Methods
        void InitializeProxy(AppDomain domain, CLRInstanceOld instance);
    }
}
