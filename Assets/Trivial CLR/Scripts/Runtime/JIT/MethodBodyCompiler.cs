
using System;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;
using TrivialCLR.Runtime.CIL;
//using Instruction = TrivialCLR.Runtime.CIL.Instruction;
using MonoInstruction = Mono.Cecil.Cil.Instruction;
using MethodBody = Mono.Cecil.Cil.MethodBody;

namespace TrivialCLR.Runtime.JIT
{
    internal class MethodBodyCompiler
    {
        // Private
        private MethodBody methodBody = null;
        private ExecutableILGenerator il;//= new ExecutableILGenerator();
        private Instruction[] instructionSet = null;
        private CILOperation[] operations = null;

        // Constructor
        public MethodBodyCompiler(MethodBody methodBody)
        {
            this.methodBody = methodBody;
        }

        // Methods
        public CILOperation[] JITOptimizeInterpretedInstructionSet(AppDomain domain)
        {
            if (operations != null)
                return operations;

            // Compile the instruction set
            il = EmitMonoInstructionSet(domain, methodBody.Instructions);

            // Build instructions
            operations = il.GetExecutableInstructions();

            return operations;
        }

        //public Instruction[] JITCompileMethodBodyToInstructionPtr(AppDomain domain)
        //{
        //    // Check for valid instruction
        //    if (instructionSet != null)
        //        return instructionSet;

        //    for(int i = 0; i < methodBody.Instructions.Count; i++)
        //    {
        //        // Emit the instruction
        //        EmitMonoInstruction(domain, methodBody.Instructions, methodBody.Instructions[i]);
        //    }

        //    // Build instruction set
        //    instructionSet = il.GetExecutableInstructionSet();

        //    return instructionSet;
        //}

        private ExecutableILGenerator EmitMonoInstructionSet(AppDomain domain, Collection<MonoInstruction> instructions)
        {
            int instructionSize = instructions.Count;

            // Create the instruction generator
            ExecutableILGenerator il = new ExecutableILGenerator(domain, instructionSize);

            // Process all instructions
            for (int i = 0; i < instructionSize; i++)
            {
                // Get the current instruction
                MonoInstruction current = instructions[i];
                Code currentCode = current.OpCode.Code;

                // Check for new
                if(currentCode == Code.Newobj)
                {
                    Type instanceType = domain.ResolveType(((MethodReference)current.Operand).DeclaringType);
                    MethodBase ctor = domain.ResolveMethodOrConstructor(current.Operand);

                    il.Emit(currentCode, ctor, instanceType);
                    continue;
                }

                // Build optimized instruction set just in time for execution
                switch(current.OpCode.OperandType)
                {
                    // No args
                    case OperandType.InlineNone:
                        {
                            il.Emit(currentCode);
                            break;
                        }


                    case OperandType.ShortInlineI:
                        {
                            il.Emit(currentCode, (int)(sbyte)current.Operand); 
                            break;
                        }
                    case OperandType.InlineI:
                        {
                            il.Emit(currentCode, (int)current.Operand); 
                            break;
                        }
                    case OperandType.InlineI8:
                        {
                            il.Emit(currentCode, (long)current.Operand); 
                            break;
                        }
                    case OperandType.ShortInlineR:
                        {
                            il.Emit(currentCode, (float)current.Operand); 
                            break;
                        }
                    case OperandType.InlineR:
                        {
                            il.Emit(currentCode, (double)current.Operand); 
                            break;
                        }
                    case OperandType.InlineString:
                        {
                            il.Emit(currentCode, (string)current.Operand); 
                            break;
                        }
                    case OperandType.InlineType:
                        {
                            il.Emit(currentCode, domain.ResolveType(current.Operand)); 
                            break;
                        }
                    case OperandType.InlineField:
                        {
                            il.Emit(currentCode, domain.ResolveField(current.Operand)); 
                            break;
                        }
                    case OperandType.InlineMethod:
                        {
                            il.Emit(currentCode, domain.ResolveMethodOrConstructor(current.Operand));
                            break;
                        }
                    case OperandType.InlineTok:
                        {
                            il.Emit(currentCode, domain.ResolveToken(current.Operand)); 
                            break;
                        }


                    case OperandType.ShortInlineArg:    
                    case OperandType.InlineArg:
                        {
                            il.Emit(currentCode, ((ParameterReference)current.Operand).Index); 
                            break;
                        }
                    case OperandType.ShortInlineVar:
                    case OperandType.InlineVar:
                        {
                            il.Emit(currentCode, ((VariableReference)current.Operand).Index); 
                            break;
                        }


                    case OperandType.ShortInlineBrTarget:
                        {
                            il.Emit(currentCode, GetInstructionOffset(instructions, current)); 
                            break;
                        }
                    case OperandType.InlineBrTarget:
                        {
                            il.Emit(currentCode, GetInstructionOffset(instructions, current)); 
                            break;
                        }
                    case OperandType.InlineSwitch:
                        {
                            il.Emit(currentCode, GetinstructionOffsets(instructions, current)); 
                            break;
                        }

                    default:
                        throw new NotSupportedException(string.Format("Op code '{0}' uses an unsupported operand type '{1}'", current, current.OpCode.OperandType));

                }
            }

            // Return the code generator
            return il;
        }

