//using RoslynCSharp;
//using UnityEngine;

//namespace Examples
//{
//    /// <summary>
//    /// IMPORTANT - This script requires an 'ICLRProxy' for inheritance of the 'MonoBehaviour' type.
//    /// </summary>
//    public class Ex_SpinningObject : MonoBehaviour
//    {
//        // Private
//        private string source = @"
//            using UnityEngine;
//            public class Example : MonoBehaviour
//            {
//                public float rotateSpeed = 50f;
//                void Update()
//                {
//                    transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
//                }
//            }";

//        public AssemblyReferenceAsset[] references;
//        public Transform somOtherObject;

//        // Methods
//        public void Start()
//        {
//            // Create a script domain as usual
//            ScriptDomain domain = ScriptDomain.CreateDomain("Example", true);

//            // Compile the source code in interpreted mode
//            ScriptType mainType = domain.CompileAndLoadMainSourceInterpreted(source, ScriptSecurityMode.UseSettings, references);

//            // Create component instance
//            mainType.CreateInstance(gameObject);
//        }

//        public void Update()
//        {
//            somOtherObject.Rotate(Vector3.up, 3f);
//        }

//    }
//}
