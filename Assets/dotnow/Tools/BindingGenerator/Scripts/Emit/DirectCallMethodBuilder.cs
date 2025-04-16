#if !UNITY_DISABLE
#if UNITY_EDITOR && NET_4_6
using dotnow.Runtime;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;

namespace dotnow.BindingGenerator.Emit
{
    internal sealed class DirectCallMethodBuilder
    {
        // Private
        private MethodInfo method = null;
        private MethodInfo[] methods = null;

        // Constants
        public const string stackArg = "stack";
        public const string offsetArg = "offset";

        // Constructor
        public DirectCallMethodBuilder(MethodInfo method)
        {
            this.method = method;
        }

        public DirectCallMethodBuilder(MethodInfo[] methods)
        {
            this.methods = methods;
        }

        // Methods
        public CodeTypeMember[] BuildMethodDirectCall()
        {
            if (methods == null)
            {
                return new CodeTypeMember[]
                {
                    BuildSingleMethod(method)
                };
            }
            else
            {
                return methods.Select(BuildSingleMethod).ToArray<CodeTypeMember>();
            }
        }

        private CodeMemberMethod BuildSingleMethod(MethodInfo methodInfo)
        {
            CodeMemberMethod codeMethod = new CodeMemberMethod();
            codeMethod.Name = GenerateMethodName(methodInfo);
            codeMethod.Attributes = MemberAttributes.Assembly | MemberAttributes.Static | MemberAttributes.Final;

            // Prevent Unity code stripping
            codeMethod.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeTypeReference(typeof(PreserveAttribute))));

            // Add CLRMethodDirectCallBindingAttribute
            AddDirectCallBindingAttribute(codeMethod, methodInfo);