        //private void EmitMonoInstruction(AppDomain domain, Collection<MonoInstruction> instructions, MonoInstruction monoInstruction)
        //{
        //    switch (monoInstruction.OpCode.Code)
        //    {
        //        default:
        //            {
        //                Code c = monoInstruction.OpCode.Code;
        //                throw new NotImplementedException("MSIL instruction is not implemented: " + monoInstruction.ToString() + "\nAt method body: " + methodBody.Method);
        //            }

        //        // unsupported
        //        case Code.Nop: il.EmitNop(); break;

        //        // Stack
        //        case Code.Pop: il.EmitPop(); break;
        //        case Code.Dup: il.EmitDup(); break;

        //        // Call
        //        case Code.Ret: il.EmitRet(); break;
        //        case Code.Call: il.EmitCall(domain.ResolveMethodOrConstructor(monoInstruction.Operand)); break;
        //        case Code.Callvirt: il.EmitCallVirt(domain, domain.ResolveMethod(monoInstruction.Operand)); break;

        //        // ldftn
        //        case Code.Ldftn: il.EmitLdFtn(domain.ResolveMethodOrConstructor(monoInstruction.Operand)); break;

        //        // exception
        //        case Code.Throw: il.EmitThrow(); break;

        //        // arithmetic
        //        case Code.Add: il.EmitAdd(); break;
        //        case Code.Add_Ovf_Un:
        //        case Code.Add_Ovf: 
        //            il.EmitAddOvf(); break;

        //        case Code.Sub: il.EmitSub(); break;
        //        case Code.Sub_Ovf:
        //        case Code.Sub_Ovf_Un:
        //            il.EmitSubOvf(); break;

        //        case Code.Mul: il.EmitMul(); break;
        //        case Code.Mul_Ovf:
        //        case Code.Mul_Ovf_Un:
        //            il.EmitMulOvf(); break;

        //        case Code.Div:
        //        case Code.Div_Un:
        //            il.EmitDiv(); break;

        //        case Code.Neg: il.EmitNeg(); break;

        //        case Code.Rem: il.EmitRem(); break;
        //        case Code.Rem_Un: il.EmitRemUn(); break;
                    

        //        // Bitwise
        //        case Code.Shl: il.EmitShl(); break;
        //        case Code.Shr: il.EmitShr(); break;

        //        // ldloc
        //        case Code.Ldloc_0: il.EmitLdLoc(0); break;
        //        case Code.Ldloc_1: il.EmitLdLoc(1); break;
        //        case Code.Ldloc_2: il.EmitLdLoc(2); break;
        //        case Code.Ldloc_3: il.EmitLdLoc(3); break;
        //        case Code.Ldloc:
        //        case Code.Ldloc_S:
        //            il.EmitLdLoc(((VariableReference)monoInstruction.Operand).Index); break;
        //        case Code.Ldloca:
        //        case Code.Ldloca_S:
        //            il.EmitLdLocA(((VariableReference)monoInstruction.Operand).Index); break;
                
        //        // stloc
        //        case Code.Stloc_0: il.EmitStLoc(0); break;
        //        case Code.Stloc_1: il.EmitStLoc(1); break;
        //        case Code.Stloc_2: il.EmitStLoc(2); break;
        //        case Code.Stloc_3: il.EmitStLoc(3); break;
        //        case Code.Stloc:
        //        case Code.Stloc_S:
        //            il.EmitStLoc(((VariableReference)monoInstruction.Operand).Index); break;

        //        // ldarg
        //        case Code.Ldarg_0: il.EmitLdArg(0); break;
        //        case Code.Ldarg_1: il.EmitLdArg(1); break;
        //        case Code.Ldarg_2: il.EmitLdArg(2); break;
        //        case Code.Ldarg_3: il.EmitLdArg(3); break;
        //        case Code.Ldarg:
        //        case Code.Ldarg_S:
        //            il.EmitLdArg(((ParameterReference)monoInstruction.Operand).Index + ((methodBody.Method.IsStatic == true) ? 0 : 1)); break;

