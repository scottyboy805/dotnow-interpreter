#if !UNITY_DISABLE
#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_IOS || UNITY_ANDROID || UNITY_WSA || UNITY_WEBGL || UNITY_SWITCH)
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace dotnow
{
    [CustomEditor(typeof(MonoBehaviourProxy), true)]
    public class MonoBehaviourProxyEditor : Editor
    {
        // Delegate
        private delegate void CustomDrawer(FieldInfo field, object instance);

        // Private
        private static Dictionary<Type, CustomDrawer> fieldDrawers = new Dictionary<Type, CustomDrawer>
        {
            { typeof(string), DrawStringField },
            { typeof(int), DrawIntField },
            { typeof(float), DrawFloatField },
        };

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

            // Draw all serialize fields
            DrawSerializeFields();
        }

        private void DrawScriptField()
        {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.BeginHorizontal();
            {
                // Script field
                GUILayout.Label("Script", GUILayout.Width(EditorGUIUtility.labelWidth));
                EditorGUILayout.ObjectField(fakeScriptInfo, typeof(MonoScript), false);
            }
            EditorGUILayout.EndHorizontal();
            EditorGUI.EndDisabledGroup();
        }

        private void DrawSerializeFields()
        {
            // Get all serializable
            foreach(FieldInfo field in type.GetInstanceFields())
            {
                // Check for non-public without attribute
                if (field.IsPublic == false && field.IsDefined(typeof(SerializeField), false) == false)
                    continue;

                // Check for explicitly ignored
                if (field.IsDefined(typeof(NonSerializedAttribute), false) == true)
                    continue;


                // Draw field
                GUILayout.BeginHorizontal();
                {
                    // Draw label
                    GUILayout.Label(field.Name, GUILayout.Width(EditorGUIUtility.labelWidth));


                    // Draw correct field
                    CustomDrawer drawer = null;
                    if(fieldDrawers.TryGetValue(field.FieldType, out drawer) == true)
                    {
                        // Update the drawer
                        drawer(field, instance);
                    }
                    else
                    {
                        EditorGUILayout.HelpBox("Unable to display serialized field!", MessageType.Warning);
                    }

                }
                GUILayout.EndHorizontal();
            }
        }

        private static void DrawStringField(FieldInfo field, object instance)
        {
            // Get the string value from instance
            string text = (string)field.GetValue(instance);

            // Display gui
            string inputText = EditorGUILayout.TextField(text);

            // Check for changed
            if(text != inputText)
            {
                // Update instance field
                field.SetValue(instance, inputText);
            }
        }

        private static void DrawIntField(FieldInfo field, object instance)
        {
            // Get the int value from instance
            int val = (int)field.GetValue(instance);

            // Display gui
            int inputVal = EditorGUILayout.IntField(val);

            // Check for changed
            if(val != inputVal)
            {
                // Update instance field
                field.SetValue(instance, inputVal);
            }
        }

        private static void DrawFloatField(FieldInfo field, object instance)
        {
            // Get the float value from instance
            float val = (float)field.GetValue(instance);

            // Display gui
            float inputVal = EditorGUILayout.FloatField(val);

            // Check for changed
            if (val != inputVal)
            {
                // Update instance field
                field.SetValue(instance, inputVal);
            }
        }
    }
}
#endif
#endif