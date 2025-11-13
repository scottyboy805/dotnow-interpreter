[dotnow.Interop.PreserveAttribute]
public sealed class UnityEngine_Transform
{
    // Binding generated from method: Void SetParent(UnityEngine.Transform)
    [dotnow.Interop.PreserveAttribute, dotnow.Interop.CLRMethodBindingAttribute(typeof(UnityEngine.Transform), "SetParent", typeof(UnityEngine.Transform))]
    public static void UnityEngine_Transform_SetParent_Transform(dotnow.Interop.StackContext context)
    {
        UnityEngine.Transform arg0 = context.ReadArgObject<UnityEngine.Transform>(0);
        UnityEngine.Transform arg1 = context.ReadArgObject<UnityEngine.Transform>(1);
        arg0.SetParent(arg1);
    }

    // Binding generated from method: Void SetParent(UnityEngine.Transform, Boolean)
    [dotnow.Interop.PreserveAttribute, dotnow.Interop.CLRMethodBindingAttribute(typeof(UnityEngine.Transform), "SetParent", typeof(UnityEngine.Transform), typeof(bool))]
    public static void UnityEngine_Transform_SetParent_Transform_Bool(dotnow.Interop.StackContext context)
    {
        UnityEngine.Transform arg0 = context.ReadArgObject<UnityEngine.Transform>(0);
        UnityEngine.Transform arg1 = context.ReadArgObject<UnityEngine.Transform>(1);
        bool arg2 = context.ReadArgValueType<bool>(2);
        arg0.SetParent(arg1, arg2);
    }

    // Binding generated from method: Void SetPositionAndRotation(UnityEngine.Vector3, UnityEngine.Quaternion)
    [dotnow.Interop.PreserveAttribute, dotnow.Interop.CLRMethodBindingAttribute(typeof(UnityEngine.Transform), "SetPositionAndRotation", typeof(UnityEngine.Vector3), typeof(UnityEngine.Quaternion))]
    public static void UnityEngine_Transform_SetPositionAndRotation_Vector3_Quaternion(dotnow.Interop.StackContext context)
    {
        UnityEngine.Transform arg0 = context.ReadArgObject<UnityEngine.Transform>(0);
        UnityEngine.Vector3 arg1 = context.ReadArgValueType<UnityEngine.Vector3>(1);
        UnityEngine.Quaternion arg2 = context.ReadArgValueType<UnityEngine.Quaternion>(2);
        arg0.SetPositionAndRotation(arg1, arg2);
    }

    // Binding generated from method: Void SetLocalPositionAndRotation(UnityEngine.Vector3, UnityEngine.Quaternion)
    [dotnow.Interop.PreserveAttribute, dotnow.Interop.CLRMethodBindingAttribute(typeof(UnityEngine.Transform), "SetLocalPositionAndRotation", typeof(UnityEngine.Vector3), typeof(UnityEngine.Quaternion))]
    public static void UnityEngine_Transform_SetLocalPositionAndRotation_Vector3_Quaternion(dotnow.Interop.StackContext context)
    {
        UnityEngine.Transform arg0 = context.ReadArgObject<UnityEngine.Transform>(0);
        UnityEngine.Vector3 arg1 = context.ReadArgValueType<UnityEngine.Vector3>(1);
        UnityEngine.Quaternion arg2 = context.ReadArgValueType<UnityEngine.Quaternion>(2);
        arg0.SetLocalPositionAndRotation(arg1, arg2);
    }

    // Binding generated from method: Void GetPositionAndRotation(UnityEngine.Vector3 ByRef, UnityEngine.Quaternion ByRef)
    [dotnow.Interop.PreserveAttribute, dotnow.Interop.CLRMethodBindingAttribute(typeof(UnityEngine.Transform), "GetPositionAndRotation", typeof(UnityEngine.Vector3), typeof(UnityEngine.Quaternion))]
    public static void UnityEngine_Transform_GetPositionAndRotation_Vector3_Quaternion(dotnow.Interop.StackContext context)
    {
        UnityEngine.Transform arg0 = context.ReadArgObject<UnityEngine.Transform>(0);
        UnityEngine.Vector3 arg1 = default(UnityEngine.Vector3);
        UnityEngine.Quaternion arg2 = default(UnityEngine.Quaternion);
        arg0.GetPositionAndRotation(out arg1, out arg2);
        context.WriteArgValueType<UnityEngine.Vector3>(1, arg1);
        context.WriteArgValueType<UnityEngine.Quaternion>(2, arg2);
    }

