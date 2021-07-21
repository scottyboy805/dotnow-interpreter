using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TrivialCLR.Runtime;
using TrivialCLR.Runtime.CIL;

namespace TrivialCLR.Reflection
{
    internal sealed class CLRAnonymousMethodBody
    {
        // Private
        private static readonly CLRExceptionHandler[] defaultExceptionHandlers = new CLRExceptionHandler[0];

        private AppDomain domain = null;
        private MethodBase method = null;
        private bool initLocals = false;
        private int maxStack = 0;
        private Type[] localTypes = null;
        private CILOperation[] instructions = null;
        private Lazy<StackLocal[]> locals = null;

        // Properties
        public MethodBase Method
        {
            get { return method; }
        }

        public bool InitLocals
        {
            get { return initLocals; }
        }

        public int MaxStack
        {
            get { return maxStack; }
        }

        public StackLocal[] Locals
        {
            get { return locals.Value; }
        }

        // Constructor
        internal CLRAnonymousMethodBody(AppDomain domain, MethodBase method, CILOperation[] instructions, bool initLocals, int maxStack, Type[] localTypes)
        {
            this.domain = domain;
            this.method = method;
            this.instructions = instructions;
            this.initLocals = initLocals;
            this.maxStack = maxStack;
            this.localTypes = localTypes;

            // Lazy initialize
            this.locals = new Lazy<StackLocal[]>(InitLocalDefaults);
        }

        // Methods
        public void ExecuteMethodBody(ExecutionEngine engine, ExecutionFrame frame)
        {
            // Profiling entry
#if UNITY && DEBUG && PROFILE
            UnityEngine.Profiling.Profiler.BeginSample(string.Concat("[CLR Interpreted] ", Method.DeclaringType.Name, ".", Method.Name, "()"));
#endif

            // Run interpreted
            engine.Execute(domain, frame, instructions, defaultExceptionHandlers);

            // Profiling entry
#if UNITY && DEBUG && PROFILE
            UnityEngine.Profiling.Profiler.EndSample();
#endif
        }

        private StackLocal[] InitLocalDefaults()
        {
            // Allocate locals
            StackLocal[] locals = new StackLocal[localTypes.Length];

            // Initialize values
            for (int i = 0; i < locals.Length; i++)
            {
                // Create local
                locals[i] = new StackLocal(domain, localTypes[i]);
            }

            return locals;
        }
    }
}
