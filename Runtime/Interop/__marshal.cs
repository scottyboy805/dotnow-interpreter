using dotnow.Runtime;
using dotnow.Runtime.CIL;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Threading;
using UnityEditor;

namespace dotnow.Interop
{   
    /// <summary>
    /// Handles cross domain operations from interpreted code to interop (host) code.
    /// </summary>
    internal static unsafe class __marshal
    {
        // Private
        // A lookup table for object array that can be reused for interop call args per thread
        private static readonly ThreadLocal<Dictionary<int, Type[]>> parameterTypeListCache = new(
            () => new Dictionary<int, Type[]>());
        // A lookup table for object array that can be reused for interop call args per thread
        private static readonly ThreadLocal<Dictionary<int, object[]>> parameterListCache = new (
            () => new Dictionary<int, object[]>());

        // Methods
        public static StackData* GetFieldInterop(ThreadContext threadContext, AssemblyLoadContext assemblyLoadContext, in CILFieldHandle field, StackData* instance)
        {
            throw new NotImplementedException();
        }

        public static void SetFieldInterop(ThreadContext threadContext, AssemblyLoadContext assemblyLoadContext, in CILFieldHandle field, StackData* instance, StackData* value)
        {
            throw new NotImplementedException();
        }

        public static void SetArrayValueInterop(ThreadContext threadContext, AssemblyLoadContext assemblyLoadContext, ICLRArrayProxy arrayProxy, StackData* spArg)
        {
            // Create stack context
            // 2 arguments = Index, value
            StackContext context = new StackContext(threadContext, assemblyLoadContext, null, spArg, 2);

            // Invoke the array proxy
            arrayProxy.SetValueDirect(context);
        }

        public static void GetArrayValueInterop(ThreadContext threadContext, AssemblyLoadContext assemblyLoadContext, ICLRArrayProxy arrayProxy, StackData* spReturn, StackData* spArg)
        {
            // Create stack context
            // 1 argument = Index
            StackContext context = new StackContext(threadContext, assemblyLoadContext, spReturn, spArg, 1);

            // Invoke the array proxy
            arrayProxy.GetValueDirect(context);
        }

        public static void InvokeConstructorInterop(ThreadContext threadContext, AssemblyLoadContext assemblyLoadContext, in CILTypeHandle type, in CILMethodHandle ctor, StackData* spReturn, StackData* spArg)
        {
            // Check for delegate
            if((ctor.Flags & CILMethodFlags.DirectInstanceDelegate) != 0)
            {
                // Get arg count
                int argCount = ctor.Signature.ArgCount;

                // Create the stack context
                StackContext directCallContext = new StackContext(threadContext, assemblyLoadContext, spReturn, spArg, argCount);

                // Check for debug
#if DEBUG
                Debug.LineFormat("[Marshal: Direct Instance] Interop method binding: '{0}'", ctor.InteropCall.Method);
#endif

                // Call the delegate
                ((DirectInstance)ctor.InteropCall)(directCallContext, type.MetaType);
            }
            // Check for reflection call
            else
            {
#if DEBUG
                Debug.LineFormat("[Marshal: Reflection Call] Interop constructor: '{0}'", ctor.MetaMethod);
#endif

                // Get parameter list for reflection
                object[] paramList = GetParameterList(ctor.Signature.ArgCount);

                // Copy parameters
                if ((ctor.Signature.Flags & CILMethodSignatureFlags.HasParameters) != 0)
                {
                    for (int i = 0; i < paramList.Length; i++)
                    {
                        // Get parameter type
                        CILTypeHandle parameterTypeHandle = ctor.Signature.Parameters[i].VariableTypeToken.GetTypeHandle(assemblyLoadContext.AppDomain);

                        // Load parameter
                        StackData.Unwrap(threadContext, parameterTypeHandle, spArg + i, ref paramList[i]);
                    }
                }

                // Reflection invoke - do not pass instance because it should be created as part of the call
                object result = ((ConstructorInfo)ctor.MetaMethod).Invoke(paramList);

                // Load return instance
                {
                    // Push return value to stack
                    StackData.Wrap(threadContext, type, result, spReturn);
                }
            }
        }

