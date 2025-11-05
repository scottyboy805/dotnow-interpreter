using dotnow.Runtime;
using System;

namespace dotnow.Interop.Runtime.Proxy
{
    public abstract class System_UnmanagedArray_1_Proxy<T> : ICLRArrayProxy where T : unmanaged
    {
        // Private
        private T[] array;

        // Get the array object
        public Array Instance => array;

        // Methods
        public void Initialize(AppDomain appDomain, Type elementType, Array instance)
        {
            // Get instance and shape
            this.array = (T[])instance;
        }

        public override string ToString()
        {
            return $"{nameof(ICLRArrayProxy)}({array})";
        }

        public void GetValueDirect(StackContext context)
        {
            // Read index as either 64 or 32 bit
            long index = context.ReadArgStackType(0) == StackTypeCode.I64
                ? context.ReadArgValueType<long>(0)
                : context.ReadArgValueType<int>(0);

            T element = array[index];

            // Return to stack as unmanaged value
            context.ReturnValueType(element);
        }

        public void SetValueDirect(StackContext context)
        {
            // Read index as either 64 or 32 bit
            long index = context.ReadArgStackType(0) == StackTypeCode.I64
                ? context.ReadArgValueType<long>(0)
                : context.ReadArgValueType<int>(0);

            // Get the value
            T value = context.ReadArgValueType<T>(1);

            // Set the element
            array[index] = value;
        }
    }

    //public abstract class System_UnmanagedArray_2_Proxy<T> : ICLRArrayProxy where T : unmanaged
    //{
    //    // Private
    //    private T[,] array;

    //    // Get the array object
    //    public Array Instance => array;

    //    // Methods
    //    public void Initialize(AppDomain appDomain, Array instance)
    //    {
    //        // Get instance and shape
    //        this.array = (T[,])instance;
    //    }

    //    public void GetValueDirect(ArrayIndex index, StackContext context)
    //    {
    //        T element = array[index.Index_0, index.Index_1];

    //        // Return to stack as unmanaged value
    //        context.ReturnValueType(element);
    //    }

    //    public void SetValueDirect(ArrayIndex index, StackContext context)
    //    {
    //        // Get the value
    //        T value = context.ReadValueType<T>(0);

    //        // Set the element
    //        array[index.Index_0, index.Index_1] = value;
    //    }
    //}

    //public abstract class System_UnmanagedArray_3_Proxy<T> : ICLRArrayProxy where T : unmanaged
    //{
    //    // Private
    //    private T[,,] array;

    //    // Get the array object
    //    public Array Instance => array;

    //    // Methods
    //    public void Initialize(AppDomain appDomain, Array instance)
    //    {
    //        // Get instance and shape
    //        this.array = (T[,,])instance;
    //    }

    //    public void GetValueDirect(ArrayIndex index, StackContext context)
    //    {
    //        T element = array[index.Index_0, index.Index_1, index.Index_2];

    //        // Return to stack as unmanaged value
    //        context.ReturnValueType(element);
    //    }

    //    public void SetValueDirect(ArrayIndex index, StackContext context)
    //    {
    //        // Get the value
    //        T value = context.ReadValueType<T>(0);

    //        // Set the element
    //        array[index.Index_0, index.Index_1, index.Index_2] = value;
    //    }
    //}
}
