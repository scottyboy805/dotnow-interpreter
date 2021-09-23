using System;
using System.Collections.Generic;

namespace dotnow.Runtime
{
    public class CLRTypeInfo
    {
        // Private
        private static Dictionary<Type, CLRTypeInfo> clrTypeInfos = new Dictionary<Type, CLRTypeInfo>();

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
