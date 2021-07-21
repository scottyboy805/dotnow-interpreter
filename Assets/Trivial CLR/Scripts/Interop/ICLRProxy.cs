
namespace TrivialCLR.Interop
{
    public interface ICLRProxy
    {
        // Methods
        void InitializeProxy(AppDomain domain, CLRInstance instance);
    }
}
