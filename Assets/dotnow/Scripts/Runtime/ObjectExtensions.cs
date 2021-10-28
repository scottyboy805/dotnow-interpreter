using System;

namespace dotnow
{
    public static class ObjectExtensions
    {
        // Methods
        public static bool IsCLRInstance(this object instance)
        {
            return instance is CLRInstance;
        }

        public static Type GetInterpretedType(this object instance)
        {
            if (instance == null)
                throw new NullReferenceException();

            if (instance is CLRInstance)
                return ((CLRInstance)instance).Type;

            return instance.GetType();
        }

        public static object Unwrap(this object instance)
        {
            // Check for valid clr instance
            if(instance != null && instance is CLRInstance)
            {
                return ((CLRInstance)instance).Unwrap();
            }
            return instance;
        }

        public static object UnwrapAs<T>(this object instance) where T : class
        {
            // Check for valid clr instance
            if (instance != null && instance is CLRInstance)
            {
                return ((CLRInstance)instance).UnwrapAs(typeof(T));
            }
            return instance;
        }

        public static object UnwrapAs(this object instance, Type unwrapType)
        {
            if (instance != null && instance is CLRInstance)
            {
                return ((CLRInstance)instance).UnwrapAs(unwrapType);
            }
            return instance;
        }
    }
}
