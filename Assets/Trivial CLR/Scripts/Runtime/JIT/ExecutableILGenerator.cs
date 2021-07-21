using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil.Cil;
using TrivialCLR.Reflection;
using TrivialCLR.Runtime.CIL;
//using Instruction = TrivialCLR.Runtime.CIL.Instruction;

namespace TrivialCLR.Runtime.JIT
{
    public class ExecutableILGenerator
    {
        // Private
        private AppDomain domain = null;
        private List<Instruction> instructionSet = new List<Instruction>();
        private List<CILOperation> operations = null;

        //private static readonly Ret ret = new Ret();
        //private static readonly Throw throwEx = new Throw();

        //private static readonly Pop pop = new Pop();

        //private static readonly Add add = new Add();
        //private static readonly AddOvf addovf = new AddOvf();

        //private static readonly Clt clt = new Clt();

        // Constructor
        public ExecutableILGenerator(AppDomain domain)
        {
            this.domain = domain;
            this.operations = new List<CILOperation>();
        }

        public ExecutableILGenerator(AppDomain domain, int instructionSize)
        {
            this.domain = domain;
            this.operations = new List<CILOperation>(instructionSize);
        }

        // Methods
        public Instruction[] GetExecutableInstructionSet()
        {
            // Get as array
            return instructionSet.ToArray();
        }

        internal CILOperation[] GetExecutableInstructions()
        {
            return operations.ToArray();
        }

        #region BuildMethod
        public MethodInfo BuildExecutableMethod(Type[] localTypes, int maxStack, Type returnType, params Type[] parameterTypes)
        {
            return new CLRAnonymousMethod(domain, GetExecutableInstructions(), true, maxStack, localTypes, returnType, parameterTypes);
        }
        #endregion

        #region Emit
        public void Emit(Code opCode)
        {
            operations.Add(new CILOperation(opCode, new StackData.Primitive { Int32 = 0 }, null));
        }

        public void Emit(Code opCode, int operand)
        {
            operations.Add(new CILOperation(opCode, new StackData.Primitive { Int32 = operand }, null));
        }

        public void Emit(Code opCode, long operand)
        {
            operations.Add(new CILOperation(opCode, new StackData.Primitive { Int64 = operand }, null));
        }

        public void Emit(Code opCode, float operand)
        {
            operations.Add(new CILOperation(opCode, new StackData.Primitive { Single = operand }, null));
        }

        public void Emit(Code opCode, double operand)
        {
            operations.Add(new CILOperation(opCode, new StackData.Primitive { Double = operand }, null));
        }

        public void Emit(Code opCode, string operand)
        {
            operations.Add(new CILOperation(opCode, default, operand));
        }

        public void Emit(Code opCode, int[] operand)
        {
            operations.Add(new CILOperation(opCode, default, operand));
        }

        public void Emit(Code opCode, Type type)
        {
            CILOperation op = new CILOperation(opCode, default, null);
            op.typeOperand = new CLRTypeInfo(type);

            operations.Add(op);
        }

        public void Emit(Code opCode, MethodBase method)
        {
            Emit(opCode, (MemberInfo)method);
            //operations.Add(new CILOperation(opCode, 0, method));
        }

        public void Emit(Code opCode, FieldInfo field)
        {
            Emit(opCode, (MemberInfo)field);
            //operations.Add(new CILOperation(opCode, 0, field));
        }

        public void Emit(Code opCode, MemberInfo member, Type specialType = null)
        {
            CILOperation op = new CILOperation(opCode, default, member);

            // Set type value
            if(specialType != null)
                op.typeOperand = new CLRTypeInfo(specialType);

            // Check for field access
            if(member is FieldInfo)
            {
                // Setup field access
                CILFieldAccess access = new CILFieldAccess((FieldInfo)member);
                access.SetupFieldAccess(domain);

                // Create access cache
                op.objectOperand = access;
            }

            // Check for method call
            if(member is MethodBase)
            {
                // Setup method call
                CILMethodInvocation invocation = new CILMethodInvocation((MethodBase)member);
                invocation.SetupMethodCall(domain);

                // Create method cache
                op.objectOperand = invocation;
            }

            // Check for type
            if(member is Type)
            {
                op.typeOperand = new CLRTypeInfo((Type)member);
            }

            operations.Add(op);
        }
        #endregion


        //public void EmitNop()
        //{
        //    instructionSet.Add(new Nop());
        //}

        //public void EmitDup()
        //{
        //    instructionSet.Add(new Dup());
        //}

        //public void EmitPop()
        //{
        //    instructionSet.Add(pop);
        //}

        //public void EmitLdObj(object loadObject)
        //{
        //    instructionSet.Add(new Ldobj(loadObject));
        //}

