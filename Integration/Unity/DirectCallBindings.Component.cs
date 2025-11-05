#if !UNITY_DISABLE
#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_IOS || UNITY_ANDROID || UNITY_WSA || UNITY_WEBGL || UNITY_SWITCH)
using System.Threading.Tasks;
using dotnow;
using dotnow.Runtime;

namespace UnityEngine
{
    internal static partial class DirectCallBindings
    {
        [Preserve]
        [CLRMethodDirectCallBinding(typeof(Component), "get_transform")]
        public static void UnityEngine_Component_GetTransform(StackData[] stack, int offset)
        {
            stack[offset].Ref = ((Component)stack[offset].Ref.Unwrap()).transform;
            stack[offset].Type = StackType.Ref;
        }
    }
}
#endif
#endif