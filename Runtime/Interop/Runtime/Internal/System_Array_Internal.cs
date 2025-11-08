using dotnow.Runtime;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

namespace dotnow.Interop.Runtime.Internal
{
    internal static class System_Array_Internal
    {
        // Private
        // A lookup table for object array that can be reused for interop call args per thread
        private static readonly ThreadLocal<Dictionary<int, long[]>> indexListCache = new(() => new());

        // Methods
        public static void MultiArrayCreateInstance_Internal(StackContext context, Type arrayType)
        {
            // Get array rank
            int rank = arrayType.GetArrayRank();

            // Create array
            long[] lengths = new long[rank];

            // Read lengths
            for(int i = 0; i < rank; i++)
            {
                // Get the length as long or int
                lengths[i] = context.ReadArgStackType(i) == StackType.I64
                    ? context.ReadArgValueType<long>(i)
                    : context.ReadArgValueType<int>(i);
            }

            // Get element type
            Type elementType = GetElementType(arrayType);

            // Create the instance
            Array instance = Array.CreateInstance(elementType, lengths);

            // Return the objects
            context.ReturnObject(instance);
        }

        public static void MultiArrayGet_Internal(StackContext context)
        {
            // Get the array
            Array array = context.ReadArgObject<Array>(0);

            // Get index list
            long[] indexes = GetIndexList(array.Rank);

            // Read all indexes
            for (int i = 0; i < indexes.Length; i++)
            {
                indexes[i] = context.ReadArgStackType(i + 1) == StackType.I64
                    ? context.ReadArgValueType<long>(i + 1)
                    : context.ReadArgValueType<int>(i + 1);
            }

            // Read the value - Can't avoid boxing here I think
            object arrayVal = array.GetValue(indexes);

            // Get element type
            Type elementType = GetElementType(array.GetType());

            // Get the value
            context.ReturnAny(elementType, arrayVal);
        }

        public static void MultiArraySet_Internal(StackContext context)
        {
            // Get the array
            Array array = context.ReadArgObject<Array>(0);

            // Get index list
            long[] indexes = GetIndexList(array.Rank);

            // Read all indexes
            for (int i = 0; i < indexes.Length; i++)
            {
                indexes[i] = context.ReadArgStackType(i + 1) == StackType.I64
                    ? context.ReadArgValueType<long>(i + 1)
                    : context.ReadArgValueType<int>(i + 1);
            }

            // Get element type
            Type elementType = GetElementType(array.GetType());

            // Get the value
            object arrayVal = context.ReadArgAny(elementType, indexes.Length + 1);

            // Read the value - Can't avoid boxing here I think
            array.SetValue(arrayVal, indexes);
        }

        private static Type GetElementType(Type arrayType)
        {
            // Get element type
            Type elementType = arrayType.GetElementType();

            // Check for enum
            if (elementType.IsEnum == true)
                elementType = elementType.GetEnumUnderlyingType();

            return elementType;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static long[] GetIndexList(int indexCount)
        {
            // Check for 0
            if (indexCount <= 0)
                return null;

            // Check for cached
            long[] indexList;
            if (indexListCache.Value.TryGetValue(indexCount, out indexList) == true)
                return indexList;

            // Create new instance
            indexList = new long[indexCount];

            // Add to cache
            indexListCache.Value[indexCount] = indexList;
            return indexList;
        }
    }
}
