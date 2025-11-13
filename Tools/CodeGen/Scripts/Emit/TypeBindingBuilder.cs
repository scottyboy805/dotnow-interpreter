using dotnow.CodeGen.Emit;
using dotnow.Interop;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace dotnow.CodeGen.Emit
{
    public class TypeBindingBuilder
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

            // Build all property accessors
            foreach((PropertyInfo, MethodInfo) accessor in CollectPropertyAccessors())
            {
                // Create the method builder
                PropertyAccessorBindingBuilder accessorBuilder = new(accessor.Item1, accessor.Item2);

                // Add to members
                members.Add(accessorBuilder.BuildMember());
            }

            // Build all methods
            foreach(MethodInfo method in CollectMethods())
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
                SyntaxFactory.ParseName(typeof(PreserveAttribute).FullName));
        }

        private IEnumerable<(PropertyInfo, MethodInfo)> CollectPropertyAccessors()
        {
            return type.GetProperties(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .SelectMany(p =>
                    new[] { p.GetMethod, p.SetMethod }
                        .Where(m => m != null && IsBindableMethod(m!, true))
                        .Select(m => (Property: p, Accessor: m!)));
        }

        private IEnumerable<MethodInfo> CollectMethods()
        {
            return type.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .Where(m => IsBindableMethod(m, false));
        }

        private bool IsBindableMethod(MethodInfo m, bool ignoreSpecialName)
        {
            return m.DeclaringType != typeof(object) && m.DeclaringType != typeof(MarshalByRefObject)
                && (ignoreSpecialName == true || !m.IsSpecialName)
                && !m.ContainsGenericParameters
                && !m.IsConstructedGenericMethod
                && !m.IsAbstract && !m.IsConstructor
                && !m.IsDefined(typeof(ObsoleteAttribute), false)
                && !BindingUtility.ContainsRefStructType(m);
        }

        public string GetTypeFlattenedName()
        {
            return string.IsNullOrEmpty(type.Namespace)
                ? type.Name
                : string.Concat(type.Namespace.Replace('.', '_').Replace('<', '_').Replace('>', '_'), "_", type.Name);
        }
    }
}