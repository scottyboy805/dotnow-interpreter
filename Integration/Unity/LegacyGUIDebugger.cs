//#if !UNITY_DISABLE
//#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_IOS || UNITY_ANDROID || UNITY_WSA || UNITY_WEBGL || UNITY_SWITCH)
//using dotnow.Debugging;
//using dotnow.Runtime;
//using UnityEditor;
//using UnityEngine;

//namespace dotnow
//{
//    public class LegacyGUIDebugger : MonoBehaviour, IDebugger
//    {
//        // Private
//        private AppDomain domain = null;
//        private ExecutionEngine engine = null;
//        private DebugFrame debugFrame = null;
//        private Vector2 scrollInstructions = Vector2.zero;
//        private Vector3 scrollEvalStack = Vector2.zero;

//        // Public
//        public bool pauseExecutionOnStart = false;

//        // Methods
//        public void OnAttachDebugger(AppDomain domain, ExecutionEngine engine)
//        {
//            this.domain = domain;
//            this.engine = engine;

//            if (pauseExecutionOnStart == true)
//                engine.PauseExecution();
//        }

//        public void OnDebugFrame(DebugFrame frame)
//        {
//            this.debugFrame = frame;
//        }

//        public void Pause()
//        {
//            if (engine != null)
//                engine.PauseExecution();
//        }

//#if !UNITY_DISABLE
//#if UNITY_EDITOR
//        public void Start()
//        {
//            // Add listener
//            AppDomain.OnDomainCreated += (AppDomain domain) =>
//            {
//                domain.AttachDebugger(this);
//            };
//        }

//        public void OnGUI()
//        {
//            if (engine == null || engine.IsDebuggerPaused == false)
//                return;

//            // Set gui scale
//            GUI.matrix = Matrix4x4.Scale(Vector3.one * 2);

//            GUIStyle labelStyle = new GUIStyle(EditorStyles.miniLabel);
//            labelStyle.normal.textColor = Color.white;

//            // Split page layout
//            GUILayout.BeginHorizontal();
//            {
//                GUILayout.BeginVertical();
//                {
//                    GUILayout.BeginHorizontal();
//                    {
//                        if (GUILayout.Button("Step Once (F10)") == true || (Event.current.type == EventType.KeyDown && Input.GetKeyDown(KeyCode.F10) == true))
//                        {
//                            engine.StepExecution();
//                        }

//                        if (GUILayout.Button("Continue (F5)") == true || (Event.current.type == EventType.KeyDown && Input.GetKeyDown(KeyCode.F5) == true))
//                        {
//                            engine.ContinueExecution();
//                        }
//                    }
//                    GUILayout.EndHorizontal();

//                    // Main instruction set layout
//                    GUILayout.BeginVertical(GUI.skin.box);
//                    {
//                        GUILayout.Label("Execution paused at:", EditorStyles.whiteLargeLabel);
//                        GUILayout.Label(debugFrame.ExecutingMethod.ToString());
//                    }
//                    GUILayout.EndVertical();

//                    Color old = GUI.contentColor;

//                    // Start scroll view
//                    scrollInstructions = GUILayout.BeginScrollView(scrollInstructions, GUI.skin.box);
//                    {
//                        for (int i = 0; i < debugFrame.InstructionSet.Length; i++)
//                        {
//                            if (i == debugFrame.ExecutingInstructionIndex)
//                            {
//                                GUI.contentColor = Color.red;
//                                GUILayout.Box("[" + i + "]: " + debugFrame.InstructionSet[i].ToString(), labelStyle);
//                            }
//                            else
//                            {
//                                GUILayout.Label("[" + i + "]: " + debugFrame.InstructionSet[i].ToString(), labelStyle);
//                            }
//                            GUI.contentColor = old;
//                        }
//                    }
//                    GUILayout.EndScrollView();


                    
//                }
//                GUILayout.EndVertical();
//                // Instruction set layout



//                // Main stack layout
//                GUILayout.BeginVertical(GUI.skin.box);
//                {
//                    scrollEvalStack = GUILayout.BeginScrollView(scrollEvalStack);
//                    {
//                        GUILayout.Label("Evaluation Stack", EditorStyles.whiteLargeLabel);

//                        GUILayout.Space(10);
//                        GUILayout.Label("Index = " + debugFrame.ExecutingFrame.stackIndex);
//                        GUILayout.Label("Max Size = " + debugFrame.ExecutingFrame.stack.Length);


//                        // Stack elements
//                        GUILayout.Space(10);
//                        GUILayout.Label("Stack data");

//                        Color old = GUI.contentColor;
//                        for (int i = 0; i < debugFrame.ExecutingFrame.stack.Length; i++)
//                        {
//                            // Check for stack max
//                            if (i > debugFrame.ExecutingFrame.stackMax)
//                                break;

//                            if (i == debugFrame.ExecutingFrame.stackIndex - 1)
//                            {
//                                GUI.contentColor = Color.red;
//                                GUILayout.Box("[" + i + "]: " + debugFrame.ExecutingFrame.stack[i], labelStyle);
//                            }
//                            else
//                            {
//                                GUILayout.Label("[" + i + "]: " + debugFrame.ExecutingFrame.stack[i], labelStyle);
//                            }
//                            GUI.contentColor = old;
//                        }
//                    }
//                    GUILayout.EndScrollView();
//                }
//                GUILayout.EndVertical();


//            }
//            GUILayout.EndHorizontal();
//        }
//#endif
//#endif
//    }
//}
//#endif
//#endif