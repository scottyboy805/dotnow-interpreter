using System;
using System.Collections.Generic;
using System.Reflection;

namespace dotnow.Interop
{
    public static class __bindings
    {
        // Private
        private static readonly Dictionary<Type, Type> proxyBindings = new();
        private static readonly Dictionary<MethodBase, DirectInstance> directInstanceBindings = new();
        private static readonly Dictionary<MethodBase, DirectCall> directCallBindings = new();
        private static readonly Dictionary<MethodBase, DirectCallGeneric> directCallGenericBindings = new();

        // Constructor
        static __bindings()
        {
            // Load bindings
            InitializeBindings();
        }

        // Methods
        public static bool HasProxyBinding(Type forType)
        {
            return proxyBindings.ContainsKey(forType);
        }

        public static bool HasDirectInstanceBinding(MethodBase method)
        {
            return directInstanceBindings.ContainsKey(method);
        }

        public static bool HasDirectCallBinding(MethodBase method)
        {
            return directCallBindings.ContainsKey(method);
        }

        public static bool HasDirectCallGenericBinding(MethodBase method)
        {
            return directCallGenericBindings.ContainsKey(method);
        }

        public static bool TryCreateProxyBindingInstance(AppDomain appDomain, Type forType, ICLRInstance forInstance, out object interopInstance)
        {
            // Check for binding available
            if (proxyBindings.TryGetValue(forType, out Type proxyType) == false)
            {
                interopInstance = null;
                return false;
            }

            // Try to create instance
            ICLRProxy proxyInstance = (ICLRProxy)Activator.CreateInstance(proxyType);

            // Try to initialize instance
            proxyInstance.Initialize(appDomain, forInstance.GetInterpretedType(), forInstance);

            // Set interop instance
            interopInstance = proxyInstance;
            return true;
        }

        public static bool TryCreateExistingProxyBindingInstance(AppDomain appDomain, Type forType, ICLRInstance forInstance, ICLRProxy existingProxy, out object interopInstance)
        {
            // Check for binding available
            if (proxyBindings.TryGetValue(forType, out Type proxyType) == false)
            {
                interopInstance = null;
                return false;
            }

            // Get existing type
            Type existingProxyType = existingProxy.GetType();

            // Check for existing
            if(proxyType == existingProxyType)
            {
                // Use the existing proxy
                interopInstance = existingProxy;
                return true;
            }

            // Try to create instance
            ICLRProxy proxyInstance = (ICLRProxy)Activator.CreateInstance(proxyType);

            // Try to initialize instance
            proxyInstance.Initialize(appDomain, forInstance.GetInterpretedType(), forInstance);

            // Set interop instance
            interopInstance = proxyInstance;
            return true;
        }

        public static bool TryGetDirectInstanceBinding(MethodBase method, out DirectInstance call)
        {
            return directInstanceBindings.TryGetValue(method, out call);
        }

        public static bool TryGetDirectCallBinding(MethodBase method, out DirectCall call)
        {
            return directCallBindings.TryGetValue(method, out call);
        }

        public static bool TryGetDirectCallGenericBinding(MethodBase method, out DirectCallGeneric call)
        {
            return directCallGenericBindings.TryGetValue(method, out call);
        }

        private static void InitializeBindings()
        {
            // Get the name of this assembly
            // We should only check assemblies that are this assembly or have a reference to this assembly.
            Assembly thisAssembly = typeof(__bindings).Assembly;
            string thisAssemblyName = thisAssembly.GetName().FullName;

            // Check all loaded
            foreach (Assembly asm in System.AppDomain.CurrentDomain.GetAssemblies())
            {
                // Check if assembly references this assembly (Proxy types can only be defined in an assembly which reference 'this' assembly)
                if (asm != thisAssembly)
                {
                    // Check references
                    AssemblyName[] references = asm.GetReferencedAssemblies();

                    bool found = false;

                    for (int i = 0; i < references.Length; i++)
                    {
                        if (references[i].FullName == thisAssemblyName)
                        {
                            found = true;
                            break;
                        }
                    }

                    // Check for found 
                    if (found == false)
                        continue;
                }

                // Get all types
                Type[] types = asm.GetTypes();


                // Proxy
                InitializeProxyBindings(types);

                // Direct instance
                InitializeDirectInstanceBindings(types);

                // Direct call
                InitializeDirectCallBindings(types);

                // Direct call generic
                InitializeDirectCallGenericBindings(types);
            }
        }

