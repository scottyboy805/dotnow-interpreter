using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace dotnow.Runtime
{
    public enum StackType : uint
    {
        Int32 = 31,
        Int64 = 63,
        UInt32 = 32,
        UInt64 = 64,
        Single,
        Double,
        Ref,            // Object
        RefBoxed,       // Boxed primitive
        ByRef           // By ref local, argument, element or field
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct StackData
    {
        // Public
        [FieldOffset(0)]
        public StackType Type;  // 4

        [FieldOffset(4)]
        private uint Padding;   // 4

        [FieldOffset(8)]
        public int Int32;
        [FieldOffset(8)]
        public long Int64;
        [FieldOffset(8)]
        public float Single;
        [FieldOffset(8)]
        public double Double;

        [FieldOffset(8)]
        public object Ref;     // 8 = 16

        // Properties
        public int Address
        {
            get { return Ref == null ? 0 : 1; }
        }

        // Methods
        public T GetRefValue<T>()
        {
            switch (Type)
            {
                case StackType.Ref:
                case StackType.RefBoxed:
                    {
                        // Check for null
                        if (Ref == null)
                            throw new NullReferenceException();

                        // Get reference value
                        return (T)Ref;
                    }
                case StackType.ByRef:
                    {
                        // Get byref reference value
                        return ((IByRef)Ref).GetReferenceValue().GetRefValue<T>();
                    }
            }
            throw new NotSupportedException("Not a reference or boxed struct type");
        }

        public object Box()
        {
            switch(Type)
            {
                case StackType.Int32: return Int32;
                case StackType.Int64: return Int64;
                case StackType.UInt32: return (uint)Int32;
                case StackType.UInt64: return (ulong)Int64;
                case StackType.Single: return (float)Single;
                case StackType.Double: return (double)Double;
            }
            return Ref;
        }

        public object BoxAsType(in CLRTypeInfo typeInfo)
        {
            // Check for enum type
            if (typeInfo.typeCode == TypeCode.Object && typeInfo.isEnum == true && typeInfo.isArray == false)
                return BoxAsTypeSlow(typeInfo.type.GetEnumUnderlyingType());

            // Handle reference type
            switch (Type)
            {
                case StackType.Ref:
                case StackType.ByRef:
                case StackType.RefBoxed:
                    return Box();
            }

            // Box based on type code
            switch(typeInfo.typeCode)
            {
                case TypeCode.Int32: return (int)Int32;
                case TypeCode.Int64: return (long)Int64;
                case TypeCode.UInt32: return (uint)Int32;
                case TypeCode.UInt64: return (ulong)Int64;
                case TypeCode.Single: return (float)Single;
                case TypeCode.Double: return (double)Double;
            }
            // Fallback to default beahviour
            return Box();
        }

        public object BoxAsTypeSlow(Type asType)
        {
            // Get type code
            TypeCode code = System.Type.GetTypeCode(asType);

            // Check for enum type
            if (code == TypeCode.Object && asType.IsEnum == true && asType.IsArray == false)
                return BoxAsTypeSlow(asType.GetEnumUnderlyingType());

            switch(Type)
            {
                case StackType.Ref:
                case StackType.ByRef:
                case StackType.RefBoxed:
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
            TypeCode code = System.Type.GetTypeCode(asType);

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
            // Check for boxed
            if (Type == StackType.RefBoxed)
            {
                switch (typeCode)
                {
                    case TypeCode.Boolean:
                        {
                            dest.Int32 = (bool)Ref ? 1 : 0;
                            dest.Type = StackType.Int32;
                            break;
                        }
                    case TypeCode.SByte:
                        {
                            dest.Int32 = (sbyte)Ref; 
                            dest.Type = StackType.Int32;
                            break;
                        }
                    case TypeCode.Byte:
                        {
                            dest.Int32 = (sbyte)(byte)Ref;
                            dest.Type = StackType.Int32;
                            break;
                        }
                    case TypeCode.Int16:
                        {
                            dest.Int32 = (short)Ref;
                            dest.Type = StackType.Int32;
                            break;
                        }
                    case TypeCode.UInt16:
                        {
                            dest.Int32 = (short)(ushort)Ref;
                            dest.Type = StackType.Int32;
                            break;
                        }
                    case TypeCode.Int32:
                        {
                            dest.Int32 = (int)Ref;
                            dest.Type = StackType.Int32;
                            break;
                        }
                    case TypeCode.UInt32:
                        {
                            dest.Int32 = (int)(uint)Ref;
                            dest.Type = StackType.UInt32;
                            break;
                        }
                    case TypeCode.Int64:
                        {
                            dest.Int64 = (long)Ref;
                            dest.Type = StackType.Int64;
                            break;
                        }
                    case TypeCode.UInt64:
                        {
                            dest.Int64 = (long)(ulong)Ref;
                            dest.Type = StackType.UInt64;
                            break;
                        }
                    case TypeCode.Single:
                        {
                            dest.Single = (float)Ref;
                            dest.Type = StackType.Single;
                            break;
                        }
                    case TypeCode.Double:
                        {
                            dest.Double = (double)Ref;
                            dest.Type = StackType.Double;
                            break;
                        }

                    default:
                        throw new NotSupportedException("Attempting to box unsupported type: " + typeCode);
                }
            }
            else
                throw new InvalidOperationException("Type must be RefBoxed in order to support unbox operation: " + Type);
        }

        public object UnboxAsType(TypeCode typeCode)
        {
            // Check for boxed type - need to unbox
            if (Type == StackType.RefBoxed)
            {
                switch (typeCode)
                {
                    case TypeCode.Boolean: return (bool)Ref;
                    case TypeCode.SByte: return (sbyte)Ref;
                    case TypeCode.Byte: return (byte)Ref;
                    case TypeCode.Char: return (char)Ref;
                    case TypeCode.Int16: return (short)Ref;
                    case TypeCode.Int32: return (int)Ref;
                    case TypeCode.Int64: return (long)Ref;
                    case TypeCode.UInt16: return (ushort)Ref;
                    case TypeCode.UInt32: return (uint)Ref;
                    case TypeCode.UInt64: return (ulong)Ref;
                    case TypeCode.Single: return (float)Ref;
                    case TypeCode.Double: return (double)Ref;
                }
            }

            switch(typeCode)
            {
                case TypeCode.Boolean: return Int32 == 0 ? false : true;
                case TypeCode.SByte: return (sbyte)Int32;
                case TypeCode.Byte: return (byte)Int32;
                case TypeCode.Char: return (char)Int32;
                case TypeCode.Int16: return (short)Int32;
                case TypeCode.Int32: return Int32;
                case TypeCode.Int64: return Int64;
                case TypeCode.UInt16: return (ushort)Int32;
                case TypeCode.UInt32: return (uint)Int32;
                case TypeCode.UInt64: return (ulong)Int64;
                case TypeCode.Single: return (float)Single;
                case TypeCode.Double: return (double)Double;
            }
            return Ref;
        }

        public override string ToString()
        {
            switch(Type)
            {
                default:
                case StackType.Int32: return Int32.ToString();
                case StackType.Int64: return Int64.ToString();
                case StackType.UInt32: return ((uint)Int32).ToString();
                case StackType.UInt64: return ((ulong)Int64).ToString();
                case StackType.Single: return Single.ToString();
                case StackType.Double: return Double.ToString();
                case StackType.Ref:
                case StackType.RefBoxed:
                    {
                        if (Ref == null)
                            return "nullptr";
                        return Ref.ToString();
                    }
                case StackType.ByRef: return Ref.ToString();
            }
        }

        public static void AllocTypedSlow(ref StackData obj, Type asType, object value, bool promoteSmallPrimitives = false)
        {
            // Get type code
            TypeCode code = System.Type.GetTypeCode(asType);

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
                        obj.Type = StackType.Int32;
                        obj.Int32 = ((bool)value) == true ? 1 : 0;
                        break;
                    }
                case TypeCode.SByte:
                    {
                        if (promoteSmallPrimitives == true)
                        {
                            obj.Type = StackType.Int32;
                            obj.Int32 = (sbyte)value;
                        }
                        else
                        {
                            obj.Type = StackType.Int32;
                            obj.Int32 = (sbyte)value;
                        }
                        break;
                    }
                case TypeCode.Char:
                    {
                        obj.Type = StackType.Int32;
                        obj.Int32 = (sbyte)(char)value;
                        break;
                    }
                case TypeCode.Int16: 
                    {
                        if (promoteSmallPrimitives == true)
                        {
                            obj.Type = StackType.Int32;
                            obj.Int32 = (short)value;
                        }
                        else
                        {
                            obj.Type = StackType.Int32;
                            obj.Int32 = (short)value;
                        }
                        break;
                    }
                case TypeCode.Int32:
                    {
                        obj.Type = StackType.Int32;
                        obj.Int32 = (int)value;
                        break;
                    }
                case TypeCode.Int64:
                    {
                        obj.Type = StackType.Int64;
                        obj.Int64 = (long)value;
                        break;
                    }
                case TypeCode.Byte:
                    {
                        if (promoteSmallPrimitives == true)
                        {
                            obj.Type = StackType.Int32;
                            obj.Int32 = (byte)value;
                        }
                        else
                        {
                            obj.Type = StackType.Int32;
                            obj.Int32 = (sbyte)(byte)value;
                        }
                        break;
                    }
                case TypeCode.UInt16:
                    {
                        if (promoteSmallPrimitives == true)
                        {
                            obj.Type = StackType.Int32;
                            obj.Int32 = (ushort)value;
                        }
                        else
                        {
                            obj.Type = StackType.Int32;
                            obj.Int32 = (short)(ushort)value;
                        }
                        break;
                    }
                case TypeCode.UInt32:
                    {
                        obj.Type = StackType.UInt32;
                        obj.Int32 = (int)(uint)value;
                        break;
                    }
                case TypeCode.UInt64:
                    {
                        obj.Type = StackType.UInt64;
                        obj.Int64 = (long)(ulong)value;
                        break;
                    }

                case TypeCode.Single:
                    {
                        obj.Type = StackType.Single;
                        obj.Single = (float)value;
                        break;
                    }
                case TypeCode.Double:
                    {
                        obj.Type = StackType.Double;
                        obj.Double = (double)value;
                        break;
                    }

                default:
                case TypeCode.String:
                case TypeCode.Object:
                    {
                        // Check for null
                        if(value == null)
                        {
                            obj.Ref = null;
                            obj.Type = StackType.Ref;
                            break;
                        }

                        // Check for value types - probably a little expensive??
                        if (value is ValueType)
                        {
                            obj.Type = StackType.RefBoxed;
                            obj.Ref = value;
                            break;
                        }

                        obj.Type = StackType.Ref;
                        obj.Ref = value;
                        break;
                    }
            }
        }

#if !API_NET35
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static void Alloc(ref StackData obj, bool val)
        {
            obj.Type = StackType.Int32;
            obj.Int32 = (val == true) ? 1 : 0;
        }

#if !API_NET35
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static void Alloc(ref StackData obj, sbyte val)
        {
            obj.Type = StackType.Int32;
            obj.Int32 = val;
        }

#if !API_NET35
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static void Alloc(ref StackData obj, byte val)
        {
            obj.Type = StackType.Int32;
            obj.Int32 = (sbyte)val;
        }

#if !API_NET35
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static void Alloc(ref StackData obj, short val)
        {
            obj.Type = StackType.Int32;
            obj.Int32 = val;
        }

#if !API_NET35
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static void Alloc(ref StackData obj, ushort val)
        {
            obj.Type = StackType.Int32;
            obj.Int32 = (short)val;
        }

#if !API_NET35
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static void Alloc(ref StackData obj, int val)
        {
            obj.Type = StackType.Int32;
            obj.Int32 = val;
        }

#if !API_NET35
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static void Alloc(ref StackData obj, uint val)
        {
            obj.Type = StackType.UInt32;
            obj.Int32 = (int)val;
        }

#if !API_NET35
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static void Alloc(ref StackData obj, long val)
        {
            obj.Type = StackType.Int64;
            obj.Int64 = val;
        }

#if !API_NET35
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static void Alloc(ref StackData obj, ulong val)
        {
            obj.Type = StackType.UInt64;
            obj.Int64 = (long)val;
        }

#if !API_NET35
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static void Alloc(ref StackData obj, float val)
        {
            obj.Type = StackType.Single;
            obj.Single = val;
        }

#if !API_NET35
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static void Alloc(ref StackData obj, double val)
        {
            obj.Type = StackType.Double;
            obj.Double = val;
        }

#if !API_NET35
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static void AllocRef(ref StackData obj, object val)
        {
            obj.Type = StackType.Ref;
            obj.Ref = val;
        }

#if !API_NET35
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static void AllocRefBoxed(ref StackData obj, object valueType)
        {
            obj.Type = StackType.RefBoxed;
            obj.Ref = valueType;
        }

#if !API_NET35
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static void ValueTypeCopy(ref StackData obj)
        {
            // Check for boxed struct
            if(obj.Type == StackType.RefBoxed && obj.Ref != null)
            {
                // This call will take the boxed value type stored in 'src' and perform a stuct copy (memberwise clone) returning a boxed reference to the new value type with same values
                obj.Ref = System.Runtime.CompilerServices.RuntimeHelpers.GetObjectValue(obj.Ref);
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
            StackType type = dest.Type;

            // Copy value
            dest = src;

            // Overwrite type - except for boxed primitved
            if(src.Type != StackType.RefBoxed)
                dest.Type = type;
        }

#if API_NET35
    public static bool NullCheck(StackData val)
#else
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool NullCheck(in StackData val)
#endif
        {
            switch (val.Type)
            {
                default:
                    throw new NotSupportedException("Null check operation is not supported on data type: " + val.Type);

                // Check by ref
                case StackType.ByRef:
                    return ((IByRef)val.Ref).Instance == null;

                    // Check reference null
                case StackType.Ref:
                case StackType.RefBoxed:
                    return val.Ref == null;
            }
        }

        public static StackType StackTypeFromTypeCode(TypeCode code)
        {
            switch(code)
            {
                case TypeCode.Boolean: return StackType.Int32;
                case TypeCode.Object:
                case TypeCode.String: return StackType.Ref;
                case TypeCode.UInt32: return StackType.UInt32;
                case TypeCode.Int32: return StackType.Int32;
                case TypeCode.UInt64: return StackType.UInt64;
                case TypeCode.Int64: return StackType.Int64;
                case TypeCode.Single: return StackType.Single;
                case TypeCode.Double: return StackType.Double;
            }

            throw new NotSupportedException("Unsupported type: " + code.ToString());
        }
    }
}
