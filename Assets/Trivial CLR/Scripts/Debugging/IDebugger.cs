using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrivialCLR.Runtime;

namespace TrivialCLR.Debugging
{
    public interface IDebugger
    {
        // Methods
        void OnAttachDebugger(AppDomain domain, ExecutionEngine engine);

        void OnDebugFrame(DebugFrame frame);
    }
}
