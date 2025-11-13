#if !UNITY_DISABLE
#if UNITY_EDITOR && NET_4_6
using UnityEngine;
using UnityEditor;
using dotnow.CodeGen.Emit;
using Microsoft.CodeAnalysis;
using System.IO;
using dotnow.BindingGenerator.Emit;

namespace dotnow.BindingGenerator
{
    public class CreateBindingsMenu
    {
        // Methods
        [MenuItem("Tools/dotnow/CodeGen/Test Method")]
        public static void GenerateBindings()
        {
            //MethodBindingBuilder builder = new MethodBindingBuilder(typeof(Transform).GetMethod("Translate", new[] { typeof(float), typeof(float), typeof(float) }));
            TypeBindingBuilder builder = new TypeBindingBuilder(typeof(Transform));

            string source = builder.BuildMember()
                .NormalizeWhitespace()
                .ToFullString();

            File.WriteAllText("testBinding.cs", source);

            //BindingsGeneratorService service = new BindingsGeneratorService();

            //service.GenerateBindingsForType(typeof(MonoBehaviour), "Assets/UnityEngine_MonoBehaviour_Generated.cs");
        }
    }
}
#endif
#endif
