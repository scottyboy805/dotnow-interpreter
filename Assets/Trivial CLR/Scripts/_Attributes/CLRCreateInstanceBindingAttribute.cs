using System;

namespace TrivialCLR
{
    public class CLRCreateInstanceBindingAttribute : Attribute
    {
        // Private
        private Type declaringType;
        private Type[] parameterTypes = null;

        // Properties
        public Type DeclaringType
        {
            get { return declaringType; }
        }

        public Type[] ParameterTypes
        {
            get { return parameterTypes; }
        }

        // Constructor
        public CLRCreateInstanceBindingAttribute(Type declaringType, Type[] parameterTypes = null)
        {
            this.declaringType = declaringType;
            this.parameterTypes = parameterTypes;
        }
    }
}
