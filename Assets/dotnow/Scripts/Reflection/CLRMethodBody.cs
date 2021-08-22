using System;
using System.Reflection;
using Mono.Cecil;
using dotnow.Runtime;
using dotnow.Runtime.CIL;
using dotnow.Runtime.JIT;
using MethodBody = Mono.Cecil.Cil.MethodBody;

namespace dotnow.Reflection
{
    public sealed class CLRMethodBody : CLRMethodBodyBase, IJITOptimizable
    {
        // Private
        private MethodBody body = null;
        private MethodBodyCompiler bodyCompiler = null;      
        private CILOperation[] instructions = null;
        private bool jitFailed = false;

        // Properties
        public override bool InitLocals
        {
            get { return body.InitLocals; }
        }

        public override int MaxStack
        {
            get { return body.MaxStackSize; }
        }

        // Constructor
        internal CLRMethodBody(AppDomain domain, MethodBase method, MethodBody body)
            : base(domain, method)
        {
            this.body = body;
            this.bodyCompiler = new MethodBodyCompiler(body);

            // Lazy initialize
            this.locals = new Lazy<StackLocal[]>(InitLocalDefaults);

            // Exception handlers
            this.exceptionHandlers = new Lazy<CLRExceptionHandler[]>(InitExceptionHandlers);
        }

        // Methods
        void IJITOptimizable.EnsureJITOptimized()
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
        }

        protected override CILOperation[] InitOperations()
        {
            JITOptimize.EnsureJITOptimized(this);
            return instructions;
        }

        protected override StackLocal[] InitLocalDefaults()
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

        protected override CLRExceptionHandler[] InitExceptionHandlers()
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
