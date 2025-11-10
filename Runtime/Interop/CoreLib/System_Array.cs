using System;
using System.Collections;

namespace dotnow.Interop.CoreLib
{
    internal static class System_Array
    {
        // Methods
        #region Properties
        [Preserve]
        [CLRMethodBinding(typeof(Array), "get_IsFixedSize")]
        public static void GetIsFixedSize_Override(StackContext context)
        {
            context.ReturnValueType(context.ReadArgObject<Array>(0)
                .IsFixedSize);
        }

        [Preserve]
        [CLRMethodBinding(typeof(Array), "get_IsReadOnly")]
        public static void GetIsReadOnly_Override(StackContext context)
        {
            context.ReturnValueType(context.ReadArgObject<Array>(0)
                .IsReadOnly);
        }

        [Preserve]
        [CLRMethodBinding(typeof(Array), "get_IsSynchronized")]
        public static void GetIsSynchronized_Override(StackContext context)
        {
            context.ReturnValueType(context.ReadArgObject<Array>(0)
                .IsSynchronized);
        }

        [Preserve]
        [CLRMethodBinding(typeof(Array), "get_Length")]
        public static void GetLength_Override(StackContext context)
        {
            context.ReturnValueType(context.ReadArgObject<Array>(0)
                .Length);
        }

        [Preserve]
        [CLRMethodBinding(typeof(Array), "get_LongLength")]
        public static void GetLongLength_Override(StackContext context)
        {
            context.ReturnValueType(context.ReadArgObject<Array>(0)
                .LongLength);
        }

        [Preserve]
        [CLRMethodBinding(typeof(Array), "get_Rank")]
        public static void GetRank_Override(StackContext context)
        {
            context.ReturnValueType(context.ReadArgObject<Array>(0)
                .Rank);
        }
        #endregion

        #region InstanceMethods
        [Preserve]
        [CLRMethodBinding(typeof(Array), nameof(Array.GetEnumerator))]
        public static void GetEnumerator_Override(StackContext context)
        {
            context.ReturnObject(context.ReadArgObject<Array>(0)
                .GetEnumerator());
        }
        #endregion

        #region StaticMethods
        [Preserve]
        [CLRMethodBinding(typeof(Array), nameof(Array.BinarySearch), typeof(Array), typeof(object))]
        public static void BinarySearch_O_Override(StackContext context)
        {
            context.ReturnValueType(Array.BinarySearch(
                context.ReadArgObject<Array>(0),
                context.ReadArgObject<object>(1)));
        }

        [Preserve]
        [CLRMethodBinding(typeof(Array), nameof(Array.BinarySearch), typeof(Array), typeof(int), typeof(int), typeof(object))]
        public static void BinarySearch_IIO_Override(StackContext context)
        {
            context.ReturnValueType(Array.BinarySearch(
                context.ReadArgObject<Array>(0),
                context.ReadArgValueType<int>(1),
                context.ReadArgValueType<int>(2),
                context.ReadArgObject<object>(3)));
        }

        [Preserve]
        [CLRMethodBinding(typeof(Array), nameof(Array.BinarySearch), typeof(Array), typeof(object), typeof(IComparer))]
        public static void BinarySearch_OC_Override(StackContext context)
        {
            context.ReturnValueType(Array.BinarySearch(
                context.ReadArgObject<Array>(0),
                context.ReadArgObject<object>(1),
                context.ReadArgObject<IComparer>(2)));
        }

        [Preserve]
        [CLRMethodBinding(typeof(Array), nameof(Array.BinarySearch), typeof(Array), typeof(int), typeof(int), typeof(object), typeof(IComparer))]
        public static void BinarySearch_IIOC_Override(StackContext context)
        {
            context.ReturnValueType(Array.BinarySearch(
                context.ReadArgObject<Array>(0),
                context.ReadArgValueType<int>(1),
                context.ReadArgValueType<int>(2),
                context.ReadArgObject<object>(3),
                context.ReadArgObject<IComparer>(4)));
        }

        [Preserve]
        [CLRMethodBinding(typeof(Array), nameof(Array.Clear), typeof(Array), typeof(int), typeof(int))]
        public static void Clear_Override(StackContext context)
        {
            Array.Clear(
                context.ReadArgObject<Array>(0),
                context.ReadArgValueType<int>(1),
                context.ReadArgValueType<int>(2));
        }
        #endregion
    }
}
