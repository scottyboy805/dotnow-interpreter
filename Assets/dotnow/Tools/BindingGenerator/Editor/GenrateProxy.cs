using UnityEditor;
using UnityEngine;

namespace dotnow.BindingGenerator
{
    public class GenrateProxy : MonoBehaviour
    {
#if UNITY_EDITOR && UNITY_DISABLE == false
        [MenuItem("Tools/Generate Proxy")]
        public static void CreateProxy()
        {
            BindingsGeneratorService service = new BindingsGeneratorService();

            service.GenerateBindingsForType(typeof(System.IO.Stream), Application.dataPath + "/TestBindings");
        }
#endif
    }
}
