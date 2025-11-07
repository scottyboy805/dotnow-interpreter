using dotnow.Interop;
using dotnow.Reflection;
using dotnow.Reflection.Internal;
using System;
using System.Linq;
using System.Reflection;

namespace dotnow.Runtime.CIL
{
    [Flags]
    internal enum CILMethodFlags : uint
    {
        None = 0,
        Body = 1 << 0,
        This = 1 << 1,
        Ctor = 1 << 4,
        Virtual = 1 << 5,
        Abstract = 1 << 6,
        Parameters = 1 << 7,
        Return = 1 << 8,
        Interop = 1 << 9,
        Native = 1 << 10,
        Interpreted = 1 << 11,

        VoidCallDelegate = 1 << 12,
        DirectInstanceDelegate = 1 << 13,
        DirectCallDelegate = 1 << 14,
        DirectCallGenericDelegate = 1 << 15,

        InternalCall = 1 << 16,
    }

    [Flags]
    internal enum CILParameterFlags : uint
    {
        None = 0,
        ByRef = 1 << 1,
    }


    internal sealed class CILMethodInfo
    {
        // Public
        /// <summary>
        /// The associated metadata method.
        /// </summary>
        public readonly MethodBase Method;
        /// <summary>
        /// The associated type info for the declaring type.
        /// </summary>
        public readonly CILTypeInfo DeclaringType;
        /// <summary>
        /// The method flags which specify how the method should be used.
        /// </summary>
        public readonly CILMethodFlags Flags;
        /// <summary>
        /// The type information for the return type.
        /// </summary>
        public readonly CILTypeInfo ReturnType;
        /// <summary>
        /// The type information for the parameter types.
        /// </summary>
        public readonly CILTypeInfo[] ParameterTypes;
        public readonly CILParameterFlags[] ParameterFlags;
        /// <summary>
        /// Contains the default local values for this method.
        /// </summary>
        public readonly StackData[] Locals;
        /// <summary>
        /// The optional delegate if an interop binding is associated with this method.
        /// </summary>
        public readonly Delegate InteropCall;
        /// <summary>
        /// The CIL bytecode instructions for this method if it is a CLR method (interpreted).
        /// </summary>
        public readonly byte[] Instructions;
        public readonly int LocalCount;
        public readonly int MaxStack;

        // Constructor
        internal CILMethodInfo(AppDomain domain, MethodBase method)
        {
            MethodInfo methodInfo = method as MethodInfo;

            // Get return type and parameters
            Type returnType = methodInfo != null ? methodInfo.ReturnType : null;
            ParameterInfo[] parameters = method.GetParameters();

            this.Method = method;
            this.DeclaringType = method.DeclaringType.GetTypeInfo(domain);
            this.Flags = GetFlags(method, returnType, parameters, out this.InteropCall);
            this.ReturnType = returnType != null ? returnType.GetTypeInfo(domain) : typeof(void).GetTypeInfo(domain);
            this.ParameterTypes = parameters.Select(p => p.ParameterType.GetTypeInfo(domain)).ToArray();
            this.ParameterFlags = parameters.Select(p => p.ParameterType.IsByRef ? CILParameterFlags.ByRef : 0).ToArray();

            // Check for interpreted
            if((Flags & CILMethodFlags.Interpreted) != 0)
            {
                // Get the body
                MethodBody body = method.GetMethodBody();

                this.Locals = body.LocalVariables.Select(l => StackData.Default(l.LocalType.GetTypeInfo(domain))).ToArray();
                this.Instructions = body.GetILAsByteArray();
                this.LocalCount = body.LocalVariables.Count;
                this.MaxStack = body.MaxStackSize;
            }
        }

        // Methods
        public override string ToString()
        {
            return $"{Method} = {Flags}";
        }

