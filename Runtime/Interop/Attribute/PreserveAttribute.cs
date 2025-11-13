using System;

namespace dotnow.Interop
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Method, AllowMultiple = false)]
    public sealed class PreserveAttribute : Attribute
    {
        // Empty class
    }
}
