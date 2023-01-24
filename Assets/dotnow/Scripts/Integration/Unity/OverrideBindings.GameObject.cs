#if !UNITY_DISABLE
#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_IOS || UNITY_ANDROID || UNITY_WSA || UNITY_WEBGL || UNITY_SWITCH)
using System;
using System.Collections.Generic;
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


        [Preserve]
        [CLRMethodBinding(typeof(GameObject), "GetComponent", typeof(Type))]
        public static object UnityEngine_GameObject_GetComponentOverride(dotnow.AppDomain domain, MethodInfo overrideMethod, object instance, object[] args)
        {
            // Get object
            GameObject go = instance as GameObject;

            // Get argument
            Type componentType = args[0] as Type;

            // Check for clr type
            if (componentType.IsCLRType() == false)
            {
                return go.GetComponent(componentType);
            }

            // Get all proxies
            getComponentsNonAlloc.Clear();
            go.GetComponents<ICLRProxy>(getComponentsNonAlloc);

            // Check for matching instance
            for (int i = 0; i < getComponentsNonAlloc.Count; i++)
            {
                // Check for matching type then return the clr instance
                if (getComponentsNonAlloc[i].Instance.Type == componentType)
                    return getComponentsNonAlloc[i].Instance;
            }

            // No component found
            return null;
        }

        [Preserve]
        [CLRMethodBinding(typeof(GameObject), "GetComponents", typeof(Type))]
        public static object UnityEngine_GameObject_GetComponentsOverride(dotnow.AppDomain domain, MethodInfo overrideMethod, object instance, object[] args)
        {
            // Get object
            GameObject go = instance as GameObject;

            // Get argument
            Type componentType = args[0] as Type;

            // Check for clr type
            if (componentType.IsCLRType() == false)
            {
                return go.GetComponents(componentType);
            }

            // Get all proxies
            getComponentsNonAlloc.Clear();
            go.GetComponents<ICLRProxy>(getComponentsNonAlloc);

            // Get results collection
            List<Component> results = new List<Component>(getComponentsNonAlloc.Count);

            // Check for matching instance
            for (int i = 0; i < getComponentsNonAlloc.Count; i++)
            {
                // Check for matching type then return the clr instance
                if (getComponentsNonAlloc[i].Instance.Type == componentType)
                    results.Add(getComponentsNonAlloc[i].Instance.Unwrap() as Component);
            }

            // No component found
            return results.ToArray();
        }
    }
}
#endif
#endif
