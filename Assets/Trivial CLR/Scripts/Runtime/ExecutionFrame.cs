using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TrivialCLR.Reflection;
using TrivialCLR.Runtime.CIL;

namespace TrivialCLR.Runtime
{
    public class ExecutionFrame
    {
        // Private
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
        internal bool abort = false;

        // Properties
        public MethodBase Method
        {
            get { return method; }
        }        

        // Constructor
        public ExecutionFrame(AppDomain domain, ExecutionEngine engine, ExecutionFrame parent, MethodBase method, int maxStackDepth, int paramCount, StackLocal[] locals)
        {
            int localCount = 0;

            // Get number of locals
            if (locals != null)
                localCount = locals.Length;

            this.engine = engine;
            this.parent = parent;
            this.method = method;
            this.stack = engine.stack; //new StackData[stackIndex + maxStackDepth];

            int localAllocSize = 0;
            int localAllocPtr = (parent == null) ? 0 : parent.stackMax;

            if (locals != null)
            {
                // Calcualte stack size required for value types
                for (int i = 0; i < locals.Length; i++)
                    localAllocSize += locals[i].clrValueTypeSize;

                // Allocate locals
                for (int i = 0; i < locals.Length; i++)
                {
                    if (locals[i].isCLRValueType == true)
                    {
                        __internal.__stack_alloc_inst(ref stack[i + localAllocPtr + localAllocSize], ref domain, locals[i].localType, ref localAllocPtr);
                    }
                    else
                    {
                        stack[i + localAllocPtr + localAllocSize] = locals[i].defaultValue;
                    }
                }
            }


            this.stackIndex = localAllocPtr + localCount;
            this.stackArgIndex = localAllocPtr + localCount;
            this.stackBaseIndex = stackArgIndex + paramCount + ((method.IsStatic == true) ? 0 : 1);
            this.stackMin = localAllocPtr;// + ((parent == null) ? 0 : parent.stackMax);
            this.stackMax = stackBaseIndex + maxStackDepth;
            
            
            // Copy locals
            //if (locals != null)
            //{
            //    Array.Copy(locals, stack, localCount);
            //}
        }

        // Methods
        #region Stack
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //public void PushNull()
        //{
        //    stack[stackIndex] = StackData.nullPtr;
        //    stackIndex++;
        //}

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //public void PushObject(object value)
        //{
        //    stack[stackIndex].type = StackObject.ObjectType.Ref;
        //    stack[stackIndex].refValue = value;

        //    stackIndex++;
        //}

        //public void PushObjectBoxed(object value)
        //{
        //    stack[stackIndex].type = StackData.ObjectType.RefBoxed;
        //    stack[stackIndex].refValue = value;

        //    stackIndex++;
        //}

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //public void PushBool(bool value)
        //{
        //    stack[stackIndex].type = StackData.ObjectType.Int32;
        //    stack[stackIndex].value.Int32 = value == true ? 1 : 0;

        //    stackIndex++;
        //}

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //public void PushInt32(int value)
        //{
        //    stack[stackIndex].type = StackObject.ObjectType.Int32;
        //    stack[stackIndex].value.Int32 = value;

        //    stackIndex++;
        //}

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //public void PushUInt32(uint value)
        //{
        //    stack[stackIndex].type = StackObject.ObjectType.UInt32;
        //    stack[stackIndex].value.Int32 = (int)value;

        //    stackIndex++;
        //}

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //public void PushInt64(long value)
        //{
        //    stack[stackIndex].type = StackData.ObjectType.Int64;
        //    stack[stackIndex].value.Int64 = value;

        //    stackIndex++;
        //}

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //public void PushUInt64(ulong value)
        //{
        //    stack[stackIndex].type = StackData.ObjectType.UInt64;
        //    stack[stackIndex].value.Int64 = (long)value;

        //    stackIndex++;
        //}

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //public void PushSingle(float value)
        //{
        //    stack[stackIndex].type = StackObject.ObjectType.Single;
        //    stack[stackIndex].value.Single = (int)value;

        //    stackIndex++;
        //}

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //public void PushDouble(double value)
        //{
        //    stack[stackIndex].type = StackData.ObjectType.Double;
        //    stack[stackIndex].value.Double = (long)value;

        //    stackIndex++;
        //}

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //public StackObject Pop()
        //{
        //    StackObject value = stack[--stackIndex];
        //    stack[stackIndex] = StackObject.nullPtr;
        //    return value;
        //}

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //public StackData Pop(int n)
        //{
        //    stackIndex -= n;
        //    StackData value = stack[stackIndex];
        //    Array.Clear(stack, stackIndex, n);
        //    return value;
        //}

        //public object Peek()
        //{
        //    return stack[stackIndex + 1];
        //}

        //public void Dup()
        //{
        //    int i = stackIndex;
        //    stack[i] = stack[i - 1];
        //    stackIndex = i + 1;

        //}
        #endregion
    }
}