        //        case Code.Ldarga:
        //        case Code.Ldarga_S:
        //            il.EmitLdArga(((ParameterReference)monoInstruction.Operand).Index + ((methodBody.Method.IsStatic == true) ? 0 : 1)); break;
                    
        //        // ldc
        //        case Code.Ldc_I4_0: il.EmitLdObj(0); break;
        //        case Code.Ldc_I4_1: il.EmitLdObj(1); break;
        //        case Code.Ldc_I4_2: il.EmitLdObj(2); break;
        //        case Code.Ldc_I4_3: il.EmitLdObj(3); break;
        //        case Code.Ldc_I4_4: il.EmitLdObj(4); break;
        //        case Code.Ldc_I4_5: il.EmitLdObj(5); break;
        //        case Code.Ldc_I4_6: il.EmitLdObj(6); break;
        //        case Code.Ldc_I4_7: il.EmitLdObj(7); break;
        //        case Code.Ldc_I4_8: il.EmitLdObj(8); break;
        //        case Code.Ldc_I4_M1: il.EmitLdObj(-1); break;

        //        case Code.Ldc_I4: 
        //        case Code.Ldc_I4_S:
        //            il.EmitLdObj(Convert.ToInt32(monoInstruction.Operand)); break;

        //        case Code.Ldc_I8: il.EmitLdObj(Convert.ToInt64(monoInstruction.Operand)); break;
        //        case Code.Ldc_R4: il.EmitLdObj(Convert.ToSingle(monoInstruction.Operand)); break;
        //        case Code.Ldc_R8: il.EmitLdObj(Convert.ToDouble(monoInstruction.Operand)); break;

        //        // ldind
        //        case Code.Ldind_I: il.EmitLdIndI(); break;
        //        case Code.Ldind_I1: il.EmitLdIndI1(); break;
        //        case Code.Ldind_I2: il.EmitLdIndI2(); break;
        //        case Code.Ldind_I4: il.EmitLdIndI4(); break;
        //        case Code.Ldind_I8: il.EmitLdIndI8(); break;
        //        case Code.Ldind_R4: il.EmitLdIndR4(); break;
        //        case Code.Ldind_R8: il.EmitLdIndR8(); break;
        //        case Code.Ldind_U1: il.EmitLdIndU1(); break;
        //        case Code.Ldind_U2: il.EmitLdIndU2(); break;
        //        case Code.Ldind_U4: il.EmitLdIndU4(); break;
        //        case Code.Ldind_Ref: il.EmitLdIndRef(); break;

        //        // stind
        //        case Code.Stind_I: il.EmitStIndI(); break;
        //        case Code.Stind_I1: il.EmitStIndI1(); break;
        //        case Code.Stind_I2: il.EmitStIndI2(); break;
        //        case Code.Stind_I4: il.EmitStIndI4(); break;
        //        case Code.Stind_I8: il.EmitStIndI8(); break;
        //        case Code.Stind_R4: il.EmitStIndR4(); break;
        //        case Code.Stind_R8: il.EmitStIndR8(); break;
        //        case Code.Stind_Ref: il.EmitStIndRef(); break;

        //        // ldconst
        //        case Code.Ldnull: il.EmitLdObj(null); break;
        //        case Code.Ldstr: il.EmitLdObj((string)monoInstruction.Operand); break;

        //        // ldtoken
        //        case Code.Ldtoken: il.EmitLdToken(domain.ResolveToken(monoInstruction.Operand)); break;

        //        // starg
        //        case Code.Starg:
        //        case Code.Starg_S:
        //            il.EmitStArg(((ParameterReference)monoInstruction.Operand).Index); break;

        //        // ldfld
        //        case Code.Ldfld: il.EmitLdFld(domain.ResolveField(monoInstruction.Operand)); break;
        //        case Code.Ldsfld: il.EmitLdFld(domain.ResolveField(monoInstruction.Operand)); break;
        //        case Code.Ldflda: il.EmitLdFld(domain.ResolveField(monoInstruction.Operand), true); break;

        //        // stfld
        //        case Code.Stfld: il.EmitStFld(domain.ResolveField(monoInstruction.Operand)); break;
        //        case Code.Stsfld: il.EmitStFld(domain.ResolveField(monoInstruction.Operand)); break;


