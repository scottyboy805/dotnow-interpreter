using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TrivialCLR.ProxyTool
{
    public class GenrateProxy : MonoBehaviour
    {
        [MenuItem("Tools/Generate Proxy")]
        public static void CreateProxy()
        {
            ProxyGeneratorService service = new ProxyGeneratorService();

            service.GenerateProxyDefinitionsForType(typeof(System.IO.Stream), Application.dataPath + "/TestBindings");
        }
    }
}
