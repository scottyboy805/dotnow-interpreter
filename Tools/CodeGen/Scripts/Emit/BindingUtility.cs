using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;

namespace dotnow.CodeGen
{
    public static class BindingUtility
    {
        // Methods
        public static TypeSyntax BuildTypeReference(Type type)
        {
            // Check for by ref
            if(type.IsByRef == true)
                type = type.GetElementType();

            // Get the type full name
            return SyntaxFactory.ParseTypeName(type.FullName);
        }

        public static Type EscapeType(Type type)
        {
            // Check for by ref
            if (type.IsByRef == true)
                type = type.GetElementType();

            return type;
        }
    }
}
