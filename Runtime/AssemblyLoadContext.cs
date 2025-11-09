using dotnow.Reflection;
using dotnow.Runtime.CIL;
using dotnow.Runtime.JIT;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;

namespace dotnow
{
    public sealed class AssemblyLoadContext : IDisposable
    {
        // Type
        private readonly struct CILMetadataReference
        {
            // Public
            public readonly AssemblyLoadContext ResolvedContext;
            public readonly int ResolvedTokenOrHash;

            // Properties
            public bool IsResolved => ResolvedTokenOrHash != 0;

            // Constructor
            public CILMetadataReference(AssemblyLoadContext resolvedContext, int resolvedToken)
            {
                this.ResolvedContext = resolvedContext;
                this.ResolvedTokenOrHash = resolvedToken;
            }
        }

        // Private
        private readonly AppDomain appDomain;
        private readonly CLRAssembly assembly;
        /// <summary>
        /// Used to stream the assembly metadata and PE image from file or memory.
        /// </summary>
        private readonly MetadataReferenceProvider metadataReferenceProvider;
        /// <summary>
        /// Reference to all interpreted assemblies that are dependent upon this context.
        /// Used to check whether we are able to unload this context, or whether it is still required by another context.
        /// </summary>
        private readonly HashSet<AssemblyLoadContext> clrDependenciesContexts = new();
        /// <summary>
        /// Reference to all interpreted assemblies required by this context.
        /// Used for resolving type references defined within loaded interpreted assemblies, otherwise the type may be an interop type or not available.
        /// The full assembly name is the key.
        /// </summary>
        private readonly Dictionary<string, AssemblyLoadContext> clrReferenceContexts = new();
        /// <summary>
        /// Contains all members defined within this assembly, keyed by the metadata token.
        /// </summary>
        private readonly Dictionary<int, MemberInfo> memberDefinitions = new();


        // Internal
        /// <summary>
        /// Fast access string table to access strings defined within the loaded assembly by metadata token.
        /// </summary>
        internal readonly Dictionary<int, string> stringDefinitions = new();
        /// <summary>
        /// Fast access type definition table to access type handles defined within the loaded assembly by metadata token.
        /// </summary>
        internal readonly CILTypeInfo[] typeDefinitions;
        internal readonly CILTypeInfo[] typeSpecificationDefinitions;
        /// <summary>
        /// Fast access field definition table to access field handles defined within the loaded assembly by metadata token.
        /// </summary>
        internal readonly CILFieldInfo[] fieldDefinitions;
        /// <summary>
        /// Fast access method definition table to access method handles defined within the loaded assembly by metadata token.
        /// </summary>
        internal readonly CILMethodInfo[] methodDefinitions;
        internal readonly CILMethodInfo[] methodSpecificationDefinitions;

        /// <summary>
        /// Semi-fast access to type definitions defined in an external assembly or module.
        /// Must be resolved before access, usually when the calling method body is loaded.
        /// </summary>
        private readonly CILMetadataReference[] typeReferences;
        private readonly CILMetadataReference[] typeSpecificationReferences;
        /// <summary>
        /// Semi-fast access to member definitions defined in an external assembly or module.
        /// Must be resolved before access, usually when the calling method body is loaded.
        /// This could be a field or method reference usually.
        /// </summary>
        private readonly CILMetadataReference[] memberReferences;
        private readonly CILMetadataReference[] memberSpecificationReferences;


        // Properties
        [DebuggerHidden]
        public AppDomain AppDomain => appDomain;
        [DebuggerHidden]
        public Assembly Assembly => assembly;

        // Constructor
        // For testing only
        internal AssemblyLoadContext(AppDomain appDomain) 
        {
            this.appDomain = appDomain;
        }

