using dotnow.Reflection;
using dotnow.Runtime.CIL;
using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace dotnow.Runtime
{
    public unsafe class ExecutionFrame
    {
        // Private
        private AppDomain domain = null;
        private ExecutionEngine engine = null;
        private ExecutionFrame parent = null;
        private MethodBase method = null;

        // Internal
        internal int instructionPtr = 0;
        internal int stackIndex = 0;
        internal int stackArgIndex = 0;
        internal int stackBaseIndex = 0;
        internal int stackMin = 0;
        internal int stackMax = 0;
        internal StackData[] stack = null;
        internal byte[] stackMemory = null;
        internal bool abort = false;

        internal StackLocal[] locals; 

        // Properties
        public ExecutionFrame Parent
        {
            get { return parent; }
        }

        public MethodBase Method
        {
            get { return method; }
        }        

        // Constructor
        public ExecutionFrame(AppDomain domain, ExecutionEngine engine, ExecutionFrame parent, MethodBase method, int maxStackDepth, int paramCount, StackLocal[] locals)
        {
            SetupFrame(domain, engine, parent, method, maxStackDepth, paramCount, locals);
        }

        // Methods
        public void SetupFrame(AppDomain domain, ExecutionEngine engine, ExecutionFrame parent, MethodBase method, int maxStackDepth, int paramCount, StackLocal[] locals)
        {
            this.domain = domain;
            this.engine = engine;
            this.locals = locals;

            // Reset ptr
            instructionPtr = 0;
            abort = false;

            int localCount = 0;

            // Get number of locals
            if (locals != null)
                localCount = locals.Length;

            this.parent = parent;
            this.method = method;
            this.stackMemory = engine.stackMemory;
            this.stack = engine.stack;

            int localAllocSize = 0;
            int localAllocPtr = (parent == null) ? 0 : parent.stackMax;


            int countLocalBytes = 0;

            if (locals != null)
            {
                // Calcualte stack size required for value types
                for (int i = 0; i < locals.Length; i++)
                    localAllocSize += locals[i].clrValueTypeSize;

                // Allocate locals
                for (int i = 0; i < locals.Length; i++)
                {
                    //throw new NotImplementedException("Requires further work");
                    if (locals[i].isCLRValueType == true)
                    {
                        __internal.__stack_alloc_inst(ref stack[i + localAllocPtr + localAllocSize], ref domain, locals[i].localType, ref localAllocPtr);
                    }
                    else
                    {
                        stack[i + localAllocPtr + localAllocSize] = locals[i].defaultValue;
                    }



                    
                }

                fixed (byte* basePtr = &stackMemory[localAllocPtr])
                {
                    byte* stackPtr = basePtr;
                    

                    for (int i = 0; i < locals.Length; i++)
                    {
                        // NEW IMPLEMENTATION
                        if (locals[i].isCLRValueType == true)
                        {

                        }
                        else
                        {
                            for(int j = 0; j < localAllocSize; j++)
                            {
                                *stackPtr = 0;
                                stackPtr++;
                            }

                            //stackPtr += localAllocSize;
                            countLocalBytes += localAllocSize;
                        }
                    }
                }
            }


            this.stackIndex = localAllocPtr + localCount;
            this.stackArgIndex = localAllocPtr + localCount;
            this.stackBaseIndex = stackArgIndex + paramCount + ((method.IsStatic == true) ? 0 : 1);
            this.stackMin = localAllocPtr;
            this.stackMax = stackBaseIndex + maxStackDepth;



            // Added after
            this.stackIndex = localAllocPtr + countLocalBytes;
            this.stackArgIndex = stackBaseIndex + countLocalBytes;
            this.stackBaseIndex = localAllocPtr + countLocalBytes;
        }

        public bool GetCurrentOperation(out CILOperation op)
        {
            if(method is CLRMethod)
            {
                // Get method body instructions
                CILOperation[] operations = ((CLRMethod)method).Body.Operations;

                // Check bounds
                if (instructionPtr >= 0 && instructionPtr < operations.Length)
                {
                    op = operations[instructionPtr];
                    return true;
                }
            }

            op = default;
            return false;
        }
    }
}
