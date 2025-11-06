//#if !UNITY_DISABLE
//#if UNITY_EDITOR && NET_4_6
//using UnityEngine;
//using System.Linq;
//using System.IO;
//using System;
//using System.Collections.Generic;
//using Object = UnityEngine.Object;

//#if UNITY_EDITOR
//using UnityEditor;
//using UnityEditor.Compilation;
//#endif

//namespace dotnow.BindingGenerator
//{
//#if UNITY_EDITOR
//    [InitializeOnLoad]
//#endif
//    [CreateAssetMenu]
//    public class AsmDefBindingsGenerator : ScriptableObject
//    {
//        // Private
//        [NonSerialized]
//        private int enableCount = 0;

//        // Public
//        public Object asmDefAsset;
//        public bool autoGenerate = true;
//        public long lastWriteTime = -1;
//        public string outputPathRelative = "";
//        [ContextMenu("dotnow/Clean Bindings")]
//        public void CleanBindings()
//        {
//            lastWriteTime = -1;
//            List<string> deletedAssetPaths = new List<string>();
//            AssetDatabase.DeleteAssets(Directory.GetFiles(outputPathRelative, "*", SearchOption.AllDirectories), deletedAssetPaths);
//        }
//        // Methods
//        private void Reset()
//        {
//            // Make sure output path is valid
//            UpdateOutputPath();
//        }

//        private void OnValidate()
//        {
//            // Make sure output path is valid
//            UpdateOutputPath();
//        }

//        private void OnEnable()
//        {
//            // Make sure output path is valid
//            UpdateOutputPath();

//            // Check for compilation
//            if (EditorApplication.isCompiling == false && autoGenerate == true && enableCount == 0)
//            {
//                RebuildBindingsForAsmDef();
//                enableCount++;
//            }
//        }

//        [ContextMenu("dotnow/Generate Bindings")]
//        public void RebuildBindingsForAsmDef()
//        {
//            // Check for no asset
//            if (asmDefAsset == null)
//                return;

//            // Try to find compilation
//            Assembly asm = CompilationPipeline.GetAssemblies().FirstOrDefault(a => a.name == asmDefAsset.name);
           
//            // Check for found
//            if(asm == null)
//            {
//                Debug.LogWarning("Failed to find compilation for asm def asset: " + asmDefAsset);
//                return;
//            }

//            // Get the target output path
//            if (File.Exists(asm.outputPath) == false)
//                return;

//            // Check last write time
//            if (File.GetLastWriteTime(asm.outputPath).Ticks <= lastWriteTime)
//                return;

//            // Get target folder
//            //string assetPath = AssetDatabase.GetAssetPath(asmDefAsset);

//            var directoryInfo = new DirectoryInfo(Application.dataPath).Parent;
//            if (directoryInfo == null)
//                directoryInfo = new DirectoryInfo(Application.dataPath);
            
//            var fullName = directoryInfo.FullName;
//            string outputPath = Path.Combine(fullName, outputPathRelative);// Path.ChangeExtension(assetPath, null);

            
//            // Create directory
//            if(Directory.Exists(outputPath) == false)
//                Directory.CreateDirectory(outputPath);

//            // Build assembly
//            RebuildBindingsForAssembly(asm.outputPath, outputPath);
                
//            // Set last write time
//            lastWriteTime = File.GetLastWriteTime(asm.outputPath).Ticks;
//            Debug.Log($"Rebuilt bindings for Assembly: {asmDefAsset.name} | Output Folder: {outputPath}");        
//        }

//        private void RebuildBindingsForAssembly(string assemblyPath, string outputFolder)
//        {
//            // Create builder
//            BindingsGeneratorService service = new BindingsGeneratorService();

//            Debug.LogFormat("Generate Bindings: From source = {0}, Target output = {1}", assemblyPath,  outputFolder);

//            // Generate bindings for assembly
//            service.GenerateBindingsForAssembly(assemblyPath, outputFolder);

//#if UNITY_EDITOR
//            // Update asset database
//            AssetDatabase.Refresh();
//#endif
//        }

//        private void UpdateOutputPath()
//        {
//            if (string.IsNullOrEmpty(outputPathRelative) == true)
//            {
//                if(asmDefAsset != null)
//                    outputPathRelative = asmDefAsset.name + "-bindings";

//#if UNITY_EDITOR
//                EditorUtility.SetDirty(this);
//#endif
//            }
//        }
//    }
//}
//#endif
//#endif