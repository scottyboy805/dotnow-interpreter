using dotnow.Runtime;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Metadata;

namespace dotnow
{
    internal static class Debug
    {
        // Internal
        internal static int executionCallDepth = 0;         // Used to indent logged instructions by call depth
        internal static int instructionBasePtr = 0;         // Used to display correct instruction offset

        // Methods
        [Conditional("DOTNOW_ENABLE_DEBUG")]
        internal static void PushCall()
        {
            executionCallDepth++;
        }

        [Conditional("DOTNOW_ENABLE_DEBUG")]
        internal static void PopCall()
        {
            executionCallDepth--;
        }

        [Conditional("DOTNOW_ENABLE_DEBUG")]
        internal static void SetInstructionBasePtr(int pc)
        {
            instructionBasePtr = pc;
        }

        [Conditional("DOTNOW_ENABLE_DEBUG")]
        internal static void Line(string output)
        {
            System.Diagnostics.Debug.WriteLine(output);
        }

        [Conditional("DOTNOW_ENABLE_DEBUG")]
        internal static void LineFormat(string format, params object[] args)
        {
            System.Diagnostics.Debug.WriteLine(format, args);
        }

        [Conditional("DOTNOW_ENABLE_DEBUG")]
        internal static void Timer(string label, Stopwatch timer)
        {
            System.Diagnostics.Debug.WriteLine("{0} took: {1}ms", label, timer.Elapsed.TotalMilliseconds);
        }

        [Conditional("DOTNOW_ENABLE_DEBUG_INSTRUCTIONS")]
        internal static void Instruction(ILOpCode op, int pc)
        {
            // Get offset address
            long offset = pc - instructionBasePtr;

            // Create format string
            string msg = string.Format(new string('\t', executionCallDepth) + "{0:X4}: {1}", offset, DebugOp(op));

            // Log output
            UnityEngine.Debug.Log(msg);
            //System.Diagnostics.Debug.WriteLine(msg);
        }

        [Conditional("DOTNOW_ENABLE_DEBUG_INSTRUCTIONS")]
        internal static void Instruction(ILOpCode op, int pc, in StackData val)
        {
            // Get offset address
            long offset = pc - instructionBasePtr;

            // Create format string
            string msg = string.Format(new string('\t', executionCallDepth) + "{0:X4}: {1}{2}", offset, DebugOp(op), val.ToString());

            // Log output
            UnityEngine.Debug.Log(msg);
            //System.Diagnostics.Debug.WriteLine(msg);
        }

        [Conditional("DOTNOW_ENABLE_DEBUG_INSTRUCTIONS")]
        internal static void Instruction(ILOpCode op, int pc, int branchOffset)
        {
            // Get offset address
            long offset = pc - instructionBasePtr;

            // Create format string
            string msg = string.Format(new string('\t', executionCallDepth) + "{0:X4}: {1}{2:X4}", offset, DebugOp(op), branchOffset);

            // Log output
            UnityEngine.Debug.Log(msg);
            //System.Diagnostics.Debug.WriteLine(msg);
        }

        [Conditional("DOTNOW_ENABLE_DEBUG_INSTRUCTIONS")]
        internal static void Instruction(ILOpCode op, int pc, in StackData val, int branchOffset)
        {
            // Get offset address
            long offset = pc - instructionBasePtr;

            // Create format string
            string msg = string.Format(new string('\t', executionCallDepth) + "{0:X4}: {1}({2}) {3:X4}", offset, DebugOp(op), val.ToString(), branchOffset);

            // Log output
            UnityEngine.Debug.Log(msg);
            //System.Diagnostics.Debug.WriteLine(msg);
        }

        [Conditional("DOTNOW_ENABLE_DEBUG_INSTRUCTIONS")]
        internal static void Instruction(ILOpCode op, int pc, MethodBase method, in StackData spArg, int argCount)
        {
            // Get offset address
            long offset = pc - instructionBasePtr;

            string argStr = "";
            for (int i = 0; i < argCount; i++)
            {
                argStr += spArg.ToString();
                if (i < argCount - 1) argStr += ", ";
            }

            // Create format string
            string msg = string.Format(new string('\t', executionCallDepth) + "{0:X4}: {1}({2}) {3}", offset, DebugOp(op), method, argStr);

            // Log output
            UnityEngine.Debug.Log(msg);
            //System.Diagnostics.Debug.WriteLine(msg);
        }

        [Conditional("DOTNOW_ENABLE_DEBUG_INSTRUCTIONS")]
        internal static void Instruction(ILOpCode op, int pc, in StackData val, bool hasVal)
        {
            // Get offset address
            long offset = pc - instructionBasePtr;

            // Create format string
            string msg = string.Format(new string('\t', executionCallDepth) + "{0:X4}: {1}{2}", offset, DebugOp(op), (hasVal ? val.ToString() : ""));

            // Log output
            UnityEngine.Debug.Log(msg);
            //System.Diagnostics.Debug.WriteLine(msg);
        }

        [Conditional("DOTNOW_ENABLE_DEBUG_INSTRUCTIONS")]
        internal static void InstructionAny(ILOpCode op, int pc, object value)
        {
            // Get offset address
            long offset = pc - instructionBasePtr;

            // Create format string
            string msg = string.Format(new string('\t', executionCallDepth) + "{0:X4}: {1}{2}", offset, DebugOp(op), value);

            // Log output
            UnityEngine.Debug.Log(msg);
            //System.Diagnostics.Debug.WriteLine(msg);
        }

        private static string DebugOp(ILOpCode op)
        {
            int fixedSize = 12;
            string opString = op.ToString();

            for (int i = opString.Length; i < fixedSize; i++)
                opString += ' ';

            return opString;
        }
    }
}
