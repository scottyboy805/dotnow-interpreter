#if !UNITY_DISABLE
#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_IOS || UNITY_ANDROID || UNITY_WSA || UNITY_WEBGL || UNITY_SWITCH)
using dotnow;
using dotnow.Runtime;
using System;
using System.Reflection;

namespace UnityEngine
{
    public static partial class CreateInstanceBindings
    {
        [Preserve]
        [CLRCreateInstanceBinding(typeof(Events.UnityAction))]
        public static object CreateUnityActionInstanceOverride(dotnow.AppDomain domain, Type type, ConstructorInfo ctor, object[] args)
        {
            // Create delegate
            return __delegate.AutoAnyInteropDelegateFromParametersAsType(type, args[0], (MethodBase)args[1]);
        }

        [Preserve]
        [CLRCreateInstanceBinding(typeof(Events.UnityAction<>))]
        public static object CreateUnityActionInstanceOverride_T0(dotnow.AppDomain domain, Type type, ConstructorInfo ctor, object[] args)
        {
            // Create delegate
            return __delegate.AutoAnyInteropDelegateFromParametersAsType(type, args[0], (MethodBase)args[1]);
        }

        [Preserve]
        [CLRCreateInstanceBinding(typeof(Events.UnityAction<,>))]
        public static object CreateUnityActionInstanceOverride_T1(dotnow.AppDomain domain, Type type, ConstructorInfo ctor, object[] args)
        {
            // Create delegate
            return __delegate.AutoAnyInteropDelegateFromParametersAsType(type, args[0], (MethodBase)args[1]);
        }

        [Preserve]
        [CLRCreateInstanceBinding(typeof(Events.UnityAction<,,,>))]
        public static object CreateUnityActionInstanceOverride_T2(dotnow.AppDomain domain, Type type, ConstructorInfo ctor, object[] args)
        {
            // Create delegate
            return __delegate.AutoAnyInteropDelegateFromParametersAsType(type, args[0], (MethodBase)args[1]);
        }
    }
}
#endif
#endif