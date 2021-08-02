using System;
using dotnow.Runtime;

namespace dotnow.Debugging
{
    public interface IDebugger
    {
        // Methods
        void OnAttachDebugger(AppDomain domain, ExecutionEngine engine);

        void OnDebugFrame(DebugFrame frame);
    }
}
