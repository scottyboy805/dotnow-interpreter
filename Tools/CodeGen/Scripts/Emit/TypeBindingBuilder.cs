using dotnow.CodeGen.Emit;
using dotnow.Interop;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace dotnow.BindingGenerator.Emit
{
    internal class TypeBindingBuilder
    {
        // Protected
        protected readonly Type type = null;

        // Constructor
        public TypeBindingBuilder(Type type)
        {
            this.type = type;
        }

        // Methods
        public TypeDeclarationSyntax BuildMember()
        {
            // Store all members
            List<MemberDeclarationSyntax> members = new();

            // Build all methods
            foreach(MethodInfo method in CollectMethod())
            {
                // Create the method builder
                MethodBindingBuilder methodBuilder = new(method);

                // Add to members
                members.Add(methodBuilder.BuildMember());
            }

            return SyntaxFactory.TypeDeclaration(SyntaxKind.ClassDeclaration,
                // Method custom attributes
                BuildTypeCustomAttributes(),
                // Method attributes
                SyntaxFactory.TokenList(
                    SyntaxFactory.Token(SyntaxKind.PublicKeyword),
                    SyntaxFactory.Token(SyntaxKind.SealedKeyword)),
                SyntaxFactory.Token(SyntaxKind.ClassKeyword),
                SyntaxFactory.Identifier(GetTypeFlattenedName()),
                default,
                default,
                default,
                SyntaxFactory.Token(SyntaxKind.OpenBraceToken),
                // Members
                SyntaxFactory.List(members),
                SyntaxFactory.Token(SyntaxKind.CloseBraceToken),
                default);
        }

        protected virtual SyntaxList<AttributeListSyntax> BuildTypeCustomAttributes()
        {
            return SyntaxFactory.SingletonList(
                SyntaxFactory.AttributeList(
                    SyntaxFactory.SingletonSeparatedList(
                        // [Preserve]
                        BuildTypeCustomPreserveAttribute())));
        }

        protected virtual AttributeSyntax BuildTypeCustomPreserveAttribute()
        {
            return SyntaxFactory.Attribute(
                SyntaxFactory.IdentifierName(nameof(PreserveAttribute)));
        }

        private IEnumerable<MethodInfo> CollectMethod()
        {
            return type.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .Where(m => m.DeclaringType != typeof(object) && m.DeclaringType != typeof(MarshalByRefObject))
                .Where(m => !m.IsSpecialName)
                .Where(m => !m.ContainsGenericParameters)
                .Where(m => !m.IsConstructedGenericMethod)
                .Where(m => !m.IsAbstract && !m.IsConstructor);
        }

        public string GetTypeFlattenedName()
        {
            return string.IsNullOrEmpty(type.Namespace)
                ? type.Name
                : string.Concat(type.Namespace.Replace('.', '_').Replace('<', '_').Replace('>', '_'), "_", type.Name);
        }
    }
}