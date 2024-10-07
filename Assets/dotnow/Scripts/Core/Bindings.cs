using dotnow.Interop;
using dotnow.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace dotnow
{
    public sealed class MakeByRef<T>
    {
        // Empty class
    }

    internal class Bindings
    {
        // Internal
        internal AppDomain domain = null;
        internal Dictionary<Type, Type> clrProxyBindings = new Dictionary<Type, Type>();                              // System type, Proxy type (MonoBehaviour, MonoBehaviourProxy)
        internal Dictionary<MethodBase, MethodBase> clrMethodBindings = new Dictionary<MethodBase, MethodBase>();     // Method to reroute, New target method (AddComponent(Type), AddComponentOverride(AppDomain, object, object[]))
        internal Dictionary<Type, MethodBase> clrCreateInstanceBindings = new Dictionary<Type, MethodBase>();         // Constructor to reroute, New target method to handle construction of object
        internal Dictionary<ConstructorInfo, MethodBase> clrCreateInstanceConstructorBindings = new Dictionary<ConstructorInfo, MethodBase>();
        internal Dictionary<Type, MethodBase> clrCreateDelegateBindings = new Dictionary<Type, MethodBase>();                                       // Delegate system type, Create method
        internal Dictionary<MethodBase, AppDomain.MethodDirectCallDelegate> clrMethodDirectCallBindings = new Dictionary<MethodBase, AppDomain.MethodDirectCallDelegate>();
        internal Dictionary<FieldInfo, AppDomain.FieldDirectAccessDelegate> clrFieldDirectAccessReadBindings = new Dictionary<FieldInfo, AppDomain.FieldDirectAccessDelegate>();
        internal Dictionary<FieldInfo, AppDomain.FieldDirectAccessDelegate> clrFieldDirectAccessWriteBindings = new Dictionary<FieldInfo, AppDomain.FieldDirectAccessDelegate>();

        // Constructor
        public Bindings(AppDomain domain)
        {
            this.domain = domain;
            InitializeDomain();
        }

        // Methods
        #region InitializeDomain
        private void InitializeDomain()
        {
            Assembly thisAssembly = typeof(AppDomain).Assembly;
            AssemblyName thisAssemblyName = thisAssembly.GetName();
            string thisAssemblyFullName = thisAssemblyName.FullName;

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
                        if (references[i].FullName == thisAssemblyFullName)
                        {
                            found = true;
                            break;
                        }
                    }

                    // Check for found 
                    if (found == false)
                        continue;
                }
                else
                {
                    //continue;
                }

                foreach (Type type in asm.GetTypes())
                {
                    InitializeProxyBindings(type);
                    InitializeMethodBindings(type);

#if !API_NET35
                    InitializeMethodDirectCallBindings(type);
                    InitializeFieldDirectAccessBindings(type);
#endif
                    InitializeCreateInstanceBindings(type);
                    InitializeCreateDelegateBindings(type);
                }
            }
        }

        private void InitializeProxyBindings(Type type)
        {
            // Check for proxy types
            if (type.IsDefined(typeof(CLRProxyBindingAttribute), false) == true)
            {
                if (typeof(ICLRProxy).IsAssignableFrom(type) == true)
                {
                    CLRProxyBindingAttribute attribute = type.GetCustomAttribute<CLRProxyBindingAttribute>();

                    // Check for interface binding
                    if (attribute.BaseProxyType.IsInterface == true)
                    {
                        bool implementsInterface = false;

                        foreach(Type interfaceType in type.GetInterfaces())
                        {
                            if (interfaceType == attribute.BaseProxyType)
                                implementsInterface = true;
                        }

                        // Make sure type implements base interface
                        if (implementsInterface == false)
                        {
#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_IOS || UNITY_ANDROID || UNITY_WSA || UNITY_WEBGL || UNITY_SWITCH) && UNITY_DISABLE == false
                            UnityEngine.Debug.LogErrorFormat("Proxy binding '{0}' must implement interface '{1}'", type, attribute.BaseProxyType);
                            return;
#else
                            throw new CLRBindingException("Proxy binding '{0}' must implement interface '{1}'", type, attribute.BaseProxyType);
#endif
                        }
                    }
                    else
                    {
                        // Make sure type inehrits from base
                        if (type.BaseType != attribute.BaseProxyType)
                        {
#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_IOS || UNITY_ANDROID || UNITY_WSA || UNITY_WEBGL || UNITY_SWITCH) && UNITY_DISABLE == false
                            UnityEngine.Debug.LogErrorFormat("Proxy binding '{0}' must derive from base class '{1}'", type, attribute.BaseProxyType);
                            return;
#else
                        throw new CLRBindingException("Proxy binding '{0}' must derive from base class '{1}'", type, attribute.BaseProxyType);
#endif
                        }
                    }

                    // Check for already exists
                    if (clrProxyBindings.ContainsKey(attribute.BaseProxyType) == true)
                    {
#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_IOS || UNITY_ANDROID || UNITY_WSA || UNITY_WEBGL || UNITY_SWITCH) && UNITY_DISABLE == false
                        UnityEngine.Debug.LogErrorFormat("A proxy binding already exists for the target type '{0}'", attribute.BaseProxyType);
                        return;
#else
                        throw new CLRBindingException("A proxy binding already exists for the target type '{0}'", attribute.BaseProxyType);
#endif
                    }

                    // Add type
                    clrProxyBindings.Add(attribute.BaseProxyType, type);
                }
                else
                {
#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_IOS || UNITY_ANDROID || UNITY_WSA || UNITY_WEBGL || UNITY_SWITCH) && UNITY_DISABLE == false
                    UnityEngine.Debug.LogErrorFormat("Proxy binding {0} must implement the 'ICLRProxy' interface", type);
                    return;
#else
                    throw new CLRBindingException("Proxy binding {0} must implement the 'ICLRProxy' interface", type);
#endif
                }
            }
        }

        private void InitializeMethodBindings(Type type)
        {
            // Check for proxy methods
            foreach (MethodBase method in type.GetMethods())
            {
                if (method.IsDefined(typeof(CLRGenericMethodBindingAttribute), false) == true)
                {
                    // Check for static correct
                    if (method.IsStatic == false)
                    {
#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_IOS || UNITY_ANDROID || UNITY_WSA || UNITY_WEBGL || UNITY_SWITCH) && UNITY_DISABLE == false
                        UnityEngine.Debug.LogErrorFormat("Method binding {0} must be declared as static", method);
                        continue;
#else
                        throw new CLRBindingException("Method binding {0} must be declared as static", method);
#endif
                    }

                    // Check for correct parameters
                    ParameterInfo[] parameterTypes = method.GetParameters();

                    if (parameterTypes.Length < 5 ||
                        parameterTypes[0].ParameterType != typeof(AppDomain) ||
                        parameterTypes[1].ParameterType != typeof(MethodInfo) ||
                        parameterTypes[2].ParameterType != typeof(object) ||
                        parameterTypes[3].ParameterType != typeof(object[]) ||
                        parameterTypes[4].ParameterType != typeof(Type[]))
                    {
#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_IOS || UNITY_ANDROID || UNITY_WSA || UNITY_WEBGL || UNITY_SWITCH) && UNITY_DISABLE == false
                        UnityEngine.Debug.LogErrorFormat("Method binding {0} must have the following parameter signature ({1}, {2}, {3}, {4})", method,
                            typeof(AppDomain),
                            typeof(MethodInfo),
                            typeof(object),
                            typeof(object[]),
                            typeof(Type[]));

                        continue;
#else
                        throw new CLRBindingException("Method binding {0} must have the following parameter signature ({1}, {2}, {3}, {4})", method,
                            typeof(AppDomain),
                            typeof(MethodInfo),
                            typeof(object),
                            typeof(object[]),
                            typeof(Type[]));
#endif
                    }

                    // Get the attribute
                    CLRGenericMethodBindingAttribute attribute = method.GetCustomAttribute<CLRGenericMethodBindingAttribute>();

                    // Resolve method
                    MethodInfo rerouteMethod = attribute.DeclaringType.GetMethods()
                        .FirstOrDefault(m => 
                            m.Name == attribute.MethodName
                            && m.IsGenericMethod == true
                            && m.GetGenericArguments().Length == attribute.GenericArgumentCount
                            && m.GetParameters().Select(p => p.ParameterType).SequenceEqual(attribute.ParameterTypes));

                    // Check for missing method
                    if (rerouteMethod == null)
                    {
                        string parameterString = "";

                        for (int i = 0; i < attribute.ParameterTypes.Length; i++)
                        {
                            parameterString += attribute.ParameterTypes[i];

                            if (i < attribute.ParameterTypes.Length - 1)
                                parameterString += ", ";
                        }

#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_IOS || UNITY_ANDROID || UNITY_WSA || UNITY_WEBGL || UNITY_SWITCH) && UNITY_DISABLE == false
                        UnityEngine.Debug.LogErrorFormat("Method binding {0} targets a method that could not be resolved: {1}.{2}{3}", method, attribute.DeclaringType, attribute.MethodName, parameterString);
                        continue;
#else
                        throw new CLRBindingException("Method binding {0} targets a method that could not be resolved: {1}.{2}(3}", method, attribute.DeclaringType, attribute.MethodName, parameterString);
#endif
                    }
                    else
                        rerouteMethod = rerouteMethod.GetGenericMethodDefinition();

                    // Check return type
                    if (rerouteMethod is MethodInfo)
                    {
                        if ((rerouteMethod).ReturnType == typeof(void) && ((MethodInfo)method).ReturnType != typeof(void))
                        {
#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_IOS || UNITY_ANDROID || UNITY_WSA || UNITY_WEBGL || UNITY_SWITCH) && UNITY_DISABLE == false
                            UnityEngine.Debug.LogErrorFormat("Method binding {0} must have a return type of '{1}'", method, typeof(void));
                            continue;
#else
                            throw new CLRBindingException("Method binding {0} must have a return type of '{1}'", method, typeof(void));
#endif
                        }
                        else if ((rerouteMethod).ReturnType != typeof(void) && ((MethodInfo)method).ReturnType != typeof(object))
                        {
#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_IOS || UNITY_ANDROID || UNITY_WSA || UNITY_WEBGL || UNITY_SWITCH) && UNITY_DISABLE == false
                            UnityEngine.Debug.LogErrorFormat("Method binding {0} must have a return type of '{1}'", method, typeof(object));
                            continue;
#else
                            throw new CLRBindingException("Method binding {0} must have a return type of '{1}'", method, typeof(object));
#endif
                        }
                    }

                    // Check for already added
                    if (clrMethodBindings.ContainsKey(rerouteMethod) == true)
                    {
#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_IOS || UNITY_ANDROID || UNITY_WSA || UNITY_WEBGL || UNITY_SWITCH) && UNITY_DISABLE == false
                        UnityEngine.Debug.LogErrorFormat("An override method binding already exists for the target method '{0}'", rerouteMethod);
                        continue;
#else
                        throw new CLRBindingException("An override method binding already exists for the taret method '{0}'", rerouteMethod);
#endif
                    }

                    // Register the method
                    clrMethodBindings.Add(rerouteMethod, new CLRMethodBindingCallSite(domain, rerouteMethod, method));
                }

                else if (method.IsDefined(typeof(CLRMethodBindingAttribute), false) == true)
                {
                    // Check for static correct
                    if (method.IsStatic == false)
                    {
#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_IOS || UNITY_ANDROID || UNITY_WSA || UNITY_WEBGL || UNITY_SWITCH) && UNITY_DISABLE == false
                        UnityEngine.Debug.LogErrorFormat("Method binding {0} must be declared as static", method);
                        continue;
#else
                        throw new CLRBindingException("Method binding {0} must be declared as static", method);
#endif
                    }

                    // Check for correct parameters
                    ParameterInfo[] parameterTypes = method.GetParameters();

                    if (parameterTypes.Length < 4 ||
                        parameterTypes[0].ParameterType != typeof(AppDomain) ||
                        parameterTypes[1].ParameterType != typeof(MethodInfo) ||
                        parameterTypes[2].ParameterType != typeof(object) ||
                        parameterTypes[3].ParameterType != typeof(object[]))
                    {
#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_IOS || UNITY_ANDROID || UNITY_WSA || UNITY_WEBGL || UNITY_SWITCH) && UNITY_DISABLE == false
                        UnityEngine.Debug.LogErrorFormat("Method binding {0} must have the following parameter signature ({1}, {2}, {3}, {4})", method,
                            typeof(AppDomain),
                            typeof(MethodInfo),
                            typeof(object),
                            typeof(object[]));

                        continue;
#else
                        throw new CLRBindingException("Method binding {0} must have the following parameter signature ({1}, {2}, {3}, {4})", method,
                            typeof(AppDomain),
                            typeof(MethodInfo),
                            typeof(object),
                            typeof(object[]));
#endif
                    }

                    // Get the attribute
                    CLRMethodBindingAttribute attribute = method.GetCustomAttribute<CLRMethodBindingAttribute>();

                    // Resolve method
                    MethodBase rerouteMethod = attribute.DeclaringType.GetMethod(attribute.MethodName, attribute.ParameterTypes);

                    // Check for missing method
                    if (rerouteMethod == null)
                    {
                        string parameterString = "";

                        for (int i = 0; i < attribute.ParameterTypes.Length; i++)
                        {
                            parameterString += attribute.ParameterTypes[i];

                            if (i < attribute.ParameterTypes.Length - 1)
                                parameterString += ", ";
                        }

#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_IOS || UNITY_ANDROID || UNITY_WSA || UNITY_WEBGL || UNITY_SWITCH) && UNITY_DISABLE == false
                        UnityEngine.Debug.LogErrorFormat("Method binding {0} targets a method that could not be resolved: {1}.{2}{3}", method, attribute.DeclaringType, attribute.MethodName, parameterString);
                        continue;
#else
                        throw new CLRBindingException("Method binding {0} targets a method that could not be resolved: {1}.{2}(3}", method, attribute.DeclaringType, attribute.MethodName, parameterString);
#endif
                    }                

                    // Check return type
                    if (rerouteMethod is MethodInfo)
                    {
                        if (((MethodInfo)rerouteMethod).ReturnType == typeof(void) && ((MethodInfo)method).ReturnType != typeof(void))
                        {
#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_IOS || UNITY_ANDROID || UNITY_WSA || UNITY_WEBGL || UNITY_SWITCH) && UNITY_DISABLE == false
                            UnityEngine.Debug.LogErrorFormat("Method binding {0} must have a return type of '{1}'", method, typeof(void));
                            continue;
#else
                            throw new CLRBindingException("Method binding {0} must have a return type of '{1}'", method, typeof(void));
#endif
                        }
                        else if (((MethodInfo)rerouteMethod).ReturnType != typeof(void) && ((MethodInfo)method).ReturnType != typeof(object))
                        {
#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_IOS || UNITY_ANDROID || UNITY_WSA || UNITY_WEBGL || UNITY_SWITCH) && UNITY_DISABLE == false
                            UnityEngine.Debug.LogErrorFormat("Method binding {0} must have a return type of '{1}'", method, typeof(object));
                            continue;
#else
                            throw new CLRBindingException("Method binding {0} must have a return type of '{1}'", method, typeof(object));
#endif
                        }
                    }

                    // Check for already added
                    if (clrMethodBindings.ContainsKey(rerouteMethod) == true)
                    {
#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_IOS || UNITY_ANDROID || UNITY_WSA || UNITY_WEBGL || UNITY_SWITCH) && UNITY_DISABLE == false
                        UnityEngine.Debug.LogErrorFormat("An override method binding already exists for the target method '{0}'", rerouteMethod);
                        continue;
#else
                        throw new CLRBindingException("An override method binding already exists for the taret method '{0}'", rerouteMethod);
#endif
                    }

                    // Register the method
                    clrMethodBindings.Add(rerouteMethod, new CLRMethodBindingCallSite(domain, rerouteMethod, method));
                }
            }
        }

