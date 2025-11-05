using dotnow.Runtime;
using System;

namespace dotnow.Interop.Runtime.Internal
{
    internal static class System_Array_Internal
    {
        // Methods
        public static void MultiArrayCreateInstance_Internal(StackContext context, Type instanceType)
        {
            // Get array rank
            int rank = instanceType.GetArrayRank();

            // Create array
            long[] lengths = new long[rank];

            // Read lengths
            for(int i = 0; i < rank; i++)
            {
                // Get the length as long or int
                lengths[i] = context.ReadArgStackType(i) == StackTypeCode.I64
                    ? context.ReadArgValueType<long>(i)
                    : context.ReadArgValueType<int>(i);
            }

            // Get element type
            Type elementType = instanceType.GetElementType();

            // Check for enum
            if(elementType.IsEnum == true)
                elementType = elementType.GetEnumUnderlyingType();

            // Create the instance
            Array instance = Array.CreateInstance(elementType, lengths);

            // Return the objects
            context.ReturnObject(instance);
        }

        public static void MultiArrayGet_Internal(StackContext context)
        {

        }

        public static void MultiArraySet_Internal(StackContext context)
        {

        }
    }
}
