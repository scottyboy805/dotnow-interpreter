using System;
using System.Reflection.Metadata;
using System.Text;

namespace dotnow.Reflection
{
    internal static class MetadataExtensions
    {
        // Methods
        public static string GetDisplayString(in this EntityHandle entity, MetadataReferenceProvider metadataProvider)
        {
            // Check for nil
            if (entity.IsNil == true)
                return "Nil";

            switch(entity.Kind)
            {
                default: throw new NotSupportedException(entity.Kind.ToString());

                // Type
                case HandleKind.TypeReference:
                    {
                        // Get as type reference
                        TypeReference typeRef = metadataProvider.MetadataReader.GetTypeReference((TypeReferenceHandle)entity);
                        return typeRef.GetDisplayString(metadataProvider);
                    }
                case HandleKind.TypeDefinition:
                    {
                        // Get as type definition
                        TypeDefinition typeDef = metadataProvider.MetadataReader.GetTypeDefinition((TypeDefinitionHandle)entity);
                        return typeDef.GetDisplayString(metadataProvider);
                    }
                case HandleKind.TypeSpecification:
                    {
                        // Get as type specification
                        TypeSpecification typeSpec = metadataProvider.MetadataReader.GetTypeSpecification((TypeSpecificationHandle)entity);
                        return typeSpec.GetDisplayString(metadataProvider);
                    }

                // Member
                case HandleKind.MemberReference:
                    {
                        // Get as member reference
                        MemberReference memberRef = metadataProvider.MetadataReader.GetMemberReference((MemberReferenceHandle)entity);
                        return memberRef.GetDisplayString(metadataProvider);
                    }

                // Method
                case HandleKind.MethodDefinition:
                    {
                        // Get as method definition
                        MethodDefinition methodDef = metadataProvider.MetadataReader.GetMethodDefinition((MethodDefinitionHandle)entity);
                        return methodDef.GetDisplayString(metadataProvider);
                    }
            }
        }

        public static string GetDisplayString(in this TypeReferenceHandle typeRefHandle, MetadataReferenceProvider metadataProvider) => GetDisplayString(metadataProvider.MetadataReader.GetTypeReference(typeRefHandle), metadataProvider);
        public static string GetDisplayString(in this TypeReference typeRef, MetadataReferenceProvider metadataProvider)
        {
            // Build name
            string fullName = metadataProvider.MetadataReader.GetString(typeRef.Name);

            // Check for namespace
            if (typeRef.Namespace.IsNil == false)
            {
                fullName = metadataProvider.MetadataReader.GetString(typeRef.Namespace)
                    + "." + fullName;
            }
            return fullName;
        }

        public static string GetDisplayString(in this TypeDefinitionHandle typeDefHandle, MetadataReferenceProvider metadataProvider) => GetDisplayString(metadataProvider.MetadataReader.GetTypeDefinition(typeDefHandle), metadataProvider);
        public static string GetDisplayString(in this TypeDefinition typeDef, MetadataReferenceProvider metadataProvider)
        {
            // Build name
            string fullName = metadataProvider.MetadataReader.GetString(typeDef.Name);

            // Check for namespace
            if(typeDef.Namespace.IsNil == false)
            {
                fullName = metadataProvider.MetadataReader.GetString(typeDef.Namespace)
                    + "." + fullName;
            }
            return fullName;
        }

        public static string GetDisplayString(in this TypeSpecificationHandle typeSpecHandle, MetadataReferenceProvider metadataProvider) => GetDisplayString(metadataProvider.MetadataReader.GetTypeSpecification(typeSpecHandle), metadataProvider);
        public static string GetDisplayString(in this TypeSpecification typeSpec, MetadataReferenceProvider metadataProvider)
        {
            // Decode the signature
            Type type = typeSpec.DecodeSignature(metadataProvider, null);

            // Get type name
            return type.FullName;
        }

