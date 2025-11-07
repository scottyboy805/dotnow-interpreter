using dotnow.Runtime;
using dotnow.Runtime.CIL;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Threading;

namespace dotnow.Interop
{   
    /// <summary>
    /// Handles cross domain operations from interpreted code to interop (host) code.
    /// </summary>
    internal static class __marshal
    {
        // Private
        // A lookup table for object array that can be reused for interop call args per thread
        private static readonly ThreadLocal<Dictionary<int, Type[]>> parameterTypeListCache = new(() => new ());
        // A lookup table for object array that can be reused for interop call args per thread
        private static readonly ThreadLocal<Dictionary<int, object[]>> parameterListCache = new (() => new ());

        // Methods
        public static void GetFieldInterop(AppDomain appDomain, in CILFieldInfo field, in StackData instance, ref StackData value)
        {
            // Wrap the instance
            object unwrappedInstance = null;

            // Unwrap the instance
            StackData.Unwrap(field.DeclaringType, instance, ref unwrappedInstance);

            // Get the value
            object unwrappedValue = field.Field.GetValue(unwrappedInstance);

            // Wrap the value
            StackData.Wrap(field.FieldType, unwrappedValue, ref value);
        }

        public static void SetFieldInterop(AppDomain appDomain, in CILFieldInfo field, in StackData instance, ref StackData value)
        {
            // Wrap the instance
            object unwrappedInstance = null;
            object unwrappedValue = null;

            // Unwrap the instance
            StackData.Unwrap(field.DeclaringType, instance, ref unwrappedInstance);

            // Unwrap the value
            StackData.Unwrap(field.FieldType, value, ref unwrappedValue);

            // Set the value
            field.Field.SetValue(unwrappedInstance, unwrappedValue);
        }

        public static void InvokeConstructorInterop(ThreadContext threadContext, AppDomain appDomain, in CILTypeInfo type, in CILMethodInfo ctor, int spArg)
        {
            // Check for delegate
            if((ctor.Flags & CILMethodFlags.DirectInstanceDelegate) != 0)
            {
                // Get arg count
                int argCount = ctor.ParameterTypes.Length;

                // Create spans for view of stack argument and return slots
                Span<StackData> stackArgs = new Span<StackData>(threadContext.stack, spArg, argCount);
                Span<StackData> stackReturn = default;

                // Create the stack context
                StackContext directCallContext = new StackContext(appDomain, stackArgs, stackReturn);

                // Check for debug
#if DEBUG
                Debug.LineFormat("[Marshal: Direct Instance] Interop method binding: '{0}'", ctor.InteropCall.Method);
#endif

                // Call the delegate
                ((DirectInstance)ctor.InteropCall)(directCallContext, type.Type);
            }
            // Check for reflection call
            else
            {
#if DEBUG
                Debug.LineFormat("[Marshal: Reflection Call] Interop constructor: '{0}'", ctor.Method);
#endif

                // Get parameter list for reflection
                object instance = null;
                object[] paramList = GetParameterList(ctor.ParameterTypes.Length);


                // Load instance
                StackData.Unwrap(type, threadContext.stack[spArg], ref instance);
                spArg++;

                // Copy parameters
                if ((ctor.Flags & CILMethodFlags.Parameters) != 0)
                {
                    for (int i = 0; i < paramList.Length; i++)
                    {
                        // Get parameter type
                        CILTypeInfo parameterTypeInfo = ctor.ParameterTypes[i];

                        // Load parameter
                        StackData.Unwrap(parameterTypeInfo, threadContext.stack[spArg + i], ref paramList[i]);
                    }
                }

                // Reflection invoke - do not pass instance because it should be created as part of the call
                ((ConstructorInfo)ctor.Method).Invoke(instance, paramList);
            }
        }

