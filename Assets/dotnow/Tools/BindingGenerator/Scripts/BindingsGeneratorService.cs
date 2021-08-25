using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using System.Reflection;
using System.Text;
using dotnow.BindingGenerator.Emit;

namespace dotnow.BindingGenerator
{
    public sealed class BindingsGeneratorService
    {
        // Public
        public bool generateProxyBindings = true;

        // Methods
        public BindingsGeneratorResult GenerateBindingsForAssembly(string assemblyPath, string outputPathOrFolder)
        {
            // Check for invalid path
            if (assemblyPath == null) throw new ArgumentNullException(nameof(assemblyPath));
            if (string.IsNullOrEmpty(assemblyPath) == true) throw new ArgumentException(nameof(assemblyPath) + " cannot be null or empty");

            // Try to load assembly
            Assembly asm = Assembly.ReflectionOnlyLoadFrom(assemblyPath);

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

            return result;
        }

        public BindingsGeneratorResult GenerateBindingsForType(Type type, string outputFolderOrPath)
        {
            // Create result
            BindingsGeneratorResult result = new BindingsGeneratorResult(outputFolderOrPath);

            // Check for proxy bindings
            if (generateProxyBindings == true)
                ProxyGenerator.GenerateProxyDefinitionsForType(type, outputFolderOrPath, result);

            return result;
        }

        //public BindingsGeneratorResult GenerateProxyDefinitionsForAssembly(Assembly assembly, string generateInFolder)
        //{
            

        //    return result;
        //}

        //public BindingsGeneratorResult GenerateProxyDefinitionsForType(Type type, string generateInFolder)
        //{
        //    // Validate
        //    if (type == null) throw new ArgumentNullException(nameof(type));
        //    if (string.IsNullOrEmpty(generateInFolder) == true) throw new ArgumentException(nameof(generateInFolder) + " cannot be null or empty");

        //    // Create result
        //    BindingsGeneratorResult result = new BindingsGeneratorResult(generateInFolder);

        //    // Call through
        //    if (CanGenerateProxyForType(type) == true)
        //    {
        //        GenerateProxyDefinitionsForTypeImpl(type, result);
        //    }

        //    // Check for no types
        //    if (result.GeneratedSourceFileCount == 0)
        //        result.SetError("No suitable types found! Proxies will only be generated for interface, abstract or non-sealed types which define one or more virtual or abstract members");

        //    return result;
        //}

        //private void GenerateProxyDefinitionsForTypeImpl(Type type, BindingsGeneratorResult result)
        //{
        //    ProxyTypeBuilder typeBuilder = new ProxyTypeBuilder(type);

        //    UnityEngine.Debug.Log(type.Assembly.Location);

        //    // Get the output name
        //    string outputName = string.Concat(typeBuilder.GetTypeFlattenedName(), ".cs");

        //    CodeDomProvider provider = CodeDomProvider.CreateProvider("C#");

        //    StringBuilder builder = new StringBuilder();

        //    using (TextWriter writer = new StringWriter(builder))
        //    {
        //        CodeCompileUnit compileUnit = new CodeCompileUnit();
        //        compileUnit.Namespaces.Add(new CodeNamespace("generated"));

        //        compileUnit.Namespaces[0].Types.Add(typeBuilder.BuildTypeProxy());

        //        provider.GenerateCodeFromCompileUnit(compileUnit, writer, options);
        //    }

        //    // Get output source string
        //    string output = builder.ToString();

        //    // Get output path
        //    string outputPath = Path.Combine(result.GenerateInFolder, outputName);

        //    DirectoryInfo parent = Directory.GetParent(outputPath);

        //    if (parent.Exists == false)
        //        parent.Create();

        //    // Write to file
        //    File.WriteAllText(outputPath, output);
        //}

        
    }
}
