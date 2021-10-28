using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace dotnow.Runtime
{
    internal static class __internal
    {
        // Used to track allocations in profiling tools
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void __gc_alloc_arrays(__heapallocator _heap, ref StackData stack, Type arrType, int length)
        {
            // Create the new instance
            Array instance = Array.CreateInstance(arrType, length);

            // Push to stack
            _heap.PinManagedObject(ref stack, instance);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void __gc_alloc_arrayl(__heapallocator _heap, ref StackData stack, Type arrType, long length)
        {
            // Create the new instance
            Array instance = Array.CreateInstance(arrType, length);

            // Push to stack
            _heap.PinManagedObject(ref stack, instance);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void __gc_alloc_inst(__heapallocator _heap, ref StackData stack, ref AppDomain domain, Type instType, MethodBase ctor, object[] args)
        {
            // Create the new instance
            object instance = domain.CreateInstance(instType, ctor, args);

            // Push to stack
            _heap.PinManagedObject(ref stack, instance);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void __stack_alloc_inst(__heapallocator _heap, ref StackData stack, ref AppDomain domain, Type valueType, ref int stackPtr)
        {
            // Get clr type
            CLRType clrType = valueType.GetCLRType();

            if (clrType == null)
                throw new InvalidOperationException("Stack allocation is only supported for interpreted types");

            // Get size of instance
            int allocSize = clrType.SizeOfInstance();

            // Create instance at stack ptr
            CLRInstance instance = CLRInstance.CreateAllocatedInstance(domain, clrType);
            instance.fieldPtr = stackPtr;

            // Push to stack
            _heap.PinManagedObject(ref stack, instance);

            // Increase stack ptr
            stackPtr += allocSize;
        }
    }
}