        private static CILMethodFlags GetFlags(MethodBase method, Type returnType, ParameterInfo[] parameters, out Delegate interopCall)
        {
            MethodInfo methodInfo = method as MethodInfo;

            // Get interop call
            interopCall = null;

            // Init flags
            CILMethodFlags flags = 0;
            {
                // Get attributes
                MethodAttributes attributes = method.Attributes;
                bool isClr = method is CLRMethodInfo || method is CLRConstructorInfo;
                bool isCtor = method is ConstructorInfo;

                // Has body
                if ((attributes & MethodAttributes.Abstract) == 0) flags |= CILMethodFlags.Body;

                // Instance method
                if ((attributes & MethodAttributes.Static) == 0) flags |= CILMethodFlags.This;

                // Constructor
                if (isCtor) flags |= CILMethodFlags.Ctor;

                // Check virtual
                if ((attributes & MethodAttributes.Virtual) != 0) flags |= CILMethodFlags.Virtual;

                // Check abstract
                if ((attributes & MethodAttributes.Abstract) != 0) flags |= CILMethodFlags.Abstract;

                // Check interop
                if (isClr == false) flags |= CILMethodFlags.Interop;

                // Check native
                if ((method.Attributes & MethodAttributes.PinvokeImpl) != 0) flags |= CILMethodFlags.Native;

                // Check for interpreted
                if (isClr == true) flags |= CILMethodFlags.Interpreted;

                // Return
                if (returnType != null && returnType != typeof(void)) flags |= CILMethodFlags.Return;

                // Parameter
                if(parameters.Length > 0) flags |= CILMethodFlags.Parameters;

                // Check void call
                if (parameters.Length == 0 && (returnType == null || returnType == typeof(void)))
                {
                    // Check for interop
                    if ((flags & CILMethodFlags.Interop) != 0 && (flags & CILMethodFlags.This) == 0 && (flags & CILMethodFlags.Abstract) == 0 && methodInfo != null)
                    {
                        flags |= CILMethodFlags.VoidCallDelegate;
                        interopCall = methodInfo.CreateDelegate(typeof(VoidCall));
                    }
                }

                // Get binding method
                MethodBase bindingMethod = method.IsGenericMethod == true && method.IsGenericMethodDefinition == false
                    ? ((MethodInfo)method).GetGenericMethodDefinition()
                    : method;

                // Check for constructor
                if (isCtor == true)
                {
                    // Check for direct instance
                    DirectInstance directCall;
                    if (__bindings.TryGetDirectInstanceBinding(bindingMethod, out directCall) == true || method is CLRInternalConstructorInfo)
                    {
                        flags |= CILMethodFlags.DirectInstanceDelegate;
                        interopCall = directCall;

                        // Check for internal
                        if (method is CLRInternalConstructorInfo clrInternalCtor)
                        {
                            flags |= CILMethodFlags.InternalCall;
                            interopCall = clrInternalCtor.DirectInternalInstance;
                        }

                        // Important - clear interpreted flag since this is now considered an interop method via proxy and must be called by the marshal
                        flags &= ~CILMethodFlags.Interpreted;
                    }
                }
                else
                {
                    // Check direct call
                    DirectCall directCall;
                    if (__bindings.TryGetDirectCallBinding(bindingMethod, out directCall) == true || method is CLRInternalMethodInfo)
                    {
                        flags |= CILMethodFlags.DirectCallDelegate;
                        interopCall = directCall;

                        // Check for internal
                        if (method is CLRInternalMethodInfo clrInternalMethod)
                        {
                            flags |= CILMethodFlags.InternalCall;
                            interopCall = clrInternalMethod.DirectInternalCall;
                        }

                        // Important - clear interpreted flag since this is now considered an interop method via proxy and must be called by the marshal
                        flags &= ~CILMethodFlags.Interpreted;
                    }

                    // Check generic direct call
                    DirectCallGeneric directCallGeneric;
                    if (__bindings.TryGetDirectCallGenericBinding(bindingMethod, out directCallGeneric) == true || method is CLRInternalMethodInfo)
                    {
                        flags |= CILMethodFlags.DirectCallGenericDelegate;
                        interopCall = directCallGeneric;

                        // Check for internal
                        if (method is CLRInternalMethodInfo clrInternalMethod)
                        {
                            flags |= CILMethodFlags.InternalCall;
                            interopCall = clrInternalMethod.DirectInternalCall;
                        }

                        // Important - clear interpreted flag since this is now considered an interop method via proxy and must be called by the marshal
                        flags &= ~CILMethodFlags.Interpreted;
                    }
                } // End is ctor
            }
            return flags;
        }
    }
}
