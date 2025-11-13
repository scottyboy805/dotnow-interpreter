using dotnow.Interop;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace dotnow.Runtime.CIL
{
    internal sealed class CILVTable
    {
        // Private
        private readonly Type virtualType;
        private Dictionary<CILMethodInfo, CILMethodInfo> virtualMethods;

        // Constructor
        public CILVTable(Type virtualType)
        {
            this.virtualType = virtualType;
        }

        // Methods
        public void GetVirtualInstanceMethod(AssemblyLoadContext assemblyLoadContext, ref CILMethodInfo virtualMethod)
        {
            // Check for abstract and virtual not present - no need for a virtual call in that case but the compiler does emit the instructions that way for the null check
            if ((virtualMethod.Flags & CILMethodFlags.Virtual) == 0 && (virtualMethod.Flags & CILMethodFlags.Abstract) == 0)
                return;

            // Check for cached - no need to go any further if the virtual method has been cached
            CILMethodInfo key = virtualMethod;
            if (virtualMethods != null && virtualMethods.TryGetValue(key, out virtualMethod) == true)
                return;

            // Get the binding flags since we will need to search for methods
            BindingFlags flags = GetVirtualBindingFlags(virtualMethod.Method.Attributes);

            // We need to resolve the virtual meta method by walking the hierarchy
            Type virtualInstanceType = virtualType;
            MethodBase virtualCall = null;

            // Get the temp parameters
            Type[] parameterTypes = __marshal.GetParameterTypeList(virtualMethod.ParameterTypes.Length);

            // Build type list
            for (int i = 0; i < parameterTypes.Length; i++)
                parameterTypes[i] = virtualMethod.ParameterTypes[i].Type;

            // Try to find target method on object and work down the hierarchy until we get a match
            while (virtualCall == null && virtualInstanceType != null)
            {
                // Try to get method
                MethodBase potentialVirtualMethod = virtualInstanceType.GetMethod(virtualMethod.Method.Name, flags, null, parameterTypes, null);

                // Check for found method
                if (potentialVirtualMethod != null)
                {
                    virtualCall = potentialVirtualMethod;
                    break;
                }

                // Move down the hierarchy
                virtualInstanceType = virtualInstanceType.BaseType;
            }

            // Check for found
            if (virtualCall == null)
                throw new MissingMethodException("Could not resolve virtual method call: " + virtualMethod.Method + " called as instance: " + virtualType);

            // Resolve the method handle - since this is a late call we need to manually init the method handle because it may not have been created until now
            virtualMethod = virtualCall.GetMethodInfo(assemblyLoadContext.AppDomain); //context.appDomain.GetOrCreateMethodHandle(context, virtualCall);

            // Create cache on demand
            if (virtualMethods == null)
                virtualMethods = new();

            // Add to cache
            virtualMethods[key] = virtualMethod;
        }

        private static BindingFlags GetVirtualBindingFlags(MethodAttributes attributes)
        {
            BindingFlags flags = 0;

            flags |= BindingFlags.Instance;
            flags |= BindingFlags.Public;

            // Apply visibility modifiers
            if ((attributes & MethodAttributes.Private) != 0) flags |= BindingFlags.NonPublic;
            if ((attributes & MethodAttributes.Public) != 0) flags |= BindingFlags.Public;
            if ((attributes & MethodAttributes.FamORAssem) != 0) flags |= BindingFlags.NonPublic;

            return flags;
        }
    }
}
