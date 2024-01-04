using dotnow.Reflection;
using dotnow.Runtime.Handle;
using dotnow.Runtime.Types;
using Mono.Cecil.Cil;
using System;

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
                byte* stackPtr = stackBasePtr;

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

                        #region Constant
                        case Code.Ldnull:
                            {
                                // Push to stack
                                *((Obj*)stackPtr) = Obj.Null;

                                // Increment stack pointer
                                stackPtr += Obj.SizeTyped;
                                break;
                            }

                        //case Code.Ldc_I4:
                        //case Code.Ldc_I4_S:
                        //    {
                        //        // Read value
                        //        I32 val = new I32 { type = TypeID.Int32, signed = (int)instruction.objectOperand };

                        //        // Push to stack
                        //        *((I32*)stackPtr) = val;

                        //        // Increment stack pointer
                        //        stackPtr += I32.SizeTyped;
                        //        break;
                        //    }

                        //case Code.Ldc_I4_0:
                        //    {
                        //        // Push to stack
                        //        *((I32*)stackPtr) = I32.ZeroSigned;

                        //        // Increment stack pointer
                        //        stackPtr += I32.SizeTyped;
                        //        break;
                        //    }
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

                                // Advance stack ptr
                                stackPtr += local.stackType.size + 1;
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
                                stackPtr -= local.stackType.size - 1;
                                break;
                            }
                        #endregion

                        #region ObjectModel
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
                frame.stackIndex = (*stackBasePtr - *stackPtr);
            }
        }
    }
}
