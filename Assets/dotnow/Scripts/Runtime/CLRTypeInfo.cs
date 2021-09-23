using System;
using System.Collections.Generic;

namespace dotnow.Runtime
{
    public class CLRTypeInfo
    {
        // Private
        private static Dictionary<Type, CLRTypeInfo> clrTypeInfos = new Dictionary<Type, CLRTypeInfo>()
        {
            // Pre-cache system types to save on lazy loading performance of common used types
            { typeof(byte), new CLRTypeInfo(typeof(byte)) },
            { typeof(ushort), new CLRTypeInfo(typeof(ushort)) },
            { typeof(uint), new CLRTypeInfo(typeof(uint)) },
            { typeof(ulong), new CLRTypeInfo(typeof(ulong)) },
            { typeof(char), new CLRTypeInfo(typeof(char)) },
            { typeof(sbyte), new CLRTypeInfo(typeof(sbyte)) },
            { typeof(short), new CLRTypeInfo(typeof(short)) },
            { typeof(int), new CLRTypeInfo(typeof(int)) },
            { typeof(long), new CLRTypeInfo(typeof(long)) },
            { typeof(float), new CLRTypeInfo(typeof(float)) },
            { typeof(double), new CLRTypeInfo(typeof(double)) },
            { typeof(decimal), new CLRTypeInfo(typeof(decimal)) },
            { typeof(string), new CLRTypeInfo(typeof(string)) },
            { typeof(object), new CLRTypeInfo(typeof(object)) },
        };

        // Public
        public Type type;
        public TypeCode typeCode;
        public TypeCode enumUnderlyingTypeCode;
        public bool isEnum;
        public bool isArray;

        // Constructor
        private CLRTypeInfo(Type type)
        {
            this.type = type;
            this.typeCode = Type.GetTypeCode(type);
            this.enumUnderlyingTypeCode = 0;
            this.isEnum = type.IsEnum;
            this.isArray = type.IsArray;

            if (this.isEnum == true)
                this.enumUnderlyingTypeCode = Type.GetTypeCode(type.GetEnumUnderlyingType());
        }

        // Methods
        public object GetDefaultValue(AppDomain domain)
        {
            // Check for simple case - avoid extra work
            if (typeCode == TypeCode.Object || typeCode == TypeCode.String)
                return null;

            return type.GetDefaultValue(domain);
        }

        public static CLRTypeInfo GetTypeInfo(Type type)
        {
            CLRTypeInfo result;

            // Try to get cached insatnce
            if (clrTypeInfos.TryGetValue(type, out result) == true)
                return result;

            // Create new
            result = new CLRTypeInfo(type);

            // Cache instance
            lock(clrTypeInfos)
            {
                clrTypeInfos.Add(type, result);
            }
            return result;
        }
    }
}