        internal AssemblyLoadContext(AppDomain appDomain, PEReader peReader, MetadataReader metadataReader, MetadataReader pdbMetadataReader, string location)
        {
            this.appDomain = appDomain;
            this.metadataReferenceProvider = new MetadataReferenceProvider(this, peReader, metadataReader, pdbMetadataReader);

            // Create assembly
            this.assembly = new CLRAssembly(metadataReferenceProvider, location);

            // Note that all tables are indexed at 1, so just add one to size and keep 0 element empty saves modifying offsets for every lookup
            // Initialize definition tables
            this.typeDefinitions = new CILTypeInfo[metadataReferenceProvider.MetadataReader.TypeDefinitions.Count + 1];
            this.fieldDefinitions = new CILFieldInfo[metadataReferenceProvider.MetadataReader.FieldDefinitions.Count + 1];
            this.methodDefinitions = new CILMethodInfo[metadataReferenceProvider.MetadataReader.MethodDefinitions.Count + 1];

            // Initialize reference tables
            this.typeReferences = new CILMetadataReference[metadataReferenceProvider.MetadataReader.TypeReferences.Count + 1];
            this.memberReferences = new CILMetadataReference[metadataReferenceProvider.MetadataReader.MemberReferences.Count + 1];

            // Initialize specification tables
            GetSpecificationTableSizes(metadataReader, out int typeSpecSize, out int methodSpecSize);

            this.typeSpecificationDefinitions = new CILTypeInfo[typeSpecSize + 1];
            this.typeSpecificationReferences = new CILMetadataReference[typeSpecSize + 1];

            this.methodSpecificationDefinitions = new CILMethodInfo[methodSpecSize + 1];
            this.memberSpecificationReferences = new CILMetadataReference[methodSpecSize + 1];
            
            // Define all metadata members
            foreach(CLRType type in assembly.CLRTypes)
            {
                // Define the type
                memberDefinitions[type.MetadataToken] = type;

                // Get all members
                foreach(MemberInfo member in type.GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static))
                {
                    // Define the member
                    memberDefinitions[member.MetadataToken] = member;
                }
            }
        }