        //public void EmitCall(MethodBase methodCall)
        //{
        //    instructionSet.Add(new Call(methodCall, methodCall.GetParameters().Length));
        //}

        //public void EmitCallVirt(AppDomain domain, MethodBase methodCall)
        //{
        //    instructionSet.Add(new Callvirt(domain, methodCall, methodCall.GetParameters().Length));
        //}

        //public void EmitRet()
        //{
        //    instructionSet.Add(ret);
        //}

        //public void EmitLdFtn(MethodBase methodTarget)
        //{
        //    instructionSet.Add(new Ldftn(methodTarget));
        //}

        //public void EmitThrow()
        //{
        //    instructionSet.Add(throwEx);
        //}

        //public void EmitAdd()
        //{
        //    instructionSet.Add(add);
        //}

        //public void EmitAddOvfUn()
        //{
        //    instructionSet.Add(add);
        //}

        //public void EmitAddOvf()
        //{
        //    instructionSet.Add(addovf);
        //}

        //public void EmitSub()
        //{
        //    instructionSet.Add(new Sub());
        //}

        //public void EmitSubOvf()
        //{
        //    instructionSet.Add(new SubOvf());
        //}

        //public void EmitMul()
        //{
        //    instructionSet.Add(new Mul());
        //}

        //public void EmitMulOvf()
        //{
        //    instructionSet.Add(new MulOvf());
        //}

        //public void EmitDiv()
        //{
        //    instructionSet.Add(new Div());
        //}

        //public void EmitNeg()
        //{
        //    instructionSet.Add(new Neg());
        //}

        //public void EmitRem()
        //{
        //    instructionSet.Add(new Rem());
        //}

        //public void EmitRemUn()
        //{
        //    instructionSet.Add(new Rem_Un());
        //}

        //public void EmitShl()
        //{
        //    instructionSet.Add(new Shl());
        //}

        //public void EmitShr()
        //{
        //    instructionSet.Add(new Shr());
        //}

        //public void EmitLdLoc(int index)
        //{
        //    instructionSet.Add(new Ldloc(index));
        //}

        //public void EmitLdLocA(int index)
        //{
        //    instructionSet.Add(new Ldloca(index));
        //}

        //public void EmitStLoc(int index)
        //{
        //    instructionSet.Add(new Stloc(index));
        //}

        //public void EmitLdArg(int index)
        //{
        //    instructionSet.Add(new Ldarg(index));            
        //}

        //public void EmitLdArga(int index)
        //{
        //    instructionSet.Add(new Ldarga(index));
        //}

        //public void EmitStArg(int index)
        //{
        //    instructionSet.Add(new Starg(index));
        //}

        //#region LdInd
        //public void EmitLdIndI()
        //{
        //    instructionSet.Add(new Ldindi());
        //}

        //public void EmitLdIndI1()
        //{
        //    instructionSet.Add(new Ldindi1());
        //}

        //public void EmitLdIndI2()
        //{
        //    instructionSet.Add(new Ldindi2());
        //}

        //public void EmitLdIndI4()
        //{
        //    instructionSet.Add(new Ldindi4());
        //}

        //public void EmitLdIndI8()
        //{
        //    instructionSet.Add(new Ldindi8());
        //}

        //public void EmitLdIndR4()
        //{
        //    instructionSet.Add(new Ldindr4());
        //}

        //public void EmitLdIndR8()
        //{
        //    instructionSet.Add(new Ldindr8());
        //}

        //public void EmitLdIndU1()
        //{
        //    instructionSet.Add(new Ldindu1());
        //}

        //public void EmitLdIndU2()
        //{
        //    instructionSet.Add(new Ldindu2());
        //}

        //public void EmitLdIndU4()
        //{
        //    instructionSet.Add(new Ldindu4());
        //}

        //public void EmitLdIndRef()
        //{
        //    instructionSet.Add(new Ldindref());
        //}
        //#endregion

        //#region StInd
        //public void EmitStIndI()
        //{
        //    instructionSet.Add(new Stindi());
        //}

        //public void EmitStIndI1()
        //{
        //    instructionSet.Add(new Stindi1());
        //}

        //public void EmitStIndI2()
        //{
        //    instructionSet.Add(new Stindi2());
        //}

        //public void EmitStIndI4()
        //{
        //    instructionSet.Add(new Stindi4());
        //}

        //public void EmitStIndI8()
        //{
        //    instructionSet.Add(new Stindi8());
        //}

        //public void EmitStIndR4()
        //{
        //    instructionSet.Add(new Stindr4());
        //}

        //public void EmitStIndR8()
        //{
        //    instructionSet.Add(new Stindr8());
        //}

