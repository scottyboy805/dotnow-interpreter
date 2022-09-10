
namespace dotnow.Interop
{
    public interface ICLRProxy
    {
        // Properties
        CLRInstance Instance { get; } 

        // Methods
        void InitializeProxy(AppDomain domain, CLRInstance instance);
    }
}
