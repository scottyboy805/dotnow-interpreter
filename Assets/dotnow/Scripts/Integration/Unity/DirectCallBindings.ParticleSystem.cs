#if !UNITY_DISABLE
#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_IOS || UNITY_ANDROID || UNITY_WSA || UNITY_WEBGL)
using dotnow;
using dotnow.Runtime;

namespace UnityEngine
{
    internal static partial class DirectCallBindings
    {
        [Preserve]
        [CLRMethodDirectCallBinding(typeof(ParticleSystem), "get_isPlaying")]
        public static void UnityEngine_ParticleSystem_IsPlaying(StackData[] stack, int offset)
        {
            stack[offset].refValue = ((ParticleSystem)stack[offset].refValue).isPlaying;
            stack[offset].type = StackData.ObjectType.Ref;
        }

        [Preserve]
        [CLRMethodDirectCallBinding(typeof(ParticleSystem), "set_emissionRate", typeof(float))]
        public static void UnityEngine_ParticleSystem_SetEmissionRate(StackData[] stack, int offset)
        {
            ((ParticleSystem)stack[offset].refValue).emissionRate = stack[offset + 1].value.Single;
        }

        [Preserve]
        [CLRMethodDirectCallBinding(typeof(ParticleSystem), "get_emissionRate")]
        public static void UnityEngine_ParticleSystem_GetEmissionRate(StackData[] stack, int offset)
        {
            stack[offset].refValue = ((ParticleSystem)stack[offset].refValue).emissionRate;
            stack[offset].type = StackData.ObjectType.Ref;
        }
    }
}
#endif
#endif