    // Binding generated from method: Void GetLocalPositionAndRotation(UnityEngine.Vector3 ByRef, UnityEngine.Quaternion ByRef)
    [dotnow.Interop.PreserveAttribute, dotnow.Interop.CLRMethodBindingAttribute(typeof(UnityEngine.Transform), "GetLocalPositionAndRotation", typeof(UnityEngine.Vector3), typeof(UnityEngine.Quaternion))]
    public static void UnityEngine_Transform_GetLocalPositionAndRotation_Vector3_Quaternion(dotnow.Interop.StackContext context)
    {
        UnityEngine.Transform arg0 = context.ReadArgObject<UnityEngine.Transform>(0);
        UnityEngine.Vector3 arg1 = default(UnityEngine.Vector3);
        UnityEngine.Quaternion arg2 = default(UnityEngine.Quaternion);
        arg0.GetLocalPositionAndRotation(out arg1, out arg2);
        context.WriteArgValueType<UnityEngine.Vector3>(1, arg1);
        context.WriteArgValueType<UnityEngine.Quaternion>(2, arg2);
    }

    // Binding generated from method: Void Translate(UnityEngine.Vector3, UnityEngine.Space)
    [dotnow.Interop.PreserveAttribute, dotnow.Interop.CLRMethodBindingAttribute(typeof(UnityEngine.Transform), "Translate", typeof(UnityEngine.Vector3), typeof(UnityEngine.Space))]
    public static void UnityEngine_Transform_Translate_Vector3_EnumInt32(dotnow.Interop.StackContext context)
    {
        UnityEngine.Transform arg0 = context.ReadArgObject<UnityEngine.Transform>(0);
        UnityEngine.Vector3 arg1 = context.ReadArgValueType<UnityEngine.Vector3>(1);
        UnityEngine.Space arg2 = context.ReadArgValueType<UnityEngine.Space>(2);
        arg0.Translate(arg1, arg2);
    }

    // Binding generated from method: Void Translate(UnityEngine.Vector3)
    [dotnow.Interop.PreserveAttribute, dotnow.Interop.CLRMethodBindingAttribute(typeof(UnityEngine.Transform), "Translate", typeof(UnityEngine.Vector3))]
    public static void UnityEngine_Transform_Translate_Vector3(dotnow.Interop.StackContext context)
    {
        UnityEngine.Transform arg0 = context.ReadArgObject<UnityEngine.Transform>(0);
        UnityEngine.Vector3 arg1 = context.ReadArgValueType<UnityEngine.Vector3>(1);
        arg0.Translate(arg1);
    }

    // Binding generated from method: Void Translate(Single, Single, Single, UnityEngine.Space)
    [dotnow.Interop.PreserveAttribute, dotnow.Interop.CLRMethodBindingAttribute(typeof(UnityEngine.Transform), "Translate", typeof(float), typeof(float), typeof(float), typeof(UnityEngine.Space))]
    public static void UnityEngine_Transform_Translate_Float_Float_Float_EnumInt32(dotnow.Interop.StackContext context)
    {
        UnityEngine.Transform arg0 = context.ReadArgObject<UnityEngine.Transform>(0);
        float arg1 = context.ReadArgValueType<float>(1);
        float arg2 = context.ReadArgValueType<float>(2);
        float arg3 = context.ReadArgValueType<float>(3);
        UnityEngine.Space arg4 = context.ReadArgValueType<UnityEngine.Space>(4);
        arg0.Translate(arg1, arg2, arg3, arg4);
    }

    // Binding generated from method: Void Translate(Single, Single, Single)
    [dotnow.Interop.PreserveAttribute, dotnow.Interop.CLRMethodBindingAttribute(typeof(UnityEngine.Transform), "Translate", typeof(float), typeof(float), typeof(float))]
    public static void UnityEngine_Transform_Translate_Float_Float_Float(dotnow.Interop.StackContext context)
    {
        UnityEngine.Transform arg0 = context.ReadArgObject<UnityEngine.Transform>(0);
        float arg1 = context.ReadArgValueType<float>(1);
        float arg2 = context.ReadArgValueType<float>(2);
        float arg3 = context.ReadArgValueType<float>(3);
        arg0.Translate(arg1, arg2, arg3);
    }

