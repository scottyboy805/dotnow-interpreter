using System;
using System.Reflection;

namespace TrivialCLR.Runtime.CIL
{
    internal sealed class CILSignature
    {
        // Private
        private static readonly CLRTypeInfo voidType = new CLRTypeInfo(typeof(void));

        // Internal
        internal CLRTypeInfo returnType;
        internal ParameterInfo[] parameters;
        internal CLRTypeInfo[] parameterTypeInfos;
        internal Type[] parameterTypes;
        internal int argumentCount;
        internal bool returnsValue;

        // Constructor
        public CILSignature(ParameterInfo[] parameters, Type returnType = null)
        {
            this.parameters = parameters;
            this.parameterTypeInfos = new CLRTypeInfo[parameters.Length];
            this.parameterTypes = new Type[parameters.Length];

            for(int i = 0; i < parameters.Length; i++)
            {
                parameterTypes[i] = parameters[i].ParameterType;
                parameterTypeInfos[i] = new CLRTypeInfo(parameterTypes[i]);
            }

            this.argumentCount = parameters.Length;            

            if (returnType != null)
            {
                this.returnType = new CLRTypeInfo(returnType);
                this.returnsValue = returnType != typeof(void);
            }
            else
            {
                this.returnType = voidType;
                this.returnsValue = false;
            }
        }
    }
}
