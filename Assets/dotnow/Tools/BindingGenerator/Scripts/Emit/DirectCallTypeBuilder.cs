#if !UNITY_DISABLE
#if UNITY_EDITOR && NET_4_6
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Reflection;

namespace dotnow.BindingGenerator.Emit
{
    internal sealed class DirectCallTypeBuilder
    {
        // Private
        private Type type = null;

        // Constructor
        public DirectCallTypeBuilder(Type type)
        {
            this.type = type;
        }

        // Methods
        public CodeTypeDeclaration BuildTypeDirectCall()
        {
            CodeTypeDeclaration codeType = new CodeTypeDeclaration(type.Name + "_DirectCallBindings");
            codeType.Comments.Add(new CodeCommentStatement("Generated from type: " + type.AssemblyQualifiedName));
            codeType.Comments.Add(new CodeCommentStatement("Generated from assembly: " + type.Assembly.Location));
            codeType.Attributes = MemberAttributes.Assembly | MemberAttributes.Static | MemberAttributes.Final;

            // Add generated attribute
            codeType.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeTypeReference(typeof(GeneratedAttribute))));

            HashSet<string> definedMethodNames = new HashSet<string>();

            // Todo - Process all properties

            // Process all methods
            foreach (MethodInfo method in type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            {
                // Skip object methods
                if (method.DeclaringType == typeof(object) || method.DeclaringType == typeof(MarshalByRefObject))
                    continue;

                // Check for property - these will be handled by ProxyPropertyBuilder
                if (method.IsSpecialName == true)
                    continue;

                // Check for already added
                if (definedMethodNames.Contains(method.ToString()) == true)
                    continue;

                if (method.IsAbstract == false)
                {
                    DirectCallMethodBuilder methodBuilder = new DirectCallMethodBuilder(method);

                    // Build method
                    codeType.Members.Add(methodBuilder.BuildMethodDirectCall());

                    definedMethodNames.Add(method.Name);
                }
            }

            return codeType;
        }

        public string GetTypeFlattenedName()
        {
            // Check for namespace
            if (string.IsNullOrEmpty(type.Namespace) == true)
                return type.Name;

            return string.Concat(type.Namespace.Replace('.', '_'), "_", type.Name);
        }
    }
}
#endif
#endif