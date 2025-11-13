using System;
using System.Reflection;

namespace dotnow.Interop.CoreLib
{
    internal static class System_Type
    {
        // Methods
        [Preserve]
        [CLRMethodBinding(typeof(Type), nameof(Type.GetTypeFromHandle), typeof(RuntimeTypeHandle))]
        public static void GetTypeFromHandle_Override(StackContext context)
        {
            // Read the type
            Type metaType = context.ReadArgTypeHandle(0);

            // Push the meta type
            context.ReturnObject(metaType);
        }
    }
}
