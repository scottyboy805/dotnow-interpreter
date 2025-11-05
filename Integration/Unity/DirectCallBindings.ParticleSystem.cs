#if !UNITY_DISABLE
#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_IOS || UNITY_ANDROID || UNITY_WSA || UNITY_WEBGL || UNITY_SWITCH)
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
            stack[offset].Int32 = ((ParticleSystem)stack[offset].Ref).isPlaying ? 1 : 0;
            stack[offset].Type = StackType.Int32;
        }

        [Preserve]
        [CLRMethodDirectCallBinding(typeof(ParticleSystem), "set_emissionRate", typeof(float))]
        public static void UnityEngine_ParticleSystem_SetEmissionRate(StackData[] stack, int offset)
        {
#pragma warning disable CS0618
            ((ParticleSystem)stack[offset].Ref).emissionRate = stack[offset + 1].Single;
#pragma warning restore CS0618
        }

        [Preserve]
        [CLRMethodDirectCallBinding(typeof(ParticleSystem.EmissionModule), "set_rateOverTimeMultiplier", typeof(float))]
        public static void UnityEngine_ParticleSystem_SetRateOverTime(StackData[] stack, int offset)
        {
            ParticleSystem.EmissionModule emission = stack[offset].GetRefValue<ParticleSystem.EmissionModule>();
            emission.rateOverTimeMultiplier = stack[offset + 1].Single;
        }

        [Preserve]
        [CLRMethodDirectCallBinding(typeof(ParticleSystem.EmissionModule), "set_rateOverDistanceMultiplier", typeof(float))]
        public static void UnityEngine_ParticleSystem_SetRateOverDistanceMultiplier(StackData[] stack, int offset)
        {
            ParticleSystem.EmissionModule emission = stack[offset].GetRefValue<ParticleSystem.EmissionModule>();
            emission.rateOverDistanceMultiplier = stack[offset + 1].Single;
        }

        [Preserve]
        [CLRMethodDirectCallBinding(typeof(ParticleSystem), "get_emissionRate")]
        public static void UnityEngine_ParticleSystem_GetEmissionRate(StackData[] stack, int offset)
        {
#pragma warning disable CS0618
            stack[offset].Single = ((ParticleSystem)stack[offset].Ref).emissionRate;
            stack[offset].Type = StackType.Single;
#pragma warning restore CS0618
        }

        [Preserve]
        [CLRMethodDirectCallBinding(typeof(ParticleSystem.EmissionModule), "get_rateOverTime")]
        public static void UnityEngine_ParticleSystem_GetRateOverTime(StackData[] stack, int offset)
        {
            ParticleSystem.EmissionModule emission = stack[offset].GetRefValue<ParticleSystem.EmissionModule>();
            stack[offset].Ref = emission.rateOverTime;
            stack[offset].Type = StackType.Ref;
        }
    }
}
#endif
#endif