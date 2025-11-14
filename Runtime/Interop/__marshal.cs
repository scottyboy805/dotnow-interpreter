using dotnow.Runtime;
using dotnow.Runtime.CIL;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
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
        public static void GetFieldInterop(AppDomain appDomain, in CILFieldInfo field, ref StackData instance, ref StackData value)
        {
            // Check for read access
            if ((field.Flags & CILFieldFlags.DirectReadDelegate) != 0)
            {
                // Create spans for view of stack instance and return value
                Span<StackData> stackArgs = MemoryMarshal.CreateSpan(ref instance, 1);
                Span<StackData> stackReturn = MemoryMarshal.CreateSpan(ref value, 1);

                // Create the stack context
                StackContext directAccessContext = new StackContext(appDomain, stackArgs, stackReturn);

                // Call the delegate
                Debug.LineFormat("[Marshal: Direct Read Access] Interop method binding: '{0}'", field.InteropReadAccess.Method);
                ((DirectAccess)field.InteropReadAccess)(directAccessContext);
            }
            // Fall back to reflection to set the field - slow and with allocations
            else
            {
                // Wrap the instance
                object unwrappedInstance = null;

                // Unwrap the instance
                StackData.Unwrap(field.DeclaringType, instance, ref unwrappedInstance);

                // Get the value
                object unwrappedValue = field.Field.GetValue(unwrappedInstance);

                // Wrap the value - Important to clear the value here so we don't end up setting a by ref value indirect when the intention is to just get the field value as is.
                value = default;
                StackData.Wrap(field.FieldType, unwrappedValue, ref value);
            }
        }

        public static void SetFieldInterop(AppDomain appDomain, in CILFieldInfo field, ref StackData instance, ref StackData value)
        {
            // Check for write access
            if ((field.Flags & CILFieldFlags.DirectWriteDelegate) != 0)
            {
                // Create spans for view of stack instance and value
                Span<StackData> stackArgs = (field.Flags & CILFieldFlags.This) != 0
                    ? MemoryMarshal.CreateSpan(ref instance, 2) // This is a bit unsafe because it relies on the fact that instance and value are in sequential memory, but it should always be the case
                    : MemoryMarshal.CreateSpan(ref value, 1);
                Span<StackData> stackReturn = default;

                // Create the stack context
                StackContext directAccessContext = new StackContext(appDomain, stackArgs, stackReturn);

                // Call the delegate
                Debug.LineFormat("[Marshal: Direct Write Access] Interop method binding: '{0}'", field.InteropWriteAccess.Method);
                ((DirectAccess)field.InteropWriteAccess)(directAccessContext);
            }
            // Fall back to reflection to set the field - slow and with allocations
            else
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
        }

        public static void InvokeConstructorInterop(ThreadContext threadContext, AppDomain appDomain, in CILTypeInfo type, in CILMethodInfo ctor, int spReturn, int spArg)
        {
            // Check for delegate
            if ((ctor.Flags & CILMethodFlags.DirectInstanceDelegate) != 0)
            {
                // Get arg count
                int argCount = ctor.ParameterTypes.Length;

                // Create spans for view of stack argument and return slots
                Span<StackData> stackArgs = new Span<StackData>(threadContext.stack, spArg, argCount);
                Span<StackData> stackReturn = new Span<StackData>(threadContext.stack, spReturn, 1);

                // Create the stack context
                StackContext directCallContext = new StackContext(appDomain, stackArgs, stackReturn);

                // Call the delegate
                Debug.LineFormat("[Marshal: Direct Instance] Interop method binding: '{0}'", ctor.InteropCall.Method);
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

                // Create instance
                StackData defaultInstance = default;

                // Check for multi array
                if ((type.Flags & CILTypeFlags.MultiArray) != 0)
                {
                    // Invoke ctor
                    instance = ((ConstructorInfo)ctor.Method).Invoke(paramList);
                }
                // Must be object
                else
                {
                    // Create instance of object
                    __gc.AllocateObject(appDomain, type, ref defaultInstance);

                    // Unwrap instance
                    StackData.Unwrap(type, defaultInstance, ref instance);

                    // Reflection invoke - do not pass instance because it should be created as part of the call
                    ((ConstructorInfo)ctor.Method).Invoke(instance, paramList);
                }

                // Wrap the instance for return
                StackData.Wrap(type, instance, ref threadContext.stack[spReturn]);
            }
        }

        public static void InvokeMethodInterop(ThreadContext threadContext, AppDomain appDomain, in CILTypeInfo constrainedType, in CILMethodInfo method, int spReturn, int spArg)
        {
            int spFirstArg = spArg;

            // Get arg count
            int argCount = (method.Flags & CILMethodFlags.This) != 0
                ? method.ParameterTypes.Length + 1
                : method.ParameterTypes.Length;

            // Check for direct call
            if ((method.Flags & CILMethodFlags.DirectCallDelegate) != 0)
            {
                // Create spans for view of stack argument and return slots
                Span<StackData> stackArgs = new Span<StackData>(threadContext.stack, spArg, argCount);
                Span<StackData> stackReturn = (method.Flags & CILMethodFlags.Return) != 0
                    ? new Span<StackData>(threadContext.stack, spReturn, 1)
                    : default;

                // Create the stack context
                StackContext directCallContext = new StackContext(appDomain, stackArgs, stackReturn);

                // Call the delegate
                Debug.LineFormat("[Marshal: Direct Call] Interop method binding: '{0}'", method.InteropCall.Method);
                ((DirectCall)method.InteropCall)(directCallContext);
            }
            // Check for generic direct call
            else if((method.Flags & CILMethodFlags.DirectCallGenericDelegate) != 0)
            {
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
                // Invoke via delegate
                Debug.LineFormat("[Marshal: Void Call] Interop method: '{0}'", method.InteropCall.Method);
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

                    // Something has gone wrong marshalling the instance
                    if (instance == null)
                        throw new Exception("Instance was null after marshalling but an instance of the following type is required: " + thisTypeInfo.Type);

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

                // For byRef parameters we need to write the interop result value back to the by ref stack location
                for(int i = 0; i < method.ParameterTypes.Length; i++)
                {
                    // Check for by ref
                    if ((method.ParameterFlags[i] & CILParameterFlags.ByRef) == 0)
                        continue;

                    // The argument should be passed by ref
                    StackData.Wrap(method.ParameterTypes[i], paramList[i], ref threadContext.stack[spArg + i]);
                }


                // Check for return
                if ((method.Flags & CILMethodFlags.Return) != 0)
                {
                    // Clear return slot so that we do not try to set an old address or something
                    threadContext.stack[spReturn] = default;

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

        /// <summary>
        /// This will take in a boxed value type object and return a new copy of that boxed object as if it was a normal struct assignment.
        /// </summary>
        /// <param name="boxedValueType"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static object CopyInteropBoxedValueTypeSlow(Type type, object boxedValueType)
        {
            // Check for null - cannot copy null
            if (boxedValueType == null)
                throw new ArgumentNullException(nameof(boxedValueType));

            object boxedCopy = FormatterServices.GetUninitializedObject(type);

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
