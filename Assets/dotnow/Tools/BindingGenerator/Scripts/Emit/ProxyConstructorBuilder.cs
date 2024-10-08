using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace dotnow.BindingGenerator.Emit
{
    public class ProxyConstructorBuilder
    {
        private ConstructorInfo constructor;
        private Type type;
        public ProxyConstructorBuilder(ConstructorInfo constructor, Type type)
        {
            this.constructor = constructor;
            this.type = type;
        }

        public CodeConstructor BuildConstructorProxy()
        {
            CodeConstructor codeConstructor = new CodeConstructor();
            codeConstructor.Attributes = MemberAttributes.Public;

            List<ParameterInfo> baseParameters = new List<ParameterInfo>();
            foreach (var param in constructor.GetParameters())
            {
                baseParameters.Add(param);
                codeConstructor.Parameters.Add(new CodeParameterDeclarationExpression(param.ParameterType, param.Name));
            }
            // Call base constructor
            codeConstructor.BaseConstructorArgs.AddRange(
                baseParameters.Select(p => new CodeArgumentReferenceExpression(p.Name)).ToArray<CodeExpression>()
                );

           
            return codeConstructor;
        }
    }
}