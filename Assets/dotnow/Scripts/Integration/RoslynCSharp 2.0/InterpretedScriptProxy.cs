#if ROSLYNCSHARP_20

namespace RoslynCSharp
{
    internal sealed class InterpretedScriptProxy : ScriptProxy
    {
        // Private
        private readonly ScriptType scriptType;
        private readonly object instance;

        // Properties
        public override ScriptType ScriptType => scriptType;
        public override object Instance => instance;

        // Constructor
        public InterpretedScriptProxy(ScriptType scriptType, object instance)
        {
            this.scriptType = scriptType;
            this.instance = instance;
        }
    }
}
#endif
