#if (UNITY_EDITOR || UNITY_STANDALONE) && UNITY_DISABLE == false
using System.Collections;
using System.Collections.Generic;
using dotnow;
using dotnow.Runtime;

namespace UnityEngine
{

    internal static class DirectCallBindings
    {
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


        //[Preserve]
        //[CLRFieldDirectAccessBinding(typeof(Vector3), "x", CLRFieldAccessMode.Read)]
        //public static void UnityEngine_Vector3_Xr(StackData[] stack, int offset)
        //{
        //    StackData.Alloc(ref stack[offset], ((Vector3)((IByRef)stack[offset].refValue).GetReferenceValue().refValue).x);
        //}

        //[Preserve]
        //[CLRFieldDirectAccessBinding(typeof(Vector3), "x", CLRFieldAccessMode.Write)]
        //public static void UnityEngine_Vector3_Xw(StackData[] stack, int offset)
        //{
        //    ((IByRef)stack[offset].refValue).SetReferenceValueR4(stack[1].value.Single);


        //    // Cannot set value directly due to non-reference type. Struct copy is required
        //    //Vector3 temp = (Vector3)((IByRef)stack[offset].refValue).GetReferenceValue().refValue;
        //    //temp.x = stack[offset + 1].value.Single;

        //    //stack[offset].refValue = temp;
        //}
    }
}
#endif