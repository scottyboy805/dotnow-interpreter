using dotnow.Reflection;
using dotnow.Runtime.Types;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace dotnow
{
    public static class TypeExtensions
    {
        // Private
        private static Dictionary<Type, bool> virtualMemberCache = new Dictionary<Type, bool>();

        // Methods
        public static bool IsCLRType(this Type type)
        {
            return type is CLRType;
        }

        public static CLRType GetCLRType(this Type type)
        {
            return type as CLRType;
        }

        public static bool IsCLRMember(this MemberInfo member)
        {
            return member is CLRField ||
                member is CLRProperty ||
                member is CLRMethod ||
                member is CLRConstructor ||
                member is CLRType;
        }

        public static Type GetInteropBaseType(this Type type)
        {
            // Only applicable to clr types
            if(type is CLRType)
            {
                Type currentBase = type;

                // Move down the hierarchy until we reach interop type
                while (currentBase is CLRType)
                    currentBase = currentBase.BaseType;

                // Return base
                return currentBase;
            }
            return type;
        }

        public static Type GetNonNullableType(this Type type)
        {
            return IsNullableType(type) == true
                ? type.GetGenericArguments()[0]
                : type;
        }

        public static bool IsNullableType(this Type type)
        {
#if API_NET35
            return type.IsGenericTypeDefinition == true && type.GetGenericTypeDefinition() == typeof(Nullable<>);
#else
            return type.IsConstructedGenericType == true && type.GetGenericTypeDefinition() == typeof(Nullable<>);
#endif
        }

        public static bool IsNumeric(this Type type)
        {
            switch (Type.GetTypeCode(GetNonNullableType(type)))
            {
                case TypeCode.Char:
                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Double:
                case TypeCode.Single:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return true;
            }
            return false;
        }

        public static bool IsArithmetic(this Type type)
        {
            type = GetNonNullableType(type);

            if(type.IsEnum == false)
            {
                switch (Type.GetTypeCode(type))
                {
                    case TypeCode.Int16:
                    case TypeCode.Int32:
                    case TypeCode.Int64:
                    case TypeCode.Double:
                    case TypeCode.Single:
                    case TypeCode.UInt16:
                    case TypeCode.UInt32:
                    case TypeCode.UInt64:
                        return true;
                }
            }
            return false;
        }

        public static object GetDefaultValue(this Type type, AppDomain domain)
        {
            // Check for class
            if (type.IsClass == true || type.IsArray == true)
                return null;

            // Check for CLR struct
            if(type is CLRType)
            {
                // Check for enum
                if (type.IsEnum == true)
                    return type.GetEnumUnderlyingType().GetDefaultValue(domain);

                // Create default struct instance
                return CLRInstance.CreateAllocatedInstance(domain, type as CLRType);
            }

            // Create instance
            return Activator.CreateInstance(type);
        }

        public static TypeID GetTypeID(this Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.SByte: return TypeID.Int8;
                case TypeCode.Byte: return TypeID.UInt8;                
                case TypeCode.Int16: return TypeID.Int16;
                case TypeCode.UInt16: return TypeID.UInt16;

                case TypeCode.Boolean:
                case TypeCode.Char:
                case TypeCode.Int32: return TypeID.Int32;
                case TypeCode.UInt32: return TypeID.UInt32;
                case TypeCode.Int64: return TypeID.Int64;
                case TypeCode.UInt64: return TypeID.UInt64;
                case TypeCode.Single: return TypeID.Single;
                case TypeCode.Double: return TypeID.Double;

                case TypeCode.Object: return TypeID.Object;
            }

            throw new NotSupportedException("Unable to get type id information for unsupported type: " + type);
        }

        public static bool HasVirtualMembers(this Type type)
        {
            // Check for null
            if (type == null)
                throw new ArgumentNullException("type");

            bool result = false;

            // Check for cached result
            if (virtualMemberCache.TryGetValue(type, out result) == true)
                return result;

            // Check all members
            foreach(MemberInfo member in type.GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy))
            {
                // Property
                if(member is PropertyInfo)
                {
                    PropertyInfo property = (PropertyInfo)member;

#if API_NET35
                    foreach(MethodInfo accessor in property.GetAccessors())
                    {
                        if (accessor.Name.StartsWith("get_") == true && accessor.IsVirtual == true) result = true;
                        else if (accessor.Name.StartsWith("set_") == true && accessor.IsVirtual == true) result = true;
                    }
#else
                    // Check for virtual
                    if (property.GetMethod != null && property.GetMethod.IsVirtual == true) result = true;
                    if (property.SetMethod != null && property.SetMethod.IsVirtual == true) result = true;
#endif
                }

                // Methods
                if(member is MethodInfo)
                {
                    MethodInfo method = (MethodInfo)member;

                    // Check for virtual
                    if (method.IsVirtual == true) result = true;
                }

                // Check for found result
                if (result == true)
                    break;
            }

            // Cache the result
            virtualMemberCache.Add(type, result);

            return result;
        }

        internal static bool AreReferenceAssignable(Type dest, Type source)
        {
            // Check for direct match
            if (dest == source)
                return true;

            // Check assignable
            if(dest.IsValueType == false && source.IsValueType == false && AreAssignable(dest, source) == true)
            {
                return true;
            }

            // Not convertible
            return false;
        }

        internal static bool AreAssignable(Type dest, Type source)
        {
            // Check for direct match
            if (dest == source)
                return true;

            // Can't use IsAssignable check on clr types - causes hard crash in some cases
            if (dest.IsCLRType() == true)
            {
                // Check for sub class - need further work to support interfaces??
                if (source.IsSubclassOf(dest) == true)
                    return true;
            }
            else
            {
                // Check for assignable
                if (dest.IsAssignableFrom(source) == true)
                    return true;
            }

            // Handle arrays
            if(dest.IsArray == true && source.IsArray == true && 
                dest.GetArrayRank() == source.GetArrayRank() &&
                AreReferenceAssignable(dest.GetElementType(), source.GetElementType()) == true)
            {
                return true;
            }

            // Handle generics arrays
            if(source.IsArray == true && dest.IsGenericType == true &&
                (dest.GetGenericTypeDefinition() == typeof(IEnumerable<>) ||
                dest.GetGenericTypeDefinition() == typeof(IList<>) ||
                dest.GetGenericTypeDefinition() == typeof(ICollection<>)) &&
                dest.GetGenericArguments()[0] == source.GetElementType())
            {
                return true;
            }

            // Not convertable
            return false;
        }

        internal static bool IsFieldAssignableSlow(FieldInfo destField, object targetValue)
        {
            // Check for null
            if(targetValue == null)
            {
                // Check if we can store null
                return destField.FieldType.IsValueType == false;
            }

            // Check for types assignable
            return AreAssignable(destField.FieldType, targetValue.GetInterpretedType());
        }

        internal static bool IsPropertyAssignableSlow(PropertyInfo destProperty, object targetValue)
        {
            // Check for null
            if (targetValue == null)
            {
                // Check if we can store null
                return destProperty.PropertyType.IsValueType == false;
            }

            // Check for types assignable
            return AreAssignable(destProperty.PropertyType, targetValue.GetInterpretedType());
        }
    }
}
