//#if !UNITY_DISABLE
//#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_IOS || UNITY_ANDROID || UNITY_WSA || UNITY_WEBGL || UNITY_SWITCH)
//using dotnow;
//using dotnow.Runtime;

//namespace UnityEngine
//{
//    internal static partial class DirectCallBindings
//    {
//        // In 6.3a5 Unity changed the math and associated classes to use `in` keyword which equates to byref in runtime.
//        // For now this is breaking bindings so the simple solution is disable it in this and newer versions until a correct fix can be implemented for bindings.
//#if !UNITY_6000_3_OR_NEWER
//        [Preserve]
//        [CLRMethodDirectCallBinding(typeof(Vector2), "op_Subtraction", typeof(Vector2), typeof(Vector2))]
//        public static void UnityEngine_Vector2_Subtraction(StackData[] stack, int offset)
//        {
//            stack[offset].Ref = (Vector2)stack[offset].Ref - (Vector2)stack[offset + 1].Ref;
//        }
//#endif
//    }
//}
//#endif
//#endif