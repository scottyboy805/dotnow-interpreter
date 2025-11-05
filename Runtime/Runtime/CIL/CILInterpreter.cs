using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace dotnow.Runtime.CIL
{
    internal static class CILInterpreter
    {
        // Methods
        public static int ExecuteMethod(ThreadContext threadContext, AssemblyLoadContext loadContext, CILMethodInfo method, int spArg)
        {
            // Check for interpreted
            if ((method.Flags & CILMethodFlags.Interpreted) == 0)
                throw new InvalidOperationException("Not supported for interop methods");

            // Get local
            int spLoc = spArg + ((method.Flags & CILMethodFlags.This) != 0 ? 1 : 0) + method.ParameterTypes.Length;

            // Get sp
            int sp = spLoc + method.LocalCount;

            // Get sp max
            int spMax = sp + method.MaxStack;

            // Check overflow - we'll handle this differently since we can't use unsafe
            if (spMax >= threadContext.stack.Length)
                throw new StackOverflowException();

            // Get the stack
            StackData[] stack = threadContext.stack;

            // Get the instructions
            byte[] instructions = method.Instructions;

            // Get program counter
            int pc = 0;
            int pcMax = instructions.Length;

            // Main execution loop
            while(pc < pcMax && threadContext.abort == false)
            {
                // Fetch the op code
                ILOpCode op = FetchDecode<ILOpCode>(instructions, ref pc);

                // Check for 2-byte encoded instructions
                if ((byte)op == 0xFE)
                    op = (ILOpCode)(((byte)op << 8) | FetchDecode<byte>(instructions, ref pc));

                // Switch (OpCode)
                switch (op)
                {
                    default:
                        throw new NotImplementedException("MSIL instruction is not implemented: " + op.ToString() + "\nAt method body: " + method.Method);

                    case ILOpCode.Nop:
                        {
                            Debug.Instruction(op, pc - 1);
                            break;
                        }

                    #region Stack
                    case ILOpCode.Dup:
                        {
                            stack[sp] = stack[sp - 1];
                            sp++;

                            Debug.Instruction(op, pc - 1);
                            break;
                        }
                    case ILOpCode.Pop:
                        {
                            sp--;

                            Debug.Instruction(op, pc - 1);
                            break;
                        }
                    #endregion

                    #region Constant
                    case ILOpCode.Ldstr:
                        {
                            // Get the token
                            int token = FetchDecode<int>(instructions, ref pc);

                            // Push to stack
                            stack[sp].Ref = loadContext.GetUserString(token);
                            stack[sp].Type = StackTypeCode.Ref;
                            sp++;

                            Debug.Instruction(op, pc - 5);
                            break;
                        }
                    case ILOpCode.Ldnull:
                        {
                            // Push null to stack
                            stack[sp].Ref = null;
                            stack[sp].Type = StackTypeCode.Ref;
                            sp++;

                            Debug.Instruction(op, pc - 1);
                            break;
                        }
                    case ILOpCode.Ldc_i4_s:
                        {
                            // Read the value
                            sbyte val = FetchDecode<sbyte>(instructions, ref pc);

                            // Push I4 to stack
                            stack[sp].I32 = val;
                            stack[sp].Type = StackTypeCode.I32;
                            sp++;

                            Debug.Instruction(op, pc - 2);
                            break;
                        }
                    case ILOpCode.Ldc_i4:
                        {
                            // Read the value
                            int val = FetchDecode<int>(instructions, ref pc);

                            // Push I4 to stack
                            stack[sp].I32 = val;
                            stack[sp].Type = StackTypeCode.I32;
                            sp++;

                            Debug.Instruction(op, pc - 5);
                            break;
                        }
                    case ILOpCode.Ldc_i8:
                        {
                            // Read the value
                            long val = FetchDecode<long>(instructions, ref pc);

                            // Push I8 to stack
                            stack[sp].I64 = val;
                            stack[sp].Type = StackTypeCode.I64;
                            sp++;

                            Debug.Instruction(op, pc - 9);
                            break;
                        }
                    case ILOpCode.Ldc_r4:
                        {
                            // Read the value
                            float val = FetchDecode<float>(instructions, ref pc);

                            // Push F4 to stack
                            stack[sp].F32 = val;
                            stack[sp].Type = StackTypeCode.F32;
                            sp++;

                            Debug.Instruction(op, pc - 5);
                            break;
                        }
                    case ILOpCode.Ldc_r8:
                        {
                            // Read the value
                            double val = FetchDecode<double>(instructions, ref pc);

                            // Push F8 to stack
                            stack[sp].F64 = val;
                            stack[sp].Type = StackTypeCode.F64;
                            sp++;

                            Debug.Instruction(op, pc - 9);
                            break;
                        }
                    case ILOpCode.Ldc_i4_0:
                        {
                            // Push I4 to stack
                            stack[sp].I32 = 0;
                            stack[sp].Type = StackTypeCode.I32;
                            sp++;

                            Debug.Instruction(op, pc - 1);
                            break;
                        }
                    case ILOpCode.Ldc_i4_1:
                        {
                            // Push I4 to stack
                            stack[sp].I32 = 1;
                            stack[sp].Type = StackTypeCode.I32;
                            sp++;

                            Debug.Instruction(op, pc - 1);
                            break;
                        }
                    case ILOpCode.Ldc_i4_2:
                        {
                            // Push I4 to stack
                            stack[sp].I32 = 2;
                            stack[sp].Type = StackTypeCode.I32;
                            sp++;

                            Debug.Instruction(op, pc - 1);
                            break;
                        }
                    case ILOpCode.Ldc_i4_3:
                        {
                            // Push I4 to stack
                            stack[sp].I32 = 3;
                            stack[sp].Type = StackTypeCode.I32;
                            sp++;

                            Debug.Instruction(op, pc - 1);
                            break;
                        }
                    case ILOpCode.Ldc_i4_4:
                        {
                            // Push I4 to stack
                            stack[sp].I32 = 4;
                            stack[sp].Type = StackTypeCode.I32;
                            sp++;

                            Debug.Instruction(op, pc - 1);
                            break;
                        }
                    case ILOpCode.Ldc_i4_5:
                        {
                            // Push I4 to stack
                            stack[sp].I32 = 5;
                            stack[sp].Type = StackTypeCode.I32;
                            sp++;

                            Debug.Instruction(op, pc - 1);
                            break;
                        }
                    case ILOpCode.Ldc_i4_6:
                        {
                            // Push I4 to stack
                            stack[sp].I32 = 6;
                            stack[sp].Type = StackTypeCode.I32;
                            sp++;

                            Debug.Instruction(op, pc - 1);
                            break;
                        }
                    case ILOpCode.Ldc_i4_7:
                        {
                            // Push I4 to stack
                            stack[sp].I32 = 7;
                            stack[sp].Type = StackTypeCode.I32;
                            sp++;

                            Debug.Instruction(op, pc - 1);
                            break;
                        }
                    case ILOpCode.Ldc_i4_8:
                        {
                            // Push I4 to stack
                            stack[sp].I32 = 8;
                            stack[sp].Type = StackTypeCode.I32;
                            sp++;

                            Debug.Instruction(op, pc - 1);
                            break;
                        }
                    case ILOpCode.Ldc_i4_m1:
                        {
                            // Push I4 to stack
                            stack[sp].I32 = -1;
                            stack[sp].Type = StackTypeCode.I32;
                            sp++;

                            Debug.Instruction(op, pc - 1);
                            break;
                        }
                    #endregion

                } // End switch
            } // End loop

            return sp; // Return the final stack pointer
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T FetchDecode<T>(byte[] instructions, ref int pc) where T : unmanaged
        {
            // Use ReadOnlySpan to avoid allocations and read directly from memory
            ReadOnlySpan<byte> span = new ReadOnlySpan<byte>(instructions, pc, Unsafe.SizeOf<T>());

            // Read as T
            T result = MemoryMarshal.Read<T>(span);

            // Increment pc offset
            pc += Unsafe.SizeOf<T>();
            return result;
        }
    }
}
