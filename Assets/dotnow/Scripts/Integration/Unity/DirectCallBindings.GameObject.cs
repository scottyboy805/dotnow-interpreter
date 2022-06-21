using dotnow;
using dotnow.Runtime;

namespace UnityEngine
{
    internal static partial class DirectCallBindings
    {
        [Preserve]
        [CLRMethodDirectCallBinding(typeof(GameObject), "get_transform")]
        public static void unityEngine_GameObject_GetTransform(StackData[] stack, int offset)
        {
            stack[offset].refValue = ((GameObject)stack[offset].refValue).transform;
            stack[offset].type = StackData.ObjectType.Ref;
        }
    }
}
