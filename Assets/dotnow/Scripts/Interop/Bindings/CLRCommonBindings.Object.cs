using System;
using System.Reflection;

namespace dotnow.Interop
{
    internal static partial class CLRCommonBindings
    {
        // Methods
        [CLRMethodBinding(typeof(object), nameof(Object.ToString))]
        public static object ToStringOverride(AppDomain domain, MethodInfo overrideMethod, object instance, object[] args)
        {
            // Use default to string for standard objects
            if (instance.IsCLRInstance() == false)
                return instance.ToString();

            // Get the clr type
            CLRType instanceType = (instance as CLRInstance).Type;

            // Check for cached tostring method
            if(instanceType.cachedToStringTarget != null)
                return instanceType.cachedToStringTarget.Invoke(instance, null);

            // Fallback to standard behaviour - type name
            return instanceType.ToString();
        }

        [CLRMethodBinding(typeof(object), nameof(Object.Equals), new Type[] { typeof(object) })]
        public static object EqualsOverride(AppDomain domain, MethodInfo overrideMethod, object instance, object[] args)
        {
            // Use default equals for standard objects
            if (instance.IsCLRInstance() == false)
                return instance.Equals(args[0]);

            // Get the clr type
            CLRType instanceType = (instance as CLRInstance).Type;

            // Check for cached equals method
            if (instanceType.cachedEqualsTarget != null)
                return instanceType.cachedEqualsTarget.Invoke(instance, args);

            // Fallback to standard behaviour
            return (instance as CLRInstance).Equals(args[0]);
        }

        [CLRMethodBinding(typeof(object), nameof(Object.GetHashCode))]
        public static object GetHashCodeOverride(AppDomain domain, MethodInfo overrideMethod, object instance, object[] args)
        {
            // Use default gethashcode for standard objects
            if (instance.IsCLRInstance() == false)
                return instance.GetHashCode();

            // Get the clr type
            CLRType instanceType = (instance as CLRInstance).Type;

            // Check for cached gethashcode method
            if (instanceType.cachedGetHashCodeTarget != null)
                return instanceType.cachedGetHashCodeTarget.Invoke(instanceType, null);

            // Fallback to standard behaviour
            return (instance as CLRInstance).GetHashCode();
        }
    }
}
