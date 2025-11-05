
namespace dotnow.Interop.Runtime.Proxy
{
    // Fast array access for decimal arrays without boxing
    [Preserve]
    [CLRArrayProxyBinding(typeof(decimal), 1)]
    internal sealed class System_DecimalArray_1_Proxy : System_UnmanagedArray_1_Proxy<decimal> {}

    //[CLRArrayProxyBinding(typeof(decimal[,]))]
    //internal sealed class System_DecimalArray_2_Proxy : System_UnmanagedArray_2_Proxy<decimal> { }

    //[CLRArrayProxyBinding(typeof(decimal[,,]))]
    //internal sealed class System_DecimalArray_3_Proxy : System_UnmanagedArray_3_Proxy<decimal> { }
}
