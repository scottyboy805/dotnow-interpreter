using dotnow.CodeGen.Emit;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Reflection;

namespace dotnow.CodeGen
{
    public class PropertyAccessorBindingBuilder : MethodBindingBuilder
    {
        // Private
        private readonly PropertyInfo property = null;

        // Properties
        public bool IsGetter => method == property.GetMethod;
        public bool IsSetter => method == property.SetMethod;

        // Constructor
        public PropertyAccessorBindingBuilder(PropertyInfo property, MethodInfo accessorMethod)
            : base(accessorMethod)
        {
            this.property = property;
        }

        // Methods
        protected override ExpressionSyntax BuildInvoke()
        {
            // Check for getter
            if(IsGetter == true)
            {
                return SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                    method.IsStatic == true
                        ? BindingUtility.BuildTypeReference(method.DeclaringType)
                        : SyntaxFactory.IdentifierName(argVar + "0"),
                    SyntaxFactory.IdentifierName(property.Name));
            }
            // Check for setter
            else if(IsSetter == true)
            {
                return SyntaxFactory.AssignmentExpression(SyntaxKind.SimpleAssignmentExpression,
                    SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                        method.IsStatic == true
                            ? BindingUtility.BuildTypeReference(method.DeclaringType)
                            : SyntaxFactory.IdentifierName(argVar + "0"),
                        SyntaxFactory.IdentifierName(property.Name)),
                    SyntaxFactory.IdentifierName(argVar + (method.IsStatic == true
                        ? "0"
                        : "1")));
            }

            // Fallback but it wont be good
            return base.BuildInvoke();
        }
    }
}
