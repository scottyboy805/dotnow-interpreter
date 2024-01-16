using dotnow.Reflection;
using dotnow.Runtime.Handle;
using dotnow.Runtime.Types;
using Mono.Cecil.Cil;
using System;
using System.Reflection;

namespace dotnow.Runtime.CIL
{
    internal static class CILInterpreterUnsafe
    {
        // Methods
        internal static unsafe void ExecuteInterpreted(AppDomain domain, ExecutionEngine engine, ref ExecutionFrameOld frame, ref CILOperation[] instructions, byte* stackBasePtr, ref CLRExceptionHandler[] exceptionHandlers, ExecutionEngine.DebugFlags debugFlags)
        {
            int instructionPtr = frame.instructionPtr;
            int instructionLength = instructions.Length;

            

            // Pin the stack memory
            //fixed (byte* stackBasePtr = &engine.stackMemory[frame.stackIndex])
            {
                byte* stackPtr = stackBasePtr + frame.stackBaseOffset;

                // Main instruction loop
                while (instructionPtr < instructionLength && frame.abort == false)
                {
                    // Get the instruction
                    CILOperation instruction = instructions[instructionPtr];


                    /// ### WARNING - Only enable this for small snippets of non-looping (Or very shallow looping) code otherwise the performance and memory allocations will be horrific and likely cause an editor crash
#if !UNITY_DISABLE
#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_IOS || UNITY_ANDROID || UNITY_WSA || UNITY_WEBGL || UNITY_SWITCH) && UNITY_PROFILE && UNITY_PROFILE_INSTRUCTIONS
                //UnityEngine.Profiling.Profiler.BeginSample(instruction.instructionName);
#endif
#endif

                    // Switch (opCode)
                    switch (instruction.opCode)
                    {
                        default:
                            throw new NotImplementedException("MSIL instruction is not implemented: " + instruction.opCode.ToString() + "\nAt method body: " + frame.Method);

                        case Code.Nop: // Do nothing - used as a debug marker
                            break;


                        #region LoadConstant
                        case Code.Ldc_I4_0:
                            {
                                // Load constant
                                *(I32*)stackPtr = I32.ZeroSigned;
                                stackPtr += I32.SizeTyped;
                                break;
                            }

                        case Code.Ldc_I4_1:
                            {
                                // Load constant
                                *(I32*)stackPtr = I32.OneSigned;
                                stackPtr += I32.SizeTyped;
                                break;
                            }

                        case Code.Ldc_I4_2:
                            {
                                // Load constant
                                *(I32*)stackPtr = I32.TwoSigned;
                                stackPtr += I32.SizeTyped;
                                break;
                            }

                        case Code.Ldc_I4_3:
                            {
                                // Load constant
                                *(I32*)stackPtr = I32.ThreeSigned;
                                stackPtr += I32.SizeTyped;
                                break;
                            }

                        case Code.Ldc_I4_4:
                            {
                                // Load constant
                                *(I32*)stackPtr = I32.FourSigned;
                                stackPtr += I32.SizeTyped;
                                break;
                            }

                        case Code.Ldc_I4_5:
                            {
                                // Load constant
                                *(I32*)stackPtr = I32.FiveSigned;
                                stackPtr += I32.SizeTyped;
                                break;
                            }

                        case Code.Ldc_I4_6:
                            {
                                // Load constant
                                *(I32*)stackPtr = I32.SixSigned;
                                stackPtr += I32.SizeTyped;
                                break;
                            }

                        case Code.Ldc_I4_7:
                            {
                                // Load constant
                                *(I32*)stackPtr = I32.SevenSigned;
                                stackPtr += I32.SizeTyped;
                                break;
                            }

                        case Code.Ldc_I4_8:
                            {
                                // Load constant
                                *(I32*)stackPtr = I32.EightSigned;
                                stackPtr += I32.SizeTyped;
                                break;
                            }

                        case Code.Ldc_I4_M1:
                            {
                                // Load constant
                                *(I32*)stackPtr = I32.MinusOne;
                                stackPtr += I32.SizeTyped;
                                break;
                            }

                        case Code.Ldc_I4:
                        case Code.Ldc_I4_S:
                            {
                                // Load constant
                                (*(I32*)stackPtr).signed = instruction.operand.Int32;
                                (*(I32*)stackPtr).type = TypeID.Int32;
                                stackPtr += I32.SizeTyped;
                                break;
                            }

                        case Code.Ldc_I8:
                            {
                                // Load constant
                                (*(I64*)stackPtr).signed = instruction.operand.Int64;
                                (*(I64*)stackPtr).type = TypeID.Int64;
                                stackPtr += I64.SizeTyped;
                                break;
                            }

                        case Code.Ldc_R4:
                            {
                                // Load constant
                                (*(F32*)stackPtr).value = instruction.operand.Single;
                                (*(F32*)stackPtr).type = TypeID.Single;
                                stackPtr += F32.SizeTyped;
                                break;
                            }

                        case Code.Ldc_R8:
                            {
                                // Load constant
                                (*(F64*)stackPtr).value = instruction.operand.Double;
                                (*(F64*)stackPtr).type = TypeID.Double;
                                stackPtr += F64.SizeTyped;
                                break;
                            }

                        case Code.Ldnull:
                            {
                                // Push to stack
                                *((Obj*)stackPtr) = Obj.Null;

                                // Increment stack pointer
                                stackPtr += Obj.SizeTyped;
                                break;
                            }

                        case Code.Ldstr:
                            {
                                // Create managed object
                                *((Obj*)stackPtr) = Obj.FromInteropObject((IntPtr)__memory.PinManagedObject(instruction.objectOperand));

                                // Increment stack pointer
                                stackPtr += Obj.SizeTyped;
                                break;
                            }
                        #endregion

                        #region Arithmetic
                        case Code.Add:
                            {
                                I32 val = *(I32*)(stackPtr - I32.SizeTyped);

                                // Peek type id
                                TypeID id = *(TypeID*)(stackPtr - 1);

                                // Check for int32
                                switch (id)
                                {
                                    case TypeID.Int32:
                                        {
                                            // Perform add
                                            (*(I32*)(stackPtr - I32.SizeTyped * 2)).signed =
                                                (*(I32*)(stackPtr - I32.SizeTyped * 2)).signed +
                                                (*(I32*)(stackPtr - I32.SizeTyped)).signed;

                                            // Decrement stack ptr
                                            stackPtr -= I32.SizeTyped;
                                            break;
                                        }
                                    case TypeID.UInt32:
                                        {
                                            // Perform add
                                            (*(I32*)(stackPtr - I32.SizeTyped * 2)).unsigned =
                                                (*(I32*)(stackPtr - I32.SizeTyped * 2)).unsigned +
                                                (*(I32*)(stackPtr - I32.SizeTyped)).unsigned;

                                            // Decrement stack ptr
                                            stackPtr -= I32.SizeTyped;
                                            break;
                                        }
                                    case TypeID.Int64:
                                        {
                                            // Perform add
                                            (*(I64*)(stackPtr - I64.SizeTyped * 2)).signed =
                                                (*(I64*)(stackPtr - I64.SizeTyped * 2)).signed +
                                                (*(I64*)(stackPtr - I64.SizeTyped)).signed;

                                            // Decrement stack ptr
                                            stackPtr -= I64.SizeTyped;
                                            break;
                                        }
                                    case TypeID.UInt64:
                                        {
                                            // Perform add
                                            (*(I64*)(stackPtr - I64.SizeTyped * 2)).unsigned =
                                                (*(I64*)(stackPtr - I64.SizeTyped * 2)).unsigned +
                                                (*(I64*)(stackPtr - I64.SizeTyped)).unsigned;

                                            // Decrement stack ptr
                                            stackPtr -= I64.SizeTyped;
                                            break;
                                        }
                                    case TypeID.Single:
                                        {
                                            // Perform add
                                            (*(F32*)(stackPtr - F32.SizeTyped * 2)).value =
                                                (*(F32*)(stackPtr - F32.SizeTyped * 2)).value +
                                                (*(F32*)(stackPtr - F32.SizeTyped)).value;

                                            // Decrement stack ptr
                                            stackPtr -= F32.SizeTyped;
                                            break;
                                        }
                                    case TypeID.Double:
                                        {
                                            // Perform add
                                            (*(F64*)(stackPtr - F64.SizeTyped * 2)).value =
                                                (*(F64*)(stackPtr - F64.SizeTyped * 2)).value +
                                                (*(F64*)(stackPtr - F64.SizeTyped)).value;

                                            // Decrement stack ptr
                                            stackPtr -= F64.SizeTyped;
                                            break;
                                        }
                                }
                                throw new NotSupportedException("Add operation: " + id);
                            }

                        case Code.Add_Ovf:
                            {
                                // Peek type id
                                TypeID id = *(TypeID*)(stackPtr - 1);

                                // Check for int32
                                switch (id)
                                {
                                    case TypeID.Int32:
                                        {
                                            checked
                                            {
                                                // Perform add
                                                (*(I32*)(stackPtr - I32.SizeTyped * 2)).signed =
                                                    (*(I32*)(stackPtr - I32.SizeTyped * 2)).signed +
                                                    (*(I32*)(stackPtr - I32.SizeTyped)).signed;
                                            }

                                            // Decrement stack ptr
                                            stackPtr -= I32.SizeTyped;
                                            break;
                                        }
                                    case TypeID.Int64:
                                        {
                                            checked
                                            {
                                                // Perform add
                                                (*(I64*)(stackPtr - I64.SizeTyped * 2)).signed =
                                                    (*(I64*)(stackPtr - I64.SizeTyped * 2)).signed +
                                                    (*(I64*)(stackPtr - I64.SizeTyped)).signed;
                                            }

                                            // Decrement stack ptr
                                            stackPtr -= I64.SizeTyped;
                                            break;
                                        }
                                }
                                throw new NotSupportedException("Add_Ovf operation: " + id);
                            }

                        case Code.Add_Ovf_Un:
                            {
                                // Peek type id
                                TypeID id = *(TypeID*)(stackPtr - 1);

                                // Check for int32
                                switch (id)
                                {
                                    case TypeID.UInt32:
                                        {
                                            checked
                                            {
                                                // Perform add
                                                (*(I32*)(stackPtr - I32.SizeTyped * 2)).unsigned =
                                                    (*(I32*)(stackPtr - I32.SizeTyped * 2)).unsigned +
                                                    (*(I32*)(stackPtr - I32.SizeTyped)).unsigned;
                                            }

                                            // Decrement stack ptr
                                            stackPtr -= I32.SizeTyped;
                                            break;
                                        }
                                    case TypeID.UInt64:
                                        {
                                            checked
                                            {
                                                // Perform add
                                                (*(I64*)(stackPtr - I64.SizeTyped * 2)).unsigned =
                                                    (*(I64*)(stackPtr - I64.SizeTyped * 2)).unsigned +
                                                    (*(I64*)(stackPtr - I64.SizeTyped)).unsigned;
                                            }

                                            // Decrement stack ptr
                                            stackPtr -= I64.SizeTyped;
                                            break;
                                        }
                                }
                                throw new NotSupportedException("Add_Ovf_Un operation: " + id);
                            }

                        case Code.Sub:
                            {
                                // Peek type id
                                TypeID id = *(TypeID*)(stackPtr - 1);

                                // Check for int32
                                switch (id)
                                {
                                    case TypeID.Int32:
                                        {
                                            // Perform sub
                                            (*(I32*)(stackPtr - I32.SizeTyped * 2)).signed =
                                                (*(I32*)(stackPtr - I32.SizeTyped * 2)).signed -
                                                (*(I32*)(stackPtr - I32.SizeTyped)).signed;

                                            // Decrement stack ptr
                                            stackPtr -= I32.SizeTyped;
                                            break;
                                        }
                                    case TypeID.UInt32:
                                        {
                                            // Perform sub
                                            (*(I32*)(stackPtr - I32.SizeTyped * 2)).unsigned =
                                                (*(I32*)(stackPtr - I32.SizeTyped * 2)).unsigned -
                                                (*(I32*)(stackPtr - I32.SizeTyped)).unsigned;

                                            // Decrement stack ptr
                                            stackPtr -= I32.SizeTyped;
                                            break;
                                        }
                                    case TypeID.Int64:
                                        {
                                            // Perform sub
                                            (*(I64*)(stackPtr - I64.SizeTyped * 2)).signed =
                                                (*(I64*)(stackPtr - I64.SizeTyped * 2)).signed -
                                                (*(I64*)(stackPtr - I64.SizeTyped)).signed;

                                            // Decrement stack ptr
                                            stackPtr -= I64.SizeTyped;
                                            break;
                                        }
                                    case TypeID.UInt64:
                                        {
                                            // Perform sub
                                            (*(I64*)(stackPtr - I64.SizeTyped * 2)).unsigned =
                                                (*(I64*)(stackPtr - I64.SizeTyped * 2)).unsigned -
                                                (*(I64*)(stackPtr - I64.SizeTyped)).unsigned;

                                            // Decrement stack ptr
                                            stackPtr -= I64.SizeTyped;
                                            break;
                                        }
                                    case TypeID.Single:
                                        {
                                            // Perform sub
                                            (*(F32*)(stackPtr - F32.SizeTyped * 2)).value =
                                                (*(F32*)(stackPtr - F32.SizeTyped * 2)).value -
                                                (*(F32*)(stackPtr - F32.SizeTyped)).value;

                                            // Decrement stack ptr
                                            stackPtr -= F32.SizeTyped;
                                            break;
                                        }
                                    case TypeID.Double:
                                        {
                                            // Perform sub
                                            (*(F64*)(stackPtr - F64.SizeTyped * 2)).value =
                                                (*(F64*)(stackPtr - F64.SizeTyped * 2)).value -
                                                (*(F64*)(stackPtr - F64.SizeTyped)).value;

                                            // Decrement stack ptr
                                            stackPtr -= F64.SizeTyped;
                                            break;
                                        }
                                }
                                throw new NotSupportedException("Subtract operation: " + id);
                            }

                        case Code.Mul:
                            {
                                // Peek type id
                                TypeID id = *(TypeID*)(stackPtr - 1);

                                // Check for int32
                                switch (id)
                                {
                                    case TypeID.Int32:
                                        {
                                            // Perform multiply
                                            (*(I32*)(stackPtr - I32.SizeTyped * 2)).signed =
                                                (*(I32*)(stackPtr - I32.SizeTyped * 2)).signed *
                                                (*(I32*)(stackPtr - I32.SizeTyped)).signed;

                                            // Decrement stack ptr
                                            stackPtr -= I32.SizeTyped;
                                            break;
                                        }
                                    case TypeID.UInt32:
                                        {
                                            // Perform multiply
                                            (*(I32*)(stackPtr - I32.SizeTyped * 2)).unsigned =
                                                (*(I32*)(stackPtr - I32.SizeTyped * 2)).unsigned *
                                                (*(I32*)(stackPtr - I32.SizeTyped)).unsigned;

                                            // Decrement stack ptr
                                            stackPtr -= I32.SizeTyped;
                                            break;
                                        }
                                    case TypeID.Int64:
                                        {
                                            // Perform multiply
                                            (*(I64*)(stackPtr - I64.SizeTyped * 2)).signed =
                                                (*(I64*)(stackPtr - I64.SizeTyped * 2)).signed *
                                                (*(I64*)(stackPtr - I64.SizeTyped)).signed;

                                            // Decrement stack ptr
                                            stackPtr -= I64.SizeTyped;
                                            break;
                                        }
                                    case TypeID.UInt64:
                                        {
                                            // Perform multiply
                                            (*(I64*)(stackPtr - I64.SizeTyped * 2)).unsigned =
                                                (*(I64*)(stackPtr - I64.SizeTyped * 2)).unsigned *
                                                (*(I64*)(stackPtr - I64.SizeTyped)).unsigned;

                                            // Decrement stack ptr
                                            stackPtr -= I64.SizeTyped;
                                            break;
                                        }
                                    case TypeID.Single:
                                        {
                                            // Perform multiply
                                            (*(F32*)(stackPtr - F32.SizeTyped * 2)).value =
                                                (*(F32*)(stackPtr - F32.SizeTyped * 2)).value *
                                                (*(F32*)(stackPtr - F32.SizeTyped)).value;

                                            // Decrement stack ptr
                                            stackPtr -= F32.SizeTyped;
                                            break;
                                        }
                                    case TypeID.Double:
                                        {
                                            // Perform multiply
                                            (*(F64*)(stackPtr - F64.SizeTyped * 2)).value =
                                                (*(F64*)(stackPtr - F64.SizeTyped * 2)).value *
                                                (*(F64*)(stackPtr - F64.SizeTyped)).value;

                                            // Decrement stack ptr
                                            stackPtr -= F64.SizeTyped;
                                            break;
                                        }
                                }
                                throw new NotSupportedException("Multiply operation: " + id);
                            }

                        case Code.Div:
                            {
                                // Peek type id
                                TypeID id = *(TypeID*)(stackPtr - 1);

                                // Check for int32
                                switch (id)
                                {
                                    case TypeID.Int32:
                                        {
                                            // Perform divide
                                            (*(I32*)(stackPtr - I32.SizeTyped * 2)).signed =
                                                (*(I32*)(stackPtr - I32.SizeTyped * 2)).signed /
                                                (*(I32*)(stackPtr - I32.SizeTyped)).signed;

                                            // Decrement stack ptr
                                            stackPtr -= I32.SizeTyped;
                                            break;
                                        }
                                    case TypeID.UInt32:
                                        {
                                            // Perform divide
                                            (*(I32*)(stackPtr - I32.SizeTyped * 2)).unsigned =
                                                (*(I32*)(stackPtr - I32.SizeTyped * 2)).unsigned /
                                                (*(I32*)(stackPtr - I32.SizeTyped)).unsigned;

                                            // Decrement stack ptr
                                            stackPtr -= I32.SizeTyped;
                                            break;
                                        }
                                    case TypeID.Int64:
                                        {
                                            // Perform divide
                                            (*(I64*)(stackPtr - I64.SizeTyped * 2)).signed =
                                                (*(I64*)(stackPtr - I64.SizeTyped * 2)).signed /
                                                (*(I64*)(stackPtr - I64.SizeTyped)).signed;

                                            // Decrement stack ptr
                                            stackPtr -= I64.SizeTyped;
                                            break;
                                        }
                                    case TypeID.UInt64:
                                        {
                                            // Perform divide
                                            (*(I64*)(stackPtr - I64.SizeTyped * 2)).unsigned =
                                                (*(I64*)(stackPtr - I64.SizeTyped * 2)).unsigned /
                                                (*(I64*)(stackPtr - I64.SizeTyped)).unsigned;

                                            // Decrement stack ptr
                                            stackPtr -= I64.SizeTyped;
                                            break;
                                        }
                                    case TypeID.Single:
                                        {
                                            // Perform divide
                                            (*(F32*)(stackPtr - F32.SizeTyped * 2)).value =
                                                (*(F32*)(stackPtr - F32.SizeTyped * 2)).value /
                                                (*(F32*)(stackPtr - F32.SizeTyped)).value;

                                            // Decrement stack ptr
                                            stackPtr -= F32.SizeTyped;
                                            break;
                                        }
                                    case TypeID.Double:
                                        {
                                            // Perform divide
                                            (*(F64*)(stackPtr - F64.SizeTyped * 2)).value =
                                                (*(F64*)(stackPtr - F64.SizeTyped * 2)).value /
                                                (*(F64*)(stackPtr - F64.SizeTyped)).value;

                                            // Decrement stack ptr
                                            stackPtr -= F64.SizeTyped;
                                            break;
                                        }
                                }
                                throw new NotSupportedException("Divide operation: " + id);
                            }
                        #endregion

                        #region Branch
                        case Code.Br:
                        case Code.Br_S:
                            {
                                // Jump to new instruction offset
                                instructionPtr += instruction.operand.Int32;
                                break;
                            }

                        case Code.Brtrue:
                        case Code.Brtrue_S:
                            {
                                // Decrement stack pointer
                                stackPtr -= I32.SizeTyped;

                                // Check for true
                                if((*((I32*)stackPtr)).signed == 1)
                                {
                                    // Jump to new instruction offset
                                    instructionPtr += instruction.operand.Int32;
                                }
                                break;
                            }

                        case Code.Brfalse:
                        case Code.Brfalse_S:
                            {
                                // Decrement stack pointer
                                stackPtr -= I32.SizeTyped;

                                // Check for true
                                if ((*((I32*)stackPtr)).signed == 0)
                                {
                                    // Jump to new instruction offset
                                    instructionPtr += instruction.operand.Int32;
                                }
                                break;
                            }
                        #endregion
                        
                        #region Local
                        case Code.Ldloc_0:
                            {
                                // Get the local
                                _CLRStackHandle local = frame.stackLocals[0];

                                // Get the address of the local
                                byte* localPtr = stackBasePtr + local.offset;
                                {
                                    // Copy from local to top of stack
                                    __memory.Copy(localPtr, stackPtr, local.stackType.size);
                                }

                                // Load type id
                                *(TypeID*)(stackPtr + local.stackType.size) = local.stackType.typeID;

                                // Advance stack ptr
                                stackPtr += local.stackType.size + 1;

                                I32 val = *(I32*)(stackPtr - I32.SizeTyped);
                                break;
                            }

                        case Code.Ldloc_1:
                            {
                                // Get the local
                                _CLRStackHandle local = frame.stackLocals[1];

                                // Get the address of the local
                                byte* localPtr = stackBasePtr + local.offset;
                                {
                                    // Copy from local to top of stack
                                    __memory.Copy(localPtr, stackPtr, local.stackType.size);
                                }

                                // Load type id
                                *(TypeID*)(stackPtr + local.stackType.size) = local.stackType.typeID;

                                // Advance stack ptr
                                stackPtr += local.stackType.size + 1;

                                I32 val = *(I32*)(stackPtr - I32.SizeTyped);
                                break;
                            }

                        case Code.Stloc_0:
                            {
                                // Get the local
                                _CLRStackHandle local = frame.stackLocals[0];

                                // Get the address of the local
                                byte* localPtr = stackBasePtr + local.offset;
                                {
                                    // Copy from stack top to local
                                    __memory.Copy(stackPtr - local.stackType.size - 1, localPtr, local.stackType.size);
                                }

                                // Decrement stack ptr
                                stackPtr -= local.stackType.size + 1;

                                I32 val = *(I32*)localPtr;
                                break;
                            }

                        case Code.Stloc_1:
                            {
                                // Get the local
                                _CLRStackHandle local = frame.stackLocals[1];

                                // Get the address of the local
                                byte* localPtr = stackBasePtr + local.offset;
                                {
                                    // Copy from stack top to local
                                    __memory.Copy(stackPtr - local.stackType.size - 1, localPtr, local.stackType.size);
                                }

                                // Decrement stack ptr
                                stackPtr -= local.stackType.size + 1;

                                I32 val = *(I32*)localPtr;
                                break;
                            }
                        #endregion

                        #region Field
                        case Code.Stfld:
                            {
                                // Get field
                                _CLRFieldHandle field = ((CLRField)((CILFieldAccess)instruction.objectOperand).targetField).Handle;

                                // Pop value address
                                byte* valuePtr = stackPtr - (field.fieldType.size + 1);

                                int val = *(int*)valuePtr;
                                int val2 = *(int*)(valuePtr - 1);

                                // Pop instance
                                Obj obj = *(Obj*)(valuePtr - Obj.SizeTyped);

                                // Check for null
                                if (obj.IsNull == true)
                                    throw new NullReferenceException();

                                // Store field
                                field.WriteFieldMemory(obj.ptr, valuePtr);

                                // Decrement stack ptr
                                stackPtr -= (field.fieldType.size + 1 + Obj.SizeTyped);
                                break;
                            }

                        case Code.Ldfld:
                            {
                                // Get field
                                _CLRFieldHandle field = ((CLRField)((CILFieldAccess)instruction.objectOperand).targetField).Handle;

                                // Pop instance
                                Obj obj = *(Obj*)(stackPtr - Obj.SizeTyped);

                                // Check for null
                                if (obj.IsNull == true)
                                    throw new NullReferenceException();

                                // Decrement stack ptr
                                stackPtr -= Obj.SizeTyped;

                                // Read field
                                field.ReadFieldMemoryTypeID(obj.ptr, stackPtr);

                                int val = *(int*)stackPtr;
                                TypeID id = *(TypeID*)(stackPtr + 4);
                                // Increment stack ptr
                                stackPtr += (field.fieldType.size + 1);
                                break;
                            }

                        case Code.Ldflda:
                            {
                                // Get field
                                _CLRFieldHandle field = ((CLRField)((CILFieldAccess)instruction.objectOperand).targetField).Handle;

                                // Pop instance
                                Obj obj = *(Obj*)(stackPtr - Obj.SizeTyped);

                                // Check for null
                                if (obj.IsNull == true)
                                    throw new NullReferenceException();

                                // Decrement stack ptr
                                stackPtr -= Obj.SizeTyped;

                                // Push field address as int ptr
                                *(IntPtr*)stackPtr = (IntPtr)((byte*)obj.ptr + field.offset);

                                // Increment stack ptr - No typeid for intptr
                                stackPtr += sizeof(IntPtr);
                                break;
                            }
                        #endregion

                        #region Indirect
                        case Code.Ldind_I1:
                            {
                                // Get address
                                IntPtr addr = *(IntPtr*)(stackPtr - sizeof(IntPtr));

                                // Load value onto stack and promote to 32bit
                                (*(I32*)(stackPtr - sizeof(IntPtr))).signed = *(sbyte*)addr;
                                (*(I32*)(stackPtr - I32.Size)).type = TypeID.Int32;

                                // Update stack ptr
                                stackPtr -= (sizeof(IntPtr) - I32.SizeTyped);
                                break;
                            }

                        case Code.Ldind_I2:
                            {
                                // Get address
                                IntPtr addr = *(IntPtr*)(stackPtr - sizeof(IntPtr));

                                // Load value onto stack and promote to 32bit
                                (*(I32*)(stackPtr - sizeof(IntPtr))).signed = *(short*)addr;
                                (*(I32*)(stackPtr - I32.Size)).type = TypeID.Int32;

                                // Update stack ptr
                                stackPtr -= (sizeof(IntPtr) - I32.SizeTyped);
                                break;
                            }

                        case Code.Ldind_I4:
                            {
                                // Get address
                                IntPtr addr = *(IntPtr*)(stackPtr - sizeof(IntPtr));

                                // Load value onto stack and promote to 32bit
                                (*(I32*)(stackPtr - sizeof(IntPtr))).signed = *(int*)addr;
                                (*(I32*)(stackPtr - I32.Size)).type = TypeID.Int32;

                                // Update stack ptr
                                stackPtr -= (sizeof(IntPtr) - I32.SizeTyped);
                                break;
                            }

                        case Code.Ldind_I8:
                            {
                                // Get address
                                IntPtr addr = *(IntPtr*)(stackPtr - sizeof(IntPtr));

                                // Load value onto stack and promote to 32bit
                                (*(I64*)(stackPtr - sizeof(IntPtr))).signed = *(long*)addr;
                                (*(I64*)(stackPtr - I64.Size)).type = TypeID.Int64;

                                // Update stack ptr
                                stackPtr -= (sizeof(IntPtr) - I64.SizeTyped);
                                break;
                            }
                        #endregion

                        #region Array
                        case Code.Newarr:
                            {
                                break;
                            }
                        #endregion

                        #region ObjectModel
                        case Code.Newobj:
                            {
                                // Get method invoke
                                CILMethodInvocation methodInvoke = (CILMethodInvocation)instruction.objectOperand;

                                // Get target constructor
                                ConstructorInfo ctor = (ConstructorInfo)methodInvoke.targetMethod;
                                
                                // Check for interop
                                if ((instruction.typeOperand.type is CLRType) == false)
                                {
                                    object[] args = null;

                                    // Allocate interop
                                    IntPtr objPtr = __memory.AllocateInterop(domain, instruction.typeOperand.type, ctor, args);

                                    // Push obj
                                    *(Obj*)stackPtr = Obj.FromInteropObject(objPtr);
                                }
                                else
                                {
                                    // Allocate memory
                                    IntPtr objPtr = __memory.Allocate(domain, ((CLRType)instruction.typeOperand.type).Handle, false, ref stackPtr);

                                    // Push obj 
                                    *(Obj*)stackPtr = Obj.FromCLRObject(objPtr);
                                }

                                // Increment ptr
                                stackPtr += Obj.SizeTyped;
                                break;
                            }

                        case Code.Ret:
                            {
                                frame.abort = true;

                                // Reset call frame
                                if (engine.currentFrame != null)
                                    engine.currentFrame = (engine.currentFrame.Parent != null) ? engine.currentFrame.Parent : null;
                                break;
                            }
                            #endregion
                    } // End switch (OpCode)





                    // Next instruction
                    instructionPtr++;

                    // Check for debugger attached
                    if ((debugFlags & ExecutionEngine.DebugFlags.DebuggerAttached) == 0)
                        continue;

                    // Check for paused
                    if ((debugFlags & ExecutionEngine.DebugFlags.DebugPause) != 0 || (debugFlags & ExecutionEngine.DebugFlags.DebugStepOnce) != 0)
                    {
                        // Save execution state
                        engine.SaveExecutionState(domain, frame, instructions, exceptionHandlers);

                        frame.instructionPtr = instructionPtr;
                        frame.stackIndex = *stackPtr;
                        return;
                    }
                } // End while

                frame.instructionPtr = instructionPtr;
                frame.stackIndex = (*stackPtr - *stackBasePtr);

                frame.stackPtr = stackPtr;
            }
        }
    }
}
