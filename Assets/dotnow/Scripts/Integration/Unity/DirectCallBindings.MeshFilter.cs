#if !UNITY_DISABLE
#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_IOS || UNITY_ANDROID || UNITY_WSA || UNITY_WEBGL)
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
            stack[offset].refValue = ((MeshFilter)stack[offset].refValue).mesh;
            stack[offset].type = StackData.ObjectType.Ref;
        }
    }
}
#endif
#endif