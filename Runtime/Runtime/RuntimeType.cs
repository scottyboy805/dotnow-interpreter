using System;
using System.Collections.Generic;

namespace dotnow.Runtime
{
    internal static class RuntimeType
    {
        // Methods
        internal static bool IsAssignable(Type dst, Type src)
        {
            // Check for direct match
            if (dst == src)
                return true;

            // Can't use IsAssignable check on clr types - causes hard crash in some cases
            if (dst.IsCLRType() == true)
            {
                // Check for sub class - need further work to support interfaces??
                if (src.IsSubclassOf(dst) == true)
                    return true;
            }
            else
            {
                // Check for assignable
                if (dst.IsAssignableFrom(src) == true)
                    return true;
            }

            // Handle arrays
            if (dst.IsArray == true && src.IsArray == true &&
                dst.GetArrayRank() == src.GetArrayRank() &&
                IsReferenceAssignable(dst.GetElementType(), src.GetElementType()) == true)
            {
                return true;
            }

            // Handle generics arrays
            if (src.IsArray == true && dst.IsGenericType == true &&
                (dst.GetGenericTypeDefinition() == typeof(IEnumerable<>) ||
                dst.GetGenericTypeDefinition() == typeof(IList<>) ||
                dst.GetGenericTypeDefinition() == typeof(ICollection<>)) &&
                dst.GetGenericArguments()[0] == src.GetElementType())
            {
                return true;
            }

            // Not convertible
            return false;
        }

        private static bool IsReferenceAssignable(Type dst, Type src)
        {
            // Check for direct match
            if (dst == src)
                return true;

            // Check assignable
            if (dst.IsValueType == false && src.IsValueType == false && IsAssignable(dst, src) == true)
                return true;

            // Not convertible
            return false;
        }
    }
}
