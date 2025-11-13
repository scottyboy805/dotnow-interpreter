//using dotnow;
//using dotnow.Interop;
//using System.Reflection;

//namespace System.Runtime.CompilerServices
//{
//    [CLRProxyBinding(typeof(IAsyncStateMachine))]
//    public sealed class IAsyncStateMachineProxy : ICLRProxy, IAsyncStateMachine
//    {
//        // Private
//        private CLRInstance instance;
//        private MethodInfo moveNextMethod = null;
//        private MethodInfo setStateMachineMethod = null;

//        // Properties
//        public CLRInstance Instance
//        {
//            get { return instance; }
//        }

//        // Methods
//        public void InitializeProxy(dotnow.AppDomain domain, CLRInstance instance)
//        {
//            this.instance = instance;
//        }

//        public void MoveNext()
//        {
//            // Find method
//            if(moveNextMethod == null)
//                moveNextMethod = instance.Type.GetMethod("MoveNext");

//            // Invoke
//            moveNextMethod.Invoke(instance, null);
//        }

//        public void SetStateMachine(IAsyncStateMachine stateMachine)
//        {
//            // Find method
//            if (setStateMachineMethod == null)
//                setStateMachineMethod = instance.Type.GetMethod("SetStateMachine", new Type[] { typeof(IAsyncStateMachine) });

//            // Invoke
//            setStateMachineMethod.Invoke(instance, new object[] { stateMachine });
//        }
//    }
//}
