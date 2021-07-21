using System.Reflection;
using TrivialCLR.Interop;
using UnityEngine;

namespace TrivialCLR.Example
{
    [CLRProxyBinding(typeof(SnakeGameBase))]
    public class SnakeGameBaseProxy : SnakeGameBase, ICLRProxy
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

            // Manually call Awake and OnEnable
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
    }
}
