#if !UNITY_DISABLE
#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_IOS || UNITY_ANDROID || UNITY_WSA || UNITY_WEBGL || UNITY_SWITCH)
using System.Collections;
using System.Collections.Generic;
using dotnow;
using dotnow.Runtime;

namespace UnityEngine
{

    internal static partial class DirectCallBindings
    {
        [Preserve]
        [CLRMethodDirectCallBinding(typeof(Transform), "get_position")]
        public static void UnityEngine_Transform_GetPosition(StackData[] stack, int offset)
        {
            stack[offset].refValue = ((Transform)stack[offset].refValue).position;
            stack[offset].type = StackData.ObjectType.Ref;
        }

        [Preserve]
        [CLRMethodDirectCallBinding(typeof(Transform), "get_localPosition")]
        public static void UnityEngine_Transform_GetLocalPosition(StackData[] stack, int offset)
        {
            stack[offset].refValue = ((Transform)stack[offset].refValue).localPosition;
            stack[offset].type = StackData.ObjectType.Ref;
        }

        [Preserve]
        [CLRMethodDirectCallBinding(typeof(Transform), "get_rotation")]
        public static void UnityEngine_Transform_GetRotation(StackData[] stack, int offset)
        {
            stack[offset].refValue = ((Transform)stack[offset].refValue).rotation;
            stack[offset].type = StackData.ObjectType.Ref;
        }

        [Preserve]
        [CLRMethodDirectCallBinding(typeof(Transform), "get_localRotation")]
        public static void GetLocalRotation(StackData[] stack, int offset)
        {
            stack[offset].refValue = ((Transform)stack[offset].refValue).localRotation;
            stack[offset].type = StackData.ObjectType.Ref;
        }

        [Preserve]
        [CLRMethodDirectCallBinding(typeof(Transform), "Rotate", typeof(float), typeof(float), typeof(float))]
        public static void UnityEngine_Transform_Rotate_SingleSingleSingle(StackData[] stack, int offset)
        {
            ((Transform)stack[offset].refValue).Rotate(stack[offset + 1].value.Single, 
                stack[offset + 2].value.Single, 
                stack[offset + 3].value.Single);
        }

        [Preserve]
        [CLRMethodDirectCallBinding(typeof(List<Transform>.Enumerator), "MoveNext")]
        public static void IEnumerator_MoveNext(StackData[] stack, int offset)
        {
            IEnumerator enumerator = (IEnumerator)((IByRef)stack[offset].refValue).GetReferenceValue().refValue;

            StackData.AllocTyped(ref stack[offset], System.TypeCode.Boolean, enumerator.MoveNext());
        }

        [Preserve]
        [CLRMethodDirectCallBinding(typeof(List<Transform>.Enumerator), "get_Current")]
        public static void IEnumerator_getCurrent(StackData[] stack, int offset)
        {
            IEnumerator enumerator = (IEnumerator)((IByRef)stack[offset].refValue).GetReferenceValue().refValue;

            StackData.AllocTyped(ref stack[offset], System.TypeCode.Object, enumerator.Current);
        }
    }
}
#endif
#endif