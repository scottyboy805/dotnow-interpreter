#if !UNITY_DISABLE
#if UNITY_EDITOR 
using System.Reflection;
using System.CodeDom;
using System.Runtime.InteropServices;

namespace dotnow.BindingGenerator.Emit
{
    public class ProxyMethodBuilder
    {
        // Private
        private MethodInfo method = null;
        private bool interfaceType = false;
        private string variableName = "";
        private bool isOverride = false;

        // Properties
        public string VariableName
        {
            get { return variableName; }
        }

        // Constructor
        public ProxyMethodBuilder(MethodInfo method, bool interfaceType, int memberIndex, bool isOverride = false)
        {
            this.method = method;
            this.interfaceType = interfaceType;
            this.variableName = string.Concat("clrTarget_", method.Name + memberIndex + 1);
            this.isOverride = isOverride;
        }

        // Methods
        public CodeMemberMethod BuildMethodProxy()
        {
            CodeMemberMethod codeMethod = new CodeMemberMethod();
            codeMethod.Name = method.Name;

            if(interfaceType == true)
            {
                codeMethod.Attributes = MemberAttributes.Public | MemberAttributes.Final;
            }
            else
            {
                MemberAttributes attributes = 0;

                if (method.IsPublic == true) attributes |= MemberAttributes.Public;
                if (method.IsVirtual == true || method.IsAbstract == true || isOverride) attributes |= MemberAttributes.Override;

                codeMethod.Attributes = attributes;
            }

            // Rest of the method remains the same...
            // Return type
            codeMethod.ReturnType = new CodeTypeReference(method.ReturnType);

            System.Type underlyingType = method.ReturnType.UnderlyingSystemType;

            // Parameter types
            ParameterInfo[] parameters = method.GetParameters();

            foreach(ParameterInfo parameter in parameters)
            {
                // Create parameter
                CodeParameterDeclarationExpression codeParameter = new CodeParameterDeclarationExpression(new CodeTypeReference(parameter.ParameterType), parameter.Name);

                // Check for modifiers
                if (parameter.IsIn == true) codeParameter.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeTypeReference(typeof(InAttribute))));
                if (parameter.IsOut == true) codeParameter.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeTypeReference(typeof(OutAttribute))));

                codeMethod.Parameters.Add(codeParameter);
            }

            // Null check
            if (method.ReturnType == typeof(void))
            {
                codeMethod.Statements.Add(new CodeConditionStatement(
                    new CodeBinaryOperatorExpression(
                        new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "instance"),
                        CodeBinaryOperatorType.ValueEquality,
                        new CodePrimitiveExpression(null)),
                            new CodeMethodReturnStatement()));
            }

            // Method body
            codeMethod.Statements.Add(new CodeConditionStatement(
                new CodeBinaryOperatorExpression(
                new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), variableName),
                CodeBinaryOperatorType.ValueEquality,
                new CodePrimitiveExpression(null)),
                    new CodeAssignStatement(
                        new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), variableName),
                        new CodeMethodInvokeExpression(
                            new CodeFieldReferenceExpression(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), ProxyTypeBuilder.instanceVar), 
                            nameof(CLRInstance.Type)),
                            nameof(CLRType.GetMethod),
                            new CodePrimitiveExpression(method.Name)))));

            // Invoke
            CodeExpression[] argList = new CodeExpression[parameters.Length];

            for (int i = 0; i < argList.Length; i++)
            {
                argList[i] = new CodeArgumentReferenceExpression(parameters[i].Name);
            }

            CodeExpression[] args = new CodeExpression[2];

            args[0] = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "instance");
            args[1] = new CodeArrayCreateExpression(
                new CodeTypeReference(typeof(object)), argList);

            // Null check before invoke
            codeMethod.Statements.Add(new CodeConditionStatement(
                    new CodeBinaryOperatorExpression(
                        new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), variableName),
                        CodeBinaryOperatorType.ValueEquality,
                        new CodePrimitiveExpression(null)),
                            method.ReturnType != typeof(void) ? new CodeMethodReturnStatement(new CodeDefaultValueExpression(new CodeTypeReference(method.ReturnType))) : new CodeMethodReturnStatement()));

            if (method.ReturnType != typeof(void))
            {
                codeMethod.Statements.Add(new CodeMethodReturnStatement(
                    new CodeCastExpression(new CodeTypeReference(method.ReturnType),
                    new CodeMethodInvokeExpression(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), VariableName),
                    nameof(MethodBase.Invoke),
                    args))));
            }
            else
            {
                codeMethod.Statements.Add(new CodeMethodInvokeExpression(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), VariableName),
                    nameof(MethodBase.Invoke),
                    args));
            }

            return codeMethod;
        }
    }
}
#endif
#endif