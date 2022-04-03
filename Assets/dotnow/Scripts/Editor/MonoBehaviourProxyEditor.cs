using UnityEditor;
using UnityEngine;

namespace dotnow
{
    [CustomEditor(typeof(MonoBehaviourProxy), true)]
    public class MonoBehaviourProxyEditor : Editor
    {
        // Private
        private MonoBehaviourProxy proxy = null;
        private CLRInstance instance = null;
        private CLRType type = null;

        private MonoScript fakeScriptInfo = null;

        // Methods
        private void OnEnable()
        {
            // Get the proxy class
            proxy = target as MonoBehaviourProxy;

            // Get the instance and type
            instance = proxy.Instance;
            type = instance.Type;

            // Create dummy script info
            fakeScriptInfo = new MonoScript();
            fakeScriptInfo.name = type.Name + " (Interpreted)";
        }

        public override void OnInspectorGUI()
        {
            // Draw script field
            DrawScriptField();
        }

        private void DrawScriptField()
        {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.BeginHorizontal();
            {
                // Script field
                GUILayout.Label("Script", GUILayout.Width(EditorGUIUtility.labelWidth));
                EditorGUILayout.ObjectField(fakeScriptInfo, typeof(MonoScript));
            }
            EditorGUILayout.EndHorizontal();
            EditorGUI.EndDisabledGroup();
        }
    }
}