    // Binding generated from method: Void Translate(UnityEngine.Vector3, UnityEngine.Transform)
    [dotnow.Interop.PreserveAttribute, dotnow.Interop.CLRMethodBindingAttribute(typeof(UnityEngine.Transform), "Translate", typeof(UnityEngine.Vector3), typeof(UnityEngine.Transform))]
    public static void UnityEngine_Transform_Translate_Vector3_Transform(dotnow.Interop.StackContext context)
    {
        UnityEngine.Transform arg0 = context.ReadArgObject<UnityEngine.Transform>(0);
        UnityEngine.Vector3 arg1 = context.ReadArgValueType<UnityEngine.Vector3>(1);
        UnityEngine.Transform arg2 = context.ReadArgObject<UnityEngine.Transform>(2);
        arg0.Translate(arg1, arg2);
    }

    // Binding generated from method: Void Translate(Single, Single, Single, UnityEngine.Transform)
    [dotnow.Interop.PreserveAttribute, dotnow.Interop.CLRMethodBindingAttribute(typeof(UnityEngine.Transform), "Translate", typeof(float), typeof(float), typeof(float), typeof(UnityEngine.Transform))]
    public static void UnityEngine_Transform_Translate_Float_Float_Float_Transform(dotnow.Interop.StackContext context)
    {
        UnityEngine.Transform arg0 = context.ReadArgObject<UnityEngine.Transform>(0);
        float arg1 = context.ReadArgValueType<float>(1);
        float arg2 = context.ReadArgValueType<float>(2);
        float arg3 = context.ReadArgValueType<float>(3);
        UnityEngine.Transform arg4 = context.ReadArgObject<UnityEngine.Transform>(4);
        arg0.Translate(arg1, arg2, arg3, arg4);
    }

    // Binding generated from method: Void Rotate(UnityEngine.Vector3, UnityEngine.Space)
    [dotnow.Interop.PreserveAttribute, dotnow.Interop.CLRMethodBindingAttribute(typeof(UnityEngine.Transform), "Rotate", typeof(UnityEngine.Vector3), typeof(UnityEngine.Space))]
    public static void UnityEngine_Transform_Rotate_Vector3_EnumInt32(dotnow.Interop.StackContext context)
    {
        UnityEngine.Transform arg0 = context.ReadArgObject<UnityEngine.Transform>(0);
        UnityEngine.Vector3 arg1 = context.ReadArgValueType<UnityEngine.Vector3>(1);
        UnityEngine.Space arg2 = context.ReadArgValueType<UnityEngine.Space>(2);
        arg0.Rotate(arg1, arg2);
    }

    // Binding generated from method: Void Rotate(UnityEngine.Vector3)
    [dotnow.Interop.PreserveAttribute, dotnow.Interop.CLRMethodBindingAttribute(typeof(UnityEngine.Transform), "Rotate", typeof(UnityEngine.Vector3))]
    public static void UnityEngine_Transform_Rotate_Vector3(dotnow.Interop.StackContext context)
    {
        UnityEngine.Transform arg0 = context.ReadArgObject<UnityEngine.Transform>(0);
        UnityEngine.Vector3 arg1 = context.ReadArgValueType<UnityEngine.Vector3>(1);
        arg0.Rotate(arg1);
    }

    // Binding generated from method: Void Rotate(Single, Single, Single, UnityEngine.Space)
    [dotnow.Interop.PreserveAttribute, dotnow.Interop.CLRMethodBindingAttribute(typeof(UnityEngine.Transform), "Rotate", typeof(float), typeof(float), typeof(float), typeof(UnityEngine.Space))]
    public static void UnityEngine_Transform_Rotate_Float_Float_Float_EnumInt32(dotnow.Interop.StackContext context)
    {
        UnityEngine.Transform arg0 = context.ReadArgObject<UnityEngine.Transform>(0);
        float arg1 = context.ReadArgValueType<float>(1);
        float arg2 = context.ReadArgValueType<float>(2);
        float arg3 = context.ReadArgValueType<float>(3);
        UnityEngine.Space arg4 = context.ReadArgValueType<UnityEngine.Space>(4);
        arg0.Rotate(arg1, arg2, arg3, arg4);
    }

