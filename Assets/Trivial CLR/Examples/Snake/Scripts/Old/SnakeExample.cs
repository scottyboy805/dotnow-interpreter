//using RoslynCSharp;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using UnityEngine;

//namespace TrivialCLR.Example
//{
//    public class SnakeExample : MonoBehaviour
//    {
//        // Private
//        private ScriptDomain domain = null;
//        private GameObject snakeGameRoot = null;
//        private string source = "";

//        // Public
//        public AssemblyReferenceAsset[] assemblyReferences;
//        public TextAsset templateCode;

//        public LegacyGUIDebugger debugger;

//        // Methods
//        public void Start()
//        {
//            List<Vector2Int> dummyList = new List<Vector2Int>();
//            // Create the domain
//            domain = ScriptDomain.CreateDomain("SnakeCode", true);
//            domain.GetAppDomain().AttachDebugger(debugger);
//            source = templateCode.text;

//            RunSnakeGame();

//            //Instantiate<GameObject>(null);
//            //Instantiate<GameObject>(null, Vector3.zero, Quaternion.identity);
//            //Instantiate<GameObject>(null, Vector3.zero, Quaternion.identity, null);
//        }

//        public void RunSnakeGame()
//        {
//            Debug.Log("Run snake game");
//            // Compile and load the code
//            ScriptAssembly asm = domain.CompileAndLoadSourceInterpreted(source, ScriptSecurityMode.UseSettings, assemblyReferences);
//            Debug.Log("After compile");
//            // Find the snake type that user created
//            ScriptType gameType = asm.FindSubTypeOf<SnakeGameBase>();

//            // Simple error message
//            if (gameType == null)
//                throw new Exception("Snake game code does not define a class that inherits from SnakeGameBase");

//            // Create a holder game object
//            snakeGameRoot = new GameObject("Snake Game");

//            // Create instance to run the game
//            gameType.CreateInstance(snakeGameRoot);
//            //debugger.Pause();
//        }
//    }
//}
