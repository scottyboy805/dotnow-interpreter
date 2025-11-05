using dotnow;
using System.Collections.Generic;
using System.Reflection;

namespace System
{
    public static class ReflectionExtensions
    {
        // Methods
#if API_NET35
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
#endif

        #region GetCustomAttributes(T)
        public static IEnumerable<T> GetCustomAttributesInterpreted<T>(this Assembly element) where T : Attribute
        {
            return GetCustomAttributesOfType<T>(element.GetCustomAttributes(typeof(T)));
        }

        public static IEnumerable<T> GetCustomAttributesInterpreted<T>(this Module element) where T : Attribute
        {
            return GetCustomAttributesOfType<T>(element.GetCustomAttributes(typeof(T)));
        }

        public static IEnumerable<T> GetCustomAttributesInterpreted<T>(this MemberInfo element) where T : Attribute
        {
            return GetCustomAttributesOfType<T>(element.GetCustomAttributes(typeof(T)));
        }

        public static IEnumerable<T> GetCustomAttributesInterpreted<T>(this ParameterInfo element) where T : Attribute
        {
            return GetCustomAttributesOfType<T>(element.GetCustomAttributes(typeof(T)));
        }
        #endregion

        #region GetCustomAttribute(T)
        public static T GetCustomAttributeInterpreted<T>(this Assembly element) where T : Attribute
        {
            return GetCustomAttributeOfType<T>(element.GetCustomAttributes(typeof(T)));
        }

        public static T GetCustomAttributeInterpreted<T>(this Module element) where T : Attribute
        {
            return GetCustomAttributeOfType<T>(element.GetCustomAttributes(typeof(T)));
        }

        public static T GetCustomAttributeInterpreted<T>(this MemberInfo element) where T : Attribute
        {
            return GetCustomAttributeOfType<T>(element.GetCustomAttributes(typeof(T)));
        }

        public static T GetCustomAttributeInterpreted<T>(this ParameterInfo element) where T : Attribute
        {
            return GetCustomAttributeOfType<T>(element.GetCustomAttributes(typeof(T)));
        }
        #endregion


        private static IEnumerable<T> GetCustomAttributesOfType<T>(IEnumerable<Attribute> customAttributes) where T : Attribute
        {
            // Find all attributes
            IEnumerable<Attribute> attributes = customAttributes;

            // Check for matched type
            foreach (Attribute attribute in attributes)
            {
                // Try to convert
                if (attribute.GetInterpretedType() == typeof(T))
                    yield return attribute as T;
            }
        }

        private static T GetCustomAttributeOfType<T>(IEnumerable<Attribute> customAttributes) where T : Attribute
        {
            // Find all attributes
            IEnumerable<Attribute> attributes = customAttributes;

            // Check for matched type
            foreach (Attribute attribute in attributes)
            {
                // Try to convert
                if (attribute.GetInterpretedType() == typeof(T))
                    return attribute as T;
            }
            return null;
        }
    }
}
