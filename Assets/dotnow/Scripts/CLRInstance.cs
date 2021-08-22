using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using dotnow.Interop;
using dotnow.Reflection;
using dotnow.Runtime;

namespace dotnow
{
    public class CLRInstance
    {
        // Private
        private CLRType type;
        private object baseProxy;
        private ICLRProxy[] baseInterfaceProxies;
        private StackData[] fields;

        // Internal
        internal int fieldPtr;

        // Properties
        public CLRType Type
        {
            get { return type; }
        }

        private ICLRProxy InteropProxy
        {
            get { return baseProxy as ICLRProxy; }
        }

        // Constructor
        public CLRInstance(CLRType type)
        {
            this.type = type;
        }

        // Methods
        public void Allocate(AppDomain domain, ICLRProxy proxy = null)
        {
            Type currentBase = type.BaseType;

            // Move down the hierarchy until we have a valid system base type
            while (currentBase.IsCLRType() == true)
                currentBase = currentBase.BaseType;

            // Check for object
            if(currentBase == null || currentBase == typeof(object) || currentBase == typeof(ValueType) || currentBase == typeof(Enum))
            {
                // Use this instance for intercepting object methods such as ToString
                baseProxy = this;
            }
            else
            {
                if (proxy == null)
                {
                    // Initialize base object
                    baseProxy = CreateBaseProxyInstance(domain, currentBase);
                }
                else
                {
                    baseProxy = proxy;
                }
            }

            // Initialize interface bases
            baseInterfaceProxies = CreateInterfaceProxyInstances(domain, type.GetInterfaces());

            // Get instance fields
            List<CLRField> instanceFields = type.GetInstanceFields();

            // Allocate reference type
            if (type.IsValueType == false)
            {
                // Allocate instance fields
                fields = new StackData[instanceFields.Count];
            }
            // Allocate value type
            else
            {
                // Get engine 
                ExecutionEngine engine = domain.GetExecutionEngine();

                // Get current stack
                fields = engine.stack;
            }

            for (int i = 0; i < instanceFields.Count; i++)
            {
                Type fieldType = instanceFields[i].FieldType;

                StackData.AllocTypedSlow(ref fields[i + fieldPtr], fieldType, fieldType.GetDefaultValue(domain));
            }

            // Setup proxy
            if (InteropProxy != null)
                InteropProxy.InitializeProxy(domain, this);

            // Setup interface proxy
            if(baseInterfaceProxies != null)
            {
                for (int i = 0; i < baseInterfaceProxies.Length; i++)
                {
                    if (baseInterfaceProxies[i] != null)
                    {
                        baseInterfaceProxies[i].InitializeProxy(domain, this);
                    }
                }
            }
        }

        public void Construct(ConstructorInfo ctor, object[] args)
        {
            // Check for ctor
            if (ctor == null)
            {
                // Struct do not always need a constructor
                if (type.IsValueType == true)
                    return;

                throw new MissingMethodException("Cannot create instance because a suitable ctor initializer was not found");
            }

            // Run ctor
            ctor.Invoke(this, args);
        }

        public object Unwrap()
        {
            return baseProxy;
        }

        public object UnwrapAs(Type unwrapType)
        {
            // Check for identical type
            if (type == unwrapType)
                return baseProxy;

            if(unwrapType.IsInterface == true && baseInterfaceProxies != null)
            {
                for(int i = 0; i < baseInterfaceProxies.Length; i++)
                {
                    if (baseInterfaceProxies[i] != null)
                    {
                        Type proxyType = baseInterfaceProxies[i].GetType();

                        if (TypeExtensions.AreAssignable(unwrapType, proxyType) == true)
                            return baseInterfaceProxies[i];
                    }
                }
            }

            // Check for subclass
            if(TypeExtensions.AreAssignable(unwrapType, Type.BaseType) == true)
                return baseProxy;

            return null;
        }

        public object GetFieldValue(CLRField field)
        {
            int fieldOffset = field.GetFieldOffset();

            if (fieldOffset == -1)
                throw new TargetException("The specified instance does not declare the field: " + field);

            return fields[fieldOffset].UnboxAsTypeSlow(field.FieldType);
        }


        public void SetFieldValue(CLRField field, object value)
        {
            int fieldOffset = field.GetFieldOffset();

            if (fieldOffset == -1)
                throw new TargetException("The specified instance does not declare the field: " + field);

            // Set field value
            StackData.AllocTypedSlow(ref fields[fieldOffset + fieldPtr], field.FieldType, value);
        }

        public override string ToString()
        {
            return string.Format("{0}({1})", nameof(CLRInstance), type);
        }

        private object CreateBaseProxyInstance(AppDomain domain, Type baseType)
        {
            if (baseType.IsAbstract == false)
            {
                if (baseType.IsSealed == true || baseType.HasVirtualMembers() == false)
                {
                    // We can create an instance of the base type without requiring a proxy
                    return FormatterServices.GetUninitializedObject(type);
                }
            }

            // Create a proxy
            return domain.CreateCLRProxyBinding(baseType);
        }

        private ICLRProxy[] CreateInterfaceProxyInstances(AppDomain domain, Type[] interfaceTypes)
        {
            // Check for none
            if (interfaceTypes == null || interfaceTypes.Length == 0)
                return null;

            ICLRProxy[] proxies = new ICLRProxy[interfaceTypes.Length];

            for(int i = 0; i < proxies.Length; i++)
            {
                // Check for clr type
                if (interfaceTypes[i].IsCLRType() == true)
                    continue;

                // Create an interface proxy
                proxies[i] = domain.CreateCLRProxyBinding(interfaceTypes[i]);
            }

            return proxies;
        }

        internal static CLRInstance CreateAllocatedInstance(AppDomain domain, CLRType type)
        {
            // Create instance
            CLRInstance instance = new CLRInstance(type);
            instance.Allocate(domain);

            return instance;
        }

        internal static CLRInstance CreateAllocatedInstance(AppDomain domain, CLRType type, ConstructorInfo ctor, object[] args)
        {
            // Create instance
            CLRInstance instance = new CLRInstance(type);
            instance.Allocate(domain);
            instance.Construct(ctor, args);

            return instance;
        }

        internal static CLRInstance CreateAllocatedInstanceWithProxy(AppDomain domain, CLRType type, ICLRProxy proxy)
        {
            // Create instance
            CLRInstance instance = new CLRInstance(type);
            instance.Allocate(domain, proxy);

            return instance;
        }

        internal static CLRInstance CreateAllocatedInstanceWithProxy(AppDomain domain, CLRType type, CLRConstructor ctor, object[] args, ICLRProxy proxy)
        {
            // Create instance
            CLRInstance instance = new CLRInstance(type);
            instance.Allocate(domain, proxy);
            instance.Construct(ctor, args);

            return instance;
        }
    }
}
