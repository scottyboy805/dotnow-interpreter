#if ROSLYNCSHARP
using RoslynCSharp.Compiler;
using System;
using System.Collections.Generic;
using System.IO;
using dotnow.Reflection;
using AppDomain = dotnow.AppDomain;
using Microsoft.CodeAnalysis.CSharp;
using UnityEngine;

namespace RoslynCSharp
{
    public static class ScriptDomainExtensions
    {
        // Type
        private struct CompilerState
        {
            // Public
            public bool loadCompiledAssemblies;
            public bool allowConcurrentCompile;
            public bool generateInMemory;
        }

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
            return domain.RegisterAssemblyPath<InterpretedScriptAssembly>(module, securityMode, filePath);
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
            return domain.RegisterAssemblyImage<InterpretedScriptAssembly>(module, securityMode, assemblyImage);
        }
#endregion

#region CompileAssembly
        public static ScriptType CompileAndLoadMainSourceInterpreted(this ScriptDomain domain, string cSharpSource, ScriptSecurityMode securityMode = ScriptSecurityMode.UseSettings, IMetadataReferenceProvider[] additionalReferenceAssemblies = null)
        {
            // Send compile request
            ScriptAssembly assembly = CompileAndLoadSourcesInterpreted(domain, new string[] { cSharpSource }, securityMode, additionalReferenceAssemblies);

            // Try to get main type
            if (assembly != null && assembly.MainType != null)
                return assembly.MainType;

            return null;
        }

        public static ScriptType CompileAndLoadMainFileInterpreted(this ScriptDomain domain, string cSharpFile, ScriptSecurityMode securityMode = ScriptSecurityMode.UseSettings, IMetadataReferenceProvider[] additionalReferenceAssemblies = null)
        {
            // Send compile request
            ScriptAssembly assembly = CompileAndLoadFilesInterpreted(domain, new string[] { cSharpFile }, securityMode, additionalReferenceAssemblies);

            // Try to get main type
            if (assembly != null && assembly.MainType != null)
                return assembly.MainType;

            return null;
        }

        public static ScriptType CompileAndLoadMainSyntaxTreeInterpreted(this ScriptDomain domain, CSharpSyntaxTree cSharpSyntaxTree, ScriptSecurityMode securityMode = ScriptSecurityMode.UseSettings, IMetadataReferenceProvider[] additionalReferenceAssemblies = null)
        {
            // Send compile request
            ScriptAssembly assembly = CompileAndLoadSyntaxTreesInterpreted(domain, new CSharpSyntaxTree[] { cSharpSyntaxTree }, securityMode, additionalReferenceAssemblies);

            // Try to get main type
            if (assembly != null && assembly.MainType != null)
                return assembly.MainType;

            return null;
        }

        public static ScriptAssembly CompileAndLoadSourceInterpreted(this ScriptDomain domain, string cSharpSource, ScriptSecurityMode securityMode = ScriptSecurityMode.UseSettings, IMetadataReferenceProvider[] additionalReferenceAssemblies = null)
        {
            return CompileAndLoadSourcesInterpreted(domain, new string[] { cSharpSource }, securityMode, additionalReferenceAssemblies);
        }

        public static ScriptAssembly CompileAndLoadSourcesInterpreted(this ScriptDomain domain, string[] cSharpSources, ScriptSecurityMode securityMode = ScriptSecurityMode.UseSettings, IMetadataReferenceProvider[] additionalReferenceAssemblies = null)
        {
            // Check for disposed
            if(domain.IsDisposed == true)
                throw new ObjectDisposedException(nameof(domain));

            // Check for compiler
            if (domain.IsCompilerServiceInitialized == false)
                throw new InvalidOperationException("The compiler service has not been initialized");

            // Send the compile request
            bool loadAssemblies = RoslynCSharpCompiler.loadCompiledAssemblies;

            RoslynCSharpCompiler.loadCompiledAssemblies = false;
            domain.CompileFromSources(cSharpSources, additionalReferenceAssemblies);
            RoslynCSharpCompiler.loadCompiledAssemblies = loadAssemblies;

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
            return domain.RegisterAssembly<InterpretedScriptCompiledAssembly>(module, securityMode, domain.CompileResult);
        }

