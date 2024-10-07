#if !UNITY_DISABLE
#if UNITY_EDITOR 
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

        public bool GenerateGetterOrSetter
        {
            get { return GenerateGetter == true || GenerateSetter == true; }
        }

        public bool GenerateGetter
        {
            get
            {
                // Try to get getter
                MethodInfo method = property.GetGetMethod(true);

                // Only interested in properties that can be overridden
                return method != null && (method.IsVirtual == true || method.IsAbstract == true );
            }
        }

        public bool GenerateSetter
        {
            get
            {
                // Try to get setter
                MethodInfo method = property.GetSetMethod(true);

                // Only interested in properties that can be overridden
                return method != null && (method.IsVirtual == true || method.IsAbstract == true);
            }
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
            MethodInfo setter = property.SetMethod;
            MethodInfo getter = property.GetMethod;
            codeProperty.Attributes = 0;

            // Property type
            codeProperty.Type = new CodeTypeReference(property.PropertyType);

            // Emit getter
           

            if(getter != null)
            {
                if (!getter.IsVirtual&& !getter.IsAbstract)
                {
                    codeProperty.Attributes =  MemberAttributes.New;
                }
                else
                {
                    codeProperty.Attributes = MemberAttributes.Override;
                }
                
                codeProperty.Attributes |= MemberAttributes.Public;
                
                // Create the method builder
                ProxyMethodBuilder getterBuilder = new ProxyMethodBuilder(getter, interfaceType, memberIndex);

                // Emit the method
                CodeMemberMethod emitMethod = getterBuilder.BuildMethodProxy();

                // Add all statements
                codeProperty.GetStatements.AddRange(emitMethod.Statements);
            }

            // Emit setter
           

            if(setter != null)
            {
                if (!setter.IsVirtual&& !setter.IsAbstract)
                {
                    codeProperty.Attributes =  MemberAttributes.New;
                }
                else
                {
                    codeProperty.Attributes = MemberAttributes.Override;
                }
            
                // Create the method builder
                ProxyMethodBuilder setterBuilder = new ProxyMethodBuilder(setter, interfaceType, memberIndex);

                // Emit the method
                CodeMemberMethod emitMethod = setterBuilder.BuildMethodProxy();
                
                // Add all statements
                codeProperty.SetStatements.AddRange(emitMethod.Statements);
            }
            codeProperty.Attributes |= MemberAttributes.Public;
            return codeProperty;
        }
    }
}
#endif
#endif
