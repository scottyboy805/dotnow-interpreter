using dotnow.Reflection;
using dotnow.Runtime.CIL;
using dotnow.Runtime.Handle;
using System;
using System.Reflection;

namespace dotnow.Runtime
{
    //public unsafe struct ExecutionFrame
    //{
    //    // Private
    //    private ExecutionFrame* parent;
    //    private ExecutionEngine engine;
    //    private MethodBase method;

    //    // Internal
    //    internal byte* instructionPtr;
    //    internal byte* stackBasePtr;
    //    internal _CLRStackHandle[] args;
    //    internal _CLRStackHandle[] locals;
    //    internal bool abort;

    //    // Constructor
    //    public ExecutionFrame(ExecutionFrame* parent, AppDomain domain, MethodBase method)
    //    {
    //        this.parent = parent;
    //        this.engine = parent != null ? parent->engine : domain.GetExecutionEngine();
    //        this.method = method;
    //    }
    //}

    public unsafe class ExecutionFrameOld
    {
        // Private
        private AppDomain domain = null;
        private ExecutionEngine engine = null;
        private ExecutionFrameOld parent = null;
        private MethodBase method = null;

        // Internal
        internal int instructionPtr = 0;
        internal int stackIndex = 0;
        internal int stackArgIndex = 0;
        internal int stackBaseIndex = 0;
        internal int stackMin = 0;
        internal int stackMax = 0;
        internal StackData[] stack = null;
        internal IntPtr stackMemory = IntPtr.Zero;
        internal bool abort = false;

        internal StackLocal[] locals;
        internal _CLRStackHandle[] stackLocals;
        internal uint stackBaseOffset = 0;

        internal byte* stackPtr = null;

        // Properties
        public ExecutionFrameOld Parent
        {
            get { return parent; }
        }

        public MethodBase Method
        {
            get { return method; }
        }        

        // Constructor
        public ExecutionFrameOld(AppDomain domain, ExecutionEngine engine, ExecutionFrameOld parent, MethodBase method, int maxStackDepth, int paramCount, StackLocal[] locals)
        {
            SetupFrame(domain, engine, parent, method, maxStackDepth, paramCount, locals);
        }

        // Methods
        public void SetupFrame(AppDomain domain, ExecutionEngine engine, ExecutionFrameOld parent, MethodBase method, int maxStackDepth, int paramCount, StackLocal[] locals)
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
                    localAllocSize += locals[i].localSize;

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


                stackLocals = new _CLRStackHandle[locals.Length];

                stackBaseOffset = 0;
                for(int i = 0; i < stackLocals.Length; i++)
                {
                    // Create local
                    stackLocals[i] = new _CLRStackHandle(locals[i].localType, stackBaseOffset, false);

                    // Update offset
                    stackBaseOffset += stackLocals[i].stackType.size;
                }



                byte* basePtr = (byte*)stackMemory; //&stackMemory[localAllocPtr])
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
