using System;
using System.Reflection;
using dotnow.Runtime;
using dotnow.Runtime.CIL;

namespace dotnow.Reflection
{
    public abstract class CLRMethodBodyBase
    {
        // Private
        private CILOperation[] cachedInstructions = null;
        private bool getInstructionsFailed = false;

        // Protected
        protected readonly AppDomain domain = null;
        protected readonly MethodBase method = null;
        protected Lazy<CILOperation[]> operations = null;
        protected Lazy<CLRExceptionHandler[]> exceptionHandlers = null;
        protected Lazy<StackLocal[]> locals = null;

        // Properties
        public MethodBase Method
        {
            get { return method; }
        }

        public CILOperation[] Operations
        {
            get { return operations.Value; }
        }

        public CLRExceptionHandler[] ExceptionHandlers
        {
            get { return exceptionHandlers.Value; }
        }

        public StackLocal[] Locals
        {
            get { return locals.Value; }
        }

        public abstract bool InitLocals { get; }

        public abstract int MaxStack { get; }

        // Constructor
        protected CLRMethodBodyBase(AppDomain domain, MethodBase method)
        {
            this.domain = domain;
            this.method = method;

            // Operations init
            this.operations = new Lazy<CILOperation[]>(InitOperations);

            // Handlers init
            this.exceptionHandlers = new Lazy<CLRExceptionHandler[]>(InitExceptionHandlers);

            // Locals init
            this.locals = new Lazy<StackLocal[]>(InitLocalDefaults);
        }

        // Methods
        public void ExecuteMethodBody(ExecutionEngine engine, ExecutionFrame frame)
        {
            // Try to get the instructions
            if(cachedInstructions == null && getInstructionsFailed == false)
            {
                // Get the operations
                cachedInstructions = Operations;
                getInstructionsFailed = cachedInstructions == null;
            }

            // Check for error
            if (cachedInstructions == null)
                throw new InvalidProgramException("Failed to get method body instructions for method: " + method);

#if !UNITY_DISABLE
#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_IOS || UNITY_ANDROID || UNITY_WSA || UNITY_WEBGL) && UNITY_PROFILE
            UnityEngine.Profiling.Profiler.BeginSample(string.Concat("[CLR Interpreted] ", Method.DeclaringType.Name, ".", Method.Name, "()"));
#endif
#endif

            // Run interpreted
            engine.Execute(domain, frame, cachedInstructions, exceptionHandlers.Value);

            // Profiling entry
#if !UNITY_DISABLE
#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_IOS || UNITY_ANDROID || UNITY_WSA || UNITY_WEBGL) && UNITY_PROFILE
            UnityEngine.Profiling.Profiler.EndSample();
#endif
#endif
        }

        protected abstract CILOperation[] InitOperations();

        protected abstract StackLocal[] InitLocalDefaults();

        protected abstract CLRExceptionHandler[] InitExceptionHandlers();
    }
}
