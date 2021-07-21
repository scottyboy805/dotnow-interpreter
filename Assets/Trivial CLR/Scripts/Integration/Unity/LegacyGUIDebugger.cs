using System;
using TrivialCLR.Debugging;
using TrivialCLR.Runtime;
using UnityEditor;
using UnityEngine;

namespace TrivialCLR
{
    public class LegacyGUIDebugger : MonoBehaviour, IDebugger
    {
        // Private
        private AppDomain domain = null;
        private ExecutionEngine engine = null;
        private DebugFrame debugFrame = null;
        private Vector2 scroll = Vector2.zero;

        // Public
        public bool pauseExecutionOnStart = false;

        // Methods
        public void OnAttachDebugger(AppDomain domain, ExecutionEngine engine)
        {
            this.domain = domain;
            this.engine = engine;

            if (pauseExecutionOnStart == true)
                engine.PauseExecution();
        }

        public void OnDebugFrame(DebugFrame frame)
        {
            this.debugFrame = frame;
        }

        public void Pause()
        {
            if (engine != null)
                engine.PauseExecution();
        }

#if UNITY_EDITOR
        public void OnGUI()
        {
            if (engine == null || engine.IsDebuggerPaused == false)
                return;

            GUIStyle labelStyle = new GUIStyle(EditorStyles.miniLabel);
            labelStyle.normal.textColor = Color.white;

            // Split page layout
            GUILayout.BeginHorizontal();
            {
                GUILayout.BeginVertical();
                {
                    // Main instruction set layout
                    GUILayout.BeginVertical(GUI.skin.box);
                    {
                        GUILayout.Label("Execution paused at:", EditorStyles.whiteLargeLabel);
                        GUILayout.Label(debugFrame.ExecutingMethod.ToString());
                    }
                    GUILayout.EndVertical();

                    Color old = GUI.contentColor;

                    // Start scroll view
                    scroll = GUILayout.BeginScrollView(scroll, GUI.skin.box);
                    {
                        for (int i = 0; i < debugFrame.InstructionSet.Length; i++)
                        {
                            if (i == debugFrame.ExecutingFrame.instructionPtr)
                            {
                                GUI.contentColor = Color.red;
                                GUILayout.Box("[" + i + "]: " + debugFrame.InstructionSet[i].ToString(), labelStyle);
                            }
                            else
                            {
                                GUILayout.Label("[" + i + "]: " + debugFrame.InstructionSet[i].ToString(), labelStyle);
                            }
                            GUI.contentColor = old;
                        }
                    }
                    GUILayout.EndScrollView();


                    GUILayout.BeginHorizontal();
                    {
                        if (GUILayout.Button("Step Once (F10)") == true || Input.GetKeyDown(KeyCode.F10) == true)
                        {
                            engine.StepExecution();
                        }

                        if(GUILayout.Button("Continue (F5)") == true || (Event.current.type == EventType.KeyDown && Input.GetKeyDown(KeyCode.F5) == true))
                        {
                            engine.ContinueExecution();
                        }
                    }
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndVertical();
                // Instruction set layout



                // Main stack layout
                GUILayout.BeginVertical(GUI.skin.box);
                {
                    GUILayout.Label("Evaluation Stack", EditorStyles.whiteLargeLabel);

                    GUILayout.Space(10);
                    GUILayout.Label("Index = " + debugFrame.ExecutingFrame.stackIndex);
                    GUILayout.Label("Max Size = " + debugFrame.ExecutingFrame.stack.Length);


                    // Stack elements
                    GUILayout.Space(10);
                    GUILayout.Label("Stack data");

                    Color old = GUI.contentColor;
                    for(int i = 0; i < debugFrame.ExecutingFrame.stack.Length; i++)
                    {
                        if(i == debugFrame.ExecutingFrame.stackIndex - 1)
                        {
                            GUI.contentColor = Color.red;
                            GUILayout.Box("[" + i + "]: " + debugFrame.ExecutingFrame.stack[i], labelStyle);
                        }
                        else
                        {
                            GUILayout.Label("[" + i + "]: " + debugFrame.ExecutingFrame.stack[i], labelStyle);
                        }
                        GUI.contentColor = old;
                    }
                }
                GUILayout.EndVertical();


            }
            GUILayout.EndHorizontal();
        }
#endif
    }
}
