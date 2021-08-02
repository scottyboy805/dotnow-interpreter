using System;
using System.Reflection;
using Mono.Cecil.Cil;

namespace dotnow.Runtime.CIL
{
    public struct CILOperation
    {
        // Public
        public Code opCode;
        public StackData.Primitive operand;
        public object objectOperand;
        public CLRTypeInfo typeOperand;

        // Constructor
        public CILOperation(Code opCode, StackData.Primitive operand, object objectOperand)
        {
            this.opCode = opCode;
            this.operand = operand;
            this.objectOperand = objectOperand;
            this.typeOperand = null;
        }

        // Methods
        public override string ToString()
        {
            if(objectOperand != null)
            {
                return string.Concat(opCode.ToString(), " ", objectOperand.ToString());
            }

            return opCode.ToString();
        }
    }
}
