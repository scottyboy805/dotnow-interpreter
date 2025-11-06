using dotnow.Interop;
using dotnow.Runtime.CIL;
using System;
using System.Runtime.Serialization;

namespace dotnow.Runtime
{
    internal static class GC
    {
        // Methods
        public static void AllocateArrayS(AppDomain domain, CILTypeInfo elementType, int length, ref StackData dst)
        {
            // Get the explicit element type
            Type explicitType = elementType.Type;

            // Check for CLR type
            if((elementType.Flags & CILTypeFlags.Interpreted) != 0)
            {
                // Check for enum
                if((elementType.Flags & CILTypeFlags.Enum) != 0)
                {
                    // Use underlying type
                    explicitType = elementType.Type.GetEnumUnderlyingType();
                }
                else
                {
                    // Create array of ICLRInstance
                    explicitType = typeof(ICLRInstance);
                }
            }

            // Allocate the array
            Array array = Array.CreateInstance(explicitType, length);

            // Push to stack
            dst.Ref = array;
            dst.Type = StackType.Ref;
        }

        public static void AllocateArrayL(AppDomain domain, CILTypeInfo elementType, long length, ref StackData dst)
        {
            // Get the explicit element type
            Type explicitType = elementType.Type;

            // Check for CLR type
            if ((elementType.Flags & CILTypeFlags.Interpreted) != 0)
            {
                // Check for enum
                if ((elementType.Flags & CILTypeFlags.Enum) != 0)
                {
                    // Use underlying type
                    explicitType = elementType.Type.GetEnumUnderlyingType();
                }
                else
                {
                    // Create array of ICLRInstance
                    explicitType = typeof(ICLRInstance);
                }
            }

            // Allocate the array
            Array array = Array.CreateInstance(explicitType, length);

            // Push to stack
            dst.Ref = array;
            dst.Type = StackType.Ref;
        }

        public static void AllocateObject(AppDomain domain, CILTypeInfo type, ref StackData dst)
        {
            // Check for interop
            if((type.Flags & CILTypeFlags.Interop) != 0)
            {
                // Check for array
                if((type.Flags & CILTypeFlags.Array) != 0)
                    throw new NotSupportedException("Cannot get uninitialized array object");

                // Create instance
                dst.Ref = FormatterServices.GetUninitializedObject(type.Type);
                dst.Type = StackType.Ref;
            }
            // Must be interpreted
            else
            {
                // Check for enum
                if ((type.Flags & CILTypeFlags.Enum) != 0)
                {
                    // Create underlying type
                    throw new NotImplementedException();
                }
                else
                {
                    // Create the instance
                    dst.Ref = CLRTypeInstance.CreateInstance(domain, type);
                    dst.Type = StackType.Ref;
                }
            }
        }
    }
}
