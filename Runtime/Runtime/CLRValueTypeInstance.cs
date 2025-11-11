using dotnow.Interop;
using dotnow.Reflection;
using dotnow.Runtime.CIL;
using System;
using System.Runtime.Serialization;

namespace dotnow.Runtime
{
    internal readonly struct CLRValueTypeInstance : ICLRInstance
    {
        // Public
        public readonly CLRType Type;
        public readonly object InteropBase;
        public readonly ICLRProxy[] InteropImplementations;
        public readonly Memory<StackData> Fields;

        // Constructor
        private CLRValueTypeInstance(AppDomain domain, CILTypeInfo typeInfo, Memory<StackData> fields)
        {
            // Check for CLR
            if ((typeInfo.Flags & CILTypeFlags.Interpreted) == 0)
                throw new ArgumentException("Only supported for interpreted value types");

            this.Type = (CLRType)typeInfo.Type;
            this.InteropBase = null;
            this.InteropImplementations = null;
            this.Fields = fields;

            // Initialize proxies
            this.InteropBase = CreateInteropBase(domain, typeInfo, this);
            this.InteropImplementations = CreateInteropInterfaces(domain, typeInfo, this);
        }

        // Methods
        public bool Equals(ICLRInstance otherInstance)
        {
            return ReferenceEquals(this, otherInstance);
        }

        public Type GetInterpretedType()
        {
            return Type;
        }

        public object Unwrap()
        {
            return InteropBase;
        }

        public object UnwrapAsType(Type asType)
        {
            // Check for identical type
            if (Type == asType)
                return InteropBase;

            // Check for subclass
            if (RuntimeType.IsAssignable(asType, Type.BaseType) == true)
                return InteropBase;

            // Check for interface
            if (asType.IsInterface == true && InteropImplementations != null)
            {
                for (int i = 0; i < InteropImplementations.Length; i++)
                {
                    // It is possible for some implementations to be null in the case of CLR interfaces
                    if (InteropImplementations[i] != null)
                    {
                        // Get proxy type
                        Type proxyType = InteropImplementations[i].GetType();

                        // Check for assignable
                        if (RuntimeType.IsAssignable(asType, proxyType) == true)
                            return InteropImplementations[i];
                    }
                }
            }

            // No match found
            return null;
        }

        private static object CreateInteropBase(AppDomain domain, CILTypeInfo typeInfo, ICLRInstance instance)
        {
            // Try to create the proxy
            object proxy = __bindings.CreateProxyBindingInstance(domain, typeInfo.InteropBaseType, instance);

            // Check for proxy
            if (proxy == null)
            {
                // Create default object
                proxy = FormatterServices.GetUninitializedObject(typeInfo.InteropBaseType);
            }
            return proxy;
        }

        private static ICLRProxy[] CreateInteropInterfaces(AppDomain domain, CILTypeInfo typeInfo, ICLRInstance instance)
        {
            // Check for none
            if (typeInfo.InteropImplementationTypes.Length == 0)
                return null;

            // Create array
            ICLRProxy[] interfaceProxies = new ICLRProxy[typeInfo.InteropImplementationTypes.Length];

            // Initialize each proxy
            for (int i = 0; i < interfaceProxies.Length; i++)
            {
                // Check for CLR type
                if (typeInfo.InteropImplementationTypes[i].IsCLRType() == true)
                    continue;

                // Create the proxy
                interfaceProxies[i] = __bindings.CreateProxyBindingInstance(domain, typeInfo.InteropImplementationTypes[i], instance);
            }

            return interfaceProxies;
        }

        internal static CLRValueTypeInstance CreateInstance(AppDomain domain, CILTypeInfo typeInfo)
        {
            // Create dedicated memory - used for return to interop called where the stack would not survive
            StackData[] dedicatedFields = new StackData[typeInfo.InstanceSize];

            // Create the instance
            return new CLRValueTypeInstance(domain, typeInfo, new Memory<StackData>(dedicatedFields));
        }

        internal static CLRValueTypeInstance CreateInstance(AppDomain domain, CILTypeInfo typeInfo, StackData[] stack, int sp)
        {
            // Create the instance
            return new CLRValueTypeInstance(domain, typeInfo, new Memory<StackData>(stack, sp, typeInfo.InstanceSize));
        }
    }
}
