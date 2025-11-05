using System;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;
using dotnow.Runtime.CIL;
using MonoInstruction = Mono.Cecil.Cil.Instruction;
using MethodBody = Mono.Cecil.Cil.MethodBody;

namespace dotnow.Runtime.JIT
{
    internal class MethodBodyCompiler
    {
        // Private
        private MethodBody methodBody = null;
        private ExecutableILGenerator il;
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

                // Attach instruction
                il.AttachMonoInstruction(current);
            }

            // Return the code generator
            return il;
        }

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
