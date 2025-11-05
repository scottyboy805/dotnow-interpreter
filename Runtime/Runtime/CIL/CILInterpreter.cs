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
            while (pc < pcMax && threadContext.abort == false)
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
                            stack[sp].Type = StackType.Ref;
                            sp++;

                            Debug.Instruction(op, pc - 5);
                            break;
                        }
                    case ILOpCode.Ldnull:
                        {
                            // Push null to stack
                            stack[sp].Ref = null;
                            stack[sp].Type = StackType.Ref;
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
                            stack[sp].Type = StackType.I32;
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
                            stack[sp].Type = StackType.I32;
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
                            stack[sp].Type = StackType.I64;
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
                            stack[sp].Type = StackType.F32;
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
                            stack[sp].Type = StackType.F64;
                            sp++;

                            Debug.Instruction(op, pc - 9);
                            break;
                        }
                    case ILOpCode.Ldc_i4_0:
                        {
                            // Push I4 to stack
                            stack[sp].I32 = 0;
                            stack[sp].Type = StackType.I32;
                            sp++;

                            Debug.Instruction(op, pc - 1);
                            break;
                        }
                    case ILOpCode.Ldc_i4_1:
                        {
                            // Push I4 to stack
                            stack[sp].I32 = 1;
                            stack[sp].Type = StackType.I32;
                            sp++;

                            Debug.Instruction(op, pc - 1);
                            break;
                        }
                    case ILOpCode.Ldc_i4_2:
                        {
                            // Push I4 to stack
                            stack[sp].I32 = 2;
                            stack[sp].Type = StackType.I32;
                            sp++;

                            Debug.Instruction(op, pc - 1);
                            break;
                        }
                    case ILOpCode.Ldc_i4_3:
                        {
                            // Push I4 to stack
                            stack[sp].I32 = 3;
                            stack[sp].Type = StackType.I32;
                            sp++;

                            Debug.Instruction(op, pc - 1);
                            break;
                        }
                    case ILOpCode.Ldc_i4_4:
                        {
                            // Push I4 to stack
                            stack[sp].I32 = 4;
                            stack[sp].Type = StackType.I32;
                            sp++;

                            Debug.Instruction(op, pc - 1);
                            break;
                        }
                    case ILOpCode.Ldc_i4_5:
                        {
                            // Push I4 to stack
                            stack[sp].I32 = 5;
                            stack[sp].Type = StackType.I32;
                            sp++;

                            Debug.Instruction(op, pc - 1);
                            break;
                        }
                    case ILOpCode.Ldc_i4_6:
                        {
                            // Push I4 to stack
                            stack[sp].I32 = 6;
                            stack[sp].Type = StackType.I32;
                            sp++;

                            Debug.Instruction(op, pc - 1);
                            break;
                        }
                    case ILOpCode.Ldc_i4_7:
                        {
                            // Push I4 to stack
                            stack[sp].I32 = 7;
                            stack[sp].Type = StackType.I32;
                            sp++;

                            Debug.Instruction(op, pc - 1);
                            break;
                        }
                    case ILOpCode.Ldc_i4_8:
                        {
                            // Push I4 to stack
                            stack[sp].I32 = 8;
                            stack[sp].Type = StackType.I32;
                            sp++;

                            Debug.Instruction(op, pc - 1);
                            break;
                        }
                    case ILOpCode.Ldc_i4_m1:
                        {
                            // Push I4 to stack
                            stack[sp].I32 = -1;
                            stack[sp].Type = StackType.I32;
                            sp++;

                            Debug.Instruction(op, pc - 1);
                            break;
                        }
                    #endregion

                    #region Argument
                    case ILOpCode.Ldarg_0:
                        {
                            // Copy from arg offset
                            stack[sp] = stack[spArg];
                            sp++;

                            Debug.Instruction(op, pc - 1, stack[sp - 1]);
                            break;
                        }
                    case ILOpCode.Ldarg_1:
                        {
                            // Copy from arg offset
                            stack[sp] = stack[spArg + 1];
                            sp++;

                            Debug.Instruction(op, pc - 1, stack[sp - 1]);
                            break;
                        }
                    case ILOpCode.Ldarg_2:
                        {
                            // Copy from arg offset
                            stack[sp] = stack[spArg + 2];
                            sp++;

                            Debug.Instruction(op, pc - 1, stack[sp - 1]);
                            break;
                        }
                    case ILOpCode.Ldarg_3:
                        {
                            // Copy from arg offset
                            stack[sp] = stack[spArg + 3];
                            sp++;

                            Debug.Instruction(op, pc - 1, stack[sp - 1]);
                            break;
                        }
                    case ILOpCode.Ldarg_s:
                        {
                            // Read the offset
                            sbyte offset = FetchDecode<sbyte>(instructions, ref pc);

                            // Copy from arg offset
                            stack[sp] = stack[spArg + offset];
                            sp++;

                            Debug.Instruction(op, pc - 2, stack[sp - 1]);
                            break;
                        }
                    case ILOpCode.Ldarg:
                        {
                            // Read the offset
                            int offset = FetchDecode<int>(instructions, ref pc);

                            // Copy from arg offset
                            stack[sp] = stack[spArg + offset];
                            sp++;

                            Debug.Instruction(op, pc - 5, stack[sp - 1]);
                            break;
                        }

                    case ILOpCode.Starg_s:
                        {
                            // Read the offset
                            sbyte offset = FetchDecode<sbyte>(instructions, ref pc);

                            // Copy from stack to arg offset
                            stack[spArg + offset] = stack[--sp];

                            Debug.Instruction(op, pc - 2, stack[spArg + offset]);
                            break;
                        }
                    case ILOpCode.Starg:
                        {
                            // Read the offset
                            int offset = FetchDecode<int>(instructions, ref pc);

                            // Copy from stack to arg offset
                            stack[spArg + offset] = stack[--sp];

                            Debug.Instruction(op, pc - 5, stack[spArg + offset]);
                            break;
                        }
                    #endregion

                    #region Local
                    case ILOpCode.Ldloc_0:
                        {
                            // Copy from local offset to stack
                            stack[sp] = stack[spLoc];
                            sp++;

                            Debug.Instruction(op, pc - 1, stack[sp - 1]);
                            break;
                        }
                    case ILOpCode.Ldloc_1:
                        {
                            // Copy from local offset to stack
                            stack[sp] = stack[spLoc + 1];
                            sp++;

                            Debug.Instruction(op, pc - 1, stack[sp - 1]);
                            break;
                        }
                    case ILOpCode.Ldloc_2:
                        {
                            // Copy from local offset to stack
                            stack[sp] = stack[spLoc + 2];
                            sp++;

                            Debug.Instruction(op, pc - 1, stack[sp - 1]);
                            break;
                        }
                    case ILOpCode.Ldloc_3:
                        {
                            // Copy from local offset to stack
                            stack[sp] = stack[spLoc + 3];
                            sp++;

                            Debug.Instruction(op, pc - 1, stack[sp - 1]);
                            break;
                        }
                    case ILOpCode.Ldloc_s:
                        {
                            // Fetch offset
                            sbyte offset = FetchDecode<sbyte>(instructions, ref pc);

                            // Copy from local offset to stack
                            stack[sp] = stack[spLoc + offset];
                            sp++;

                            Debug.Instruction(op, pc - 2, stack[sp - 1]);
                            break;
                        }
                    case ILOpCode.Ldloc:
                        {
                            // Fetch offset
                            int offset = FetchDecode<int>(instructions, ref pc);

                            // Copy from local offset to stack
                            stack[sp] = stack[spLoc + offset];
                            sp++;

                            Debug.Instruction(op, pc - 5, stack[sp - 1]);
                            break;
                        }

                    case ILOpCode.Stloc_0:
                        {
                            // Copy from stack to local offset
                            stack[spLoc] = stack[--sp];

                            Debug.Instruction(op, pc - 1, stack[spLoc]);
                            break;
                        }
                    case ILOpCode.Stloc_1:
                        {
                            // Copy from stack to local offset
                            stack[spLoc + 1] = stack[--sp];

                            Debug.Instruction(op, pc - 1, stack[spLoc]);
                            break;
                        }
                    case ILOpCode.Stloc_2:
                        {
                            // Copy from stack to local offset
                            stack[spLoc + 2] = stack[--sp];

                            Debug.Instruction(op, pc - 1, stack[spLoc]);
                            break;
                        }
                    case ILOpCode.Stloc_3:
                        {
                            // Copy from stack to local offset
                            stack[spLoc + 3] = stack[--sp];

                            Debug.Instruction(op, pc - 1, stack[spLoc]);
                            break;
                        }
                    case ILOpCode.Stloc_s:
                        {
                            // Fetch offset
                            sbyte offset = FetchDecode<sbyte>(instructions, ref pc);

                            // Copy from stack to local offset
                            stack[spLoc + offset] = stack[--sp];

                            Debug.Instruction(op, pc - 1, stack[spLoc]);
                            break;
                        }
                    case ILOpCode.Stloc:
                        {
                            // Fetch offset
                            int offset = FetchDecode<int>(instructions, ref pc);

                            // Copy from stack to local offset
                            stack[spLoc + offset] = stack[--sp];

                            Debug.Instruction(op, pc - 1, stack[spLoc]);
                            break;
                        }
                    #endregion

                    #region Compare
                    case ILOpCode.Ceq:
                        {
                            // Decrement ptr
                            sp--;

                            // Check type
                            switch (stack[sp].Type)
                            {
                                default: throw new NotSupportedException(stack[sp].Type.ToString());

                                case StackType.I32: stack[sp - 1].I32 = stack[sp - 1].I32 == stack[sp].I32 ? 1 : 0; break;
                                case StackType.U32: stack[sp - 1].I32 = (uint)stack[sp - 1].I32 == (uint)stack[sp].I32 ? 1 : 0; break;
                                case StackType.I64: stack[sp - 1].I32 = stack[sp - 1].I64 == stack[sp].I64 ? 1 : 0; break;
                                case StackType.U64: stack[sp - 1].I32 = (ulong)stack[sp - 1].I64 == (ulong)stack[sp].I64 ? 1 : 0; break;
                                case StackType.Ptr: stack[sp - 1].I32 = stack[sp - 1].Ptr == stack[sp].Ptr ? 1 : 0; break;
                                case StackType.UPtr: stack[sp - 1].I32 = (UIntPtr)(long)stack[sp - 1].Ptr == (UIntPtr)(long)stack[sp].Ptr ? 1 : 0; break;
                                case StackType.F32: stack[sp - 1].I32 = stack[sp - 1].F32 == stack[sp].F32 ? 1 : 0; break;
                                case StackType.F64: stack[sp - 1].I32 = stack[sp - 1].F64 == stack[sp].F64 ? 1 : 0; break;
                                case StackType.Ref: stack[sp - 1].I32 = stack[sp - 1].Ref == stack[sp].Ref ? 1 : 0; break;
                            }

                            // Set type to boolean - I32 on stack
                            stack[sp - 1].Type = StackType.I32;

                            Debug.Instruction(op, pc - 2, stack[sp - 1]);
                            break;
                        }

                    case ILOpCode.Cgt:
                        {
                            // Decrement ptr
                            sp--;

                            // Check type - signed greater than
                            switch (stack[sp].Type)
                            {
                                default: throw new NotSupportedException(stack[sp].Type.ToString());

                                case StackType.I32: stack[sp - 1].I32 = stack[sp - 1].I32 > stack[sp].I32 ? 1 : 0; break;
                                case StackType.U32: stack[sp - 1].I32 = stack[sp - 1].I32 > stack[sp].I32 ? 1 : 0; break; // Treat as signed
                                case StackType.I64: stack[sp - 1].I32 = stack[sp - 1].I64 > stack[sp].I64 ? 1 : 0; break;
                                case StackType.U64: stack[sp - 1].I32 = stack[sp - 1].I64 > stack[sp].I64 ? 1 : 0; break; // Treat as signed
                                case StackType.Ptr: stack[sp - 1].I32 = (long)stack[sp - 1].Ptr > (long)stack[sp].Ptr ? 1 : 0; break;
                                case StackType.UPtr: stack[sp - 1].I32 = (long)stack[sp - 1].Ptr > (long)stack[sp].Ptr ? 1 : 0; break;
                                case StackType.F32: stack[sp - 1].I32 = stack[sp - 1].F32 > stack[sp].F32 ? 1 : 0; break;
                                case StackType.F64: stack[sp - 1].I32 = stack[sp - 1].F64 > stack[sp].F64 ? 1 : 0; break;
                                case StackType.Ref: stack[sp - 1].I32 = 0; break; // References cannot be compared with >
                            }

                            // Set type to boolean - I32 on stack
                            stack[sp - 1].Type = StackType.I32;

                            Debug.Instruction(op, pc - 2, stack[sp - 1]);
                            break;
                        }

                    case ILOpCode.Cgt_un:
                        {
                            // Decrement ptr
                            sp--;

                            // Check type - unsigned greater than
                            switch (stack[sp].Type)
                            {
                                default: throw new NotSupportedException(stack[sp].Type.ToString());

                                case StackType.I32: stack[sp - 1].I32 = (uint)stack[sp - 1].I32 > (uint)stack[sp].I32 ? 1 : 0; break;
                                case StackType.U32: stack[sp - 1].I32 = (uint)stack[sp - 1].I32 > (uint)stack[sp].I32 ? 1 : 0; break;
                                case StackType.I64: stack[sp - 1].I32 = (ulong)stack[sp - 1].I64 > (ulong)stack[sp].I64 ? 1 : 0; break;
                                case StackType.U64: stack[sp - 1].I32 = (ulong)stack[sp - 1].I64 > (ulong)stack[sp].I64 ? 1 : 0; break;
                                case StackType.Ptr: stack[sp - 1].I32 = (ulong)(long)stack[sp - 1].Ptr > (ulong)(long)stack[sp].Ptr ? 1 : 0; break;
                                case StackType.UPtr: stack[sp - 1].I32 = (ulong)(long)stack[sp - 1].Ptr > (ulong)(long)stack[sp].Ptr ? 1 : 0; break;
                                case StackType.F32: stack[sp - 1].I32 = !(stack[sp - 1].F32 <= stack[sp].F32) ? 1 : 0; break; // Handle NaN properly
                                case StackType.F64: stack[sp - 1].I32 = !(stack[sp - 1].F64 <= stack[sp].F64) ? 1 : 0; break; // Handle NaN properly
                                case StackType.Ref: stack[sp - 1].I32 = 0; break; // References cannot be compared with >
                            }

                            // Set type to boolean - I32 on stack
                            stack[sp - 1].Type = StackType.I32;

                            Debug.Instruction(op, pc - 2, stack[sp - 1]);
                            break;
                        }

                    case ILOpCode.Clt:
                        {
                            // Decrement ptr
                            sp--;

                            // Check type - signed less than
                            switch (stack[sp].Type)
                            {
                                default: throw new NotSupportedException(stack[sp].Type.ToString());

                                case StackType.I32: stack[sp - 1].I32 = stack[sp - 1].I32 < stack[sp].I32 ? 1 : 0; break;
                                case StackType.U32: stack[sp - 1].I32 = stack[sp - 1].I32 < stack[sp].I32 ? 1 : 0; break; // Treat as signed
                                case StackType.I64: stack[sp - 1].I32 = stack[sp - 1].I64 < stack[sp].I64 ? 1 : 0; break;
                                case StackType.U64: stack[sp - 1].I32 = stack[sp - 1].I64 < stack[sp].I64 ? 1 : 0; break; // Treat as signed
                                case StackType.Ptr: stack[sp - 1].I32 = (long)stack[sp - 1].Ptr < (long)stack[sp].Ptr ? 1 : 0; break;
                                case StackType.UPtr: stack[sp - 1].I32 = (long)stack[sp - 1].Ptr < (long)stack[sp].Ptr ? 1 : 0; break;
                                case StackType.F32: stack[sp - 1].I32 = stack[sp - 1].F32 < stack[sp].F32 ? 1 : 0; break;
                                case StackType.F64: stack[sp - 1].I32 = stack[sp - 1].F64 < stack[sp].F64 ? 1 : 0; break;
                                case StackType.Ref: stack[sp - 1].I32 = 0; break; // References cannot be compared with <
                            }

                            // Set type to boolean - I32 on stack
                            stack[sp - 1].Type = StackType.I32;

                            Debug.Instruction(op, pc - 2, stack[sp - 1]);
                            break;
                        }

                    case ILOpCode.Clt_un:
                        {
                            // Decrement ptr
                            sp--;

                            // Check type - unsigned less than
                            switch (stack[sp].Type)
                            {
                                default: throw new NotSupportedException(stack[sp].Type.ToString());

                                case StackType.I32: stack[sp - 1].I32 = (uint)stack[sp - 1].I32 < (uint)stack[sp].I32 ? 1 : 0; break;
                                case StackType.U32: stack[sp - 1].I32 = (uint)stack[sp - 1].I32 < (uint)stack[sp].I32 ? 1 : 0; break;
                                case StackType.I64: stack[sp - 1].I32 = (ulong)stack[sp - 1].I64 < (ulong)stack[sp].I64 ? 1 : 0; break;
                                case StackType.U64: stack[sp - 1].I32 = (ulong)stack[sp - 1].I64 < (ulong)stack[sp].I64 ? 1 : 0; break;
                                case StackType.Ptr: stack[sp - 1].I32 = (ulong)(long)stack[sp - 1].Ptr < (ulong)(long)stack[sp].Ptr ? 1 : 0; break;
                                case StackType.UPtr: stack[sp - 1].I32 = (ulong)(long)stack[sp - 1].Ptr < (ulong)(long)stack[sp].Ptr ? 1 : 0; break;
                                case StackType.F32: stack[sp - 1].I32 = !(stack[sp - 1].F32 >= stack[sp].F32) ? 1 : 0; break; // Handle NaN properly
                                case StackType.F64: stack[sp - 1].I32 = !(stack[sp - 1].F64 >= stack[sp].F64) ? 1 : 0; break; // Handle NaN properly
                                case StackType.Ref: stack[sp - 1].I32 = 0; break; // References cannot be compared with <
                            }

                            // Set type to boolean - I32 on stack
                            stack[sp - 1].Type = StackType.I32;

                            Debug.Instruction(op, pc - 2, stack[sp - 1]);
                            break;
                        }
                    #endregion

                    #region Arithmetic
                    case ILOpCode.Add:
                        {
                            // Decrement ptr
                            sp--;

                            unchecked
                            {
                                // Check type
                                switch (stack[sp].Type)
                                {
                                    default: throw new NotSupportedException(stack[sp].Type.ToString());

                                    case StackType.I32: stack[sp - 1].I32 = stack[sp - 1].I32 + stack[sp].I32; break;
                                    case StackType.U32: stack[sp - 1].I32 = (int)((uint)stack[sp - 1].I32 + (uint)stack[sp].I32); break;
                                    case StackType.I64: stack[sp - 1].I64 = stack[sp - 1].I64 + stack[sp].I64; break;
                                    case StackType.U64: stack[sp - 1].I64 = (long)((ulong)stack[sp - 1].I64 + (ulong)stack[sp].I64); break;
                                    case StackType.Ptr: stack[sp - 1].Ptr = (IntPtr)((long)stack[sp - 1].Ptr + (long)stack[sp].Ptr); break;
                                    case StackType.UPtr: stack[sp - 1].Ptr = (IntPtr)((ulong)stack[sp - 1].Ptr + (ulong)stack[sp].Ptr); break;
                                    case StackType.F32: stack[sp - 1].F32 = stack[sp - 1].F32 + stack[sp].F32; break;
                                    case StackType.F64: stack[sp - 1].F64 = stack[sp - 1].F64 + stack[sp].F64; break;
                                }
                            }

                            Debug.Instruction(op, pc - 1, stack[sp - 1]);
                            break;
                        }

                    case ILOpCode.Sub:
                        {
                            // Decrement ptr
                            sp--;

                            unchecked
                            {
                                // Check type
                                switch (stack[sp].Type)
                                {
                                    default: throw new NotSupportedException(stack[sp].Type.ToString());

                                    case StackType.I32: stack[sp - 1].I32 = stack[sp - 1].I32 - stack[sp].I32; break;
                                    case StackType.U32: stack[sp - 1].I32 = (int)((uint)stack[sp - 1].I32 - (uint)stack[sp].I32); break;
                                    case StackType.I64: stack[sp - 1].I64 = stack[sp - 1].I64 - stack[sp].I64; break;
                                    case StackType.U64: stack[sp - 1].I64 = (long)((ulong)stack[sp - 1].I64 - (ulong)stack[sp].I64); break;
                                    case StackType.Ptr: stack[sp - 1].Ptr = (IntPtr)((long)stack[sp - 1].Ptr - (long)stack[sp].Ptr); break;
                                    case StackType.UPtr: stack[sp - 1].Ptr = (IntPtr)((ulong)stack[sp - 1].Ptr - (ulong)stack[sp].Ptr); break;
                                    case StackType.F32: stack[sp - 1].F32 = stack[sp - 1].F32 - stack[sp].F32; break;
                                    case StackType.F64: stack[sp - 1].F64 = stack[sp - 1].F64 - stack[sp].F64; break;
                                }
                            }

                            Debug.Instruction(op, pc - 1, stack[sp - 1]);
                            break;
                        }

                    case ILOpCode.Mul:
                        {
                            // Decrement ptr
                            sp--;

                            unchecked
                            {
                                // Check type
                                switch (stack[sp].Type)
                                {
                                    default: throw new NotSupportedException(stack[sp].Type.ToString());

                                    case StackType.I32: stack[sp - 1].I32 = stack[sp - 1].I32 * stack[sp].I32; break;
                                    case StackType.U32: stack[sp - 1].I32 = (int)((uint)stack[sp - 1].I32 * (uint)stack[sp].I32); break;
                                    case StackType.I64: stack[sp - 1].I64 = stack[sp - 1].I64 * stack[sp].I64; break;
                                    case StackType.U64: stack[sp - 1].I64 = (long)((ulong)stack[sp - 1].I64 * (ulong)stack[sp].I64); break;
                                    case StackType.F32: stack[sp - 1].F32 = stack[sp - 1].F32 * stack[sp].F32; break;
                                    case StackType.F64: stack[sp - 1].F64 = stack[sp - 1].F64 * stack[sp].F64; break;
                                }
                            }

                            Debug.Instruction(op, pc - 1, stack[sp - 1]);
                            break;
                        }

                    case ILOpCode.Div:
                        {
                            // Decrement ptr
                            sp--;

                            unchecked
                            {
                                // Check type - signed division
                                switch (stack[sp].Type)
                                {
                                    default: throw new NotSupportedException(stack[sp].Type.ToString());

                                    case StackType.I32:
                                        {
                                            // Check for divide by zero
                                            if (stack[sp].I32 == 0) 
                                                throw new DivideByZeroException();

                                            stack[sp - 1].I32 = stack[sp - 1].I32 / stack[sp].I32;
                                            break;
                                        }
                                    case StackType.U32:
                                        {
                                            // Check for divide by zero
                                            if (stack[sp].I32 == 0) 
                                                throw new DivideByZeroException();

                                            stack[sp - 1].I32 = stack[sp - 1].I32 / stack[sp].I32; // Treat as signed
                                            break;
                                        }
                                    case StackType.I64:
                                        {
                                            // Check for divide by zero
                                            if (stack[sp].I64 == 0) 
                                                throw new DivideByZeroException();

                                            stack[sp - 1].I64 = stack[sp - 1].I64 / stack[sp].I64;
                                            break;
                                        }
                                    case StackType.U64:
                                        {
                                            // Check for divide by zero
                                            if (stack[sp].I64 == 0) 
                                                throw new DivideByZeroException();

                                            stack[sp - 1].I64 = stack[sp - 1].I64 / stack[sp].I64; // Treat as signed
                                            break;
                                        }
                                    case StackType.F32: stack[sp - 1].F32 = stack[sp - 1].F32 / stack[sp].F32; break;
                                    case StackType.F64: stack[sp - 1].F64 = stack[sp - 1].F64 / stack[sp].F64; break;
                                }
                            }

                            Debug.Instruction(op, pc - 1, stack[sp - 1]);
                            break;
                        }

                    case ILOpCode.Div_un:
                        {
                            // Decrement ptr
                            sp--;

                            unchecked
                            {
                                // Check type - unsigned division
                                switch (stack[sp].Type)
                                {
                                    default: throw new NotSupportedException(stack[sp].Type.ToString());

                                    case StackType.I32:
                                        {
                                            // Check for divide by zero
                                            if ((uint)stack[sp].I32 == 0) 
                                                throw new DivideByZeroException();

                                            stack[sp - 1].I32 = (int)((uint)stack[sp - 1].I32 / (uint)stack[sp].I32);
                                            break;
                                        }
                                    case StackType.U32:
                                        {
                                            // Check for divide by zero
                                            if ((uint)stack[sp].I32 == 0)
                                                throw new DivideByZeroException();

                                            stack[sp - 1].I32 = (int)((uint)stack[sp - 1].I32 / (uint)stack[sp].I32);
                                            break;
                                        }
                                    case StackType.I64:
                                        {
                                            // Check for divide by zero
                                            if ((ulong)stack[sp].I64 == 0) 
                                                throw new DivideByZeroException();

                                            stack[sp - 1].I64 = (long)((ulong)stack[sp - 1].I64 / (ulong)stack[sp].I64);
                                            break;
                                        }
                                    case StackType.U64:
                                        {
                                            // Check for divide by zero
                                            if ((ulong)stack[sp].I64 == 0) 
                                                throw new DivideByZeroException();

                                            stack[sp - 1].I64 = (long)((ulong)stack[sp - 1].I64 / (ulong)stack[sp].I64);
                                            break;
                                        }
                                    case StackType.F32: stack[sp - 1].F32 = stack[sp - 1].F32 / stack[sp].F32; break;
                                    case StackType.F64: stack[sp - 1].F64 = stack[sp - 1].F64 / stack[sp].F64; break;
                                }
                            }

                            Debug.Instruction(op, pc - 1, stack[sp - 1]);
                            break;
                        }

                    case ILOpCode.Rem:
                        {
                            // Decrement ptr
                            sp--;

                            unchecked
                            {
                                // Check type - signed remainder
                                switch (stack[sp].Type)
                                {
                                    default: throw new NotSupportedException(stack[sp].Type.ToString());

                                    case StackType.I32:
                                        {
                                            // Check for divide by zero
                                            if (stack[sp].I32 == 0) 
                                                throw new DivideByZeroException();

                                            stack[sp - 1].I32 = stack[sp - 1].I32 % stack[sp].I32;
                                            break;
                                        }
                                    case StackType.U32:
                                        {
                                            // Check for divide by zero
                                            if (stack[sp].I32 == 0) 
                                                throw new DivideByZeroException();

                                            stack[sp - 1].I32 = stack[sp - 1].I32 % stack[sp].I32; // Treat as signed
                                            break;
                                        }
                                    case StackType.I64:
                                        {
                                            // Check for divide by zero
                                            if (stack[sp].I64 == 0) 
                                                throw new DivideByZeroException();

                                            stack[sp - 1].I64 = stack[sp - 1].I64 % stack[sp].I64;
                                            break;
                                        }
                                    case StackType.U64:
                                        {
                                            // Check for divide by zero
                                            if (stack[sp].I64 == 0) 
                                                throw new DivideByZeroException();

                                            stack[sp - 1].I64 = stack[sp - 1].I64 % stack[sp].I64; // Treat as signed
                                            break;
                                        }
                                    case StackType.F32: stack[sp - 1].F32 = stack[sp - 1].F32 % stack[sp].F32; break;
                                    case StackType.F64: stack[sp - 1].F64 = stack[sp - 1].F64 % stack[sp].F64; break;
                                }
                            }

                            Debug.Instruction(op, pc - 1, stack[sp - 1]);
                            break;
                        }

                    case ILOpCode.Rem_un:
                        {
                            // Decrement ptr
                            sp--;

                            unchecked
                            {
                                // Check type - unsigned remainder
                                switch (stack[sp].Type)
                                {
                                    default: throw new NotSupportedException(stack[sp].Type.ToString());

                                    case StackType.I32:
                                        {
                                            // Check for divide by zero
                                            if ((uint)stack[sp].I32 == 0) 
                                                throw new DivideByZeroException();

                                            stack[sp - 1].I32 = (int)((uint)stack[sp - 1].I32 % (uint)stack[sp].I32);
                                            break;
                                        }
                                    case StackType.U32:
                                        {
                                            // Check for divide by zero
                                            if ((uint)stack[sp].I32 == 0) 
                                                throw new DivideByZeroException();

                                            stack[sp - 1].I32 = (int)((uint)stack[sp - 1].I32 % (uint)stack[sp].I32);
                                            break;
                                        }
                                    case StackType.I64:
                                        {
                                            // Check for divide by zero
                                            if ((ulong)stack[sp].I64 == 0) 
                                                throw new DivideByZeroException();

                                            stack[sp - 1].I64 = (long)((ulong)stack[sp - 1].I64 % (ulong)stack[sp].I64);
                                            break;
                                        }
                                    case StackType.U64:
                                        {
                                            // Check for divide by zero
                                            if ((ulong)stack[sp].I64 == 0) 
                                                throw new DivideByZeroException();

                                            stack[sp - 1].I64 = (long)((ulong)stack[sp - 1].I64 % (ulong)stack[sp].I64);
                                            break;
                                        }
                                    case StackType.F32: stack[sp - 1].F32 = stack[sp - 1].F32 % stack[sp].F32; break;
                                    case StackType.F64: stack[sp - 1].F64 = stack[sp - 1].F64 % stack[sp].F64; break;
                                }
                            }

                            Debug.Instruction(op, pc - 1, stack[sp - 1]);
                            break;
                        }

                    case ILOpCode.Add_ovf:
                        {
                            // Decrement ptr
                            sp--;

                            checked
                            {
                                // Check type - signed addition with overflow check
                                switch (stack[sp].Type)
                                {
                                    default: throw new NotSupportedException(stack[sp].Type.ToString());

                                    case StackType.I32: stack[sp - 1].I32 = stack[sp - 1].I32 + stack[sp].I32; break;
                                    case StackType.U32: stack[sp - 1].I32 = stack[sp - 1].I32 + stack[sp].I32; break; // Treat as signed
                                    case StackType.I64: stack[sp - 1].I64 = stack[sp - 1].I64 + stack[sp].I64; break;
                                    case StackType.U64: stack[sp - 1].I64 = stack[sp - 1].I64 + stack[sp].I64; break; // Treat as signed
                                    case StackType.Ptr: stack[sp - 1].Ptr = (IntPtr)((long)stack[sp - 1].Ptr + (long)stack[sp].Ptr); break;
                                    case StackType.UPtr: stack[sp - 1].Ptr = (IntPtr)((long)stack[sp - 1].Ptr + (long)stack[sp].Ptr); break;
                                }
                            }

                            Debug.Instruction(op, pc - 1, stack[sp - 1]);
                            break;
                        }

                    case ILOpCode.Add_ovf_un:
                        {
                            // Decrement ptr
                            sp--;

                            checked
                            {
                                // Check type - unsigned addition with overflow check
                                switch (stack[sp].Type)
                                {
                                    default: throw new NotSupportedException(stack[sp].Type.ToString());

                                    case StackType.I32: stack[sp - 1].I32 = (int)((uint)stack[sp - 1].I32 + (uint)stack[sp].I32); break;
                                    case StackType.U32: stack[sp - 1].I32 = (int)((uint)stack[sp - 1].I32 + (uint)stack[sp].I32); break;
                                    case StackType.I64: stack[sp - 1].I64 = (long)((ulong)stack[sp - 1].I64 + (ulong)stack[sp].I64); break;
                                    case StackType.U64: stack[sp - 1].I64 = (long)((ulong)stack[sp - 1].I64 + (ulong)stack[sp].I64); break;
                                    case StackType.Ptr: stack[sp - 1].Ptr = (IntPtr)((ulong)(long)stack[sp - 1].Ptr + (ulong)(long)stack[sp].Ptr); break;
                                    case StackType.UPtr: stack[sp - 1].Ptr = (IntPtr)((ulong)(long)stack[sp - 1].Ptr + (ulong)(long)stack[sp].Ptr); break;
                                }
                            }

                            Debug.Instruction(op, pc - 1, stack[sp - 1]);
                            break;
                        }

                    case ILOpCode.Sub_ovf:
                        {
                            // Decrement ptr
                            sp--;

                            checked
                            {
                                // Check type - signed subtraction with overflow check
                                switch (stack[sp].Type)
                                {
                                    default: throw new NotSupportedException(stack[sp].Type.ToString());

                                    case StackType.I32: stack[sp - 1].I32 = stack[sp - 1].I32 - stack[sp].I32; break;
                                    case StackType.U32: stack[sp - 1].I32 = stack[sp - 1].I32 - stack[sp].I32; break; // Treat as signed
                                    case StackType.I64: stack[sp - 1].I64 = stack[sp - 1].I64 - stack[sp].I64; break;
                                    case StackType.U64: stack[sp - 1].I64 = stack[sp - 1].I64 - stack[sp].I64; break; // Treat as signed
                                    case StackType.Ptr: stack[sp - 1].Ptr = (IntPtr)((long)stack[sp - 1].Ptr - (long)stack[sp].Ptr); break;
                                    case StackType.UPtr: stack[sp - 1].Ptr = (IntPtr)((long)stack[sp - 1].Ptr - (long)stack[sp].Ptr); break;
                                }
                            }

                            Debug.Instruction(op, pc - 1, stack[sp - 1]);
                            break;
                        }

                    case ILOpCode.Sub_ovf_un:
                        {
                            // Decrement ptr
                            sp--;

                            checked
                            {
                                // Check type - unsigned subtraction with overflow check
                                switch (stack[sp].Type)
                                {
                                    default: throw new NotSupportedException(stack[sp].Type.ToString());

                                    case StackType.I32: stack[sp - 1].I32 = (int)((uint)stack[sp - 1].I32 - (uint)stack[sp].I32); break;
                                    case StackType.U32: stack[sp - 1].I32 = (int)((uint)stack[sp - 1].I32 - (uint)stack[sp].I32); break;
                                    case StackType.I64: stack[sp - 1].I64 = (long)((ulong)stack[sp - 1].I64 - (ulong)stack[sp].I64); break;
                                    case StackType.U64: stack[sp - 1].I64 = (long)((ulong)stack[sp - 1].I64 - (ulong)stack[sp].I64); break;
                                    case StackType.Ptr: stack[sp - 1].Ptr = (IntPtr)((ulong)(long)stack[sp - 1].Ptr - (ulong)(long)stack[sp].Ptr); break;
                                    case StackType.UPtr: stack[sp - 1].Ptr = (IntPtr)((ulong)(long)stack[sp - 1].Ptr - (ulong)(long)stack[sp].Ptr); break;
                                }
                            }

                            Debug.Instruction(op, pc - 1, stack[sp - 1]);
                            break;
                        }

                    case ILOpCode.Mul_ovf:
                        {
                            // Decrement ptr
                            sp--;

                            checked
                            {
                                // Check type - signed multiplication with overflow check
                                switch (stack[sp].Type)
                                {
                                    default: throw new NotSupportedException(stack[sp].Type.ToString());

                                    case StackType.I32: stack[sp - 1].I32 = stack[sp - 1].I32 * stack[sp].I32; break;
                                    case StackType.U32: stack[sp - 1].I32 = stack[sp - 1].I32 * stack[sp].I32; break; // Treat as signed
                                    case StackType.I64: stack[sp - 1].I64 = stack[sp - 1].I64 * stack[sp].I64; break;
                                    case StackType.U64: stack[sp - 1].I64 = stack[sp - 1].I64 * stack[sp].I64; break; // Treat as signed
                                }
                            }

                            Debug.Instruction(op, pc - 1, stack[sp - 1]);
                            break;
                        }

                    case ILOpCode.Mul_ovf_un:
                        {
                            // Decrement ptr
                            sp--;

                            checked
                            {
                                // Check type - unsigned multiplication with overflow check
                                switch (stack[sp].Type)
                                {
                                    default: throw new NotSupportedException(stack[sp].Type.ToString());

                                    case StackType.I32: stack[sp - 1].I32 = (int)((uint)stack[sp - 1].I32 * (uint)stack[sp].I32); break;
                                    case StackType.U32: stack[sp - 1].I32 = (int)((uint)stack[sp - 1].I32 * (uint)stack[sp].I32); break;
                                    case StackType.I64: stack[sp - 1].I64 = (long)((ulong)stack[sp - 1].I64 * (ulong)stack[sp].I64); break;
                                    case StackType.U64: stack[sp - 1].I64 = (long)((ulong)stack[sp - 1].I64 * (ulong)stack[sp].I64); break;
                                }
                            }

                            Debug.Instruction(op, pc - 1, stack[sp - 1]);
                            break;
                        }
                    #endregion

                } // End switch
            } // End loop

            return sp; // Return the final stack pointer
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static T FetchDecode<T>(byte[] instructions, ref int pc) where T : unmanaged
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
