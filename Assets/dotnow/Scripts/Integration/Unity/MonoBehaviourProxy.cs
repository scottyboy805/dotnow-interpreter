#if !UNITY_DISABLE
#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_IOS || UNITY_ANDROID || UNITY_WSA || UNITY_WEBGL)
using System;
using System.Reflection;
using dotnow;
using dotnow.Interop;
using AppDomain = dotnow.AppDomain;

namespace UnityEngine
{
#if API_NET35
    [CLRProxyBinding(typeof(MonoBehaviour))]
    public class MonoBehaviourProxy : MonoBehaviour, ICLRProxy
    {
        // Type
        private enum MethodImplementedFlags : byte
        {
            Update = 1,
            LateUpdate = 2,
            FixedUpdate = 4,
        }

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

        private MethodImplementedFlags checkedMethods = 0;
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

            // Manually call awake and OnEnable since they will do nothing when called by Unity
            Awake();
            OnEnable();
        }

        public void Awake()
        {
            // When Unity calls this method, we have not yet had chance to 'InitializeProxy'. This method will be called manually when ready.
            if (domain == null)
                return;

            if (awakeHook == null) awakeHook = instanceType.GetMethod("Awake");
            if (awakeHook != null) awakeHook.Invoke(instance, null);
        }

        public void Start()
        {
            if (startHook == null) startHook = instanceType.GetMethod("Start");
            if (startHook != null) startHook.Invoke(instance, null);
        }

        public void OnDestroy()
        {
            if (onDestroyHook == null) onDestroyHook = instanceType.GetMethod("OnDestroy");
            if (onDestroyHook != null) onDestroyHook.Invoke(instance, null);
        }

        public void OnEnable()
        {
            // When Unity calls this method, we have not yet had chance to 'InitializeProxy'. This method will be called manually when ready.
            if (domain == null)
                return;

            if (onEnableHook == null) onEnableHook = instanceType.GetMethod("OnEnable");
            if (onEnableHook != null) onEnableHook.Invoke(instance, null);
        }

        public void OnDisable()
        {
            if (onDisableHook == null) onDisableHook = instanceType.GetMethod("OnDisable");
            if (onDisableHook != null) onDisableHook.Invoke(instance, null);
        }

        public void Update()
        {
            // Check for method implemented - Update is called alot so there is extra logic here to make sure 'GetMethod' is only called once and the reuslt is cached
            if ((checkedMethods & MethodImplementedFlags.Update) == 0)
            {
                checkedMethods |= MethodImplementedFlags.Update;
                updateHook = instanceType.GetMethod("Update");
            }

            if (updateHook != null) updateHook.Invoke(instance, null);
        }

        public void LateUpdate()
        {
            // Check for method implemented - Late update is called alot so there is extra logic here to make sure 'GetMethod' is only called once and the reuslt is cached
            if ((checkedMethods & MethodImplementedFlags.LateUpdate) == 0)
            {
                checkedMethods |= MethodImplementedFlags.LateUpdate;
                lateUpdateHook = instanceType.GetMethod("LateUpdate");
            }

            if (lateUpdateHook != null) lateUpdateHook.Invoke(instance, null);
        }

        public void FixedUpdate()
        {
            // Check for method implemented - Late update is called alot so there is extra logic here to make sure 'GetMethod' is only called once and the reuslt is cached
            if ((checkedMethods & MethodImplementedFlags.FixedUpdate) == 0)
            {
                checkedMethods |= MethodImplementedFlags.FixedUpdate;
                fixedUpdateHook = instanceType.GetMethod("FixedUpdate");
            }

            if (fixedUpdateHook != null) fixedUpdateHook.Invoke(instance, null);
        }

        public void OnCollisionEnter(Collision collision)
        {
            if (onCollisionEnterHook == null) onCollisionEnterHook = instanceType.GetMethod("OnCollisionEnter", bindings);
            if (onCollisionEnterHook != null) onCollisionEnterHook.Invoke(instance, new object[] { collision });
        }

        public void OnCollisionStay(Collision collision)
        {
            if (onCollisionStayHook == null) onCollisionStayHook = instanceType.GetMethod("OnCollisionStay", bindings);
            if (onCollisionStayHook != null) onCollisionStayHook.Invoke(instance, new object[] { collision });
        }

        public void OnCollisionExit(Collision collision)
        {
            if (onCollisionExitHook == null) onCollisionExitHook = instanceType.GetMethod("OnCollisionExit", bindings);
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
            if (componentType.IsCLRType() == false)
            {
                // Use default unity behaviour
                return go.GetComponent(componentType);
            }

            // Get proxy types
            foreach (MonoBehaviourProxy proxy in go.GetComponents<MonoBehaviourProxy>())
            {
                if (proxy.instanceType == componentType)
                {
                    return proxy.instance;
                }
            }
            return null;
        }
    }
