#if ROSLYNCSHARP_20
using dotnow;
using System.Linq;
using System.Reflection;
using Trivial.CodeSecurity;

namespace RoslynCSharp
{
    internal sealed class InterpretedScriptAssembly : ScriptAssembly
    {
        // Private
        private readonly Assembly clrModule;

        // Properties
        public override Assembly SystemAssembly => clrModule;
        public override ScriptType[] Types => GetAllTypes();

        // Constructor
        public InterpretedScriptAssembly(ScriptDomain domain, IAssemblyLoader assemblyLoader, AssemblySource source, CompileResult compileResult, CodeSecurityReport securityReport, IMainTypeSelector mainTypeSelector)
            : base(domain, source, compileResult, securityReport, mainTypeSelector)
        {
            this.clrModule = source.LoadAssembly(assemblyLoader);
        }

        // Methods
        private ScriptType[] GetAllTypes()
        {
            return clrModule.GetTypes()
                .Select(t => new InterpretedScriptType(this, null, t))
                .ToArray();
        }
    }
}
#endif
