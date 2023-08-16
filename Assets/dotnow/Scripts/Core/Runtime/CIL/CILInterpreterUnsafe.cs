using dotnow.Reflection;
using dotnow.Runtime.Types;
using Mono.Cecil.Cil;
using System;

namespace dotnow.Runtime.CIL
{
    internal static class CILInterpreterUnsafe
    {
        // Methods
        internal static unsafe void ExecuteInterpreted(AppDomain domain, ExecutionEngine engine, ref ExecutionFrame frame, ref CILOperation[] instructions, ref CLRExceptionHandler[] exceptionHandlers, ExecutionEngine.DebugFlags debugFlags)
        {
            int instructionPtr = frame.instructionPtr;
            int instructionLength = instructions.Length;

            

            // Pin the stack memory
            fixed (byte* stackBasePtr = &engine.stack[frame.stackIndex])
            {
                byte* stackPtr = stackBasePtr;

                // Main instruction loop
                while (instructionPtr < instructionLength && frame.abort == false)
                {
                    // Get the instruction
                    CILOperation instruction = instructions[instructionPtr];


                    /// ### WARNING - Only enable this for small snippets of non-looping (Or very shallow looping) code otherwise the performance and memory allocations will be horrific and likley cause an editor crash
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
                                stackPtr -= I32.Size;

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
                                stackPtr -= I32.Size;

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
                                stackPtr += Obj.Size;
                                break;
                            }

                        case Code.Ldc_I4:
                        case Code.Ldc_I4_S:
                            {
                                // Read value
                                I32 val = new I32 { type = TypeID.Int32, signed = (int)instruction.objectOperand };

                                // Push to stack
                                *((I32*)stackPtr) = val;

                                // Increment stack pointer
                                stackPtr += I32.Size;
                                break;
                            }

                        case Code.Ldc_I4_0:
                            {
                                // Push to stack
                                *((I32*)stackPtr) = I32.ZeroSigned;

                                // Increment stack pointer
                                stackPtr += I32.Size;
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
                frame.stackIndex = *stackPtr;
            }
        }
    }
}
