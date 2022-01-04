using System;
using System.Reflection;
using System.Threading;
using dotnow.Runtime;

namespace dotnow.Interop
{
    internal static partial class CLRCommonBindings
    {
        // Methods
        [CLRMethodBinding(typeof(Monitor), "Enter", typeof(object))]
        public static void MonitorEnterOverride(AppDomain domain, MethodInfo overrideMethod, object instance, object[] args)
        {
            if (args[0].IsCLRInstance() == true)
            {
                Monitor.Enter(args[0].Unwrap());
            }
            else
            {
                Monitor.Enter(args[0]);
            }
        }

        [CLRMethodBinding(typeof(Monitor), "Exit", typeof(object))]
        public static void MonitorExitOverride(AppDomain domain, MethodInfo overrideMethod, object instance, object[] args)
        {
            if (args[0].IsCLRInstance() == true)
            {
                Monitor.Exit(args[0].Unwrap());
            }
            else
            {
                Monitor.Exit(args[0]);
            }
        }

        [CLRCreateInstanceBinding(typeof(Thread), new Type[] { typeof(ThreadStart) })]
        public static object CreateThreadInstanceOverride_ThreadStart(AppDomain domain, Type type, ConstructorInfo ctor, object[] args)
        {
            // Get target delegate
            MulticastDelegate threadStartDelegate = (MulticastDelegate)args[0];

            // Create new thread context
            CLRThreadContext context = new CLRThreadContext(domain, threadStartDelegate.Method, threadStartDelegate.Target);

            // Create thread
            Thread thread = new Thread(context.__ThreadMainBridge);

            // Set thread
            context.thread = thread;
            return thread;
        }

        [CLRCreateInstanceBinding(typeof(Thread), new Type[] { typeof(ThreadStart), typeof(int) })]
        public static object CreateThreadInstanceOverride_ThreadStart_MaxStack(AppDomain domain, Type type, ConstructorInfo ctor, object[] args)
        {
            // Get target delegate
            MulticastDelegate threadStartDelegate = (MulticastDelegate)args[0];
            int maxStack = (int)args[1];

            // Create new thread context
            CLRThreadContext context = new CLRThreadContext(domain, threadStartDelegate.Method, threadStartDelegate.Target, maxStack);

            // Create thread
            Thread thread = new Thread(context.__ThreadMainBridge, maxStack);

            // Set thread
            context.thread = thread;
            return thread;
        }

        [CLRCreateInstanceBinding(typeof(Thread), new Type[] { typeof(ParameterizedThreadStart) })]
        public static object CreateThreadInstanceOverride_ParameterizedThreadStart(AppDomain domain, Type type, ConstructorInfo ctor, object[] args)
        {
            // Get target delegate
            MulticastDelegate threadStartDelegate = (MulticastDelegate)args[0];

            // Create new thread context
            CLRThreadContext context = new CLRThreadContext(domain, threadStartDelegate.Method, threadStartDelegate.Target);

            // Create thread
            Thread thread = new Thread(context.__ThreadMainBridgeParameterized);

            // Set thread
            context.thread = thread;
            return thread;
        }

        [CLRCreateInstanceBinding(typeof(Thread), new Type[] { typeof(ParameterizedThreadStart), typeof(int) })]
        public static object CreateThreadInstanceOverride_ParameterizedThreadStart_MaxStack(AppDomain domain, Type type, ConstructorInfo ctor, object[] args)
        {
            // Get target delegate
            MulticastDelegate threadStartDelegate = (MulticastDelegate)args[0];
            int maxStack = (int)args[1];

            // Create new thread context
            CLRThreadContext context = new CLRThreadContext(domain, threadStartDelegate.Method, threadStartDelegate.Target, maxStack);

            // Create thread
            Thread thread = new Thread(context.__ThreadMainBridgeParameterized, maxStack);

            // Set thread
            context.thread = thread;
            return thread;
        }
    }
}
