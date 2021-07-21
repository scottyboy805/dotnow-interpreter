//using RoslynCSharp;
//using UnityEngine;

//namespace TrivialCLR.Examples
//{
//    public class Ex_Fibonacci : MonoBehaviour
//    {
//        // Private
//        private string source = @"
//            public class Example
//            {
//                public static int Fibonacci(int n)
//                {
//                    int a = 0;
//                    int b = 1;
//                    // In N steps, compute Fibonacci sequence iteratively.
//                    for (int i = 0; i < n; i++)
//                    {
//                        int temp = a;
//                        a = b;
//                        b = temp + b;
//                    }
//                    return a;
//                }
//            }";

//        // Methods
//        public void Start()
//        {
//            // Create a script domain as usual
//            ScriptDomain domain = ScriptDomain.CreateDomain("Example", true);

//            // Compile the source code in interpreted mode
//            ScriptType mainType = domain.CompileAndLoadMainSourceInterpreted(source);

//            // Call the finbonaci method in external code
//            for(int i = 0; i < 20; i++)
//            {
//                Debug.Log(mainType.CallStatic("Fibonacci", i));
//            }
//        }
//    }
//}
