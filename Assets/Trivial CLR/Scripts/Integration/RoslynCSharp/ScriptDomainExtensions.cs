#if ROSLYNCSHARP
using RoslynCSharp.Compiler;
using System;
using System.Collections.Generic;
using System.IO;
using TrivialCLR.Reflection;
using AppDomain = TrivialCLR.AppDomain;

namespace RoslynCSharp
{
    public static class ScriptDomainExtensions
    {
        // Private
        private static Dictionary<ScriptDomain, AppDomain> clrDomains = new Dictionary<ScriptDomain, AppDomain>();

        // Methods
        #region LoadAssembly
        public static ScriptAssembly LoadAssemblyInterpreted(this ScriptDomain domain, string filePath, ScriptSecurityMode securityMode = ScriptSecurityMode.UseSettings)
        {
            // Check disposed
            if (domain.IsDisposed == true)
                throw new ObjectDisposedException(nameof(domain));

            // Get the app domain
            AppDomain appDomain = GetAppDomain(domain);

            // Open the file stream
            FileStream stream = File.OpenRead(filePath);

            // Load the assembly
            CLRModule module = appDomain.LoadModuleStream(stream, false);

            // Register assembly
            return domain.RegisterAssemblyPath<TrivialCLRScriptAssembly>(module, securityMode, filePath);
        }

        public static ScriptAssembly LoadAssemblyInterpreted(this ScriptDomain domain, byte[] assemblyImage, ScriptSecurityMode securityMode = ScriptSecurityMode.UseSettings)
        {
            // Check disposed
            if (domain.IsDisposed == true)
                throw new ObjectDisposedException(nameof(domain));

            // Get the app domain
            AppDomain appDomain = GetAppDomain(domain);

            // Create stream
            MemoryStream stream = new MemoryStream(assemblyImage);

            // Load the assembly
            CLRModule module = appDomain.LoadModuleStream(stream, false);

            // Register assembly
            return domain.RegisterAssemblyImage<TrivialCLRScriptAssembly>(module, securityMode, assemblyImage);
        }
        #endregion

        #region CompileAssembly
        public static ScriptType CompileAndLoadMainSourceInterpreted(this ScriptDomain domain, string cSharpSource, ScriptSecurityMode securityMode = ScriptSecurityMode.UseSettings, IMetadataReferenceProvider[] additionalReferenceAssemblies = null)
        {
            // Send compile request
            ScriptAssembly assembly = CompileAndLoadSourceInterpreted(domain, cSharpSource, securityMode, additionalReferenceAssemblies);

            // Try to get main type
            if (assembly != null && assembly.MainType != null)
                return assembly.MainType;

            return null;
        }

        public static ScriptAssembly CompileAndLoadSourceInterpreted(this ScriptDomain domain, string cSharpSource, ScriptSecurityMode securityMode = ScriptSecurityMode.UseSettings, IMetadataReferenceProvider[] additionalReferenceAssemblies = null)
        {
            // Check for disposed
            if(domain.IsDisposed == true)
                throw new ObjectDisposedException(nameof(domain));

            // Check for compiler
            if (domain.IsCompilerServiceInitialized == false)
                throw new InvalidOperationException("The compiler service has not been initialized");
            UnityEngine.Debug.Log("Before compile");
            // Send the compile request
            bool loadAssemblies = RoslynCSharpCompiler.loadCompiledAssemblies;

            RoslynCSharpCompiler.loadCompiledAssemblies = false;
            domain.CompileFromSource(cSharpSource, additionalReferenceAssemblies);
            RoslynCSharpCompiler.loadCompiledAssemblies = loadAssemblies;
            UnityEngine.Debug.Log("After compile");
            // Log output to console
            domain.LogCompilerOutputToConsole();

            // Get app domain
            AppDomain appDomain = GetAppDomain(domain);

            CLRModule module = null;

            if (domain.CompileResult.Success == true)
            {
                // Create stream
                MemoryStream stream = new MemoryStream(domain.CompileResult.OutputAssemblyImage);

                // Load the assembly into domain
                module = appDomain.LoadModuleStream(stream, false);
            }

            // Register assembly
            return domain.RegisterAssembly<TrivialCLRScriptCompiledAssembly>(module, securityMode, domain.CompileResult);
        }
        #endregion

        public static AppDomain GetAppDomain(this ScriptDomain domain)
        {
            AppDomain result = null;

            // Check for already created
            if (clrDomains.TryGetValue(domain, out result) == true)
                return result;

            // Create a new domain
            result = new AppDomain();

            // Register domain
            clrDomains.Add(domain, result);

            return result;
        }
    }
}
#endif