using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Linq;
using System.Reflection;
using System.Text;

namespace dotnow.CodeGen
{
    public static class BindingUtility
    {
        // Methods
        public static TypeSyntax BuildTypeReference(Type type)
        {
            // Check for by ref
            if (type.IsByRef == true)
                type = type.GetElementType();

            if (type.IsGenericType)
            {
                // Example: System.ReadOnlySpan<Vector3>
                var genericTypeDef = type.GetGenericTypeDefinition();
                var typeName = genericTypeDef.FullName!;

                // Trim the `1 or `2 arity markers
                var backtickIndex = typeName.IndexOf('`');
                if (backtickIndex > 0)
                    typeName = typeName.Substring(0, backtickIndex);

                // Use the namespace + generic type name
                var genericName = SyntaxFactory.GenericName(
                    SyntaxFactory.Identifier(typeName.Split('.').Last()))
                    .WithTypeArgumentList(
                        SyntaxFactory.TypeArgumentList(
                            SyntaxFactory.SeparatedList(type.GetGenericArguments()
                                .Select(t => BuildTypeReference(t)))));

                // Prepend namespace (e.g., System.ReadOnlySpan<T>)
                var namespaceName = string.Join(".",
                    typeName.Split('.').Reverse().Skip(1).Reverse());

                return namespaceName.Length == 0
                    ? (TypeSyntax)genericName
                    : SyntaxFactory.QualifiedName(SyntaxFactory.ParseName(namespaceName), genericName);
            }
            else if (type.IsArray)
            {
                return SyntaxFactory.ArrayType(BuildTypeReference(type.GetElementType()!))
                    .WithRankSpecifiers(
                        SyntaxFactory.SingletonList(SyntaxFactory.ArrayRankSpecifier(
                            SyntaxFactory.SingletonSeparatedList<ExpressionSyntax>(
                                SyntaxFactory.OmittedArraySizeExpression()))));
            }
            else if (type.IsPrimitive || type == typeof(string) || type == typeof(object))
            {
                // Handle common predefined types
                return type switch
                {
                    var t when t == typeof(void) => SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.VoidKeyword)),
                    var t when t == typeof(bool) => SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.BoolKeyword)),
                    var t when t == typeof(byte) => SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.ByteKeyword)),
                    var t when t == typeof(sbyte) => SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.SByteKeyword)),
                    var t when t == typeof(short) => SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.ShortKeyword)),
                    var t when t == typeof(ushort) => SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.UShortKeyword)),
                    var t when t == typeof(int) => SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.IntKeyword)),
                    var t when t == typeof(uint) => SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.UIntKeyword)),
                    var t when t == typeof(long) => SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.LongKeyword)),
                    var t when t == typeof(ulong) => SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.ULongKeyword)),
                    var t when t == typeof(float) => SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.FloatKeyword)),
                    var t when t == typeof(double) => SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.DoubleKeyword)),
                    var t when t == typeof(decimal) => SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.DecimalKeyword)),
                    var t when t == typeof(string) => SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.StringKeyword)),
                    var t when t == typeof(object) => SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.ObjectKeyword)),
                    _ => SyntaxFactory.ParseTypeName(type.FullName!)
                };
            }
            else
            {
                // Non-generic, non-array, non-primitive
                return SyntaxFactory.ParseTypeName(type.FullName ?? type.Name);
            }
        }

        public static bool IsReferenceType(Type type)
        {
            type = EscapeType(type);

            // Check for class or interface
            return type.IsClass == true || type.IsInterface == true;
        }

        public static bool ContainsRefStructType(MethodInfo method)
        {
            // Check return
            if (IsRefStructType(method.ReturnType) == true)
                return true;

            // Check generics
            if(method.IsGenericMethod == true && method.GetGenericArguments().Any(t => IsRefStructType(t)) == true)
                return true;

            // Check parameters
            if(method.GetParameters().Select(p => p.ParameterType).Any(p => IsRefStructType(p)) == true)
                return true;

            return false;
        }

        public static bool IsRefStructType(Type type)
        {
            // Check for by ref like
            if (type.IsByRefLike == true)
                return true;

            // Check for generics
            if(type.IsGenericType == true)
            {
                if (type.GenericTypeArguments.Any(
                    t => IsRefStructType(t)) == true)
                    return true;
            }

            // Not by ref like
            return false;
        }

        public static Type EscapeType(Type type)
        {
            // Check for by ref
            if (type.IsByRef == true)
                type = type.GetElementType();

            return type;
        }

        private static void BuildGenericTypeString(StringBuilder builder, Type type)
        {
            // Check for namespace
            if (string.IsNullOrEmpty(type.Namespace) == false)
            {
                builder.Append(type.Namespace);
                builder.Append('.');
            }

            // Add name
            builder.Append(type.Name);

            // Check for generic
            if(type.IsGenericType == true)
            {
                // Generic start
                builder.Append('<');

                // Generic end
                builder.Append('>');
            }
        }
    }
}
