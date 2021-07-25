using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading;
using Mono.Cecil;
using Mono.Collections.Generic;
using TrivialCLR.Debugging;
using TrivialCLR.Interop;
using TrivialCLR.Reflection;
using TrivialCLR.Runtime;
using TrivialCLR.Runtime.CIL;

namespace TrivialCLR
{
    public class AppDomain : IDisposable
    {
        // Delegate
        public delegate void MethodDirectCallDelegate(StackData[] stack, int offset);
        public delegate void FieldDirectAccessDelegate(StackData[] stack, int offset);

        // Internal
        internal Thread mainThread = null;

        // Private
        private Dictionary<Type, Type> clrProxyBindings = new Dictionary<Type, Type>();                              // System type, Proxy type (MonoBehaviour, MonoBehaviourProxy)
        private Dictionary<MethodBase, MethodBase> clrMethodBindings = new Dictionary<MethodBase, MethodBase>();     // Method to reroute, New taregt method (AddComponent(Type), AddComponentOverride(AppDomain, object, object[]))
        private Dictionary<Type, MethodBase> clrCreateInstanceBindings = new Dictionary<Type, MethodBase>();         // Constructor to reroute, New target method to handle construction of object
        private Dictionary<ConstructorInfo, MethodBase> clrCreateInstanceConstructorBindings = new Dictionary<ConstructorInfo, MethodBase>();
        private Dictionary<MethodBase, MethodDirectCallDelegate> clrMethodDirectCallBindings = new Dictionary<MethodBase, MethodDirectCallDelegate>();
        private Dictionary<FieldInfo, FieldDirectAccessDelegate> clrFieldDirectAccessReadBindings = new Dictionary<FieldInfo, FieldDirectAccessDelegate>();
        private Dictionary<FieldInfo, FieldDirectAccessDelegate> clrFieldDirectAccessWriteBindings = new Dictionary<FieldInfo, FieldDirectAccessDelegate>();
        

        private ExecutionEngine engine = null;
        private Dictionary<Thread, CLRThreadContext> threadEngines = new Dictionary<Thread, CLRThreadContext>();
        private HashSet<CLRModule> moduleCache = new HashSet<CLRModule>();
        private HashSet<CLRType> typeCache = new HashSet<CLRType>();
        private Dictionary<TypeReference, Type> typeReferenceCache = new Dictionary<TypeReference, Type>();
        private Dictionary<TypeDefinition, Type> typeLookupCache = new Dictionary<TypeDefinition, Type>();
        private Dictionary<FieldReference, FieldInfo> fieldReferenceCache = new Dictionary<FieldReference, FieldInfo>();
        private Dictionary<FieldDefinition, FieldInfo> fieldLookupCache = new Dictionary<FieldDefinition, FieldInfo>();
        private Dictionary<PropertyReference, PropertyInfo> propertyReferenceCache = new Dictionary<PropertyReference, PropertyInfo>();
        private Dictionary<PropertyDefinition, PropertyInfo> propertyLookupCache = new Dictionary<PropertyDefinition, PropertyInfo>();
        private Dictionary<MethodReference, MethodBase> methodReferenceCache = new Dictionary<MethodReference, MethodBase>();
        private Dictionary<MethodDefinition, MethodBase> methodLookupCache = new Dictionary<MethodDefinition, MethodBase>();
        private Dictionary<MethodBase, CILSignature> methodSignatureCache = new Dictionary<MethodBase, CILSignature>();
        private Dictionary<Type, VTable> methodVTableCache = new Dictionary<Type, VTable>();

        private const BindingFlags memberFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.ExactBinding;

        // Properties
        public bool IsDisposed
        {
            get { return mainThread == null; }
        }

        // Constructor
        public AppDomain()
        {
            this.mainThread = Thread.CurrentThread;
            this.engine = new ExecutionEngine(mainThread);

            InitializeDomain();
        }

        // Methods
        public void Dispose()
        {
            // Check for already disposed
            if (mainThread == null)
                return;

            mainThread = null;
            engine = null;

            // Destroy any running threads
            foreach (CLRThreadContext context in threadEngines.Values)
                DestroyThreadExecutionContext(context);

            // Release memory
            clrProxyBindings = null;
            clrMethodBindings = null;
            clrMethodDirectCallBindings = null;
            clrFieldDirectAccessReadBindings = null;
            clrFieldDirectAccessWriteBindings = null;
            clrCreateInstanceBindings = null;
            clrCreateInstanceConstructorBindings = null;

            threadEngines = null;
            moduleCache = null;
            typeCache = null;
            typeReferenceCache = null;
            typeLookupCache = null;
            fieldLookupCache = null;
            fieldLookupCache = null;
            propertyReferenceCache = null;
            propertyLookupCache = null;
            methodReferenceCache = null;
            methodLookupCache = null;
            methodSignatureCache = null;
            methodVTableCache = null;
        }

        internal void CreateThreadExecutionContext(CLRThreadContext context)
        {
            Thread threadContext = context.thread;

            // Check for context already created - This should never happen
            if (threadEngines.ContainsKey(threadContext) == true)
                throw new CLRRuntimeException("Corrupt execution state! Attempting to create a new execution context for an existing thread context");

            // Register context
            threadEngines.Add(threadContext, context);
        }

        internal void DestroyThreadExecutionContext(CLRThreadContext context)
        {
            Thread threadContext = context.thread;

            // try to remove
            if (threadEngines.ContainsKey(threadContext) == true)
                threadEngines.Remove(threadContext);
        }

        internal ExecutionEngine GetExecutionEngine()
        {
            // Get executing thread as key
            Thread executingThread = Thread.CurrentThread;

            // Check for main thread
            if(executingThread != mainThread)
            {
                CLRThreadContext context;
                if (threadEngines.TryGetValue(executingThread, out context) == true)
                    return context.engine;

                throw new CLRRuntimeException("Execution context is not valid or initialized for the current thread. Execution cannot continue!");
            }

            return engine;
        }        

        #region LoadModule
        public CLRModule LoadModuleStream(Stream input, bool keepOpen)
        {
            // Try to load the definition
            AssemblyDefinition definition = AssemblyDefinition.ReadAssembly(input);
            CLRModule module = null;

            // Check for success
            if (definition != null)
            {
                // Check for already loaded
                if (IsModuleLoaded(definition.MainModule.Name) == true)
                {
                    // Get the loaded module
                    return GetLoadedModule(definition.Name.Name);
                }
                else
                {
                    // Get load location
                    string location = "";

                    if (input is FileStream)
                        location = (input as FileStream).Name;

                    // Create the module
                    module = new CLRModule(this, definition, location);

                    // Register the module
                    moduleCache.Add(module);

                    // Register all module types and memebrs
                    DefineModule(module);
                }
            }

            // Release the stream
            if (keepOpen == false)
                input.Dispose();

            return module;
        }
        #endregion

