using dotnow;
using dotnow.Interop;
using System;
using System.Reflection;
using AppDomain = dotnow.AppDomain;

namespace UnityEngine
{
    [CLRProxyBinding(typeof(MonoBehaviour))]
    public class MonoBehaviourProxy : MonoBehaviour, ICLRProxy
    {
        // Private
        private ICLRInstance instance;
        private MethodBase startMethod;
        private MethodBase updateMethod;

        // Properties
        public ICLRInstance Instance => instance;

        // Methods
        public void Initialize(AppDomain appDomain, Type type, ICLRInstance instance)
        {
            this.instance = instance;

            this.startMethod = type.GetMethod(nameof(Start), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            this.updateMethod = type.GetMethod(nameof(Update), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            // Manually call awake and OnEnable since they will do nothing when called by Unity
            Awake();
            //OnEnable();
        }

        public void Awake()
        {
            //// When Unity calls this method, we have not yet had chance to 'InitializeProxy'. This method will be called manually when ready.
            //if (string.IsNullOrEmpty(typeInfo) == false)
            //{
            //    InstantiateProxy();
            //}
            //else
            //{
            //    if (domain == null)
            //        return;

            //    cache.InvokeProxyMethod(0, nameof(Awake));
            //}
        }

        public void Start()
        {
            //cache.InvokeProxyMethod(1, nameof(Start));
            startMethod?.Invoke(instance, null);
        }

        //public void OnDestroy()
        //{
        //    cache.InvokeProxyMethod(2, nameof(OnDestroy));
        //}

        //public void OnEnable()
        //{
        //    // When Unity calls this method, we have not yet had chance to 'InitializeProxy'. This method will be called manually when ready.
        //    if (domain == null)
        //        return;

        //    // Set type info on enable
        //    if (gameObject.activeInHierarchy == true)
        //        this.typeInfo = instance?.Type.Name;

        //    cache.InvokeProxyMethod(3, nameof(OnEnable));
        //}

        //public void OnDisable()
        //{
        //    cache.InvokeProxyMethod(4, nameof(OnDisable));
        //}

        public void Update()
        {
            //cache.InvokeProxyMethod(5, nameof(Update));
            updateMethod?.Invoke(instance, null);
        }

        //public void LateUpdate()
        //{
        //    cache.InvokeProxyMethod(6, nameof(LateUpdate));
        //}

        //public void FixedUpdate()
        //{
        //    cache.InvokeProxyMethod(7, nameof(FixedUpdate));
        //}

        //public void OnCollisionEnter(Collision collision)
        //{
        //    cache.InvokeProxyMethod(8, nameof(OnCollisionEnter), new object[] { collision });
        //}

        //public void OnCollisionStay(Collision collision)
        //{
        //    cache.InvokeProxyMethod(9, nameof(OnCollisionStay), new object[] { collision });
        //}

        //public void OnCollisionExit(Collision collision)
        //{
        //    cache.InvokeProxyMethod(10, nameof(OnCollisionExit), new object[] { collision });
        //}

        //public void OnGUI()
        //{
        //    cache.InvokeProxyMethod(11, nameof(OnGUI));
        //}

        public static Component AddComponent(AppDomain domain, Type type, GameObject go)
        {
            if(type.IsCLRType() == false)
                return go.AddComponent(type);

            MonoBehaviourProxy proxy = go.AddComponent<MonoBehaviourProxy>();

            domain.CreateInstanceFromProxy(type, proxy);

            return proxy;
        }
    }
}