        private static void InitializeProxyBindings(IEnumerable<Type> types)
        {
            // Process all types
            foreach(Type type in types)
            {
                // Check for attribute
                if (type.IsDefined(typeof(CLRProxyBindingAttribute), false) == false)
                    continue;

                // Get the attribute
                CLRProxyBindingAttribute attribute = type.GetCustomAttribute<CLRProxyBindingAttribute>();

                // Check abstract
                if(type.IsAbstract == true)
                {
                    ReportBindingError("CLR proxy binding '{0}' cannot be marked as abstract", type);
                    continue;
                }

                // Check for ICLRProxy
                if(typeof(ICLRProxy).IsAssignableFrom(type) == false)
                {
                    ReportBindingError("CLR proxy binding '{0}' must implement '{1}'", type, typeof(ICLRProxy));
                    continue;
                }

                // Check for attribute
                if(attribute.ForType.IsInterface == true)
                {
                    bool implementsInterface = false;

                    foreach (Type interfaceType in type.GetInterfaces())
                    {
                        if (interfaceType == attribute.ForType)
                            implementsInterface = true;
                    }

                    // Make sure type implements base interface
                    if (implementsInterface == false)
                    {
                        ReportBindingError("CLR proxy binding '{0}' must implement '{1}'", type, attribute.ForType);
                        continue;
                    }
                }
                // Check for inherits
                else
                {
                    if(type.BaseType != attribute.ForType)
                    {
                        ReportBindingError("CLR proxy binding '{0}' must derive from '{1}'", type, attribute.ForType);
                        continue;
                    }
                }

                // Check for proxy already defined
                if(proxyBindings.ContainsKey(attribute.ForType) == true)
                {
                    ReportBindingError("Attempting to register multiple CLR proxy bindings for type '{0}'", attribute.ForType);
                    continue;
                }

                // Add the type
                proxyBindings.Add(attribute.ForType, type);
            }
        }
                
