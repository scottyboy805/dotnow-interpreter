using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading;
using Mono.Cecil;
using Mono.Collections.Generic;
using dotnow.Debugging;
using dotnow.Interop;
using dotnow.Reflection;
using dotnow.Runtime;
using dotnow.Runtime.CIL;
using System.Runtime.CompilerServices;
using dotnow.Runtime.JIT;

[assembly: InternalsVisibleTo("dotnow.Integration")]

namespace dotnow
{
    public class AppDomain : IDisposable
    {
        // Delegate
        public delegate void MethodDirectCallDelegate(StackData[] stack, int offset);
        public delegate void FieldDirectAccessDelegate(StackData[] stack, int offset);

        // Internal
        internal static event Action<AppDomain> OnDomainCreated;

        internal Thread mainThread = null;
        internal Dictionary<MethodBase, object> delegateCache = new Dictionary<MethodBase, object>();

        // Private
        private Bindings bindings = null;
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

        public IReadOnlyCollection<CLRModule> Modules
        {
            get { return moduleCache; }
        }

        // Constructor
        public AppDomain()
        {
            this.mainThread = Thread.CurrentThread;
            this.engine = new ExecutionEngine(mainThread);

            // Initialize bindings
            bindings = new Bindings(this);

            // Run jit on interpreter method - This is a big method that takes a long time to JIT on demand - we don't want to see that time in the host application so we should do it at initialize time.
#if DISABLE_JIT_PREWARM == false
            MethodInfo method = typeof(CILInterpreterUnsafe).GetMethod("ExecuteInterpreted", BindingFlags.Static | BindingFlags.NonPublic);
            RuntimeHelpers.PrepareMethod(method.MethodHandle);
#endif

            // Trigger domain create
            if(OnDomainCreated != null)
                OnDomainCreated.Invoke(this);
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
        public CLRModule LoadModule(string path, bool keepOpen, bool optimizeOnLoad = true)
        {
            // Try to read stream
            Stream input = File.OpenRead(path);

            // Load the stream
            return LoadModuleStream(input, keepOpen, optimizeOnLoad);
        }

        public CLRModule LoadModuleData(byte[] moduleData, bool keepOpen, bool optimizeOnLoad = true)
        {
            // Create stream
            MemoryStream input = new MemoryStream(moduleData);

            // Load the stream
            return LoadModuleStream(input, keepOpen, optimizeOnLoad);
        }

        public CLRModule LoadModuleStream(Stream input, bool keepOpen, bool optimizeOnLoad = true)
        {
            // Try to load the definition
            AssemblyDefinition definition = AssemblyDefinition.ReadAssembly(input, new ReaderParameters(ReadingMode.Deferred));
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

                    // Optimize mode at this stage so that exection can run as fast as possible
                    if(optimizeOnLoad == true)
                        JITOptimize.EnsureJITOptimized(module);
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

                string mainTypeFullName = reference.FullName.Replace("/", "+");


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
                        genericTypes[i] = ResolveType(generics[i], typeContext);

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
                {
                    // Check for system type used as the base generic
                    if (resolvedType.IsCLRType() == false)
                    {
                        // 'SystemType<InterpretedType>' must be mapped to 'SystemType<object>' because the clr does not know about interpreted types at all
                        for (int i = 0; i < genericTypes.Length; i++)
                        {
                            if (genericTypes[i].IsCLRType() == true)
                                genericTypes[i] = typeof(object);
                        }
                    }

                    resolvedType = resolvedType.MakeGenericType(genericTypes);
                }

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
                    parameters[i] = ResolveType(reference.Parameters[i].ParameterType, resolvedDeclaringType);

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

                    // Handle implicit and explicit operators
                    if (reference.Name.StartsWith("op_", StringComparison.Ordinal) == true && (reference.Name == "op_Implicit" || reference.Name == "op_Explicit"))
                    {
                        // Resolve return type
                        Type returnType = ResolveType(reference.ReturnType, resolvedDeclaringType);

                        // Get public static methods - implicit and explicit operators must be public static so we can save some work
                        MethodInfo[] methods = resolvedDeclaringType.GetMethods(BindingFlags.Public | BindingFlags.Static);

                        for(int i = 0; i < methods.Length; i++)
                        {
                            if (methods[i].Name == reference.Name)
                            {
                                // Operations must have 1 parameter
                                Type paramType = methods[i].GetParameters()[0].ParameterType;

                                // Check for matching parameter type
                                if(paramType == parameters[0] && returnType == methods[i].ReturnType)
                                {
                                    resolvedMethod = methods[i];
                                    break;
                                }
                            }
                        }
                    }
                    // Standard behaviour for normal methods
                    else
                    {
                        // Try to resolve the field
                        resolvedMethod = resolvedDeclaringType.GetMethod(reference.Name, parameters);
                    }
                }

                // Check for success
                if (resolvedMethod != null)
                    return GetOverrideMethodBinding(resolvedMethod);
            }

#if !UNITY_DISABLE
#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_IOS || UNITY_ANDROID || UNITY_WSA || UNITY_WEBGL || UNITY_SWITCH)
            if (UnityEngine.Application.isEditor == false)
                UnityEngine.Debug.Log("This method may have been stripped from the build if you are using IL2CPP!");
#endif
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
            if (type == null) throw new ArgumentNullException("type");

