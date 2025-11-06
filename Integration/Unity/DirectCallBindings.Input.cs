//#if !UNITY_DISABLE
//#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_IOS || UNITY_ANDROID || UNITY_WSA || UNITY_WEBGL || UNITY_SWITCH)
//using dotnow;
//using dotnow.Runtime;

//namespace UnityEngine
//{
//    internal static partial class DirectCallBindings
//    {
//        [Preserve]
//        [CLRMethodDirectCallBinding(typeof(Input), "GetKey", typeof(KeyCode))]
//        public static void UnityEngine_Input_GetKey(StackData[] stack, int offset)
//        {
//            stack[offset].Int32 = Input.GetKey((KeyCode)stack[offset].Int32) ? 1 : 0;
//            stack[offset].Type = StackType.Int32;
//        }

//        [Preserve]
//        [CLRMethodDirectCallBinding(typeof(Input), "GetKeyDown", typeof(KeyCode))]
//        public static void UnityEngine_Input_GetKeyDown(StackData[] stack, int offset)
//        {
//            stack[offset].Int32 = Input.GetKeyDown((KeyCode)stack[offset].Int32) ? 1 : 0;
//            stack[offset].Type = StackType.Int32;
//        }

//        [Preserve]
//        [CLRMethodDirectCallBinding(typeof(Input), "GetKeyUp", typeof(KeyCode))]
//        public static void UnityEngine_Input_GetKeyUp(StackData[] stack, int offset)
//        {
//            stack[offset].Int32 = Input.GetKeyUp((KeyCode)stack[offset].Int32) ? 1 : 0;
//            stack[offset].Type = StackType.Int32;
//        }
//    }
//}
//#endif
//#endif