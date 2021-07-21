using System;

namespace TrivialCLR
{
    [AttributeUsage (AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method | AttributeTargets.Constructor)]
    public sealed class PreserveAttribute : Attribute
    {
        // Empty class
        // Prevent code stripping of managed bytecode (UnityEgnine)
        // From the docs: https://docs.unity3d.com/ScriptReference/Scripting.PreserveAttribute.html
        // ... We can define our own attribute with the exact name 'PreserveAttribute' and the linker will respect this attribute and prevent code stripping. Avoids a dependency on UnityEngine.
    }
}
