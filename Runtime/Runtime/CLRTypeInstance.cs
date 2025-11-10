using dotnow.Interop;
using dotnow.Reflection;
using dotnow.Runtime.CIL;
using System;
using System.Runtime.Serialization;

namespace dotnow.Runtime
{
    internal sealed class CLRTypeInstance : ICLRInstance
    {
        // Public
        public readonly CLRType Type;
        public readonly object InteropBase;
        public readonly ICLRProxy[] InteropImplementations;
        public readonly StackData[] Fields;

        // Constructor
        private CLRTypeInstance(AppDomain domain, CILTypeInfo typeInfo, ICLRProxy existingProxy)
        {
            // Check for CLR
            if ((typeInfo.Flags & CILTypeFlags.Interpreted) == 0)
                throw new ArgumentException("Only supported for interpreted types");

            this.Type = (CLRType)typeInfo.Type;
            this.InteropBase = CreateInteropBase(domain, typeInfo, existingProxy);
            this.InteropImplementations = CreateInteropInterfaces(domain, typeInfo, existingProxy);
            this.InteropImplementations = new ICLRProxy[typeInfo.InteropImplementationTypes.Length];
            this.Fields = new StackData[typeInfo.InstanceSize];
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

        // Methods
        private object CreateInteropBase(AppDomain domain, CILTypeInfo typeInfo, ICLRProxy existingProxy)
        {
            object proxy = existingProxy;

            // Create proxy existing
            if (proxy == null)
            {
                // Try to create the proxy
                proxy = __bindings.CreateProxyBindingInstance(domain, typeInfo.InteropBaseType, this);
            }

            // Check for proxy
            if (proxy == null)
            {
                // Create default object
                proxy = FormatterServices.GetUninitializedObject(typeInfo.InteropBaseType);
            }
            return proxy;
        }

        private ICLRProxy[] CreateInteropInterfaces(AppDomain domain, CILTypeInfo typeInfo, ICLRProxy existingProxy)
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
                interfaceProxies[i] = __bindings.CreateProxyBindingInstance(domain, typeInfo.InteropImplementationTypes[i], this);
            }

            return interfaceProxies;
        }

        internal static CLRTypeInstance CreateInstance(AppDomain domain, CILTypeInfo typeInfo)
        {
            // Create the instance
            return new CLRTypeInstance(domain, typeInfo, null);
        }

        internal static CLRTypeInstance CreateInstanceFromProxy(AppDomain domain, CILTypeInfo typeInfo, ICLRProxy proxy)
        {
            // Create the instance
            CLRTypeInstance instance = new CLRTypeInstance(domain, typeInfo, proxy);

            // Initialize proxy
            proxy.Initialize(domain, typeInfo.Type, instance);
            return instance;
        }
    }
}
