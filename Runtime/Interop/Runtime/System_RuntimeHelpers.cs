using dotnow.Reflection;
using dotnow.Runtime;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace dotnow.Interop.Runtime
{
    internal static class System_RuntimeHelpers
    {
        // Methods
        [Preserve]
        [CLRMethodBinding(typeof(RuntimeHelpers), nameof(RuntimeHelpers.InitializeArray), typeof(Array), typeof(RuntimeFieldHandle))]
        public static unsafe void InitializeArray_Override(StackContext context)
        {
            // Read the array
            Array array = context.ReadArgObject<Array>(0);

            // Read the field handle - this method will only be called for interpreted fields
            CLRFieldInfo field = (CLRFieldInfo)context.ReadArgFieldHandle(1);

            // Get the element type
            Type elementType = array.GetType().GetElementType();

            // Get the initial data
            UnmanagedMemory<byte> initData = field.GetFieldInitData();

            // Get array length in bytes
            uint arrayLength = (uint)Buffer.ByteLength(array);

            // Check size
            if ((uint)initData.Size != arrayLength)
                throw new InvalidOperationException("Field init data is not of the expected size");

            // Pin the array memory
            GCHandle handle = GCHandle.Alloc(array, GCHandleType.Pinned);
            try
            {
                // Get array address
                void* dst = (void*)handle.AddrOfPinnedObject();

                // Copy from field init
                __gc.CopyMemory(initData.Ptr, dst, arrayLength);
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
