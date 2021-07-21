using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using System.Reflection;
using System.Text;
using TrivialCLR.ProxyTool.Emit;

namespace TrivialCLR.ProxyTool
{
    public sealed class ProxyGeneratorService
    {
        // Public
        public CodeGeneratorOptions options = new CodeGeneratorOptions
        {
            BracingStyle = "C",
            BlankLinesBetweenMembers = false,
        };

        // Methods
        public ProxyGeneratorResult GenerateProxyDefinitionsForAssembly(Assembly assembly, string generateInFolder)
        {
            // Validate
            if (assembly == null) throw new ArgumentNullException(nameof(assembly));
            if (string.IsNullOrEmpty(generateInFolder) == true) throw new ArgumentException(nameof(generateInFolder) + " cannot be null or empty");

            // Create result
            ProxyGeneratorResult result = new ProxyGeneratorResult(generateInFolder);

            // Get all types
            foreach(Type type in assembly.GetTypes())
            {
                if(CanGenerateProxyForType(type) == true)
                {
                    GenerateProxyDefinitionsForTypeImpl(type, result);
                }
            }

            // Check for no types
            if (result.GeneratedSourceFileCount == 0)
                result.SetError("No suitable types found! Proxies will only be generated for interface, abstract or non-sealed types which define one or more virtual or abstract members");

            return result;
        }

        public ProxyGeneratorResult GenerateProxyDefinitionsForType(Type type, string generateInFolder)
        {
            // Validate
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (string.IsNullOrEmpty(generateInFolder) == true) throw new ArgumentException(nameof(generateInFolder) + " cannot be null or empty");

            // Create result
            ProxyGeneratorResult result = new ProxyGeneratorResult(generateInFolder);

            // Call through
            if (CanGenerateProxyForType(type) == true)
            {
                GenerateProxyDefinitionsForTypeImpl(type, result);
            }

            // Check for no types
            if (result.GeneratedSourceFileCount == 0)
                result.SetError("No suitable types found! Proxies will only be generated for interface, abstract or non-sealed types which define one or more virtual or abstract members");

            return result;
        }

        private void GenerateProxyDefinitionsForTypeImpl(Type type, ProxyGeneratorResult result)
        {
            ProxyTypeBuilder typeBuilder = new ProxyTypeBuilder(type);

            UnityEngine.Debug.Log(type.Assembly.Location);

            // Get the output name
            string outputName = string.Concat(typeBuilder.GetTypeFlattenedName(), ".cs");

            CodeDomProvider provider = CodeDomProvider.CreateProvider("C#");

            StringBuilder builder = new StringBuilder();

            using (TextWriter writer = new StringWriter(builder))
            {
                CodeCompileUnit compileUnit = new CodeCompileUnit();
                compileUnit.Namespaces.Add(new CodeNamespace("generated"));

                compileUnit.Namespaces[0].Types.Add(typeBuilder.BuildTypeProxy());

                provider.GenerateCodeFromCompileUnit(compileUnit, writer, options);
            }

            // Get output source string
            string output = builder.ToString();

            // Get output path
            string outputPath = Path.Combine(result.GenerateInFolder, outputName);

            DirectoryInfo parent = Directory.GetParent(outputPath);

            if (parent.Exists == false)
                parent.Create();

            // Write to file
            File.WriteAllText(outputPath, output);
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
            foreach(MethodInfo method in type.GetMethods())
            {
                if (method.IsVirtual == true || method.IsAbstract == true)
                    return true;
            }

            // Check for virtual or abstract properties
            foreach(PropertyInfo property in type.GetProperties())
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
