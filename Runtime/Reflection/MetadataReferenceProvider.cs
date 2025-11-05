using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;

namespace dotnow.Reflection
{
    /// <summary>
    /// Used to resolve and construct types from metadata, with full support for generics.
    /// </summary>
    internal sealed class MetadataReferenceProvider : ISignatureTypeProvider<Type, MemberInfo>
    {
        // Private
        private readonly AssemblyLoadContext assemblyLoadContext = null;
        private readonly PEReader peReader = null;
        private readonly MetadataReader metadataReader = null;
        private readonly MetadataReader debugSymbolsReader = null;

        // Properties
        public AppDomain AppDomain
        {
            get { return assemblyLoadContext.AppDomain; }
        }

        public AssemblyLoadContext AssemblyLoadContext
        {
            get { return assemblyLoadContext; }
        }

        public PEReader PEReader
        {
            get { return peReader; }
        }

        public MetadataReader MetadataReader
        {
            get { return metadataReader; }
        }

        public MetadataReader DebugSymbolsReader
        {
            get { return debugSymbolsReader; }
        }

        public bool HasDebugSymbols
        {
            get { return debugSymbolsReader != null; }
        }

        // Constructor
        public MetadataReferenceProvider(AssemblyLoadContext loadContext, PEReader peReader, MetadataReader metadataReader, MetadataReader pdbMetadataReader)
        {
            this.assemblyLoadContext = loadContext;
            this.peReader = peReader;
            this.metadataReader = metadataReader;
            this.debugSymbolsReader = pdbMetadataReader;
        }

        // Methods
        public Type ResolveMetadataType(in EntityHandle handle)
        {
            // First resolve the type
            assemblyLoadContext.ResolveType(handle);

            // Get the meta type
            return assemblyLoadContext.GetTypeHandle(handle).Type;
        }

        public FieldInfo ResolveMetadataField(in EntityHandle handle)
        {
            // First resolve the field - TODO

            // Get the meta field
            return assemblyLoadContext.GetFieldHandle(handle).Field;
        }

        public MethodBase ResolveMetadataMethod(in EntityHandle handle)
        {
            // First resolve the method
            assemblyLoadContext.ResolveMethod(handle);

            // Get the meta method
            return assemblyLoadContext.GetMethodHandle(handle).Method;
        }

        Type IConstructedTypeProvider<Type>.GetArrayType(Type elementType, ArrayShape shape)
        {
            // Initialize tha array
            return elementType.MakeArrayType(shape.Rank);
        }

        Type IConstructedTypeProvider<Type>.GetByReferenceType(Type elementType)
        {
            // Get as byref
            return elementType.MakeByRefType();
        }

        Type ISignatureTypeProvider<Type, MemberInfo>.GetFunctionPointerType(MethodSignature<Type> signature)
        {
            throw new NotImplementedException();
        }

        Type IConstructedTypeProvider<Type>.GetGenericInstantiation(Type genericType, ImmutableArray<Type> typeArguments)
        {
            return genericType.MakeGenericType(typeArguments.ToArray());
        }

        Type ISignatureTypeProvider<Type, MemberInfo>.GetGenericMethodParameter(MemberInfo genericContext, int index)
        {
            // Check for generic context and generic instance
            if(genericContext is MethodBase genericMethod && (genericMethod.IsGenericMethod == true && genericMethod.IsGenericMethodDefinition == false))
            {
                // Get the generic arguments
                Type[] genericMethodArguments = genericMethod.GetGenericArguments();
                return genericMethodArguments[index];
            }

            return Type.MakeGenericMethodParameter(index);
        }

        Type ISignatureTypeProvider<Type, MemberInfo>.GetGenericTypeParameter(MemberInfo genericContext, int index)
        {
            // Check for context and generic instance
            if (genericContext is Type genericType && (genericType.IsGenericType == true && genericType.IsGenericTypeDefinition == false))
            {
                // Get the generic arguments
                Type[] genericTypeArguments = genericType.GetGenericArguments();
                return genericTypeArguments[index];
            }

            return new CLRGenericParameterType(index);
        }

        Type ISignatureTypeProvider<Type, MemberInfo>.GetModifiedType(Type modifier, Type unmodifiedType, bool isRequired)
        {
            // Do nothing currently
            return unmodifiedType;
        }

        Type ISignatureTypeProvider<Type, MemberInfo>.GetPinnedType(Type elementType)
        {
            // Pinned not supported
            return elementType;
        }

        Type IConstructedTypeProvider<Type>.GetPointerType(Type elementType)
        {
            // Get as pointer
            return elementType.MakePointerType();
        }

        Type ISimpleTypeProvider<Type>.GetPrimitiveType(PrimitiveTypeCode typeCode)
        {
            return typeCode switch
            {                
                PrimitiveTypeCode.Void => typeof(void),
                PrimitiveTypeCode.Boolean => typeof(bool),
                PrimitiveTypeCode.Char => typeof(char),
                PrimitiveTypeCode.SByte => typeof(sbyte),
                PrimitiveTypeCode.Byte => typeof(byte),
                PrimitiveTypeCode.Int16 => typeof(short),
                PrimitiveTypeCode.UInt16 => typeof(ushort),
                PrimitiveTypeCode.Int32 => typeof(int),
                PrimitiveTypeCode.UInt32 => typeof(uint),
                PrimitiveTypeCode.Int64 => typeof(long),
                PrimitiveTypeCode.UInt64 => typeof(ulong),
                PrimitiveTypeCode.IntPtr => typeof(IntPtr),
                PrimitiveTypeCode.UIntPtr => typeof(UIntPtr),
                PrimitiveTypeCode.Single => typeof(float),
                PrimitiveTypeCode.Double => typeof(double),
                PrimitiveTypeCode.Object => typeof(object),
                PrimitiveTypeCode.String => typeof(string),
                PrimitiveTypeCode.TypedReference => typeof(TypedReference),
                _ => throw new InvalidOperationException(),
            };
        }

        Type ISZArrayTypeProvider<Type>.GetSZArrayType(Type elementType)
        {
            // Make as array
            return elementType.MakeArrayType();
        }

        Type ISimpleTypeProvider<Type>.GetTypeFromDefinition(MetadataReader reader, TypeDefinitionHandle handle, byte rawTypeKind)
        {
            // Resolve the definition
            return ResolveMetadataType(handle);
        }

        Type ISimpleTypeProvider<Type>.GetTypeFromReference(MetadataReader reader, TypeReferenceHandle handle, byte rawTypeKind)
        {
            // Resolve the type
            return ResolveMetadataType(handle);
        }

        Type ISignatureTypeProvider<Type, MemberInfo>.GetTypeFromSpecification(MetadataReader reader, MemberInfo genericContext, TypeSpecificationHandle handle, byte rawTypeKind)
        {
            return ResolveMetadataType(handle);
        }
    }
}
