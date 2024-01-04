using System;
using dotnow.Runtime;

namespace dotnow
{
    public static class ObjectExtensions
    {
        // Methods
        public static bool IsCLRInstance(this object instance)
        {
            return instance is CLRInstanceOld;
        }

        public static bool IsCLRInstanceOrByRefInstance(this object instance)
        {
            if (instance is IByRef)
                return true;

            return instance is CLRInstanceOld;
        }

        public static Type GetInterpretedType(this object instance)
        {
            if (instance == null) 
                throw new ArgumentNullException("instance");

            // Get clr type for instance
            if (instance is CLRInstanceOld)
                return ((CLRInstanceOld)instance).Type;

            return instance.GetType();
        }

        public static CLRType GetCLRInterpretedType(this object instance, bool throwOnError = true)
        {
            if (instance == null)
                throw new ArgumentNullException("instance");

            // Get clr type for instance
            if (instance is CLRInstanceOld)
                return ((CLRInstanceOld)instance).Type;

            // check for exception
            if (throwOnError == true)
                throw new InvalidOperationException("Specified instance is not a clr type");

            return null;
        }

        public static object Unwrap(this object instance)
        {
            // Check for valid clr instance
            if(instance != null && instance is CLRInstanceOld)
            {
                return ((CLRInstanceOld)instance).Unwrap();
            }
            return instance;
        }

        public static T UnwrapAs<T>(this object instance) where T : class
        {
            // Check for valid clr instance
            if (instance != null && instance is CLRInstanceOld)
            {
                return ((CLRInstanceOld)instance).UnwrapAs(typeof(T)) as T;
            }
            return instance as T;
        }

        public static object UnwrapAs(this object instance, Type unwrapType)
        {
            if (instance != null && instance is CLRInstanceOld)
            {
                return ((CLRInstanceOld)instance).UnwrapAs(unwrapType);
            }
            return instance;
        }
    }
}