#if !API_NET35
        private void InitializeMethodDirectCallBindings(Type type)
        {
            // Check for proxy methods
            foreach (MethodBase method in type.GetMethods())
            {
                if (method.IsDefined(typeof(CLRMethodDirectCallBindingAttribute), false) == true)
                {
                    // Check for static correct
                    if (method.IsStatic == false)
                    {
#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_IOS || UNITY_ANDROID || UNITY_WSA || UNITY_WEBGL || UNITY_SWITCH) && UNITY_DISABLE == false
                        UnityEngine.Debug.LogErrorFormat("Method direct call binding {0} must be declared as static", method);
                        continue;
#else
                        throw new CLRBindingException("Method direct call binding {0} must be declared as static", method);
#endif
                    }

                    // Check for correct parameters
                    ParameterInfo[] parameterTypes = method.GetParameters();

                    if (parameterTypes.Length < 2 ||
                        parameterTypes[0].ParameterType != typeof(StackData[]) ||
                        parameterTypes[1].ParameterType != typeof(int))
                    {
#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_IOS || UNITY_ANDROID || UNITY_WSA || UNITY_WEBGL || UNITY_SWITCH) && UNITY_DISABLE == false
                        UnityEngine.Debug.LogErrorFormat("Method direct call binding {0} must have the following parameter signature ({1}, {2})", method,
                            typeof(StackData[]),
                            typeof(int));

                        continue;
#else
                        throw new CLRBindingException("Method direct call binding {0} must have the following parameter signature ({1}, {2})", method,
                            typeof(StackData[]),
                            typeof(int));
#endif
                    }

                    // Get the attribute
                    CLRMethodDirectCallBindingAttribute attribute = method.GetCustomAttribute<CLRMethodDirectCallBindingAttribute>();

                    // Resolve method
                    MethodBase delegateMethod = attribute.DeclaringType.GetMethod(attribute.MethodName, attribute.ParameterTypes);

                    // Check for missing method
                    if (delegateMethod == null)
                    {
                        string parameterString = "";

                        for (int i = 0; i < attribute.ParameterTypes.Length; i++)
                        {
                            parameterString += attribute.ParameterTypes[i];

                            if (i < attribute.ParameterTypes.Length - 1)
                                parameterString += ", ";
                        }

#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_IOS || UNITY_ANDROID || UNITY_WSA || UNITY_WEBGL || UNITY_SWITCH) && UNITY_DISABLE == false
                        UnityEngine.Debug.LogErrorFormat("Method direct call binding {0} targets a method that could not be resolved: {1}.{2}{3}", method, attribute.DeclaringType, attribute.MethodName, parameterString);
                        continue;
#else
                        throw new CLRBindingException("Method direct call binding {0} targets a method that could not be resolved: {1}.{2}(3}", method, attribute.DeclaringType, attribute.MethodName, parameterString);
#endif
                    }

                    // Create delegate
                    AppDomain.MethodDirectCallDelegate handler = (AppDomain.MethodDirectCallDelegate)((MethodInfo)method).CreateDelegate(typeof(AppDomain.MethodDirectCallDelegate), null);

                    // Check for already added
                    if (clrMethodDirectCallBindings.ContainsKey(delegateMethod) == true)
                    {
#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_IOS || UNITY_ANDROID || UNITY_WSA || UNITY_WEBGL || UNITY_SWITCH) && UNITY_DISABLE == false
                        UnityEngine.Debug.LogErrorFormat("A direct call method binding already exists for the target method '{0}'", delegateMethod);
                        continue;
#else
                        throw new CLRBindingException("A direct call method binding already exists for the target method '{0}'", delegateMethod);
#endif
                    }

                    // Register the method
                    clrMethodDirectCallBindings.Add(delegateMethod, handler);
                }
            }
        }

        private void InitializeFieldDirectAccessBindings(Type type)
        {
            // Check for proxy methods
            foreach (MethodBase method in type.GetMethods())
            {
                if (method.IsDefined(typeof(CLRFieldDirectAccessBindingAttribute), false) == true)
                {
                    // Check for static correct
                    if (method.IsStatic == false)
                    {
#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_IOS || UNITY_ANDROID || UNITY_WSA || UNITY_WEBGL || UNITY_SWITCH) && UNITY_DISABLE == false
                        UnityEngine.Debug.LogErrorFormat("Field direct access binding {0} must be declared as static", method);
                        continue;
#else
                        throw new CLRBindingException("Field direct access binding {0} must be declared as static", method);
#endif
                    }

                    // Check for correct parameters
                    ParameterInfo[] parameterTypes = method.GetParameters();

                    if (parameterTypes.Length < 2 ||
                        parameterTypes[0].ParameterType != typeof(StackData[]) ||
                        parameterTypes[1].ParameterType != typeof(int))
                    {
#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_IOS || UNITY_ANDROID || UNITY_WSA || UNITY_WEBGL || UNITY_SWITCH) && UNITY_DISABLE == false
                        UnityEngine.Debug.LogErrorFormat("Field direct access binding {0} must have the following parameter signature ({1}, {2})", method,
                            typeof(StackData[]),
                            typeof(int));

                        continue;
#else
                        throw new CLRBindingException("Field direct access binding {0} must have the following parameter signature ({1}, {2})", method,
                            typeof(StackData[]),
                            typeof(int));
#endif
                    }

                    // Get the attribute
                    CLRFieldDirectAccessBindingAttribute attribute = method.GetCustomAttribute<CLRFieldDirectAccessBindingAttribute>();

                    // Resolve field
                    FieldInfo fieldAccessor = attribute.DeclaringType.GetField(attribute.FieldName);

                    // Check for missing method
                    if (fieldAccessor == null)
                    {
#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_IOS || UNITY_ANDROID || UNITY_WSA || UNITY_WEBGL || UNITY_SWITCH) && UNITY_DISABLE == false
                        UnityEngine.Debug.LogErrorFormat("Field direct access binding {0} targets a method that could not be resolved: {1}.{2}", method, attribute.DeclaringType, attribute.FieldName);
                        continue;
#else
                        throw new CLRBindingException("Field direct access binding {0} targets a method that could not be resolved: {1}.{2}", method, attribute.DeclaringType, attribute.FieldName);
#endif
                    }

                    // Check return type
                    if (method is MethodInfo)
                    {
                        if (((MethodInfo)method).ReturnType != typeof(void))
                        {
#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_IOS || UNITY_ANDROID || UNITY_WSA || UNITY_WEBGL || UNITY_SWITCH) && UNITY_DISABLE == false
                            UnityEngine.Debug.LogErrorFormat("Field direct access binding {0} must have a return type of '{1}'", method, typeof(void));
                            continue;
#else
                            throw new CLRBindingException("Field direct access binding {0} must have a return type of '{1}'", method, typeof(void));
#endif
                        }
                    }

                    // Create delegate
                    AppDomain.FieldDirectAccessDelegate handler = (AppDomain.FieldDirectAccessDelegate)((MethodInfo)method).CreateDelegate(typeof(AppDomain.FieldDirectAccessDelegate), null);

                    // Register the accessor
                    if (attribute.FieldAccessMode == CLRFieldAccessMode.Read)
                    {
                        // Check for already exists
                        if (clrFieldDirectAccessReadBindings.ContainsKey(fieldAccessor) == true)
                        {
#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_IOS || UNITY_ANDROID || UNITY_WSA || UNITY_WEBGL || UNITY_SWITCH) && UNITY_DISABLE == false
                            UnityEngine.Debug.LogErrorFormat("A direct access field binding (Read) already exists for the target field '{0}'", fieldAccessor);
                            continue;
#else
                            throw new CLRBindingException("A direct access field binding (Read) already exists for the target field '{0}'", fieldAccessor);
#endif
                        }

                        clrFieldDirectAccessReadBindings.Add(fieldAccessor, handler);
                    }
                    else if (attribute.FieldAccessMode == CLRFieldAccessMode.Write)
                    {
                        // Check for already exists
                        if (clrFieldDirectAccessWriteBindings.ContainsKey(fieldAccessor) == true)
                        {
#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_IOS || UNITY_ANDROID || UNITY_WSA || UNITY_WEBGL || UNITY_SWITCH) && UNITY_DISABLE == false
                            UnityEngine.Debug.LogErrorFormat("A direct access field binding (Write) already exists for the target field '{0}'", fieldAccessor);
                            continue;
#else
                            throw new CLRBindingException("A direct access field binding (Write) already exists for the target field '{0}'", fieldAccessor);
#endif
                        }

                        clrFieldDirectAccessWriteBindings.Add(fieldAccessor, handler);
                    }
                }
            }
        }