    // Binding generated from method: Void Rotate(Single, Single, Single)
    [dotnow.Interop.PreserveAttribute, dotnow.Interop.CLRMethodBindingAttribute(typeof(UnityEngine.Transform), "Rotate", typeof(float), typeof(float), typeof(float))]
    public static void UnityEngine_Transform_Rotate_Float_Float_Float(dotnow.Interop.StackContext context)
    {
        UnityEngine.Transform arg0 = context.ReadArgObject<UnityEngine.Transform>(0);
        float arg1 = context.ReadArgValueType<float>(1);
        float arg2 = context.ReadArgValueType<float>(2);
        float arg3 = context.ReadArgValueType<float>(3);
        arg0.Rotate(arg1, arg2, arg3);
    }

    // Binding generated from method: Void Rotate(UnityEngine.Vector3, Single, UnityEngine.Space)
    [dotnow.Interop.PreserveAttribute, dotnow.Interop.CLRMethodBindingAttribute(typeof(UnityEngine.Transform), "Rotate", typeof(UnityEngine.Vector3), typeof(float), typeof(UnityEngine.Space))]
    public static void UnityEngine_Transform_Rotate_Vector3_Float_EnumInt32(dotnow.Interop.StackContext context)
    {
        UnityEngine.Transform arg0 = context.ReadArgObject<UnityEngine.Transform>(0);
        UnityEngine.Vector3 arg1 = context.ReadArgValueType<UnityEngine.Vector3>(1);
        float arg2 = context.ReadArgValueType<float>(2);
        UnityEngine.Space arg3 = context.ReadArgValueType<UnityEngine.Space>(3);
        arg0.Rotate(arg1, arg2, arg3);
    }

    // Binding generated from method: Void Rotate(UnityEngine.Vector3, Single)
    [dotnow.Interop.PreserveAttribute, dotnow.Interop.CLRMethodBindingAttribute(typeof(UnityEngine.Transform), "Rotate", typeof(UnityEngine.Vector3), typeof(float))]
    public static void UnityEngine_Transform_Rotate_Vector3_Float(dotnow.Interop.StackContext context)
    {
        UnityEngine.Transform arg0 = context.ReadArgObject<UnityEngine.Transform>(0);
        UnityEngine.Vector3 arg1 = context.ReadArgValueType<UnityEngine.Vector3>(1);
        float arg2 = context.ReadArgValueType<float>(2);
        arg0.Rotate(arg1, arg2);
    }

    // Binding generated from method: Void RotateAround(UnityEngine.Vector3, UnityEngine.Vector3, Single)
    [dotnow.Interop.PreserveAttribute, dotnow.Interop.CLRMethodBindingAttribute(typeof(UnityEngine.Transform), "RotateAround", typeof(UnityEngine.Vector3), typeof(UnityEngine.Vector3), typeof(float))]
    public static void UnityEngine_Transform_RotateAround_Vector3_Vector3_Float(dotnow.Interop.StackContext context)
    {
        UnityEngine.Transform arg0 = context.ReadArgObject<UnityEngine.Transform>(0);
        UnityEngine.Vector3 arg1 = context.ReadArgValueType<UnityEngine.Vector3>(1);
        UnityEngine.Vector3 arg2 = context.ReadArgValueType<UnityEngine.Vector3>(2);
        float arg3 = context.ReadArgValueType<float>(3);
        arg0.RotateAround(arg1, arg2, arg3);
    }

    // Binding generated from method: Void LookAt(UnityEngine.Transform, UnityEngine.Vector3)
    [dotnow.Interop.PreserveAttribute, dotnow.Interop.CLRMethodBindingAttribute(typeof(UnityEngine.Transform), "LookAt", typeof(UnityEngine.Transform), typeof(UnityEngine.Vector3))]
    public static void UnityEngine_Transform_LookAt_Transform_Vector3(dotnow.Interop.StackContext context)
    {
        UnityEngine.Transform arg0 = context.ReadArgObject<UnityEngine.Transform>(0);
        UnityEngine.Transform arg1 = context.ReadArgObject<UnityEngine.Transform>(1);
        UnityEngine.Vector3 arg2 = context.ReadArgValueType<UnityEngine.Vector3>(2);
        arg0.LookAt(arg1, arg2);
    }

