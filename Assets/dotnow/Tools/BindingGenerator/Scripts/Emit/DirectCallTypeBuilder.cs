#if !UNITY_DISABLE
#if UNITY_EDITOR 
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace dotnow.BindingGenerator.Emit
{
    internal sealed class DirectCallTypeBuilder
    {
        private Type type = null;

        public DirectCallTypeBuilder(Type type)
        {
            this.type = type;
        }

        public CodeTypeDeclaration BuildTypeDirectCall()
        {
            CodeTypeDeclaration codeType = new CodeTypeDeclaration(type.Name + "_DirectCallBindings");
            codeType.Comments.Add(new CodeCommentStatement("Generated from type: " + type.AssemblyQualifiedName));
            codeType.Comments.Add(new CodeCommentStatement("Generated from assembly: " + type.Assembly.Location));
            codeType.Attributes = MemberAttributes.Assembly | MemberAttributes.Static | MemberAttributes.Final;

            codeType.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeTypeReference(typeof(GeneratedAttribute))));

            var methodGroups = CollectMethodGroups();

            foreach (var methodGroup in methodGroups)
            {
                DirectCallMethodBuilder methodBuilder = new DirectCallMethodBuilder(methodGroup.ToArray());
                codeType.Members.AddRange(methodBuilder.BuildMethodDirectCall());
            }

            return codeType;
        }

        private IEnumerable<IGrouping<string, MethodInfo>> CollectMethodGroups()
        {
            return type.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .Where(m => m.DeclaringType != typeof(object) && m.DeclaringType != typeof(MarshalByRefObject))
                .Where(m => !m.IsSpecialName)
                .Where(m => !m.ContainsGenericParameters)
                .Where(m => !m.IsAbstract && !m.IsConstructor)
                .GroupBy(m => m.Name);
        }

        public string GetTypeFlattenedName()
        {
            return string.IsNullOrEmpty(type.Namespace)
                ? type.Name
                : string.Concat(type.Namespace.Replace('.', '_').Replace('<', '_').Replace('>', '_'), "_", type.Name);
        }
    }
}
#endif
#endif