        //        // br
        //        case Code.Br:
        //        case Code.Br_S:
        //            il.EmitBr(GetInstructionOffset(instructions, monoInstruction)); break;

        //        // brtrue
        //        case Code.Brtrue:
        //        case Code.Brtrue_S:
        //            il.EmitBrTrue(GetInstructionOffset(instructions, monoInstruction)); break;

        //        // brfalse
        //        case Code.Brfalse:
        //        case Code.Brfalse_S:
        //            il.EmitBrFalse(GetInstructionOffset(instructions, monoInstruction)); break;

        //        // blt
        //        case Code.Blt:
        //        case Code.Blt_S:
        //        case Code.Blt_Un:
        //        case Code.Blt_Un_S:
        //            il.EmitBlt(GetInstructionOffset(instructions, monoInstruction)); break;

        //        // ble
        //        case Code.Ble:
        //        case Code.Ble_S:
        //        case Code.Ble_Un:
        //        case Code.Ble_Un_S:
        //            il.EmitBle(GetInstructionOffset(instructions, monoInstruction)); break;

        //        // bgt
        //        case Code.Bgt:
        //        case Code.Bgt_S:
        //        case Code.Bgt_Un:
        //        case Code.Bgt_Un_S:
        //            il.EmitBgt(GetInstructionOffset(instructions, monoInstruction)); break;

        //        // bge
        //        case Code.Bge:
        //        case Code.Bge_S:
        //        case Code.Bge_Un:
        //        case Code.Bge_Un_S:
        //            il.EmitBge(GetInstructionOffset(instructions, monoInstruction)); break;

        //        // beq
        //        case Code.Beq:
        //        case Code.Beq_S:
        //            il.EmitBeq(GetInstructionOffset(instructions, monoInstruction)); break;

        //        // bne
        //        case Code.Bne_Un:
        //        case Code.Bne_Un_S:
        //            il.EmitBne(GetInstructionOffset(instructions, monoInstruction)); break;

        //        // switch
        //        case Code.Switch: il.EmitSwitch(GetinstructionOffsets(instructions, monoInstruction)); break;

        //        // box
        //        case Code.Box: il.EmitBox(domain.ResolveType(monoInstruction.Operand)); break;

        //        // clt
        //        case Code.Clt:
        //        case Code.Clt_Un:
        //            il.EmitClt(); break;

        //        // cgt
        //        case Code.Cgt:
        //        case Code.Cgt_Un:
        //            il.EmitCgt(); break;

        //        // ceq
        //        case Code.Ceq: il.EmitCeq(); break;

        //        // newobj
        //        case Code.Newobj: il.EmitNew(domain, domain.ResolveType(((MethodReference)monoInstruction.Operand).DeclaringType), domain.ResolveConstructor(monoInstruction.Operand)); break;
        //        case Code.Initobj: il.EmitInitObj(); break;

        //        // isinst
        //        case Code.Isinst: il.EmitIsInst(domain.ResolveType(monoInstruction.Operand)); break;

        //        // array
        //        case Code.Newarr: il.EmitNewArr(domain.ResolveType(monoInstruction.Operand)); break;
                
                
        //        case Code.Ldlen: il.EmitLdLen(); break;

        //        // ldelem
        //        case Code.Ldelem_Any: il.EmitLdElem(domain.ResolveType(monoInstruction.Operand)); break;
        //        case Code.Ldelem_I: il.EmitLdElemI(); break;
        //        case Code.Ldelem_I1: il.EmitLdElemI1(); break;
        //        case Code.Ldelem_I2: il.EmitLdElemI2(); break;
        //        case Code.Ldelem_I4: il.EmitLdElemI4(); break;
        //        case Code.Ldelem_I8: il.EmitLdElemI8(); break;
        //        case Code.Ldelem_R4: il.EmitLdElemR4(); break;
        //        case Code.Ldelem_R8: il.EmitLdElemR8(); break;
        //        case Code.Ldelem_U1: il.EmitLdElemU1(); break;
        //        case Code.Ldelem_U2: il.EmitLdElemU2(); break;
        //        case Code.Ldelem_U4: il.EmitLdElemU4(); break;
        //        case Code.Ldelem_Ref: il.EmitLdElemRef(); break;
        //        case Code.Ldelema: il.EmitLdElemA(); break;

