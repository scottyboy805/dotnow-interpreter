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
        public unsafe void Throw<T>(byte* pc) where T : Exception, new()
        {
            // Create the exception
            Exception e = new T();

            // Throw the exception on this thread
            Throw(e, pc);
        }

        public unsafe void Throw(Exception e, byte* pc)
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

        public unsafe void PrepareReflectionMethodFrame(AppDomain appDomain, in CILMethodHandle methodHandle, object instance, object[] args, out StackData* spArg, out StackData* sp)
        {
            // Get stack base pointer
            StackData* managedPtr = stack.Ptr;
            byte* unmanagedPtr;

            // Allocate the method handle
            methodHandle.AllocateMethodStack(this, stack.Ptr, stack.MaxPtr, false, out _, out spArg, out sp, out unmanagedPtr);

            // Check for this
            if((methodHandle.Flags & CILMethodFlags.This) != 0)
            {
                // Get initial address
                StackData* thisPtr = managedPtr;

                // Initialize argument slot
                PrepareMethodVariable(appDomain, methodHandle.This, ref managedPtr, ref unmanagedPtr);

                // Get the type handle because we need to update the managed offset
                CILTypeHandle thisTypeHandle = methodHandle.This.VariableTypeToken.GetTypeHandle(appDomain);

                // Load instance
                StackData.Wrap(this, thisTypeHandle, instance, thisPtr);
            }

            // Check for arguments
            if ((methodHandle.Signature.Flags & CILMethodSignatureFlags.HasParameters) != 0)
            {
                // Get the signature
                CILMethodSignatureHandle methodSignature = methodHandle.Signature;

                // Process all arguments
                for (int i = 0; i < methodSignature.ArgCount; i++)
                {
                    // Get initial address
                    StackData* argPtr = managedPtr;

                    // Initialize argument slot
                    PrepareMethodVariable(appDomain, methodSignature.Parameters[i], ref managedPtr, ref unmanagedPtr);

                    // Get the type handle because we need to update the managed offset
                    CILTypeHandle parameterTypeHandle = methodSignature.Parameters[i].VariableTypeToken.GetTypeHandle(appDomain);

                    // Check for instance method
                    bool hasThis = (methodHandle.Flags & CILMethodFlags.This) != 0;

                    // Load the argument
                    StackData.Wrap(this, parameterTypeHandle, args[i], argPtr);
                }
            }

            // Check for locals
            if (methodHandle.Body.LocalCount > 0)
            {
                // Get the body
                CILMethodBodyHandle methodBody = methodHandle.Body;

                // Process all locals
                for (int i = 0; i < methodBody.LocalCount; i++)
                {
                    // Initialize local slot
                    PrepareMethodVariable(appDomain, methodBody.Locals[i], ref managedPtr, ref unmanagedPtr);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="signature"></param>
        /// <param name="body"></param>
        /// <param name="spArg">The stack address where arguments passed into this method begin. Arguments will be copied from this location into the new method frame</param>
        /// <param name="sp">The stack pointer where the evaluation stack for the new frame begins</param>
        public unsafe void PrepareMethodFrame(AppDomain appDomain, in CILMethodHandle methodHandle, StackData* spArgCaller, StackData* spCaller, out StackData* spReturn, out StackData* spArg, out StackData* sp)
        {
            // Get stack base pointer - Where this method frame will begin
            StackData* managedPtr = spCaller;
            byte* unmanagedPtr;

            // Check for return value
            bool hasReturn = (methodHandle.Signature.Flags & CILMethodSignatureFlags.HasReturn) != 0;

            // Allocate the method handle
            // Make sure we allocate a slot (and unmanaged memory in the case of unmanaged value type) for the return value
            methodHandle.AllocateMethodStack(this, managedPtr, stack.MaxPtr, hasReturn, out spReturn, out spArg, out sp, out unmanagedPtr);

            // Check for return
            if(hasReturn == true)
            {
                // Prepare method return slot
                PrepareMethodVariable(appDomain, methodHandle.Signature.Return, ref managedPtr, ref unmanagedPtr);
            }

            // Check for this
            if ((methodHandle.Flags & CILMethodFlags.This) != 0)
            {
                // Get initial address
                StackData* thisPtr = managedPtr;

                // Initialize argument slot
                PrepareMethodVariable(appDomain, methodHandle.This, ref managedPtr, ref unmanagedPtr);

                // Get the type handle because we need to update the managed offset
                CILTypeHandle thisTypeHandle = methodHandle.This.VariableTypeToken.GetTypeHandle(appDomain);

                // Perform stack copy
                // This will copy the argument into the new frame, and will use correct copy semantics for value type vs reference type
                //if((methodHandle.Flags & CILMethodFlags.Ctor) == 0)
                if(spArgCaller->Type != 0)
                    StackData.Copy(this, thisTypeHandle, spArgCaller, thisPtr);

                // Increment the caller
                spArgCaller++;
            }

            // Check for arguments
            if ((methodHandle.Signature.Flags & CILMethodSignatureFlags.HasParameters) != 0)
            {
                // Get the signature
                CILMethodSignatureHandle methodSignature = methodHandle.Signature;

                // Process all arguments
                for (int i = 0; i < methodSignature.ArgCount; i++)
                {
                    // Get initial address
                    StackData* argPtr = managedPtr;

                    // Initialize argument slot
                    PrepareMethodVariable(appDomain, methodSignature.Parameters[i], ref managedPtr, ref unmanagedPtr);

                    // Get the type handle for copy
                    CILTypeHandle argTypeHandle = methodSignature.Parameters[i].VariableTypeToken.GetTypeHandle(appDomain);

                    // Perform stack copy
                    // This will copy the argument into the new frame, and will use correct copy semantics for value type vs reference type
                    // The slot should be kept empty for out parameters specifically
                    //if ((methodSignature.Parameters[i].Flags & CILMethodVariableFlags.Out) == 0)
                        StackData.Copy(this, argTypeHandle, spArgCaller + i, argPtr);
                }
            }

            // Check for locals
            if(methodHandle.Body.LocalCount > 0)
            {
                // Get the method body
                CILMethodBodyHandle methodBody = methodHandle.Body;

                // Process all locals
                for(int i = 0; i < methodBody.LocalCount; i++)
                {
                    // Initialize local slot
                    PrepareMethodVariable(appDomain, methodBody.Locals[i], ref managedPtr, ref unmanagedPtr);
                }
            }
        }

        private unsafe void PrepareMethodVariable(AppDomain appDomain, in CILMethodVariableHandle variableHandle, ref StackData* managedPtr, ref byte* unmanagedPtr)
        {
            // Get the type handle because we need to update the managed offset
            CILTypeHandle parameterTypeHandle = variableHandle.VariableTypeToken.GetTypeHandle(appDomain);

            // Check for unmanaged user struct - primitives should not be handled here because they can be loaded on the stack natively
            if ((variableHandle.Flags & CILMethodVariableFlags.UnmanagedValueType) != 0 && (variableHandle.Flags & CILMethodVariableFlags.PrimitiveType) == 0)
            {
                // Initialize with address
                managedPtr->Ptr = (IntPtr)unmanagedPtr;
                managedPtr->Type = StackTypeCode.UnmanagedValueType;
                managedPtr->Register = parameterTypeHandle.InstanceSize;

                // Increment the unmanaged ptr
                unmanagedPtr += parameterTypeHandle.InstanceSize;
            }
            // Check for managed
            else if ((variableHandle.Flags & CILMethodVariableFlags.ManagedValueType) != 0)
            {
                // Initialize with stack object address
                managedPtr->Register = managedStack.Count;
                managedPtr->Type = StackTypeCode.ManagedStackValueTypeReference;

                // Create uninitialized instance
                object defaultInstance = CLRTypeInstance.CreateInstance(appDomain, parameterTypeHandle);

                // Add empty slot
                managedStack.Add(defaultInstance);
            }
            // Check for reference type
            else if ((variableHandle.Flags & CILMethodVariableFlags.ReferenceType) != 0)
            {
                // Initialize with stack object address
                managedPtr->Register = managedStack.Count;
                managedPtr->Type = StackTypeCode.ManagedStackClassReference;

                // Add empty slot
                managedStack.Add(null);
            }
            // Get default
            else
            {
                // Initialize to default value
                StackData.Default(this, parameterTypeHandle, managedPtr);
            }

            // Increment ptr
            managedPtr++;
        }

        public string GetCallStack()
        {
            // Check for no call
            if (callStack.Count == 0)
                return "Null";

            StringBuilder builder = new StringBuilder();

            // Process all methods
            foreach(CallContext call in callStack.Reverse())
            {
                // Get the method
                CILMethodHandle methodHandle = call.Method;

                // Check for interop
                if((methodHandle.Flags & CILMethodFlags.Interop) != 0)
                {
                    // Append method
                    builder.AppendLine(call.Method.MetaMethod.ToString());
                }
                else
                {
                    // Append method name
                    builder.Append(call.Method.MetaMethod.ToString());

                    MetadataDebugInformation debugInfo = null;

                    // Check for clr method
                    if(call.Method.MetaMethod is CLRMethodInfo clrMethod)
                    {
                        // Get debug info
                        debugInfo = clrMethod.DebugInformation;
                    }
                    // Check for clr constructor
                    else if(call.Method.MetaMethod is CLRConstructorInfo clrConstructor)
                    {
                        // Get debug info
                        debugInfo = clrConstructor.DebugInformation;
                    }

                    // Check for debug info available
                    if(debugInfo != null)
                    {

                    }

                    // Add line
                    builder.AppendLine();
                }
            }

            return builder.ToString();
        }
    }
}
