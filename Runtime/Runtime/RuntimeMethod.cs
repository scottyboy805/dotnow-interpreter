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
            object returnValue = null;

            // Load the instance and arguments onto the stack
            threadContext.PushReflectionMethodFrame(assemblyLoadContext.AppDomain, methodInfo, obj, args, out spArg);

            try
            {
                // Execute method with interpreter
                int spReturn = Invoke(threadContext, assemblyLoadContext, methodInfo, spArg);


                // Check for return type
                if ((methodInfo.Flags & CILMethodFlags.Return) != 0)
                {
                    // Get return type handle
                    CILTypeInfo returnTypeInfo = methodInfo.ReturnType;

                    // Attempt to unwrap the resulting type for interop
                    StackData.Unwrap(returnTypeInfo, threadContext.stack[spReturn], ref returnValue);
                }
            }
            finally
            {
                // Complete the method frame
                // Must run after return value is extracted because it performs stack cleanup
                threadContext.PopMethodFrame();
            }

            // No return value
            return returnValue;
        }

        internal static int Invoke(ThreadContext threadContext, AssemblyLoadContext loadContext, CILMethodInfo method, int spArg)
        {
            int pc = 0;         // Instruction pointer
            int spReturn = 0;   // Stack return pointer

            // Handle any runtime exceptions thrown by user or interpreter code
            try
            {
                // Execute the bytecode
                spReturn = CILInterpreter.ExecuteMethodBytecode(threadContext, loadContext, method, ref pc, spArg);
            }
            catch (Exception e)
            {
                // Try to get the handler
                if (GetHandler(method, e, pc, out CILExceptionHandlerInfo handler) == true)
                {
                    // Jump to handler offset
                    pc = handler.HandlerOffset;

                    // Execute the handler portion
                    spReturn = CILInterpreter.ExecuteMethodBytecode(threadContext, loadContext, method, ref pc, spArg);
                }
                // No handler available - just throw the exception back to the caller
                else
                {
                    // Rethrow
                    throw e;
                }
            }
            finally
            {

            }

            // Get stack return pointer
            return spReturn;
        }

        internal static bool GetHandler(CILMethodInfo method, Exception exception, int pc, out CILExceptionHandlerInfo handler)
        {

            // No handler available
            handler = default;
            return false;
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
