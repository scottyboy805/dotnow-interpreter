#if !UNITY_DISABLE
#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_IOS || UNITY_ANDROID || UNITY_WSA || UNITY_WEBGL || UNITY_SWITCH)
using dotnow;
using dotnow.Runtime;

namespace UnityEngine
{
    internal static partial class DirectCallBindings
    {
        [Preserve]
        [CLRMethodDirectCallBinding(typeof(Time), "get_time")]
        public static void UnityEngine_Time_GetTime(StackData[] stack, int offset)
        {
            stack[offset].value.Single = Time.time;
            stack[offset].type = StackData.ObjectType.Single;
        }
    }
}
#endif
#endif