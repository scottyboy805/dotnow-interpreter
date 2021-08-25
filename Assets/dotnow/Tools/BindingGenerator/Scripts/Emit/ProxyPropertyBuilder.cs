using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace dotnow.BindingGenerator.Emit
{
    public class ProxyPropertyBuilder
    {
        // Private
        private PropertyInfo property = null;
        private bool interfaceType = false;
        private int memberIndex = 0;

        // Constructor
        public ProxyPropertyBuilder(PropertyInfo property, bool interfaceType, int memberIndex)
        {
            this.property = property;
            this.interfaceType = interfaceType;
            this.memberIndex = memberIndex;
        }

        // Methods
        public CodeMemberProperty BuildPropertyProxy()
        {
            CodeMemberProperty codeProperty = new CodeMemberProperty();
            codeProperty.Name = property.Name;

            if(interfaceType == true)
            {
                codeProperty.Attributes = MemberAttributes.Public | MemberAttributes.Final;
            }
            else
            {
                MemberAttributes attributes = 0;

            }

            // Emit getter
            MethodInfo getter = property.GetMethod;

            if(getter != null)
            {
                // Create the method builder
                ProxyMethodBuilder getterBuilder = new ProxyMethodBuilder(getter, interfaceType, memberIndex);

                // Emit the method
                CodeMemberMethod emitMethod = getterBuilder.BuildMethodProxy();

                // Add all statements
                codeProperty.GetStatements.AddRange(emitMethod.Statements);
            }

            // Emit setter
            MethodInfo setter = property.SetMethod;

            if(setter != null)
            {
                // Create the method builder
                ProxyMethodBuilder setterBuilder = new ProxyMethodBuilder(setter, interfaceType, memberIndex);

                // Emit the method
                CodeMemberMethod emitMethod = setterBuilder.BuildMethodProxy();

                // Add all statements
                codeProperty.SetStatements.AddRange(emitMethod.Statements);
            }

            return codeProperty;
        }
    }
}
