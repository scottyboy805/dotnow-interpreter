using System.Collections.Generic;
using System.Reflection;
using dotnow.Runtime;
using dotnow.Runtime.CIL;

namespace dotnow.Debugging
{
    public class DebugFrame
    {
        // Private
        private MethodBase executingMethod = null;
        private ExecutionFrame executingFrame = null;
        private CILOperation[] instructionSet = null;
        private int instructionPointer = 0;

        private List<DebugVariable> arguments = new List<DebugVariable>();
        private List<DebugVariable> locals = new List<DebugVariable>();
        
        // Properties
        public MethodBase ExecutingMethod
        {
            get { return executingMethod; }
        }

        public ExecutionFrame ExecutingFrame
        {
            get { return executingFrame; }
        }

        public CILOperation[] InstructionSet
        {
            get { return instructionSet; }
        }

        public int ExecutingInstructionIndex
        {
            get { return instructionPointer; }
        }

        public CILOperation ExecutingInstruction
        {
            get { return instructionSet[instructionPointer]; }
        }

        public DebugVariable ThisInstance
        {
            get { return (executingMethod.IsStatic == false) ? arguments[0] : null; }
        }

        public IList<DebugVariable> Arguments
        {
            get { return arguments; }
        }

        public IList<DebugVariable> Locals
        {
            get { return locals; }
        }

        // Constructor
        public DebugFrame(MethodBase executingMethod, ExecutionFrame frame, CILOperation[] instructionSet, int instructionPointer)
        {
            this.executingMethod = executingMethod;
            this.executingFrame = frame;
            this.instructionSet = instructionSet;
            this.instructionPointer = instructionPointer;
        }

        // Methods
        public void AddArgumentVariable(int argOffset)
        {
            string argName = "";

            if(executingMethod.IsStatic == false && argOffset == 0)
            {
                argName = "this";
            }
            else
            {
                int addOffset = executingMethod.IsStatic == true ? 0 : 1;
                argName = executingMethod.GetParameters()[argOffset - addOffset].Name;
            }

            arguments.Add(new DebugVariable(executingFrame, argName, executingMethod.GetMethodBody().LocalVariables.Count + argOffset));
        }

        public void AddLocalVariable(int localOffset)
        {
            locals.Add(new DebugVariable(executingFrame, string.Format("_local[{0}]", localOffset), localOffset));
        }
    }
}
