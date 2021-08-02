using System;
using System.Reflection;
using dotnow;
using dotnow.Interop;
using AppDomain = dotnow.AppDomain;

namespace UnityEngine
{
    [CLRProxyBinding(typeof(MonoBehaviour))]
    public class MonoBehaviourProxy : MonoBehaviour, ICLRProxy
    {
        // Private
        private AppDomain domain = null;
        private CLRType instanceType = null;
        private CLRInstance instance = null;

        private BindingFlags bindings = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
        private MethodBase awakeHook = null;
        private MethodBase startHook = null;
        private MethodBase onDestroyHook = null;
        private MethodBase onEnableHook = null;
        private MethodBase onDisableHook = null;
        private MethodBase updateHook = null;
        private MethodBase lateUpdateHook = null;
        private MethodBase fixedUpdateHook = null;
        private MethodBase onCollisionEnterHook = null;
        private MethodBase onCollisionStayHook = null;
        private MethodBase onCollisionExitHook = null;

        // Methods
        public void InitializeProxy(AppDomain domain, CLRInstance instance)
        {
            this.domain = domain;
            this.instanceType = instance.Type;
            this.instance = instance;

            // Manually call awake and OnEnable since they will do nother when called by Unity
            Awake();
            OnEnable();
        }

        public void Awake()
        {
            if (domain == null)
                return;

            if (awakeHook == null) awakeHook = instanceType.GetMethod(nameof(Awake), bindings);
            if (awakeHook != null) awakeHook.Invoke(instance, null);
        }

        public void Start()
        {
            if (startHook == null) startHook = instanceType.GetMethod(nameof(Start), bindings);
            if (startHook != null) startHook.Invoke(instance, null);
        }

        public void OnDestroy()
        {
            if (onDestroyHook == null) onDestroyHook = instanceType.GetMethod(nameof(OnDestroy), bindings);
            if (onDestroyHook != null) onDestroyHook.Invoke(instance, null);
        }

        public void OnEnable()
        {
            if (domain == null)
                return;

            if (onEnableHook == null) onEnableHook = instanceType.GetMethod(nameof(OnEnable), bindings);
            if (onEnableHook != null) onEnableHook.Invoke(instance, null);
        }

        public void OnDisable()
        {
            if (onDisableHook == null) onDisableHook = instanceType.GetMethod(nameof(OnDisable), bindings);
            if (onDisableHook != null) onDisableHook.Invoke(instance, null);
        }

        public void Update()
        {
            if (updateHook == null) updateHook = instanceType.GetMethod(nameof(Update), bindings);
            if (updateHook != null) updateHook.Invoke(instance, null);
        }

        public void LateUpdate()
        {
            if (lateUpdateHook == null) lateUpdateHook = instanceType.GetMethod(nameof(LateUpdate), bindings);
            if (lateUpdateHook != null) lateUpdateHook.Invoke(instance, null);
        }

        public void FixedUpdate()
        {
            if (fixedUpdateHook == null) fixedUpdateHook = instanceType.GetMethod(nameof(FixedUpdate), bindings);
            if (fixedUpdateHook != null) fixedUpdateHook.Invoke(instance, null);
        }

        public void OnCollisionEnter(Collision collision)
        {
            if (onCollisionEnterHook == null) onCollisionEnterHook = instanceType.GetMethod(nameof(OnCollisionEnter), bindings);
            if (onCollisionEnterHook != null) onCollisionEnterHook.Invoke(instance, new object[] { collision });
        }

        public void OnCollisionStay(Collision collision)
        {
            if (onCollisionStayHook == null) onCollisionStayHook = instanceType.GetMethod(nameof(OnCollisionStay), bindings);
            if (onCollisionStayHook != null) onCollisionStayHook.Invoke(instance, new object[] { collision });
        }

        public void OnCollisionExit(Collision collision)
        {
            if (onCollisionExitHook == null) onCollisionExitHook = instanceType.GetMethod(nameof(OnCollisionExit), bindings);
            if (onCollisionExitHook != null) onCollisionExitHook.Invoke(instance, new object[] { collision });
        }

        [CLRMethodBinding(typeof(GameObject), "AddComponent", typeof(Type))]
        public static object AddComponentOverride(AppDomain domain, MethodInfo overrideMethod, object instance, object[] args)
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

        [CLRMethodBinding(typeof(GameObject), "GetComponent", typeof(Type))]
        public static object GetComponentOverride(AppDomain domain, MethodInfo overrideMethod, object instance, object[] args)
        {
            // Get instance
            GameObject go = instance as GameObject;

            // Get argument
            Type componentType = args[0] as Type;

            // Check for clr type
            if(componentType.IsCLRType() == false)
            {
                // Use default unity behaviour
                return go.GetComponent(componentType);
            }

            // Get proxy types
            foreach(MonoBehaviourProxy proxy in go.GetComponents<MonoBehaviourProxy>())
            {
                if(proxy.instanceType == componentType)
                {
                    return proxy.instance;
                }
            }
            return null;
        }
    }
}
