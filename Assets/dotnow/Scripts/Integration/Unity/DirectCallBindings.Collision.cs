#if !UNITY_DISABLE
#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_IOS || UNITY_ANDROID || UNITY_WSA || UNITY_WEBGL || UNITY_SWITCH)
using dotnow;
using dotnow.Runtime;

namespace UnityEngine
{
    internal static partial class DirectCallBindings
    {
        [Preserve]
        [CLRMethodDirectCallBinding(typeof(Collision), "get_collider")]
        public static void UnityEngine_Collision_GetCollider(StackData[] stack, int offset)
        {
            stack[offset].refValue = ((Collision)stack[offset].refValue).collider;
            stack[offset].type = StackData.ObjectType.Ref;
        }
    }
}
#endif
#endif