        #region ObjectModel
        public MemberInfo ResolveToken(object token)
        {
            // Check for member info
            if (token is MemberInfo)
                return token as MemberInfo;

            // Check for reference
            if (token is MemberReference)
                return ResolveToken(token as MemberReference);

            throw new MissingMemberException("Failed to resolve token " + token);
        }

        public MemberInfo ResolveToken(MemberReference reference)
        {
            // Check for types
            if (reference is TypeReference)
                return ResolveType(reference as TypeReference);

            // Check for fields
            if (reference is FieldReference)
                return ResolveField(reference as FieldReference);

            // Check for properties
            if (reference is PropertyReference)
                return ResolveProperty(reference as PropertyReference);

            // Check for constructors and methods
            if (reference is MethodReference)
            {
                MethodReference methodRef = reference as MethodReference;

                if (methodRef.ReturnType == null)
                    return ResolveConstructor(methodRef);
                else
                    return ResolveMethod(methodRef);
            }

            // Other members are not supported
            throw new NotSupportedException("Only type, field, property and method members can be resolved");
        }

        public Type ResolveType(object typeToken)
        {
            // Check for field info
            if (typeToken is Type)
                return typeToken as Type;

            // Check for field reference
            if (typeToken is TypeReference)
                return ResolveType(typeToken as TypeReference);

            throw new MissingFieldException("Failed to resolve type reference " + typeToken);
        }

        public Type ResolveType(TypeReference reference, Type typeContext = null, Type[] accessContext = null)
        {
            Type resolvedType = null;

            // Check for cached type
            if (typeReferenceCache.TryGetValue(reference, out resolvedType) == true)
                return resolvedType;

            // Try to resolve the reference
            TypeDefinition definition = null;

            try
            {                
                // Catch any exceptions generated by trying to reslove unloaded modules
                definition = reference.Resolve();
            }
            catch { }

            GenericInstanceType genericInstance = (reference as GenericInstanceType);

            // Check for success
            if (definition != null && IsModuleLoaded(definition.Module.Name) == true && genericInstance == null)
            {
                // Try to resolve definition
                if (typeLookupCache.TryGetValue(definition, out resolvedType) == true)
                {
                    // Cache the reference and return the field
                    typeReferenceCache.Add(reference, resolvedType);

                    return CreateFinalType(reference, resolvedType);
                }

                // Failed to resolve
                throw new MissingFieldException(string.Format("Referenced type '{0}' could not be resolved. Ensure that the declaring assembly is loaded", reference.FullName));
            }
            else
            {
                /// ######## Need to parse the full name and resolve generic types separatley
                /// 

                string mainTypeFullName = reference.FullName;


                // IMPORTANT - Do not cache generics as this can result in incorrect type lookup
                // Check for generic argument type
                if (mainTypeFullName.Length >= 2 && mainTypeFullName[0] == '!' && char.IsNumber(mainTypeFullName[1]) == true)
                {
                    // Check for no type context
                    if (typeContext == null)
                        throw new TypeLoadException("Type context must be provided when resolving a generic argument type");

                    // Get the number char
                    char numberChar = mainTypeFullName[1];

                    // Get as numerical value
                    int genericIndex = (int)char.GetNumericValue(numberChar);

                    // These types have already been resolved
                    Type[] resolvedGenericTypes = typeContext.GetGenericArguments();

                    // Get the generic at the specified index
                    resolvedType = resolvedGenericTypes[genericIndex];

                    // Cache the result
                    //if (resolvedType != null)
                    //{
                    //    // Use indirect access for system types
                    //    //if (resolvedType.IsCLRType() == false)
                    //    //    resolvedType = new CLRIndirectType(resolvedType);

                    //    typeReferenceCache.Add(reference, resolvedType);
                    //}

                    return CreateFinalType(reference, resolvedType);
                }
                else if(mainTypeFullName.Length >= 3 && mainTypeFullName[0] == '!' && mainTypeFullName[1] == '!' && char.IsNumber(mainTypeFullName[2]) == true)
                {
                    // Check for no type context
                    if (typeContext == null)
                        throw new TypeLoadException("Type context must be provided when resolving a generic argument type");

                    // Get the number char
                    char numberChar = mainTypeFullName[2];

                    // Get as numerical value
                    int genericIndex = (int)char.GetNumericValue(numberChar);

                    // These types have already been resolved
                    Type[] resolvedGenericTypes = typeContext.GetGenericArguments();

                    // Get the generic at the specified index
                    resolvedType = resolvedGenericTypes[genericIndex];

                    // Cache the result
                    //if (resolvedType != null)
                    //{
                    //    // Use indirect access for system types
                    //    //if (resolvedType.IsCLRType() == false)
                    //    //    resolvedType = new CLRIndirectType(resolvedType);

                    //    typeReferenceCache.Add(reference, resolvedType);
                    //}

                    return CreateFinalType(reference, resolvedType);
                }


                // Check for generic type reference
                Type[] genericTypes = null;
                genericInstance = reference as GenericInstanceType;

                if (genericInstance != null)
                {
                    // Get all generic parameters
                    Collection<TypeReference> generics = genericInstance.GenericArguments;

                    // Create array
                    genericTypes = new Type[generics.Count];

                    // Resolve all generic types
                    for (int i = 0; i < genericTypes.Length; i++)
                        genericTypes[i] = ResolveType(generics[i]);

                    // Remove generic section from argument
                    int startIndex = mainTypeFullName.IndexOf('<');
                    int endIndex = mainTypeFullName.LastIndexOf('>');

                    mainTypeFullName = mainTypeFullName.Remove(startIndex, endIndex - startIndex + 1);
                }


                if(definition != null && IsModuleLoaded(definition.Module.Name) == true)
                {
                    if (typeLookupCache.TryGetValue(definition, out resolvedType) == true)
                    {
                        if (resolvedType != null)
                        {
                            // Make generic from resolved type
                            resolvedType = resolvedType.MakeGenericType(genericTypes);

                            // Use indirect access for system types
                            //if (resolvedType.IsCLRType() == false)
                            //    resolvedType = new CLRIndirectType(resolvedType);

                            // Add to cache
                            typeReferenceCache.Add(reference, resolvedType);
                        }
                    }
                    return CreateFinalType(reference, resolvedType);
                }


                // Build the assembly qualifed name
                string referenceType = mainTypeFullName + ", " + reference.Scope.Name;

                // Try to resolve the system type
                resolvedType = Type.GetType(referenceType, false);

                // Apply generic parameter types
                if (resolvedType != null && genericTypes != null)
                    resolvedType = resolvedType.MakeGenericType(genericTypes);

                // Cache the result
                if (resolvedType != null)
                {
                    // Use indirect access for system types
                    //if (resolvedType.IsCLRType() == false)
                    //    resolvedType = new CLRIndirectType(resolvedType);

                    typeReferenceCache.Add(reference, resolvedType);
                }
            }

            if (resolvedType == null)
                throw new TypeLoadException("The type '" + reference + "' could not be loaded!");

            return CreateFinalType(reference, resolvedType);
        }