#else
    [CLRProxyBinding(typeof(MonoBehaviour))]
    public class MonoBehaviourProxy : MonoBehaviour, ICLRProxy
    {
        // Type
        private enum MethodImplementedFlags : byte
        {
            Update = 1,
            LateUpdate = 2,
            FixedUpdate = 4,
        }

        // Private
        private AppDomain domain = null;
        private CLRType instanceType = null;
        private CLRInstance instance = null;

        private BindingFlags bindings = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        private Action awakeHook = null;
        private Action startHook = null;
        private Action onDestroyHook = null;
        private Action onEnableHook = null;
        private Action onDisableHook = null;

        private MethodImplementedFlags checkedMethods = 0;
        private Action updateHook = null;
        private Action lateUpdateHook = null;
        private Action fixedUpdateHook = null;

        private MethodBase onCollisionEnterHook = null;
        private MethodBase onCollisionStayHook = null;
        private MethodBase onCollisionExitHook = null;

        // Methods
        public void InitializeProxy(AppDomain domain, CLRInstance instance)
        {
            this.domain = domain;
            this.instanceType = instance.Type;
            this.instance = instance;

            // Manually call awake and OnEnable since they will do nothing when called by Unity
            Awake();
            OnEnable();
        }

        public void Awake()
        {
            // When Unity calls this method, we have not yet had chance to 'InitializeProxy'. This method will be called manually when ready.
            if (domain == null)
                return;

            if(awakeHook == null) awakeHook = instanceType.GetMethod(nameof(Awake))?.CreateDelegate(typeof(Action), instance) as Action;
            if (awakeHook != null) awakeHook();
        }

        public void Start()
        {
            if (startHook == null) startHook = instanceType.GetMethod(nameof(Start))?.CreateDelegate(typeof(Action), instance) as Action;
            if (startHook != null) startHook();
        }

        public void OnDestroy()
        {
            if (onDestroyHook == null) onDestroyHook = instanceType.GetMethod(nameof(OnDestroy))?.CreateDelegate(typeof(Action), instance) as Action;
            if (onDestroyHook != null) onDestroyHook();
        }

        public void OnEnable()
        {
            // When Unity calls this method, we have not yet had chance to 'InitializeProxy'. This method will be called manually when ready.
            if (domain == null)
                return;

            if (onEnableHook == null) onEnableHook = instanceType.GetMethod(nameof(OnEnable))?.CreateDelegate(typeof(Action), instance) as Action;
            if (onEnableHook != null) onEnableHook();
        }

        public void OnDisable()
        {
            if (onDisableHook == null) onDisableHook = instanceType.GetMethod(nameof(OnDisable))?.CreateDelegate(typeof(Action), instance) as Action;
            if (onDisableHook != null) onDisableHook();
        }

        public void Update()
        {
            // Check for method implemented - Update is called alot so there is extra logic here to make sure 'GetMethod' is only called once and the reuslt is cached
            if((checkedMethods & MethodImplementedFlags.Update) == 0)
            {
                checkedMethods |= MethodImplementedFlags.Update;
                updateHook = instanceType.GetMethod(nameof(Update))?.CreateDelegate(typeof(Action), instance) as Action;
            }

            if (updateHook != null) updateHook();
        }

        public void LateUpdate()
        {
            // Check for method implemented - Late update is called alot so there is extra logic here to make sure 'GetMethod' is only called once and the reuslt is cached
            if ((checkedMethods & MethodImplementedFlags.LateUpdate) == 0)
            {
                checkedMethods |= MethodImplementedFlags.LateUpdate;
                lateUpdateHook = instanceType.GetMethod(nameof(LateUpdate))?.CreateDelegate(typeof(Action), instance) as Action;
            }

            if (lateUpdateHook != null) lateUpdateHook();
        }

        public void FixedUpdate()
        {
            // Check for method implemented - Late update is called alot so there is extra logic here to make sure 'GetMethod' is only called once and the reuslt is cached
            if ((checkedMethods & MethodImplementedFlags.FixedUpdate) == 0)
            {
                checkedMethods |= MethodImplementedFlags.FixedUpdate;
                fixedUpdateHook = instanceType.GetMethod(nameof(FixedUpdate))?.CreateDelegate(typeof(Action), instance) as Action;
            }

            if (fixedUpdateHook != null) fixedUpdateHook();
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
#endif
}
#endif
#endif