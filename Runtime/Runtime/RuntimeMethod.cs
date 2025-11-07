using dotnow.Runtime.CIL;
using System;

namespace dotnow.Runtime
{
    internal readonly ref struct RuntimeMethod
    {
        // Private
        private readonly ThreadContext threadContext;
        private readonly AssemblyLoadContext assemblyLoadContext;
        private readonly CILMethodInfo methodInfo;

        // Constructor
        public RuntimeMethod(ThreadContext threadContext, AssemblyLoadContext assemblyLoadContext, in CILMethodInfo methodInfo)
        {
            // Check for null
            if (threadContext == null)
                throw new ArgumentNullException(nameof(threadContext));

            if (assemblyLoadContext == null)
                throw new ArgumentNullException(nameof(assemblyLoadContext));

            this.threadContext = threadContext;
            this.assemblyLoadContext = assemblyLoadContext;
            this.methodInfo = methodInfo;
        }

        // Methods
        public object ReflectionInvoke(object obj, object[] args)
        {
            // Perform checks
            CheckMethod();

            // Check for instance
            if ((methodInfo.Flags & CILMethodFlags.This) != 0 && obj == null)
                threadContext.Throw<NullReferenceException>();

            // Get stack pointers
            int spArg = 0;

            // Load the instance and arguments onto the stack
            threadContext.PushReflectionMethodFrame(assemblyLoadContext.AppDomain, methodInfo, obj, args, out spArg);

            // Execute method with interpreter
            int spReturn = CILInterpreter.ExecuteMethod(threadContext, assemblyLoadContext, methodInfo, spArg);

            // Complete the method frame
            threadContext.PopMethodFrame();

            object returnValue = null;

            // Check for return type
            if ((methodInfo.Flags & CILMethodFlags.Return) != 0)
            {
                // Get return type handle
                CILTypeInfo returnTypeInfo = methodInfo.ReturnType;

                // Attempt to unwrap the resulting type for interop
                StackData.Unwrap(returnTypeInfo, ref threadContext.stack[spReturn], ref returnValue);
            }

            // No return value
            return returnValue;
        }

        private void CheckMethod()
        {
            // Check for abstract
            if ((methodInfo.Flags & CILMethodFlags.Abstract) != 0)
                threadContext.Throw(new InvalidOperationException("Cannot invoke and abstract method"));

            // Check for no body
            if ((methodInfo.Flags & CILMethodFlags.Body) == 0 || (methodInfo.Flags & CILMethodFlags.Native) != 0)
                threadContext.Throw(new InvalidOperationException("Cannot invoke a method that is external or has no body"));
        }
    }
}
