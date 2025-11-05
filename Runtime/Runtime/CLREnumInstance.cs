using dotnow.Interop;
using dotnow.Reflection;
using dotnow.Runtime.CIL;
using System;

namespace dotnow.Runtime
{
    internal readonly struct CLREnumInstance : ICLRInstance
    {
        // Public
        public readonly CLRType Type;
        public readonly StackData Value;

        // Constructor
        internal CLREnumInstance(CILTypeInfo typeInfo, StackData value)
        {
            // Check for CLR
            if ((typeInfo.Flags & CILTypeFlags.Interpreted) == 0)
                throw new ArgumentException("Only supported for interpreted types");

            this.Type = (CLRType)typeInfo.Type;
            this.Value = value;
        }

        public CLREnumInstance(CLRType metaType, object enumValue)
        {
            // Check for null
            if (metaType == null)
                throw new ArgumentNullException(nameof(metaType));

            this.Type = metaType;

            // Check type
            Type enumType = metaType.GetEnumUnderlyingType();

            // Get type handle
            CILTypeInfo typeInfo = enumType.GetTypeInfo(metaType.AppDomain);

            // Wrap the object
            StackData wrapped = default;
            StackData.Wrap(typeInfo, enumValue, ref wrapped);

            this.Value = wrapped;
        }

        // Methods
        public override string ToString()
        {
            // Get enum value
            object enumValue = UnwrapAsType(Type.GetEnumUnderlyingType());

            // Get enum name
            string name;
            if (Type.CLREnumNames.TryGetValue(enumValue, out name) == false)
                throw new Exception("Could not get enum name: " + Type);

            return name;
        }

        public Type GetInterpretedType()
        {
            return Type;
        }

        public bool Equals(ICLRInstance instance)
        {
            // Check for enum
            if (instance is CLREnumInstance enumInstance)
            {
                // Check type and value
                return enumInstance.Type == Type &&
                    enumInstance.Value.IsPrimitiveEqual(Value);
            }
            return false;
        }

        public object Unwrap()
        {
            // Get the type handle
            CILTypeInfo typeInfo = Type.GetTypeInfo(Type.AppDomain);

            // Copy the value
            StackData tempValue = Value;

            // Get the type - context is not needed here because the enum value should never be and object, but only a primitive integer type
            object unwrapped = null;
            StackData.Unwrap(typeInfo, ref tempValue, ref unwrapped);

            return unwrapped;
        }

        public object UnwrapAsType(Type asType)
        {
            // Check for CLR type
            if (asType is CLRType)
                throw new InvalidOperationException(nameof(asType) + " cannot be an interpreted type");

            // Check for null
            if (asType == null || asType == typeof(object))
                return ((ICLRInstance)this).Unwrap();

            // Get type code
            TypeCode asTypeCode = System.Type.GetTypeCode(asType);

            // Check for supported type
            switch (asTypeCode)
            {
                // Allowed types
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64: break;

                default: throw new NotSupportedException("Cannot get enum value as type: " + asType);
            }

            // Copy the value
            StackData tempValue = Value;

            // Get type handle
            CILTypeInfo typeInfo = asType.GetTypeInfo(Type.AppDomain);

            // Get default unwrapped
            object unwrapped = default;
            StackData.Unwrap(typeInfo, ref tempValue, ref unwrapped);

            return unwrapped;
        }
    }
}
