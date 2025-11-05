using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using dotnow.Runtime.CIL;

namespace dotnow.Runtime
{
    internal static class __internal
    {
        // Used to track allocations in profiling tools
#if !API_NET35
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        internal static void __gc_alloc_arrays(ref StackData stack, Type arrType, int length)
        {
            // Check for clr type - Must be stored as CLRInstance to avoid invalid cast exceptions
            if (arrType.IsCLRType() == true)
            {
                // Check for enum
                if (arrType.IsEnum == true)
                {
                    // Use enum underlying value
                    arrType = arrType.GetEnumUnderlyingType();
                }
                else
                {
                    // Use clr instance
                    arrType = typeof(CLRInstance);
                }
            }

            // Create the new instance
            Array instance = Array.CreateInstance(arrType, length);

            // Push to stack
            stack.Ref = instance;
            stack.Type = StackType.Ref;
        }

#if !API_NET35
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        internal static void __gc_alloc_arrayl(ref StackData stack, Type arrType, long length)
        {
            // Check for clr type - Must be stored as CLRInstance to avoid invalid cast exceptions
            if (arrType.IsCLRType() == true)
            {
                arrType = typeof(CLRInstance);
            }

            // Create the new instance
            Array instance = Array.CreateInstance(arrType, length);

            // Push to stack
            stack.Ref = instance;
            stack.Type = StackType.Ref;
        }

#if !API_NET35
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        internal static void __gc_alloc_inst(ref StackData stack, ref AppDomain domain, Type instType, MethodBase ctor, object[] args)
        {
            // Create the new instance
            object instance = domain.CreateInstance(instType, ctor, args);

            // Push to stack
            stack.Ref = instance;
            stack.Type = StackType.Ref;
        }

#if !API_NET35
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        internal static void __stack_alloc_inst(ref StackData stack, ref AppDomain domain, Type valueType, ref int stackPtr)
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
            stack.Ref = instance;
            stack.Type = StackType.Ref;

            // Increase stack ptr
            stackPtr += allocSize;
        }

#if API_NET35
        internal static void __gc_alloc_addr_fld(ref StackData stack, CILFieldAccess field, StackData inst)
#else
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void __gc_alloc_addr_fld(ref StackData stack, CILFieldAccess field, in StackData inst)
#endif
        {
            // Push to stack
            stack.Ref = new ByRefField(field, inst);
     stack.Type = StackType.ByRef;
        }

#if !API_NET35
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        internal static void __gc_alloc_addr_elem(ref StackData stack, Array arr, int index)
        {
            // Push to stack
     stack.Ref = new ByRefElement(arr, index);
      stack.Type = StackType.ByRef;
        }

#if !API_NET35
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        internal static void __gc_alloc_addr_elem(ref StackData stack, Array arr, long index)
        {
            // Push to stack
            stack.Ref = new ByRefElement(arr, index);
            stack.Type = StackType.ByRef;
        }

#if !API_NET35
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        internal static void __gc_alloc_addr_stack(ref StackData stack, StackData[] stackInst, int index)
        {
            // Push to stack
            stack.Ref = new ByRefVariable(stackInst, index);
  stack.Type = StackType.ByRef;
        }
    }
}
