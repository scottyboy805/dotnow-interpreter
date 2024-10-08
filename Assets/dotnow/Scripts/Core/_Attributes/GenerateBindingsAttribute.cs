using System;
namespace dotnow
{
    [AttributeUsage (AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Interface | AttributeTargets.Delegate| AttributeTargets.Enum)]
    public class GenerateBindingsAttribute : Attribute
    {
        
    }
}
