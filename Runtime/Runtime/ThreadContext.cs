using dotnow.Interop;
using dotnow.Reflection;
using dotnow.Runtime.CIL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading;

namespace dotnow.Runtime
{
    internal enum CallInstance
    {
        NoInstance,
        NewObjectInstance,
        ExistingObjectInstance
    }

    internal sealed class ThreadContext
    {
        // Type
        internal struct CallFrame
        {
            // Public
            public CILMethodInfo MethodInfo;
        }

        // Private
        private static readonly FieldInfo exceptionStackTraceField = typeof(Exception).GetField("_stackTraceString", BindingFlags.NonPublic | BindingFlags.Instance);
        private static readonly FieldInfo remoteStackTraceString = typeof(Exception).GetField("_remoteStackTraceString", BindingFlags.Instance | BindingFlags.NonPublic);


        // Public
        public const int DefaultStackSize = 128;        // Realistically needs to be more, but ok for testing

        // Internal
        internal readonly StackData[] stack = default;
        internal readonly Stack<CallFrame> callStack = new();
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

        public CILMethodInfo CurrentMethodHandle
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                // Get if available
                if (callStack.Count > 0)
                    return callStack.Peek().MethodInfo;

                return default;
            }
        }

        public MethodBase CurrentMethod
        {
            get
            {
                // Get meta method if available
                if (callStack.Count > 0)
                    return callStack.Peek().MethodInfo.Method;

                // Not running any method
                return null;
            }
        }

        public Assembly CurrentAssembly
        {
            get { return CurrentMethod?.DeclaringType.Assembly; }
        }

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
            // Get the call stack
            string stackTrace = GetCallStack();

            // Replace the call stack
            remoteStackTraceString.SetValue(e, stackTrace);

            // Throw the exception
            ExceptionDispatchInfo.Capture(e).Throw();
        }

        public void PushReflectionMethodFrame(AppDomain appDomain, in CILMethodInfo methodHandle, object instance, object[] args, out int spArg)
        {
            int sp = 0;

            // Copy instance to stack
            if ((methodHandle.Flags & CILMethodFlags.This) != 0)
                StackData.Wrap(methodHandle.DeclaringType, instance, ref stack[sp++]);

            // Copy arguments to the stack
            if (args != null)
            {
                for (int i = 0; i < args.Length; i++)
                    StackData.Wrap(methodHandle.ParameterTypes[i], args[i], ref stack[sp++]);
            }

            // Push the frame
            PushMethodFrame(appDomain, methodHandle, CallInstance.ExistingObjectInstance, 0, 0, out spArg);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="signature"></param>
        /// <param name="body"></param>
        /// <param name="spArg">The stack address where arguments passed into this method begin. Arguments will be copied from this location into the new method frame</param>
        /// <param name="sp">The stack pointer where the evaluation stack for the new frame begins</param>
        public void PushMethodFrame(AppDomain appDomain, in CILMethodInfo methodInfo, CallInstance pushThis, int spArgCaller, int spCaller, out int spArg)
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
            spArg = spCaller;

            // Copy instance
            int srcOffset = 0;
            int dstOffset = 0;
            if((methodInfo.Flags & CILMethodFlags.This) != 0 && pushThis != CallInstance.NoInstance) //isNewObj == false)
            {
                // No instance is provided for constructors
                //if (isNewObj == false)
                if(pushThis == CallInstance.ExistingObjectInstance)
                {
                    // Copy the instance
                    StackData.CopyFrame(methodInfo.DeclaringType, stack[spArgCaller], ref stack[spArg]);
                    srcOffset++;
                }
                dstOffset++;
            }

            // Copy all values to the new frame
            for(int i = 0; i < methodInfo.ParameterTypes.Length; i++)
            {
                // Copy the value to the new frame
                StackData.CopyFrame(methodInfo.ParameterTypes[i], stack[spArgCaller + i + srcOffset], ref stack[spArg + i + dstOffset]);
            }

            // Push the frame
            callStack.Push(new CallFrame
            {
                MethodInfo = methodInfo,
            });
        }

        public void PopMethodFrame()
        {
            // Pop the frame
            callStack.Pop();
        }

        public string GetCallStack()
        {
            // Check for no call
            if (callStack.Count == 0)
                return string.Empty;

            StringBuilder builder = new StringBuilder();

            // Process all methods
            foreach (CallFrame call in callStack)
            {
                // Get the method
                CILMethodInfo methodInfo = call.MethodInfo;

                // Check for interop
                if ((methodInfo.Flags & CILMethodFlags.Interop) != 0)
                {
                    // Append method
                    builder.Append("   at ");
                    builder.AppendLine(call.MethodInfo.Method.ToString());
                }
                else
                {
                    // Append method name
                    builder.Append("   at ");
                    builder.Append(call.MethodInfo.Method.ToString());

                    MetadataDebugInformation debugInfo = null;

                    // Check for clr method
                    if (call.MethodInfo.Method is CLRMethodInfo clrMethod)
                    {
                        // Get debug info
                        debugInfo = clrMethod.DebugInformation;
                    }
                    // Check for clr constructor
                    else if (call.MethodInfo.Method is CLRConstructorInfo clrConstructor)
                    {
                        // Get debug info
                        debugInfo = clrConstructor.DebugInformation;
                    }

                    // Check for debug info available
                    if (debugInfo != null)
                    {

                    }

                    // Add line
                    builder.AppendLine();
                }
            }

            builder.AppendLine("   at --- [dotnow Call Stack] ---");

            return builder.ToString();
        }
    }
}
