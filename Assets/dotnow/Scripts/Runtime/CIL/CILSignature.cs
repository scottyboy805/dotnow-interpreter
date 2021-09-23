using System;
using System.Reflection;

namespace dotnow.Runtime.CIL
{
    internal sealed class CILSignature
    {
        // Private
        private static readonly CLRTypeInfo voidType = CLRTypeInfo.GetTypeInfo(typeof(void));

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
                parameterTypeInfos[i] = CLRTypeInfo.GetTypeInfo(parameterTypes[i]);
            }

            this.argumentCount = parameters.Length;            

            if (returnType != null)
            {
                this.returnType = CLRTypeInfo.GetTypeInfo(returnType);
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
