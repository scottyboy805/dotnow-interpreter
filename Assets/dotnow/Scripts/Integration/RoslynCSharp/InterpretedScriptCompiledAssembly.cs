#if ROSLYNCSHARP
using RoslynCSharp.Compiler;
using System;
using Trivial.CodeSecurity;

namespace RoslynCSharp
{
    internal class InterpretedScriptCompiledAssembly : TrivialCLRScriptAssembly, IScriptCompiledAssembly
    {
        // Private
        private CompilationResult result = null;
        private DateTime compiledTime = DateTime.MinValue;

        // Properties
        public override CompilationResult CompileResult
        {
            get { return result; }
        }

        public override string AssemblyPath
        {
            get { return result.OutputFile; }
        }

        public override byte[] AssemblyImage
        {
            get { return result.OutputAssemblyImage; }
        }

        public override bool IsRuntimeCompiled
        {
            get { return true; }
        }

        public override DateTime RuntimeCompiledTime
        {
            get { return compiledTime; }
        }

        // Methods
        public void MarkAsRuntimeCompiled(CompilationResult compileResult)
        {
            this.result = compileResult;
            this.compiledTime = DateTime.Now;
        }

        protected override CodeSecurityEngine CreateSecurityEngine()
        {
            // Check for failure
            if (result.Success == false)
                return null;

            // Load from image
            if (result.OutputAssemblyImage != null)
                return new CodeSecurityEngine(result.OutputAssemblyImage);

            // Load from file
            if (result.OutputFile != null)
                return new CodeSecurityEngine(result.OutputFile);

            // Load from location
            if (result.OutputAssembly != null)
                return new CodeSecurityEngine(result.OutputAssembly.Location);

            // No loaded assembly data
            return null;
        }
    }
}
#endif
