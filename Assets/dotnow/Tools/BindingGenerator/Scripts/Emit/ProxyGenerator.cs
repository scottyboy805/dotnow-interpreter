
#if UNITY_EDITOR
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace dotnow.BindingGenerator.Emit
{
    public static class ProxyGenerator
    {
        // Public
        public static string namespaceAppendName = "Generated";

        // Methods
        public static void GenerateProxyDefinitionsForAssembly(Assembly assembly, string outputFolderOrPath, BindingsGeneratorResult result)
        {
            // Validate
            if (assembly == null) throw new ArgumentNullException(nameof(assembly));
            if (string.IsNullOrEmpty(outputFolderOrPath) == true) throw new ArgumentException(nameof(outputFolderOrPath) + " cannot be null or empty");

            // Check for extension - all source code will be emitted to the same file
            bool isOutputFile = Path.HasExtension(outputFolderOrPath) == true;

            // Create a code provider
            CodeDomProvider provider = CodeDomProvider.CreateProvider("C#");

            // Get the compile unit
            CodeCompileUnit root = new CodeCompileUnit();

            // Get all types
            foreach (Type type in assembly.GetTypes())
            {
                if (CanGenerateProxyForType(type) == true)
                {
                    // Generate the type declaration 
                    string outputName = GenerateProxyDefinitionsForType(root, type, result);

                    // Write the output to individual file
                    if(isOutputFile == false)
                    {
                        // Get the output file path
                        string outputPath = Path.Combine(outputFolderOrPath, outputName);

                        // Create directory if need
                        if (Directory.Exists(outputFolderOrPath) == false)
                            Directory.CreateDirectory(outputFolderOrPath);

                        // Create the file stream
                        using(TextWriter writer = new StreamWriter(outputPath))
                        {
                            // Generate the soruce code
                            provider.GenerateCodeFromCompileUnit(root, writer, result.codeOptions);
                        }

                        // Create new empty root for next type
                        root = new CodeCompileUnit();
                    }
                }

                // Emit single source file
                if (isOutputFile == true)
                {
                    using (TextWriter writer = new StreamWriter(outputFolderOrPath))
                    {
                        // Generate the soruce code
                        provider.GenerateCodeFromCompileUnit(root, writer, result.codeOptions);
                    }
                }
            }

            // Check for no types
            if (result.GeneratedSourceFileCount == 0)
                result.SetError("No suitable types found! Proxies will only be generated for interface, abstract or non-sealed types which define one or more virtual or abstract members");
        }

        public static void GenerateProxyDefinitionsForType(Type type, string outputFolderOrPath, BindingsGeneratorResult result)
        {
            // Validate
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (string.IsNullOrEmpty(outputFolderOrPath) == true) throw new ArgumentException(nameof(outputFolderOrPath) + " cannot be null or empty");

            // Check for extension - all source code will be emitted to the same file
            bool isOutputFile = Path.HasExtension(outputFolderOrPath) == true;

            // Create a code provider
            CodeDomProvider provider = CodeDomProvider.CreateProvider("C#");

            // Get the compile unit
            CodeCompileUnit root = new CodeCompileUnit();
            
            if (CanGenerateProxyForType(type) == true)
            {
                // Generate the type declaration 
                string outputName = GenerateProxyDefinitionsForType(root, type, result);

                // Get the target output path
                string outputPath = isOutputFile == true
                    ? outputFolderOrPath 
                    : Path.Combine(outputFolderOrPath, outputName);

                // Create directory if need
                if (isOutputFile == false && Directory.Exists(outputFolderOrPath) == false)
                    Directory.CreateDirectory(outputFolderOrPath);

                // Create the file stream
                using (TextWriter writer = new StreamWriter(outputPath))
                {
                    // Generate the soruce code
                    provider.GenerateCodeFromCompileUnit(root, writer, result.codeOptions);
                }
            }

            // Check for no types
            if (result.GeneratedSourceFileCount == 0)
                result.SetError("No suitable types found! Proxies will only be generated for interface, abstract or non-sealed types which define one or more virtual or abstract members");
        }

        private static string GenerateProxyDefinitionsForType(CodeCompileUnit root, Type type, BindingsGeneratorResult result)
        {
            // Create a proxy type builder
            ProxyTypeBuilder typeBuilder = new ProxyTypeBuilder(type);

            UnityEngine.Debug.Log(type.Assembly.Location);

            // Create namespace node
            CodeNamespace namespaceNode = (string.IsNullOrEmpty(type.Namespace) == true)
                ? new CodeNamespace(namespaceAppendName)
                : new CodeNamespace(type.Namespace + "." + namespaceAppendName);

            // Check for namespace already added
            CodeNamespace existingNode = root.Namespaces.Cast<CodeNamespace>()
                .FirstOrDefault(n => n.Name == namespaceNode.Name);


            if (existingNode != null)
            {
                // Use the existing namespace
                namespaceNode = existingNode;
            }
            else
            {
                // Add the namespace to the compile unit
                root.Namespaces.Add(namespaceNode);
            }

            // Emit the proxy type declaration
            namespaceNode.Types.Add(typeBuilder.BuildTypeProxy());

            // Return output type name
            return string.Concat(typeBuilder.GetTypeFlattenedName(), ".cs");
        }

        private static bool CanGenerateProxyForType(Type type)
        {
            // Check for interfaces
            if (type.IsInterface == true)
                return true;

            // Check for sealed
            if (type.IsSealed == true)
                return false;

            // Check for virtual or abstract methods
            foreach (MethodInfo method in type.GetMethods())
            {
                if (method.IsVirtual == true || method.IsAbstract == true)
                    return true;
            }

            // Check for virtual or abstract properties
            foreach (PropertyInfo property in type.GetProperties())
            {
                MethodInfo get = property.GetGetMethod();

                if (get != null && (get.IsVirtual == true || get.IsAbstract == true))
                    return true;

                MethodInfo set = property.GetSetMethod();

                if (set != null && (set.IsVirtual == true || set.IsAbstract == true))
                    return true;
            }

            return false;
        }
    }
}
#endif