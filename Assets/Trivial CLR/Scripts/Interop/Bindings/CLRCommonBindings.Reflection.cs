using System;
using System.Reflection;

namespace TrivialCLR.Interop
{
    internal static partial class CLRCommonBindings
    {
        // Methods
        [CLRMethodBinding(typeof(Type), "GetTypeFromHandle", typeof(RuntimeTypeHandle))]
        public static object GetTypeFromHandleOverride(AppDomain domain, MethodInfo overrideMethod, object instance, object[] args)
        {
            // Check for type object
            if (args[0] is Type)
            {
                // CLR types already exist as type definitions on the stack
                return args[0];
            }

            // Must also support system types
            return Type.GetTypeFromHandle((RuntimeTypeHandle)args[0]);
        }

        [CLRMethodBinding(typeof(FieldInfo), "GetFieldFromHandle", typeof(RuntimeFieldHandle))]
        public static object GetFieldFromHandleOverride(AppDomain domain, MethodInfo overrideMethod, object instance, object[] args)
        {
            // Check for field object
            if (args[0] is FieldInfo)
            {
                // CLR fields already exist as field definitions on the stack
                return args[0];
            }

            // Must also support system fields
            return FieldInfo.GetFieldFromHandle((RuntimeFieldHandle)args[0]);
        }

        [CLRMethodBinding(typeof(MethodBase), "GetMethodFromHandle", typeof(RuntimeMethodHandle))]
        public static object GetMethodFromHandleOverride(AppDomain domain, MethodInfo overrideMethod, object instance, object[] args)
        {
            // Check for method object
            if (args[0] is MethodBase)
            {
                // CLR methods already exist as method definitions on the stack
                return args[0];
            }

            // Must also support system methods
            return MethodBase.GetMethodFromHandle((RuntimeMethodHandle)args[0]);
        }
    }
}