#endif

        private void InitializeCreateInstanceBindings(Type type)
        {
            // Check for proxy methods
            foreach (MethodBase method in type.GetMethods())
            {
                if (method.IsDefined(typeof(CLRCreateInstanceBindingAttribute), false) == true)
                {
                    // Check for static correct
                    if (method.IsStatic == false)
                    {
#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_IOS || UNITY_ANDROID || UNITY_WSA || UNITY_WEBGL || UNITY_SWITCH) && UNITY_DISABLE == false
                        UnityEngine.Debug.LogErrorFormat("Create instance binding {0} must be declared as static", method);
                        continue;
#else
                        throw new CLRBindingException("Create instance binding {0} must be declared as static", method);
#endif
                    }

                    // Check for correct parameters
                    ParameterInfo[] parameterTypes = method.GetParameters();

                    if (parameterTypes.Length == 0 ||
                        parameterTypes[0].ParameterType != typeof(AppDomain) ||
                        parameterTypes[1].ParameterType != typeof(Type) ||
                        parameterTypes[2].ParameterType != typeof(ConstructorInfo) ||
                        parameterTypes[3].ParameterType != typeof(object[]))
                    {
#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_IOS || UNITY_ANDROID || UNITY_WSA || UNITY_WEBGL || UNITY_SWITCH) && UNITY_DISABLE == false
                        UnityEngine.Debug.LogErrorFormat("Create instance binding {0} must have the following parameter signature ({1}, {2}, {3}, {4})", method,
                            typeof(AppDomain),
                            typeof(Type),
                            typeof(ConstructorInfo),
                            typeof(object[]));

                        continue;
#else
                        throw new CLRBindingException("Create instance binding {0} must have the following parameter signature ({1}, {2}, {3}, {4})", method,
                            typeof(AppDomain),
                            typeof(Type),
                            typeof(ConstructorInfo),
                            typeof(object[]));
#endif
                    }

                    // Get the attribute
                    CLRCreateInstanceBindingAttribute attribute = method.GetCustomAttribute<CLRCreateInstanceBindingAttribute>();

                    if (((MethodInfo)method).ReturnType != typeof(object))
                    {
#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_IOS || UNITY_ANDROID || UNITY_WSA || UNITY_WEBGL || UNITY_SWITCH) && UNITY_DISABLE == false
                        UnityEngine.Debug.LogErrorFormat("Create instance binding {0} must have a return type of '{1}'", method, typeof(object));
                        continue;
#else
                        throw new CLRBindingException("Create instance binding {0} must have a return type of '{1}'", method, typeof(object));
#endif
                    }

                    // Check for parameter types specified
                    if (attribute.ParameterTypes != null)
                    {
                        // Try to find constructor
                        ConstructorInfo rerouteCtor = attribute.DeclaringType.GetConstructor(attribute.ParameterTypes);

                        // Check for missing method
                        if (rerouteCtor == null)
                        {
                            string parameterString = "";

                            for (int i = 0; i < attribute.ParameterTypes.Length; i++)
                            {
                                parameterString += attribute.ParameterTypes[i];

                                if (i < attribute.ParameterTypes.Length - 1)
                                    parameterString += ", ";
                            }

#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_IOS || UNITY_ANDROID || UNITY_WSA || UNITY_WEBGL || UNITY_SWITCH) && UNITY_DISABLE == false
                            UnityEngine.Debug.LogErrorFormat("Create instance constructor binding {0} targets a constructor that could not be resolved: {1}.ctor{2}", method, attribute.DeclaringType, parameterString);
                            continue;
#else
                            throw new CLRBindingException("Create instance constructor binding {0} targets a constructor that could not be resolved: {1}.ctor{2}", method, attribute.DeclaringType, parameterString);
#endif
                        }

                        // Check for already exists
                        if (clrCreateInstanceConstructorBindings.ContainsKey(rerouteCtor) == true)
                        {
#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_IOS || UNITY_ANDROID || UNITY_WSA || UNITY_WEBGL || UNITY_SWITCH) && UNITY_DISABLE == false
                            UnityEngine.Debug.LogErrorFormat("An override create instance constructor binding already exists for the target constructor '{0}'", rerouteCtor);
                            continue;
#else
                            throw new CLRBindingException("An override create instance constructor binding already exists for the target constructor '{0}'", rerouteCtor);
#endif
                        }

                        // Register method
                        clrCreateInstanceConstructorBindings.Add(rerouteCtor, new CLRCreateInstanceBindingCallSite(domain, attribute.DeclaringType, rerouteCtor, method));
                    }
                    else
                    {
                        // Check for already exists
                        if (clrCreateInstanceBindings.ContainsKey(attribute.DeclaringType) == true)
                        {
#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_IOS || UNITY_ANDROID || UNITY_WSA || UNITY_WEBGL || UNITY_SWITCH) && UNITY_DISABLE == false
                            UnityEngine.Debug.LogErrorFormat("An override create instance binding already exists for the target type '{0}'", attribute.DeclaringType);
                            continue;
#else
                            throw new CLRBindingException("An override create instance binding already exists for the target type '{0}'", attribute.DeclaringType);
#endif
                        }

                        // Register the method
                        clrCreateInstanceBindings.Add(attribute.DeclaringType, new CLRCreateInstanceBindingCallSite(domain, attribute.DeclaringType, null, method));
                    }
                }
            }
        }


        private void InitializeCreateDelegateBindings(Type type)
        {
            // Check for proxy methods
            foreach (MethodBase method in type.GetMethods())
            {
                if (method.IsDefined(typeof(CLRCreateDelegateBindingAttribute), false) == true)
                {
                    // Check for static correct
                    if (method.IsStatic == false)
                    {
#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_IOS || UNITY_ANDROID || UNITY_WSA || UNITY_WEBGL || UNITY_SWITCH) && UNITY_DISABLE == false
                        UnityEngine.Debug.LogErrorFormat("Create delegate binding {0} must be declared as static", method);
                        continue;
#else
                        throw new CLRBindingException("Create delegate binding {0} must be declared as static", method);
#endif
                    }

                    // Check for correct parameters
                    ParameterInfo[] parameterTypes = method.GetParameters();

                    if (parameterTypes.Length != 4 ||
                        parameterTypes[0].ParameterType != typeof(AppDomain) ||
                        parameterTypes[1].ParameterType != typeof(Type) ||
                        parameterTypes[2].ParameterType != typeof(MethodBase) ||
                        parameterTypes[3].ParameterType != typeof(object))
                    {
#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_IOS || UNITY_ANDROID || UNITY_WSA || UNITY_WEBGL || UNITY_SWITCH) && UNITY_DISABLE == false
                        UnityEngine.Debug.LogErrorFormat("Create delegate binding {0} must have the following parameter signature ({1}, {2}, {3}, {4})", method,
                            typeof(AppDomain),
                            typeof(Type),
                            typeof(MethodBase),
                            typeof(object));

                        continue;
#else
                        throw new CLRBindingException("Create delegate binding {0} must have the following parameter signature ({1}, {2}, {3}, {4})", method,
                            typeof(AppDomain),
                            typeof(Type),
                            typeof(MethodBase),
                            typeof(object));
#endif
                    }

                    // Get the attribute
                    CLRCreateDelegateBindingAttribute attribute = method.GetCustomAttribute<CLRCreateDelegateBindingAttribute>();
                    
                    // Check attribute type is delegate
                    if(typeof(Delegate).IsAssignableFrom(attribute.DelegateType) == false)
                    {
#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_IOS || UNITY_ANDROID || UNITY_WSA || UNITY_WEBGL || UNITY_SWITCH) && UNITY_DISABLE == false
                        UnityEngine.Debug.LogErrorFormat("Create delegate binding {0} must specify a delegate type via attribute", method);
                        continue;
#else
                        throw new CLRBindingException("Create delegate binding {0} must specify a delegate type via attribute", method);
#endif
                    }

                    // Check return type
                    if (((MethodInfo)method).ReturnType != attribute.DelegateType)
                    {
#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_IOS || UNITY_ANDROID || UNITY_WSA || UNITY_WEBGL || UNITY_SWITCH) && UNITY_DISABLE == false
                        UnityEngine.Debug.LogErrorFormat("Create delegate binding {0} must have a return type of '{1}'", method, attribute.DelegateType);
                        continue;
#else
                        throw new CLRBindingException("Create delegate binding {0} must have a return type of '{1}'", method, attribute.DelegateType);
#endif
                    }

                    // Check for already exists
                    if (clrCreateDelegateBindings.ContainsKey(attribute.DelegateType) == true)
                    {
#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_IOS || UNITY_ANDROID || UNITY_WSA || UNITY_WEBGL || UNITY_SWITCH) && UNITY_DISABLE == false
                        UnityEngine.Debug.LogErrorFormat("An override create delegate binding already exists for the target delegate '{0}'", attribute.DelegateType);
                        continue;
#else
                        throw new CLRBindingException("An override create delegate binding already exists for the target delegate '{0}'", attribute.DelegateType);
#endif
                    }

                    // Register method
                    clrCreateDelegateBindings.Add(attribute.DelegateType, method);                    
                }
            }
        }
        #endregion

        internal static Type[] GetBindingParameters(Type[] parameterTypes)
        {
            // Check for any parameters
            if (parameterTypes != null && parameterTypes.Length > 0)
            {
                // Check for by ref
                for(int i = 0; i < parameterTypes.Length; i++)
                {
                    // Resolve by ref
                    if (parameterTypes[i].IsGenericType == true && parameterTypes[i].GetGenericTypeDefinition() == typeof(MakeByRef<>))
                    {
                        // Convert to by ref type
                        parameterTypes[i] = parameterTypes[i]
                            .GetGenericArguments()[0]
                            .MakeByRefType();
                    }
                }
            }
            return parameterTypes;
        }
    }
}
