using System;
using System.Reflection;

namespace dotnow.Reflection
{
    public class CLRAnonymousParameter : ParameterInfo
    {
        // Private
        private Type parameterType = null;
        private int parameterIndex = -1;

        // Properties
        public override string Name
        {
            get
            {
                if (parameterIndex != -1)
                    return parameterIndex.ToString();

                return base.Name;
            }
        }

        public override Type ParameterType
        {
            get { return parameterType; }
        }

        // Constructor
        internal CLRAnonymousParameter(Type parameterType, int parameterIndex = -1)
        {
            this.parameterType = parameterType;
            this.parameterIndex = parameterIndex;
        }
    }
}
