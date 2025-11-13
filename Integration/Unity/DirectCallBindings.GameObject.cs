#if !UNITY_DISABLE
#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_IOS || UNITY_ANDROID || UNITY_WSA || UNITY_WEBGL || UNITY_SWITCH)
using dotnow.Interop;

namespace UnityEngine
{
    internal static partial class DirectCallBindings
    {
        [Preserve]
        [CLRMethodBinding(typeof(GameObject), "get_transform")]
        public static void UnityEngine_GameObject_GetTransform(StackContext context)
        {
            // Get the game object instance from the stack, at offset 0
            GameObject go = context.ReadArgObject<GameObject>(0);

            // Write the transform back to the stack as the return value
            context.ReturnObject<Transform>(go.transform);
        }
    }
}
#endif
#endif