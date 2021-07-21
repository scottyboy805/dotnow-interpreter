using System;
using System.Reflection;

namespace TrivialCLR.Interop
{
    internal sealed class CLRAbstractParameterInfo : ParameterInfo
    {
        // Private
        private Type parameterType = null;

        // Properties
        public override Type ParameterType
        {
            get { return parameterType; }
        }

        // Constructor
        public CLRAbstractParameterInfo(Type parameterType)
        {
            this.parameterType = parameterType;
        }
    }
}