        private Type CreateFinalType(TypeReference reference, Type inputType)
        {
            // Make array
            if (reference.IsArray == true && inputType.IsArray == false)
            {
                ArrayType array = reference as ArrayType;

                // Make an array type
                inputType = inputType.MakeArrayType(array.Rank);
            }
            return inputType;
        }

        public FieldInfo ResolveField(object fieldToken)
        {
            // Check for field info
            if (fieldToken is FieldInfo)
                return fieldToken as FieldInfo;

            // Check for field reference
            if (fieldToken is FieldReference)
                return ResolveField(fieldToken as FieldReference);

            throw new MissingFieldException("Failed to resolve field reference " + fieldToken);
        }

        public FieldInfo ResolveField(FieldReference reference)
        {
            FieldInfo resolvedField = null;

            // Check for cached field
            if (fieldReferenceCache.TryGetValue(reference, out resolvedField) == true)
                return resolvedField;

            // Check for generic context
            if (GenericContext.TypeContext != null)
                return GenericContext.TypeContext.GetField(reference.Name, memberFlags);

            // Try to resolve in loaded types
            FieldDefinition definition = null;

            try
            {
                // Catch any exceptions generated by trying to reslove unloaded modules
                definition = reference.Resolve();
            }
            catch { }


            if (definition != null && IsModuleLoaded(definition.DeclaringType.Module.Name) == true && (reference.DeclaringType is GenericInstanceType) == false &&(reference.FieldType is GenericParameter) == false)
            {
                // Try to resolve definition
                if (fieldLookupCache.TryGetValue(definition, out resolvedField) == true)
                {
                    // Cache the reference and return the field
                    fieldReferenceCache.Add(reference, resolvedField);
                    return resolvedField;
                }

                // Failed to resolve
                throw new MissingFieldException(string.Format("Referenced field '{0}::{1}' could not be resolved. Ensure that the declaring assembly is loaded", reference.DeclaringType.FullName, reference.Name));
            }

            // Resolve interop field
            Type resolvedDeclaringType = ResolveType(reference.DeclaringType);

            if (resolvedDeclaringType != null)
            {
                // Try to resolve the field
                resolvedField = resolvedDeclaringType.GetField(reference.Name, memberFlags);

                // Check for success
                if (resolvedField != null)
                {
                    // Cache the interop reference and return the field
                    fieldReferenceCache.Add(reference, resolvedField);
                    return resolvedField;
                }
            }

            throw new MissingFieldException("Failed to resolve field reference " + reference.FullName);
        }

        public PropertyInfo ResolveProperty(object propertyToken)
        {
            // Check for field info
            if (propertyToken is PropertyInfo)
                return propertyToken as PropertyInfo;

            // Check for field reference
            if (propertyToken is PropertyInfo)
                return ResolveProperty(propertyToken as PropertyInfo);

            throw new MissingFieldException("Failed to resolve property reference " + propertyToken);
        }

        public PropertyInfo ResolveProperty(PropertyReference reference)
        {
            PropertyInfo resolvedProperty = null;

            // Check for cached field
            if (propertyReferenceCache.TryGetValue(reference, out resolvedProperty) == true)
                return resolvedProperty;

            // Check for generic context
            if (GenericContext.TypeContext != null)
                return GenericContext.TypeContext.GetProperty(reference.Name, memberFlags);

            // Try to resolve in loaded types
            PropertyDefinition definition = null;

            try
            {
                // Catch any exceptions generated by trying to reslove unloaded modules
                definition = reference.Resolve();
            }
            catch { }


            if (definition != null && IsModuleLoaded(definition.DeclaringType.Module.Name) == true)
            {
                // Try to resolve definition
                if (propertyLookupCache.TryGetValue(definition, out resolvedProperty) == true)
                    return resolvedProperty;

                // Failed to resolve
                throw new MissingMemberException(string.Format("Referenced property '{0}::{1}' could not be resolved. Ensure that the declaring assembly is loaded", reference.DeclaringType.FullName, reference.Name));
            }

            // Resolve interop field
            Type resolvedDeclaringType = ResolveType(reference.DeclaringType);

            if (resolvedDeclaringType != null)
            {
                // Try to resolve the field
                resolvedProperty = resolvedDeclaringType.GetProperty(reference.Name);

                // Check for success
                if (resolvedProperty != null)
                    return resolvedProperty;
            }

            // Failed to resolve
            throw new MissingMemberException(string.Format("Referenced property '{0}::{1}' could not be resolved. Ensure that the declaring assembly is loaded", reference.DeclaringType.FullName, reference.Name));
        }

        public MethodBase ResolveConstructor(object ctorToken)
        {
            // Check for field info
            if (ctorToken is ConstructorInfo)
                return ctorToken as ConstructorInfo;

            // Check for field reference
            if (ctorToken is MethodReference)
                return ResolveConstructor(ctorToken as MethodReference);

            throw new MissingMethodException("Failed to resolve ctor reference " + ctorToken);
        }

