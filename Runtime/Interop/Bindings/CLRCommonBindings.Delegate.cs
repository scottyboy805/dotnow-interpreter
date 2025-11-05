using dotnow.Runtime;
using System;
using System.Reflection;

namespace dotnow.Interop
{
    internal static partial class CLRCommonBindings
    {
        // Methods
        [CLRCreateInstanceBinding(typeof(Action))]
        public static object CreateActionInstanceOverride(AppDomain domain, Type type, ConstructorInfo ctor, object[] args)
        {
            // Create delegate
            return __delegate.AutoActionInteropDelegate(domain, args[0], (MethodBase)args[1]);//.ActionInteropDelegate(args[0], (MethodBase)args[1]);
        }

        [CLRCreateInstanceBinding(typeof(Action<>))]
        public static object CreateActionT1InstanceOverride(AppDomain domain, Type type, ConstructorInfo ctor, object[] args)
        {
            // Create delegate
            return __delegate.AutoActionInteropDelegateFromParameters(domain, type, args[0], (MethodBase)args[1]);
        }

        [CLRCreateInstanceBinding(typeof(Action<,>))]
        public static object CreateActionT2InstanceOverride(AppDomain domain, Type type, ConstructorInfo ctor, object[] args)
        {
            // Create delegate
            return __delegate.AutoActionInteropDelegateFromParameters(domain, type, args[0], (MethodBase)args[1]);
        }

        [CLRCreateInstanceBinding(typeof(Action<,,>))]
        public static object CreateActionT3InstanceOverride(AppDomain domain, Type type, ConstructorInfo ctor, object[] args)
        {
            // Create delegate
            return __delegate.AutoActionInteropDelegateFromParameters(domain, type, args[0], (MethodBase)args[1]);
        }

        [CLRCreateInstanceBinding(typeof(Action<,,,>))]
        public static object CreateActionT4InstanceOverride(AppDomain domain, Type type, ConstructorInfo ctor, object[] args)
        {
            // __delegate cannot support more than 3 arguments, so we must fall back to useing object for all args which requires casting on the recevine end
            Action<object, object, object, object> delegateCallIntermediate = (object val0, object val1, object val2, object val3) =>
            {
                // Get instance and method target
                object instance = args[0];
                MethodBase method = (MethodBase)args[1];

                // Invoke the target method with argument
                method.Invoke(instance, new object[] { val0, val1, val2, val3 });
            };

            return delegateCallIntermediate;
        }

        [CLRCreateInstanceBinding(typeof(Func<>))]
        public static object CreateFuncInstanceOverride(AppDomain domain, Type type, ConstructorInfo ctor, object[] args)
        {
            // Create delegate
            return __delegate.AutoFuncInteropDelegateFromParameters(domain, type, args[0], (MethodBase)args[1]);
        }

        [CLRCreateInstanceBinding(typeof(Func<,>))]
        public static object CreateFuncT1InstanceOverride(AppDomain domain, Type type, ConstructorInfo ctor, object[] args)
        {
            // Create delegate
            return __delegate.AutoFuncInteropDelegateFromParameters(domain, type, args[0], (MethodBase)args[1]);
        }

        [CLRCreateInstanceBinding(typeof(Func<,,>))]
        public static object CreateFuncT2InstanceOverride(AppDomain domain, Type type, ConstructorInfo ctor, object[] args)
        {
            // Create delegate
            return __delegate.AutoFuncInteropDelegateFromParameters(domain, type, args[0], (MethodBase)args[1]);
        }

        [CLRCreateInstanceBinding(typeof(MulticastDelegate))]
        public static object CreateMulticastDelegateInstanceOverride(AppDomain domain, Type type, ConstructorInfo ctor, object[] args)
        {
            //return __delegate.AutoAnyInteropDelegateFromParametersAsType(typeof(MulticastDelegate), args[0], (MethodBase)args[1]);


            // Get method target
            MethodBase method = (MethodBase)args[1];

            int paramCount = method.GetParameters().Length;
            bool returnVal = (method as MethodInfo) != null && (method as MethodInfo).ReturnType != typeof(void);

            if (returnVal == false)
            {
                switch (paramCount)
                {
                    case 0: return CreateActionInstanceOverride(domain, type, ctor, args);
                    case 1: return CreateActionT1InstanceOverride(domain, type, ctor, args);
                    case 2: return CreateActionT2InstanceOverride(domain, type, ctor, args);
                    case 3: return CreateActionT3InstanceOverride(domain, type, ctor, args);
                    case 4: return CreateActionT4InstanceOverride(domain, type, ctor, args);
                }
            }
            else
            {
                switch (paramCount)
                {
                    case 0: return CreateFuncInstanceOverride(domain, type, ctor, args);
                    case 1: return CreateFuncT1InstanceOverride(domain, type, ctor, args);
                    case 2: return CreateFuncT2InstanceOverride(domain, type, ctor, args);
                }
            }

            throw new NotSupportedException("Delegates are limited to an argument count of 4");
        }
    }
}
