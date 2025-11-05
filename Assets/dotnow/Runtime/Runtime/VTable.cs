using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using dotnow.Runtime.CIL;

namespace dotnow.Runtime
{
    internal sealed class VTable
    {
        // Private
        private Dictionary<MethodBase, MethodBase> virtualMethods = null;

        // Methods
        public MethodBase GetVirtualMethodInvocation(MethodBase virtualInvocationTarget, Type runtimeType, CILSignature signature, MethodAttributes attrib)
        {
            // The method is not overridable so a virtual call is not possible
            if ((attrib & MethodAttributes.Abstract) == 0 && (attrib & MethodAttributes.Virtual) == 0)
                return virtualInvocationTarget;


            // Check for cached virtual call
            MethodBase cachedVirtualCall;

            if (virtualMethods != null && virtualMethods.TryGetValue(virtualInvocationTarget, out cachedVirtualCall) == true)
                return cachedVirtualCall;


            MethodBase originalCall = virtualInvocationTarget;
            MethodBase virtualCall = null;


            // Get binding flags
            // Get binding flags for target method
            BindingFlags flags = MethodAttributesToBindingFlags(attrib);

            // Try to find target method on object
            while (virtualCall == null && runtimeType != null)
            {
                // Try to get method
                MethodBase method = null;
                try
                {
                    method = runtimeType.GetMethod(originalCall.Name, flags, null, signature.parameterTypes, null);
                }
                catch (Exception e)
                {
                    // Get all potential methods
                    MethodInfo[] methods = runtimeType.GetMethods(flags);

                    // Find all closely matching methods
                    IEnumerable<MethodInfo> potentialMethods = methods.Where(m =>
                        m.Name == originalCall.Name && m.IsGenericMethod == originalCall.IsGenericMethod &&
                        m.GetParameters().Length == originalCall.GetParameters().Length &&
                        m.GetGenericArguments().Length == originalCall.GetGenericArguments().Length);

                    try
                    {
                        // Try to get only match
                        MethodInfo resolvedMethod = potentialMethods.SingleOrDefault();
                        method = resolvedMethod;
                        
                        // Make method generic
                        if (resolvedMethod != null && resolvedMethod.IsGenericMethod)
                            method = resolvedMethod.MakeGenericMethod(originalCall.GetGenericArguments());
                    }
                    catch
                    {
                        // Throw first exception
                        throw e;
                    }
                }

                // Check for found method
                if (method != null)
                    virtualCall = method;

                // Move down the hierarchy until we find a suitable method
                runtimeType = runtimeType.BaseType;
            }

            // Check for virtual method override
            if (virtualCall != null)
            {
                // Add to v table
                CacheVirtualMethodInvocation(virtualInvocationTarget, virtualCall);
                return virtualCall;
            }

            // Add to v table
            CacheVirtualMethodInvocation(virtualInvocationTarget, virtualInvocationTarget);
            return virtualInvocationTarget;
        }

        private void CacheVirtualMethodInvocation(MethodBase invocationTarget, MethodBase virtualCall)
        {
            // Create the lookup
            if (virtualMethods == null)
            {
                virtualMethods = new Dictionary<MethodBase, MethodBase>();
            }
            else
            {
                // Check for key
                if (virtualMethods.ContainsKey(invocationTarget) == true)
                    return;
            }

            // Cache the method
            virtualMethods.Add(invocationTarget, virtualCall);
        }

        private BindingFlags MethodAttributesToBindingFlags(MethodAttributes attributes)
        {
            BindingFlags flags = 0;

            flags |= BindingFlags.Instance;
            flags |= BindingFlags.DeclaredOnly;

            if ((attributes & MethodAttributes.Private) != 0) flags |= BindingFlags.NonPublic;
            if ((attributes & MethodAttributes.Public) != 0) flags |= BindingFlags.Public;
            if ((attributes & MethodAttributes.FamORAssem) != 0) flags |= BindingFlags.NonPublic;

            return flags;
        }
    }
}
