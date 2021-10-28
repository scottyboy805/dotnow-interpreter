using System;
using System.Runtime.InteropServices;

namespace dotnow.Runtime
{
#if UNSAFE
    [StructLayout(LayoutKind.Explicit)]
    public unsafe struct StackData
#else
    [StructLayout(LayoutKind.Explicit)]
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
            RefField,       // Addr of field
            RefElement,     // Addr of array element
            RefStack,       // Addr of stack element
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
        }

        // Public
        public static readonly StackData nullPtr = new StackData { type = ObjectType.Null };

        [FieldOffset(0)]
        public ObjectType type;     // 4
        [FieldOffset(4)]
        public Primitive value;     // 8
        [FieldOffset(4)]
        public int address;         // union

        // Properties
        public bool IsObject
        {
            get
            {
                switch (type)
                {
                    case ObjectType.Ref:
                    case ObjectType.RefField:
                    case ObjectType.RefElement:
                    case ObjectType.RefStack:
                        return true;
                }
                return false;
            }
        }

        // Methods
        /// <summary>
        /// Convert this stack data to its managed object representation.
        /// This may require fetching the object from the heap.
        /// Use <see cref="Box(__heapallocator)"/> where possible for slight performance improvement.
        /// </summary>
        /// <returns></returns>
        public object Box()
        {
            return Box(__heapallocator.GetCurrent());
        }

        public object Box(__heapallocator _heap)
        {
            switch (type)
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
            return _heap.FetchPinnedValue(this);
        }

        public object UnboxAsTypeSlow(__heapallocator _heap, Type asType)
        {
            // Get type code
            TypeCode code = Type.GetTypeCode(asType);

            // Check for enum type
            if (code == TypeCode.Object && asType.IsEnum == true && asType.IsArray == false)
                return UnboxAsTypeSlow(_heap, asType.GetEnumUnderlyingType());

            return UnboxAsType(_heap, code);
        }

        public object UnboxAsType(__heapallocator _heap, in CLRTypeInfo typeInfo)
        {
            // Check for enum type
            if (typeInfo.typeCode == TypeCode.Object && typeInfo.isEnum == true && typeInfo.isArray == false)
                return UnboxAsType(_heap, typeInfo.enumUnderlyingTypeCode);

            // Unbox by type code
            return UnboxAsType(_heap, typeInfo.typeCode);
        }

        public object UnboxAsType(__heapallocator _heap, TypeCode typeCode)
        {
            // Cannot box nullable
            if (type == ObjectType.Null)
                return null;

            // Check for boxed type - need to unbox
            if (IsObject == true)
            {
                // Get the value from memory
                return _heap.FetchPinnedValue(this);
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
            return null;
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
                case ObjectType.RefField:
                case ObjectType.RefElement:
                case ObjectType.RefStack:
                    {
                        if (address > 0)
                            return string.Format("{0}({1}): {2}", type, address, __heapallocator.GetCurrent().FetchPinnedValue(this));

                        return "nullptr";
                    }
            }
        }

        public static void AllocTypedSlow(__heapallocator _heap, ref StackData obj, Type asType, object value)
        {
            // Get type code
            TypeCode code = Type.GetTypeCode(asType);

            // Check for enum
            if (code == TypeCode.Object && asType.IsEnum == true && asType.IsArray == false)
            {
                AllocTypedSlow(_heap, ref obj, asType.GetEnumUnderlyingType(), value);
                return;
            }

            AllocTyped(_heap, ref obj, code, value);
        }

        public static void AllocTyped(__heapallocator _heap, ref StackData obj, in CLRTypeInfo typeInfo, object value)
        {
            // Check for enum
            if(typeInfo.typeCode == TypeCode.Object && typeInfo.isEnum == true && typeInfo.isArray == false)
            {
                AllocTyped(_heap, ref obj, typeInfo.enumUnderlyingTypeCode, value);
                return;
            }

            AllocTyped(_heap, ref obj, typeInfo.typeCode, value);
        }

        public static void AllocTyped(__heapallocator _heap, ref StackData obj, TypeCode typeCode, object value)
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
                        obj.type = ObjectType.Int8;
                        obj.value.Int8 = (sbyte)value;
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
                        obj.type = ObjectType.Int16;
                        obj.value.Int16 = (short)value;
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
                        obj.type = ObjectType.UInt8;
                        obj.value.Int8 = (sbyte)(byte)value;
                        break;
                    }
                case TypeCode.UInt16:
                    {
                        obj.type = ObjectType.UInt16;
                        obj.value.Int16 = (short)(ushort)value;
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
                        _heap.PinManagedObject(ref obj, value);
                        break;
                    }
            }
        }

        public static void AllocTypedPrimitive(ref StackData obj, TypeCode typeCode, object primitiveValue)
        {
            switch (typeCode)
            {
                case TypeCode.Boolean:
                    {
                        obj.type = ObjectType.Int32;
                        obj.value.Int32 = ((bool)primitiveValue) == true ? 1 : 0;
                        break;
                    }
                case TypeCode.SByte:
                    {
                        obj.type = ObjectType.Int8;
                        obj.value.Int8 = (sbyte)primitiveValue;
                        break;
                    }
                case TypeCode.Char:
                    {
                        obj.type = ObjectType.UInt8;
                        obj.value.Int8 = (sbyte)(char)primitiveValue;
                        break;
                    }
                case TypeCode.Int16:
                    {
                        obj.type = ObjectType.Int16;
                        obj.value.Int16 = (short)primitiveValue;
                        break;
                    }
                case TypeCode.Int32:
                    {
                        obj.type = ObjectType.Int32;
                        obj.value.Int32 = (int)primitiveValue;
                        break;
                    }
                case TypeCode.Int64:
                    {
                        obj.type = ObjectType.Int64;
                        obj.value.Int64 = (long)primitiveValue;
                        break;
                    }
                case TypeCode.Byte:
                    {
                        obj.type = ObjectType.UInt8;
                        obj.value.Int8 = (sbyte)(byte)primitiveValue;
                        break;
                    }
                case TypeCode.UInt16:
                    {
                        obj.type = ObjectType.UInt16;
                        obj.value.Int16 = (short)(ushort)primitiveValue;
                        break;
                    }
                case TypeCode.UInt32:
                    {
                        obj.type = ObjectType.UInt32;
                        obj.value.Int32 = (int)(uint)primitiveValue;
                        break;
                    }
                case TypeCode.UInt64:
                    {
                        obj.type = ObjectType.UInt64;
                        obj.value.Int64 = (long)(ulong)primitiveValue;
                        break;
                    }

                case TypeCode.Single:
                    {
                        obj.type = ObjectType.Single;
                        obj.value.Single = (float)primitiveValue;
                        break;
                    }
                case TypeCode.Double:
                    {
                        obj.type = ObjectType.Double;
                        obj.value.Double = (double)primitiveValue;
                        break;
                    }
            }
            throw new NotSupportedException("Cannot allocate a reference type. Use AllocTyped or AllocRef");
        }

        public static void Alloc(ref StackData obj, bool val)
        {
            obj.type = ObjectType.Int32;
            obj.value.Int32 = (val == true) ? 1 : 0;
        }

        public static void Alloc(ref StackData obj, sbyte val)
        {
            obj.type = ObjectType.Int8;
            obj.value.Int8 = val;
        }

        public static void Alloc(ref StackData obj, byte val)
        {
            obj.type = ObjectType.UInt8;
            obj.value.Int8 = (sbyte)val;
        }

        public static void Alloc(ref StackData obj, short val)
        {
            obj.type = ObjectType.Int16;
            obj.value.Int16 = val;
        }

        public static void Alloc(ref StackData obj, ushort val)
        {
            obj.type = ObjectType.UInt16;
            obj.value.Int16 = (short)val;
        }

        public static void Alloc(ref StackData obj, int val)
        {
            obj.type = ObjectType.Int32;
            obj.value.Int32 = val;
        }

        public static void Alloc(ref StackData obj, uint val)
        {
            obj.type = ObjectType.UInt32;
            obj.value.Int32 = (int)val;
        }

        public static void Alloc(ref StackData obj, long val)
        {
            obj.type = ObjectType.Int64;
            obj.value.Int64 = val;
        }

        public static void Alloc(ref StackData obj, ulong val)
        {
            obj.type = ObjectType.UInt64;
            obj.value.Int64 = (long)val;
        }

        public static void Alloc(ref StackData obj, float val)
        {
            obj.type = ObjectType.Single;
            obj.value.Single = val;
        }

        public static void Alloc(ref StackData obj, double val)
        {
            obj.type = ObjectType.Double;
            obj.value.Double = val;
        }

        public static void AllocRef(__heapallocator _heap, ref StackData obj, object val)
        {
            _heap.PinManagedObject(ref obj, val);
        }
    }
}