        private static void InitializeDirectInstanceBindings(IEnumerable<Type> types)
        {
            // Process all types
            foreach (Type type in types)
            {
                // Get all methods
                foreach (MethodInfo method in type.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
                {
                    // Check for attribute
                    CLRCreateInstanceBindingAttribute attribute = method.GetCustomAttribute<CLRCreateInstanceBindingAttribute>(false);

                    // Check for found
                    if (attribute == null)
                        continue;

                    // Check static
                    if (method.IsStatic == false)
                    {
                        ReportBindingError("CLR create instance binding '{0}' must be marked as 'static'", method);
                        continue;
                    }

                    // Check return
                    if (method.ReturnType != typeof(void))
                    {
                        ReportBindingError("CLR create instance binding '{0}' must have a return type of 'void'", method);
                        continue;
                    }

                    // Check parameters
                    ParameterInfo[] parameters = method.GetParameters();

                    if (parameters.Length != 2 || parameters[0].ParameterType != typeof(StackContext) || parameters[1].ParameterType != typeof(Type))
                    {
                        ReportBindingError("CLR create instance binding '{0}' must have the parameter types '({1}', {2})", method, typeof(StackContext).FullName, typeof(Type).FullName);
                        continue;
                    }

                    // Get the method token
                    MethodBase bindingMethod = ResolveMethod(attribute.ResolveInitializerBinding);

                    // Check for no token
                    if (bindingMethod == null)
                    {
                        ReportBindingError("CLR create instance binding '{0}' could not resolve the target initializer '{1}.ctor'", method, attribute.DeclaringType);
                        continue;
                    }

                    // Check for already added
                    if (directCallBindings.ContainsKey(bindingMethod) == true)
                    {
                        ReportBindingError("CLR create instance binding is already registered for the target method '{1}.ctor'", attribute.DeclaringType);
                        continue;
                    }

                    // Method binding is valid and can be registered
                    DirectInstance bindingDelegate = (DirectInstance)method.CreateDelegate(typeof(DirectInstance));

                    // Register binding
                    directInstanceBindings[bindingMethod] = bindingDelegate;
                }
            }
        }

        private static void InitializeDirectCallBindings(IEnumerable<Type> types)
        {
            // Process all types
            foreach (Type type in types)
            {
                // Get all methods
                foreach (MethodInfo method in type.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
                {
                    // Check for attribute
                    CLRMethodBindingAttribute attribute = method.GetCustomAttribute<CLRMethodBindingAttribute>(false);

                    // Check for found - Prevent finding derived attributes
                    if (attribute == null || attribute.GetType() != typeof(CLRMethodBindingAttribute))
                        continue;

                    // Check static
                    if (method.IsStatic == false)
                    {
                        ReportBindingError("CLR method binding '{0}' must be marked as 'static'", method);
                        continue;
                    }

                    // Check return
                    if (method.ReturnType != typeof(void))
                    {
                        ReportBindingError("CLR method binding '{0}' must have a return type of 'void'", method);
                        continue;
                    }

                    // Check parameters
                    ParameterInfo[] parameters = method.GetParameters();

                    if (parameters.Length != 1 || parameters[0].ParameterType != typeof(StackContext))
                    {
                        ReportBindingError("CLR method binding '{0}' must have a single parameter of type '{1}'", method, typeof(StackContext).FullName);
                        continue;
                    }

                    // Get the method token
                    MethodBase bindingMethod = ResolveMethod(attribute.ResolveMethodBinding);

                    // Check for no token
                    if(bindingMethod == null)
                    {
                        ReportBindingError("CLR method binding '{0}' could not resolve the target method '{1}.{2}'", method, attribute.DeclaringType, attribute.MethodName);
                        continue;
                    }

                    // Check for already added
                    if(directCallBindings.ContainsKey(bindingMethod) == true)
                    {
                        ReportBindingError("CLR method binding is already registered for the target method '{1}.{2}1'", attribute.DeclaringType, attribute.MethodName);
                        continue;
                    }

                    // Method binding is valid and can be registered
                    DirectCall bindingDelegate = (DirectCall)method.CreateDelegate(typeof(DirectCall));

                    // Register binding
                    directCallBindings[bindingMethod] = bindingDelegate;
                }
            }
        }

        private static void InitializeDirectCallGenericBindings(IEnumerable<Type> types)
        {
            // Process all types
            foreach (Type type in types)
            {
                // Get all methods
                foreach (MethodInfo method in type.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
                {
                    // Check for attribute
                    CLRGenericMethodBindingAttribute attribute = method.GetCustomAttribute<CLRGenericMethodBindingAttribute>(false);

                    // Check for found
                    if (attribute == null)
                        continue;

                    // Check static
                    if (method.IsStatic == false)
                    {
                        ReportBindingError("CLR method binding '{0}' must be marked as 'static'", method);
                        continue;
                    }

                    // Check return
                    if (method.ReturnType != typeof(void))
                    {
                        ReportBindingError("CLR method binding '{0}' must have a return type of 'void'", method);
                        continue;
                    }

                    // Check parameters
                    ParameterInfo[] parameters = method.GetParameters();

                    if (parameters.Length != 2 || parameters[0].ParameterType != typeof(StackContext) || parameters[1].ParameterType != typeof(Type[]))
                    {
                        ReportBindingError("CLR method binding '{0}' must have a parameter list of type '{1}, {2}'", method, typeof(StackContext).FullName, typeof(Type[]).FullName);
                        continue;
                    }

                    // Get the method token
                    MethodBase bindingMethod = ResolveMethod(attribute.ResolveMethodBinding);

                    // Check for no token
                    if (bindingMethod == null)
                    {
                        ReportBindingError("CLR method binding '{0}' could not resolve the target method '{1}.{2}'", method, attribute.DeclaringType, attribute.MethodName);
                        continue;
                    }

                    // Check for already added
                    if (directCallBindings.ContainsKey(bindingMethod) == true)
                    {
                        ReportBindingError("CLR method binding is already registered for the target method '{1}.{2}1'", attribute.DeclaringType, attribute.MethodName);
                        continue;
                    }

                    // Method binding is valid and can be registered
                    DirectCallGeneric bindingDelegate = (DirectCallGeneric)method.CreateDelegate(typeof(DirectCallGeneric));

                    // Register binding
                    directCallGenericBindings[bindingMethod] = bindingDelegate;
                }
            }
        }

        private static MethodBase ResolveMethod(Func<MethodBase> resolveMethod)
        {
            try
            {
                // Resolve the method
                return resolveMethod();
            }
            catch(Exception e)
            {
                ReportBindingError("Could not resolve binding: " + e);
                return null;
            }
        }

        private static void ReportBindingError(string format, params object[] args)
        {
#if DEBUG
            Debug.LineFormat(format, args);
#else
            throw new TargetException("CLR binding error: " + string.Format(format, args));
#endif
        }
    }
}