    // Binding generated from method: Void LookAt(UnityEngine.Transform)
    [dotnow.Interop.PreserveAttribute, dotnow.Interop.CLRMethodBindingAttribute(typeof(UnityEngine.Transform), "LookAt", typeof(UnityEngine.Transform))]
    public static void UnityEngine_Transform_LookAt_Transform(dotnow.Interop.StackContext context)
    {
        UnityEngine.Transform arg0 = context.ReadArgObject<UnityEngine.Transform>(0);
        UnityEngine.Transform arg1 = context.ReadArgObject<UnityEngine.Transform>(1);
        arg0.LookAt(arg1);
    }

    // Binding generated from method: Void LookAt(UnityEngine.Vector3, UnityEngine.Vector3)
    [dotnow.Interop.PreserveAttribute, dotnow.Interop.CLRMethodBindingAttribute(typeof(UnityEngine.Transform), "LookAt", typeof(UnityEngine.Vector3), typeof(UnityEngine.Vector3))]
    public static void UnityEngine_Transform_LookAt_Vector3_Vector3(dotnow.Interop.StackContext context)
    {
        UnityEngine.Transform arg0 = context.ReadArgObject<UnityEngine.Transform>(0);
        UnityEngine.Vector3 arg1 = context.ReadArgValueType<UnityEngine.Vector3>(1);
        UnityEngine.Vector3 arg2 = context.ReadArgValueType<UnityEngine.Vector3>(2);
        arg0.LookAt(arg1, arg2);
    }

    // Binding generated from method: Void LookAt(UnityEngine.Vector3)
    [dotnow.Interop.PreserveAttribute, dotnow.Interop.CLRMethodBindingAttribute(typeof(UnityEngine.Transform), "LookAt", typeof(UnityEngine.Vector3))]
    public static void UnityEngine_Transform_LookAt_Vector3(dotnow.Interop.StackContext context)
    {
        UnityEngine.Transform arg0 = context.ReadArgObject<UnityEngine.Transform>(0);
        UnityEngine.Vector3 arg1 = context.ReadArgValueType<UnityEngine.Vector3>(1);
        arg0.LookAt(arg1);
    }

    // Binding generated from method: UnityEngine.Vector3 TransformDirection(UnityEngine.Vector3)
    [dotnow.Interop.PreserveAttribute, dotnow.Interop.CLRMethodBindingAttribute(typeof(UnityEngine.Transform), "TransformDirection", typeof(UnityEngine.Vector3))]
    public static void UnityEngine_Transform_TransformDirection_Vector3(dotnow.Interop.StackContext context)
    {
        UnityEngine.Transform arg0 = context.ReadArgObject<UnityEngine.Transform>(0);
        UnityEngine.Vector3 arg1 = context.ReadArgValueType<UnityEngine.Vector3>(1);
        UnityEngine.Vector3 result = arg0.TransformDirection(arg1);
        context.ReturnValueType<UnityEngine.Vector3>(result);
    }

    // Binding generated from method: UnityEngine.Vector3 TransformDirection(Single, Single, Single)
    [dotnow.Interop.PreserveAttribute, dotnow.Interop.CLRMethodBindingAttribute(typeof(UnityEngine.Transform), "TransformDirection", typeof(float), typeof(float), typeof(float))]
    public static void UnityEngine_Transform_TransformDirection_Float_Float_Float(dotnow.Interop.StackContext context)
    {
        UnityEngine.Transform arg0 = context.ReadArgObject<UnityEngine.Transform>(0);
        float arg1 = context.ReadArgValueType<float>(1);
        float arg2 = context.ReadArgValueType<float>(2);
        float arg3 = context.ReadArgValueType<float>(3);
        UnityEngine.Vector3 result = arg0.TransformDirection(arg1, arg2, arg3);
        context.ReturnValueType<UnityEngine.Vector3>(result);
    }

    // Binding generated from method: UnityEngine.Vector3 InverseTransformDirection(UnityEngine.Vector3)
    [dotnow.Interop.PreserveAttribute, dotnow.Interop.CLRMethodBindingAttribute(typeof(UnityEngine.Transform), "InverseTransformDirection", typeof(UnityEngine.Vector3))]
    public static void UnityEngine_Transform_InverseTransformDirection_Vector3(dotnow.Interop.StackContext context)
    {
        UnityEngine.Transform arg0 = context.ReadArgObject<UnityEngine.Transform>(0);
        UnityEngine.Vector3 arg1 = context.ReadArgValueType<UnityEngine.Vector3>(1);
        UnityEngine.Vector3 result = arg0.InverseTransformDirection(arg1);
        context.ReturnValueType<UnityEngine.Vector3>(result);
    }

