#if !UNITY_DISABLE
#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_IOS || UNITY_ANDROID || UNITY_WSA || UNITY_WEBGL)
using dotnow;
using dotnow.Runtime;

namespace UnityEngine
{
    internal static partial class DirectCallBindings
    {
        [Preserve]
        [CLRMethodDirectCallBinding(typeof(Collider), "get_gameObject")]
        public static void UnityEngine_Collider_GetGameObject(StackData[] stack, int offset)
        {
            stack[offset].refValue = ((Collider)stack[offset].refValue).gameObject;
            stack[offset].type = StackData.ObjectType.Ref;
        }
    }
}
#endif
#endif