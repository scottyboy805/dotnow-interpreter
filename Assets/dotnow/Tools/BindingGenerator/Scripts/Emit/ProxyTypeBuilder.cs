using System;
using System.CodeDom;
using System.Reflection;
using dotnow.Interop;

namespace dotnow.BindingGenerator.Emit
{
    internal sealed class ProxyTypeBuilder
    {
        // Private
        private Type type = null;

        // Public
        public const string appDomainVar = "appDomain";
        public const string instanceVar = "instance";

        // Constructor
        public ProxyTypeBuilder(Type type)
        {
            this.type = type;
        }

        // Methods
        public CodeTypeDeclaration BuildTypeProxy()
        {
            CodeTypeDeclaration codeType = new CodeTypeDeclaration(type.Name + "_ProxyBinding");
            codeType.Comments.Add(new CodeCommentStatement("Generated from type: " + type.AssemblyQualifiedName));
            codeType.Comments.Add(new CodeCommentStatement("Generated from assembly: " + type.Assembly.Location));
            codeType.Attributes = MemberAttributes.Public | MemberAttributes.Final;
            
            // Create base type
            codeType.BaseTypes.Add(new CodeTypeReference(type));
            codeType.BaseTypes.Add(new CodeTypeReference(typeof(ICLRProxy)));

            // Create attribute
            codeType.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeTypeReference(typeof(CLRProxyBindingAttribute)),
                new CodeAttributeArgument(new CodeTypeOfExpression(new CodeTypeReference(type)))));

            // Create app domain field
            codeType.Members.Add(new CodeMemberField(new CodeTypeReference(typeof(AppDomain)), appDomainVar));

            // Create instance field
            codeType.Members.Add(new CodeMemberField(new CodeTypeReference(typeof(CLRInstance)), instanceVar));


            // Implement method
            codeType.Members.Add(BuildCLRProxyImplementation());


            // Implement methods
            if(type.IsInterface == true)
            {

            }
            else
            {
                foreach(MethodInfo method in type.GetMethods())
                {
                    // Skip object methods
                    if (method.DeclaringType == typeof(object))
                        continue;

                    if(method.IsVirtual == true || method.IsAbstract == true)
                    {
                        ProxyMethodBuilder methodBuilder = new ProxyMethodBuilder(method, false);

                        // Build field
                        codeType.Members.Add(new CodeMemberField(new CodeTypeReference(typeof(MethodBase)), methodBuilder.VariableName));

                        codeType.Members.Add(methodBuilder.BuildMethodProxy());
                    }
                }
            }

            return codeType;
        }

        public string GetTypeFlattenedName()
        {
            return string.Concat(type.Namespace.Replace('.', '_'), "_", type.Name);
        }

        private CodeMemberMethod BuildCLRProxyImplementation()
        {
            // Create the method
            CodeMemberMethod codeMethod = new CodeMemberMethod();
            codeMethod.Name = nameof(ICLRProxy.InitializeProxy);
            codeMethod.Attributes = MemberAttributes.Public | MemberAttributes.Final;

            // Get target method
            MethodInfo implementMethod = typeof(ICLRProxy).GetMethod(nameof(ICLRProxy.InitializeProxy), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            ParameterInfo[] implementMethodParameters = implementMethod.GetParameters();

            // Create parameters
            foreach(ParameterInfo parameter in implementMethodParameters)
            {
                codeMethod.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference(parameter.ParameterType), parameter.Name));
            }

            // Create assign app domain
            codeMethod.Statements.Add(new CodeAssignStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), appDomainVar),
                new CodeVariableReferenceExpression(implementMethodParameters[0].Name)));

            // Create assign instance
            codeMethod.Statements.Add(new CodeAssignStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), instanceVar),
                new CodeVariableReferenceExpression(implementMethodParameters[1].Name)));

            return codeMethod;
        }
    }
}