    // Binding generated from method: UnityEngine.Vector3 InverseTransformDirection(Single, Single, Single)
    [dotnow.Interop.PreserveAttribute, dotnow.Interop.CLRMethodBindingAttribute(typeof(UnityEngine.Transform), "InverseTransformDirection", typeof(float), typeof(float), typeof(float))]
    public static void UnityEngine_Transform_InverseTransformDirection_Float_Float_Float(dotnow.Interop.StackContext context)
    {
        UnityEngine.Transform arg0 = context.ReadArgObject<UnityEngine.Transform>(0);
        float arg1 = context.ReadArgValueType<float>(1);
        float arg2 = context.ReadArgValueType<float>(2);
        float arg3 = context.ReadArgValueType<float>(3);
        UnityEngine.Vector3 result = arg0.InverseTransformDirection(arg1, arg2, arg3);
        context.ReturnValueType<UnityEngine.Vector3>(result);
    }

    // Binding generated from method: UnityEngine.Vector3 TransformVector(UnityEngine.Vector3)
    [dotnow.Interop.PreserveAttribute, dotnow.Interop.CLRMethodBindingAttribute(typeof(UnityEngine.Transform), "TransformVector", typeof(UnityEngine.Vector3))]
    public static void UnityEngine_Transform_TransformVector_Vector3(dotnow.Interop.StackContext context)
    {
        UnityEngine.Transform arg0 = context.ReadArgObject<UnityEngine.Transform>(0);
        UnityEngine.Vector3 arg1 = context.ReadArgValueType<UnityEngine.Vector3>(1);
        UnityEngine.Vector3 result = arg0.TransformVector(arg1);
        context.ReturnValueType<UnityEngine.Vector3>(result);
    }

    // Binding generated from method: UnityEngine.Vector3 TransformVector(Single, Single, Single)
    [dotnow.Interop.PreserveAttribute, dotnow.Interop.CLRMethodBindingAttribute(typeof(UnityEngine.Transform), "TransformVector", typeof(float), typeof(float), typeof(float))]
    public static void UnityEngine_Transform_TransformVector_Float_Float_Float(dotnow.Interop.StackContext context)
    {
        UnityEngine.Transform arg0 = context.ReadArgObject<UnityEngine.Transform>(0);
        float arg1 = context.ReadArgValueType<float>(1);
        float arg2 = context.ReadArgValueType<float>(2);
        float arg3 = context.ReadArgValueType<float>(3);
        UnityEngine.Vector3 result = arg0.TransformVector(arg1, arg2, arg3);
        context.ReturnValueType<UnityEngine.Vector3>(result);
    }

    // Binding generated from method: UnityEngine.Vector3 InverseTransformVector(UnityEngine.Vector3)
    [dotnow.Interop.PreserveAttribute, dotnow.Interop.CLRMethodBindingAttribute(typeof(UnityEngine.Transform), "InverseTransformVector", typeof(UnityEngine.Vector3))]
    public static void UnityEngine_Transform_InverseTransformVector_Vector3(dotnow.Interop.StackContext context)
    {
        UnityEngine.Transform arg0 = context.ReadArgObject<UnityEngine.Transform>(0);
        UnityEngine.Vector3 arg1 = context.ReadArgValueType<UnityEngine.Vector3>(1);
        UnityEngine.Vector3 result = arg0.InverseTransformVector(arg1);
        context.ReturnValueType<UnityEngine.Vector3>(result);
    }

    // Binding generated from method: UnityEngine.Vector3 InverseTransformVector(Single, Single, Single)
    [dotnow.Interop.PreserveAttribute, dotnow.Interop.CLRMethodBindingAttribute(typeof(UnityEngine.Transform), "InverseTransformVector", typeof(float), typeof(float), typeof(float))]
    public static void UnityEngine_Transform_InverseTransformVector_Float_Float_Float(dotnow.Interop.StackContext context)
    {
        UnityEngine.Transform arg0 = context.ReadArgObject<UnityEngine.Transform>(0);
        float arg1 = context.ReadArgValueType<float>(1);
        float arg2 = context.ReadArgValueType<float>(2);
        float arg3 = context.ReadArgValueType<float>(3);
        UnityEngine.Vector3 result = arg0.InverseTransformVector(arg1, arg2, arg3);
        context.ReturnValueType<UnityEngine.Vector3>(result);
    }