        public MethodBase ResolveConstructor(MethodReference reference)
        {
            ConstructorInfo resolvedCtor = null;
            MethodBase ctorBase = null;

            // Check for cached field
            if (methodReferenceCache.TryGetValue(reference, out ctorBase) == true)
            {
                resolvedCtor = ctorBase as ConstructorInfo;
                return resolvedCtor;
            }

            // Try to resolve in loaded types
            MethodDefinition definition = null;

            try
            {
                // Catch any exceptions generated by trying to reslove unloaded modules
                definition = reference.Resolve();
            }
            catch { }


            if (definition != null && IsModuleLoaded(definition.DeclaringType.Module.Name) == true && (reference is GenericInstanceMethod) == false)
            {
                // Try to resolve definition
                if (methodLookupCache.TryGetValue(definition, out ctorBase) == true)
                {
                    resolvedCtor = ctorBase as ConstructorInfo;
                    return resolvedCtor;
                }

                // Failed to resolve
                throw new MissingMethodException(string.Format("Referenced constructor '{0}::{1}' could not be resolved. Ensure that the declaring assembly is loaded", reference.DeclaringType.FullName, reference.Name));
            }

            // Resolve interop field
            Type resolvedDeclaringType = ResolveType(reference.DeclaringType);

            if (resolvedDeclaringType != null)
            {
                // Create parameter types
                Type[] parameters = new Type[reference.Parameters.Count];

                // Resolve all parameters
                for (int i = 0; i < parameters.Length; i++)
                    parameters[i] = ResolveType(reference.Parameters[i].ParameterType);

                if (parameters.Length == 0)
                    parameters = Type.EmptyTypes;

                // Try to resolve the field
                resolvedCtor = resolvedDeclaringType.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, parameters, null);
                
                // Check for success
                if (resolvedCtor != null)
                    return resolvedCtor;
            }

            // Failed to resolve
            throw new MissingMethodException(string.Format("Referenced constructor '{0}::{1}' could not be resolved. Ensure that the declaring assembly is loaded", reference.DeclaringType.FullName, reference.Name));
        }

        public MethodBase ResolveMethod(object methodToken)
        {
            // Check for field info
            if (methodToken is MethodInfo)
                return methodToken as MethodInfo;

            // Check for field reference
            if (methodToken is MethodReference)
                return ResolveMethod(methodToken as MethodReference);

            throw new MissingMethodException("Failed to resolve method reference " + methodToken);
        }

        public MethodBase ResolveMethod(MethodReference reference)
        {
            MethodInfo resolvedMethod = null;
            MethodBase methodBase = null;

            // Check for cached field
            if (methodReferenceCache.TryGetValue(reference, out methodBase) == true)
            {
                resolvedMethod = methodBase as MethodInfo;
                return GetOverrideMethodBinding(resolvedMethod);
            }

            // Check for generic context
            if (GenericContext.TypeContext != null)
                return GenericContext.TypeContext.GetMethod(reference.Name, memberFlags);

            // Try to resolve in loaded types
            MethodDefinition definition = null;

            try
            {
                // Catch any exceptions generated by trying to reslove unloaded modules
                definition = reference.Resolve();
            }
            catch { }

            if (definition != null && IsModuleLoaded(definition.DeclaringType.Module.Name) == true && (reference.DeclaringType is GenericInstanceType) == false && (reference is GenericInstanceMethod) == false)
            {
                // Try to resolve definition
                if (methodLookupCache.TryGetValue(definition, out methodBase) == true)
                {
                    resolvedMethod = methodBase as MethodInfo;
                    return GetOverrideMethodBinding(resolvedMethod);
                }

                // Failed to resolve
                throw new MissingMethodException(string.Format("Referenced method '{0}::{1}' could not be resolved. Ensure that the declaring assembly is loaded", reference.DeclaringType.FullName, reference.Name));
            }

            // Resolve interop field
            Type resolvedDeclaringType = ResolveType(reference.DeclaringType);

            if (resolvedDeclaringType != null)
            {
                Type[] genericArguments = Type.EmptyTypes;

                // Check for member access generics
                if(reference.ContainsGenericParameter == true)
                {
                    GenericInstanceMethod genericMethod = (reference as GenericInstanceMethod);

                    if (genericMethod != null)
                    {
                        // Create new array
                        genericArguments = new Type[genericMethod.GenericArguments.Count];

                        // Resolve all generics
                        for (int i = 0; i < genericArguments.Length; i++)
                            genericArguments[i] = ResolveType(genericMethod.GenericArguments[i], resolvedDeclaringType);
                    }
                }

                if (genericArguments.Length > 0)
                {
                    // Get all potentail methods
                    MethodInfo[] methods = resolvedDeclaringType.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);

                    // Select generic method
                    resolvedMethod = methods.Where(m => m.Name == reference.Name && m.IsGenericMethod == true && m.GetParameters().Length == reference.Parameters.Count)
                        .SingleOrDefault();

                    // Make method generic
                    if(resolvedMethod != null)
                        resolvedMethod = resolvedMethod.MakeGenericMethod(genericArguments);
                }
                else
                {
                    // Create parameter types
                    Type[] parameters = Type.EmptyTypes;

                    // Check if the method has types
                    if (reference.Parameters.Count > 0)
                    {
                        parameters = new Type[reference.Parameters.Count];

                        // Resolve all parameters
                        for (int i = 0; i < parameters.Length; i++)
                            parameters[i] = ResolveType(reference.Parameters[i].ParameterType, resolvedDeclaringType);
                    }

                    // Try to resolve the field
                    resolvedMethod = resolvedDeclaringType.GetMethod(reference.Name, parameters);
                }

                // Check for success
                if (resolvedMethod != null)
                    return GetOverrideMethodBinding(resolvedMethod);
            }

            string additionalHint = "";

#if UNITY
            if(UnityEngine.Application.isPlayer == true)
                additionalHint = "This method may have been stripped from the build if you are using IL2CPP!";
#endif

            // Failed to resolve
            throw new MissingMethodException("Failed to resolve method reference " + reference);
        }

        public MethodBase ResolveMethodOrConstructor(object methodOrCtorToken)
        {
            // Check for field info
            if (methodOrCtorToken is MethodBase)
                return methodOrCtorToken as MethodBase;

            // Check for field reference
            if (methodOrCtorToken is MethodReference)
                return ResolveMethodOrConstructor(methodOrCtorToken as MethodReference);

            throw new MissingMethodException("Failed to resolve method or ctor reference " + methodOrCtorToken);
        }

        public MethodBase ResolveMethodOrConstructor(MethodReference reference)
        {
            if(reference.Name == ".ctor")
            {
                // Resolve constructor
                return ResolveConstructor(reference);
            }

            // Resolve as method
            return ResolveMethod(reference);
        }
#endregion