        //public void EmitStIndRef()
        //{
        //    instructionSet.Add(new Stindref());
        //}
        //#endregion

        //public void EmitLdFld(FieldInfo field, bool addressOf = false)
        //{
        //    if(addressOf == true)
        //    {
        //        instructionSet.Add(new Ldflda(field));
        //    }
        //    else
        //    {
        //        if (field.IsStatic == true) instructionSet.Add(new Ldsfld(field));
        //        else instructionSet.Add(new Ldfld(field));
        //    }
        //}

        //public void EmitStFld(FieldInfo field)
        //{
        //    if (field.IsStatic == true) instructionSet.Add(new Stsfld(field));
        //    else instructionSet.Add(new Stfld(field));
        //}

        //public void EmitBr(int offset)
        //{
        //    instructionSet.Add(new Br(offset));
        //}

        //public void EmitBrTrue(int offset)
        //{
        //    instructionSet.Add(new Brtrue(offset));
        //}

        //public void EmitBrFalse(int offset)
        //{
        //    instructionSet.Add(new Brfalse(offset));
        //}

        //public void EmitBlt(int offset)
        //{
        //    instructionSet.Add(new Blt(offset));
        //}

        //public void EmitBgt(int offset)
        //{
        //    instructionSet.Add(new Bgt(offset));
        //}

        //public void EmitBle(int offset)
        //{
        //    instructionSet.Add(new Ble(offset));
        //}

        //public void EmitBge(int offset)
        //{
        //    instructionSet.Add(new Bge(offset));
        //}

        //public void EmitBeq(int offset)
        //{
        //    instructionSet.Add(new Beq(offset));
        //}

        //public void EmitBne(int offset)
        //{
        //    instructionSet.Add(new Bne(offset));
        //}

        //public void EmitSwitch(int[] offsets)
        //{
        //    instructionSet.Add(new Switch(offsets));
        //}

        //public void EmitBox(Type boxType)
        //{
        //    instructionSet.Add(new Box(boxType));
        //}

        //public void EmitClt()
        //{
        //    instructionSet.Add(clt);
        //}

        //public void EmitCgt()
        //{
        //    instructionSet.Add(new Cgt());
        //}

        //public void EmitCeq()
        //{
        //    instructionSet.Add(new Ceq());
        //}

        //public void EmitNew(AppDomain domain, Type instanceType, MethodBase ctor)
        //{
        //    instructionSet.Add(new New(domain, instanceType, ctor, ctor.GetParameters().Length));
        //}

        //public void EmitInitObj()
        //{
        //    instructionSet.Add(new InitObj());
        //}

        //public void EmitIsInst(Type type)
        //{
        //    instructionSet.Add(new Isinst(type));
        //}

        //public void EmitLdToken(MemberInfo memberToken)
        //{
        //    instructionSet.Add(new Ldtoken(memberToken));
        //}

        //public void EmitNewArr(Type elementType)
        //{
        //    instructionSet.Add(new Newarr(elementType));
        //}

        //#region LdElem
        //public void EmitLdElem(Type elementType)
        //{
        //    instructionSet.Add(new Ldelem(elementType));
        //}

        //public void EmitLdElemI()
        //{
        //    instructionSet.Add(new Ldelemi());
        //}

        //public void EmitLdElemI1()
        //{
        //    instructionSet.Add(new Ldelemi1());
        //}

        //public void EmitLdElemI2()
        //{
        //    instructionSet.Add(new Ldelemi2());
        //}

        //public void EmitLdElemI4()
        //{
        //    instructionSet.Add(new Ldelemi4());
        //}

        //public void EmitLdElemI8()
        //{
        //    instructionSet.Add(new Ldelemi8());
        //}

        //public void EmitLdElemR4()
        //{
        //    instructionSet.Add(new Ldelemr4());
        //}

        //public void EmitLdElemR8()
        //{
        //    instructionSet.Add(new Ldelemr8());
        //}

        //public void EmitLdElemU1()
        //{
        //    instructionSet.Add(new Ldelemu1());
        //}

        //public void EmitLdElemU2()
        //{
        //    instructionSet.Add(new Ldelemu2());
        //}

        //public void EmitLdElemU4()
        //{
        //    instructionSet.Add(new Ldelemu4());
        //}

        //public void EmitLdElemRef()
        //{
        //    instructionSet.Add(new Ldelemref());
        //}

        //public void EmitLdElemA()
        //{
        //    instructionSet.Add(new Ldelema());
        //}
        //#endregion

        //#region StElem
        //public void EmitStElem(Type elementType)
        //{
        //    instructionSet.Add(new Stelem(elementType));
        //}

