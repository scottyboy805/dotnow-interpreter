using dotnow.Interop;
using dotnow.Reflection;
using System;
using System.Linq;
using System.Reflection;

namespace dotnow
{
    public static class CLRExtensions
    {
        // Methods
        public static object Unwrap(this object obj)
        {
            // Check for CLR instance
            if (obj is ICLRInstance clrInstance)
                return clrInstance.Unwrap();

            return obj;
        }

        public static object UnwrapAsType(this object obj, Type asType)
        {
            // Check for CLR instance
            if (obj is ICLRInstance clrInstance)
                return clrInstance.UnwrapAsType(asType);

            return obj;
        }

        public static T UnwrapAsType<T>(this object obj)
        {
            // Check for CLR instance
            if (obj is ICLRInstance clrInstance)
            {
                // Try to unwrap
                object unwrapped = clrInstance.UnwrapAsType(typeof(T));

                // Get as T
                return unwrapped != null
                    ? (T)unwrapped
                    : default;
            }

            // Try to cast
            return obj is T
                ? (T)obj
                : default;
        }

        public static Type GetInterpretedType(this object instance)
        {
            // Check for instance
            if (instance == null)
                throw new ArgumentNullException(nameof(instance));

            // Check for Clr type
            if (instance is ICLRInstance inst)
                return inst.GetInterpretedType();

            // Check for proxy
            if (instance is ICLRProxy proxy)
                return proxy.Instance.GetInterpretedType();

            // Use interop type
            return instance.GetType();
        }

        public static bool IsCLRType(this Type type)
        {
            // Check for CLR
            if (type is CLRType)
                return true;

            return false;
        }

        public static bool IsCLRField(this FieldInfo field)
        {
            // Check for CLR
            if (field is CLRFieldInfo)
                return true;

            return false;
        }

        public static bool IsCLRMethod(this MethodBase method)
        {
            // Check for CLR
            if (method is CLRMethodInfo or CLRConstructorInfo)
                return true;

            return false;
        }

        public static MethodInfo MakeCLRGeneric(this MethodInfo method, Type[] genericTypes)
        {
            // Check for clr
            // If the method is a clr method or any of the generic types are a clr type, then we need to use CLRGenericMethodInfo.
            // Even if the method is an interop method, it is not possible to construct an interop generic with a clr type, so this solution works, but the method will not be executable without a direct call binding.
            bool makeClrGeneric = method.IsCLRMethod() == true
                || genericTypes.Any(t => t.IsCLRType() == true);

            // Check for clr
            if (makeClrGeneric == true)
                return new CLRGenericMethodInfo(method, genericTypes);

            // Must be interop
            return method.MakeGenericMethod(genericTypes);
        }
    }
}