        // Methods
        public void Dispose()
        {
            // Check for referenced by other contexts
            if (clrDependenciesContexts.Count > 0)
                throw new InvalidOperationException($"Cannot unload assembly context because it is referenced by {clrDependenciesContexts.First().metadataReferenceProvider.AssemblyLoadContext.Assembly.FullName}");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal string GetUserString(int token)
        {
            string value;
            if(stringDefinitions.TryGetValue(token, out value) == false)
            {
                // Get the string handle
                UserStringHandle handle = MetadataTokens.UserStringHandle(token);

                // Read the string from the stream
                value = metadataReferenceProvider.MetadataReader.GetUserString(handle);

                // Cache the value
                stringDefinitions[token] = value;
            }
            return value;
        }
        internal MemberInfo GetMemberHandle(EntityHandle handle)
        {
            switch(handle.Kind)
            {
                // Check for type
                case HandleKind.TypeDefinition:
                case HandleKind.TypeReference:
                case HandleKind.TypeSpecification:
                    return GetTypeHandle(handle).Type;

                // Check for field
                case HandleKind.FieldDefinition:
                    return GetFieldHandle(handle).Field;

                // Check for method
                case HandleKind.MethodDefinition:
                case HandleKind.MethodSpecification:
                    return GetMethodHandle(handle).Method;
            }

            // Could be a field or a method
            CILMethodInfo method = GetMethodHandle(handle);

            // Check for found
            if (method != null)
                return method.Method;

            // Must be a field?
            CILFieldInfo field = GetFieldHandle(handle);

            // Check for found
            if (field != null)
                return field.Field;

            throw new MissingMemberException("Could not resolve member handle");
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal CILTypeInfo GetTypeHandle(EntityHandle handle)
        {
            // Get the row into the table
            int row = MetadataTokens.GetRowNumber(handle);

            // Check kind
            switch (handle.Kind)
            {
                case HandleKind.TypeDefinition:
                    {
                        // Get definition at row
                        return typeDefinitions[row];
                    }
                case HandleKind.TypeReference:
                    {
                        // Get the reference
                        ref CILMetadataReference @ref = ref typeReferences[row];

                        // Check for reference to an interop type
                        return @ref.ResolvedContext == null
                            ? appDomain.interopTypeHandles[@ref.ResolvedTokenOrHash]
                            : @ref.ResolvedContext.typeDefinitions[@ref.ResolvedTokenOrHash];
                    }
                case HandleKind.TypeSpecification:
                    {
                        // Get the reference
                        ref CILMetadataReference @ref = ref typeSpecificationReferences[row];

                        // Check for reference to an interop type
                        return @ref.ResolvedContext == null
                            ? appDomain.interopTypeHandles[@ref.ResolvedTokenOrHash]
                            : @ref.ResolvedContext.typeSpecificationDefinitions[@ref.ResolvedTokenOrHash];
                    }
                default:
                    throw new InvalidOperationException("Token is not a valid type");
            }
        }

        internal CILFieldInfo GetFieldHandle(EntityHandle handle)
        {
            // Get the row into the table
            int row = MetadataTokens.GetRowNumber(handle);

            // Check kind
            switch (handle.Kind)
            {
                case HandleKind.FieldDefinition:
                    {
                        // Get definition at row
                        return fieldDefinitions[row];
                    }
                case HandleKind.MemberReference:
                    {
                        // Get the reference
                        ref CILMetadataReference @ref = ref memberReferences[row];

                        // Check for reference to an interop type
                        return @ref.ResolvedContext == null
                            ? appDomain.interopFieldHandles[@ref.ResolvedTokenOrHash]
                            : @ref.ResolvedContext.fieldDefinitions[@ref.ResolvedTokenOrHash];
                    }
                default:
                    throw new InvalidOperationException("Token is not a valid field");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal CILMethodInfo GetMethodHandle(EntityHandle handle)
        {
            // Get the row into the table
            int row = MetadataTokens.GetRowNumber(handle);

            // Check kind
            switch (handle.Kind)
            {
                case HandleKind.MethodDefinition:
                    {
                        // Get definition at row
                        return methodDefinitions[row];
                    }
                case HandleKind.MemberReference:
                    {
                        // Get the reference
                        ref CILMetadataReference @ref = ref memberReferences[row];

                        // Get reference to interpreted or interop method
                        return @ref.ResolvedContext == null
                            ? appDomain.interopMethodHandles[@ref.ResolvedTokenOrHash]
                            : @ref.ResolvedContext.methodDefinitions[@ref.ResolvedTokenOrHash];
                    }
                case HandleKind.MethodSpecification:
                    {
                        // Get the reference
                        ref CILMetadataReference @ref = ref memberSpecificationReferences[row];

                        // Get reference to interpreted or interop generic method
                        return @ref.ResolvedContext == null
                            ? appDomain.interopMethodHandles[@ref.ResolvedTokenOrHash]
                            : @ref.ResolvedContext.methodSpecificationDefinitions[@ref.ResolvedTokenOrHash];
                    }
                default:
                    throw new InvalidOperationException("Token is not a valid method");
            }
        }

        /// <summary>
        /// Attempts to resolve and load the handle for the type with the given handle.
        /// </summary>
        /// <param name="handle">The metadata handle which should be a valid type handle</param>
        internal void ResolveType(EntityHandle handle)
        {
            // Check for nil
            if (handle.IsNil == true)
                throw new InvalidOperationException("Type handle is nil");

            switch(handle.Kind)
            {
                case HandleKind.TypeDefinition:
                    {
                        // Get the row
                        int row = MetadataTokens.GetRowNumber(handle);

                        // Check for already resolved
                        if (typeDefinitions[row] != null)
                            return;

                        // Resolve the definition
                        ResolveTypeDefinition((TypeDefinitionHandle)handle, out typeDefinitions[row]);
                        break;
                    }
                case HandleKind.TypeReference:
                    {
                        // Get the row
                        int row = MetadataTokens.GetRowNumber(handle);

                        // Check for already resolved
                        if (typeReferences[row].IsResolved == true)
                            return;

                        // Resolve the reference
                        ResolveTypeReference((TypeReferenceHandle)handle, out typeReferences[row]);
                        break;
                    }
                case HandleKind.TypeSpecification:
                    {
                        // Get the row
                        int row = MetadataTokens.GetRowNumber(handle);

                        // Check for already resolved
                        if (typeSpecificationReferences[row].IsResolved == true)
                            return;

                        // Resolve the specification
                        ResolveTypeSpecification((TypeSpecificationHandle)handle, out typeSpecificationReferences[row]);
                        break;
                    }
                    throw new NotSupportedException(handle.Kind.ToString());
            }
        }

        private void ResolveTypeDefinition(TypeDefinitionHandle typeDefinitionHandle, out CILTypeInfo @ref)
        {
            // Get metadata token
            int token = MetadataTokens.GetToken(typeDefinitionHandle);

            // Lookup the metadata member
            Type definedType = memberDefinitions[token] as Type;

            // Create the type handle
            @ref = new CILTypeInfo(definedType);
        }

        private void ResolveTypeReference(TypeReferenceHandle typeReferenceHandle, out CILMetadataReference @ref)
        {
            // Get the type reference
            TypeReference typeReference = metadataReferenceProvider.MetadataReader.GetTypeReference(typeReferenceHandle);

            // Get type name
            string typeName = metadataReferenceProvider.MetadataReader.GetString(typeReference.Name);

            // Get type namespace
            string typeNamespace = typeReference.Namespace.IsNil == false
                ? metadataReferenceProvider.MetadataReader.GetString(typeReference.Namespace)
                : null;

            // Get type full name
            string typeFullName = typeNamespace != null
                ? string.Concat(typeNamespace, ".", typeName)
                : typeName;
            
            // Check scope
            switch(typeReference.ResolutionScope.Kind)
            {
                case HandleKind.AssemblyReference:
                    {
                        // Get the assembly reference
                        AssemblyReference assemblyReference = metadataReferenceProvider.MetadataReader.GetAssemblyReference((AssemblyReferenceHandle)typeReference.ResolutionScope);

                        // Get the assembly name
                        AssemblyName referenceAssemblyName = assemblyReference.GetAssemblyName();

                        // Check for interpreted
                        AssemblyLoadContext referenceContext;
                        if(clrReferenceContexts.TryGetValue(referenceAssemblyName.FullName, out referenceContext) == true)
                        {
                            // Now try to lookup the type
                            Type resolvedType = metadataReferenceProvider.AssemblyLoadContext.Assembly.GetType(typeName);

                            // Check for found
                            if (resolvedType == null)
                                throw new TypeLoadException(typeFullName);

                            // Get the token
                            EntityHandle handle = MetadataTokens.EntityHandle(resolvedType.MetadataToken);

                            // Get the row
                            int definitionRow = MetadataTokens.GetRowNumber(handle);

                            // Resolve type handle
                            if (referenceContext.typeDefinitions[definitionRow].Type == null)
                            {
                                // Initialize the type handle
                                referenceContext.typeDefinitions[definitionRow] = new CILTypeInfo(resolvedType);
                            }

                            // Resolve the type handle
                            @ref = new CILMetadataReference(
                                referenceContext,
                                definitionRow);
                        }
                        // Must be an interop type
                        else
                        {
                            // Get the full assembly qualified name
                            string assemblyQualifiedTypeName = string.Concat(typeFullName, ", ", referenceAssemblyName.FullName);

                            // Try to lookup the type
                            Type resolvedType = Type.GetType(assemblyQualifiedTypeName, false);

                            // Check for found
                            if(resolvedType == null)
                                throw new TypeLoadException(assemblyQualifiedTypeName);

                            // Ensure that the interop type handle is resolved
                            int hash = appDomain.ResolveInteropTypeHandle(resolvedType);

                            // Resolve the type
                            @ref = new CILMetadataReference(
                                null,                       // Null context because the type is an interop type
                                hash);                      // use hash code as key for interop types
                        }
                        break;
                    }
                default:
                    throw new NotSupportedException(typeReference.ResolutionScope.Kind.ToString());
            }
        }

        private void ResolveTypeSpecification(TypeSpecificationHandle typeSpecificationHandle, out CILMetadataReference @ref)
        {
            // Get the type reference
            TypeSpecification typeSpecification = metadataReferenceProvider.MetadataReader.GetTypeSpecification(typeSpecificationHandle);
            
            // Try to construct the generic type
            Type resolvedGenericType = typeSpecification.DecodeSignature(metadataReferenceProvider, null);

            // Try to get load context
            AssemblyLoadContext referenceContext = resolvedGenericType.GetLoadContext();

            // Check for any
            if(referenceContext != null)
            {
                // Get the row
                int specificationRow = MetadataTokens.GetRowNumber(typeSpecificationHandle);

                // Resolve type handle
                if (referenceContext.typeSpecificationDefinitions[specificationRow].Type == null)
                {
                    // Initialize the type handle
                    referenceContext.typeSpecificationDefinitions[specificationRow] = new CILTypeInfo(resolvedGenericType);
                }

                // Resolve the type handle
                @ref = new CILMetadataReference(
                    referenceContext,
                    specificationRow);
            }
            // Must be an interop type
            else
            {
                // Ensure that the interop type handle is resolved
                int hash = appDomain.ResolveInteropTypeHandle(resolvedGenericType);

                // Resolve the type
                @ref = new CILMetadataReference(
                    null,                       // Null context because the type is an interop type
                    hash);
            }
        }

        internal void ResolveField(EntityHandle handle)
        {
            switch (handle.Kind)
            {
                case HandleKind.FieldDefinition:
                    {
                        // Get the row
                        int row = MetadataTokens.GetRowNumber(handle);

                        // Check for already resolved
                        if (fieldDefinitions[row] != null)
                            return;

                        // Get the field definition
                        FieldDefinition fieldDefinition = metadataReferenceProvider.MetadataReader.GetFieldDefinition((FieldDefinitionHandle)handle);

                        // Get parent type handle
                        TypeDefinitionHandle declaringTypeDefinitionHandle = fieldDefinition.GetDeclaringType();

                        // Resolve the declaring type which will force all fields to be initialized
                        ResolveType(declaringTypeDefinitionHandle);

                        // Lookup the metadata member
                        FieldInfo definedField = memberDefinitions[MetadataTokens.GetToken(handle)] as FieldInfo;

                        // Create the field handle
                        fieldDefinitions[row] = new CILFieldInfo(appDomain, definedField);
                        break;
                    }
                case HandleKind.MemberReference:
                    {
                        // Get the row
                        int row = MetadataTokens.GetRowNumber(handle);

                        // Check for already resolved
                        if (memberReferences[row].IsResolved == true)
                            return;

                        // Get the member reference
                        MemberReference memberReference = metadataReferenceProvider.MetadataReader.GetMemberReference((MemberReferenceHandle)handle);

                        // Check for field
                        if (memberReference.GetKind() != MemberReferenceKind.Field)
                            throw new InvalidOperationException("Attempting to resolve method reference as a field");

                        // Get the parent type handle
                        EntityHandle declaringTypeHandle = memberReference.Parent;

                        // Resolve the declaring type which will force all fields to be initialized
                        ResolveType(declaringTypeHandle);
                        break;
                    }
                    throw new NotSupportedException(handle.Kind.ToString());
            }
        }

        internal void ResolveMethod(EntityHandle handle)
        {
            switch(handle.Kind)
            {
                case HandleKind.MethodDefinition:
                    {
                        // Get the row
                        int row = MetadataTokens.GetRowNumber(handle);

                        // Check for already resolved
                        if (methodDefinitions[row] != null)
                            return;

                        // Resolve the definition
                        ResolveMethodDefinition((MethodDefinitionHandle)handle, out methodDefinitions[row]);
                        break;
                    }
                case HandleKind.MemberReference:
                    {
                        // Get the row
                        int row = MetadataTokens.GetRowNumber(handle);

                        // Check for already resolved
                        if (memberReferences[row].IsResolved == true)
                            return;

                        // Resolve the reference
                        ResolveMethodReference((MemberReferenceHandle)handle, out memberReferences[row]);
                        break;
                    }
                case HandleKind.MethodSpecification:
                    {
                        // Get the row
                        int row = MetadataTokens.GetRowNumber(handle);

                        // Check for already resolved
                        if (methodSpecificationDefinitions[row] != null)
                            return;

                        // Resolve the generic definition
                        ResolveMethodSpecification((MethodSpecificationHandle)handle, out memberSpecificationReferences[row]);
                        break;
                    }
                default:
                    throw new NotSupportedException(handle.Kind.ToString());
            }
        }

        private void ResolveMethodDefinition(MethodDefinitionHandle methodDefinitionHandle, out CILMethodInfo @ref)
        {
            // Get metadata token
            int token = MetadataTokens.GetToken(methodDefinitionHandle);

            // Lookup the metadata member
            MethodBase definedMethod = memberDefinitions[token] as MethodBase;

            // Ensure that the declaring type is loaded
            ResolveType(MetadataTokens.EntityHandle(definedMethod.DeclaringType.MetadataToken));

            // Create the type handle
            @ref = new CILMethodInfo(appDomain, definedMethod);

            // Analyze and resolve members
            // Crucial step because this forces CIL handles to be loaded for referenced members.
            ILAnalyzer.ResolveMetadataTokens(this, @ref);
        }

        private void ResolveMethodReference(MemberReferenceHandle handle, out CILMetadataReference @ref)
        {
            // Get the member reference
            MemberReference memberReference = metadataReferenceProvider.MetadataReader.GetMemberReference(handle);

            // Check kind
            if (memberReference.GetKind() != MemberReferenceKind.Method)
                throw new InvalidOperationException("Metadata token is not a valid method reference");

            // Resolve the declaring type
            ResolveType(memberReference.Parent);

            // Get the declaring type handle
            CILTypeInfo declaringTypeInfo = GetTypeHandle(memberReference.Parent);

            // Get the method name
            string methodName = metadataReferenceProvider.MetadataReader.GetString(memberReference.Name);


            // Resolve signature
            MethodBase resolvedMethod = null;
            MethodSignature<Type> signature = memberReference.DecodeMethodSignature(metadataReferenceProvider, declaringTypeInfo.Type);

            // Build params list
            Type[] parameters = Type.EmptyTypes;

            // Check for any parameters
            if (signature.ParameterTypes.Length > 0)
            {
                // Create array
                parameters = new Type[signature.ParameterTypes.Length];

                // Insert resolve type
                for (int i = 0; i < parameters.Length; i++)
                    parameters[i] = signature.ParameterTypes[i];
            }

            // Check for constructor
            bool isCtor = methodName == ".ctor" || methodName == "..ctor";

            // Get the binding flags for instance or static
            BindingFlags bindingFlags = signature.Header.IsInstance == true
                ? BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance
                : BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

            // Resolve constructor
            if (isCtor == true)
            {
                // Try to resolve ctor
                resolvedMethod = declaringTypeInfo.Type.GetConstructor(bindingFlags, Type.DefaultBinder, parameters, null);
            }
            else
            {
                // Try to resolve method
                try
                {
                    // Try simple resolve
                    resolvedMethod = declaringTypeInfo.Type.GetMethod(methodName, parameters);
                }
                catch (AmbiguousMatchException)
                {
                    // Try to resolve with extra args
                    resolvedMethod = declaringTypeInfo.Type.GetMethod(methodName, signature.GenericParameterCount, bindingFlags, Type.DefaultBinder, parameters, null);
                }
            }

            // Check for resolved
            if (resolvedMethod == null)
                throw new MissingMethodException(memberReference.GetDisplayString(metadataReferenceProvider));

            // Get the assembly context where the method is loaded
            AssemblyLoadContext referenceContext = resolvedMethod.GetLoadContext();

            // Check for interpreted
            if (referenceContext != null)
            {
                // Get the row
                int definitionRow = MetadataTokens.GetRowNumber(handle);

                // Create the method handle
                if (referenceContext.methodDefinitions[definitionRow].Method == null)
                {
                    // Get the method handle
                    CILMethodInfo methodHandle = new CILMethodInfo(appDomain, resolvedMethod);

                    // Initialize method handle
                    referenceContext.methodDefinitions[definitionRow] = methodHandle;

                    // Analyze and resolve members
                    // Crucial step because this forces CIL handles to be loaded for referenced members.
                    ILAnalyzer.ResolveMetadataTokens(this, methodHandle);
                }

                // Resolve the method handle
                @ref = new CILMetadataReference(
                    referenceContext,
                    definitionRow);                
            }
            // Must be an interop method
            else
            {
                // Ensure that the interop type handle is resolved
                int hash = appDomain.ResolveInteropMethodHandle(resolvedMethod);

                // Resolve the type
                @ref = new CILMetadataReference(
                    null,                       // Null context because the method is an interop method
                    hash);                      // Use hash code as key for interop methods
            }            
        }

        private void ResolveMethodSpecification(MethodSpecificationHandle methodSpecificationHandle, out CILMetadataReference @ref)
        {
            // Get the type reference
            MethodSpecification methodSpecification = metadataReferenceProvider.MetadataReader.GetMethodSpecification(methodSpecificationHandle);            

            // Try to construct the generic type
            ImmutableArray<Type> resolvedGenericTypes = methodSpecification.DecodeSignature(metadataReferenceProvider, null);

            // Resolve the generic definition for the method - Only method info can be generic
            MethodInfo resolvedMethodDefinition = (MethodInfo)metadataReferenceProvider.ResolveMetadataMethod(methodSpecification.Method);

            // Construct generic method
            MethodInfo constructedMethodInstance = resolvedMethodDefinition.MakeCLRGeneric(resolvedGenericTypes.ToArray());

            // Get the assembly context where the method is loaded
            AssemblyLoadContext referenceContext = resolvedMethodDefinition.GetLoadContext();

            // Check for any
            if (referenceContext != null)
            {
                // Get the row
                int specificationRow = MetadataTokens.GetRowNumber(methodSpecificationHandle);

                // Resolve type handle
                if (referenceContext.methodSpecificationDefinitions[specificationRow].Method == null)
                {
                    // Initialize the type handle
                    referenceContext.methodSpecificationDefinitions[specificationRow] = new CILMethodInfo(appDomain, constructedMethodInstance);
                }

                // Resolve the type handle
                @ref = new CILMetadataReference(
                    referenceContext,
                    specificationRow);
            }
            // Must be an interop type
            else
            {
                // Ensure that the interop type handle is resolved
                int hash = appDomain.ResolveInteropMethodHandle(constructedMethodInstance);

                // Resolve the type
                @ref = new CILMetadataReference(
                    null,                       // Null context because the type is an interop type
                    hash);
            }
        }

        private static void GetSpecificationTableSizes(MetadataReader metadataReader, out int typeSpecSize, out int methodSpecSize)
        {
            // Get number of type specifications
            typeSpecSize = 0;
            try
            {
                for (int i = 1; ; i++)
                {
                    TypeSpecificationHandle handle = MetadataTokens.TypeSpecificationHandle(i);

                    // Looks like the signature is null meaning we have read all the data??
                    if (metadataReader.GetTypeSpecification(handle).Signature.IsNil == true)
                        break;

                    typeSpecSize++;
                }
            }
            catch { }

            // Get number of method specifications
            methodSpecSize = 0;
            try
            {
                for (int i = 1; ; i++)
                {
                    MethodSpecificationHandle handle = MetadataTokens.MethodSpecificationHandle(i);

                    // Looks like the signature is null meaning we have read all the data??
                    if (metadataReader.GetMethodSpecification(handle).Signature.IsNil == true)
                        break;

                    methodSpecSize++;
                }
            }
            catch { }
        }
    }
}
