#if !UNITY_DISABLE
#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_IOS || UNITY_ANDROID || UNITY_WSA || UNITY_WEBGL)
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using dotnow.Reflection;
using UnityEngine;

namespace dotnow.Examples
{
    public class SnakeExample : MonoBehaviour
    {
        // Public
        public TextAsset assemblyModule;

        // Public
        private void Start()
        {
            // Check for game assembly to load in interpreted mode
            if(assemblyModule == null)
            {
                Debug.LogError("Assembly module is not assigned. You should assign a pre-compiled managed assembly as a text asset");
                Debug.LogError("You might need to build the snake example project to generate this asset!");
                return;
            }

            // Create app domain
            AppDomain domain = new AppDomain();

            // Load the module
            CLRModule module = domain.LoadModuleStream(new MemoryStream(assemblyModule.bytes), false);

            // Find the main type
            Type mainType = module.GetTypes()
                .Where(t => t.IsSubclassOf(typeof(MonoBehaviour)))
                .FirstOrDefault();

            if(mainType == null)
            {
                Debug.LogError("Failed to find main type in assembly: " + module);
                return;
            }

            Debug.Log("Using main type: " + mainType);

            // Add component - cannot use 'gameObject.AddComponent' because it would crash the engine.
            OverrideBindings.AddComponentOverride(domain, null, gameObject, new object[] { mainType });

            // Hack to prevent code stripping of required method - a more permanent fix is required!
            Instantiate<GameObject>(null);
            Instantiate<GameObject>(null, Vector3.zero, Quaternion.identity);
            Instantiate<GameObject>(null, Vector3.zero, Quaternion.identity, null);
            Vector2Int.RoundToInt(default(Vector2Int));

            // Hack to force AOT compilation of generic type with specific generic argument which is required by the game a more permanent fix is required!
            // Perhaps use List<object> in such cases for support for non-compiled AOT generics
            List<Vector2Int> dummyList = new List<Vector2Int>();
        }
    }
}
#endif
#endif