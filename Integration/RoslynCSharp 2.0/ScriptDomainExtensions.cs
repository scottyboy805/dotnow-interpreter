#if ROSLYNCSHARP_20
using Microsoft.CodeAnalysis;
using RoslynCSharp.Project;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Trivial.CodeSecurity;
using UnityEngine;
using AppDomain = dotnow.AppDomain;

namespace RoslynCSharp
{
    public static class ScriptDomainExtensions
    {
        // Type
        /// <summary>
        /// Used for loading PE assembly into dotnow app domain for interpreted execution.
        /// </summary>
        private sealed class AppDomainContext : IAssemblyLoader
        {
            // Public
            public readonly AppDomain AppDomain;

            // Constructor
            public AppDomainContext(AppDomain appDomain)
            {
                this.AppDomain = appDomain;
            }

            // Methods
            public Assembly LoadAssembly(string assemblyPath, string debugSymbolsPath = null)
            {
                return AppDomain.LoadAssemblyFrom(assemblyPath, true);
            }

            public Assembly LoadAssembly(byte[] assemblyImage, byte[] debugSymbolsImage = null)
            {
                return AppDomain.LoadAssemblyStream(new MemoryStream(assemblyImage), null, true);
            }
        }

        // Private
        private static Dictionary<ScriptDomain, AppDomainContext> clrDomains = new();

        // Methods
        #region LoadAssembly
        public static ScriptAssembly LoadAssemblyFromResourcesInterpreted(this ScriptDomain domain, string assemblyResourcesPath, ScriptSecurityMode securityMode = ScriptSecurityMode.UseSettings, IMainTypeSelector mainTypeSelector = null)
        {
            domain.CheckDisposed();

            // Check arg
            if (string.IsNullOrEmpty(assemblyResourcesPath) == true)
                throw new ArgumentException("Assembly resources path cannot be null or empty");

            // Load assembly asset
            TextAsset assemblyAsset = Resources.Load<TextAsset>(assemblyResourcesPath);

            // Check for load error
            if (assemblyAsset == null)
                throw new DllNotFoundException("Could not load assembly from resources: " + assemblyResourcesPath);

            // Load from bytes
            return domain.LoadAssemblyInterpreted(assemblyAsset.bytes, securityMode, mainTypeSelector);
        }

        public static ScriptAssembly LoadAssemblyInterpreted(this ScriptDomain domain, string assemblyPath, ScriptSecurityMode securityMode = ScriptSecurityMode.UseSettings, IMainTypeSelector mainTypeSelector = null)
        {
            domain.CheckDisposed();

            // Create source
            AssemblySource source = AssemblySource.FromFile(assemblyPath, null);

            // Load the assembly
            return RegisterAssembly(domain, domain.GetAppDomainContext(), source, null, out _, securityMode, mainTypeSelector);
        }

        public static ScriptAssembly LoadAssemblyInterpreted(this ScriptDomain domain, byte[] assemblyImage, ScriptSecurityMode securityMode = ScriptSecurityMode.UseSettings, IMainTypeSelector mainTypeSelector = null)
        {
            domain.CheckDisposed();

            // Create source
            AssemblySource source = AssemblySource.FromMemory(assemblyImage, null);

            // Load the assembly
            return RegisterAssembly(domain, domain.GetAppDomainContext(), source, null, out _, securityMode, mainTypeSelector);
        }
        #endregion

        #region LoadMainAssembly
        public static ScriptType LoadMainAssemblyFromResourcesInterpreted(this ScriptDomain domain, string assemblyResourcesPath, ScriptSecurityMode securityMode = ScriptSecurityMode.UseSettings, IMainTypeSelector mainTypeSelector = null)
            => LoadAssemblyFromResourcesInterpreted(domain, assemblyResourcesPath, securityMode, mainTypeSelector)?.MainType;

        public static ScriptType LoadMainAssemblyInterpreted(this ScriptDomain domain, string assemblyPath, ScriptSecurityMode securityMode = ScriptSecurityMode.UseSettings, IMainTypeSelector mainTypeSelector = null)
            => LoadAssemblyInterpreted(domain, assemblyPath, securityMode, mainTypeSelector)?.MainType;

