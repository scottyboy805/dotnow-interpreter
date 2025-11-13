using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using dotnow.Interop;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;

namespace dotnow.CodeGen.Emit
{
    internal class MethodBindingBuilder
    {
        // Protected
        protected readonly MethodInfo method = null;

        // Constants
        protected const string contextArg = "context";
        protected const string instanceVar = "instance";
        protected const string argVar = "arg";
        protected const string returnVar = "result";

        // Constructor
        public MethodBindingBuilder(MethodInfo method)
        {
            this.method = method;
        }

        // Methods
        public MethodDeclarationSyntax BuildMember()
        {
            return BuildMethod();
        }

        private MethodDeclarationSyntax BuildMethod()
        {
            // Build the body
            BlockSyntax body = BuildMethodBody();

            // Build the method definition
            MethodDeclarationSyntax codeMethod = SyntaxFactory.MethodDeclaration(
                // Method custom attributes
                BuildMethodCustomAttributes(),
                // Method attributes
                SyntaxFactory.TokenList(
                    SyntaxFactory.Token(SyntaxKind.PublicKeyword),
                    SyntaxFactory.Token(SyntaxKind.StaticKeyword)),
                // Method return
                SyntaxFactory.PredefinedType(
                    SyntaxFactory.Token(SyntaxKind.VoidKeyword)),
                null,
                // Method name
                SyntaxFactory.Identifier(GenerateMethodName(method)),
                // Method type parameters
                null,
                // Method parameters
                BuildMethodParameters(),
                // Method generic constraints
                default,
                // Method body
                body,
                // Semi colon
                default(SyntaxToken));

            // Get the method
            return codeMethod
                .WithLeadingTrivia(
                    SyntaxFactory.Comment("// Binding generated from method: " + method.ToString()));
        }

        protected virtual SyntaxList<AttributeListSyntax> BuildMethodCustomAttributes()
        {
            return SyntaxFactory.SingletonList(
                SyntaxFactory.AttributeList(
                    SyntaxFactory.SeparatedList(new[] {
                        // [Preserve]
                        BuildMethodCustomPreserveAttribute(),
                        // [CLRMethodBinding]
                        BuildMethodCustomBindingAttribute() })));
        }

        protected virtual AttributeSyntax BuildMethodCustomPreserveAttribute()
        {
            return SyntaxFactory.Attribute(
                SyntaxFactory.IdentifierName(nameof(PreserveAttribute)));
        }

        protected virtual AttributeSyntax BuildMethodCustomBindingAttribute()
        {
            IEnumerable<ExpressionSyntax> methodExpressions = new ExpressionSyntax[]
            { 
                // Get declaring type
                SyntaxFactory.TypeOfExpression(
                    BindingUtility.BuildTypeReference(method.DeclaringType)),

                // Get method name
                SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression,
                    SyntaxFactory.Literal(method.Name)),
            };

            // Get argument types
            IEnumerable<ExpressionSyntax> argumentTypes = method.GetParameters().Select(p =>
                BindingUtility.BuildTypeReference(p.ParameterType));

            // Build the attribute
            return SyntaxFactory.Attribute(
                SyntaxFactory.IdentifierName(nameof(CLRMethodBindingAttribute)),
                SyntaxFactory.AttributeArgumentList(
                    SyntaxFactory.SeparatedList(
                        Enumerable.Concat(methodExpressions, argumentTypes)
                            .Select(e => SyntaxFactory.AttributeArgument(e)))));
        }

        protected virtual ParameterListSyntax BuildMethodParameters()
        {
            return SyntaxFactory.ParameterList(
                SyntaxFactory.SeparatedList(new ParameterSyntax[]
                {
                    // Stack context parameter
                    SyntaxFactory.Parameter(
                        default, default, BindingUtility.BuildTypeReference(typeof(StackContext)),
                        SyntaxFactory.Identifier(contextArg), null)
                }));
        }

        protected virtual BlockSyntax BuildMethodBody()
        {
            return SyntaxFactory.Block(
                BuildMethodStatements());
        }

        protected virtual IEnumerable<StatementSyntax> BuildMethodStatements()
        {
            List<StatementSyntax> statements = new();

            int arg = 0;

            // Read instance
            if(method.IsStatic == false)
            {
                statements.Add(SyntaxFactory.LocalDeclarationStatement(
                    SyntaxFactory.VariableDeclaration(
                        BindingUtility.BuildTypeReference(method.DeclaringType),
                    SyntaxFactory.SingletonSeparatedList(
                        SyntaxFactory.VariableDeclarator(
                            SyntaxFactory.Identifier(argVar + arg++),
                            null,
                            SyntaxFactory.EqualsValueClause(
                                BuildReadArgumentStackContextExpression(method.DeclaringType, 0)))))));
            }

            // Read arguments
            foreach(ParameterInfo parameter in method.GetParameters())
            {
                statements.Add(SyntaxFactory.LocalDeclarationStatement(
                    SyntaxFactory.VariableDeclaration(
                        BindingUtility.BuildTypeReference(parameter.ParameterType),
                    SyntaxFactory.SingletonSeparatedList(
                        SyntaxFactory.VariableDeclarator(
                            SyntaxFactory.Identifier(argVar + arg++),
                            null,
                            SyntaxFactory.EqualsValueClause(
                                BuildReadArgumentStackContextExpression(parameter.ParameterType, arg - 1)))))));
            }

            // Call the static method
            InvocationExpressionSyntax invoke = SyntaxFactory.InvocationExpression(
                // Type.method
                SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                    method.IsStatic == true
                        ? BindingUtility.BuildTypeReference(method.DeclaringType)
                        : SyntaxFactory.IdentifierName(argVar + "0"),
                    SyntaxFactory.IdentifierName(method.Name)),
                // (...)
                BuildInvokeArgumentList());