#region CreateInstance
        public object CreateUninitializedInstance(Type type)
        {
            // Check for null
            if (type == null) throw new ArgumentNullException(nameof(type));

            // Check for clr type
            if (type.IsCLRType() == true)
            {
                // Get clr type
                CLRType clrType = type as CLRType;

                // Create instance
                return CLRInstance.CreateAllocatedInstance(this, clrType);
            }
            else
            {
                // Get uninitialized object
                return FormatterServices.GetUninitializedObject(type);
            }
        }

        public object CreateInstance(Type type)
        {
            // Check for null
            if (type == null) throw new ArgumentNullException(nameof(type));

            // Try to get create instance binding
            MethodBase createInstanceOverride = GetOverrideCreateInstanceBinding(type);

            // Invoke override create instance provider
            if (createInstanceOverride != null)
                return createInstanceOverride.Invoke(null, null);

            // Check for clr type
            if (type.IsCLRType() == true)
            {
                // Get clr type
                CLRType clrType = type as CLRType;

                // Get ctor
                CLRConstructor ctor = GetRuntimeCLRCtor(clrType, null);

                // Check for constructor
                if (ctor == null)
                    throw new MissingMethodException("Failed to resolve suitable ctor object initializer");

                // Create instance
                return CLRInstance.CreateAllocatedInstance(this, clrType, ctor, null);
            }
            else
            {
                // Get uninitialized object
                return Activator.CreateInstance(type);
            }
        }

        public object CreateInstance(Type type, object[] args)
        {
            // Check for null
            if (type == null) throw new ArgumentNullException(nameof(type));

            // Try to get create instance binding
            MethodBase createInstanceOverride = GetOverrideCreateInstanceBinding(type);

            // Invoke override create instance provider
            if (createInstanceOverride != null)
                return createInstanceOverride.Invoke(null, args);

            // Check for clr type
            if (type.IsCLRType() == true)
            {
                // Get clr type
                CLRType clrType = type as CLRType;

                // Get ctor
                CLRConstructor ctor = GetRuntimeCLRCtor(clrType, args);

                // Check for constructor
                if (ctor == null)
                    throw new MissingMethodException("Failed to resolve suitable ctor object initializer");

                // Create instance
                return CLRInstance.CreateAllocatedInstance(this, clrType, ctor, args);
            }
            else
            {
                // Create using default ctor
                if (args == null || args.Length == 0)
                    return Activator.CreateInstance(type);

                // Get uninitialized object
                return Activator.CreateInstance(type, args);
            }
        }

        internal object CreateInstance(Type type, MethodBase ctor, params object[] args)
        {
            // Check for null
            if (type == null) throw new ArgumentNullException(nameof(type));

            // Try to get create instance binding
            MethodBase createInstanceOverride = GetOverrideCreateInstanceBinding(type, ctor as ConstructorInfo);

            // Check for dynamic resolve
            if (createInstanceOverride is CLRCreateInstanceBindingCallSite)
                ((CLRCreateInstanceBindingCallSite)createInstanceOverride).DynamicOriginalConstructorCall(ctor);

            // Invoke override create instance provider
            if (createInstanceOverride != null)
                return createInstanceOverride.Invoke(null, args);

            // Check for clr type
            if (type.IsCLRType() == true)
            {
                // Get clr type
                CLRType clrType = type as CLRType;


                // Check for multidimensional array
                if(type.IsArray == true && type.GetArrayRank() >= 2)
                {
                    int[] lengths = new int[args.Length];

                    for (int i = 0; i < args.Length; i++)
                        lengths[i] = (int)args[i];

                    return Array.CreateInstance(typeof(object), lengths);
                }

                // Check for constructor
                if (ctor == null || ((ctor is CLRConstructor) == false && ctor.Name.StartsWith("_SpecialRuntime") == false))
                    throw new MissingMethodException("Failed to resolve suitable ctor object initializer");

                // Get params
                ParameterInfo[] parameters = ctor.GetParameters();

                // Validate arguments and parameters here


                // Create instance
                return CLRInstance.CreateAllocatedInstance(this, clrType, ctor as ConstructorInfo, args);
            }
            else
            {
                Type rt = type.UnderlyingSystemType.GetType();

                // Create using default ctor
                if (args == null || args.Length == 0)
                    return Activator.CreateInstance(type);

                // Get uninitialized object
                return Activator.CreateInstance(type, args);
            }
        }

        public object CreateInstanceFromProxy(Type type, ICLRProxy proxy)
        {
            // Check for clr type
            if (type.IsCLRType() == false)
                throw new ArgumentException("The specified type must be a CLRType");

            // Check for proxy
            if (proxy == null)
                throw new ArgumentNullException("A valid proxy instance must be provided");

            // Get clr type
            CLRType clrType = type as CLRType;

            // Get ctor
            CLRConstructor ctor = GetRuntimeCLRCtor(clrType, null);

            // Check for constructor
            if (ctor == null)
                throw new MissingMethodException("Failed to resolve suitable ctor object initializer");

            // Create instance
            return CLRInstance.CreateAllocatedInstanceWithProxy(this, clrType, ctor, null, proxy);
        }

        public object CreateInstanceFromProxy(Type type, ICLRProxy proxy, object[] args)
        {
            // Check for clr type
            if (type.IsCLRType() == false)
                throw new ArgumentException("The specified type must be a CLRType");

            // Check for proxy
            if (proxy == null)
                throw new ArgumentNullException("A valid proxy instance must be provided");

            // Get clr type
            CLRType clrType = type as CLRType;

            // Get ctor
            CLRConstructor ctor = GetRuntimeCLRCtor(clrType, args);

            // Check for constructor
            if (ctor == null)
                throw new MissingMethodException("Failed to resolve suitable ctor object initializer");

            // Create instance
            return CLRInstance.CreateAllocatedInstanceWithProxy(this, clrType, ctor, args, proxy);
        }

        private CLRConstructor GetRuntimeCLRCtor(CLRType type, object[] args)
        {
            // Check for arguments
            if (args == null || args.Length == 0)
            {
                // Get default constructor
                return type.GetConstructor(Type.EmptyTypes) as CLRConstructor;
            }

            // Find best matching constructor
            CLRConstructor[] constructors = type.GetConstructors() as CLRConstructor[];
            CLRConstructor bestMatch = null;

            for(int i = 0; i < constructors.Length; i++)
            {
                // Get parameters
                ParameterInfo[] parameters = constructors[i].GetParameters();

                // Check for too many arguments
                if (args.Length > parameters.Length)
                    continue;

                int matchCount = 0;

                for(int j = 0; j < parameters.Length; j++)
                {
                    if(args[j] == null)
                    {
                        if(parameters[j].ParameterType.IsClass == true)
                        {
                            matchCount++;
                        }
                    }
                    else
                    {
                        if(parameters[j].ParameterType.IsAssignableFrom(args[j].GetType()) == true)
                        {
                            matchCount++;
                        }
                    }
                }

                // Check for matching constructor
                if (matchCount == args.Length)
                    bestMatch = constructors[i];
            }

            return bestMatch;
        }
