using System;
using System.Reflection;
using System.Runtime.InteropServices;
using dotnow.Reflection;

namespace dotnow.Interop
{
    internal static partial class CLRCommonBindings
    {
        // Methods
        [CLRMethodBinding(typeof(System.Runtime.CompilerServices.RuntimeHelpers), "InitializeArray", typeof(Array), typeof(RuntimeFieldHandle))]
        public static void InitializeArrayOverride(AppDomain domain, MethodInfo overrideMethod, object instance, object[] args)
        {
            // Get correct arguments
            Array array = (Array)args[0];
            CLRField field = (CLRField)args[1];

            // Get the initial data
            byte[] data = field.InitialData;

            // Get the element type
            Type elementType = array.GetType().GetElementType();

#if UNSAFE
            unsafe
            {
                fixed (byte* pointer = data)
                {
                    byte* localPointer = pointer;

                    for (int i = 0; i < array.Length; i++)
                    {
                        // Get the object value
                        array.SetValue(Marshal.PtrToStructure((IntPtr)localPointer, elementType), i);

                        // Increase pointer
                        localPointer += Marshal.SizeOf(elementType);
                    }
                }
            }
#else
            // Allocate fixed data
            GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);

            // Get address of data
            IntPtr basePtr = handle.AddrOfPinnedObject();
            int localPtr = 0;

            // Process all elements
            for(int i = 0; i < array.Length; i++)
            {
                // Calcualte pointer for element data
                IntPtr ptr = (IntPtr)((int)basePtr + localPtr);

                // Set array value
                array.SetValue(Marshal.PtrToStructure(ptr, elementType), i);

                // Increase pointer offset
                localPtr += Marshal.SizeOf(elementType);
            }

            // Release data
            handle.Free();
#endif            
        }

        
    }
}