        //public void EmitStElemI()
        //{
        //    instructionSet.Add(new Stelemi());
        //}

        //public void EmitStElemI1()
        //{
        //    instructionSet.Add(new Stelemi1());
        //}

        //public void EmitStElemI2()
        //{
        //    instructionSet.Add(new Stelemi2());
        //}

        //public void EmitStElemI4()
        //{
        //    instructionSet.Add(new Stelemi4());
        //}

        //public void EmitStElemI8()
        //{
        //    instructionSet.Add(new Stelemr8());
        //}

        //public void EmitStElemR4()
        //{
        //    instructionSet.Add(new Stelemr4());
        //}

        //public void EmitStElemR8()
        //{
        //    instructionSet.Add(new Stelemr8());
        //}

        //public void EmitStElemRef()
        //{
        //    instructionSet.Add(new Stelemref());
        //}
        //#endregion

        //public void EmitLdLen()
        //{
        //    instructionSet.Add(new LdLen());
        //}

        //#region Conv
        //public void EmitConvI()
        //{
        //    instructionSet.Add(new Convi());
        //}

        //public void EmitConvI1()
        //{
        //    instructionSet.Add(new Convi1());
        //}

        //public void EmitConvI2()
        //{
        //    instructionSet.Add(new Convi2());
        //}

        //public void EmitConvI4()
        //{
        //    instructionSet.Add(new Convi4());
        //}

        //public void EmitConvI8()
        //{
        //    instructionSet.Add(new Convi8());
        //}

        //public void EmitConvU()
        //{
        //    instructionSet.Add(new Convu());
        //}

        //public void EmitConvU1()
        //{
        //    instructionSet.Add(new Convu1());
        //}

        //public void EmitConvU2()
        //{
        //    instructionSet.Add(new Convu2());
        //}

        //public void EmitConvU4()
        //{
        //    instructionSet.Add(new Convu4());
        //}

        //public void EmitConvU8()
        //{
        //    instructionSet.Add(new Convu8());
        //}

        //public void EmitConvRUn()
        //{
        //    instructionSet.Add(new Convrun());
        //}

        //public void EmitConvR4()
        //{
        //    instructionSet.Add(new Convr4());
        //}

        //public void EmitConvR8()
        //{
        //    instructionSet.Add(new Convr8());
        //}

        //public void EmitConvOvfI()
        //{
        //    instructionSet.Add(new Convovfi());
        //}

        //public void EmitConvOvfIUn()
        //{
        //    instructionSet.Add(new Convovfiun());
        //}

        //public void EmitConvOvfI1()
        //{
        //    instructionSet.Add(new Convovfi1());
        //}

        //public void EmitConvOvfI1Un()
        //{
        //    instructionSet.Add(new Convovfi1un());
        //}

        //public void EmitConvOvfi2()
        //{
        //    instructionSet.Add(new Convovfi2());
        //}

        //public void EmitConvOvfI2Un()
        //{
        //    instructionSet.Add(new Convovfi2un());
        //}

        //public void EmitConvOvfI4()
        //{
        //    instructionSet.Add(new Convovfi4());
        //}

        //public void EmitConvOvfI4Un()
        //{
        //    instructionSet.Add(new Convovfi4un());
        //}

        //public void EmitConvOvfI8()
        //{
        //    instructionSet.Add(new Convovfi8());
        //}

        //public void EmitConvOvfI8Un()
        //{
        //    instructionSet.Add(new Convovfi8un());
        //}

        //public void EmitConvOvfU()
        //{
        //    instructionSet.Add(new Convovfu());
        //}

        //public void EmitConvOvfUUn()
        //{
        //    instructionSet.Add(new Convovfuun());
        //}

        //public void EmitConvOvfU1()
        //{
        //    instructionSet.Add(new Convovfu1());
        //}

        //public void EmitConvOvfU1Un()
        //{
        //    instructionSet.Add(new Convovfu1un());
        //}

        //public void EmitConvOvfU2()
        //{
        //    instructionSet.Add(new Convovfu2());
        //}

        //public void EmitConvOvfU2Un()
        //{
        //    instructionSet.Add(new Convovfu2un());
        //}

        //public void EmitConvOvfU4()
        //{
        //    instructionSet.Add(new Convovfu4());
        //}

        //public void EmitConvOvfU4Un()
        //{
        //    instructionSet.Add(new Convovfu4un());
        //}

        //public void EmitConvOvfU8()
        //{
        //    instructionSet.Add(new Convovfu8());
        //}

        //public void EmitConvOvfU8Un()
        //{
        //    instructionSet.Add(new Convovfu8un());
        //}
        //#endregion
        //#endregion
    }
}
