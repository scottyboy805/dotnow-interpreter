#if !UNITY_DISABLE
#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_IOS || UNITY_ANDROID || UNITY_WSA || UNITY_WEBGL || UNITY_SWITCH)
using dotnow;
using dotnow.Interop;
using System;
using System.Reflection;
using AppDomain = dotnow.AppDomain;

namespace UnityEngine
{
    public static partial class OverrideBindings
    {
        [Preserve]
        [CLRGenericMethodBinding(typeof(ScriptableObject), "CreateInstance", 1)]
        public static object CreateInstanceTOverride(AppDomain domain, MethodInfo overrideMethod, object instance, object[] args, Type[] genericTypes)
            => CreateInstanceOverride(domain, overrideMethod, instance, new object[1] { genericTypes[0] });

        [Preserve]
        [CLRMethodBinding(typeof(ScriptableObject), "CreateInstance", typeof(Type))]
        public static object CreateInstanceOverride(AppDomain domain, MethodInfo overrideMethod, object instance, object[] args)
        {
            // Get argument
            Type scriptableType = args[0] as Type;

            // Check for CLR type
            if(scriptableType.IsCLRType() == false)
            {
                // Use default unity create instance
                return ScriptableObject.CreateInstance(scriptableType);
            }

            // Handle create instance manually via a proxy
            Type proxyType = domain.GetCLRProxyBindingForType(scriptableType.BaseType);

			// Validate type
			if (typeof(ScriptableObject).IsAssignableFrom(proxyType) == false)
				throw new InvalidOperationException("A type deriving from scriptable object must be provided");

            // Create proxy instance
            ICLRProxy proxyInstance = (ICLRProxy)ScriptableObject.CreateInstance(proxyType);

            // Create clr instance
            return domain.CreateInstanceFromProxy(scriptableType, proxyInstance);
		}
	}
}
#endif
#endif