        public static ScriptAssembly CompileAndLoadFileInterpreted(this ScriptDomain domain, string cSharpFile, ScriptSecurityMode securityMode = ScriptSecurityMode.UseSettings, IMetadataReferenceProvider[] additionalReferenceAssemblies = null)
        {
            return CompileAndLoadFilesInterpreted(domain, new string[] { cSharpFile }, securityMode, additionalReferenceAssemblies);
        }

        public static ScriptAssembly CompileAndLoadFilesInterpreted(this ScriptDomain domain, string[] cSharpFiles, ScriptSecurityMode securityMode = ScriptSecurityMode.UseSettings, IMetadataReferenceProvider[] additionalReferenceAssemblies = null)
        {
            // Check for disposed
            if (domain.IsDisposed == true)
                throw new ObjectDisposedException(nameof(domain));

            // Check for compiler
            if (domain.IsCompilerServiceInitialized == false)
                throw new InvalidOperationException("The compiler service has not been initialized");

            // Send the compile request
            bool loadAssemblies = RoslynCSharpCompiler.loadCompiledAssemblies;

            RoslynCSharpCompiler.loadCompiledAssemblies = false;
            domain.CompileFromFiles(cSharpFiles, additionalReferenceAssemblies);
            RoslynCSharpCompiler.loadCompiledAssemblies = loadAssemblies;

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
            return domain.RegisterAssembly<InterpretedScriptCompiledAssembly>(module, securityMode, domain.CompileResult);
        }

        public static ScriptAssembly CompileAndLoadSyntaxTreeInterpreted(this ScriptDomain domain, CSharpSyntaxTree syntaxTree, ScriptSecurityMode securityMode = ScriptSecurityMode.UseSettings, IMetadataReferenceProvider[] additionalReferenceAssemblies = null)
        {
            return CompileAndLoadSyntaxTreesInterpreted(domain, new CSharpSyntaxTree[] { syntaxTree }, securityMode, additionalReferenceAssemblies);
        }

        public static ScriptAssembly CompileAndLoadSyntaxTreesInterpreted(this ScriptDomain domain, CSharpSyntaxTree[] cSharpSyntaxTrees, ScriptSecurityMode securityMode = ScriptSecurityMode.UseSettings, IMetadataReferenceProvider[] additionalReferenceAssemblies = null)
        {
            // Check for disposed
            if (domain.IsDisposed == true)
                throw new ObjectDisposedException(nameof(domain));

            // Check for compiler
            if (domain.IsCompilerServiceInitialized == false)
                throw new InvalidOperationException("The compiler service has not been initialized");

            // Send the compile request
            bool loadAssemblies = RoslynCSharpCompiler.loadCompiledAssemblies;

            // Make sure settings are valid for current target
            CompilerState state = ForceCompilerSettings();

            // Compile call
            domain.CompileFromSyntaxTrees(cSharpSyntaxTrees, additionalReferenceAssemblies);

            // Restore settings to original before compile call
            RestoreCompilerSettings(state);

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
            return domain.RegisterAssembly<InterpretedScriptCompiledAssembly>(module, securityMode, domain.CompileResult);
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

        private static CompilerState ForceCompilerSettings()
        {
            // Get current state
            CompilerState state = new CompilerState
            {
                loadCompiledAssemblies = RoslynCSharpCompiler.loadCompiledAssemblies,
                allowConcurrentCompile = RoslynCSharp.Settings.AllowConcurrentCompile,
                generateInMemory = RoslynCSharp.Settings.GenerateInMemory,
            };

#if UNITY_WEBGL
            if(RoslynCSharp.Settings.AllowConcurrentCompile == true)
            {
                RoslynCSharp.Settings.AllowConcurrentCompile = false;
                Debug.LogWarning("Concurrent compile is not supported for WebGL execution. Disabling compiler setting!");
            }
#endif

#if UNITY_WEBGL || UNITY_ANDROID || UNITY_IOS
            if (RoslynCSharp.Settings.GenerateInMemory == false)
            {
                RoslynCSharp.Settings.GenerateInMemory = true;
                Debug.LogWarning("Generate in memory must be enabled for current platform. Enabling compiler setting!");
            }
#endif

            return state;
        }

        private static void RestoreCompilerSettings(in CompilerState state)
        {
            RoslynCSharpCompiler.loadCompiledAssemblies = state.loadCompiledAssemblies;
            RoslynCSharp.Settings.AllowConcurrentCompile = state.allowConcurrentCompile;
            RoslynCSharp.Settings.GenerateInMemory = state.generateInMemory;
        }
    }
}
#endif