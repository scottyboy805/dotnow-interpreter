#if !UNITY_DISABLE
#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_IOS || UNITY_ANDROID || UNITY_WSA || UNITY_WEBGL)
using dotnow;
using dotnow.Runtime;

namespace UnityEngine
{
    internal static partial class DirectCallBindings
    {
        [Preserve]
        [CLRMethodDirectCallBinding(typeof(Rigidbody), "set_velocity", typeof(Vector3))]
        public static void UnityEngine_Rigidbody_SetVelocity(StackData[] stack, int offset)
        {
            ((Rigidbody)stack[offset].refValue).velocity = (Vector3)stack[offset + 1].refValue;
        }

        [Preserve]
        [CLRMethodDirectCallBinding(typeof(Rigidbody), "set_angularVelocity", typeof(Vector3))]
        public static void UnityEngine_Rigidbody_SetAngularVelocity(StackData[] stack, int offset)
        {
            ((Rigidbody)stack[offset].refValue).angularVelocity = (Vector3)stack[offset + 1].refValue;
        }
    }
}
#endif
#endif