        public static ScriptType LoadMainAssemblyInterpreted(this ScriptDomain domain, byte[] assemblyImage, ScriptSecurityMode securityMode = ScriptSecurityMode.UseSettings, IMainTypeSelector mainTypeSelector = null)
            => LoadAssemblyInterpreted(domain, assemblyImage, securityMode, mainTypeSelector)?.MainType;
        #endregion



        #region CompileAndLoadSource
        public static ScriptAssembly CompileAndLoadSourceInterpreted(this ScriptDomain domain, string cSharpSource, CompileOptions options = null, ScriptSecurityMode securityMode = ScriptSecurityMode.UseSettings)
            => CompileAndLoadSourcesInterpreted(domain, new[] { cSharpSource }, options, securityMode);
        public static ScriptAssembly CompileAndLoadSourceInterpreted(this ScriptDomain domain, string cSharpSource, out CompileResult compileResult, out CodeSecurityReport securityReport, CompileOptions options = null, ScriptSecurityMode securityMode = ScriptSecurityMode.UseSettings)
            => CompileAndLoadSourcesInterpreted(domain, new[] { cSharpSource }, out compileResult, out securityReport, options, securityMode);
        public static ScriptAssembly CompileAndLoadSourcesInterpreted(this ScriptDomain domain, string[] cSharpSources, CompileOptions options = null, ScriptSecurityMode securityMode = ScriptSecurityMode.UseSettings)
            => CompileAndLoadSourcesInterpreted(domain, cSharpSources, out _, out _, options, securityMode);
        public static ScriptAssembly CompileAndLoadSourcesInterpreted(this ScriptDomain domain, string[] cSharpSources, out CompileResult compileResult, out CodeSecurityReport securityReport, CompileOptions options = null, ScriptSecurityMode securityMode = ScriptSecurityMode.UseSettings)
        {
            domain.CheckDisposed();

            // Check options
            if (options == null)
                options = CompileOptions.FromSettings();

            // Compile request
            compileResult = domain.CompileSources(cSharpSources, options);
            securityReport = null;

            // Check for success
            if (compileResult.Success == false)
                return null;

            // Load the assembly
            return RegisterAssembly(domain, domain.GetAppDomainContext(), compileResult.Assembly, compileResult, out _, securityMode, options.MainTypeSelector);
        }
        #endregion

        #region CompileAndLoadFile
        public static ScriptAssembly CompileAndLoadFileInterpreted(this ScriptDomain domain, string cSharpFile, CompileOptions options = null, ScriptSecurityMode securityMode = ScriptSecurityMode.UseSettings)
            => CompileAndLoadFilesInterpreted(domain, new[] { cSharpFile }, options, securityMode);
        public static ScriptAssembly CompileAndLoadFileInterpreted(this ScriptDomain domain, string cSharpFile, out CompileResult compileResult, out CodeSecurityReport securityReport, CompileOptions options = null, ScriptSecurityMode securityMode = ScriptSecurityMode.UseSettings)
            => CompileAndLoadFilesInterpreted(domain, new[] { cSharpFile }, out compileResult, out securityReport, options, securityMode);
        public static ScriptAssembly CompileAndLoadFilesInterpreted(this ScriptDomain domain, string[] cSharpFiles, CompileOptions options = null, ScriptSecurityMode securityMode = ScriptSecurityMode.UseSettings)
            => CompileAndLoadFilesInterpreted(domain, cSharpFiles, out _, out _, options, securityMode);
        public static ScriptAssembly CompileAndLoadFilesInterpreted(this ScriptDomain domain, string[] cSharpFiles, out CompileResult compileResult, out CodeSecurityReport securityReport, CompileOptions options = null, ScriptSecurityMode securityMode = ScriptSecurityMode.UseSettings)
        {
            domain.CheckDisposed();

            // Check options
            if (options == null)
                options = CompileOptions.FromSettings();

            // Compile request
            compileResult = domain.CompileFiles(cSharpFiles, options);
            securityReport = null;

            // Check for success
            if (compileResult.Success == false)
                return null;

            // Load the assembly
            return RegisterAssembly(domain, domain.GetAppDomainContext(), compileResult.Assembly, compileResult, out _, securityMode, options.MainTypeSelector);
        }
        #endregion