            // Add parameters
            codeMethod.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference(typeof(StackData[])), stackArg));
            codeMethod.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference(typeof(int)), offsetArg));

            // Build method body
            try
            {
                BuildMethodBody(codeMethod, methodInfo);
            }
            catch (Exception ex)
            {
                codeMethod.Comments.Add(new CodeCommentStatement($"An error occurred while generating this method: {ex.Message}"));
            }

            return codeMethod;
        }

        private void AddDirectCallBindingAttribute(CodeMemberMethod codeMethod, MethodInfo methodInfo)
        {
            List<CodeAttributeArgument> attributeArgs = new List<CodeAttributeArgument>
            {
                new CodeAttributeArgument(new CodeTypeOfExpression(methodInfo.DeclaringType)),
                new CodeAttributeArgument(new CodePrimitiveExpression(methodInfo.Name)),
                new CodeAttributeArgument(new CodePrimitiveExpression(methodInfo.IsGenericMethod))
            };

            var genericTypes = methodInfo.GetGenericArguments();
            CodeExpression[] genericTypeExpressions = genericTypes.Select(t => new CodeTypeOfExpression(t)).ToArray<CodeExpression>();
            attributeArgs.Add(new CodeAttributeArgument(new CodeArrayCreateExpression(typeof(Type), genericTypeExpressions)));

            var parameterTypes = methodInfo.GetParameters().Select(p => p.ParameterType).ToArray();
            CodeExpression[] parameterTypeExpressions = parameterTypes.Select(t => new CodeTypeOfExpression(t)).ToArray<CodeExpression>();
            attributeArgs.Add(new CodeAttributeArgument(new CodeArrayCreateExpression(typeof(Type), parameterTypeExpressions)));

            codeMethod.CustomAttributes.Add(new CodeAttributeDeclaration(
                new CodeTypeReference(typeof(CLRMethodDirectCallBindingAttribute)),
                attributeArgs.ToArray()));
        }

        private string GenerateMethodName(MethodInfo methodInfo)
        {
            StringBuilder nameBuilder = new StringBuilder();

            if (methodInfo.DeclaringType != null)
            {
                if (methodInfo.DeclaringType.Namespace != null)
                {
                    nameBuilder.Append(methodInfo.DeclaringType.Namespace.Replace('.', '_'));
                    nameBuilder.Append("_");
                }
                nameBuilder.Append(methodInfo.DeclaringType.Name.Replace('.', '_').Replace('<', '_').Replace('>', '_'));
                nameBuilder.Append("_");
            }

            nameBuilder.Append(methodInfo.Name.Replace('.', '_').Replace('<', '_').Replace('>', '_'));

            var parameters = methodInfo.GetParameters();
            if (parameters.Length > 0)
            {
                nameBuilder.Append("_");
                nameBuilder.Append(string.Join("_", parameters.Select(p => GetTypeAlias(p.ParameterType))));
            }

            if (methodInfo.IsGenericMethod)
            {
                var genericArgs = methodInfo.GetGenericArguments();
                nameBuilder.Append("_G_");
                nameBuilder.Append(string.Join("_", genericArgs.Select(GetTypeAlias)));
            }

            return nameBuilder.ToString();
        }

        private string GetTypeAlias(Type type)
        {
            if (type == null) return "Null";
            if (type.IsGenericParameter) return $"T{type.GenericParameterPosition}";
            if (type.IsGenericType)
            {
                var genericTypeDef = type.GetGenericTypeDefinition();
                var genericArgs = type.GetGenericArguments();
                string baseTypeName = genericTypeDef.Name.Split('`')[0];
                return $"{baseTypeName}Of{string.Join("And", genericArgs.Select(GetSimpleTypeName))}";
            }
            return GetSimpleTypeName(type);
        }

        private string GetSimpleTypeName(Type type)
        {
            if (type.IsEnum) return $"Enum{Enum.GetUnderlyingType(type).Name}";
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Boolean: return "Bool";
                case TypeCode.Byte: return "Byte";
                case TypeCode.Char: return "Char";
                case TypeCode.Decimal: return "Decimal";
                case TypeCode.Double: return "Double";
                case TypeCode.Int16: return "Short";
                case TypeCode.Int32: return "Int";
                case TypeCode.Int64: return "Long";
                case TypeCode.SByte: return "SByte";
                case TypeCode.Single: return "Float";
                case TypeCode.String: return "String";
                case TypeCode.UInt16: return "UShort";
                case TypeCode.UInt32: return "UInt";
                case TypeCode.UInt64: return "ULong";
                default: return type.Name;
            }
        }

        private void BuildMethodBody(CodeMemberMethod codeMethod, MethodInfo methodInfo)
        {
            bool isStatic = methodInfo.IsStatic;
            CodeExpression accessExpression = isStatic
                ? new CodeTypeReferenceExpression(methodInfo.DeclaringType)
                : BuildMethodArgument(methodInfo.DeclaringType, 0);

            ParameterInfo[] parameters = methodInfo.GetParameters();
            CodeExpression[] argExpressions = new CodeExpression[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                argExpressions[i] = BuildMethodArgument(parameters[i].ParameterType, i + (isStatic ? 0 : 1));
            }

            CodeMethodInvokeExpression invokeExpression;
            if (methodInfo.IsGenericMethod)
            {
                var genericArgs = methodInfo.GetGenericArguments();
                var genericArgReferences = genericArgs.Select(t => new CodeTypeReference(t)).ToArray();
                var methodReference = new CodeMethodReferenceExpression(accessExpression, methodInfo.Name, genericArgReferences);
                invokeExpression = new CodeMethodInvokeExpression(methodReference, argExpressions);
            }
            else
            {
                invokeExpression = new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(accessExpression, methodInfo.Name), argExpressions);
            }

            if (methodInfo.ReturnType != typeof(void))
            {
                CodeMethodInvokeExpression allocExpression;
                //CodeExpression typeofExpression = new CodeTypeOfExpression(methodInfo.ReturnType);

                if (methodInfo.ReturnType.IsValueType == true && methodInfo.ReturnType.IsPrimitive == false)
                {
                    // Use AllocRefBoxed for non-primitive value types
                    allocExpression = new CodeMethodInvokeExpression(
                        new CodeTypeReferenceExpression(typeof(StackData)),
                        "AllocRefBoxed",
                        new CodeArgumentReferenceExpression("ref stack[offset]"),
                        invokeExpression
                        );
                }
                else if (methodInfo.ReturnType.IsClass || methodInfo.ReturnType.IsInterface)
                {
                    // Use AllocRef for reference types
                    allocExpression = new CodeMethodInvokeExpression(
                        new CodeTypeReferenceExpression(typeof(StackData)),
                        "AllocRef",
                        new CodeArgumentReferenceExpression("ref stack[offset]"),
                        invokeExpression
                        );
                }
                else
                {
                    // Use specific Alloc methods for primitive types
                    string allocMethodName = GetAllocMethodName(methodInfo.ReturnType);
                    allocExpression = new CodeMethodInvokeExpression(
                        new CodeTypeReferenceExpression(typeof(StackData)),
                        allocMethodName,
                        new CodeArgumentReferenceExpression("ref stack[offset]"),
                        invokeExpression
                        );
                }

                codeMethod.Statements.Add(allocExpression);
            }
            else
            {
                codeMethod.Statements.Add(invokeExpression);
            }

            // Handle ref and out parameters
            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i].ParameterType.IsByRef)
                {
                    Type elementType = parameters[i].ParameterType.GetElementType();
                    CodeMethodInvokeExpression allocExpression;

                    if (elementType.IsValueType == true && elementType.IsPrimitive == false)
                    {
                        allocExpression = new CodeMethodInvokeExpression(
                            new CodeTypeReferenceExpression(typeof(StackData)),
                            "AllocRefBoxed",
                            new CodeArgumentReferenceExpression($"ref stack[offset + {i + (isStatic ? 0 : 1)}]"),
                            argExpressions[i]
                            );
                    }
                    else if (elementType != null && (elementType.IsClass || elementType.IsInterface))
                    {
                        allocExpression = new CodeMethodInvokeExpression(
                            new CodeTypeReferenceExpression(typeof(StackData)),
                            "AllocRef",
                            new CodeArgumentReferenceExpression($"ref stack[offset + {i + (isStatic ? 0 : 1)}]"),
                            argExpressions[i]
                            );
                    }
                    else
                    {
                        string allocMethodName = GetAllocMethodName(elementType);
                        allocExpression = new CodeMethodInvokeExpression(
                            new CodeTypeReferenceExpression(typeof(StackData)),
                            allocMethodName,
                            new CodeArgumentReferenceExpression($"ref stack[offset + {i + (isStatic ? 0 : 1)}]"),
                            argExpressions[i]
                            );
                    }

                    codeMethod.Statements.Add(allocExpression);
                }
            }
        }

        private string GetAllocMethodName(Type type)
        {
            if (type == typeof(bool)) return "Alloc";
            if (type == typeof(sbyte)) return "Alloc";
            if (type == typeof(byte)) return "Alloc";
            if (type == typeof(short)) return "Alloc";
            if (type == typeof(ushort)) return "Alloc";
            if (type == typeof(int)) return "Alloc";
            if (type == typeof(uint)) return "Alloc";
            if (type == typeof(long)) return "Alloc";
            if (type == typeof(ulong)) return "Alloc";
            if (type == typeof(float)) return "Alloc";
            if (type == typeof(double)) return "Alloc";
            if (type.IsEnum) return "Alloc";

            // For any other type, use AllocTyped
            return "AllocTyped";
        }

        private CodeExpression BuildMethodArgument(Type argType, int offset)
        {
            // Emit array index expression
            CodeIndexerExpression indexExpression = new CodeIndexerExpression(
                new CodeVariableReferenceExpression(stackArg),
                new CodeBinaryOperatorExpression(new CodeVariableReferenceExpression(offsetArg), CodeBinaryOperatorType.Add, new CodeSnippetExpression(offset.ToString())));

            // Get type code
            TypeCode type = Type.GetTypeCode(argType);

            // Check type
            if (type != TypeCode.Object && type != TypeCode.String && !argType.IsEnum)
            {
                // Emit start of field access
                CodeFieldReferenceExpression primitiveField = new CodeFieldReferenceExpression(indexExpression, nameof(StackData.value));

                switch (type)
                {
                    default:
                        {
                            return new CodeDefaultValueExpression(new CodeTypeReference(argType));
                        }

                    case TypeCode.Byte:
                    case TypeCode.SByte:
                        {
                            // Check if case is needed
                            CodeExpression finalExpression = new CodeFieldReferenceExpression(primitiveField, nameof(StackData.Primitive.Int8));

                            // Insert cast conversion
                            if (type == TypeCode.Byte)
                                finalExpression = new CodeCastExpression(new CodeTypeReference(typeof(byte)), finalExpression);

                            // Get expression
                            return finalExpression;
                        }

                    case TypeCode.Int16:
                    case TypeCode.UInt16:
                        {
                            // Check if case is needed
                            CodeExpression finalExpression = new CodeFieldReferenceExpression(primitiveField, nameof(StackData.Primitive.Int16));

                            // Insert cast conversion
                            if (type == TypeCode.UInt16)
                                finalExpression = new CodeCastExpression(new CodeTypeReference(typeof(ushort)), finalExpression);

                            // Get expression
                            return finalExpression;
                        }

                    case TypeCode.Int32:
                    case TypeCode.UInt32:
                        {
                            // Check if case is needed
                            CodeExpression finalExpression = new CodeFieldReferenceExpression(primitiveField, nameof(StackData.Primitive.Int32));

                            // Insert cast conversion
                            if (type == TypeCode.UInt32)
                                finalExpression = new CodeCastExpression(new CodeTypeReference(typeof(uint)), finalExpression);

                            // Get expression
                            return finalExpression;
                        }

                    case TypeCode.Int64:
                    case TypeCode.UInt64:
                        {
                            // Check if case is needed
                            CodeExpression finalExpression = new CodeFieldReferenceExpression(primitiveField, nameof(StackData.Primitive.Int64));

                            // Insert cast conversion
                            if (type == TypeCode.UInt64)
                                finalExpression = new CodeCastExpression(new CodeTypeReference(typeof(ulong)), finalExpression);

                            // Get expression
                            return finalExpression;
                        }

                    case TypeCode.Single:
                        {
                            // Check if case is needed
                            return new CodeFieldReferenceExpression(primitiveField, nameof(StackData.Primitive.Single));
                        }

                    case TypeCode.Double:
                        {
                            // Check if case is needed
                            return new CodeFieldReferenceExpression(primitiveField, nameof(StackData.Primitive.Double));
                        }
                }
            }
            else
            {
                // Return ref value with cast
                return new CodeCastExpression(new CodeTypeReference(argType),
                    new CodeFieldReferenceExpression(indexExpression, nameof(StackData.refValue)));
            }
        }

        private string GetEnumUnderlyingTypeField(Type enumType)
        {
            Type underlyingType = Enum.GetUnderlyingType(enumType);
            return GetPrimitiveFieldName(Type.GetTypeCode(underlyingType));
        }

        private string GetPrimitiveFieldName(TypeCode typeCode)
        {
            switch (typeCode)
            {
                case TypeCode.Boolean:
                case TypeCode.Byte:
                case TypeCode.SByte:
                    return "Int8";
                case TypeCode.Char:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                    return "Int16";
                case TypeCode.Int32:
                case TypeCode.UInt32:
                    return "Int32";
                case TypeCode.Int64:
                case TypeCode.UInt64:
                    return "Int64";
                case TypeCode.Single:
                    return "Single";
                case TypeCode.Double:
                    return "Double";
                default:
                    throw new ArgumentException($"Unsupported primitive type: {typeCode}");
            }
        }
    }
}
#endif
#endif
