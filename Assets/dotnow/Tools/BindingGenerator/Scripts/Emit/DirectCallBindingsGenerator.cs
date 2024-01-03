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
    internal class DirectCallBindingsGenerator
    {
        // Public
        public static string namespaceAppendName = "Generated";

        // Methods
        public static void GenerateDirectCallBindingsForAssembly(Assembly assembly, string outputFolderOrPath, BindingsGeneratorResult result)
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
            foreach(Type type in assembly.GetTypes())
            {
                // Check if we can generate for type
                if (CanGenerateProxyForType(type) == true)
                {
                    // Generate the type declaration
                    string outputName = GenerateDirectCallBindingsForType(root, type, result);

                    if (isOutputFile == false)
                    {
                        // Get the target output path
                        string outputPath = Path.Combine(outputFolderOrPath, outputName);

                        // Create directory if needed
                        if (Directory.Exists(outputFolderOrPath) == false)
                            Directory.CreateDirectory(outputFolderOrPath);

                        // Create the file stream
                        using (TextWriter writer = new StreamWriter(outputPath))
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

        public static void GenerateDirectCallBindingsForType(Type type, string outputFolderOrPath, BindingsGeneratorResult result)
        {
            // Validate
            if (type == null) throw new ArgumentNullException(nameof(type));
            if(string.IsNullOrEmpty(outputFolderOrPath) == true) throw new ArgumentException(nameof(outputFolderOrPath) + " cannot be null or empty");

            // Check for extension
            bool isOutputFile = Path.HasExtension(outputFolderOrPath);

            // Create a code provider
            CodeDomProvider provider = CodeDomProvider.CreateProvider("C#");

            // Get the compile unit
            CodeCompileUnit root = new CodeCompileUnit();

            // Check if we can generate for type
            if(CanGenerateProxyForType(type) == true)
            {
                // Generate the type declaration
                string outputName = GenerateDirectCallBindingsForType(root, type, result);

                // Get the target output path
                string outputPath = isOutputFile == true
                    ? outputFolderOrPath
                    : Path.Combine(outputFolderOrPath, outputName + "-DirectCall");

                // Create directory if needed
                if (isOutputFile == false && Directory.Exists(outputFolderOrPath) == false)
                    Directory.CreateDirectory(outputFolderOrPath);

                // Create the file stream
                using(TextWriter writer = new StreamWriter(outputPath))
                {
                    // Generate the source code
                    provider.GenerateCodeFromCompileUnit(root, writer, result.codeOptions);
                }
            }

            // Check for no types
            if (result.GeneratedSourceFileCount == 0)
                result.SetError("No suitable types found! Direct calls will only be generated for non-interface and non-enum types");
        }

        internal static string GenerateDirectCallBindingsForType(CodeCompileUnit root, Type type, BindingsGeneratorResult result)
        {
            // Create a direct call type builder
            DirectCallTypeBuilder typeBuilder = new DirectCallTypeBuilder(type);

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

            // Emit the type declaration
            namespaceNode.Types.Add(typeBuilder.BuildTypeDirectCall());

            // Return output type name
            return string.Concat(typeBuilder.GetTypeFlattenedName(), "-DirectCall.cs");
        }

        private static bool CanGenerateProxyForType(Type type)
        {
            // Skip generated types
            if (type.IsDefined(typeof(GeneratedAttribute)) == true)
                return false;

            // Check for interfaces or enums
            if (type.IsInterface == true || type.IsEnum == true)
                return false;

            return true;
        }
    }
}
#endif
#endif