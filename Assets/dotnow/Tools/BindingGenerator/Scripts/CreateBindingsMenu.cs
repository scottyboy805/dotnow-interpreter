#if !UNITY_DISABLE
#if UNITY_EDITOR 
using UnityEngine;
using UnityEditor;

namespace dotnow.BindingGenerator
{
    public class CreateBindingsMenu
    {
        // Methods
        [MenuItem("Tools/Bindings/Generate Mono Behaviour Bindings")]
        public static void GenerateBindings()
        {
            BindingsGeneratorService service = new BindingsGeneratorService();

            service.GenerateBindingsForType(typeof(MonoBehaviour), "Assets/UnityEngine_MonoBehaviour_Generated.cs");
        }
    }
}
#endif
#endif