        public static void InvokeMethodInterop(ThreadContext threadContext, AssemblyLoadContext assemblyLoadContext, in CILTypeHandle constrained, in CILMethodHandle method, StackData* spReturn, StackData* spArg)
        {
            // Get return stack - Always 1 slot before the first arg if the method has a return value
            StackData* spFirstArg = spArg;

             // Check for direct call
            if ((method.Flags & CILMethodFlags.DirectCallDelegate) != 0)
            {
                // Get arg count
                int argCount = (method.Flags & CILMethodFlags.This) != 0
                    ? method.Signature.ArgCount + 1
                    : method.Signature.ArgCount;

                // Create the stack context
                StackContext directCallContext = new StackContext(threadContext, assemblyLoadContext, spReturn, spArg, argCount);

                // Check for debug
#if DEBUG
                Debug.LineFormat("[Marshal: Direct Call] Interop method binding: '{0}'", method.InteropCall.Method);
#endif

                // Call the delegate
                ((DirectCall)method.InteropCall)(directCallContext);
            }
            // Check for generic direct call
            else if((method.Flags & CILMethodFlags.DirectCallGenericDelegate) != 0)
            {
                // Get arg count
                int argCount = (method.Flags & CILMethodFlags.This) != 0
                    ? method.Signature.ArgCount + 1
                    : method.Signature.ArgCount;

                // Get generic arguments
                Type[] genericArguments = method.MetaMethod.GetGenericArguments();

                // Create the stack context
                StackContext directCallGenericContext = new StackContext(threadContext, assemblyLoadContext, spReturn, spArg, argCount);

                // Check for debug
#if DEBUG
                Debug.LineFormat("[Marshal: Direct Call Generic] Interop method binding: '{0}'", method.InteropCall.Method);
#endif

                // Call the delegate
                ((DirectCallGeneric)method.InteropCall)(directCallGenericContext, genericArguments);
            }
            // Check for void call
            else if ((method.Flags & CILMethodFlags.VoidCallDelegate) != 0)
            {
                // Check for debug
#if DEBUG
                Debug.LineFormat("[Marshal: Void Call] Interop method: '{0}'", method.InteropCall.Method);
#endif

                // Invoke via delegate
                ((VoidCall)method.InteropCall)();
            }
            // Check for reflection call
            else
            {
#if DEBUG
                Debug.LineFormat("[Marshal: Reflection Call] Interop method: '{0}'", method.MetaMethod);
#endif

                // Get parameter list for reflection
                object instance = null;
                object[] paramList = GetParameterList(method.Signature.ArgCount);

                // Copy instance
                if ((method.Flags & CILMethodFlags.This) != 0)
                {
                    // Get instance type - check for optional constraint
                    CILTypeHandle thisTypeHandle = constrained.MetaType == null
                        ? method.This.VariableTypeToken.GetTypeHandle(assemblyLoadContext.AppDomain)
                        : constrained;

                    // Load instance
                    StackData.Unwrap(threadContext, thisTypeHandle, spArg, ref instance);

                    // Increment ptr to step over instance and start for args
                    spArg++;
                }

                // Copy parameters
                if ((method.Signature.Flags & CILMethodSignatureFlags.HasParameters) != 0)
                {
                    for (int i = 0; i < paramList.Length; i++)
                    {
                        // Get parameter type
                        CILTypeHandle parameterTypeHandle = method.Signature.Parameters[i].VariableTypeToken.GetTypeHandle(assemblyLoadContext.AppDomain);

                        // Load parameter
                        StackData.Unwrap(threadContext, parameterTypeHandle, spArg + i, ref paramList[i]);
                    }
                }

                // Reflection invoke
                object result = method.MetaMethod.Invoke(instance, paramList);

                // Marshal by ref instance
                // Copy the modified by ref argument back to the associated stack slot
                if ((method.Flags & CILMethodFlags.This) != 0 && (spFirstArg->IsAddress == true || spFirstArg->Type == StackTypeCode.UnmanagedValueType))
                {
                    // Get instance type
                    CILTypeHandle thisTypeHandle = constrained.MetaType == null
                        ? method.This.VariableTypeToken.GetTypeHandle(assemblyLoadContext.AppDomain)
                        : constrained; method.This.VariableTypeToken.GetTypeHandle(assemblyLoadContext.AppDomain);

                    // Wrap the instance
                    StackData.Wrap(threadContext, thisTypeHandle, instance, spFirstArg);
                }

                // TODO - marshal by ref arguments in a similar way

                // Check for return
                if ((method.Signature.Flags & CILMethodSignatureFlags.HasReturn) != 0)
                {
                    // Get return type
                    CILTypeHandle returnTypeHandle = method.Signature.Return.VariableTypeToken.GetTypeHandle(assemblyLoadContext.AppDomain);

                    // Push return value to stack
                    StackData.Wrap(threadContext, returnTypeHandle, result, spReturn);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Type[] GetParameterTypeList(int parameterCount)
        {
            // Check for 0
            if (parameterCount <= 0)
                return Type.EmptyTypes;

            // Check for cached
            Type[] paramList;
            if (parameterTypeListCache.Value.TryGetValue(parameterCount, out paramList) == true)
                return paramList;

            // Create new instance
            paramList = new Type[parameterCount];

            // Add to cache
            parameterTypeListCache.Value[parameterCount] = paramList;
            return paramList;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static object[] GetParameterList(int parameterCount)
        {
            // Check for 0
            if (parameterCount <= 0)
                return null;

            // Check for cached
            object[] paramList;
            if (parameterListCache.Value.TryGetValue(parameterCount, out paramList) == true)
                return paramList;

            // Create new instance
            paramList = new object[parameterCount];

            // Add to cache
            parameterListCache.Value[parameterCount] = paramList;
            return paramList;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BoxUnmanagedInteropValueType(in CILTypeHandle typeHandle, void* src, out object boxedValueType)
        {
            // Initialize new default instance
            boxedValueType = FormatterServices.GetUninitializedObject(typeHandle.MetaType);

            // Pin the boxed object
            GCHandle handle = GCHandle.Alloc(boxedValueType, GCHandleType.Pinned);
            try
            {
                // Get pointer to the pinned object's data
                void* dst = (void*)handle.AddrOfPinnedObject();

                // Get size of the struct
                int size = Marshal.SizeOf(typeHandle.MetaType);

                // Copy memory
                Buffer.MemoryCopy(src, dst, size, size);
            }
            finally
            {
                handle.Free(); // Always free pinned handle
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void UnboxUnmanagedInteropValueType(object boxedValueType, void* dst)
        {
            // Maybe this is the best way to achieve it??
            Marshal.StructureToPtr(boxedValueType, (IntPtr)dst, false);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe object CopyInteropBoxedValueTypeSlow(object boxedValueType)
        {
            // Check for null - cannot copy null
            if (boxedValueType == null)
                throw new ArgumentNullException(nameof(boxedValueType));

            // Get type - note we need to use the system type rather that the interpreted type, and the runtime will copy CLRInstance correctly
            Type metaType = boxedValueType.GetType();

            object boxedCopy = FormatterServices.GetUninitializedObject(metaType);

            // Get typed reference
            TypedReference srcRef = __makeref(boxedValueType);
            TypedReference dstRef = __makeref(boxedCopy);

            // Perform GC safe copy of memory for value types that may contain managed fields
            // Bit slower than direct memory copy, but it is the only way to achieve a safe copy and not crash the GC
            __refvalue(dstRef, object) = __refvalue(srcRef, object);

            return boxedCopy;
        }
    }
}
