using dotnow.Reflection;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace dotnow.Interop.Runtime
{
    internal static class System_RuntimeHelpers
    {
        // Methods
        [Preserve]
        [CLRMethodBinding(typeof(RuntimeHelpers), nameof(RuntimeHelpers.InitializeArray), typeof(Array), typeof(RuntimeFieldHandle))]
        public static void InitializeArray_Override(StackContext context)
        {
            // Read the array
            Array array = context.ReadArgObject<Array>(0);

            // Read the field handle - this method will only be called for interpreted fields
            CLRFieldInfo field = (CLRFieldInfo)context.ReadArgFieldHandle(1);

            // Get the element type
            Type elementType = array.GetType().GetElementType();

            // Get the initial data
            byte[] initData = field.GetFieldInitData();

            // Pin the array memory
            GCHandle handle = GCHandle.Alloc(array, GCHandleType.Pinned);
            try
            {
                // Get array address
                IntPtr dst = handle.AddrOfPinnedObject();

                // Copy the memory
                Marshal.Copy(initData, 0, dst, initData.Length);
            }
            finally
            {
                // Release pinned memory
                handle.Free();
            }
        }

        [Preserve]
        [CLRMethodBinding(typeof(RuntimeHelpers), nameof(RuntimeHelpers.GetUninitializedObject), typeof(Type))]
        public static void GetUninitializedObject_Override(StackContext context)
        {
            // Get type
            Type type = context.ReadArgObject<Type>(0);

            // Get return object
            object result = null;

            // Check for clr type
            if(type is CLRType clrType)
            {
                // Create default instance
                result = context.AppDomain.CreateUninitializedInstance(clrType);
            }
            // Must be interop
            else
            {
                // Get default object
                result = RuntimeHelpers.GetUninitializedObject(type);
            }

            // Return object
            context.ReturnObject(result);
        }
    }
}
