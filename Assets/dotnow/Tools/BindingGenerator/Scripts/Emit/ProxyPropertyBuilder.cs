
#if UNITY_EDITOR && NET_4_6 && UNITY_DISABLE == false
using System.CodeDom;
using System.Reflection;

namespace dotnow.BindingGenerator.Emit
{
    public class ProxyPropertyBuilder
    {
        // Private
        private PropertyInfo property = null;
        private bool interfaceType = false;
        private int memberIndex = 0;
        private string variableNameGetter = "";
        private string variableNameSetter = "";

        // Properties
        public string VariableNameGetter
        {
            get { return variableNameGetter; }
        }

        public string VariableNameSetter
        {
            get { return variableNameSetter; }
        }

        // Constructor
        public ProxyPropertyBuilder(PropertyInfo property, bool interfaceType, int memberIndex)
        {
            this.property = property;
            this.interfaceType = interfaceType;
            this.memberIndex = memberIndex;
            this.variableNameGetter = string.Concat("clrTarget_get_", property.Name + memberIndex + 1);
            this.variableNameSetter = string.Concat("clrTarget_set_", property.Name + memberIndex + 1);
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

                attributes |= MemberAttributes.Public;
                attributes |= MemberAttributes.Final;

                codeProperty.Attributes = attributes;
            }

            // Property type
            codeProperty.Type = new CodeTypeReference(property.PropertyType);

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
#endif
