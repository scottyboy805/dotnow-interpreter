using System.Threading.Tasks;
using dotnow;
using dotnow.Runtime;

namespace UnityEngine
{
    internal static partial class DirectCallBindings
    {
        [Preserve]
        [CLRMethodDirectCallBinding(typeof(Component), "get_transform")]
        public static void unityEngine_Component_GetTransform(StackData[] stack, int offset)
        {
            stack[offset].refValue = ((Component)stack[offset].refValue.Unwrap()).transform;
            stack[offset].type = StackData.ObjectType.Ref;
        }
    }
}
