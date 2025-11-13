//using System;
//using System.Collections.Generic;
//using System.Reflection;
//using System.Linq;
//using System.Text;
//using dotnow.Interop;
//using System.CodeDom;

//namespace dotnow.CodeGen.Emit
//{
//    internal sealed class DirectCallMethodBuilder
//    {
//        // Private
//        private MethodInfo method = null;

//        // Constants
//        public const string contextArg = "context";
//        public const string instanceVar = "instance";

//        // Constructor
//        public DirectCallMethodBuilder(MethodInfo method)
//        {
//            this.method = method;
//        }

//        // Methods
//        public CodeMemberMethod BuildMember()
//        {
//            return BuildMethod();
//        }

//        private CodeMemberMethod BuildMethod()
//        {
//            CodeMemberMethod codeMethod = new CodeMemberMethod();
//            codeMethod.Name = GenerateMethodName(method);
//            codeMethod.Attributes = MemberAttributes.Assembly | MemberAttributes.Static | MemberAttributes.Final;

//            // Prevent Unity code stripping
//            codeMethod.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeTypeReference(typeof(PreserveAttribute))));

//            // Add [Preserve]
//            AddPreserveAttribute(codeMethod);

//            // Add [CLRMethodBinding]            
//            AddMethodBindingAttribute(codeMethod, method);

//            // Add parameters
//            codeMethod.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference(typeof(StackContext)), contextArg));

//            // Build method body
//            try
//            {
//                BuildMethodBody(codeMethod, method);
//            }
//            catch (Exception ex)
//            {
//                codeMethod.Comments.Add(new CodeCommentStatement($"An error occurred while generating this method: {ex.Message}"));
//            }

//            return codeMethod;
//        }

//        private void AddPreserveAttribute(CodeMemberMethod codeMethod)
//        {
//            codeMethod.CustomAttributes.Add(new CodeAttributeDeclaration(
//                new CodeTypeReference(typeof(PreserveAttribute))));
//        }

//        private void AddMethodBindingAttribute(CodeMemberMethod codeMethod, MethodInfo methodInfo)
//        {
//            List<CodeAttributeArgument> attributeArgs = new List<CodeAttributeArgument>
//            {
//                new CodeAttributeArgument(new CodeTypeOfExpression(methodInfo.DeclaringType)),
//                new CodeAttributeArgument(new CodePrimitiveExpression(methodInfo.Name)),
//                new CodeAttributeArgument(new CodePrimitiveExpression(methodInfo.IsGenericMethod))
//            };

//            var genericTypes = methodInfo.GetGenericArguments();
//            CodeExpression[] genericTypeExpressions = genericTypes.Select(t => new CodeTypeOfExpression(t)).ToArray<CodeExpression>();
//            attributeArgs.Add(new CodeAttributeArgument(new CodeArrayCreateExpression(typeof(Type), genericTypeExpressions)));

//            var parameterTypes = methodInfo.GetParameters().Select(p => p.ParameterType).ToArray();
//            CodeExpression[] parameterTypeExpressions = parameterTypes.Select(t => new CodeTypeOfExpression(t)).ToArray<CodeExpression>();
//            attributeArgs.Add(new CodeAttributeArgument(new CodeArrayCreateExpression(typeof(Type), parameterTypeExpressions)));

//            codeMethod.CustomAttributes.Add(new CodeAttributeDeclaration(
//                new CodeTypeReference(typeof(CLRMethodBindingAttribute)),
//                attributeArgs.ToArray()));
//        }

//        private string GenerateMethodName(MethodInfo methodInfo)
//        {
//            StringBuilder nameBuilder = new StringBuilder();

//            if (methodInfo.DeclaringType != null)
//            {
//                if (methodInfo.DeclaringType.Namespace != null)
//                {
//                    nameBuilder.Append(methodInfo.DeclaringType.Namespace.Replace('.', '_'));
//                    nameBuilder.Append("_");
//                }
//                nameBuilder.Append(methodInfo.DeclaringType.Name.Replace('.', '_').Replace('<', '_').Replace('>', '_'));
//                nameBuilder.Append("_");
//            }

//            nameBuilder.Append(methodInfo.Name.Replace('.', '_').Replace('<', '_').Replace('>', '_'));

//            var parameters = methodInfo.GetParameters();
//            if (parameters.Length > 0)
//            {
//                nameBuilder.Append("_");
//                nameBuilder.Append(string.Join("_", parameters.Select(p => GetTypeAlias(p.ParameterType))));
//            }

//            if (methodInfo.IsGenericMethod)
//            {
//                var genericArgs = methodInfo.GetGenericArguments();
//                nameBuilder.Append("_G_");
//                nameBuilder.Append(string.Join("_", genericArgs.Select(GetTypeAlias)));
//            }

//            return nameBuilder.ToString();
//        }

//        private string GetTypeAlias(Type type)
//        {
//            if (type == null) return "Null";
//            if (type.IsGenericParameter) return $"T{type.GenericParameterPosition}";
//            if (type.IsGenericType)
//            {
//                var genericTypeDef = type.GetGenericTypeDefinition();
//                var genericArgs = type.GetGenericArguments();
//                string baseTypeName = genericTypeDef.Name.Split('`')[0];
//                return $"{baseTypeName}Of{string.Join("And", genericArgs.Select(GetSimpleTypeName))}";
//            }
//            return GetSimpleTypeName(type);
//        }

//        private string GetSimpleTypeName(Type type)
//        {
//            if (type.IsEnum) return $"Enum{Enum.GetUnderlyingType(type).Name}";
//            switch (Type.GetTypeCode(type))
//            {
//                case TypeCode.Boolean: return "Bool";
//                case TypeCode.Byte: return "Byte";
//                case TypeCode.Char: return "Char";
//                case TypeCode.Decimal: return "Decimal";
//                case TypeCode.Double: return "Double";
//                case TypeCode.Int16: return "Short";
//                case TypeCode.Int32: return "Int";
//                case TypeCode.Int64: return "Long";
//                case TypeCode.SByte: return "SByte";
//                case TypeCode.Single: return "Float";
//                case TypeCode.String: return "String";
//                case TypeCode.UInt16: return "UShort";
//                case TypeCode.UInt32: return "UInt";
//                case TypeCode.UInt64: return "ULong";
//                default: return type.Name;
//            }
//        }

//        private void BuildMethodBody(CodeMemberMethod codeMethod, MethodInfo methodInfo)
//        {
//            // Get type
//            Type objectType = methodInfo.DeclaringType;

//            // Check for instance
//            bool hasInstance = methodInfo.IsStatic == false;

//            // Check for instance
//            if(hasInstance == true)
//            {
//            }
//            else
//            {

//            }
//        }

//        private CodeExpression ReadStackContext(Type type, int offset)
//        {
//            // Check for reference type
//            if(type.IsClass == true)
//            {
//                return new CodeMethodInvokeExpression(
//                    new CodeVariableReferenceExpression(contextArg),
//                    nameof(StackContext.ReadArgObject),)
//            }
//        }
//    }
//}