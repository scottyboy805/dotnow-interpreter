#if !UNITY_DISABLE
#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_IOS || UNITY_ANDROID || UNITY_WSA || UNITY_WEBGL || UNITY_SWITCH)
using dotnow;
using dotnow.Interop;
using System;
using System.Reflection;
using AppDomain = dotnow.AppDomain;

namespace UnityEngine
{
    [CLRProxyBinding(typeof(ScriptableObject))]
    public class ScriptableObjectProxy : ScriptableObject, ICLRProxy
    {
        // Private
        private AppDomain domain = null;
        private CLRInstance instance = null;
        private MethodInfo onEnableBinding = null;
        private MethodInfo onDisableBinding = null;

        // Properties
        public CLRInstance Instance => instance;

        // Methods
        public void InitializeProxy(AppDomain domain, CLRInstance instance)
        {
            this.domain = domain;
            this.instance = instance;

            onEnableBinding = instance.Type.GetMethod("OnEnable", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, Type.DefaultBinder, Type.EmptyTypes, null);
            onDisableBinding = instance.Type.GetMethod("OnDisable", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, Type.DefaultBinder, Type.EmptyTypes, null);

            // Manually call enable after the instance was created
            OnEnable();
        }

        private void OnEnable()
        {
            if (onEnableBinding != null)
                onEnableBinding.Invoke(instance, null);
        }

        private void OnDisable()
        {
            if(onDisableBinding != null)
                onDisableBinding.Invoke(instance, null);
        }
    }
}
#endif
#endif