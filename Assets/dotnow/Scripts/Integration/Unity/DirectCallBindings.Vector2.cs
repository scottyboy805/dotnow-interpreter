#if !UNITY_DISABLE
#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_IOS || UNITY_ANDROID || UNITY_WSA || UNITY_WEBGL)
using dotnow;
using dotnow.Runtime;

namespace UnityEngine
{
    internal static partial class DirectCallBindings
    {
        [Preserve]
        [CLRMethodDirectCallBinding(typeof(Vector2), "op_Subtraction", typeof(Vector2), typeof(Vector2))]
        public static void UnityEngine_Vector2_Subtraction(StackData[] stack, int offset)
        {
            stack[offset].refValue = (Vector2)stack[offset].refValue - (Vector2)stack[offset + 1].refValue;
        }
    }
}
#endif
#endif