#endregion

#region ProxyBinding
        public ICLRProxy CreateCLRProxyBinding(Type type)
        {
            Type bindingProxy;

            // Create instance
            if (clrProxyBindings.TryGetValue(type, out bindingProxy) == true)
            {
                // Create instance of proxy
                object proxyInstance = Activator.CreateInstance(bindingProxy);

                // Get as instance
                return (ICLRProxy)proxyInstance;
            }

            // Generate error
            throw new Exception("Failed to find suitable proxy binding for type: " + type);
        }

        public Type GetCLRProxyBindingForType(Type type, bool throwOnError = true)
        {
            Type bindingProxy;

            // Try to find type
            if (clrProxyBindings.TryGetValue(type, out bindingProxy) == true)
                return bindingProxy;

            // Check for throw
            if(throwOnError == true)
                throw new Exception("Failed to find suitable proxy binding for type: " + type);

            return null;
        }

        public void AddDynamicCLRProxyBinding(Type targetType, Type proxyBindingType)
        {
            if (targetType == null) throw new ArgumentNullException(nameof(targetType));
            if (proxyBindingType == null) throw new ArgumentNullException(nameof(proxyBindingType));

            if (typeof(ICLRProxy).IsAssignableFrom(proxyBindingType) == true)
            {
                // Check for already added
                if (clrProxyBindings.ContainsKey(targetType) == true)
                    throw new CLRBindingException("A proxy binding already exists for the target type '{0}'", targetType);

                // Add type
                clrProxyBindings.Add(targetType, proxyBindingType);
            }
            else
            {
                throw new CLRBindingException("Proxy binding {0} must implement the 'ICLRProxy' interface", proxyBindingType);
            }
        }

        public MethodBase GetOverrideMethodBinding(MethodBase targetMethod)
        {
            MethodBase target;

            // Check for cached method redirect
            if (clrMethodBindings.TryGetValue(targetMethod, out target) == true)
                return target;

            return targetMethod;
        }

        public void AddDynamicOverrideMethodBinding(MethodBase overrideMethod, MethodBase rerouteMethod)
        {
            if (overrideMethod == null) throw new ArgumentNullException(nameof(overrideMethod));
            if (rerouteMethod == null) throw new ArgumentNullException(nameof(rerouteMethod));

            // Check for static correct
            if (rerouteMethod.IsStatic == false)
            {
                throw new CLRBindingException("Method binding {0} must be declared as static", rerouteMethod);
            }

            // Check for correct parameters
            ParameterInfo[] parameterTypes = rerouteMethod.GetParameters();

            if (parameterTypes.Length < 4 ||
                parameterTypes[0].ParameterType != typeof(AppDomain) ||
                parameterTypes[1].ParameterType != typeof(MethodInfo) ||
                parameterTypes[2].ParameterType != typeof(object) ||
                parameterTypes[3].ParameterType != typeof(object[]))
            {
                throw new CLRBindingException("Method binding {0} must have the following parameter signature ({1}, {2}, {3}, {4})", rerouteMethod,
                        typeof(AppDomain),
                        typeof(MethodInfo),
                        typeof(object),
                        typeof(object[]));
            }

            // Check return type
            if (overrideMethod is MethodInfo)
            {
                if (((MethodInfo)overrideMethod).ReturnType == typeof(void) && ((MethodInfo)rerouteMethod).ReturnType != typeof(void))
                {
                    throw new CLRBindingException("Method binding {0} must have a return type of '{1}'", rerouteMethod, typeof(void));
                }
                else if (((MethodInfo)overrideMethod).ReturnType != typeof(void) && ((MethodInfo)rerouteMethod).ReturnType != typeof(object))
                {
                    throw new CLRBindingException("Method binding {0} must have a return type of '{1}'", rerouteMethod, typeof(object));
                }
            }

            // Check for already added
            if (clrMethodBindings.ContainsKey(overrideMethod) == true)
                throw new CLRBindingException(string.Format("A method override already exists for the target method '{0}'", overrideMethod));

            // Register the method
            clrMethodBindings.Add(overrideMethod, new CLRMethodBindingCallSite(this, overrideMethod, rerouteMethod));
        }

        public MethodDirectCallDelegate GetDirectCallDelegate(MethodBase targetMethod)
        {
            MethodDirectCallDelegate target;

            // Check for cached delegate
            if (clrMethodDirectCallBindings.TryGetValue(targetMethod, out target) == true)
                return target;

            return null;
        }

        public void AddDynamicDirectCallDelegate(MethodBase targetMethod, MethodDirectCallDelegate directCallDelegate)
        {
            if (targetMethod == null) throw new ArgumentNullException(nameof(targetMethod));
            if (directCallDelegate == null) throw new ArgumentNullException(nameof(directCallDelegate));

            // Check for already added
            if (clrMethodDirectCallBindings.ContainsKey(targetMethod) == true)
                throw new CLRBindingException("A direct call method binding already exists for the target method '{0}'", targetMethod);

            // Add direct call
            clrMethodDirectCallBindings.Add(targetMethod, directCallDelegate);
        }

        public FieldDirectAccessDelegate GetDirectAccessDelegate(FieldInfo targetField, CLRFieldAccessMode accessMode)
        {
            FieldDirectAccessDelegate target;

            // Check for cached delegate
            if(accessMode == CLRFieldAccessMode.Read)
            {
                if (clrFieldDirectAccessReadBindings.TryGetValue(targetField, out target) == true)
                    return target;
            }
            else if(accessMode == CLRFieldAccessMode.Write)
            {
                if (clrFieldDirectAccessWriteBindings.TryGetValue(targetField, out target) == true)
                    return target;
            }
            return null;
        }

        public void AddDynamicDirectAccessDelegate(FieldInfo targetField, FieldDirectAccessDelegate directAccessDelegate, CLRFieldAccessMode accessMode)
        {
            if (targetField == null) throw new ArgumentNullException(nameof(targetField));
            if (directAccessDelegate == null) throw new ArgumentNullException(nameof(directAccessDelegate));

            // Check for access type
            if(accessMode == CLRFieldAccessMode.Read)
            {
                // Check for already added
                if (clrFieldDirectAccessReadBindings.ContainsKey(targetField) == true)
                    throw new CLRBindingException("A direct access field binding (Read) already exists for the target field '{0}'", targetField);

                // Add binding
                clrFieldDirectAccessReadBindings.Add(targetField, directAccessDelegate);
            }
            else if(accessMode == CLRFieldAccessMode.Write)
            {
                // Check for already added
                if (clrFieldDirectAccessWriteBindings.ContainsKey(targetField) == true)
                    throw new CLRBindingException("A direct access field binding (Write) already exists for the target field '{0}", targetField);
            }
        }

        public MethodBase GetOverrideCreateInstanceBinding(Type createInstanceType, ConstructorInfo ctor = null)
        {
            // Check for generic
            if (createInstanceType.IsGenericType == true)
                createInstanceType = createInstanceType.GetGenericTypeDefinition();

            MethodBase target;

            lock (clrCreateInstanceConstructorBindings)
            {
                if (ctor != null && clrCreateInstanceConstructorBindings.TryGetValue(ctor, out target) == true)
                    return target;
            }

            lock (clrCreateInstanceBindings)
            {
                // Check for cached method redirect
                if (clrCreateInstanceBindings.TryGetValue(createInstanceType, out target) == true)
                    return target;
            }

            // Check for delegate
            if (createInstanceType.BaseType == typeof(MulticastDelegate))
                return GetOverrideCreateInstanceBinding(createInstanceType.BaseType);

            return null;
        }

        public void AddOverrideCreateInstanceBinding(Type createInstanceType, MethodBase createInstanceMethod)
        {
            if (createInstanceType == null) throw new ArgumentNullException(nameof(createInstanceType));
            if (createInstanceMethod == null) throw new ArgumentNullException(nameof(createInstanceMethod));

            // Check for already added
            if (clrCreateInstanceBindings.ContainsKey(createInstanceType) == true)
                throw new CLRBindingException("A create instance override binding already exists for the target type '{0}'", createInstanceType);

            // Add type
            clrCreateInstanceBindings.Add(createInstanceType, createInstanceMethod);

        }

        public void AddOverrideCreateInstanceBinding(ConstructorInfo createInstanceCtor, MethodBase createInstanceMethod)
        {
            if (createInstanceCtor == null) throw new ArgumentNullException(nameof(createInstanceCtor));
            if (createInstanceMethod == null) throw new ArgumentNullException(nameof(createInstanceMethod));

            // Check for already added
            if (clrCreateInstanceConstructorBindings.ContainsKey(createInstanceCtor) == true)
                throw new CLRBindingException("A create instance override binding already exists for the target constructor '{0}'", createInstanceCtor);

            // Add ctor
            clrCreateInstanceConstructorBindings.Add(createInstanceCtor, createInstanceMethod);
        }