    // Binding generated from method: UnityEngine.Vector3 TransformPoint(UnityEngine.Vector3)
    [dotnow.Interop.PreserveAttribute, dotnow.Interop.CLRMethodBindingAttribute(typeof(UnityEngine.Transform), "TransformPoint", typeof(UnityEngine.Vector3))]
    public static void UnityEngine_Transform_TransformPoint_Vector3(dotnow.Interop.StackContext context)
    {
        UnityEngine.Transform arg0 = context.ReadArgObject<UnityEngine.Transform>(0);
        UnityEngine.Vector3 arg1 = context.ReadArgValueType<UnityEngine.Vector3>(1);
        UnityEngine.Vector3 result = arg0.TransformPoint(arg1);
        context.ReturnValueType<UnityEngine.Vector3>(result);
    }

    // Binding generated from method: UnityEngine.Vector3 TransformPoint(Single, Single, Single)
    [dotnow.Interop.PreserveAttribute, dotnow.Interop.CLRMethodBindingAttribute(typeof(UnityEngine.Transform), "TransformPoint", typeof(float), typeof(float), typeof(float))]
    public static void UnityEngine_Transform_TransformPoint_Float_Float_Float(dotnow.Interop.StackContext context)
    {
        UnityEngine.Transform arg0 = context.ReadArgObject<UnityEngine.Transform>(0);
        float arg1 = context.ReadArgValueType<float>(1);
        float arg2 = context.ReadArgValueType<float>(2);
        float arg3 = context.ReadArgValueType<float>(3);
        UnityEngine.Vector3 result = arg0.TransformPoint(arg1, arg2, arg3);
        context.ReturnValueType<UnityEngine.Vector3>(result);
    }

    // Binding generated from method: UnityEngine.Vector3 InverseTransformPoint(UnityEngine.Vector3)
    [dotnow.Interop.PreserveAttribute, dotnow.Interop.CLRMethodBindingAttribute(typeof(UnityEngine.Transform), "InverseTransformPoint", typeof(UnityEngine.Vector3))]
    public static void UnityEngine_Transform_InverseTransformPoint_Vector3(dotnow.Interop.StackContext context)
    {
        UnityEngine.Transform arg0 = context.ReadArgObject<UnityEngine.Transform>(0);
        UnityEngine.Vector3 arg1 = context.ReadArgValueType<UnityEngine.Vector3>(1);
        UnityEngine.Vector3 result = arg0.InverseTransformPoint(arg1);
        context.ReturnValueType<UnityEngine.Vector3>(result);
    }

    // Binding generated from method: UnityEngine.Vector3 InverseTransformPoint(Single, Single, Single)
    [dotnow.Interop.PreserveAttribute, dotnow.Interop.CLRMethodBindingAttribute(typeof(UnityEngine.Transform), "InverseTransformPoint", typeof(float), typeof(float), typeof(float))]
    public static void UnityEngine_Transform_InverseTransformPoint_Float_Float_Float(dotnow.Interop.StackContext context)
    {
        UnityEngine.Transform arg0 = context.ReadArgObject<UnityEngine.Transform>(0);
        float arg1 = context.ReadArgValueType<float>(1);
        float arg2 = context.ReadArgValueType<float>(2);
        float arg3 = context.ReadArgValueType<float>(3);
        UnityEngine.Vector3 result = arg0.InverseTransformPoint(arg1, arg2, arg3);
        context.ReturnValueType<UnityEngine.Vector3>(result);
    }

    // Binding generated from method: Void DetachChildren()
    [dotnow.Interop.PreserveAttribute, dotnow.Interop.CLRMethodBindingAttribute(typeof(UnityEngine.Transform), "DetachChildren")]
    public static void UnityEngine_Transform_DetachChildren(dotnow.Interop.StackContext context)
    {
        UnityEngine.Transform arg0 = context.ReadArgObject<UnityEngine.Transform>(0);
        arg0.DetachChildren();
    }

    // Binding generated from method: Void SetAsFirstSibling()
    [dotnow.Interop.PreserveAttribute, dotnow.Interop.CLRMethodBindingAttribute(typeof(UnityEngine.Transform), "SetAsFirstSibling")]
    public static void UnityEngine_Transform_SetAsFirstSibling(dotnow.Interop.StackContext context)
    {
        UnityEngine.Transform arg0 = context.ReadArgObject<UnityEngine.Transform>(0);
        arg0.SetAsFirstSibling();
    }

