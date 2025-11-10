using dotnow.Interop;
using System;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

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

            // Get the stack
            StackData[] stack = threadContext.stack;

            // Get local
            int spLoc = spArg + ((method.Flags & CILMethodFlags.This) != 0 ? 1 : 0) + method.ParameterTypes.Length;

            // Copy locals
            Array.Copy(method.Locals, 0, stack, spLoc, method.LocalCount);

            // Get sp
            int sp = spLoc + method.LocalCount;

            // Get sp max
            int spMax = sp + method.MaxStack;

            // Check overflow - we'll handle this differently since we can't use unsafe
            if (spMax >= threadContext.stack.Length)
                threadContext.Throw<StackOverflowException>();

            // Get the instructions
            byte[] instructions = method.Instructions;

            // Get program counter
            int pc = 0;
            int pcMax = instructions.Length;

            // Main execution loop
            while (pc < pcMax && threadContext.abort == false)
            {
                // Fetch the op code
                ILOpCode op = FetchOpCode(instructions, ref pc);

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

                    case ILOpCode.Volatile: continue;

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
                    case ILOpCode.Ldarga_s:
                        {
                            // Read the offset
                            sbyte offset = FetchDecode<sbyte>(instructions, ref pc);

                            // Copy from arg offset
                            stack[sp].Ref = IByRef.MakeByRefStack(stack, spArg + offset);
                            stack[sp].Type = StackType.ByRef;
                            sp++;

                            Debug.Instruction(op, pc - 2, stack[sp - 1]);
                            break;
                        }
                    case ILOpCode.Ldarga:
                        {
                            // Read the offset
                            int offset = FetchDecode<int>(instructions, ref pc);

                            // Copy from arg offset
                            stack[sp].Ref = IByRef.MakeByRefStack(stack, spArg + offset);
                            stack[sp].Type = StackType.ByRef;
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
                    case ILOpCode.Ldloca_s:
                        {
                            // Fetch offset
                            sbyte offset = FetchDecode<sbyte>(instructions, ref pc);

                            // Copy from local offset to stack
                            stack[sp].Ref = IByRef.MakeByRefStack(stack, spLoc + offset);
                            stack[sp].Type = StackType.ByRef;
                            sp++;

                            Debug.Instruction(op, pc - 2, stack[sp - 1]);
                            break;
                        }
                    case ILOpCode.Ldloca:
                        {
                            // Fetch offset
                            int offset = FetchDecode<int>(instructions, ref pc);

                            // Copy from local offset to stack
                            stack[sp].Ref = IByRef.MakeByRefStack(stack, spLoc + offset);
                            stack[sp].Type = StackType.ByRef;
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

                    #region Field
                    case ILOpCode.Ldfld:
                        {
                            // Get method token
                            int token = FetchDecode<int>(instructions, ref pc);

                            // Get handle
                            EntityHandle fieldHandle = MetadataTokens.EntityHandle(token);

                            // Load the method
                            CILFieldInfo field = loadContext.GetFieldHandle(fieldHandle);

                            // Pop the instance
                            StackData instance = stack[--sp];

                            // Check for null
                            if (instance.Ref == null)
                                threadContext.Throw<NullReferenceException>();

                            // Load the field
                            RuntimeField.GetInstanceFieldDirect(loadContext.AppDomain, field, instance, ref stack[sp]);
                            sp++;

                            // Debug execution
                            Debug.Instruction(op, pc - 5, stack[sp - 1]);
                            break;
                        }
                    case ILOpCode.Ldflda:
                        {
                            // Get method token
                            int token = FetchDecode<int>(instructions, ref pc);

                            // Get handle
                            EntityHandle fieldHandle = MetadataTokens.EntityHandle(token);

                            // Load the method
                            CILFieldInfo field = loadContext.GetFieldHandle(fieldHandle);

                            // Pop the instance
                            StackData instance = stack[--sp];

                            // Check for null
                            if (instance.Ref == null)
                                threadContext.Throw<NullReferenceException>();

                            // Push the field reference to the stack
                            stack[sp].Ref = IByRef.MakeByRefInstanceField(loadContext.AppDomain, field, instance);
                            stack[sp++].Type = StackType.ByRef;

                            // Debug execution
                            Debug.Instruction(op, pc - 5, stack[sp - 1]);
                            break;
                        }
                    case ILOpCode.Ldsfld:
                        {
                            // Get method token
                            int token = FetchDecode<int>(instructions, ref pc);

                            // Get handle
                            EntityHandle fieldHandle = MetadataTokens.EntityHandle(token);

                            // Load the method
                            CILFieldInfo field = loadContext.GetFieldHandle(fieldHandle);

                            // Load the field
                            RuntimeField.GetStaticFieldDirect(loadContext.AppDomain, field, ref stack[sp]);
                            sp++;

                            // Debug execution
                            Debug.Instruction(op, pc - 5, stack[sp - 1]);
                            break;
                        }
                    case ILOpCode.Ldsflda:
                        {
                            // Get method token
                            int token = FetchDecode<int>(instructions, ref pc);

                            // Get handle
                            EntityHandle fieldHandle = MetadataTokens.EntityHandle(token);

                            // Load the method
                            CILFieldInfo field = loadContext.GetFieldHandle(fieldHandle);

                            // Push the field reference to the stack
                            stack[sp].Ref = IByRef.MakeByRefStaticField(loadContext.AppDomain, field);
                            stack[sp++].Type = StackType.ByRef;

                            // Debug execution
                            Debug.Instruction(op, pc - 5, stack[sp - 1]);
                            break;
                        }
                    case ILOpCode.Stfld:
                        {
                            // Get method token
                            int token = FetchDecode<int>(instructions, ref pc);

                            // Get handle
                            EntityHandle fieldHandle = MetadataTokens.EntityHandle(token);

                            // Load the method
                            CILFieldInfo field = loadContext.GetFieldHandle(fieldHandle);

                            // Pop value and instance
                            StackData value = stack[--sp];
                            StackData instance = stack[--sp];

                            // Check for null
                            if (instance.Ref == null)
                                threadContext.Throw<NullReferenceException>();

                            // Set the field
                            RuntimeField.SetInstanceFieldDirect(loadContext.AppDomain, field, instance, ref value);

                            // Debug execution
                            Debug.Instruction(op, pc - 5, value);
                            break;
                        }
                    case ILOpCode.Stsfld:
                        {
                            // Get method token
                            int token = FetchDecode<int>(instructions, ref pc);

                            // Get handle
                            EntityHandle fieldHandle = MetadataTokens.EntityHandle(token);

                            // Load the method
                            CILFieldInfo field = loadContext.GetFieldHandle(fieldHandle);

                            // Pop value
                            StackData value = stack[--sp];

                            // Load the field
                            RuntimeField.SetStaticFieldDirect(loadContext.AppDomain, field, ref value);

                            // Debug execution
                            Debug.Instruction(op, pc - 5, value);
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
                                case StackType.Ref: stack[sp - 1].I32 = stack[sp - 1].Address > stack[sp].Address ? 1 : 0; break;
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
                                case StackType.Ref: stack[sp - 1].I32 = stack[sp - 1].Address > stack[sp].Address ? 1 : 0; break;
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
                                case StackType.Ref: stack[sp - 1].I32 = stack[sp - 1].Address < stack[sp].Address ? 1 : 0; break;
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
                                case StackType.Ref: stack[sp - 1].I32 = stack[sp - 1].Address < stack[sp].Address ? 1 : 0; break;
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
                                                threadContext.Throw<DivideByZeroException>();

                                            stack[sp - 1].I32 = stack[sp - 1].I32 / stack[sp].I32;
                                            break;
                                        }
                                    case StackType.U32:
                                        {
                                            // Check for divide by zero
                                            if (stack[sp].I32 == 0)
                                                threadContext.Throw<DivideByZeroException>();

                                            stack[sp - 1].I32 = stack[sp - 1].I32 / stack[sp].I32; // Treat as signed
                                            break;
                                        }
                                    case StackType.I64:
                                        {
                                            // Check for divide by zero
                                            if (stack[sp].I64 == 0)
                                                threadContext.Throw<DivideByZeroException>();

                                            stack[sp - 1].I64 = stack[sp - 1].I64 / stack[sp].I64;
                                            break;
                                        }
                                    case StackType.U64:
                                        {
                                            // Check for divide by zero
                                            if (stack[sp].I64 == 0)
                                                threadContext.Throw<DivideByZeroException>();

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
                                                threadContext.Throw<DivideByZeroException>();

                                            stack[sp - 1].I32 = (int)((uint)stack[sp - 1].I32 / (uint)stack[sp].I32);
                                            break;
                                        }
                                    case StackType.U32:
                                        {
                                            // Check for divide by zero
                                            if ((uint)stack[sp].I32 == 0)
                                                threadContext.Throw<DivideByZeroException>();

                                            stack[sp - 1].I32 = (int)((uint)stack[sp - 1].I32 / (uint)stack[sp].I32);
                                            break;
                                        }
                                    case StackType.I64:
                                        {
                                            // Check for divide by zero
                                            if ((ulong)stack[sp].I64 == 0)
                                                threadContext.Throw<DivideByZeroException>();

                                            stack[sp - 1].I64 = (long)((ulong)stack[sp - 1].I64 / (ulong)stack[sp].I64);
                                            break;
                                        }
                                    case StackType.U64:
                                        {
                                            // Check for divide by zero
                                            if ((ulong)stack[sp].I64 == 0)
                                                threadContext.Throw<DivideByZeroException>();

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
                                                threadContext.Throw<DivideByZeroException>();

                                            stack[sp - 1].I32 = stack[sp - 1].I32 % stack[sp].I32;
                                            break;
                                        }
                                    case StackType.U32:
                                        {
                                            // Check for divide by zero
                                            if (stack[sp].I32 == 0)
                                                threadContext.Throw<DivideByZeroException>();

                                            stack[sp - 1].I32 = stack[sp - 1].I32 % stack[sp].I32; // Treat as signed
                                            break;
                                        }
                                    case StackType.I64:
                                        {
                                            // Check for divide by zero
                                            if (stack[sp].I64 == 0)
                                                threadContext.Throw<DivideByZeroException>();

                                            stack[sp - 1].I64 = stack[sp - 1].I64 % stack[sp].I64;
                                            break;
                                        }
                                    case StackType.U64:
                                        {
                                            // Check for divide by zero
                                            if (stack[sp].I64 == 0)
                                                threadContext.Throw<DivideByZeroException>();

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
                                                threadContext.Throw<DivideByZeroException>();

                                            stack[sp - 1].I32 = (int)((uint)stack[sp - 1].I32 % (uint)stack[sp].I32);
                                            break;
                                        }
                                    case StackType.U32:
                                        {
                                            // Check for divide by zero
                                            if ((uint)stack[sp].I32 == 0)
                                                threadContext.Throw<DivideByZeroException>();

                                            stack[sp - 1].I32 = (int)((uint)stack[sp - 1].I32 % (uint)stack[sp].I32);
                                            break;
                                        }
                                    case StackType.I64:
                                        {
                                            // Check for divide by zero
                                            if ((ulong)stack[sp].I64 == 0)
                                                threadContext.Throw<DivideByZeroException>();

                                            stack[sp - 1].I64 = (long)((ulong)stack[sp - 1].I64 % (ulong)stack[sp].I64);
                                            break;
                                        }
                                    case StackType.U64:
                                        {
                                            // Check for divide by zero
                                            if ((ulong)stack[sp].I64 == 0)
                                                threadContext.Throw<DivideByZeroException>();

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

                    #region Bitwise
                    case ILOpCode.And:
                        {
                            // Decrement ptr
                            sp--;

                            switch (stack[sp].Type)
                            {
                                default: throw new NotSupportedException(stack[sp].Type.ToString());

                                case StackType.I32: stack[sp - 1].I32 = stack[sp - 1].I32 & stack[sp].I32; break;
                                case StackType.U32: stack[sp - 1].I32 = (int)((uint)stack[sp - 1].I32 & (uint)stack[sp].I32); break;
                                case StackType.I64: stack[sp - 1].I64 = stack[sp - 1].I64 & stack[sp].I64; break;
                                case StackType.U64: stack[sp - 1].I64 = (long)((ulong)stack[sp - 1].I64 & (ulong)stack[sp].I64); break;
                            }

                            Debug.Instruction(op, pc - 1, stack[sp - 1]);
                            break;
                        }
                    case ILOpCode.Or:
                        {
                            // Decrement ptr
                            sp--;

                            switch (stack[sp].Type)
                            {
                                default: throw new NotSupportedException(stack[sp].Type.ToString());

                                case StackType.I32: stack[sp - 1].I32 = stack[sp - 1].I32 | stack[sp].I32; break;
                                case StackType.U32: stack[sp - 1].I32 = (int)((uint)stack[sp - 1].I32 | (uint)stack[sp].I32); break;
                                case StackType.I64: stack[sp - 1].I64 = stack[sp - 1].I64 | stack[sp].I64; break;
                                case StackType.U64: stack[sp - 1].I64 = (long)((ulong)stack[sp - 1].I64 | (ulong)stack[sp].I64); break;
                            }

                            Debug.Instruction(op, pc - 1, stack[sp - 1]);
                            break;
                        }
                    case ILOpCode.Xor:
                        {
                            // Decrement ptr
                            sp--;

                            switch (stack[sp].Type)
                            {
                                default: throw new NotSupportedException(stack[sp].Type.ToString());

                                case StackType.I32: stack[sp - 1].I32 = stack[sp - 1].I32 ^ stack[sp].I32; break;
                                case StackType.U32: stack[sp - 1].I32 = (int)((uint)stack[sp - 1].I32 ^ (uint)stack[sp].I32); break;
                                case StackType.I64: stack[sp - 1].I64 = stack[sp - 1].I64 ^ stack[sp].I64; break;
                                case StackType.U64: stack[sp - 1].I64 = (long)((ulong)stack[sp - 1].I64 ^ (ulong)stack[sp].I64); break;
                            }

                            Debug.Instruction(op, pc - 1, stack[sp - 1]);
                            break;
                        }
                    case ILOpCode.Not:
                        {
                            switch (stack[sp- 1].Type)
                            {
                                default: throw new NotSupportedException(stack[sp - 1].Type.ToString());

                                case StackType.I32: stack[sp - 1].I32 = ~stack[sp - 1].I32; break;
                                case StackType.U32: stack[sp - 1].I32 = (int)~(uint)stack[sp - 1].I32; break;
                                case StackType.I64: stack[sp - 1].I64 = ~stack[sp - 1].I64; break;
                                case StackType.U64: stack[sp - 1].I64 = (long)~(ulong)stack[sp - 1].I64; break;
                            }

                            Debug.Instruction(op, pc - 1, stack[sp - 1]);
                            break;
                        }
                    case ILOpCode.Shl:
                        {
                            // Decrement ptr
                            sp--;

                            switch (stack[sp - 1].Type)
                            {
                                default: throw new NotSupportedException(stack[sp - 1].Type.ToString());

                                case StackType.I32: stack[sp - 1].I32 = stack[sp - 1].I32 << stack[sp].I32; break;
                                case StackType.U32: stack[sp - 1].I32 = (int)((uint)stack[sp - 1].I32 << stack[sp].I32); break;
                                case StackType.I64: stack[sp - 1].I64 = stack[sp - 1].I64 << stack[sp].I32; break;
                                case StackType.U64: stack[sp - 1].I64 = (long)((ulong)stack[sp - 1].I64 << stack[sp].I32); break;
                            }

                            Debug.Instruction(op, pc - 1, stack[sp - 1]);
                            break;
                        }
                    case ILOpCode.Shr:
                        {
                            // Decrement ptr
                            sp--;

                            switch (stack[sp - 1].Type)
                            {
                                default: throw new NotSupportedException(stack[sp - 1].Type.ToString());

                                case StackType.I32: stack[sp - 1].I32 = stack[sp - 1].I32 >> stack[sp].I32; break;
                                case StackType.U32: stack[sp - 1].I32 = (int)((uint)stack[sp - 1].I32 >> stack[sp].I32); break;
                                case StackType.I64: stack[sp - 1].I64 = stack[sp - 1].I64 >> stack[sp].I32; break;
                                case StackType.U64: stack[sp - 1].I64 = (long)((ulong)stack[sp - 1].I64 >> stack[sp].I32); break;
                            }

                            Debug.Instruction(op, pc - 1, stack[sp - 1]);
                            break;
                        }
                    case ILOpCode.Shr_un:
                        {
                            // Decrement ptr
                            sp--;

                            switch (stack[sp - 1].Type)
                            {
                                default: throw new NotSupportedException(stack[sp - 1].Type.ToString());

                                case StackType.I32: stack[sp - 1].I32 = stack[sp - 1].I32 >> stack[sp].I32; break;
                                case StackType.U32: stack[sp - 1].I32 = (int)((uint)stack[sp - 1].I32 >> stack[sp].I32); break;
                                case StackType.I64: stack[sp - 1].I64 = stack[sp - 1].I64 >> stack[sp].I32; break;
                                case StackType.U64: stack[sp - 1].I64 = (long)((ulong)stack[sp - 1].I64 >> stack[sp].I32); break;
                            }

                            Debug.Instruction(op, pc - 1, stack[sp - 1]);
                            break;
                        }
                    #endregion

                    #region Convert
                    case ILOpCode.Conv_i:
                        {
                            // Check type on stack
                            switch (stack[sp - 1].Type)
                            {
                                default: throw new NotSupportedException(stack[sp - 1].Type.ToString());

                                case StackType.I32: stack[sp - 1].Ptr = (IntPtr)stack[sp - 1].I32; break;
                                case StackType.U32: stack[sp - 1].Ptr = (IntPtr)(uint)stack[sp - 1].I32; break;
                                case StackType.I64: stack[sp - 1].Ptr = (IntPtr)stack[sp - 1].I64; break;
                                case StackType.U64: stack[sp - 1].Ptr = (IntPtr)(ulong)stack[sp - 1].I64; break;
                            }
                            // Convert to Ptr/native int
                            stack[sp - 1].Type = StackType.Ptr;

                            // Debug execution
                            Debug.Instruction(op, pc - 1, stack[sp - 1]);
                            break;
                        }
                    case ILOpCode.Conv_u:
                        {
                            // Check type on stack
                            switch (stack[sp - 1].Type)
                            {
                                default: throw new NotSupportedException(stack[sp - 1].Type.ToString());

                                case StackType.I32: stack[sp - 1].Ptr = (IntPtr)(uint)stack[sp - 1].I32; break;
                                case StackType.U32: stack[sp - 1].Ptr = (IntPtr)(uint)stack[sp - 1].I32; break;
                                case StackType.I64: stack[sp - 1].Ptr = (IntPtr)(ulong)stack[sp - 1].I64; break;
                                case StackType.U64: stack[sp - 1].Ptr = (IntPtr)(ulong)stack[sp - 1].I64; break;
                                case StackType.F32: stack[sp - 1].Ptr = (IntPtr)(uint)stack[sp - 1].F32; break;
                                case StackType.F64: stack[sp - 1].Ptr = (IntPtr)(uint)stack[sp - 1].F64; break;
                                case StackType.Ptr: stack[sp - 1].Ptr = (IntPtr)(ulong)(long)stack[sp - 1].Ptr; break;
                                case StackType.UPtr: stack[sp - 1].Ptr = stack[sp - 1].Ptr; break;
                            }
                            // Convert to UPtr/unsigned native int
                            stack[sp - 1].Type = StackType.UPtr;

                            // Debug execution
                            Debug.Instruction(op, pc - 1, stack[sp - 1]);
                            break;
                        }
                    case ILOpCode.Conv_i1:
                        {
                            // Check type on stack
                            switch (stack[sp - 1].Type)
                            {
                                default: throw new NotSupportedException(stack[sp - 1].Type.ToString());

                                case StackType.I32: stack[sp - 1].I32 = (sbyte)stack[sp - 1].I32; break;
                                case StackType.U32: stack[sp - 1].I32 = (sbyte)(uint)stack[sp - 1].I32; break;
                                case StackType.I64: stack[sp - 1].I32 = (sbyte)stack[sp - 1].I64; break;
                                case StackType.U64: stack[sp - 1].I32 = (sbyte)(ulong)stack[sp - 1].I64; break;
                                case StackType.F32: stack[sp - 1].I32 = (sbyte)stack[sp - 1].F32; break;
                                case StackType.F64: stack[sp - 1].I32 = (sbyte)stack[sp - 1].F64; break;
                                case StackType.Ptr: stack[sp - 1].I32 = (sbyte)(long)stack[sp - 1].Ptr; break;
                                case StackType.UPtr: stack[sp - 1].I32 = (sbyte)(ulong)(long)stack[sp - 1].Ptr; break;
                            }
                            // Convert to I32 (signed byte is promoted to I32 on stack)
                            stack[sp - 1].Type = StackType.I32;

                            // Debug execution
                            Debug.Instruction(op, pc - 1, stack[sp - 1]);
                            break;
                        }
                    case ILOpCode.Conv_u1:
                        {
                            // Check type on stack
                            switch (stack[sp - 1].Type)
                            {
                                default: throw new NotSupportedException(stack[sp - 1].Type.ToString());

                                case StackType.I32: stack[sp - 1].I32 = (byte)stack[sp - 1].I32; break;
                                case StackType.U32: stack[sp - 1].I32 = (byte)(uint)stack[sp - 1].I32; break;
                                case StackType.I64: stack[sp - 1].I32 = (byte)stack[sp - 1].I64; break;
                                case StackType.U64: stack[sp - 1].I32 = (byte)(ulong)stack[sp - 1].I64; break;
                                case StackType.F32: stack[sp - 1].I32 = (byte)stack[sp - 1].F32; break;
                                case StackType.F64: stack[sp - 1].I32 = (byte)stack[sp - 1].F64; break;
                                case StackType.Ptr: stack[sp - 1].I32 = (byte)(long)stack[sp - 1].Ptr; break;
                                case StackType.UPtr: stack[sp - 1].I32 = (byte)(ulong)(long)stack[sp - 1].Ptr; break;
                            }
                            // Convert to I32 (unsigned byte is promoted to I32 on stack)
                            stack[sp - 1].Type = StackType.I32;

                            // Debug execution
                            Debug.Instruction(op, pc - 1, stack[sp - 1]);
                            break;
                        }
                    case ILOpCode.Conv_i2:
                        {
                            // Check type on stack
                            switch (stack[sp - 1].Type)
                            {
                                default: throw new NotSupportedException(stack[sp - 1].Type.ToString());

                                case StackType.I32: stack[sp - 1].I32 = (short)stack[sp - 1].I32; break;
                                case StackType.U32: stack[sp - 1].I32 = (short)(uint)stack[sp - 1].I32; break;
                                case StackType.I64: stack[sp - 1].I32 = (short)stack[sp - 1].I64; break;
                                case StackType.U64: stack[sp - 1].I32 = (short)(ulong)stack[sp - 1].I64; break;
                                case StackType.F32: stack[sp - 1].I32 = (short)stack[sp - 1].F32; break;
                                case StackType.F64: stack[sp - 1].I32 = (short)stack[sp - 1].F64; break;
                                case StackType.Ptr: stack[sp - 1].I32 = (short)(long)stack[sp - 1].Ptr; break;
                                case StackType.UPtr: stack[sp - 1].I32 = (short)(ulong)(long)stack[sp - 1].Ptr; break;
                            }
                            // Convert to I32 (signed short is promoted to I32 on stack)
                            stack[sp - 1].Type = StackType.I32;

                            // Debug execution
                            Debug.Instruction(op, pc - 1, stack[sp - 1]);
                            break;
                        }
                    case ILOpCode.Conv_u2:
                        {
                            // Check type on stack
                            switch (stack[sp - 1].Type)
                            {
                                default: throw new NotSupportedException(stack[sp - 1].Type.ToString());

                                case StackType.I32: stack[sp - 1].I32 = (ushort)stack[sp - 1].I32; break;
                                case StackType.U32: stack[sp - 1].I32 = (ushort)(uint)stack[sp - 1].I32; break;
                                case StackType.I64: stack[sp - 1].I32 = (ushort)stack[sp - 1].I64; break;
                                case StackType.U64: stack[sp - 1].I32 = (ushort)(ulong)stack[sp - 1].I64; break;
                                case StackType.F32: stack[sp - 1].I32 = (ushort)stack[sp - 1].F32; break;
                                case StackType.F64: stack[sp - 1].I32 = (ushort)stack[sp - 1].F64; break;
                                case StackType.Ptr: stack[sp - 1].I32 = (ushort)(long)stack[sp - 1].Ptr; break;
                                case StackType.UPtr: stack[sp - 1].I32 = (ushort)(ulong)(long)stack[sp - 1].Ptr; break;
                            }
                            // Convert to I32 (unsigned short is promoted to I32 on stack)
                            stack[sp - 1].Type = StackType.I32;

                            // Debug execution
                            Debug.Instruction(op, pc - 1, stack[sp - 1]);
                            break;
                        }
                    case ILOpCode.Conv_i4:
                        {
                            // Check type on stack
                            switch (stack[sp - 1].Type)
                            {
                                default: throw new NotSupportedException(stack[sp - 1].Type.ToString());

                                case StackType.I32: /* already I32 */ break;
                                case StackType.U32: stack[sp - 1].I32 = (int)(uint)stack[sp - 1].I32; break;
                                case StackType.I64: stack[sp - 1].I32 = (int)stack[sp - 1].I64; break;
                                case StackType.U64: stack[sp - 1].I32 = (int)(ulong)stack[sp - 1].I64; break;
                                case StackType.F32: stack[sp - 1].I32 = (int)stack[sp - 1].F32; break;
                                case StackType.F64: stack[sp - 1].I32 = (int)stack[sp - 1].F64; break;
                                case StackType.Ptr: stack[sp - 1].I32 = (int)(long)stack[sp - 1].Ptr; break;
                                case StackType.UPtr: stack[sp - 1].I32 = (int)(ulong)(long)stack[sp - 1].Ptr; break;
                            }
                            // Convert to I32
                            stack[sp - 1].Type = StackType.I32;

                            // Debug execution
                            Debug.Instruction(op, pc - 1, stack[sp - 1]);
                            break;
                        }
                    case ILOpCode.Conv_u4:
                        {
                            // Check type on stack
                            switch (stack[sp - 1].Type)
                            {
                                default: throw new NotSupportedException(stack[sp - 1].Type.ToString());

                                case StackType.I32: stack[sp - 1].I32 = (int)(uint)stack[sp - 1].I32; break;
                                case StackType.U32: /* already U32 as I32 */ break;
                                case StackType.I64: stack[sp - 1].I32 = (int)(uint)stack[sp - 1].I64; break;
                                case StackType.U64: stack[sp - 1].I32 = (int)(uint)(ulong)stack[sp - 1].I64; break;
                                case StackType.F32: stack[sp - 1].I32 = (int)(uint)stack[sp - 1].F32; break;
                                case StackType.F64: stack[sp - 1].I32 = (int)(uint)stack[sp - 1].F64; break;
                                case StackType.Ptr: stack[sp - 1].I32 = (int)(uint)(long)stack[sp - 1].Ptr; break;
                                case StackType.UPtr: stack[sp - 1].I32 = (int)(uint)(ulong)(long)stack[sp - 1].Ptr; break;
                            }
                            // Convert to U32 (stored as I32)
                            stack[sp - 1].Type = StackType.U32;

                            // Debug execution
                            Debug.Instruction(op, pc - 1, stack[sp - 1]);
                            break;
                        }
                    case ILOpCode.Conv_i8:
                        {
                            // Check type on stack
                            switch (stack[sp - 1].Type)
                            {
                                default: throw new NotSupportedException(stack[sp - 1].Type.ToString());

                                case StackType.I32: stack[sp - 1].I64 = stack[sp - 1].I32; break;
                                case StackType.U32: stack[sp - 1].I64 = (uint)stack[sp - 1].I32; break;
                                case StackType.I64: /* already I64 */ break;
                                case StackType.U64: stack[sp - 1].I64 = (long)(ulong)stack[sp - 1].I64; break;
                                case StackType.F32: stack[sp - 1].I64 = (long)stack[sp - 1].F32; break;
                                case StackType.F64: stack[sp - 1].I64 = (long)stack[sp - 1].F64; break;
                                case StackType.Ptr: stack[sp - 1].I64 = (long)stack[sp - 1].Ptr; break;
                                case StackType.UPtr: stack[sp - 1].I64 = (long)(ulong)(long)stack[sp - 1].Ptr; break;
                            }
                            // Convert to I64
                            stack[sp - 1].Type = StackType.I64;

                            // Debug execution
                            Debug.Instruction(op, pc - 1, stack[sp - 1]);
                            break;
                        }
                    case ILOpCode.Conv_u8:
                        {
                            // Check type on stack
                            switch (stack[sp - 1].Type)
                            {
                                default: throw new NotSupportedException(stack[sp - 1].Type.ToString());

                                case StackType.I32: stack[sp - 1].I64 = (long)(uint)stack[sp - 1].I32; break;
                                case StackType.U32: stack[sp - 1].I64 = (long)(uint)stack[sp - 1].I32; break;
                                case StackType.I64: stack[sp - 1].I64 = (long)(ulong)stack[sp - 1].I64; break;
                                case StackType.U64: /* already U64 as I64 */ break;
                                case StackType.F32: stack[sp - 1].I64 = (long)(ulong)stack[sp - 1].F32; break;
                                case StackType.F64: stack[sp - 1].I64 = (long)(ulong)stack[sp - 1].F64; break;
                                case StackType.Ptr: stack[sp - 1].I64 = (long)(ulong)(long)stack[sp - 1].Ptr; break;
                                case StackType.UPtr: stack[sp - 1].I64 = (long)(ulong)(long)stack[sp - 1].Ptr; break;
                            }
                            // Convert to U64 (stored as I64)
                            stack[sp - 1].Type = StackType.U64;

                            // Debug execution
                            Debug.Instruction(op, pc - 1, stack[sp - 1]);
                            break;
                        }
                    case ILOpCode.Conv_r4:
                        {
                            // Check type on stack
                            switch (stack[sp - 1].Type)
                            {
                                default: throw new NotSupportedException(stack[sp - 1].Type.ToString());

                                case StackType.I32: stack[sp - 1].F32 = stack[sp - 1].I32; break;
                                case StackType.U32: stack[sp - 1].F32 = (uint)stack[sp - 1].I32; break;
                                case StackType.I64: stack[sp - 1].F32 = stack[sp - 1].I64; break;
                                case StackType.U64: stack[sp - 1].F32 = (ulong)stack[sp - 1].I64; break;
                                case StackType.F32: /* already F32 */ break;
                                case StackType.F64: stack[sp - 1].F32 = (float)stack[sp - 1].F64; break;
                                case StackType.Ptr: stack[sp - 1].F32 = (long)stack[sp - 1].Ptr; break;
                                case StackType.UPtr: stack[sp - 1].F32 = (ulong)(long)stack[sp - 1].Ptr; break;
                            }
                            // Convert to F32
                            stack[sp - 1].Type = StackType.F32;

                            // Debug execution
                            Debug.Instruction(op, pc - 1, stack[sp - 1]);
                            break;
                        }
                    case ILOpCode.Conv_r8:
                        {
                            // Check type on stack
                            switch (stack[sp - 1].Type)
                            {
                                default: throw new NotSupportedException(stack[sp - 1].Type.ToString());

                                case StackType.I32: stack[sp - 1].F64 = stack[sp - 1].I32; break;
                                case StackType.U32: stack[sp - 1].F64 = (uint)stack[sp - 1].I32; break;
                                case StackType.I64: stack[sp - 1].F64 = stack[sp - 1].I64; break;
                                case StackType.U64: stack[sp - 1].F64 = (ulong)stack[sp - 1].I64; break;
                                case StackType.F32: stack[sp - 1].F64 = stack[sp - 1].F32; break;
                                case StackType.F64: /* already F64 */ break;
                                case StackType.Ptr: stack[sp - 1].F64 = (long)stack[sp - 1].Ptr; break;
                                case StackType.UPtr: stack[sp - 1].F64 = (ulong)(long)stack[sp - 1].Ptr; break;
                            }
                            // Convert to F64
                            stack[sp - 1].Type = StackType.F64;

                            // Debug execution
                            Debug.Instruction(op, pc - 1, stack[sp - 1]);
                            break;
                        }
                    #endregion

                    #region Branch
                    case ILOpCode.Br_s:
                        {
                            // Fetch offset
                            sbyte offset = FetchDecode<sbyte>(instructions, ref pc);

                            // Debug execution
                            Debug.Instruction(op, pc - 2, offset);

                            // Update offset
                            pc += offset;
                            break;
                        }
                    case ILOpCode.Br:
                        {
                            // Fetch offset
                            int offset = FetchDecode<int>(instructions, ref pc);

                            // Debug execution
                            Debug.Instruction(op, pc - 5, offset);

                            // Update offset
                            pc += offset;
                            break;
                        }
                    case ILOpCode.Brtrue_s:
                        {
                            // Fetch offset
                            sbyte offset = FetchDecode<sbyte>(instructions, ref pc);

                            // Pop value
                            sp--;
                            bool jmp = false;

                            // Debug execution
                            Debug.Instruction(op, pc - 2, offset);

                            switch(stack[sp].Type)
                            {
                                default: throw new NotSupportedException(stack[sp].Type.ToString());

                                case StackType.I32: jmp = stack[sp].I32 != 0; break;
                                case StackType.Ref: jmp = stack[sp].Address != 0; break;
                            }

                            // Conditional
                            if (jmp == true)
                            {
                                // Update offset
                                pc += offset;
                            }
                            break;
                        }
                    case ILOpCode.Brtrue:
                        {
                            // Fetch offset
                            int offset = FetchDecode<int>(instructions, ref pc);

                            // Pop value
                            sp--;
                            bool jmp = false;

                            // Debug execution
                            Debug.Instruction(op, pc - 2, offset);

                            switch (stack[sp].Type)
                            {
                                default: throw new NotSupportedException(stack[sp].Type.ToString());

                                case StackType.I32: jmp = stack[sp].I32 != 0; break;
                                case StackType.Ref: jmp = stack[sp].Address != 0; break;
                            }

                            // Conditional
                            if (jmp == true)
                            {
                                // Update offset
                                pc += offset;
                            }
                            break;
                        }
                    case ILOpCode.Brfalse_s:
                        {
                            // Fetch offset
                            sbyte offset = FetchDecode<sbyte>(instructions, ref pc);

                            // Pop value
                            sp--;
                            bool jmp = false;

                            // Debug execution
                            Debug.Instruction(op, pc - 2, offset);

                            switch (stack[sp].Type)
                            {
                                default: throw new NotSupportedException(stack[sp].Type.ToString());

                                case StackType.I32: jmp = stack[sp].I32 == 0; break;
                                case StackType.Ref: jmp = stack[sp].Address == 0; break;
                            }

                            // Conditional
                            if (jmp == true)
                            {
                                // Update offset
                                pc += offset;
                            }
                            break;
                        }
                    case ILOpCode.Brfalse:
                        {
                            // Fetch offset
                            int offset = FetchDecode<int>(instructions, ref pc);

                            // Pop value
                            sp--;
                            bool jmp = false;

                            // Debug execution
                            Debug.Instruction(op, pc - 2, offset);

                            switch (stack[sp].Type)
                            {
                                default: throw new NotSupportedException(stack[sp].Type.ToString());

                                case StackType.I32: jmp = stack[sp].I32 == 0; break;
                                case StackType.Ref: jmp = stack[sp].Address == 0; break;
                            }

                            // Conditional
                            if (jmp == true)
                            {
                                // Update offset
                                pc += offset;
                            }
                            break;
                        }
                    case ILOpCode.Beq_s:
                        {
                            // Fetch offset
                            sbyte offset = FetchDecode<sbyte>(instructions, ref pc);

                            // Pop values
                            StackData right = stack[--sp];
                            StackData left = stack[--sp];

                            bool jmp = false;

                            switch(left.Type)
                            {
                                default: throw new NotSupportedException(left.Type.ToString());

                                case StackType.I32: jmp = left.I32 == right.I32; break;
                                case StackType.U32: jmp = (uint)left.I32 == (uint)right.I32; break;
                                case StackType.I64: jmp = left.I64 == right.I64; break;
                                case StackType.U64: jmp = (ulong)left.I64 == (ulong)right.I64; break;
                                case StackType.Ptr: jmp = left.Ptr == right.Ptr; break;
                                case StackType.UPtr: jmp = (UIntPtr)(ulong)left.Ptr == (UIntPtr)(ulong)right.Ptr; break;
                                case StackType.F32: jmp = left.F32 == right.F32; break;
                                case StackType.F64: jmp = left.F64 == right.F64; break;
                                case StackType.Ref: jmp = left.Ref == right.Ref; break;
                            }

                            // Debug execution
                            Debug.Instruction(op, pc - 2, offset);

                            // Conditional
                            if (jmp == true)
                            {
                                // Update offset
                                pc += offset;
                            }
                            break;
                        }
                    case ILOpCode.Beq:
                        {
                            // Fetch offset
                            int offset = FetchDecode<int>(instructions, ref pc);

                            // Pop values
                            StackData right = stack[--sp];
                            StackData left = stack[--sp];

                            bool jmp = false;

                            switch (left.Type)
                            {
                                default: throw new NotSupportedException(left.Type.ToString());

                                case StackType.I32: jmp = left.I32 == right.I32; break;
                                case StackType.U32: jmp = (uint)left.I32 == (uint)right.I32; break;
                                case StackType.I64: jmp = left.I64 == right.I64; break;
                                case StackType.U64: jmp = (ulong)left.I64 == (ulong)right.I64; break;
                                case StackType.Ptr: jmp = left.Ptr == right.Ptr; break;
                                case StackType.UPtr: jmp = (UIntPtr)(ulong)left.Ptr == (UIntPtr)(ulong)right.Ptr; break;
                                case StackType.F32: jmp = left.F32 == right.F32; break;
                                case StackType.F64: jmp = left.F64 == right.F64; break;
                                case StackType.Ref: jmp = left.Ref == right.Ref; break;
                            }

                            // Debug execution
                            Debug.Instruction(op, pc - 5, offset);

                            // Conditional
                            if (jmp == true)
                            {
                                // Update offset
                                pc += offset;
                            }
                            break;
                        }

                    case ILOpCode.Bne_un_s:
                        {
                            // Fetch offset
                            sbyte offset = FetchDecode<sbyte>(instructions, ref pc);

                            // Pop values
                            StackData right = stack[--sp];
                            StackData left = stack[--sp];

                            bool jmp = false;

                            switch (left.Type)
                            {
                                default: throw new NotSupportedException(left.Type.ToString());

                                case StackType.I32: jmp = left.I32 != right.I32; break;
                                case StackType.U32: jmp = (uint)left.I32 != (uint)right.I32; break;
                                case StackType.I64: jmp = left.I64 != right.I64; break;
                                case StackType.U64: jmp = (ulong)left.I64 != (ulong)right.I64; break;
                                case StackType.Ptr: jmp = left.Ptr != right.Ptr; break;
                                case StackType.UPtr: jmp = (UIntPtr)(ulong)left.Ptr != (UIntPtr)(ulong)right.Ptr; break;
                                case StackType.F32: jmp = left.F32 != right.F32; break;
                                case StackType.F64: jmp = left.F64 != right.F64; break;
                                case StackType.Ref: jmp = left.Ref != right.Ref; break;
                            }

                            // Debug execution
                            Debug.Instruction(op, pc - 2, offset);

                            // Conditional
                            if (jmp == true)
                            {
                                // Update offset
                                pc += offset;
                            }
                            break;
                        }

                    case ILOpCode.Bne_un:
                        {
                            // Fetch offset
                            int offset = FetchDecode<int>(instructions, ref pc);

                            // Pop values
                            StackData right = stack[--sp];
                            StackData left = stack[--sp];

                            bool jmp = false;

                            switch (left.Type)
                            {
                                default: throw new NotSupportedException(left.Type.ToString());

                                case StackType.I32: jmp = left.I32 != right.I32; break;
                                case StackType.U32: jmp = (uint)left.I32 != (uint)right.I32; break;
                                case StackType.I64: jmp = left.I64 != right.I64; break;
                                case StackType.U64: jmp = (ulong)left.I64 != (ulong)right.I64; break;
                                case StackType.Ptr: jmp = left.Ptr != right.Ptr; break;
                                case StackType.UPtr: jmp = (UIntPtr)(ulong)left.Ptr != (UIntPtr)(ulong)right.Ptr; break;
                                case StackType.F32: jmp = left.F32 != right.F32; break;
                                case StackType.F64: jmp = left.F64 != right.F64; break;
                                case StackType.Ref: jmp = left.Ref != right.Ref; break;
                            }

                            // Debug execution
                            Debug.Instruction(op, pc - 5, offset);

                            // Conditional
                            if (jmp == true)
                            {
                                // Update offset
                                pc += offset;
                            }
                            break;
                        }

                    case ILOpCode.Bge_s:
                        {
                            // Fetch offset
                            sbyte offset = FetchDecode<sbyte>(instructions, ref pc);

                            // Pop values
                            StackData right = stack[--sp];
                            StackData left = stack[--sp];

                            bool jmp = false;

                            switch (left.Type)
                            {
                                default: throw new NotSupportedException(left.Type.ToString());

                                case StackType.I32: jmp = left.I32 >= right.I32; break;
                                case StackType.U32: jmp = left.I32 >= right.I32; break; // Treat as signed
                                case StackType.I64: jmp = left.I64 >= right.I64; break;
                                case StackType.U64: jmp = left.I64 >= right.I64; break; // Treat as signed
                                case StackType.Ptr: jmp = (long)left.Ptr >= (long)right.Ptr; break;
                                case StackType.UPtr: jmp = (long)left.Ptr >= (long)right.Ptr; break;
                                case StackType.F32: jmp = left.F32 >= right.F32; break;
                                case StackType.F64: jmp = left.F64 >= right.F64; break;
                                case StackType.Ref: jmp = false; break; // References cannot be compared with >=
                            }

                            // Debug execution
                            Debug.Instruction(op, pc - 2, offset);

                            // Conditional
                            if (jmp == true)
                            {
                                // Update offset
                                pc += offset;
                            }
                            break;
                        }

                    case ILOpCode.Bge:
                        {
                            // Fetch offset
                            int offset = FetchDecode<int>(instructions, ref pc);

                            // Pop values
                            StackData right = stack[--sp];
                            StackData left = stack[--sp];

                            bool jmp = false;

                            switch (left.Type)
                            {
                                default: throw new NotSupportedException(left.Type.ToString());

                                case StackType.I32: jmp = left.I32 >= right.I32; break;
                                case StackType.U32: jmp = left.I32 >= right.I32; break; // Treat as signed
                                case StackType.I64: jmp = left.I64 >= right.I64; break;
                                case StackType.U64: jmp = left.I64 >= right.I64; break; // Treat as signed
                                case StackType.Ptr: jmp = (long)left.Ptr >= (long)right.Ptr; break;
                                case StackType.UPtr: jmp = (long)left.Ptr >= (long)right.Ptr; break;
                                case StackType.F32: jmp = left.F32 >= right.F32; break;
                                case StackType.F64: jmp = left.F64 >= right.F64; break;
                                case StackType.Ref: jmp = false; break; // References cannot be compared with >=
                            }

                            // Debug execution
                            Debug.Instruction(op, pc - 5, offset);

                            // Conditional
                            if (jmp == true)
                            {
                                // Update offset
                                pc += offset;
                            }
                            break;
                        }

                    case ILOpCode.Bge_un_s:
                        {
                            // Fetch offset
                            sbyte offset = FetchDecode<sbyte>(instructions, ref pc);

                            // Pop values
                            StackData right = stack[--sp];
                            StackData left = stack[--sp];

                            bool jmp = false;

                            switch (left.Type)
                            {
                                default: throw new NotSupportedException(left.Type.ToString());

                                case StackType.I32: jmp = (uint)left.I32 >= (uint)right.I32; break;
                                case StackType.U32: jmp = (uint)left.I32 >= (uint)right.I32; break;
                                case StackType.I64: jmp = (ulong)left.I64 >= (ulong)right.I64; break;
                                case StackType.U64: jmp = (ulong)left.I64 >= (ulong)right.I64; break;
                                case StackType.Ptr: jmp = (ulong)(long)left.Ptr >= (ulong)(long)right.Ptr; break;
                                case StackType.UPtr: jmp = (ulong)(long)left.Ptr >= (ulong)(long)right.Ptr; break;
                                case StackType.F32: jmp = !(left.F32 < right.F32); break; // Handle NaN properly
                                case StackType.F64: jmp = !(left.F64 < right.F64); break; // Handle NaN properly
                                case StackType.Ref: jmp = false; break; // References cannot be compared with >=
                            }

                            // Debug execution
                            Debug.Instruction(op, pc - 2, offset);

                            // Conditional
                            if (jmp == true)
                            {
                                // Update offset
                                pc += offset;
                            }
                            break;
                        }

                    case ILOpCode.Bge_un:
                        {
                            // Fetch offset
                            int offset = FetchDecode<int>(instructions, ref pc);

                            // Pop values
                            StackData right = stack[--sp];
                            StackData left = stack[--sp];

                            bool jmp = false;

                            switch (left.Type)
                            {
                                default: throw new NotSupportedException(left.Type.ToString());

                                case StackType.I32: jmp = (uint)left.I32 >= (uint)right.I32; break;
                                case StackType.U32: jmp = (uint)left.I32 >= (uint)right.I32; break;
                                case StackType.I64: jmp = (ulong)left.I64 >= (ulong)right.I64; break;
                                case StackType.U64: jmp = (ulong)left.I64 >= (ulong)right.I64; break;
                                case StackType.Ptr: jmp = (ulong)(long)left.Ptr >= (ulong)(long)right.Ptr; break;
                                case StackType.UPtr: jmp = (ulong)(long)left.Ptr >= (ulong)(long)right.Ptr; break;
                                case StackType.F32: jmp = !(left.F32 < right.F32); break; // Handle NaN properly
                                case StackType.F64: jmp = !(left.F64 < right.F64); break; // Handle NaN properly
                                case StackType.Ref: jmp = false; break; // References cannot be compared with >=
                            }

                            // Debug execution
                            Debug.Instruction(op, pc - 5, offset);

                            // Conditional
                            if (jmp == true)
                            {
                                // Update offset
                                pc += offset;
                            }
                            break;
                        }

                    case ILOpCode.Bgt_s:
                        {
                            // Fetch offset
                            sbyte offset = FetchDecode<sbyte>(instructions, ref pc);

                            // Pop values
                            StackData right = stack[--sp];
                            StackData left = stack[--sp];

                            bool jmp = false;

                            switch (left.Type)
                            {
                                default: throw new NotSupportedException(left.Type.ToString());

                                case StackType.I32: jmp = left.I32 > right.I32; break;
                                case StackType.U32: jmp = left.I32 > right.I32; break; // Treat as signed
                                case StackType.I64: jmp = left.I64 > right.I64; break;
                                case StackType.U64: jmp = left.I64 > right.I64; break; // Treat as signed
                                case StackType.Ptr: jmp = (long)left.Ptr > (long)right.Ptr; break;
                                case StackType.UPtr: jmp = (long)left.Ptr > (long)right.Ptr; break;
                                case StackType.F32: jmp = left.F32 > right.F32; break;
                                case StackType.F64: jmp = left.F64 > right.F64; break;
                                case StackType.Ref: jmp = false; break; // References cannot be compared with >
                            }

                            // Debug execution
                            Debug.Instruction(op, pc - 2, offset);

                            // Conditional
                            if (jmp == true)
                            {
                                // Update offset
                                pc += offset;
                            }
                            break;
                        }

                    case ILOpCode.Bgt:
                        {
                            // Fetch offset
                            int offset = FetchDecode<int>(instructions, ref pc);

                            // Pop values
                            StackData right = stack[--sp];
                            StackData left = stack[--sp];

                            bool jmp = false;

                            switch (left.Type)
                            {
                                default: throw new NotSupportedException(left.Type.ToString());

                                case StackType.I32: jmp = left.I32 > right.I32; break;
                                case StackType.U32: jmp = left.I32 > right.I32; break; // Treat as signed
                                case StackType.I64: jmp = left.I64 > right.I64; break;
                                case StackType.U64: jmp = left.I64 > right.I64; break; // Treat as signed
                                case StackType.Ptr: jmp = (long)left.Ptr > (long)right.Ptr; break;
                                case StackType.UPtr: jmp = (long)left.Ptr > (long)right.Ptr; break;
                                case StackType.F32: jmp = left.F32 > right.F32; break;
                                case StackType.F64: jmp = left.F64 > right.F64; break;
                                case StackType.Ref: jmp = false; break; // References cannot be compared with >
                            }

                            // Debug execution
                            Debug.Instruction(op, pc - 5, offset);

                            // Conditional
                            if (jmp == true)
                            {
                                // Update offset
                                pc += offset;
                            }
                            break;
                        }

                    case ILOpCode.Bgt_un_s:
                        {
                            // Fetch offset
                            sbyte offset = FetchDecode<sbyte>(instructions, ref pc);

                            // Pop values
                            StackData right = stack[--sp];
                            StackData left = stack[--sp];

                            bool jmp = false;

                            switch (left.Type)
                            {
                                default: throw new NotSupportedException(left.Type.ToString());

                                case StackType.I32: jmp = (uint)left.I32 > (uint)right.I32; break;
                                case StackType.U32: jmp = (uint)left.I32 > (uint)right.I32; break;
                                case StackType.I64: jmp = (ulong)left.I64 > (ulong)right.I64; break;
                                case StackType.U64: jmp = (ulong)left.I64 > (ulong)right.I64; break;
                                case StackType.Ptr: jmp = (ulong)(long)left.Ptr > (ulong)(long)right.Ptr; break;
                                case StackType.UPtr: jmp = (ulong)(long)left.Ptr > (ulong)(long)right.Ptr; break;
                                case StackType.F32: jmp = !(left.F32 <= right.F32); break; // Handle NaN properly
                                case StackType.F64: jmp = !(left.F64 <= right.F64); break; // Handle NaN properly
                                case StackType.Ref: jmp = false; break; // References cannot be compared with >
                            }

                            // Debug execution
                            Debug.Instruction(op, pc - 2, offset);

                            // Conditional
                            if (jmp == true)
                            {
                                // Update offset
                                pc += offset;
                            }
                            break;
                        }

                    case ILOpCode.Bgt_un:
                        {
                            // Fetch offset
                            int offset = FetchDecode<int>(instructions, ref pc);

                            // Pop values
                            StackData right = stack[--sp];
                            StackData left = stack[--sp];

                            bool jmp = false;

                            switch (left.Type)
                            {
                                default: throw new NotSupportedException(left.Type.ToString());

                                case StackType.I32: jmp = (uint)left.I32 > (uint)right.I32; break;
                                case StackType.U32: jmp = (uint)left.I32 > (uint)right.I32; break;
                                case StackType.I64: jmp = (ulong)left.I64 > (ulong)right.I64; break;
                                case StackType.U64: jmp = (ulong)left.I64 > (ulong)right.I64; break;
                                case StackType.Ptr: jmp = (ulong)(long)left.Ptr > (ulong)(long)right.Ptr; break;
                                case StackType.UPtr: jmp = (ulong)(long)left.Ptr > (ulong)(long)right.Ptr; break;
                                case StackType.F32: jmp = !(left.F32 <= right.F32); break; // Handle NaN properly
                                case StackType.F64: jmp = !(left.F64 <= right.F64); break; // Handle NaN properly
                                case StackType.Ref: jmp = false; break; // References cannot be compared with >
                            }

                            // Debug execution
                            Debug.Instruction(op, pc - 5, offset);

                            // Conditional
                            if (jmp == true)
                            {
                                // Update offset
                                pc += offset;
                            }
                            break;
                        }

                    case ILOpCode.Ble_s:
                        {
                            // Fetch offset
                            sbyte offset = FetchDecode<sbyte>(instructions, ref pc);

                            // Pop values
                            StackData right = stack[--sp];
                            StackData left = stack[--sp];

                            bool jmp = false;

                            switch (left.Type)
                            {
                                default: throw new NotSupportedException(left.Type.ToString());

                                case StackType.I32: jmp = left.I32 <= right.I32; break;
                                case StackType.U32: jmp = left.I32 <= right.I32; break; // Treat as signed
                                case StackType.I64: jmp = left.I64 <= right.I64; break;
                                case StackType.U64: jmp = left.I64 <= right.I64; break; // Treat as signed
                                case StackType.Ptr: jmp = (long)left.Ptr <= (long)right.Ptr; break;
                                case StackType.UPtr: jmp = (long)left.Ptr <= (long)right.Ptr; break;
                                case StackType.F32: jmp = left.F32 <= right.F32; break;
                                case StackType.F64: jmp = left.F64 <= right.F64; break;
                                case StackType.Ref: jmp = false; break; // References cannot be compared with <=
                            }

                            // Debug execution
                            Debug.Instruction(op, pc - 2, offset);

                            // Conditional
                            if (jmp == true)
                            {
                                // Update offset
                                pc += offset;
                            }
                            break;
                        }

                    case ILOpCode.Ble:
                        {
                            // Fetch offset
                            int offset = FetchDecode<int>(instructions, ref pc);

                            // Pop values
                            StackData right = stack[--sp];
                            StackData left = stack[--sp];

                            bool jmp = false;

                            switch (left.Type)
                            {
                                default: throw new NotSupportedException(left.Type.ToString());

                                case StackType.I32: jmp = left.I32 <= right.I32; break;
                                case StackType.U32: jmp = left.I32 <= right.I32; break; // Treat as signed
                                case StackType.I64: jmp = left.I64 <= right.I64; break;
                                case StackType.U64: jmp = left.I64 <= right.I64; break; // Treat as signed
                                case StackType.Ptr: jmp = (long)left.Ptr <= (long)right.Ptr; break;
                                case StackType.UPtr: jmp = (long)left.Ptr <= (long)right.Ptr; break;
                                case StackType.F32: jmp = left.F32 <= right.F32; break;
                                case StackType.F64: jmp = left.F64 <= right.F64; break;
                                case StackType.Ref: jmp = false; break; // References cannot be compared with <=
                            }

                            // Debug execution
                            Debug.Instruction(op, pc - 5, offset);

                            // Conditional
                            if (jmp == true)
                            {
                                // Update offset
                                pc += offset;
                            }
                            break;
                        }

                    case ILOpCode.Ble_un_s:
                        {
                            // Fetch offset
                            sbyte offset = FetchDecode<sbyte>(instructions, ref pc);

                            // Pop values
                            StackData right = stack[--sp];
                            StackData left = stack[--sp];

                            bool jmp = false;

                            switch (left.Type)
                            {
                                default: throw new NotSupportedException(left.Type.ToString());

                                case StackType.I32: jmp = (uint)left.I32 <= (uint)right.I32; break;
                                case StackType.U32: jmp = (uint)left.I32 <= (uint)right.I32; break;
                                case StackType.I64: jmp = (ulong)left.I64 <= (ulong)right.I64; break;
                                case StackType.U64: jmp = (ulong)left.I64 <= (ulong)right.I64; break;
                                case StackType.Ptr: jmp = (ulong)(long)left.Ptr <= (ulong)(long)right.Ptr; break;
                                case StackType.UPtr: jmp = (ulong)(long)left.Ptr <= (ulong)(long)right.Ptr; break;
                                case StackType.F32: jmp = !(left.F32 > right.F32); break; // Handle NaN properly
                                case StackType.F64: jmp = !(left.F64 > right.F64); break; // Handle NaN properly
                                case StackType.Ref: jmp = false; break; // References cannot be compared with <=
                            }

                            // Debug execution
                            Debug.Instruction(op, pc - 2, offset);

                            // Conditional
                            if (jmp == true)
                            {
                                // Update offset
                                pc += offset;
                            }
                            break;
                        }

                    case ILOpCode.Ble_un:
                        {
                            // Fetch offset
                            int offset = FetchDecode<int>(instructions, ref pc);

                            // Pop values
                            StackData right = stack[--sp];
                            StackData left = stack[--sp];

                            bool jmp = false;

                            switch (left.Type)
                            {
                                default: throw new NotSupportedException(left.Type.ToString());

                                case StackType.I32: jmp = (uint)left.I32 <= (uint)right.I32; break;
                                case StackType.U32: jmp = (uint)left.I32 <= (uint)right.I32; break;
                                case StackType.I64: jmp = (ulong)left.I64 <= (ulong)right.I64; break;
                                case StackType.U64: jmp = (ulong)left.I64 <= (ulong)right.I64; break;
                                case StackType.Ptr: jmp = (ulong)(long)left.Ptr <= (ulong)(long)right.Ptr; break;
                                case StackType.UPtr: jmp = (ulong)(long)left.Ptr <= (ulong)(long)right.Ptr; break;
                                case StackType.F32: jmp = !(left.F32 > right.F32); break; // Handle NaN properly
                                case StackType.F64: jmp = !(left.F64 > right.F64); break; // Handle NaN properly
                                case StackType.Ref: jmp = false; break; // References cannot be compared with <=
                            }

                            // Debug execution
                            Debug.Instruction(op, pc - 5, offset);

                            // Conditional
                            if (jmp == true)
                            {
                                // Update offset
                                pc += offset;
                            }
                            break;
                        }

                    case ILOpCode.Blt_s:
                        {
                            // Fetch offset
                            sbyte offset = FetchDecode<sbyte>(instructions, ref pc);

                            // Pop values
                            StackData right = stack[--sp];
                            StackData left = stack[--sp];

                            bool jmp = false;

                            switch (left.Type)
                            {
                                default: throw new NotSupportedException(left.Type.ToString());

                                case StackType.I32: jmp = left.I32 < right.I32; break;
                                case StackType.U32: jmp = left.I32 < right.I32; break; // Treat as signed
                                case StackType.I64: jmp = left.I64 < right.I64; break;
                                case StackType.U64: jmp = left.I64 < right.I64; break; // Treat as signed
                                case StackType.Ptr: jmp = (long)left.Ptr < (long)right.Ptr; break;
                                case StackType.UPtr: jmp = (long)left.Ptr < (long)right.Ptr; break;
                                case StackType.F32: jmp = left.F32 < right.F32; break;
                                case StackType.F64: jmp = left.F64 < right.F64; break;
                                case StackType.Ref: jmp = false; break; // References cannot be compared with <
                            }

                            // Debug execution
                            Debug.Instruction(op, pc - 2, offset);

                            // Conditional
                            if (jmp == true)
                            {
                                // Update offset
                                pc += offset;
                            }
                            break;
                        }

                    case ILOpCode.Blt:
                        {
                            // Fetch offset
                            int offset = FetchDecode<int>(instructions, ref pc);

                            // Pop values
                            StackData right = stack[--sp];
                            StackData left = stack[--sp];

                            bool jmp = false;

                            switch (left.Type)
                            {
                                default: throw new NotSupportedException(left.Type.ToString());

                                case StackType.I32: jmp = left.I32 < right.I32; break;
                                case StackType.U32: jmp = left.I32 < right.I32; break; // Treat as signed
                                case StackType.I64: jmp = left.I64 < right.I64; break;
                                case StackType.U64: jmp = left.I64 < right.I64; break; // Treat as signed
                                case StackType.Ptr: jmp = (long)left.Ptr < (long)right.Ptr; break;
                                case StackType.UPtr: jmp = (long)left.Ptr < (long)right.Ptr; break;
                                case StackType.F32: jmp = left.F32 < right.F32; break;
                                case StackType.F64: jmp = left.F64 < right.F64; break;
                                case StackType.Ref: jmp = false; break; // References cannot be compared with <
                            }

                            // Debug execution
                            Debug.Instruction(op, pc - 5, offset);

                            // Conditional
                            if (jmp == true)
                            {
                                // Update offset
                                pc += offset;
                            }
                            break;
                        }

                    case ILOpCode.Blt_un_s:
                        {
                            // Fetch offset
                            sbyte offset = FetchDecode<sbyte>(instructions, ref pc);

                            // Pop values
                            StackData right = stack[--sp];
                            StackData left = stack[--sp];

                            bool jmp = false;

                            switch (left.Type)
                            {
                                default: throw new NotSupportedException(left.Type.ToString());

                                case StackType.I32: jmp = (uint)left.I32 < (uint)right.I32; break;
                                case StackType.U32: jmp = (uint)left.I32 < (uint)right.I32; break;
                                case StackType.I64: jmp = (ulong)left.I64 < (ulong)right.I64; break;
                                case StackType.U64: jmp = (ulong)left.I64 < (ulong)right.I64; break;
                                case StackType.Ptr: jmp = (ulong)(long)left.Ptr < (ulong)(long)right.Ptr; break;
                                case StackType.UPtr: jmp = (ulong)(long)left.Ptr < (ulong)(long)right.Ptr; break;
                                case StackType.F32: jmp = !(left.F32 >= right.F32); break; // Handle NaN properly
                                case StackType.F64: jmp = !(left.F64 >= right.F64); break; // Handle NaN properly
                                case StackType.Ref: jmp = false; break; // References cannot be compared with <
                            }

                            // Debug execution
                            Debug.Instruction(op, pc - 2, offset);

                            // Conditional
                            if (jmp == true)
                            {
                                // Update offset
                                pc += offset;
                            }
                            break;
                        }

                    case ILOpCode.Blt_un:
                        {
                            // Fetch offset
                            int offset = FetchDecode<int>(instructions, ref pc);

                            // Pop values
                            StackData right = stack[--sp];
                            StackData left = stack[--sp];

                            bool jmp = false;

                            switch (left.Type)
                            {
                                default: throw new NotSupportedException(left.Type.ToString());

                                case StackType.I32: jmp = (uint)left.I32 < (uint)right.I32; break;
                                case StackType.U32: jmp = (uint)left.I32 < (uint)right.I32; break;
                                case StackType.I64: jmp = (ulong)left.I64 < (ulong)right.I64; break;
                                case StackType.U64: jmp = (ulong)left.I64 < (ulong)right.I64; break;
                                case StackType.Ptr: jmp = (ulong)(long)left.Ptr < (ulong)(long)right.Ptr; break;
                                case StackType.UPtr: jmp = (ulong)(long)left.Ptr < (ulong)(long)right.Ptr; break;
                                case StackType.F32: jmp = !(left.F32 >= right.F32); break; // Handle NaN properly
                                case StackType.F64: jmp = !(left.F64 >= right.F64); break; // Handle NaN properly
                                case StackType.Ref: jmp = false; break; // References cannot be compared with <
                            }

                            // Debug execution
                            Debug.Instruction(op, pc - 5, offset);

                            // Conditional
                            if (jmp == true)
                            {
                                // Update offset
                                pc += offset;
                            }
                            break;
                        }

                    case ILOpCode.Switch:
                        {
                            int pcStart = pc;

                            // Get the number of switch cases
                            int length = FetchDecode<int>(instructions, ref pc);
                            int offset = 0; // Fall through??

                            // Pop jump index
                            StackData index = stack[--sp];

                            // Check bounds
                            if (index.I32 >= 0 && index.I32 < length)
                            {
                                // Calculate the pc where the jump offset is stored
                                int pcOffset = pc + (sizeof(int) * index.I32);

                                // Try to read the offset from the table
                                offset = FetchDecode<int>(instructions, ref pcOffset);
                            }

                            // Update pc to end of the table
                            pc += sizeof(int) * length;

                            // Jump to offset
                            pc += offset;

                            // Debug execution
                            Debug.Instruction(op, pcStart - 1, index);
                            break;
                        }
                    #endregion

                    #region Indirect
                    case ILOpCode.Ldind_i:
                        {
                            // Pop address
                            StackData address = stack[sp - 1];

                            // Read native int
                            stack[sp - 1].Ptr = ((IByRef)address.Ref).GetValueI();
                            stack[sp - 1].Type = StackType.Ptr;

                            // Debug execution
                            Debug.Instruction(op, pc - 1, stack[sp - 1]);
                            break;
                        }
                    case ILOpCode.Ldind_i1:
                        {
                            // Pop address
                            StackData address = stack[sp - 1];

                            // Read sbyte
                            stack[sp - 1].I32 = ((IByRef)address.Ref).GetValueI1();
                            stack[sp - 1].Type = StackType.I32;

                            // Debug execution
                            Debug.Instruction(op, pc - 1, stack[sp - 1]);
                            break;
                        }
                    case ILOpCode.Ldind_i2:
                        {
                            // Pop address
                            StackData address = stack[sp - 1];

                            // Read short
                            stack[sp - 1].I32 = ((IByRef)address.Ref).GetValueI2();
                            stack[sp - 1].Type = StackType.I32;

                            // Debug execution
                            Debug.Instruction(op, pc - 1, stack[sp - 1]);
                            break;
                        }
                    case ILOpCode.Ldind_i4:
                        {
                            // Pop address
                            StackData address = stack[sp - 1];

                            // Read int
                            stack[sp - 1].I32 = ((IByRef)address.Ref).GetValueI4();
                            stack[sp - 1].Type = StackType.I32;

                            // Debug execution
                            Debug.Instruction(op, pc - 1, stack[sp - 1]);
                            break;
                        }
                    case ILOpCode.Ldind_i8:
                        {
                            // Pop address
                            StackData address = stack[sp - 1];

                            // Read long
                            stack[sp - 1].I64 = ((IByRef)address.Ref).GetValueI8();
                            stack[sp - 1].Type = StackType.I64;

                            // Debug execution
                            Debug.Instruction(op, pc - 1, stack[sp - 1]);
                            break;
                        }
                    case ILOpCode.Ldind_u1:
                        {
                            // Pop address
                            StackData address = stack[sp - 1];

                            // Read byte
                            stack[sp - 1].I32 = ((IByRef)address.Ref).GetValueU1();
                            stack[sp - 1].Type = StackType.I32;

                            // Debug execution
                            Debug.Instruction(op, pc - 1, stack[sp - 1]);
                            break;
                        }
                    case ILOpCode.Ldind_u2:
                        {
                            // Pop address
                            StackData address = stack[sp - 1];

                            // Read ushort
                            stack[sp - 1].I32 = ((IByRef)address.Ref).GetValueU2();
                            stack[sp - 1].Type = StackType.I32;

                            // Debug execution
                            Debug.Instruction(op, pc - 1, stack[sp - 1]);
                            break;
                        }
                    case ILOpCode.Ldind_u4:
                        {
                            // Pop address
                            StackData address = stack[sp - 1];

                            // Read uint
                            stack[sp - 1].I32 = (int)((IByRef)address.Ref).GetValueU4();
                            stack[sp - 1].Type = StackType.I32;

                            // Debug execution
                            Debug.Instruction(op, pc - 1, stack[sp - 1]);
                            break;
                        }
                    case ILOpCode.Ldind_r4:
                        {
                            // Pop address
                            StackData address = stack[sp - 1];

                            // Read float
                            stack[sp - 1].F32 = ((IByRef)address.Ref).GetValueR4();
                            stack[sp - 1].Type = StackType.F32;

                            // Debug execution
                            Debug.Instruction(op, pc - 1, stack[sp - 1]);
                            break;
                        }
                    case ILOpCode.Ldind_r8:
                        {
                            // Pop address
                            StackData address = stack[sp - 1];

                            // Read double
                            stack[sp - 1].F64 = ((IByRef)address.Ref).GetValueR8();
                            stack[sp - 1].Type = StackType.F64;

                            // Debug execution
                            Debug.Instruction(op, pc - 1, stack[sp - 1]);
                            break;
                        }
                    case ILOpCode.Ldind_ref:
                        {
                            // Pop address
                            StackData address = stack[sp - 1];

                            // Read ref
                            stack[sp - 1].Ref = ((IByRef)address.Ref).GetValueRef();
                            stack[sp - 1].Type = StackType.Ref;

                            // Debug execution
                            Debug.Instruction(op, pc - 1, stack[sp - 1]);
                            break;
                        }

                    case ILOpCode.Stind_i:
                        {
                            // Pop value and address
                            StackData value = stack[--sp];
                            StackData address = stack[--sp];

                            // Write native int
                            ((IByRef)address.Ref).SetValueI(value.Ptr);

                            // Debug execution
                            Debug.Instruction(op, pc - 1, value);
                            break;
                        }
                    case ILOpCode.Stind_i1:
                        {
                            // Pop value and address
                            StackData value = stack[--sp];
                            StackData address = stack[--sp];

                            // Write sbyte
                            ((IByRef)address.Ref).SetValueI1((sbyte)value.I32);

                            // Debug execution
                            Debug.Instruction(op, pc - 1, value);
                            break;
                        }
                    case ILOpCode.Stind_i2:
                        {
                            // Pop value and address
                            StackData value = stack[--sp];
                            StackData address = stack[--sp];

                            // Write short
                            ((IByRef)address.Ref).SetValueI2((short)value.I32);

                            // Debug execution
                            Debug.Instruction(op, pc - 1, value);
                            break;
                        }
                    case ILOpCode.Stind_i4:
                        {
                            // Pop value and address
                            StackData value = stack[--sp];
                            StackData address = stack[--sp];

                            // Write int
                            ((IByRef)address.Ref).SetValueI4((int)value.I32);

                            // Debug execution
                            Debug.Instruction(op, pc - 1, value);
                            break;
                        }
                    case ILOpCode.Stind_i8:
                        {
                            // Pop value and address
                            StackData value = stack[--sp];
                            StackData address = stack[--sp];

                            // Write long
                            ((IByRef)address.Ref).SetValueI8((long)value.I32);

                            // Debug execution
                            Debug.Instruction(op, pc - 1, value);
                            break;
                        }
                    case ILOpCode.Stind_r4:
                        {
                            // Pop value and address
                            StackData value = stack[--sp];
                            StackData address = stack[--sp];

                            // Write float
                            ((IByRef)address.Ref).SetValueR4((float)value.F32);

                            // Debug execution
                            Debug.Instruction(op, pc - 1, value);
                            break;
                        }
                    case ILOpCode.Stind_r8:
                        {
                            // Pop value and address
                            StackData value = stack[--sp];
                            StackData address = stack[--sp];

                            // Write double
                            ((IByRef)address.Ref).SetValueR8((double)value.F64);

                            // Debug execution
                            Debug.Instruction(op, pc - 1, value);
                            break;
                        }
                    case ILOpCode.Stind_ref:
                        {
                            // Pop value and address
                            StackData value = stack[--sp];
                            StackData address = stack[--sp];

                            // Write reference
                            ((IByRef)address.Ref).SetValueRef(value.Ref);

                            // Debug execution
                            Debug.Instruction(op, pc - 1, value);
                            break;
                        }
                    #endregion

                    #region Array
                    case ILOpCode.Newarr:
                        {
                            // Read the element token
                            int token = FetchDecode<int>(instructions, ref pc);

                            // Get handle
                            EntityHandle typeHandle = MetadataTokens.EntityHandle(token);

                            // Get the element type info
                            CILTypeInfo elementType = loadContext.GetTypeHandle(typeHandle);

                            // Check for long length
                            if (stack[sp - 1].Type == StackType.I64)
                            {
                                // Allocate array long form
                                __gc.AllocateArrayL(loadContext.AppDomain, elementType, stack[sp - 1].I64, ref stack[sp - 1]);
                            }
                            // Use int length
                            else
                            {
                                // Allocate array short form
                                __gc.AllocateArrayS(loadContext.AppDomain, elementType, stack[sp - 1].I32, ref stack[sp - 1]);
                            }

                            // Log execution
                            Debug.Instruction(op, pc - 5, stack[sp - 1]);
                            break;
                        }

                    case ILOpCode.Ldlen:
                        {
                            // Pop array reference
                            StackData instance = stack[--sp];

                            // Check for null
                            Array array;
                            if ((array = instance.Ref as Array) == null)
                                threadContext.Throw<NullReferenceException>();

                            // Push length to stack as native int
                            stack[sp].Ptr = (IntPtr)array.Length;
                            stack[sp++].Type = StackType.Ptr;

                            // Debug execution
                            Debug.Instruction(op, pc - 1, stack[sp - 1]);
                            break;
                        }

                    case ILOpCode.Ldelem_i:
                        {
                            // Pop index and instance
                            StackData index = stack[--sp];
                            StackData instance = stack[--sp];

                            // Check for null
                            Array array;
                            if ((array = instance.Ref as Array) == null)
                                threadContext.Throw<NullReferenceException>();

                            // Get index
                            long explicitIndex = index.Type == StackType.I64
                                ? index.I64 : index.I32;

                            // Check bounds
                            if (explicitIndex < 0 || explicitIndex >= array.Length)
                                threadContext.Throw<IndexOutOfRangeException>();

                            // Push to stack
                            stack[sp].Ptr = ((IntPtr[])array)[explicitIndex];
                            stack[sp++].Type = StackType.Ptr;

                            // Debug execution
                            Debug.Instruction(op, pc - 1, stack[sp - 1]);
                            break;
                        }

                    case ILOpCode.Ldelem_i1:
                        {
                            // Pop index and instance
                            StackData index = stack[--sp];
                            StackData instance = stack[--sp];

                            // Check for null
                            Array array;
                            if ((array = instance.Ref as Array) == null)
                                threadContext.Throw<NullReferenceException>();

                            // Get index
                            long explicitIndex = index.Type == StackType.I64
                                ? index.I64 : index.I32;

                            // Check bounds
                            if (explicitIndex < 0 || explicitIndex >= array.Length)
                                threadContext.Throw<IndexOutOfRangeException>();

                            // Push to stack as I32 (signed byte promoted to I32)
                            stack[sp].I32 = ((sbyte[])array)[explicitIndex];
                            stack[sp++].Type = StackType.I32;

                            // Debug execution
                            Debug.Instruction(op, pc - 1, stack[sp - 1]);
                            break;
                        }

                    case ILOpCode.Ldelem_u1:
                        {
                            // Pop index and instance
                            StackData index = stack[--sp];
                            StackData instance = stack[--sp];

                            // Check for null
                            Array array;
                            if ((array = instance.Ref as Array) == null)
                                threadContext.Throw<NullReferenceException>();

                            // Get index
                            long explicitIndex = index.Type == StackType.I64
                                ? index.I64 : index.I32;

                            // Check bounds
                            if (explicitIndex < 0 || explicitIndex >= array.Length)
                                threadContext.Throw<IndexOutOfRangeException>();

                            // Push to stack as I32 (unsigned byte promoted to I32)
                            stack[sp].I32 = ((byte[])array)[explicitIndex];
                            stack[sp++].Type = StackType.I32;

                            // Debug execution
                            Debug.Instruction(op, pc - 1, stack[sp - 1]);
                            break;
                        }

                    case ILOpCode.Ldelem_i2:
                        {
                            // Pop index and instance
                            StackData index = stack[--sp];
                            StackData instance = stack[--sp];

                            // Check for null
                            Array array;
                            if ((array = instance.Ref as Array) == null)
                                threadContext.Throw<NullReferenceException>();

                            // Get index
                            long explicitIndex = index.Type == StackType.I64
                                ? index.I64 : index.I32;

                            // Check bounds
                            if (explicitIndex < 0 || explicitIndex >= array.Length)
                                threadContext.Throw<IndexOutOfRangeException>();

                            // Push to stack as I32 (signed short promoted to I32)
                            stack[sp].I32 = ((short[])array)[explicitIndex];
                            stack[sp++].Type = StackType.I32;

                            // Debug execution
                            Debug.Instruction(op, pc - 1, stack[sp - 1]);
                            break;
                        }

                    case ILOpCode.Ldelem_u2:
                        {
                            // Pop index and instance
                            StackData index = stack[--sp];
                            StackData instance = stack[--sp];

                            // Check for null
                            Array array;
                            if ((array = instance.Ref as Array) == null)
                                threadContext.Throw<NullReferenceException>();

                            // Get index
                            long explicitIndex = index.Type == StackType.I64
                                ? index.I64 : index.I32;

                            // Check bounds
                            if (explicitIndex < 0 || explicitIndex >= array.Length)
                                threadContext.Throw<IndexOutOfRangeException>();

                            // Push to stack as I32 (unsigned short promoted to I32)
                            stack[sp].I32 = ((ushort[])array)[explicitIndex];
                            stack[sp++].Type = StackType.I32;

                            // Debug execution
                            Debug.Instruction(op, pc - 1, stack[sp - 1]);
                            break;
                        }

                    case ILOpCode.Ldelem_i4:
                        {
                            // Pop index and instance
                            StackData index = stack[--sp];
                            StackData instance = stack[--sp];

                            // Check for null
                            Array array;
                            if ((array = instance.Ref as Array) == null)
                                threadContext.Throw<NullReferenceException>();

                            // Get index
                            long explicitIndex = index.Type == StackType.I64
                                    ? index.I64 : index.I32;

                            // Check bounds
                            if (explicitIndex < 0 || explicitIndex >= array.Length)
                                threadContext.Throw<IndexOutOfRangeException>();

                            // Push to stack as I32
                            stack[sp].I32 = ((int[])array)[explicitIndex];
                            stack[sp++].Type = StackType.I32;

                            // Debug execution
                            Debug.Instruction(op, pc - 1, stack[sp - 1]);
                            break;
                        }

                    case ILOpCode.Ldelem_u4:
                        {
                            // Pop index and instance
                            StackData index = stack[--sp];
                            StackData instance = stack[--sp];

                            // Check for null
                            Array array;
                            if ((array = instance.Ref as Array) == null)
                                threadContext.Throw<NullReferenceException>();

                            // Get index
                            long explicitIndex = index.Type == StackType.I64
                                ? index.I64 : index.I32;

                            // Check bounds
                            if (explicitIndex < 0 || explicitIndex >= array.Length)
                                threadContext.Throw<IndexOutOfRangeException>();

                            // Push to stack as U32 (stored as I32)
                            stack[sp].I32 = (int)((uint[])array)[explicitIndex];
                            stack[sp++].Type = StackType.U32;

                            // Debug execution
                            Debug.Instruction(op, pc - 1, stack[sp - 1]);
                            break;
                        }

                    case ILOpCode.Ldelem_i8:
                        {
                            // Pop index and instance
                            StackData index = stack[--sp];
                            StackData instance = stack[--sp];

                            // Check for null
                            Array array;
                            if ((array = instance.Ref as Array) == null)
                                threadContext.Throw<NullReferenceException>();

                            // Get index
                            long explicitIndex = index.Type == StackType.I64
                                ? index.I64 : index.I32;

                            // Check bounds
                            if (explicitIndex < 0 || explicitIndex >= array.Length)
                                threadContext.Throw<IndexOutOfRangeException>();

                            // Push to stack as I64
                            stack[sp].I64 = ((long[])array)[explicitIndex];
                            stack[sp++].Type = StackType.I64;

                            // Debug execution
                            Debug.Instruction(op, pc - 1, stack[sp - 1]);
                            break;
                        }

                    case ILOpCode.Ldelem_r4:
                        {
                            // Pop index and instance
                            StackData index = stack[--sp];
                            StackData instance = stack[--sp];

                            // Check for null
                            Array array;
                            if ((array = instance.Ref as Array) == null)
                                threadContext.Throw<NullReferenceException>();

                            // Get index
                            long explicitIndex = index.Type == StackType.I64
                                ? index.I64 : index.I32;

                            // Check bounds
                            if (explicitIndex < 0 || explicitIndex >= array.Length)
                                threadContext.Throw<IndexOutOfRangeException>();

                            // Push to stack as F32
                            stack[sp].F32 = ((float[])array)[explicitIndex];
                            stack[sp++].Type = StackType.F32;

                            // Debug execution
                            Debug.Instruction(op, pc - 1, stack[sp - 1]);
                            break;
                        }

                    case ILOpCode.Ldelem_r8:
                        {
                            // Pop index and instance
                            StackData index = stack[--sp];
                            StackData instance = stack[--sp];

                            // Check for null
                            Array array;
                            if ((array = instance.Ref as Array) == null)
                                threadContext.Throw<NullReferenceException>();

                            // Get index
                            long explicitIndex = index.Type == StackType.I64
                                ? index.I64 : index.I32;

                            // Check bounds
                            if (explicitIndex < 0 || explicitIndex >= array.Length)
                                threadContext.Throw<IndexOutOfRangeException>();

                            // Push to stack as F64
                            stack[sp].F64 = ((double[])array)[explicitIndex];
                            stack[sp++].Type = StackType.F64;

                            // Debug execution
                            Debug.Instruction(op, pc - 1, stack[sp - 1]);
                            break;
                        }

                    case ILOpCode.Ldelem_ref:
                        {
                            // Pop index and instance
                            StackData index = stack[--sp];
                            StackData instance = stack[--sp];

                            // Check for null
                            Array array;
                            if ((array = instance.Ref as Array) == null)
                                threadContext.Throw<NullReferenceException>();

                            // Get index
                            long explicitIndex = index.Type == StackType.I64
                                ? index.I64 : index.I32;

                            // Check bounds
                            if (explicitIndex < 0 || explicitIndex >= array.Length)
                                threadContext.Throw<IndexOutOfRangeException>();

                            // Push to stack as reference
                            stack[sp].Ref = ((object[])array)[explicitIndex];
                            stack[sp++].Type = StackType.Ref;

                            // Debug execution
                            Debug.Instruction(op, pc - 1, stack[sp - 1]);
                            break;
                        }

                    case ILOpCode.Stelem_i:
                        {
                            // Pop value, index and instance
                            StackData value = stack[--sp];
                            StackData index = stack[--sp];
                            StackData instance = stack[--sp];

                            // Check for null
                            Array array;
                            if ((array = instance.Ref as Array) == null)
                                threadContext.Throw<NullReferenceException>();

                            // Get index
                            long explicitIndex = index.Type == StackType.I64
                                ? index.I64 : index.I32;

                            // Check bounds
                            if (explicitIndex < 0 || explicitIndex >= array.Length)
                                threadContext.Throw<IndexOutOfRangeException>();

                            // Store value to array
                            ((IntPtr[])array)[explicitIndex] = value.Ptr;

                            // Debug execution
                            Debug.Instruction(op, pc - 1);
                            break;
                        }

                    case ILOpCode.Stelem_i1:
                        {
                            // Pop value, index and instance
                            StackData value = stack[--sp];
                            StackData index = stack[--sp];
                            StackData instance = stack[--sp];

                            // Check for null
                            Array array;
                            if ((array = instance.Ref as Array) == null)
                                threadContext.Throw<NullReferenceException>();

                            // Get index
                            long explicitIndex = index.Type == StackType.I64
                                ? index.I64 : index.I32;

                            // Check bounds
                            if (explicitIndex < 0 || explicitIndex >= array.Length)
                                threadContext.Throw<IndexOutOfRangeException>();

                            // Store value to array (truncate to signed byte)
                            ((sbyte[])array)[explicitIndex] = (sbyte)value.I32;

                            // Debug execution
                            Debug.Instruction(op, pc - 1);
                            break;
                        }

                    case ILOpCode.Stelem_i2:
                        {
                            // Pop value, index and instance
                            StackData value = stack[--sp];
                            StackData index = stack[--sp];
                            StackData instance = stack[--sp];

                            // Check for null
                            Array array;
                            if ((array = instance.Ref as Array) == null)
                                threadContext.Throw<NullReferenceException>();

                            // Get index
                            long explicitIndex = index.Type == StackType.I64
                                ? index.I64 : index.I32;

                            // Check bounds
                            if (explicitIndex < 0 || explicitIndex >= array.Length)
                                threadContext.Throw<IndexOutOfRangeException>();

                            // Store value to array (truncate to signed short)
                            ((short[])array)[explicitIndex] = (short)value.I32;

                            // Debug execution
                            Debug.Instruction(op, pc - 1);
                            break;
                        }

                    case ILOpCode.Stelem_i4:
                        {
                            // Pop value, index and instance
                            StackData value = stack[--sp];
                            StackData index = stack[--sp];
                            StackData instance = stack[--sp];

                            // Check for null
                            Array array;
                            if ((array = instance.Ref as Array) == null)
                                threadContext.Throw<NullReferenceException>();

                            // Get index
                            long explicitIndex = index.Type == StackType.I64
                                ? index.I64 : index.I32;

                            // Check bounds
                            if (explicitIndex < 0 || explicitIndex >= array.Length)
                                threadContext.Throw<IndexOutOfRangeException>();

                            // Store value to array
                            ((int[])array)[explicitIndex] = value.I32;

                            // Debug execution
                            Debug.Instruction(op, pc - 1);
                            break;
                        }

                    case ILOpCode.Stelem_i8:
                        {
                            // Pop value, index and instance
                            StackData value = stack[--sp];
                            StackData index = stack[--sp];
                            StackData instance = stack[--sp];

                            // Check for null
                            Array array;
                            if ((array = instance.Ref as Array) == null)
                                threadContext.Throw<NullReferenceException>();

                            // Get index
                            long explicitIndex = index.Type == StackType.I64
                                ? index.I64 : index.I32;

                            // Check bounds
                            if (explicitIndex < 0 || explicitIndex >= array.Length)
                                threadContext.Throw<IndexOutOfRangeException>();

                            // Store value to array
                            ((long[])array)[explicitIndex] = value.I64;

                            // Debug execution
                            Debug.Instruction(op, pc - 1);
                            break;
                        }

                    case ILOpCode.Stelem_r4:
                        {
                            // Pop value, index and instance
                            StackData value = stack[--sp];
                            StackData index = stack[--sp];
                            StackData instance = stack[--sp];

                            // Check for null
                            Array array;
                            if ((array = instance.Ref as Array) == null)
                                threadContext.Throw<NullReferenceException>();

                            // Get index
                            long explicitIndex = index.Type == StackType.I64
                                ? index.I64 : index.I32;

                            // Check bounds
                            if (explicitIndex < 0 || explicitIndex >= array.Length)
                                threadContext.Throw<IndexOutOfRangeException>();

                            // Store value to array
                            ((float[])array)[explicitIndex] = value.F32;

                            // Debug execution
                            Debug.Instruction(op, pc - 1);
                            break;
                        }

                    case ILOpCode.Stelem_r8:
                        {
                            // Pop value, index and instance
                            StackData value = stack[--sp];
                            StackData index = stack[--sp];
                            StackData instance = stack[--sp];

                            // Check for null
                            Array array;
                            if ((array = instance.Ref as Array) == null)
                                threadContext.Throw<NullReferenceException>();

                            // Get index
                            long explicitIndex = index.Type == StackType.I64
                                ? index.I64 : index.I32;

                            // Check bounds
                            if (explicitIndex < 0 || explicitIndex >= array.Length)
                                threadContext.Throw<IndexOutOfRangeException>();

                            // Store value to array
                            ((double[])array)[explicitIndex] = value.F64;

                            // Debug execution
                            Debug.Instruction(op, pc - 1);
                            break;
                        }

                    case ILOpCode.Stelem_ref:
                        {
                            // Pop value, index and instance
                            StackData value = stack[--sp];
                            StackData index = stack[--sp];
                            StackData instance = stack[--sp];

                            // Check for null
                            Array array;
                            if ((array = instance.Ref as Array) == null)
                                threadContext.Throw<NullReferenceException>();

                            // Get index
                            long explicitIndex = index.Type == StackType.I64
                                ? index.I64 : index.I32;

                            // Check bounds
                            if (explicitIndex < 0 || explicitIndex >= array.Length)
                                threadContext.Throw<IndexOutOfRangeException>();

                            // Store value to array
                            ((object[])array)[explicitIndex] = value.Ref;

                            // Debug execution
                            Debug.Instruction(op, pc - 1);
                            break;
                        }

                    case ILOpCode.Ldelem:
                        {
                            // Read the element token
                            int token = FetchDecode<int>(instructions, ref pc);

                            // Get handle
                            EntityHandle typeHandle = MetadataTokens.EntityHandle(token);

                            // Get the element type info
                            CILTypeInfo elementType = loadContext.GetTypeHandle(typeHandle);

                            // Pop index and instance
                            StackData index = stack[--sp];
                            StackData instance = stack[--sp];

                            // Check for null
                            Array array;
                            if ((array = instance.Ref as Array) == null)
                                threadContext.Throw<NullReferenceException>();

                            // Get index
                            long explicitIndex = index.Type == StackType.I64
                                ? index.I64 : index.I32;

                            // Check bounds
                            if (explicitIndex < 0 || explicitIndex >= array.Length)
                                threadContext.Throw<IndexOutOfRangeException>();

                            // Get the element value and wrap it onto stack
                            object value = array.GetValue(explicitIndex);
                            StackData.Wrap(elementType, value, ref stack[sp]);
                            sp++;

                            // Debug execution
                            Debug.Instruction(op, pc - 5, stack[sp - 1]);
                            break;
                        }

                    case ILOpCode.Stelem:
                        {
                            // Read the element token
                            int token = FetchDecode<int>(instructions, ref pc);

                            // Get handle
                            EntityHandle typeHandle = MetadataTokens.EntityHandle(token);

                            // Get the element type info
                            CILTypeInfo elementType = loadContext.GetTypeHandle(typeHandle);

                            // Pop value, index and instance
                            StackData value = stack[--sp];
                            StackData index = stack[--sp];
                            StackData instance = stack[--sp];

                            // Check for null
                            Array array;
                            if ((array = instance.Ref as Array) == null)
                                threadContext.Throw<NullReferenceException>();

                            // Get index
                            long explicitIndex = index.Type == StackType.I64
                                ? index.I64 : index.I32;

                            // Check bounds
                            if (explicitIndex < 0 || explicitIndex >= array.Length)
                                threadContext.Throw<IndexOutOfRangeException>();

                            // Unwrap the value and store it in the array
                            object unwrappedValue = null;
                            StackData.Unwrap(elementType, value, ref unwrappedValue);
                            array.SetValue(unwrappedValue, explicitIndex);

                            // Debug execution
                            Debug.Instruction(op, pc - 5);
                            break;
                        }

                    case ILOpCode.Ldelema:
                        {
                            // Read the element token
                            int token = FetchDecode<int>(instructions, ref pc);

                            // Get handle
                            EntityHandle typeHandle = MetadataTokens.EntityHandle(token);

                            // Get the element type info
                            CILTypeInfo elementType = loadContext.GetTypeHandle(typeHandle);

                            // Pop index and instance
                            StackData index = stack[--sp];
                            StackData instance = stack[--sp];

                            // Check for null
                            Array array;
                            if ((array = instance.Ref as Array) == null)
                                threadContext.Throw<NullReferenceException>();

                            // Get index
                            long explicitIndex = index.Type == StackType.I64
                                ? index.I64 : index.I32;

                            // Check bounds
                            if (explicitIndex < 0 || explicitIndex >= array.Length)
                                threadContext.Throw<IndexOutOfRangeException>();

                            // Create a managed reference to the array element
                            stack[sp].Ref = IByRef.MakeByRefElement(array, (int)explicitIndex);
                            stack[sp++].Type = StackType.ByRef;

                            // Debug execution
                            Debug.Instruction(op, pc - 5, stack[sp - 1]);
                            break;
                        }
                    #endregion

                    #region Object
                    case ILOpCode.Newobj:
                        {
                            // Get method token
                            int token = FetchDecode<int>(instructions, ref pc);

                            // Get handle
                            EntityHandle ctorHandle = MetadataTokens.EntityHandle(token);

                            // Load the method
                            CILMethodInfo ctorMethod = loadContext.GetMethodHandle(ctorHandle);
                            
                            // Get load context
                            AssemblyLoadContext ctorLoadContext = ctorMethod.Method.GetLoadContext();

                            // Get the stack index where the method arguments were loaded
                            int spArgCaller = sp - ctorMethod.ParameterTypes.Length;

                            // Check for interop
                            bool interop = (ctorMethod.Flags & CILMethodFlags.Interop) != 0;

                            // Prepare the call
                            threadContext.PushMethodFrame(loadContext.AppDomain, ctorMethod, interop ? CallInstance.NoInstance : CallInstance.NewObjectInstance, spArgCaller, sp, out int spCall);

                            // Debug execution
                            Debug.Instruction(op, pc - 5, ctorMethod.Method, spCall, ctorMethod.ParameterTypes.Length);

                            // Execute the method
                            if ((ctorMethod.Flags & CILMethodFlags.Interpreted) != 0)
                            {
                                // Create the new instance
                                __gc.AllocateObject(loadContext.AppDomain, ctorMethod.DeclaringType, ref stack[spCall]);

                                // Execute the method
                                ExecuteMethod(threadContext, ctorLoadContext, ctorMethod, spCall);
                            }
                            // Check for interop
                            else if((ctorMethod.Flags & CILMethodFlags.Interop) != 0)
                            {
                                // Invoke with marshal
                                // The instance may be created by an interop binding, otherwise it will be allocated by the interop ctor
                                __marshal.InvokeConstructorInterop(threadContext, loadContext.AppDomain, ctorMethod.DeclaringType, ctorMethod, spCall, spCall);
                            }
                            else
                                throw new NotSupportedException("Constructor cannot be executed: " + ctorMethod);

                            // Decrement stack pointer
                            sp -= ctorMethod.ParameterTypes.Length;

                            // Note we do not do a frame copy here - just a simple copy
                            stack[sp] = stack[spCall];
                            sp++;

                            // Pop the frame
                            threadContext.PopMethodFrame();
                            break;
                        }

                    case ILOpCode.Call:
                    case ILOpCode.Callvirt:
                        {
                            // Get method token
                            int token = FetchDecode<int>(instructions, ref pc);

                            // Get handle
                            EntityHandle callHandle = MetadataTokens.EntityHandle(token);

                            // Load the method
                            CILMethodInfo callMethod = loadContext.GetMethodHandle(callHandle);

                            // Get load context
                            AssemblyLoadContext callLoadContext = callMethod.Method.GetLoadContext();                            

                            // Get argument count including instance
                            int argCount = (callMethod.Flags & CILMethodFlags.This) != 0
                                ? callMethod.ParameterTypes.Length + 1
                                : callMethod.ParameterTypes.Length;

                            // Get the stack index where the method arguments were loaded
                            int spArgCaller = sp - argCount;

                            // Check for null
                            object instance = null;
                            if ((callMethod.Flags & CILMethodFlags.This) != 0 && (instance = stack[spArgCaller].Ref) == null)
                                threadContext.Throw<NullReferenceException>();

                            // Push the frame
                            threadContext.PushMethodFrame(loadContext.AppDomain, callMethod, CallInstance.ExistingObjectInstance, spArgCaller, sp, out int spCall);
                            int spReturn = sp;

                            // Debug execution
                            Debug.Instruction(op, pc - 5, callMethod.Method, spCall, argCount);

                            // Check for interpreted
                            if ((callMethod.Flags & CILMethodFlags.Interpreted) != 0)
                            {
                                // Handle virtual calls
                                // We need to perform late binding based o the instance type, so that the correct derived virtual method is called
                                if((callMethod.Flags & CILMethodFlags.This) != 0)
                                {
                                    // Get instance type
                                    Type instanceType = instance.GetInterpretedType();

                                    // Get explicit type info
                                    CILTypeInfo virtualType = instanceType.GetTypeInfo(loadContext.AppDomain);

                                    // Try to get the virtual method
                                    virtualType.VTable.GetVirtualInstanceMethod(loadContext, ref callMethod);
                                }

                                // Execute the method
                                spReturn = ExecuteMethod(threadContext, callLoadContext, callMethod, spCall);
                            }
                            else if ((callMethod.Flags & CILMethodFlags.Interop) != 0)
                            {
                                // Call interop method
                                __marshal.InvokeMethodInterop(threadContext, loadContext.AppDomain, null, callMethod, spReturn, spCall);
                            }
                            else
                                throw new NotSupportedException("Method cannot be executed: " + callMethod);

                            // Decrement stack pointer
                            sp -= argCount;

                            // Copy return value
                            if ((callMethod.Flags & CILMethodFlags.Return) != 0)
                            {
                                // Note we do not do a frame copy here - just a simple copy
                                stack[sp] = stack[spReturn];
                                sp++;
                            }

                            // Pop the frame
                            threadContext.PopMethodFrame();

                            break;
                        }
                    case ILOpCode.Ret:
                        {
                            // Debug execution
                            Debug.Instruction(op, pc - 1, stack[sp - 1]);

                            // Abort execution - set pc to max value and execution will finish naturally
                            pc = pcMax;
                            break;
                        }
                    case ILOpCode.Throw:
                        {
                            // Pop the exception
                            Exception ex = stack[--sp].Ref as Exception;

                            // Check for null
                            if(ex == null)
                            {
                                threadContext.Throw<NullReferenceException>();
                                break;
                            }

                            // Throw the user exception
                            threadContext.Throw(ex);

                            // Debug execution
                            Debug.Instruction(op, pc - 1, stack[sp - 1]);
                            break;
                        }
                    case ILOpCode.Box:
                        {
                            // Get method token
                            int token = FetchDecode<int>(instructions, ref pc);

                            // Get handle
                            EntityHandle boxHandle = MetadataTokens.EntityHandle(token);

                            // Load the method
                            CILTypeInfo boxType = loadContext.GetTypeHandle(boxHandle);

                            // Perform the box operation
                            StackData.Box(boxType, ref stack[sp - 1]);

                            // Debug execution
                            Debug.Instruction(op, pc - 5, stack[sp - 1]);
                            break;
                        }
                    case ILOpCode.Unbox_any:
                        {
                            // Get method token
                            int token = FetchDecode<int>(instructions, ref pc);

                            // Get handle
                            EntityHandle boxHandle = MetadataTokens.EntityHandle(token);

                            // Load the method
                            CILTypeInfo unboxType = loadContext.GetTypeHandle(boxHandle);

                            try
                            {
                                // Perform the unbox operation
                                StackData.Unbox(unboxType, ref stack[sp - 1]);
                            }
                            catch (InvalidCastException e)
                            {
                                // Throw as runtime exception with interpreted stack trace
                                threadContext.Throw(e);
                            }

                            // Debug execution
                            Debug.Instruction(op, pc - 5, stack[sp - 1]);
                            break;
                        }
                    case ILOpCode.Ldtoken:
                        {
                            // Get token
                            int token = FetchDecode<int>(instructions, ref pc);

                            // Get handle
                            EntityHandle handle = MetadataTokens.EntityHandle(token);

                            // Resolve the member
                            MemberInfo member = loadContext.GetMemberHandle(handle);

                            // Push member reference
                            stack[sp].Ref = member;
                            stack[sp++].Type = StackType.Ref;

                            // Debug execution
                            Debug.Instruction(op, pc - 5, stack[sp - 1]);
                            break;
                        }

                    case ILOpCode.Initobj:
                        {
                            // Get method token
                            int token = FetchDecode<int>(instructions, ref pc);

                            // Get handle
                            EntityHandle typeHandle = MetadataTokens.EntityHandle(token);

                            // Get the type info
                            CILTypeInfo objType = loadContext.GetTypeHandle(typeHandle);

                            // Pop the address
                            StackData address = stack[--sp];

                            // Init to default
                            StackData.Default(objType, ref address);

                            // Debug execution
                            Debug.Instruction(op, pc - 1, address);
                            break;
                        }
                    case ILOpCode.Stobj:
                        {
                            // Get method token
                            int token = FetchDecode<int>(instructions, ref pc);

                            // Get handle
                            EntityHandle typeHandle = MetadataTokens.EntityHandle(token);

                            // Get the type info
                            CILTypeInfo objType = loadContext.GetTypeHandle(typeHandle);

                            // Pop value and address
                            StackData value = stack[--sp];
                            StackData address = stack[--sp];

                            // Write reference
                            ((IByRef)address.Ref).SetValueRef(value.Ref);

                            // Debug execution
                            Debug.Instruction(op, pc - 1, value);
                            break;
                        }
                        #endregion

                } // End switch
            } // End loop

            return sp - 1; // Return the final stack pointer
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static ILOpCode FetchOpCode(byte[] instructions, ref int pc)
        {
            // Fetch the op code
            ILOpCode op = (ILOpCode)FetchDecode<byte>(instructions, ref pc);

            // Check for 2-byte encoded instructions
            if ((byte)op == 0xFE)
                op = (ILOpCode)(((byte)op << 8) | FetchDecode<byte>(instructions, ref pc));

            return op;
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