#endregion

#region Module
        private bool IsModuleLoaded(string moduleName)
        {
            // Check for name
            foreach (CLRModule module in moduleCache)
            {
                if (module.Assembly.MainModule.Name == moduleName)
                    return true;
            }
            return false;
        }

        private CLRModule GetLoadedModule(string moduleName)
        {
            // Check for name
            foreach (CLRModule module in moduleCache)
            {
                if (module.AssemblyName.Name == moduleName)
                    return module;
            }
            return null;
        }

        private void DefineModule(CLRModule module)
        {
            foreach(CLRType type in module.CLRTypes)
            {
                DefineModuleType(type);
            }
        }

        private void DefineModuleType(CLRType type)
        {
            // Register the main type
            typeCache.Add(type);
            typeLookupCache.Add(type.Definition, type);

            // Process all members
            foreach (MemberInfo member in type.AllMembers)
            {
                // Register nested types
                if (member is CLRType)
                {
                    CLRType nestedType = member as CLRType;
                    DefineModuleType(nestedType);
                }

                // Register fields
                if (member is CLRField)
                {
                    CLRField field = member as CLRField;
                    fieldLookupCache.Add(field.Definition, field);
                }

                // Register properties
                if (member is CLRProperty)
                {
                    CLRProperty property = member as CLRProperty;
                    propertyLookupCache.Add(property.Definition, property);
                }

                // Register constructors
                if (member is CLRConstructor)
                {
                    CLRConstructor constructor = member as CLRConstructor;
                    methodLookupCache.Add(constructor.Definition, constructor);
                }

                // Register methods
                if (member is CLRMethod)
                {
                    CLRMethod method = member as CLRMethod;
                    methodLookupCache.Add(method.Definition, method);
                }
            }
        }
#endregion

        internal VTable GetMethodVTableForType(Type type)
        {
            VTable vTable = null;
            if (methodVTableCache.TryGetValue(type, out vTable) == true)
                return vTable;

            vTable = new VTable();
            methodVTableCache.Add(type, vTable);

            return vTable;
        }

        internal CILSignature GetMethodSignature(MethodBase method)
        {
            CILSignature signature;
            if (methodSignatureCache.TryGetValue(method, out signature) == true)
                return signature;

            Type returnType = null;

            // Get return type for method
            if (method is MethodInfo)
                returnType = ((MethodInfo)method).ReturnType;

            // Create signature
            signature = new CILSignature(method.GetParameters(), returnType);

            // Cache signature
            methodSignatureCache.Add(method, signature);
            return signature;
        }