            // Check for return value
            if(method.ReturnType != null && method.ReturnType != typeof(void))
            {
                // Assign return value to variable
                statements.Add(SyntaxFactory.LocalDeclarationStatement(
                    SyntaxFactory.VariableDeclaration(
                        BindingUtility.BuildTypeReference(method.ReturnType),
                    SyntaxFactory.SingletonSeparatedList(
                        SyntaxFactory.VariableDeclarator(
                            SyntaxFactory.Identifier(returnVar),
                            null,
                            SyntaxFactory.EqualsValueClause(
                                invoke))))));
            }
            else
            {
                // No return - just call the method direct
                statements.Add(SyntaxFactory.ExpressionStatement(invoke));
            }

            // Todo - handle ref/out arguments
            int paramIndex = 0;
            foreach (ParameterInfo parameter in method.GetParameters())
            {
                // Check for by ref or out
                if (parameter.IsOut == true || parameter.ParameterType.IsByRef == true)
                {
                    // Add the call to write arg value back to stack
                    statements.Add(SyntaxFactory.ExpressionStatement(
                        BuildWriteArgumentStackContextExpression(parameter.ParameterType,
                            method.IsStatic == false
                                ? paramIndex + 1 :
                                paramIndex)));
                }

                paramIndex++;
            }

            // Check for return
            if (method.ReturnType != null && method.ReturnType != typeof(void))
            {
                // Write return value back to stack
                statements.Add(SyntaxFactory.ExpressionStatement(BuildWriteReturnStackContextExpression(method.ReturnType)));
            }

