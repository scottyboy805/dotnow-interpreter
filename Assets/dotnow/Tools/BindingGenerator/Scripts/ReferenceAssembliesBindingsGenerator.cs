using System;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace dotnow.BindingGenerator
{
    [CreateAssetMenu(menuName = "dotnow/Reference Assemblies Bindings")]
    public class ReferenceAssembliesBindingsGenerator : ScriptableObject
    {
        // Public
        public string generatePath = "Assets/Bindings/Generated";
        public string[] assemblyNameOrPaths =
        {
            "UnityEngine.CoreModule",
        };

        // Methods
        [ContextMenu("dotnow/Generate Bindings")]
        public void RebuildBindingsForReferenceAssemblies()
        {
            // Create service
            BindingsGeneratorService service = new BindingsGeneratorService();

            // Generate bindings
            foreach (string nameOrPath in assemblyNameOrPaths)
            {
                // Check for error
                if (string.IsNullOrEmpty(nameOrPath) == true)
                    continue;

                // Get the output path
                string outputPath = Path.GetFullPath(generatePath).Replace('\\', '/');

                // Create directory
                if (Directory.Exists(outputPath) == false)
                    Directory.CreateDirectory(outputPath);

                Debug.LogFormat("Generate Bindings: From source = {0}, Target output = {1}", nameOrPath, outputPath);

                // Generate bindings
                RebuildBindingsForReferenceAssembly(service, nameOrPath, outputPath);
            }

#if UNITY_EDITOR
            // Update asset database
            AssetDatabase.Refresh();
#endif
        }

        public void RebuildBindingsForReferenceAssembly(BindingsGeneratorService service, string nameOrPath, string outputPath)
        {
            // Try to load
            Assembly asm = null;
            try
            {
                asm = Assembly.Load(nameOrPath);
            }
            catch(Exception e)
            {
                Debug.LogError("Bindings will not be generated for assembly: " + nameOrPath);
                Debug.LogException(e);
                return;
            }

            // Run generate service
            service.GenerateBindingsForAssembly(asm, outputPath);
        }
    }
}