        #region CompileAndLoadSyntaxTree
        public static ScriptAssembly CompileAndLoadSyntaxTreeInterpreted(this ScriptDomain domain, SyntaxTree syntaxTree, CompileOptions options = null, ScriptSecurityMode securityMode = ScriptSecurityMode.UseSettings)
            => CompileAndLoadSyntaxTreesInterpreted(domain, new[] { syntaxTree }, out _, out _, options, securityMode);
        public static ScriptAssembly CompileAndLoadSyntaxTreeInterpreted(this ScriptDomain domain, SyntaxTree syntaxTree, out CompileResult compileResult, out CodeSecurityReport securityReport, CompileOptions options = null, ScriptSecurityMode securityMode = ScriptSecurityMode.UseSettings)
            => CompileAndLoadSyntaxTreesInterpreted(domain, new[] { syntaxTree }, out compileResult, out securityReport, options, securityMode);
        public static ScriptAssembly CompileAndLoadSyntaxTreesInterpreted(this ScriptDomain domain, SyntaxTree[] syntaxTrees, CompileOptions options = null, ScriptSecurityMode securityMode = ScriptSecurityMode.UseSettings)
            => CompileAndLoadSyntaxTreesInterpreted(domain, syntaxTrees, out _, out _, options, securityMode);
        public static ScriptAssembly CompileAndLoadSyntaxTreesInterpreted(this ScriptDomain domain, SyntaxTree[] syntaxTrees, out CompileResult compileResult, out CodeSecurityReport securityReport, CompileOptions options = null, ScriptSecurityMode securityMode = ScriptSecurityMode.UseSettings)
        {
            domain.CheckDisposed();

            // Check options
            if (options == null)
                options = CompileOptions.FromSettings();

            // Compile request
            compileResult = domain.CompileSyntaxTrees(syntaxTrees, options);
            securityReport = null;

            // Check for success
            if (compileResult.Success == false)
                return null;

            // Load the assembly
            return RegisterAssembly(domain, domain.GetAppDomainContext(), compileResult.Assembly, compileResult, out _, securityMode, options.MainTypeSelector);
        }
        #endregion

        #region CompileAndLoadProject
        public static ScriptAssembly CompileAndLoadProjectInterpreted(this ScriptDomain domain, CSharpProject project, CompileOptions options = null, ScriptSecurityMode securityMode = ScriptSecurityMode.UseSettings)
            => CompileAndLoadProjectInterpreted(domain, project, out _, out _, options, securityMode);
        public static ScriptAssembly CompileAndLoadProjectInterpreted(this ScriptDomain domain, CSharpProject project, out CompileResult compileResult, out CodeSecurityReport securityReport, CompileOptions options = null, ScriptSecurityMode securityMode = ScriptSecurityMode.UseSettings)
        {
            domain.CheckDisposed();

            // Check options
            if (options == null)
                options = CompileOptions.FromSettings();

            // Compile request
            compileResult = domain.CompileProject(project, options);
            securityReport = null;

            // Check for success
            if (compileResult.Success == false)
                return null;

            // Load the assembly
            return RegisterAssembly(domain, domain.GetAppDomainContext(), compileResult.Assembly, compileResult, out _, securityMode, options.MainTypeSelector);
        }
        #endregion

        #region CompileAndLoadFolder
        public static ScriptAssembly CompileAndLoadDirectoryInterpreted(this ScriptDomain domain, string directoryPath, SearchOption searchOption, CompileOptions options = null, ScriptSecurityMode securityMode = ScriptSecurityMode.UseSettings)
            => CompileAndLoadDirectoryInterpreted(domain, directoryPath, searchOption, out _, out _, options, securityMode);
        public static ScriptAssembly CompileAndLoadDirectoryInterpreted(this ScriptDomain domain, string directoryPath, SearchOption searchOption, out CompileResult compileResult, out CodeSecurityReport securityReport, CompileOptions options = null, ScriptSecurityMode securityMode = ScriptSecurityMode.UseSettings)
        {
            domain.CheckDisposed();

            // Check options
            if (options == null)
                options = CompileOptions.FromSettings();

            // Compile request
            compileResult = domain.CompileDirectory(directoryPath, searchOption, options);
            securityReport = null;

            // Check for success
            if (compileResult.Success == false)
                return null;

            // Load the assembly
            return RegisterAssembly(domain, domain.GetAppDomainContext(), compileResult.Assembly, compileResult, out _, securityMode, options.MainTypeSelector);
        }
        #endregion



