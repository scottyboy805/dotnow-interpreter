using dotnow.Reflection;
using System;

namespace dotnow.Runtime.CIL
{
    internal static class CILInterpreterUnsafe
    {
        // Methods
        internal static unsafe void ExecuteInterpreted(AppDomain domain, ExecutionEngine engine, ref ExecutionFrame frame, ref CILOperation[] instructions, ref CLRExceptionHandler[] exceptionHandlers, ExecutionEngine.DebugFlags debugFlags)
        {
            int instructionLength = instructions.Length;

            int stackPtr = frame.stackIndex;        // ldloc.1
            int instructionPtr = frame.instructionPtr;

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

                }
            }

            frame.instructionPtr = instructionPtr;
            frame.stackIndex = stackPtr;
        }
    }
}
