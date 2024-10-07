#if !UNITY_DISABLE
#if UNITY_EDITOR 
using System;
using System.Reflection;
using dotnow.BindingGenerator.Emit;

namespace dotnow.BindingGenerator
{
    public sealed class BindingsGeneratorService
    {
        // Public
        public bool generateProxyBindings = true;
        public bool generateDirectCallBindings = true;

        // Methods
        public BindingsGeneratorResult GenerateBindingsForAssembly(string assemblyPath, string outputPathOrFolder)
        {
            // Check for invalid path
            if (assemblyPath == null) throw new ArgumentNullException(nameof(assemblyPath));
            if (string.IsNullOrEmpty(assemblyPath) == true) throw new ArgumentException(nameof(assemblyPath) + " cannot be null or empty");

            // Try to load assembly
            Assembly asm = Assembly.LoadFrom(assemblyPath);

            // Call through
            return GenerateBindingsForAssembly(asm, outputPathOrFolder);
        }

        public BindingsGeneratorResult GenerateBindingsForAssembly(Assembly assembly, string outputFolderOrPath)
        {
            // Create result
            BindingsGeneratorResult result = new BindingsGeneratorResult(outputFolderOrPath);

            // Check for proxy bindings
            if (generateProxyBindings == true)
                ProxyGenerator.GenerateProxyDefinitionsForAssembly(assembly, outputFolderOrPath, result);

            // Check for direct call
            if(generateDirectCallBindings == true)
                DirectCallBindingsGenerator.GenerateDirectCallBindingsForAssembly(assembly, outputFolderOrPath, result);

            return result;
        }

        public BindingsGeneratorResult GenerateBindingsForType(Type type, string outputFolderOrPath)
        {
            // Create result
            BindingsGeneratorResult result = new BindingsGeneratorResult(outputFolderOrPath);

            // Check for proxy bindings
            if (generateProxyBindings == true)
                ProxyGenerator.GenerateProxyDefinitionsForType(type, outputFolderOrPath, result);

            // Check for direct call
            if (generateDirectCallBindings == true)
                DirectCallBindingsGenerator.GenerateDirectCallBindingsForType(type, outputFolderOrPath, result);

            return result;
        }        
    }
}
#endif
#endif