    // Binding generated from method: Void SetAsLastSibling()
    [dotnow.Interop.PreserveAttribute, dotnow.Interop.CLRMethodBindingAttribute(typeof(UnityEngine.Transform), "SetAsLastSibling")]
    public static void UnityEngine_Transform_SetAsLastSibling(dotnow.Interop.StackContext context)
    {
        UnityEngine.Transform arg0 = context.ReadArgObject<UnityEngine.Transform>(0);
        arg0.SetAsLastSibling();
    }

    // Binding generated from method: Void SetSiblingIndex(Int32)
    [dotnow.Interop.PreserveAttribute, dotnow.Interop.CLRMethodBindingAttribute(typeof(UnityEngine.Transform), "SetSiblingIndex", typeof(int))]
    public static void UnityEngine_Transform_SetSiblingIndex_Int(dotnow.Interop.StackContext context)
    {
        UnityEngine.Transform arg0 = context.ReadArgObject<UnityEngine.Transform>(0);
        int arg1 = context.ReadArgValueType<int>(1);
        arg0.SetSiblingIndex(arg1);
    }

    // Binding generated from method: Int32 GetSiblingIndex()
    [dotnow.Interop.PreserveAttribute, dotnow.Interop.CLRMethodBindingAttribute(typeof(UnityEngine.Transform), "GetSiblingIndex")]
    public static void UnityEngine_Transform_GetSiblingIndex(dotnow.Interop.StackContext context)
    {
        UnityEngine.Transform arg0 = context.ReadArgObject<UnityEngine.Transform>(0);
        int result = arg0.GetSiblingIndex();
        context.ReturnValueType<int>(result);
    }

    // Binding generated from method: UnityEngine.Transform Find(System.String)
    [dotnow.Interop.PreserveAttribute, dotnow.Interop.CLRMethodBindingAttribute(typeof(UnityEngine.Transform), "Find", typeof(string))]
    public static void UnityEngine_Transform_Find_String(dotnow.Interop.StackContext context)
    {
        UnityEngine.Transform arg0 = context.ReadArgObject<UnityEngine.Transform>(0);
        string arg1 = context.ReadArgObject<string>(1);
        UnityEngine.Transform result = arg0.Find(arg1);
        context.ReturnObject<UnityEngine.Transform>(result);
    }

    // Binding generated from method: Boolean IsChildOf(UnityEngine.Transform)
    [dotnow.Interop.PreserveAttribute, dotnow.Interop.CLRMethodBindingAttribute(typeof(UnityEngine.Transform), "IsChildOf", typeof(UnityEngine.Transform))]
    public static void UnityEngine_Transform_IsChildOf_Transform(dotnow.Interop.StackContext context)
    {
        UnityEngine.Transform arg0 = context.ReadArgObject<UnityEngine.Transform>(0);
        UnityEngine.Transform arg1 = context.ReadArgObject<UnityEngine.Transform>(1);
        bool result = arg0.IsChildOf(arg1);
        context.ReturnValueType<bool>(result);
    }

    // Binding generated from method: System.Collections.IEnumerator GetEnumerator()
    [dotnow.Interop.PreserveAttribute, dotnow.Interop.CLRMethodBindingAttribute(typeof(UnityEngine.Transform), "GetEnumerator")]
    public static void UnityEngine_Transform_GetEnumerator(dotnow.Interop.StackContext context)
    {
        UnityEngine.Transform arg0 = context.ReadArgObject<UnityEngine.Transform>(0);
        System.Collections.IEnumerator result = arg0.GetEnumerator();
        context.ReturnObject<System.Collections.IEnumerator>(result);
    }

    // Binding generated from method: UnityEngine.Transform GetChild(Int32)
    [dotnow.Interop.PreserveAttribute, dotnow.Interop.CLRMethodBindingAttribute(typeof(UnityEngine.Transform), "GetChild", typeof(int))]
    public static void UnityEngine_Transform_GetChild_Int(dotnow.Interop.StackContext context)
    {
        UnityEngine.Transform arg0 = context.ReadArgObject<UnityEngine.Transform>(0);
        int arg1 = context.ReadArgValueType<int>(1);
        UnityEngine.Transform result = arg0.GetChild(arg1);
        context.ReturnObject<UnityEngine.Transform>(result);
    }
}