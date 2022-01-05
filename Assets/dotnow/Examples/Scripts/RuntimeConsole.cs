#if !UNITY_DISABLE
#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_IOS || UNITY_ANDROID || UNITY_WSA || UNITY_WEBGL)
using System.Collections.Generic;
using UnityEngine;

public class RuntimeConsole : MonoBehaviour
{
    // Types
    private struct MessageInfo
    {
        // Public
        public int warningLevel;
        public string message;
    }

    // Private
    private List<MessageInfo> messages = new List<MessageInfo>();
    private Vector2 scroll = Vector2.zero;

    // Public
    public int messageHistory = 100;

    // Methods
    public void OnEnable()
    {
        Application.logMessageReceived += OnDebugLogMessageReceived;
    }

    public void OnDisable()
    {
        Application.logMessageReceived -= OnDebugLogMessageReceived;
    }

    public void OnGUI()
    {
        Color originalColor = GUI.color;

        int spacing = -10;

        scroll = GUILayout.BeginScrollView(scroll, GUI.skin.box, GUILayout.ExpandWidth(true), GUILayout.Height(200));
        {
            foreach(MessageInfo msg in messages)
            {
                if (msg.warningLevel == 0) GUI.color = Color.white;
                else if (msg.warningLevel == 1) GUI.color = Color.yellow;
                else if (msg.warningLevel == 2) GUI.color = Color.red;

                GUILayout.Label(msg.message);
                GUILayout.Space(spacing);
            }
        }
        GUILayout.EndScrollView();
        GUILayout.Space(-spacing * 2);

        GUI.color = originalColor;
    }

    public void AddMessage(string message, int warningLevel)
    {
        messages.Add(new MessageInfo
        {
            message = message,
            warningLevel = warningLevel,
        });

        if (messages.Count > messageHistory)
            messages.RemoveAt(0);

        // Scroll to bottom
        scroll.y = float.MaxValue;
    }

    private void OnDebugLogMessageReceived(string condition, string stackTrace, LogType type)
    {
        int warningLevel = 2;

        if (type == LogType.Log) warningLevel = 0;
        else if (type == LogType.Warning) warningLevel = 1;

        AddMessage(condition, warningLevel);
    }
}
#endif
#endif