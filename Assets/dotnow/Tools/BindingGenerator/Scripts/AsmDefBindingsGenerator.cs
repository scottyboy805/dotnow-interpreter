#if !UNITY_DISABLE
#if UNITY_EDITOR && NET_4_6
using UnityEngine;
using System.Linq;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Compilation;
#endif

namespace dotnow.BindingGenerator
{
#if UNITY_EDITOR
    [InitializeOnLoad]
#endif
    [CreateAssetMenu]
    public class AsmDefBindingsGenerator : ScriptableObject
    {
        // Public
        public Object asmDefAsset;
        public long lastWriteTime = -1;
        public string outputPathRelative = "_Bindings-Generated";

        // Constructor

        // Methods
        public void OnEnable()
        {
            // Check for compiliation
            if(EditorApplication.isCompiling == false)
            {
                RebuildBindingsForAsmDef();
            }
        }

        public void RebuildBindingsForAsmDef()
        {
            // Check for no asset
            if (asmDefAsset == null)
                return;

            // Try to find compilation
            Assembly asm = CompilationPipeline.GetAssemblies().FirstOrDefault(a => a.name == asmDefAsset.name);

            // Check for found
            if(asm == null)
            {
                Debug.LogWarning("Failed to find compilation for asm def asset: " + asmDefAsset);
                return;
            }

            // Get the target output path
            if (File.Exists(asm.outputPath) == false)
                return;

            // Check last write time
            if (File.GetLastWriteTime(asm.outputPath).Ticks <= lastWriteTime)
                return;

            // Get target folder
            string assetPath = AssetDatabase.GetAssetPath(asmDefAsset);

            string outputPath = Path.ChangeExtension(assetPath, null);

            // Make path relative to this asset
            if(string.IsNullOrEmpty(outputPathRelative) == false)
                outputPath = string.Concat(outputPath, "/", outputPathRelative);

            // Build assembly
            RebuildBindingsForAssembly(asm.outputPath, outputPath);

            // Set last write time
            lastWriteTime = File.GetLastWriteTime(asm.outputPath).Ticks;
        }

        private void RebuildBindingsForAssembly(string assemblyPath, string outputFolder)
        {
            // Create builder
            BindingsGeneratorService service = new BindingsGeneratorService();

            Debug.Log("Target output path: " + outputFolder);

            // Generate bindings for assembly
            service.GenerateBindingsForAssembly(assemblyPath, outputFolder);
        }
    }
}
#endif
#endif