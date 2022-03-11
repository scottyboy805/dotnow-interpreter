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

        // Methods
        public static void LoadByRefArgument(CILSignature signature, IByRef byRef, object[] arguments, int offset)
        {
            CLRTypeInfo paramTypeInfo = signature.parameterTypeInfos[offset];

            switch (paramTypeInfo.typeCode)
            {
                case TypeCode.Boolean: byRef.SetReferenceValueI4((bool)arguments[offset] ? 1 : 0); break;
                case TypeCode.Byte: byRef.SetReferenceValueI1((sbyte)(byte)arguments[offset]); break;
                case TypeCode.SByte: byRef.SetReferenceValueI1((sbyte)arguments[offset]); break;
                case TypeCode.Int16: byRef.SetReferenceValueI2((short)arguments[offset]); break;
                case TypeCode.UInt16: byRef.SetReferenceValueI2((short)(ushort)arguments[offset]); break;
                case TypeCode.Int32: byRef.SetReferenceValueI4((int)arguments[offset]); break;
                case TypeCode.UInt32: byRef.SetReferenceValueI4((int)(uint)arguments[offset]); break;
                case TypeCode.Int64: byRef.SetReferenceValueI8((long)arguments[offset]); break;
                case TypeCode.UInt64: byRef.SetReferenceValueI8((long)(ulong)arguments[offset]); break;
                case TypeCode.Single: byRef.SetReferenceValueR4((float)arguments[offset]); break;
                case TypeCode.Double: byRef.SetReferenceValueR8((double)arguments[offset]); break;

                case TypeCode.String:
                case TypeCode.Object:
                    {
                        byRef.SetReferenceValue((StackData)arguments[offset]);
                        break;
                    }
            }
        }
    }
}