            object inst;

            // Check for clr type
            if (type.IsCLRType() == true)
            {
                // Get clr type
                CLRType clrType = type as CLRType;

                // Create instance
                inst = CLRInstanceOld.CreateAllocatedInstance(this, clrType);
            }
            else
            {
                // Get uninitialized object
                inst = FormatterServices.GetUninitializedObject(type);
            }

            // Check for exception
            if (inst is Exception)
                inst = engine.CreateException((Exception)inst);

            return inst;
        }

        public object CreateInstance(Type type)
        {
            // Check for null
            if (type == null) throw new ArgumentNullException("type");

            object inst;

            // Try to get create instance binding
            MethodBase createInstanceOverride = GetOverrideCreateInstanceBinding(type);

            // Invoke override create instance provider
            if (createInstanceOverride != null)
            {
                inst = createInstanceOverride.Invoke(null, null);
            }
            else
            {
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
                    inst = CLRInstanceOld.CreateAllocatedInstance(this, clrType, ctor, null);
                }
                else
                {
                    // Get uninitialized object
                    inst = Activator.CreateInstance(type);
                }
            }

            // Check for exception
            if (inst is Exception)
                inst = engine.CreateException((Exception)inst);

            return inst;
        }

        public object CreateInstance(Type type, object[] args)
        {
            // Check for null
            if (type == null) throw new ArgumentNullException("type");

            object inst;

            // Try to get create instance binding
            MethodBase createInstanceOverride = GetOverrideCreateInstanceBinding(type);

            // Invoke override create instance provider
            if (createInstanceOverride != null)
            {
                inst = createInstanceOverride.Invoke(null, args);
            }
            else
            {
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
                    inst = CLRInstanceOld.CreateAllocatedInstance(this, clrType, ctor, args);
                }
                else
                {
                    // Create using default ctor
                    if (args == null || args.Length == 0)
                    {
                        inst = Activator.CreateInstance(type);
                    }
                    else
                    {
                        // Get uninitialized object
                        inst = Activator.CreateInstance(type, args);
                    }
                }
            }

            // Check for exception
            if (inst is Exception)
                inst = engine.CreateException((Exception)inst);

            return inst;
        }

        internal object CreateInstance(Type type, MethodBase ctor, params object[] args)
        {
            // Check for null
            if (type == null) throw new ArgumentNullException("type");

            object inst = null;

            // Try to get create instance binding
            MethodBase createInstanceOverride = GetOverrideCreateInstanceBinding(type, ctor as ConstructorInfo);

            // Check for dynamic resolve
            if (createInstanceOverride is CLRCreateInstanceBindingCallSite)
                ((CLRCreateInstanceBindingCallSite)createInstanceOverride).DynamicOriginalConstructorCall(ctor, type);

            // Invoke override create instance provider
            if (createInstanceOverride != null)
            {
                inst = createInstanceOverride.Invoke(null, args);
            }
            else
            {
                // Check for clr type
                if (type.IsCLRType() == true)
                {
                    // Get clr type
                    CLRType clrType = type as CLRType;


                    // Check for multidimensional array
                    if (type.IsArray == true && type.GetArrayRank() >= 2)
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
                    inst = CLRInstanceOld.CreateAllocatedInstance(this, clrType, ctor as ConstructorInfo, args);
                }
                else
                {
                    Type rt = type.UnderlyingSystemType.GetType();

                    // Create using default ctor
                    if (args == null || args.Length == 0)
                    {
                        // Create instance default
                        inst = Activator.CreateInstance(type);
                    }
                    else
                    {
                        try
                        {
                            // Create instance - try to infer ctor from args
                            inst = Activator.CreateInstance(type, args);
                        }
                        catch(MissingMethodException)
                        {
                            if (ctor == null)
                                throw;

                            // Create instance using explicit constructor
                            inst = FormatterServices.GetUninitializedObject(type);
                            ctor.Invoke(inst, args);
                        }
                    }
                }
            }

            // Check for exception
            if (inst is Exception)
                inst = engine.CreateException((Exception)inst);

            return inst;
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
            return CLRInstanceOld.CreateAllocatedInstanceWithProxy(this, clrType, ctor, null, proxy);
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
            return CLRInstanceOld.CreateAllocatedInstanceWithProxy(this, clrType, ctor, args, proxy);
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
            if (bindings.clrProxyBindings.TryGetValue(type, out bindingProxy) == true)
            {
                // Create instance of proxy
                object proxyInstance = Activator.CreateInstance(bindingProxy);

                // Get as instance
                return (ICLRProxy)proxyInstance;
            }

            // Generate error
            throw new Exception("Failed to find suitable proxy binding for type: " + type);
        }

        public object CreateCLRProxyBindingOrImplicitInteropInstance(Type type)
        {
            Type bindingProxy;

            // Create instance
            if (bindings.clrProxyBindings.TryGetValue(type, out bindingProxy) == true)
            {
                // Create instance of proxy
                object proxyInstance = Activator.CreateInstance(bindingProxy);

                // Get as instance
                return proxyInstance;
            }


            // Check if type has public accessible constructor and no virtual or abstract methods - we can create the instance automatically in that case with out requiring a a proxy implementation
            if(IsProxyBindingRequiredForType(type) == false)
            {
                // Create uninitialized instance
                // Ctor will be run when CLRInstance is initialized
                return FormatterServices.GetUninitializedObject(type);
            }

            // Generate error
            throw new Exception("Failed to find suitable proxy binding for type and an implicit interop instance could not be created automatically: " + type);
        }

        public bool IsProxyBindingRequiredForType(Type type)
        {
            // Check for null
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            // Check for abstract
            if (type.IsAbstract == true)
                return true;

            // Check for any virtual members
            return type.HasVirtualMembers() == false;
        }

        public Type GetCLRProxyBindingForType(Type type, bool throwOnError = true)
        {
            Type bindingProxy;

            // If type is multi inheritance then move down the hierarchy to select the first interop base type
            type = type.GetInteropBaseType();

            // Try to find type
            if (bindings.clrProxyBindings.TryGetValue(type, out bindingProxy) == true)
                return bindingProxy;

            // Check for throw
            if(throwOnError == true)
                throw new Exception("Failed to find suitable proxy binding for type: " + type);

            return null;
        }

        public void AddDynamicCLRProxyBinding(Type targetType, Type proxyBindingType)
        {
            if (targetType == null) throw new ArgumentNullException("targetType");
            if (proxyBindingType == null) throw new ArgumentNullException("proxyBindingType");

            if (typeof(ICLRProxy).IsAssignableFrom(proxyBindingType) == true)
            {
                // Check for already added
                if (bindings.clrProxyBindings.ContainsKey(targetType) == true)
                    throw new CLRBindingException("A proxy binding already exists for the target type '{0}'", targetType);

                // Add type
                bindings.clrProxyBindings.Add(targetType, proxyBindingType);
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
            if (bindings.clrMethodBindings.TryGetValue(targetMethod, out target) == true)
                return target;

            return targetMethod;
        }

        public void AddDynamicOverrideMethodBinding(MethodBase overrideMethod, MethodBase rerouteMethod)
        {
            if (overrideMethod == null) throw new ArgumentNullException("overrideMethod");
            if (rerouteMethod == null) throw new ArgumentNullException("RerouteMethod");

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
            if (bindings.clrMethodBindings.ContainsKey(overrideMethod) == true)
                throw new CLRBindingException(string.Format("A method override already exists for the target method '{0}'", overrideMethod));

            // Register the method
            bindings.clrMethodBindings.Add(overrideMethod, new CLRMethodBindingCallSite(this, overrideMethod, rerouteMethod));
        }

        public MethodDirectCallDelegate GetDirectCallDelegate(MethodBase targetMethod)
        {
            MethodDirectCallDelegate target;

            // Check for cached delegate
            if (bindings.clrMethodDirectCallBindings.TryGetValue(targetMethod, out target) == true)
                return target;

            return null;
        }

        public void AddDynamicDirectCallDelegate(MethodBase targetMethod, MethodDirectCallDelegate directCallDelegate)
        {
            if (targetMethod == null) throw new ArgumentNullException("targetMethod");
            if (directCallDelegate == null) throw new ArgumentNullException("directCallDelegate");

            // Check for already added
            if (bindings.clrMethodDirectCallBindings.ContainsKey(targetMethod) == true)
                throw new CLRBindingException("A direct call method binding already exists for the target method '{0}'", targetMethod);

            // Add direct call
            bindings.clrMethodDirectCallBindings.Add(targetMethod, directCallDelegate);
        }

        public FieldDirectAccessDelegate GetDirectAccessDelegate(FieldInfo targetField, CLRFieldAccessMode accessMode)
        {
            FieldDirectAccessDelegate target;

            // Check for cached delegate
            if(accessMode == CLRFieldAccessMode.Read)
            {
                if (bindings.clrFieldDirectAccessReadBindings.TryGetValue(targetField, out target) == true)
                    return target;
            }
            else if(accessMode == CLRFieldAccessMode.Write)
            {
                if (bindings.clrFieldDirectAccessWriteBindings.TryGetValue(targetField, out target) == true)
                    return target;
            }
            return null;
        }

        public void AddDynamicDirectAccessDelegate(FieldInfo targetField, FieldDirectAccessDelegate directAccessDelegate, CLRFieldAccessMode accessMode)
        {
            if (targetField == null) throw new ArgumentNullException("targetField");
            if (directAccessDelegate == null) throw new ArgumentNullException("directAccessDelegate");

            // Check for access type
            if(accessMode == CLRFieldAccessMode.Read)
            {
                // Check for already added
                if (bindings.clrFieldDirectAccessReadBindings.ContainsKey(targetField) == true)
                    throw new CLRBindingException("A direct access field binding (Read) already exists for the target field '{0}'", targetField);

                // Add binding
                bindings.clrFieldDirectAccessReadBindings.Add(targetField, directAccessDelegate);
            }
            else if(accessMode == CLRFieldAccessMode.Write)
            {
                // Check for already added
                if (bindings.clrFieldDirectAccessWriteBindings.ContainsKey(targetField) == true)
                    throw new CLRBindingException("A direct access field binding (Write) already exists for the target field '{0}", targetField);
            }
        }

        public MethodBase GetOverrideCreateInstanceBinding(Type createInstanceType, ConstructorInfo ctor = null)
        {
            // Check for generic
            if (createInstanceType.IsGenericType == true)
                createInstanceType = createInstanceType.GetGenericTypeDefinition();

            MethodBase target;

            lock (bindings.clrCreateInstanceConstructorBindings)
            {
                if (ctor != null && bindings.clrCreateInstanceConstructorBindings.TryGetValue(ctor, out target) == true)
                    return target;
            }

            lock (bindings.clrCreateInstanceBindings)
            {
                // Check for cached method redirect
                if (bindings.clrCreateInstanceBindings.TryGetValue(createInstanceType, out target) == true)
                    return target;
            }

            // Check for delegate
            if (createInstanceType.BaseType == typeof(MulticastDelegate))
                return GetOverrideCreateInstanceBinding(createInstanceType.BaseType);

            return null;
        }

        public void AddOverrideCreateInstanceBinding(Type createInstanceType, MethodBase createInstanceMethod)
        {
            if (createInstanceType == null) throw new ArgumentNullException("createInstanceType");
            if (createInstanceMethod == null) throw new ArgumentNullException("createInstanceMethod");

            // Check for already added
            if (bindings.clrCreateInstanceBindings.ContainsKey(createInstanceType) == true)
                throw new CLRBindingException("A create instance override binding already exists for the target type '{0}'", createInstanceType);

            // Add type
            bindings.clrCreateInstanceBindings.Add(createInstanceType, createInstanceMethod);

        }

        public void AddOverrideCreateInstanceBinding(ConstructorInfo createInstanceCtor, MethodBase createInstanceMethod)
        {
            if (createInstanceCtor == null) throw new ArgumentNullException("createInstanceCtor");
            if (createInstanceMethod == null) throw new ArgumentNullException("createInstanceMethod");

            // Check for already added
            if (bindings.clrCreateInstanceConstructorBindings.ContainsKey(createInstanceCtor) == true)
                throw new CLRBindingException("A create instance override binding already exists for the target constructor '{0}'", createInstanceCtor);

            // Add ctor
            bindings.clrCreateInstanceConstructorBindings.Add(createInstanceCtor, createInstanceMethod);
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

        public Type ResolveType(string assemblyName, string typeName)
        {
            CLRModule match = null;

            // Try to find module (Maybe optimize this later)
            foreach(CLRModule module in moduleCache)
            {
                if(module.FullName == assemblyName || module.GetName().Name == assemblyName)
                {
                    match = module;
                    break;
                }
            }

            if(match != null)
            {
                // Try to get type
                return match.GetType(typeName, false);
            }

            // Type not found
            return null;
        }

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

            ParameterInfo[] parameterTypes = null;

            try
            {
                parameterTypes = method.GetParameters();
            }
            catch
            {
                parameterTypes = new ParameterInfo[0];
            }

            // Create signature
            signature = new CILSignature(parameterTypes, returnType);

            // Cache signature
            methodSignatureCache.Add(method, signature);
            return signature;
        }

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
