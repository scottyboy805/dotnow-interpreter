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

            return codeMethod;
        }
    }
}
#endif
#endif