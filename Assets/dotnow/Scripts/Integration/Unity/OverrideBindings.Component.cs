#if !UNITY_DISABLE
#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_IOS || UNITY_ANDROID || UNITY_WSA || UNITY_WEBGL)
using System;
using System.Collections.Generic;
using System.Reflection;
using dotnow;
using dotnow.Interop;

namespace UnityEngine
{
    public static partial class OverrideBindings
    {
        private static readonly List<ICLRProxy> getComponentsNonAlloc = new List<ICLRProxy>();

        [Preserve]
        [CLRMethodBinding(typeof(Component), "GetComponent", typeof(Type))]
        public static object UnityEngine_Component_GetComponentOverride(dotnow.AppDomain domain, MethodInfo overrideMethod, object instance, object[] args)
        {
            // Get component
            Component comp = instance as Component;

            // Get argument
            Type componentType = args[0] as Type;

            // Check for clr type
            if (componentType.IsCLRType() == false)
            {
                return comp.GetComponent(componentType);
            }

            // Get all proxies
            getComponentsNonAlloc.Clear();
            comp.GetComponents<ICLRProxy>(getComponentsNonAlloc);

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
    }
}
#endif
#endif