#region InitializeDomain
        private void InitializeDomain()
        {
            Assembly thisAssembly = typeof(AppDomain).Assembly;
            AssemblyName thisAssemblyName = thisAssembly.GetName();

            foreach (Assembly asm in System.AppDomain.CurrentDomain.GetAssemblies())
            {
                // Check if assembly references this assembly (Proxy types can only be defined in an assembly which reference 'this' assembly)
                if(asm != thisAssembly)
                {
                    // Check references
                    AssemblyName[] references = asm.GetReferencedAssemblies();

                    bool found = false;

                    for(int i = 0; i < references.Length; i++)
                    {
                        if(references[i].FullName == thisAssemblyName.FullName)
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
                    InitializeMethodDirectCallBindings(type);
                    InitializeFieldDirectAccessBindings(type);
                    InitializeCreateInstanceBindings(type);
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

                    // Check for already exists
                    if (clrProxyBindings.ContainsKey(attribute.BaseProxyType) == true)
                    {
#if UNITY
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
#if UNITY
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
                if (method.IsDefined(typeof(CLRMethodBindingAttribute), false) == true)
                {
                    // Check for static correct
                    if (method.IsStatic == false)
                    {
#if UNITY
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
#if UNITY
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

#if UNITY
                        UnityEngine.Debug.LogErrorFormat("Method binding {0} targets a method that could not be resolved: {1}.{2}(3}", method, attribute.DeclaringType, attribute.MethodName, parameterString);
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
#if UNITY
                            UnityEngine.Debug.LogErrorFormat("Method binding {0} must have a return type of '{1}'", method, typeof(void));
                            continue;
#else
                            throw new CLRBindingException("Method binding {0} must have a return type of '{1}'", method, typeof(void));
#endif
                        }
                        else if (((MethodInfo)rerouteMethod).ReturnType != typeof(void) && ((MethodInfo)method).ReturnType != typeof(object))
                        {
#if UNITY
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
#if UNITY
                        UnityEngine.Debug.LogErrorFormat("An override method binding already exists for the taret method '{0}'", rerouteMethod);
                        continue;
#else
                        throw new CLRBindingException("An override method binding already exists for the taret method '{0}'", rerouteMethod);
#endif
                    }

                    // Register the method
                    clrMethodBindings.Add(rerouteMethod, new CLRMethodBindingCallSite(this, rerouteMethod, method));
                }
            }
        }

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
#if UNITY
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
#if UNITY
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

#if UNITY
                        UnityEngine.Debug.LogErrorFormat("Method direct call binding {0} targets a method that could not be resolved: {1}.{2}(3}", method, attribute.DeclaringType, attribute.MethodName, parameterString);
                        continue;
#else
                        throw new CLRBindingException("Method direct call binding {0} targets a method that could not be resolved: {1}.{2}(3}", method, attribute.DeclaringType, attribute.MethodName, parameterString);
#endif
                    }

                    // Create delegate
                    MethodDirectCallDelegate handler = (MethodDirectCallDelegate)((MethodInfo)method).CreateDelegate(typeof(MethodDirectCallDelegate), null);

                    // Check for already added
                    if (clrMethodDirectCallBindings.ContainsKey(delegateMethod) == true)
                    {
#if UNITY
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
#if UNITY
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
#if UNITY
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
#if UNITY
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
#if UNITY
                            UnityEngine.Debug.LogErrorFormat("Field direct access binding {0} must have a return type of '{1}'", method, typeof(void));
                            continue;
#else
                            throw new CLRBindingException("Field direct access binding {0} must have a return type of '{1}'", method, typeof(void));
#endif
                        }
                    }

                    // Create delegate
                    FieldDirectAccessDelegate handler = (FieldDirectAccessDelegate)((MethodInfo)method).CreateDelegate(typeof(FieldDirectAccessDelegate), null);

                    // Register the accessor
                    if(attribute.FieldAccessMode == CLRFieldAccessMode.Read)
                    {
                        // Check for already exists
                        if (clrFieldDirectAccessReadBindings.ContainsKey(fieldAccessor) == true)
                        {
#if UNITY
                            UnityEngine.Debug.LogErrorFormat("A direct access field binding (Read) already exists for the target field '{0}'", fieldAccessor);
                            continue;
#else
                            throw new CLRBindingException("A direct access field binding (Read) already exists for the target field '{0}'", fieldAccessor);
#endif
                        }

                        clrFieldDirectAccessReadBindings.Add(fieldAccessor, handler);
                    }
                    else if(attribute.FieldAccessMode == CLRFieldAccessMode.Write)
                    {
                        // Check for already exists
                        if (clrFieldDirectAccessWriteBindings.ContainsKey(fieldAccessor) == true)
                        {
#if UNITY
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
#if UNITY
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
#if UNITY
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

                    if(((MethodInfo)method).ReturnType != typeof(object))
                    {
#if UNITY
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

#if UNITY
                            UnityEngine.Debug.LogErrorFormat("Create instance constructor binding {0} targets a constructor that could not be resolved: {1}.ctor{2}", method, attribute.DeclaringType, parameterString);
                            continue;
#else
                            throw new CLRBindingException("Create instance constructor binding {0} targets a constructor that could not be resolved: {1}.ctor{2}", method, attribute.DeclaringType, parameterString);
#endif
                        }

                        // Check for already exists
                        if(clrCreateInstanceConstructorBindings.ContainsKey(rerouteCtor) == true)
                        {
#if UNITY
                            UnityEngine.Debug.LogErrorFormat("An override create instance constructor binding already exists for the target constructor '{0}'", rerouteCtor);
                            continue;
#else
                            throw new CLRBindingException("An override create instance constructor binding already exists for the target constructor '{0}'", rerouteCtor);
#endif
                        }

                        // Register method
                        clrCreateInstanceConstructorBindings.Add(rerouteCtor, new CLRCreateInstanceBindingCallSite(this, attribute.DeclaringType, rerouteCtor, method));
                    }
                    else
                    {
                        // Check for already exists
                        if (clrCreateInstanceBindings.ContainsKey(attribute.DeclaringType) == true)
                        {
#if UNITY
                            UnityEngine.Debug.LogErrorFormat("An override create instance binding already exists for the target type '{0}'", attribute.DeclaringType);
                            continue;
#else
                            throw new CLRBindingException("An override create instance binding already exists for the target type '{0}'", attribute.DeclaringType);
#endif
                        }

                        // Register the method
                        clrCreateInstanceBindings.Add(attribute.DeclaringType, new CLRCreateInstanceBindingCallSite(this, attribute.DeclaringType, null, method));
                    }
                }
            }
        }
#endregion

#region Debugger
        public void AttachDebugger(IDebugger debugger)
        {
            if (debugger == null)
                return;

            ExecutionEngine engine = GetExecutionEngine();

            if (engine.IsDebuggerAttached == true)
                throw new InvalidOperationException("A debugger is already attached for this app domain");

            engine.SetDebugger(debugger);

            debugger.OnAttachDebugger(this, engine);
        }

        public void DetatchDebugger(IDebugger debugger)
        {
            throw new NotImplementedException();
        }
#endregion
    }
}
