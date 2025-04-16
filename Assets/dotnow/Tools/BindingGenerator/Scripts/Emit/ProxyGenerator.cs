#if !UNITY_DISABLE
#if UNITY_EDITOR && NET_4_6 
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
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
                if (type.GetCustomAttributes(typeof(GenerateBindingsAttribute), false).Length == 0)
                {
                    continue;
                }
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
                            // Generate the source code
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
                        // Generate the source code
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
                    : Path.Combine(outputFolderOrPath, outputName + "-Proxy");

                // Create directory if need
                if (isOutputFile == false && Directory.Exists(outputFolderOrPath) == false)
                    Directory.CreateDirectory(outputFolderOrPath);

                // Create the file stream
                using (TextWriter writer = new StreamWriter(outputPath))
                {
                    // Generate the source code
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
            return string.Concat(typeBuilder.GetTypeFlattenedName(), "-Proxy.cs");
        }

        private static bool CanGenerateProxyForType(Type type)
        {
            // Skip generated types
            if (type.IsDefined(typeof(GeneratedAttribute), false))
                return false;
            // Skip Unity-generated types
            if (type.Name.StartsWith("UnitySourceGenerated"))
                return false;
            
            // Handle inner classes
            if (type.IsNested && !type.IsNestedPublic)
                return false;

            // Check for interfaces
            if (type.IsInterface)
                return true;

            // Check for sealed
            if (type.IsSealed)
                return false;

            // Check for virtual or abstract methods
            if (type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Any(m => m.IsVirtual || m.IsAbstract))
                return true;

            // Check for virtual or abstract properties
            if (type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Any(p => (p.GetMethod != null && (p.GetMethod.IsVirtual || p.GetMethod.IsAbstract)) ||
                          (p.SetMethod != null && (p.SetMethod.IsVirtual || p.SetMethod.IsAbstract))))
                return true;

            return false;
        }
    }
}
#endif
#endif