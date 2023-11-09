#if !UNITY_DISABLE
#if UNITY_EDITOR && NET_4_6
using dotnow.Runtime;
using System;
using System.CodeDom;
using System.Reflection;

namespace dotnow.BindingGenerator.Emit
{
    internal sealed class DirectCallMethodBuilder
    {
        // Private
        private MethodInfo method = null;

        // Public
        public const string stackArg = "stack";
        public const string offsetArg = "offset";

        // Constructor
        public DirectCallMethodBuilder(MethodInfo method)
        {
            this.method = method;
        }

        // Methods
        public CodeMemberMethod BuildMethodDirectCall()
        {
            CodeMemberMethod codeMethod = new CodeMemberMethod();
            codeMethod.Name = (method.DeclaringType.Namespace != null)
                ? method.DeclaringType.Namespace.Replace('.', '_') + "_" + method.DeclaringType.Name + "_" + method.Name
                : method.DeclaringType.Name + "_" + method.Name;
            codeMethod.Attributes = MemberAttributes.Assembly | MemberAttributes.Static | MemberAttributes.Final;

            // Prevent Unity code stripping
            codeMethod.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeTypeReference(typeof(PreserveAttribute))));
            
            // Register binding with dotnow
            codeMethod.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeTypeReference(typeof(CLRMethodDirectCallBindingAttribute)),
                new CodeAttributeArgument(new CodeTypeOfExpression(method.DeclaringType)), new CodeAttributeArgument(new CodeSnippetExpression("\"" + method.Name + "\""))));

            // Stack parameter
            codeMethod.Parameters.Add(new CodeParameterDeclarationExpression(
                new CodeTypeReference(typeof(StackData[])), stackArg));

            // Offset parameter
            codeMethod.Parameters.Add(new CodeParameterDeclarationExpression(
                new CodeTypeReference(typeof(int)), offsetArg));


            // TODO - method body impl
            BuildMethodBody(codeMethod);

            return codeMethod;
        }

        private void BuildMethodBody(CodeMemberMethod codeMethod)
        {
            bool errorGeneratingArgs = false;

            // Check for static
            bool isStatic = method.IsStatic;

            // Generate access expression
            CodeExpression accessExpression = isStatic == false
                ?  BuildMethodArgument(method.DeclaringType, 0)
                : new CodeTypeReferenceExpression(new CodeTypeReference(method.DeclaringType));

            // Build the arguments
            ParameterInfo[] parameter = method.GetParameters();

            // Create array
            CodeExpression[] argExpressions = new CodeExpression[parameter.Length];

            for(int i = 0; i < parameter.Length; i++)
            {
                argExpressions[i] = BuildMethodArgument(parameter[i].ParameterType, i + (isStatic == false ? 1 : 0));

                // Check for error
                if (argExpressions[i] is CodeDefaultValueExpression)
                    errorGeneratingArgs = true;
            }

            // Build the method invoke
            CodeMethodInvokeExpression invokeExpression = new CodeMethodInvokeExpression(
                new CodeMethodReferenceExpression(accessExpression, method.Name),
                argExpressions);

            // Check for error
            if (errorGeneratingArgs == true)
                codeMethod.Comments.Add(new CodeCommentStatement("An error occurred while generating this method: Unknown/unsupported arg type"));

            // Add to method body
            codeMethod.Statements.Add(invokeExpression);
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
            if(type != TypeCode.Object && type != TypeCode.String)
            {
                // Emit start of field access
                CodeFieldReferenceExpression primitiveField = new CodeFieldReferenceExpression(indexExpression, nameof(StackData.value));

                switch(type)
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
    }
}
#endif
#endif