        public static void InvokeMethodInterop(ThreadContext threadContext, AppDomain appDomain, in CILTypeInfo constrainedType, in CILMethodInfo method, int spReturn, int spArg)
        {
            int spFirstArg = spArg;

             // Check for direct call
            if ((method.Flags & CILMethodFlags.DirectCallDelegate) != 0)
            {
                // Get arg count
                int argCount = (method.Flags & CILMethodFlags.This) != 0
                    ? method.ParameterTypes.Length + 1
                    : method.ParameterTypes.Length;

                // Create spans for view of stack argument and return slots
                Span<StackData> stackArgs = new Span<StackData>(threadContext.stack, spArg, argCount);
                Span<StackData> stackReturn = (method.Flags & CILMethodFlags.Return) != 0
                    ? new Span<StackData>(threadContext.stack, spReturn, 1)
                    : default;

                // Create the stack context
                StackContext directCallContext = new StackContext(appDomain, stackArgs, stackReturn);

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
                    ? method.ParameterTypes.Length + 1
                    : method.ParameterTypes.Length;

                // Create spans for view of stack argument and return slots
                Span<StackData> stackArgs = new Span<StackData>(threadContext.stack, spArg, argCount);
                Span<StackData> stackReturn = (method.Flags & CILMethodFlags.Return) != 0
                    ? new Span<StackData>(threadContext.stack, spReturn, 1)
                    : default;

                // Get generic arguments
                Type[] genericArguments = method.Method.GetGenericArguments();

                // Create the stack context
                StackContext directCallGenericContext = new StackContext(appDomain, stackArgs, stackReturn);

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
                Debug.LineFormat("[Marshal: Reflection Call] Interop method: '{0}'", method.Method);
#endif

                // Get parameter list for reflection
                object instance = null;
                object[] paramList = GetParameterList(method.ParameterTypes.Length);

                // Copy instance
                if ((method.Flags & CILMethodFlags.This) != 0)
                {
                    // Get instance type - check for optional constraint
                    CILTypeInfo thisTypeInfo = constrainedType == null
                        ? method.DeclaringType
                        : constrainedType;

                    // Load instance
                    StackData.Unwrap(thisTypeInfo, threadContext.stack[spArg], ref instance);

                    // Increment ptr to step over instance and start for args
                    spArg++;
                }

                // Copy parameters
                if ((method.Flags & CILMethodFlags.Parameters) != 0)
                {
                    for (int i = 0; i < paramList.Length; i++)
                    {
                        // Get parameter type
                        CILTypeInfo parameterTypeInfo = method.ParameterTypes[i];

                        // Load parameter
                        StackData.Unwrap(parameterTypeInfo, threadContext.stack[spArg + i], ref paramList[i]);
                    }
                }

                // Reflection invoke
                object result = method.Method.Invoke(instance, paramList);

                // Marshal by ref instance
                // Copy the modified by ref argument back to the associated stack slot
                if ((method.Flags & CILMethodFlags.This) != 0 && threadContext.stack[spFirstArg].Ref is ValueType)
                {
                    // Get instance type
                    CILTypeInfo thisTypeInfo = constrainedType == null
                        ? method.DeclaringType
                        : constrainedType;

                    // Wrap the instance
                    StackData.Wrap(thisTypeInfo, instance, ref threadContext.stack[spFirstArg]);
                }

                // TODO - marshal by ref arguments in a similar way

                // Check for return
                if ((method.Flags & CILMethodFlags.Return) != 0)
                {
                    // Get return type
                    CILTypeInfo returnTypeInfo = method.ReturnType;

                    // Push return value to stack
                    StackData.Wrap(returnTypeInfo, result, ref threadContext.stack[spReturn]);
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

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //public static void BoxUnmanagedInteropValueType(in CILTypeHandle typeHandle, void* src, out object boxedValueType)
        //{
        //    // Initialize new default instance
        //    boxedValueType = FormatterServices.GetUninitializedObject(typeHandle.MetaType);

        //    // Pin the boxed object
        //    GCHandle handle = GCHandle.Alloc(boxedValueType, GCHandleType.Pinned);
        //    try
        //    {
        //        // Get pointer to the pinned object's data
        //        void* dst = (void*)handle.AddrOfPinnedObject();

        //        // Get size of the struct
        //        int size = Marshal.SizeOf(typeHandle.MetaType);

        //        // Copy memory
        //        Buffer.MemoryCopy(src, dst, size, size);
        //    }
        //    finally
        //    {
        //        handle.Free(); // Always free pinned handle
        //    }
        //}

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //public static void UnboxUnmanagedInteropValueType(object boxedValueType, void* dst)
        //{
        //    // Maybe this is the best way to achieve it??
        //    Marshal.StructureToPtr(boxedValueType, (IntPtr)dst, false);
        //}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static object CopyInteropBoxedValueTypeSlow(object boxedValueType)
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
