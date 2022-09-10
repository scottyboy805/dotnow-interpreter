#if !UNITY_DISABLE
#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_IOS || UNITY_ANDROID || UNITY_WSA || UNITY_WEBGL)
using System;
using System.Reflection;
using dotnow;
using dotnow.Interop;

namespace UnityEngine
{
    public static partial class OverrideBindings
    {
        [Preserve]
        [CLRMethodBinding(typeof(GameObject), "AddComponent", typeof(Type))]
        public static object AddComponentOverride(dotnow.AppDomain domain, MethodInfo overrideMethod, object instance, object[] args)
        {
            // Get instance
            GameObject go = instance as GameObject;

            // Get argument
            Type componentType = args[0] as Type;

            // Check for clr type
            if (componentType.IsCLRType() == false)
            {
                // Use default unity behaviour
                return go.AddComponent(componentType);
            }

            // Handle add component manually
            Type proxyType = domain.GetCLRProxyBindingForType(componentType.BaseType);

            // Validate type
            if (typeof(MonoBehaviour).IsAssignableFrom(proxyType) == false)
                throw new InvalidOperationException("A type deriving from mono behaviour must be provided");

            // Create proxy instance
            ICLRProxy proxyInstance = (ICLRProxy)go.AddComponent(proxyType);

            // Create clr instance
            return domain.CreateInstanceFromProxy(componentType, proxyInstance);
        }
    }
}
#endif
#endif