        //        // stelem
        //        case Code.Stelem_Any: il.EmitStElem(domain.ResolveType(monoInstruction.Operand)); break;
        //        case Code.Stelem_I: il.EmitStElemI(); break;
        //        case Code.Stelem_I1: il.EmitStElemI1(); break;
        //        case Code.Stelem_I2: il.EmitStElemI2(); break;
        //        case Code.Stelem_I4: il.EmitStElemI4(); break;
        //        case Code.Stelem_I8: il.EmitStElemI8(); break;
        //        case Code.Stelem_R4: il.EmitStElemR4(); break;
        //        case Code.Stelem_R8: il.EmitStElemR8(); break;
        //        case Code.Stelem_Ref: il.EmitStElemRef(); break;

        //        // convert
        //        case Code.Conv_I: il.EmitConvI(); break;
        //        case Code.Conv_I1: il.EmitConvI1(); break;
        //        case Code.Conv_I2: il.EmitConvI2(); break;
        //        case Code.Conv_I4: il.EmitConvI4(); break;
        //        case Code.Conv_I8: il.EmitConvI8(); break;
        //        case Code.Conv_U: il.EmitConvU(); break;
        //        case Code.Conv_U1: il.EmitConvU1(); break;
        //        case Code.Conv_U2: il.EmitConvU2(); break;
        //        case Code.Conv_U4: il.EmitConvU4(); break;
        //        case Code.Conv_U8: il.EmitConvU8(); break;
        //        case Code.Conv_R_Un: il.EmitConvRUn(); break;
        //        case Code.Conv_R4: il.EmitConvR4(); break;
        //        case Code.Conv_R8: il.EmitConvR8(); break;

        //        case Code.Conv_Ovf_I: il.EmitConvOvfI(); break;
        //        case Code.Conv_Ovf_I_Un: il.EmitConvOvfIUn(); break;
        //        case Code.Conv_Ovf_I1: il.EmitConvOvfI1(); break;
        //        case Code.Conv_Ovf_I1_Un: il.EmitConvOvfI1Un(); break;
        //        case Code.Conv_Ovf_I2: il.EmitConvOvfi2(); break;
        //        case Code.Conv_Ovf_I2_Un: il.EmitConvOvfI2Un(); break;
        //        case Code.Conv_Ovf_I4: il.EmitConvOvfI4(); break;
        //        case Code.Conv_Ovf_I4_Un: il.EmitConvOvfI4Un(); break;
        //        case Code.Conv_Ovf_I8: il.EmitConvOvfI8(); break;
        //        case Code.Conv_Ovf_I8_Un: il.EmitConvOvfI8Un(); break;
        //        case Code.Conv_Ovf_U: il.EmitConvOvfU(); break;
        //        case Code.Conv_Ovf_U_Un: il.EmitConvOvfUUn(); break;
        //        case Code.Conv_Ovf_U1: il.EmitConvOvfU1(); break;
        //        case Code.Conv_Ovf_U1_Un: il.EmitConvOvfU1Un(); break;
        //        case Code.Conv_Ovf_U2: il.EmitConvOvfU2(); break;
        //        case Code.Conv_Ovf_U2_Un: il.EmitConvOvfU2Un(); break;
        //        case Code.Conv_Ovf_U4: il.EmitConvOvfU4(); break;
        //        case Code.Conv_Ovf_U4_Un: il.EmitConvOvfU4Un(); break;
        //        case Code.Conv_Ovf_U8: il.EmitConvOvfU8(); break;
        //        case Code.Conv_Ovf_U8_Un: il.EmitConvOvfU8Un(); break;
        //    }
        //}

        private int GetInstructionOffset(Collection<MonoInstruction> instructions, MonoInstruction instruction)
        {
            // Get branch target
            MonoInstruction branchTarget = (MonoInstruction)instruction.Operand;

            // Calcualte instruction indexes
            int currentIndex = instructions.IndexOf(instruction);
            int targetIndex = instructions.IndexOf(branchTarget);

            // Calcualte relative offset required to jump to target instruction
            return (targetIndex - currentIndex);
        }

        private int[] GetinstructionOffsets(Collection<MonoInstruction> instructions, MonoInstruction instruction)
        {
            // Get branch target
            MonoInstruction[] switchTargets = (MonoInstruction[])instruction.Operand;

            // Allocate return array
            int[] instructionOffsets = new int[switchTargets.Length];

            for(int i = 0; i < switchTargets.Length; i++)
            {
                // Calcualte instruction indexes
                int currentIndex = instructions.IndexOf(instruction);
                int targetIndex = instructions.IndexOf(switchTargets[i]);

                // Calcualte relative instruction offset
                instructionOffsets[i] = (targetIndex - currentIndex);
            }

            return instructionOffsets;
        }
    }
}
