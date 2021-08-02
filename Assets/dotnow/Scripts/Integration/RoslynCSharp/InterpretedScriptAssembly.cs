#if ROSLYNCSHARP
using RoslynCSharp.Compiler;
using System;
using System.Reflection;

namespace RoslynCSharp
{
    internal class InterpretedScriptAssembly : ScriptAssembly
    {
        // Private
        private ScriptDomain domain = null;
        private Assembly clrModule = null;

        // Properties
        public override ScriptDomain Domain
        {
            get { return domain; }
        }

        public override Assembly SystemAssembly
        {
            get { return clrModule; }
        }

        public override bool IsRuntimeCompiled
        {
            get { return false; }
        }

        public override DateTime RuntimeCompiledTime
        {
            get { return DateTime.MinValue; }
        }

        public override CompilationResult CompileResult
        {
            get { return null; }
        }

        // Construction
        protected override void ConstructInstance(ScriptDomain domain, Assembly systemAssembly)
        {
            this.domain = domain;
            this.clrModule = systemAssembly;
        }

        // Methods
        protected override ScriptType CreateRootScriptType(Type systemType)
        {
            return ScriptType.CreateScriptType<TrivialCLRScriptType>(this, null, systemType);
        }
    }
}
#endif