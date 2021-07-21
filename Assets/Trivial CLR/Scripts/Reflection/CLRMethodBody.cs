using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using Mono.Cecil;
using TrivialCLR.Runtime;
using TrivialCLR.Runtime.CIL;
using TrivialCLR.Runtime.JIT;
using MethodBody = Mono.Cecil.Cil.MethodBody;

namespace TrivialCLR.Reflection
{
    public sealed class CLRMethodBody
    {
        // Private
        private AppDomain domain = null;
        private MethodBase method = null;
        private MethodBody body = null;
        private MethodBodyCompiler bodyCompiler = null;      
        private CILOperation[] instructions = null;
        private Lazy<CLRExceptionHandler[]> exceptionHandlers = null;
        private Lazy<StackLocal[]> locals = null;
        private bool jitFailed = false;

        // Properties
        public MethodBase Method
        {
            get { return method; }
        }

        public bool InitLocals
        {
            get { return body.InitLocals; }
        }

        public int MaxStack
        {
            get { return body.MaxStackSize; }
        }

        public CLRExceptionHandler[] ExceptionHandlers
        {
            get { return exceptionHandlers.Value; }
        }

        public StackLocal[] Locals
        {
            get { return locals.Value; }
        }

        // Constructor
        internal CLRMethodBody(AppDomain domain, MethodBase method, MethodBody body)
        {
            this.domain = domain;
            this.method = method;
            this.body = body;
            this.bodyCompiler = new MethodBodyCompiler(body);

            // Lazy initialize
            this.locals = new Lazy<StackLocal[]>(InitLocalDefaults);

            // Exception handlers
            this.exceptionHandlers = new Lazy<CLRExceptionHandler[]>(InitExceptionHandlers);
        }

        // Methods
        public void ExecuteMethodBody(ExecutionEngine engine, ExecutionFrame frame)
        {
            // Create instructions
            if (instructions == null)
            {
                lock (bodyCompiler)
                {
                    // Check for jit failed
                    if (jitFailed == true)
                        throw new InvalidProgramException(string.Format("JIT compilation failed to run for the target method body: {0}. The method may use instructions or features that are not supported", method));

                    Type genericContext = null;

                    // Check for generic type
                    if (Method.DeclaringType.IsGenericType == true)
                        genericContext = Method.DeclaringType;

                    using (new GenericContext(genericContext))
                    {
                        try
                        {
                            // Get the compiled method body instruction pointer
                            instructions = bodyCompiler.JITOptimizeInterpretedInstructionSet(domain);
                        }
                        catch
                        {
                            jitFailed = true;
                            throw;
                        }
                    }
                }
            }

            // Check for error
            if (instructions == null)
                throw new InvalidProgramException("Failed to JIT compile method body: " + method);

            // Profiling entry
#if UNITY && DEBUG && PROFILE
            UnityEngine.Profiling.Profiler.BeginSample(string.Concat("[CLR Interpreted] ", Method.DeclaringType.Name, ".", Method.Name, "()"));
#endif

            // Run interpreted
            engine.Execute(domain, frame, instructions, exceptionHandlers.Value);

            // Profiling entry
#if UNITY && DEBUG && PROFILE
            UnityEngine.Profiling.Profiler.EndSample();
#endif
        }

        private StackLocal[] InitLocalDefaults()
        {
            // Allocate locals
            StackLocal[] locals = new StackLocal[body.Variables.Count];

            // Initialize values
            for(int i = 0; i < locals.Length; i++)
            {
                Type localType = null;

                // Resolve generics
                if(method.ContainsGenericParameters == true)
                {
                    GenericParameter parameter = body.Variables[i].VariableType as GenericParameter;

                    if (parameter != null)
                    {
                        localType = method.DeclaringType.GenericTypeArguments[parameter.Position];
                    }
                }

                // Resolve the type
                if (localType == null)
                {
                    localType = domain.ResolveType(body.Variables[i].VariableType);
                }

                // Create the local
                locals[i] = new StackLocal(domain, localType);
            }

            return locals;
        }

        private CLRExceptionHandler[] InitExceptionHandlers()
        {
            CLRExceptionHandler[] handlers = new CLRExceptionHandler[body.ExceptionHandlers.Count];

            for(int i = 0; i < handlers.Length; i++)
            {
                handlers[i] = new CLRExceptionHandler(domain, this, body, body.ExceptionHandlers[i]);
            }

            return handlers;
        }
    }
}
