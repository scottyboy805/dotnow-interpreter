#if !UNITY_DISABLE
#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_IOS || UNITY_ANDROID || UNITY_WSA || UNITY_WEBGL || UNITY_SWITCH)
using dotnow;
using dotnow.Runtime;

namespace UnityEngine
{
    internal static partial class DirectCallBindings
    {
        [Preserve]
        [CLRMethodDirectCallBinding(typeof(MeshFilter), "get_mesh")]
        public static void UnityEngine_MeshFilter_GetMesh(StackData[] stack, int offset)
        {
            stack[offset].Ref = ((MeshFilter)stack[offset].Ref).mesh;
            stack[offset].Type = StackType.Ref;
        }
    }
}
#endif
#endif