        #region CompileAndLoadMainSource
        public static ScriptType CompileAndLoadMainSourceInterpreted(this ScriptDomain domain, string cSharpSource, CompileOptions options = null, ScriptSecurityMode securityMode = ScriptSecurityMode.UseSettings)
            => CompileAndLoadSourceInterpreted(domain, cSharpSource, options, securityMode)?.MainType;
        public static ScriptType CompileAndLoadMainSourcesInterpreted(this ScriptDomain domain, string[] cSharpSources, CompileOptions options = null, ScriptSecurityMode securityMode = ScriptSecurityMode.UseSettings)
            => CompileAndLoadSourcesInterpreted(domain, cSharpSources, options, securityMode)?.MainType;

        public static ScriptType CompileAndLoadMainSourceInterpreted(this ScriptDomain domain, string cSharpSource, out CompileResult compileResult, out CodeSecurityReport securityReport, CompileOptions options = null, ScriptSecurityMode securityMode = ScriptSecurityMode.UseSettings)
            => CompileAndLoadSourceInterpreted(domain, cSharpSource, out compileResult, out securityReport, options, securityMode)?.MainType;
        public static ScriptType CompileAndLoadMainSourcesInterpreted(this ScriptDomain domain, string[] cSharpSources, out CompileResult compileResult, out CodeSecurityReport securityReport, CompileOptions options = null, ScriptSecurityMode securityMode = ScriptSecurityMode.UseSettings)
            => CompileAndLoadSourcesInterpreted(domain, cSharpSources, out compileResult, out securityReport, options, securityMode)?.MainType;
        #endregion

        #region CompileAndLoadMainFile
        public static ScriptType CompileAndLoadMainFileInterpreted(this ScriptDomain domain, string cSharpFile, CompileOptions options = null, ScriptSecurityMode securityMode = ScriptSecurityMode.UseSettings)
            => CompileAndLoadFileInterpreted(domain, cSharpFile, options, securityMode)?.MainType;
        public static ScriptType CompileAndLoadMainFilesInterpreted(this ScriptDomain domain, string[] cSharpFiles, CompileOptions options = null, ScriptSecurityMode securityMode = ScriptSecurityMode.UseSettings)
            => CompileAndLoadFilesInterpreted(domain, cSharpFiles, options, securityMode)?.MainType;

        public static ScriptType CompileAndLoadMainFileInterpreted(this ScriptDomain domain, string cSharpFile, out CompileResult compileResult, out CodeSecurityReport securityReport, CompileOptions options = null, ScriptSecurityMode securityMode = ScriptSecurityMode.UseSettings)
            => CompileAndLoadFileInterpreted(domain, cSharpFile, out compileResult, out securityReport, options, securityMode)?.MainType;
        public static ScriptType CompileAndLoadMainFilesInterpreted(this ScriptDomain domain, string[] cSharpFiles, out CompileResult compileResult, out CodeSecurityReport securityReport, CompileOptions options = null, ScriptSecurityMode securityMode = ScriptSecurityMode.UseSettings)
            => CompileAndLoadFilesInterpreted(domain, cSharpFiles, out compileResult, out securityReport, options, securityMode)?.MainType;
        #endregion

        #region CompileAndLoadMainSyntaxTree
        public static ScriptType CompileAndLoadMainSyntaxTreeInterpreted(this ScriptDomain domain, SyntaxTree syntaxTree, CompileOptions options = null, ScriptSecurityMode securityMode = ScriptSecurityMode.UseSettings)
            => CompileAndLoadSyntaxTreeInterpreted(domain, syntaxTree, options, securityMode)?.MainType;
        public static ScriptType CompileAndLoadMainSyntaxTreesInterpreted(this ScriptDomain domain, SyntaxTree[] syntaxTrees, CompileOptions options = null, ScriptSecurityMode securityMode = ScriptSecurityMode.UseSettings)
            => CompileAndLoadSyntaxTreesInterpreted(domain, syntaxTrees, options, securityMode)?.MainType;

