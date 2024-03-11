using System;
using System.Reflection;
using System.Threading;

namespace dotnow.Runtime
{
    internal sealed class CLRThreadContext
    {
        // Internal
        internal Thread thread = null;
        internal ExecutionEngine engine = null;

        // Private
        private AppDomain domain = null;
        private MethodInfo targetMethod = null;
        private object targetInstance = null;
        private int maxStack = 0;

        // Constructor
        public CLRThreadContext(AppDomain domain, MethodInfo targetMethod, object targetInstance, int maxStack = 0)
        {
            this.domain = domain;
            this.targetMethod = targetMethod;
            this.targetInstance = targetInstance;
            this.maxStack = maxStack;
        }

        public CLRThreadContext(AppDomain domain, int maxStack)
        {
            this.domain = domain;
            this.maxStack = maxStack;

            // Make sure we are not on main
            if (domain.mainThread == Thread.CurrentThread)
                throw new InvalidOperationException("Thread context cannot be created from main thread. Make sure you are calling from worker thread or use `CreateThreadInstanceOverride_ThreadStart`/`CreateThreadInstanceOverride_ParameterizedThreadStart`");

            // Store thread state
            thread = Thread.CurrentThread;

            // Create engine and thread context - we must already be on worker thread
            engine = new ExecutionEngine(thread, maxStack);
        }

        // Methods
        internal void __ThreadMainBridge()
        {
            // Get thread
            thread = Thread.CurrentThread;

            // Create engine
            engine = new ExecutionEngine(thread, maxStack);

            // Create thread context for domain - required to support reflection on separate threads etc.
            domain.CreateThreadExecutionContext(this);


            // ### This method is called by 'Thread' when 'Start' is called in interpreted or interop code
            // Execute the method on the thread
            // Interpreted methods will go through CLRMethod -> CLRMethodBody -> GetExecutionEngine (For current thread) -> Interpret on this thread
            targetMethod.Invoke(targetInstance, null);


            // The thread method has exited - We can safely release the thread
            domain.DestroyThreadExecutionContext(this);
        }

        internal void __ThreadMainBridgeParameterized(object parameter)
        {
            // Create engine
            engine = new ExecutionEngine(thread, maxStack);

            // Create thread context for domain - required to support reflection on separate threads etc.
            domain.CreateThreadExecutionContext(this);

            // ### This method is called by 'Thread' when 'Start' is called in interpteted or interop code
            // Execute the method on the thread
            // Interpreted methods will go through CLRMethod -> CLRMethodBody -> GetExecutionEngine (For current thread) -> Interpret on this thread
            targetMethod.Invoke(targetInstance, new object[] { parameter });


            // The thread method has exited - We can safely release the thread
            domain.DestroyThreadExecutionContext(this);
        }

        internal void __InternalAbortThread()
        {
            if (thread.IsAlive == true)
            {
                // Wait for thread join
                thread.Join(500);

                // Check for not dead
                if (thread.IsAlive == true)
                {
                    try
                    {
                        thread.Abort();
                    }
                    catch(NotSupportedException)
                    {
                        throw engine.CreateException(new ApplicationException(string.Format("Failed to terminate thread with id '{0}'!", thread.ManagedThreadId)));
                    }
                }
            }
        }
    }
}
