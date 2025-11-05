using dotnow.Interop;
using dotnow.Reflection;
using dotnow.Runtime.CIL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace dotnow.Runtime
{
    internal sealed class ThreadContext
    {
        // Private
        private static readonly FieldInfo exceptionStackTraceField = typeof(Exception).GetField("_stackTraceString", BindingFlags.NonPublic | BindingFlags.Instance);

        // Public
        public const int DefaultStackSize = 4096;

        // Internal
        internal readonly StackData[] stack = default;        
        internal readonly Thread thread = null;
        internal int callDepth = 0;
        internal bool abort = false;

        // Properties
        public int StackSize
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return stack.Length; }
        }

        public bool IsMainThread
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return thread == Thread.CurrentThread; }
        }

        //public CILMethodHandle CurrentMethodHandle
        //{
        //    [MethodImpl(MethodImplOptions.AggressiveInlining)]
        //    get
        //    {
        //        // Get if available
        //        if (callStack.Count > 0)
        //            return callStack.Peek().Method;

        //        return default;
        //    }
        //}

        //public MethodBase CurrentMethod
        //{
        //    get
        //    {
        //        // Get meta method if available
        //        if (callStack.Count > 0)
        //            return callStack.Peek().Method.MetaMethod;

        //        // Not running any method
        //        return null;
        //    }
        //}

        //public Assembly CurrentAssembly
        //{
        //    get { return CurrentMethod?.DeclaringType.Assembly; }                
        //}

        // Constructor
        public ThreadContext(int stackSize = DefaultStackSize)
        {
            this.stack = new StackData[stackSize];
            this.thread = Thread.CurrentThread;
        }

        // Methods
        public void Throw<T>() where T : Exception, new()
        {
            // Create the exception
            Exception e = new T();

            // Throw the exception on this thread
            Throw(e);
        }

        public void Throw(Exception e)
        {
            // Inject stack trace
            exceptionStackTraceField.SetValue(e, "Hello World");

            // Raise - TODO - We should search for the handler and only throw if no handler was found
            throw e;
        }

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //public unsafe void PushCall(in CILMethodHandle method)
        //{
        //    callStack.Push(new CallContext(method, managedStack.Count));

        //    // Increment call depth
        //    callDepth++;
        //}

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //public unsafe void PopCall(out CILMethodHandle method)
        //{
        //    // Check for any
        //    if (callStack.Count == 0)
        //        throw new InvalidOperationException("No call loaded onto stack");

        //    // Pop from stack
        //    CallContext call = callStack.Pop();

        //    // Return to previous call
        //    method = call.Method;
            
        //    if (callDepth > 1)
        //    {
        //        // Pop managed stack - allow references to be reclaimed by gc
        //        while (managedStack.Count > call.ManagedStackOffset)
        //            managedStack.RemoveAt(managedStack.Count - 1);
        //    }

        //    // Decrement call depth
        //    callDepth--;
        //}

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //public unsafe void PushManagedObject(StackTypeCode type, object obj, StackData* sp)
        //{
        //    // Check type
        //    if (type != StackTypeCode.ManagedStackValueTypeReference && type != StackTypeCode.ManagedStackClassReference)
        //        throw new InvalidOperationException("Not a valid managed stack object");

        //    // Check for null
        //    if (type == StackTypeCode.ManagedStackValueTypeReference && obj == null)
        //        throw new InvalidOperationException("Object cannot be null when a value type is specified");

        //    // Check for null - we can save growing the managed stack unnecessarily
        //    if(type == StackTypeCode.ManagedStackClassReference && obj == null)
        //    {
        //        // Set stack object to a null reference
        //        sp->Type = type;
        //        sp->Register = 0;
        //        sp->Ptr = IntPtr.Zero;

        //        // Exit early
        //        return;
        //    }

        //    // Get stack index
        //    int index = managedStack.Count;

        //    // Push stack object
        //    managedStack.Add(obj);

        //    // Update pushed object
        //    sp->Type = type;
        //    sp->Register = index;
        //    sp->Ptr = IntPtr.Zero;
        //}

        //public void ClearManagedStack()
        //{
        //    managedStack.Clear();
        //    managedStack.Add(null);
        //}

        public void PushReflectionMethodFrame(AppDomain appDomain, in CILMethodInfo methodHandle, object instance, object[] args, out int spArg)
        {
            int sp = 0;

            // Copy instance to stack
            if ((methodHandle.Flags & CILMethodFlags.This) != 0)
                StackData.Wrap(methodHandle.DeclaringType, instance, ref stack[sp++]);

            // Copy arguments to the stack
            for(int i = 0; i < args.Length; i++)
                StackData.Wrap(methodHandle.ParameterTypes[i], args[i], ref stack[sp++]);

            // Get the sp arg
            spArg = sp;

            // Push the frame
            PushMethodFrame(appDomain, methodHandle, 0, sp);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="signature"></param>
        /// <param name="body"></param>
        /// <param name="spArg">The stack address where arguments passed into this method begin. Arguments will be copied from this location into the new method frame</param>
        /// <param name="sp">The stack pointer where the evaluation stack for the new frame begins</param>
        public void PushMethodFrame(AppDomain appDomain, in CILMethodInfo methodInfo, int spArgCaller, int spCaller)
        {
            // Get instance and argument size
            int requiredStackArgInst = methodInfo.ParameterTypes.Length;

            // Take instance into account also
            if ((methodInfo.Flags & CILMethodFlags.This) != 0)
                requiredStackArgInst++;

            // Calculate slots required
            int requiredStack = requiredStackArgInst
                + methodInfo.LocalCount
                + methodInfo.MaxStack;

            

            // Check for overflow
            if (spArgCaller + requiredStack >= stack.Length)
                throw new StackOverflowException();

            // Start where the previous stack pointer is currently at
            int spArg = spCaller + requiredStack;

            // Copy all values to the new frame
            for(int i = 0; i < requiredStackArgInst; i++)
            {
                // Check for value type but not a primitive
                if ((methodInfo.ParameterTypes[i].Flags & CILTypeFlags.ValueType) != 0 && (methodInfo.ParameterTypes[i].Flags & CILTypeFlags.PrimitiveType) == 0)
                {
                    // Perform value type copy
                    stack[spArg + i].Ref = __marshal.CopyInteropBoxedValueTypeSlow(stack[spArgCaller + i].Ref);
                }
                else
                {
                    // Simply copy is fine
                    stack[spArg + i] = stack[spArgCaller + i];
                }
            }
        }

        //public string GetCallStack()
        //{
        //    // Check for no call
        //    if (callStack.Count == 0)
        //        return "Null";

        //    StringBuilder builder = new StringBuilder();

        //    // Process all methods
        //    foreach(CallContext call in callStack.Reverse())
        //    {
        //        // Get the method
        //        CILMethodHandle methodHandle = call.Method;

        //        // Check for interop
        //        if((methodHandle.Flags & CILMethodFlags.Interop) != 0)
        //        {
        //            // Append method
        //            builder.AppendLine(call.Method.MetaMethod.ToString());
        //        }
        //        else
        //        {
        //            // Append method name
        //            builder.Append(call.Method.MetaMethod.ToString());

        //            MetadataDebugInformation debugInfo = null;

        //            // Check for clr method
        //            if(call.Method.MetaMethod is CLRMethodInfo clrMethod)
        //            {
        //                // Get debug info
        //                debugInfo = clrMethod.DebugInformation;
        //            }
        //            // Check for clr constructor
        //            else if(call.Method.MetaMethod is CLRConstructorInfo clrConstructor)
        //            {
        //                // Get debug info
        //                debugInfo = clrConstructor.DebugInformation;
        //            }

        //            // Check for debug info available
        //            if(debugInfo != null)
        //            {

        //            }

        //            // Add line
        //            builder.AppendLine();
        //        }
        //    }

        //    return builder.ToString();
        //}
    }
}
