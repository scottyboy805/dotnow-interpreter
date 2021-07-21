using TrivialCLR.Runtime;

namespace TrivialCLR.Debugging
{
    public class DebugVariable
    {
        // Private
        private ExecutionFrame frame = null;
        private string variableName = "";
        private int variableIndex = 0;

        // Properties
        public string VariableName
        {
            get { return variableName; }
        }

        public object VariableValue
        {
            get { return frame.stack[variableIndex]; }
        }

        // Constructor
        public DebugVariable(ExecutionFrame frame, string variableName, int variableIndex)
        {
            this.frame = frame;
            this.variableName = variableName;
            this.variableIndex = variableIndex;
        }

        // Methods
        public override string ToString()
        {
            return string.Format("{0} = {1}", variableName, VariableValue);
        }
    }
}
