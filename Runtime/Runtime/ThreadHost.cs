using System;
using System.Threading;

namespace dotnow.Runtime
{
    /// <summary>
    /// Used for launching new managed threads in interpreted code via `Thread .ctor`.
    /// </summary>
    internal readonly struct ThreadHost
    {
        // Private
        private readonly AppDomain domain;
        private readonly Delegate threadCall;
        private readonly int maxStack;

        // Constructor
        internal ThreadHost(AppDomain domain, Delegate threadCall, int maxStack)
        {
            this.domain = domain;
            this.threadCall = threadCall;
            this.maxStack = maxStack;
        }

        // Methods
        internal void LaunchThread()
        {
            // Get execution context for thread
            ThreadContext context = domain.GetThreadContext(maxStack);
            
            // Call the delegate to start execution on this thread
            ((ThreadStart)threadCall)();
        }

        internal void LaunchThreadParameterized(object parameter)
        {
            // Get execution context for thread
            ThreadContext context = domain.GetThreadContext(maxStack);

            // Call the delegate with parameter
            ((ParameterizedThreadStart)threadCall)(parameter);
        }
    }
}