        public static ScriptType CompileAndLoadMainSyntaxTreeInterpreted(this ScriptDomain domain, SyntaxTree syntaxTree, out CompileResult compileResult, out CodeSecurityReport securityReport, CompileOptions options = null, ScriptSecurityMode securityMode = ScriptSecurityMode.UseSettings)
            => CompileAndLoadSyntaxTreeInterpreted(domain, syntaxTree, out compileResult, out securityReport, options, securityMode)?.MainType;
        public static ScriptType CompileAndLoadMainSyntaxTreesInterpreted(this ScriptDomain domain, SyntaxTree[] syntaxTrees, out CompileResult compileResult, out CodeSecurityReport securityReport, CompileOptions options = null, ScriptSecurityMode securityMode = ScriptSecurityMode.UseSettings)
            => CompileAndLoadSyntaxTreesInterpreted(domain, syntaxTrees, out compileResult, out securityReport, options, securityMode)?.MainType;
        #endregion

        #region CompileAndLoadMainProject
        public static ScriptType CompileAndLoadMainProjectInterpreted(this ScriptDomain domain, CSharpProject project, CompileOptions options = null, ScriptSecurityMode securityMode = ScriptSecurityMode.UseSettings)
            => CompileAndLoadProjectInterpreted(domain, project, options, securityMode)?.MainType;

        public static ScriptType CompileAndLoadMainProjectInterpreted(this ScriptDomain domain, CSharpProject project, out CompileResult compileResult, out CodeSecurityReport securityReport, CompileOptions options = null, ScriptSecurityMode securityMode = ScriptSecurityMode.UseSettings)
            => CompileAndLoadProjectInterpreted(domain, project, out compileResult, out securityReport, options, securityMode)?.MainType;
        #endregion

        #region CompileAndLoadMainFolder
        public static ScriptType CompileAndLoadMainDirectoryInterpreted(this ScriptDomain domain, string directoryPath, SearchOption searchOption, CompileOptions options = null, ScriptSecurityMode securityMode = ScriptSecurityMode.UseSettings)
            => CompileAndLoadDirectoryInterpreted(domain, directoryPath, searchOption, options, securityMode)?.MainType;

        public static ScriptType CompileAndLoadMainDirectoryInterpreted(this ScriptDomain domain, string directoryPath, SearchOption searchOption, out CompileResult compileResult, out CodeSecurityReport securityReport, CompileOptions options = null, ScriptSecurityMode securityMode = ScriptSecurityMode.UseSettings)
            => CompileAndLoadDirectoryInterpreted(domain, directoryPath, searchOption, out compileResult, out securityReport, options, securityMode)?.MainType;
        #endregion


        public static AppDomain GetAppDomain(this ScriptDomain domain)
        {
            return domain.GetAppDomainContext().AppDomain;
        }

        private static AppDomainContext GetAppDomainContext(this ScriptDomain domain)
        {
            // Try to get cached
            AppDomainContext appDomain;
            if (clrDomains.TryGetValue(domain, out appDomain) == true)
                return appDomain;

            // Create new
            appDomain = new(new AppDomain());

            // Add to cached
            clrDomains[domain] = appDomain;
            return appDomain;
        }

        private static ScriptAssembly RegisterAssembly(this ScriptDomain domain, AppDomainContext appDomainContext, AssemblySource source, CompileResult compileResult, out CodeSecurityReport securityReport, ScriptSecurityMode securityMode, IMainTypeSelector mainTypeSelector)
        {
            // Run security checks
            if (domain.SecurityCheckAssembly(source, securityMode, out securityReport) == false)
                return null;

            // Create the assembly
            InterpretedScriptAssembly asm = new InterpretedScriptAssembly(domain, appDomainContext, source, compileResult, securityReport, mainTypeSelector);

            // Register assembly
            return domain.RegisterAssembly(asm);
        }

        private static void CheckDisposed(this ScriptDomain domain)
        {
            if (domain == null || domain.IsDisposed == true)
                throw new ObjectDisposedException(nameof(domain));
        }
    }
}
#endif