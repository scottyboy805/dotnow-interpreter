using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace dotnow.Runtime
{
#if UNSAFE
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct StackData
#else
    [StructLayout(LayoutKind.Sequential)]
    public struct StackData
#endif
    {
        public enum ObjectType
        {
            Null = 0,
            Int8 = 7,
            Int16 = 15,
            Int32 = 31,
            Int64 = 63,
            UInt8 = 8,
            UInt16 = 16,
            UInt32 = 32,
            UInt64 = 64,
            Single,
            Double,
            Ref,            // Object
            RefBoxed,       // Boxed primitive
            ByRef           // By ref local, argument, element or field
        }

#if UNSAFE
        // Do not store primitive types as object (Very slow - requires boxing at every instruction)
        [StructLayout(LayoutKind.Explicit)]
        public unsafe struct Primitive
#else
        // Do not store primitive types as object (Very slow - requires boxing at every instruction)
        [StructLayout(LayoutKind.Explicit)]
        public struct Primitive
#endif
        {
            [FieldOffset(0)]
            public sbyte Int8;
            [FieldOffset(0)]
            public short Int16;
            [FieldOffset(0)]
            public int Int32;
            [FieldOffset(0)]
            public long Int64;
            [FieldOffset(0)]
            public float Single;
            [FieldOffset(0)]
            public double Double;

            [FieldOffset(0)]
            public GCHandle Handle;
        }

        // Public
        public static readonly StackData nullPtr = new StackData { type = ObjectType.Null };

        public ObjectType type;     // 4
        public Primitive value;     // 8
        public object refValue;     // 4 = 16

        // Properties
        public int Address
        {
            get { return refValue == null ? 0 : 1; }
        }

        // Methods
        public T GetRefValue<T>()
        {
            switch (type)
            {
                case ObjectType.Ref:
                case ObjectType.RefBoxed:
                    {
                        // Check for null
                        if (refValue == null)
                            throw new NullReferenceException();

                        // Get reference value
                        return (T)refValue;
                    }
                case ObjectType.ByRef:
                    {
                        // Get byref reference value
                        return ((IByRef)refValue).GetReferenceValue().GetRefValue<T>();
                    }
            }
            throw new NotSupportedException("Not a reference or boxed struct type");
        }

        public object Box()
        {
            switch(type)
            {
                case ObjectType.Null: return null;
                case ObjectType.Int8: return (sbyte)value.Int8;
                case ObjectType.Int16: return (short)value.Int16;
                case ObjectType.Int32: return value.Int32;
                case ObjectType.Int64: return value.Int64;
                case ObjectType.UInt8: return (byte)value.Int8;
                case ObjectType.UInt16: return (ushort)value.Int16;
                case ObjectType.UInt32: return (uint)value.Int32;
                case ObjectType.UInt64: return (ulong)value.Int64;
                case ObjectType.Single: return (float)value.Single;
                case ObjectType.Double: return (double)value.Double;
            }
            return refValue;
        }

        public object BoxAsType(in CLRTypeInfo typeInfo)
        {
            // Check for enum type
            if (typeInfo.typeCode == TypeCode.Object && typeInfo.isEnum == true && typeInfo.isArray == false)
                return BoxAsTypeSlow(typeInfo.type.GetEnumUnderlyingType());

            // Handle reference type
            switch (type)
            {
                case ObjectType.Null:
                case ObjectType.Ref:
                case ObjectType.ByRef:
                case ObjectType.RefBoxed:
                    return Box();
            }

            // Box based on type code
            switch(typeInfo.typeCode)
            {
                case TypeCode.SByte: return (sbyte)value.Int8;
                case TypeCode.Int16: return (short)value.Int16;
                case TypeCode.Int32: return (int)value.Int32;
                case TypeCode.Int64: return (long)value.Int64;
                case TypeCode.Byte: return (byte)value.Int8;
                case TypeCode.UInt16: return (ushort)value.Int16;
                case TypeCode.UInt32: return (uint)value.Int32;
                case TypeCode.UInt64: return (ulong)value.Int64;
                case TypeCode.Single: return (float)value.Single;
                case TypeCode.Double: return (double)value.Double;
            }
            // Fallback to default beahviour
            return Box();
        }

        public object BoxAsTypeSlow(Type asType)
        {
            // Get type code
            TypeCode code = Type.GetTypeCode(asType);

            // Check for enum type
            if (code == TypeCode.Object && asType.IsEnum == true && asType.IsArray == false)
                return BoxAsTypeSlow(asType.GetEnumUnderlyingType());

            switch(type)
            {
                case ObjectType.Null:
                case ObjectType.Ref:
                case ObjectType.ByRef:
                case ObjectType.RefBoxed:
                    return Box();
            }

            // Convert to type
            if (asType.IsEnum == false)
            {
                return Convert.ChangeType(Box(), asType);
            }
            // If we somehow reached here with an enum type, use Enum.ToObject
            return Enum.ToObject(asType, Box());
        }

        public object UnboxAsTypeSlow(Type asType)
        {
            // Get type code
            TypeCode code = Type.GetTypeCode(asType);

            // Check for enum type
            if (code == TypeCode.Object && asType.IsEnum == true && asType.IsArray == false)
                return UnboxAsTypeSlow(asType.GetEnumUnderlyingType());

            return UnboxAsType(code);
        }

#if API_NET35
        public object UnboxAsType(CLRTypeInfo typeInfo)
#else
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public object UnboxAsType(in CLRTypeInfo typeInfo)
#endif
        {
            // Check for enum type
            if (typeInfo.typeCode == TypeCode.Object && typeInfo.isEnum == true && typeInfo.isArray == false)
                return UnboxAsType(typeInfo.enumUnderlyingTypeCode);

            // Unbox by type code
            return UnboxAsType(typeInfo.typeCode);
        }

#if !API_NET35
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public void UnboxAsType(ref StackData dest, in CLRTypeInfo typeInfo)
        {
            // Check for enum type
            if (typeInfo.typeCode == TypeCode.Object && typeInfo.isEnum == true && typeInfo.isArray == false)
            {
                UnboxAsType(ref dest, typeInfo.enumUnderlyingTypeCode);
            }
            else
            {
                // Unbox by type code
                UnboxAsType(ref dest, typeInfo.typeCode);
            }
        }

        public void UnboxAsType(ref StackData dest, TypeCode typeCode)
        {
            // Check for null
            if(type == ObjectType.Null)
            {
                dest = StackData.nullPtr;
                return;
            }

            // Check for boxed
            if (type == ObjectType.RefBoxed)
            {
                switch (typeCode)
                {
                    case TypeCode.Boolean:
                        {
                            dest.value.Int32 = (bool)refValue ? 1 : 0;
                            dest.type = ObjectType.Int32;
                            break;
                        }
                    case TypeCode.SByte:
                        {
                            dest.value.Int8 = (sbyte)refValue; 
                            dest.type = ObjectType.Int8;
                            break;
                        }
                    case TypeCode.Byte:
                        {
                            dest.value.Int8 = (sbyte)(byte)refValue;
                            dest.type = ObjectType.UInt8;
                            break;
                        }
                    case TypeCode.Int16:
                        {
                            dest.value.Int16 = (short)refValue;
                            dest.type = ObjectType.Int16;
                            break;
                        }
                    case TypeCode.UInt16:
                        {
                            dest.value.Int16 = (short)(ushort)refValue;
                            dest.type = ObjectType.UInt16;
                            break;
                        }
                    case TypeCode.Int32:
                        {
                            dest.value.Int32 = (int)refValue;
                            dest.type = ObjectType.Int32;
                            break;
                        }
                    case TypeCode.UInt32:
                        {
                            dest.value.Int32 = (int)(uint)refValue;
                            dest.type = ObjectType.UInt32;
                            break;
                        }
                    case TypeCode.Int64:
                        {
                            dest.value.Int64 = (long)refValue;
                            dest.type = ObjectType.Int64;
                            break;
                        }
                    case TypeCode.UInt64:
                        {
                            dest.value.Int64 = (long)(ulong)refValue;
                            dest.type = ObjectType.UInt64;
                            break;
                        }
                    case TypeCode.Single:
                        {
                            dest.value.Single = (float)refValue;
                            dest.type = ObjectType.Single;
                            break;
                        }
                    case TypeCode.Double:
                        {
                            dest.value.Double = (double)refValue;
                            dest.type = ObjectType.Double;
                            break;
                        }

                    default:
                        throw new NotSupportedException("Attemtping to box unsupported type: " + typeCode);
                }
            }
            else
                throw new InvalidOperationException("Type must be RefBoxed in order to support unbox operation: " + type);
        }

        public object UnboxAsType(TypeCode typeCode)
        {
            // Cannot box nullable
            if (type == ObjectType.Null)
                return null;
            // Check for boxed type - need to unbox
            if (type == ObjectType.RefBoxed)
            {
                switch (typeCode)
                {
                    case TypeCode.Boolean: return (bool)refValue;
                    case TypeCode.SByte: return (sbyte)refValue;
                    case TypeCode.Byte: return (byte)refValue;
                    case TypeCode.Char: return (char)refValue;
                    case TypeCode.Int16: return (short)refValue;
                    case TypeCode.Int32: return (int)refValue;
                    case TypeCode.Int64: return (long)refValue;
                    case TypeCode.UInt16: return (ushort)refValue;
                    case TypeCode.UInt32: return (uint)refValue;
                    case TypeCode.UInt64: return (ulong)refValue;
                    case TypeCode.Single: return (float)refValue;
                    case TypeCode.Double: return (double)refValue;
                }
            }

            switch(typeCode)
            {
                case TypeCode.Boolean: return value.Int32 == 0 ? false : true;
                case TypeCode.SByte: return (sbyte)value.Int8;
                case TypeCode.Byte: return (byte)value.Int8;
                case TypeCode.Char: return (char)value.Int8;
                case TypeCode.Int16: return (short)value.Int16;
                case TypeCode.Int32: return value.Int32;
                case TypeCode.Int64: return value.Int64;
                case TypeCode.UInt16: return (ushort)value.Int16;
                case TypeCode.UInt32: return (uint)value.Int32;
                case TypeCode.UInt64: return (ulong)value.Int64;
                case TypeCode.Single: return (float)value.Single;
                case TypeCode.Double: return (double)value.Double;
            }
            return refValue;
        }

        public override string ToString()
        {
            switch(type)
            {
                default:
                case ObjectType.Null: return "nullptr";
                case ObjectType.Int8: return value.Int8.ToString();
                case ObjectType.Int16: return value.Int16.ToString();
                case ObjectType.Int32: return value.Int32.ToString();
                case ObjectType.Int64: return value.Int64.ToString();
                case ObjectType.UInt8: return ((byte)value.Int8).ToString();
                case ObjectType.UInt16: return ((ushort)value.Int16).ToString();
                case ObjectType.UInt32: return ((uint)value.Int32).ToString();
                case ObjectType.UInt64: return ((ulong)value.Int64).ToString();
                case ObjectType.Single: return value.Single.ToString();
                case ObjectType.Double: return value.Double.ToString();
                case ObjectType.Ref:
                case ObjectType.RefBoxed:
                    {
                        if (refValue == null)
                            return "nullptr";
                        return refValue.ToString();
                    }
                case ObjectType.ByRef: return refValue.ToString();
            }
        }

        public static void AllocTypedSlow(ref StackData obj, Type asType, object value, bool promoteSmallPrimitives = false)
        {
            // Get type code
            TypeCode code = Type.GetTypeCode(asType);

            // Check for enum
            if (code == TypeCode.Object && asType.IsEnum == true && asType.IsArray == false)
            {
                AllocTypedSlow(ref obj, asType.GetEnumUnderlyingType(), value, promoteSmallPrimitives);
                return;
            }

            AllocTyped(ref obj, code, value, promoteSmallPrimitives);
        }

#if API_NET35
        public static void AllocTyped(ref StackData obj, CLRTypeInfo typeInfo, object value)
#else
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AllocTyped(ref StackData obj, in CLRTypeInfo typeInfo, object value, bool promoteSmallPrimitives = false)
#endif
        {
            // Check for enum
            if(typeInfo.typeCode == TypeCode.Object && typeInfo.isEnum == true && typeInfo.isArray == false)
            {
                AllocTyped(ref obj, typeInfo.enumUnderlyingTypeCode, value);
                return;
            }

            AllocTyped(ref obj, typeInfo.typeCode, value, promoteSmallPrimitives);
        }

        public static void AllocTyped(ref StackData obj, TypeCode typeCode, object value, bool promoteSmallPrimitives = false)
        {
            switch (typeCode)
            {
                case TypeCode.Boolean:
                    {
                        obj.type = ObjectType.Int32;
                        obj.value.Int32 = ((bool)value) == true ? 1 : 0;
                        break;
                    }
                case TypeCode.SByte:
                    {
                        if (promoteSmallPrimitives == true)
                        {
                            obj.type = ObjectType.Int32;
                            obj.value.Int32 = (sbyte)value;
                        }
                        else
                        {
                            obj.type = ObjectType.Int8;
                            obj.value.Int8 = (sbyte)value;
                        }
                        break;
                    }
                case TypeCode.Char:
                    {
                        obj.type = ObjectType.UInt8;
                        obj.value.Int8 = (sbyte)(char)value;
                        break;
                    }
                case TypeCode.Int16: 
                    {
                        if (promoteSmallPrimitives == true)
                        {
                            obj.type = ObjectType.Int32;
                            obj.value.Int32 = (short)value;
                        }
                        else
                        {
                            obj.type = ObjectType.Int16;
                            obj.value.Int16 = (short)value;
                        }
                        break;
                    }
                case TypeCode.Int32:
                    {
                        obj.type = ObjectType.Int32;
                        obj.value.Int32 = (int)value;
                        break;
                    }
                case TypeCode.Int64:
                    {
                        obj.type = ObjectType.Int64;
                        obj.value.Int64 = (long)value;
                        break;
                    }
                case TypeCode.Byte:
                    {
                        if (promoteSmallPrimitives == true)
                        {
                            obj.type = ObjectType.Int32;
                            obj.value.Int32 = (byte)value;
                        }
                        else
                        {
                            obj.type = ObjectType.UInt8;
                            obj.value.Int8 = (sbyte)(byte)value;
                        }
                        break;
                    }
                case TypeCode.UInt16:
                    {
                        if (promoteSmallPrimitives == true)
                        {
                            obj.type = ObjectType.Int32;
                            obj.value.Int32 = (ushort)value;
                        }
                        else
                        {
                            obj.type = ObjectType.UInt16;
                            obj.value.Int16 = (short)(ushort)value;
                        }
                        break;
                    }
                case TypeCode.UInt32:
                    {
                        obj.type = ObjectType.UInt32;
                        obj.value.Int32 = (int)(uint)value;
                        break;
                    }
                case TypeCode.UInt64:
                    {
                        obj.type = ObjectType.UInt64;
                        obj.value.Int64 = (long)(ulong)value;
                        break;
                    }

                case TypeCode.Single:
                    {
                        obj.type = ObjectType.Single;
                        obj.value.Single = (float)value;
                        break;
                    }
                case TypeCode.Double:
                    {
                        obj.type = ObjectType.Double;
                        obj.value.Double = (double)value;
                        break;
                    }

                default:
                case TypeCode.String:
                case TypeCode.Object:
                    {
                        // Check for null
                        if(value == null)
                        {
                            obj = nullPtr;
                            obj.type = ObjectType.Ref;
                            break;
                        }

                        // Check for value types - probably a little expensive??
                        if (value is ValueType)
                        {
                            obj.type = ObjectType.RefBoxed;
                            obj.refValue = value;
                            break;
                        }

                        obj.type = ObjectType.Ref;
                        obj.refValue = value;
                        break;
                    }
            }
        }

#if !API_NET35
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static void Alloc(ref StackData obj, bool val)
        {
            obj.type = ObjectType.Int32;
            obj.value.Int32 = (val == true) ? 1 : 0;
        }

#if !API_NET35
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static void Alloc(ref StackData obj, sbyte val)
        {
            obj.type = ObjectType.Int8;
            obj.value.Int8 = val;
        }

#if !API_NET35
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static void Alloc(ref StackData obj, byte val)
        {
            obj.type = ObjectType.UInt8;
            obj.value.Int8 = (sbyte)val;
        }

#if !API_NET35
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static void Alloc(ref StackData obj, short val)
        {
            obj.type = ObjectType.Int16;
            obj.value.Int16 = val;
        }

#if !API_NET35
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static void Alloc(ref StackData obj, ushort val)
        {
            obj.type = ObjectType.UInt16;
            obj.value.Int16 = (short)val;
        }

#if !API_NET35
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static void Alloc(ref StackData obj, int val)
        {
            obj.type = ObjectType.Int32;
            obj.value.Int32 = val;
        }

#if !API_NET35
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static void Alloc(ref StackData obj, uint val)
        {
            obj.type = ObjectType.UInt32;
            obj.value.Int32 = (int)val;
        }

#if !API_NET35
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static void Alloc(ref StackData obj, long val)
        {
            obj.type = ObjectType.Int64;
            obj.value.Int64 = val;
        }

#if !API_NET35
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static void Alloc(ref StackData obj, ulong val)
        {
            obj.type = ObjectType.UInt64;
            obj.value.Int64 = (long)val;
        }

#if !API_NET35
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static void Alloc(ref StackData obj, float val)
        {
            obj.type = ObjectType.Single;
            obj.value.Single = val;
        }

#if !API_NET35
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static void Alloc(ref StackData obj, double val)
        {
            obj.type = ObjectType.Double;
            obj.value.Double = val;
        }

#if !API_NET35
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static void AllocRef(ref StackData obj, object val)
        {
            obj.type = ObjectType.Ref;
            obj.refValue = val;
        }

#if !API_NET35
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static void AllocRefBoxed(ref StackData obj, object valueType)
        {
            obj.type = ObjectType.RefBoxed;
            obj.refValue = valueType;
        }

#if !API_NET35
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static void ValueTypeCopy(ref StackData obj)
        {
            // Check for boxed struct
            if(obj.type == ObjectType.RefBoxed && obj.refValue != null)
            {
                // This call will take the boxed value type stored in 'src' and perform a stuct copy (memberwise clone) returning a boxed reference to the new value type with same values
                obj.refValue = System.Runtime.CompilerServices.RuntimeHelpers.GetObjectValue(obj.refValue);
            }
        }

#if API_NET35
        public static void AssignKeepType(ref StackData dest, StackData src)
#else
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AssignKeepType(ref StackData dest, in StackData src)
#endif
        {
            // Get existing type
            ObjectType type = dest.type;

            // Copy value
            dest = src;

            // Check for primitive promoted types
            switch(type)
            {
                case ObjectType.Int8:
                case ObjectType.Int16:
                case ObjectType.UInt8:
                case ObjectType.UInt16:                
                    return;
            }

            // Overwrite type - except for boxed primitved
            if(src.type != ObjectType.RefBoxed)
                dest.type = type;
        }

#if API_NET35
    public static bool NullCheck(StackData val)
#else
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool NullCheck(in StackData val)
#endif
        {
            switch (val.type)
            {
                default:
                    throw new NotSupportedException("Null check operation is not supported on data type: " + val.type);

                    // Check explicit null
                case ObjectType.Null: 
                    return true;

                // Check by ref
                case ObjectType.ByRef:
                    return ((IByRef)val.refValue).Instance == null;

                    // Check reference null
                case ObjectType.Ref:
                case ObjectType.RefBoxed:
                    return val.refValue == null;
            }
        }

        public static ObjectType StackTypeFromTypeCode(TypeCode code)
        {
            switch(code)
            {
                case TypeCode.Boolean: return ObjectType.Int32;
                case TypeCode.Object:
                case TypeCode.String: return ObjectType.Ref;
                case TypeCode.Byte: return ObjectType.UInt8;
                case TypeCode.SByte: return ObjectType.Int8;
                case TypeCode.UInt16: return ObjectType.UInt16;
                case TypeCode.Int16: return ObjectType.Int16;
                case TypeCode.UInt32: return ObjectType.UInt32;
                case TypeCode.Int32: return ObjectType.Int32;
                case TypeCode.UInt64: return ObjectType.UInt64;
                case TypeCode.Int64: return ObjectType.Int64;
                case TypeCode.Single: return ObjectType.Single;
                case TypeCode.Double: return ObjectType.Double;
            }

            throw new NotSupportedException("Unsupported type: " + code.ToString());
        }
    }
}