            return statements;
        }

        protected virtual ArgumentListSyntax BuildInvokeArgumentList()
        {
            return SyntaxFactory.ArgumentList(
                SyntaxFactory.SeparatedList(
                    method.GetParameters().Select((p, i) => BuildInvokeArgument(p, i))));
        }

        protected virtual ArgumentSyntax BuildInvokeArgument(ParameterInfo parameter, int index)
        {
            SyntaxToken special = default;

            // Check for out
            if(parameter.IsOut == true)
                special = SyntaxFactory.Token(SyntaxKind.OutKeyword);

            // Check for ref
            if (parameter.ParameterType.IsByRef == true)
                special = SyntaxFactory.Token(SyntaxKind.RefKeyword);

            // Build the argument
            return SyntaxFactory.Argument(
                null,
                // ref/out
                special,
                // argX
                SyntaxFactory.IdentifierName(method.IsStatic == false
                    ? argVar + (index + 1)
                    : argVar + index));
        }

        protected virtual ExpressionSyntax BuildReadArgumentStackContextExpression(Type parameterType, int stackOffset)
        {
            // Get the name of the method to call
            string contextMethodName = parameterType.IsClass == true
                ? nameof(StackContext.ReadArgObject)
                : nameof(StackContext.ReadArgValueType);

            // Read a reference type
            return SyntaxFactory.InvocationExpression(
                // context.ReadArgObject<type>
                SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                    // context
                    SyntaxFactory.IdentifierName(contextArg),
                    // ReadArgObject<type>
                    SyntaxFactory.GenericName(
                        SyntaxFactory.Identifier(contextMethodName),
                        // <type>
                        SyntaxFactory.TypeArgumentList(
                        SyntaxFactory.SingletonSeparatedList(
                            BindingUtility.BuildTypeReference(parameterType))))),
                // (stackOffset)
                SyntaxFactory.ArgumentList(
                    SyntaxFactory.SingletonSeparatedList(
                        // stackOffset
                        SyntaxFactory.Argument(
                            SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression,
                                SyntaxFactory.Literal(stackOffset))))));
        }

        protected virtual ExpressionSyntax BuildWriteArgumentStackContextExpression(Type parameterType, int stackOffset)
        {
            // Get the name of the method to call
            string contextMethodName = parameterType.IsClass == true
                ? nameof(StackContext.WriteArgObject)
                : nameof(StackContext.WriteArgValueType);

            // Read a reference type
            return SyntaxFactory.InvocationExpression(
                // context.ReadArgObject<type>
                SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                    // context
                    SyntaxFactory.IdentifierName(contextArg),
                    // ReadArgObject<type>
                    SyntaxFactory.GenericName(
                        SyntaxFactory.Identifier(contextMethodName),
                        // <type>
                        SyntaxFactory.TypeArgumentList(
                        SyntaxFactory.SingletonSeparatedList(
                            BindingUtility.BuildTypeReference(parameterType))))),
                // (stackOffset)
                SyntaxFactory.ArgumentList(
                    SyntaxFactory.SingletonSeparatedList(
                        // stackOffset
                        SyntaxFactory.Argument(
                            SyntaxFactory.IdentifierName(argVar + stackOffset)))));
        }

        protected virtual ExpressionSyntax BuildWriteReturnStackContextExpression(Type returnType)
        {
            // Get the name of the method to call
            string contextMethodName = returnType.IsClass == true
                ? nameof(StackContext.ReturnObject)
                : nameof(StackContext.ReturnValueType);

            // Read a reference type
            return SyntaxFactory.InvocationExpression(
                // context.ReadArgObject<type>
                SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                    // context
                    SyntaxFactory.IdentifierName(contextArg),
                    // ReadArgObject<type>
                    SyntaxFactory.GenericName(
                        SyntaxFactory.Identifier(contextMethodName),
                        // <type>
                        SyntaxFactory.TypeArgumentList(
                        SyntaxFactory.SingletonSeparatedList(
                            BindingUtility.BuildTypeReference(returnType))))),
                // (stackOffset)
                SyntaxFactory.ArgumentList(
                    SyntaxFactory.SingletonSeparatedList(
                        // stackOffset
                        SyntaxFactory.Argument(
                            SyntaxFactory.IdentifierName(returnVar)))));
        }

        //private void AddPreserveAttribute(CodeMemberMethod codeMethod)
        //{
        //    codeMethod.CustomAttributes.Add(new CodeAttributeDeclaration(
        //        new CodeTypeReference(typeof(PreserveAttribute))));
        //}

        //private void AddMethodBindingAttribute(CodeMemberMethod codeMethod, MethodInfo methodInfo)
        //{
        //    List<CodeAttributeArgument> attributeArgs = new List<CodeAttributeArgument>
        //    {
        //        new CodeAttributeArgument(new CodeTypeOfExpression(methodInfo.DeclaringType)),
        //        new CodeAttributeArgument(new CodePrimitiveExpression(methodInfo.Name)),
        //        new CodeAttributeArgument(new CodePrimitiveExpression(methodInfo.IsGenericMethod))
        //    };

        //    var genericTypes = methodInfo.GetGenericArguments();
        //    CodeExpression[] genericTypeExpressions = genericTypes.Select(t => new CodeTypeOfExpression(t)).ToArray<CodeExpression>();
        //    attributeArgs.Add(new CodeAttributeArgument(new CodeArrayCreateExpression(typeof(Type), genericTypeExpressions)));

        //    var parameterTypes = methodInfo.GetParameters().Select(p => p.ParameterType).ToArray();
        //    CodeExpression[] parameterTypeExpressions = parameterTypes.Select(t => new CodeTypeOfExpression(t)).ToArray<CodeExpression>();
        //    attributeArgs.Add(new CodeAttributeArgument(new CodeArrayCreateExpression(typeof(Type), parameterTypeExpressions)));

        //    codeMethod.CustomAttributes.Add(new CodeAttributeDeclaration(
        //        new CodeTypeReference(typeof(CLRMethodBindingAttribute)),
        //        attributeArgs.ToArray()));
        //}

        private string GenerateMethodName(MethodInfo methodInfo)
        {
            StringBuilder nameBuilder = new StringBuilder();

            if (methodInfo.DeclaringType != null)
            {
                if (methodInfo.DeclaringType.Namespace != null)
                {
                    nameBuilder.Append(BindingUtility.EscapeType(methodInfo.DeclaringType).Namespace.Replace('.', '_'));
                    nameBuilder.Append("_");
                }
                nameBuilder.Append(BindingUtility.EscapeType(methodInfo.DeclaringType).Name.Replace('.', '_').Replace('<', '_').Replace('>', '_'));
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
            type = BindingUtility.EscapeType(type);

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
            type = BindingUtility.EscapeType(type);

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

        //private void BuildMethodBody(CodeMemberMethod codeMethod, MethodInfo methodInfo)
        //{
        //    // Get type
        //    Type objectType = methodInfo.DeclaringType;

        //    // Check for instance
        //    bool hasInstance = methodInfo.IsStatic == false;

        //    // Check for instance
        //    if (hasInstance == true)
        //    {
        //    }
        //    else
        //    {

        //    }
        //}

        //private CodeExpression ReadStackContext(Type type, int offset)
        //{
        //    // Check for reference type
        //    if (type.IsClass == true)
        //    {
        //        return new CodeMethodInvokeExpression(
        //            new CodeVariableReferenceExpression(contextArg),
        //            nameof(StackContext.ReadArgObject),)
        //    }
        //}
    }
}