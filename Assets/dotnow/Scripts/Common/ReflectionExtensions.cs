#if API_NET35
using System.Reflection;

namespace System
{
    public static class ReflectionExtensions
    {
        // Methods
        public static Type GetEnumUnderlyingType(this Type type)
        {
            if (type.IsEnum == false)
                throw new InvalidOperationException("Type must be an enum");

            FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            if (fields == null || fields.Length <= 0)
                throw new ArgumentException("Invalid enum type");

            return fields[0].FieldType;
        }

        public static T GetCustomAttribute<T>(this Type type) where T : Attribute
        {
            object[] attrib = type.GetCustomAttributes(typeof(T), false);

            if (attrib.Length > 0)
                return attrib[0] as T;

            return null;
        }

        public static T GetCustomAttribute<T>(this MethodBase method) where T : Attribute
        {
            object[] attrib = method.GetCustomAttributes(typeof(T), false);

            if (attrib.Length > 0)
                return attrib[0] as T;

            return null;
        }
    }
}
#endif