        public static string GetDisplayString(in this MemberReferenceHandle memberRefHandle, MetadataReferenceProvider metadataProvider) => GetDisplayString(metadataProvider.MetadataReader.GetMemberReference(memberRefHandle), metadataProvider);
        public static string GetDisplayString(in this MemberReference memberRef, MetadataReferenceProvider metadataProvider)
        {
            StringBuilder builder = new StringBuilder();

            // Check for kind
            switch(memberRef.GetKind())
            {
                case MemberReferenceKind.Field:
                    {
                        // Get signature
                        Type fieldType = memberRef.DecodeFieldSignature(metadataProvider, null);

                        // Field type
                        builder.Append(fieldType);
                        builder.Append(' ');

                        // Declaring type
                        builder.Append(memberRef.Parent.GetDisplayString(metadataProvider));
                        builder.Append('.');

                        // Field name
                        builder.Append(metadataProvider.MetadataReader.GetString(memberRef.Name));
                        break;
                    }
                case MemberReferenceKind.Method:
                    {
                        // Get signature
                        MethodSignature<Type> methodSignature = memberRef.DecodeMethodSignature(metadataProvider, null);

                        // Get name
                        string methodName = metadataProvider.MetadataReader.GetString(memberRef.Name);
                        bool isCtor = methodName == ".ctor" || methodName == "..ctor";

                        // Return type
                        if (methodSignature.ReturnType != null && isCtor == false)
                        {
                            builder.Append(methodSignature.ReturnType);
                            builder.Append(' ');
                        }

                        // Declaring type
                        builder.Append(memberRef.Parent.GetDisplayString(metadataProvider));

                        if(isCtor == false)
                            builder.Append('.');

                        // Method name
                        builder.Append(methodName);

                        // Parameters
                        builder.Append('(');
                        {
                            // Process all
                            for (int i = 0; i < methodSignature.ParameterTypes.Length; i++)
                            {
                                // Display type string
                                builder.Append(methodSignature.ParameterTypes[i]);

                                // Append divider
                                if (i < methodSignature.ParameterTypes.Length - 1)
                                    builder.Append(", ");
                            }
                        }
                        builder.Append(")");
                        break;
                    }
            }

            return builder.ToString();
        }

        public static string GetDisplayString(in this FieldDefinitionHandle fieldDefHandle, MetadataReferenceProvider metadataProvider) => GetDisplayString(metadataProvider.MetadataReader.GetFieldDefinition(fieldDefHandle), metadataProvider);
        public static string GetDisplayString(in this FieldDefinition fieldDef, MetadataReferenceProvider metadataProvider)
        {
            StringBuilder builder = new StringBuilder();
            string s = metadataProvider.MetadataReader.GetString(fieldDef.Name);

            // Get signature
            Type fieldType = fieldDef.DecodeSignature(metadataProvider, null);

            // Field type
            builder.Append(fieldType);
            builder.Append(' ');

            // Declaring type
            builder.Append(fieldDef.GetDeclaringType().GetDisplayString(metadataProvider));
            builder.Append('.');

            // Field name
            builder.Append(metadataProvider.MetadataReader.GetString(fieldDef.Name));

            return builder.ToString();
        }

        public static string GetDisplayString(in this MethodDefinitionHandle methodDefHandle, MetadataReferenceProvider metadataProvider) => GetDisplayString(metadataProvider.MetadataReader.GetMethodDefinition(methodDefHandle), metadataProvider);
        public static string GetDisplayString(in this MethodDefinition methodDef, MetadataReferenceProvider metadataProvider)
        {
            StringBuilder builder = new StringBuilder();

            // Get signature
            MethodSignature<Type> methodSignature = methodDef.DecodeSignature(metadataProvider, null);

            // Return type
            if(methodSignature.ReturnType != null)
            {
                builder.Append(methodSignature.ReturnType);
                builder.Append(' ');
            }

            // Declaring type
            builder.Append(methodDef.GetDeclaringType().GetDisplayString(metadataProvider));
            builder.Append('.');

            // Method name
            builder.Append(metadataProvider.MetadataReader.GetString(methodDef.Name));

            // Parameters
            builder.Append('(');
            {
                // Process all
                for (int i = 0; i < methodSignature.ParameterTypes.Length; i++)
                {
                    // Display type string
                    builder.Append(methodSignature.ParameterTypes[i]);

                    // Append divider
                    if(i < methodSignature.ParameterTypes.Length - 1)
                        builder.Append(", ");
                }
            }
            builder.Append(")");

            return builder.ToString();
        }
    }
}
