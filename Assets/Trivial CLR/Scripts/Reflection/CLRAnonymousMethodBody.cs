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
    internal sealed class CLRAnonymousMethodBody : CLRMethodBodyBase
    {
        // Private
        private static readonly CLRExceptionHandler[] defaultExceptionHandlers = new CLRExceptionHandler[0];

        private bool initLocals = false;
        private int maxStack = 0;
        private Type[] localTypes = null;
        private CILOperation[] instructions = null;

        // Properties
        public override bool InitLocals
        {
            get { return initLocals; }
        }

        public override int MaxStack
        {
            get { return maxStack; }
        }

        // Constructor
        internal CLRAnonymousMethodBody(AppDomain domain, MethodBase method, CILOperation[] instructions, bool initLocals, int maxStack, Type[] localTypes)
            : base(domain, method)
        {
            this.instructions = instructions;
            this.initLocals = initLocals;
            this.maxStack = maxStack;
            this.localTypes = localTypes;

            // Lazy initialize
            this.locals = new Lazy<StackLocal[]>(InitLocalDefaults);
        }

        // Methods
        protected override CILOperation[] InitOperations()
        {
            return instructions;
        }

        protected override StackLocal[] InitLocalDefaults()
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

        protected override CLRExceptionHandler[] InitExceptionHandlers()
        {
            // Not supported at the moment
            return defaultExceptionHandlers;
        }
    }
}
