using System;

namespace TrivialCLR.Runtime
{
    public class CLRTypeInfo
    {
        // Public
        public Type type;
        public TypeCode typeCode;
        public TypeCode enumUnderlyingTypeCode;
        public bool isEnum;
        public bool isArray;

        // Constructor
        public CLRTypeInfo(Type type)
        {
            this.type = type;
            this.typeCode = Type.GetTypeCode(type);
            this.enumUnderlyingTypeCode = 0;
            this.isEnum = type.IsEnum;
            this.isArray = type.IsArray;

            if (this.isEnum == true)
                this.enumUnderlyingTypeCode = Type.GetTypeCode(type.GetEnumUnderlyingType());
        }
    }
}
