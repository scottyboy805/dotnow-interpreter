using dotnow.Interop;
using dotnow.Reflection;
using dotnow.Runtime.CIL;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace dotnow.Runtime
{
    public enum StackType : uint
    {
        /// <summary>
        /// No valid data stored.
        /// </summary>
        Invalid = 0,

        /// <summary>
        /// Represents a signed 32-bit int stored on the stack (System.Int).
        /// Note that any type smaller than 32-bits is always promoted to I32 when pushing onto the stack (Minimum storable type).
        /// </summary>
        I32,
        /// <summary>
        /// Represents an unsigned 32-bit int stored on the stack (System.Unit).
        /// </summary>
        U32,
        /// <summary>
        /// Represents a signed 64-bit int stored on the stack (System.Long).
        /// </summary>
        I64,
        /// <summary>
        /// Represent an unsigned 64-bit int stored on the stack (System.ULong).
        /// </summary>
        U64,
        /// <summary>
        /// Represents a single precision floating point value stored on the stack (System.Single).
        /// </summary>
        F32,
        /// <summary>
        /// Represents a double precision floating point value stored on the stack (System.Double).
        /// </summary>
        F64,
        /// <summary>
        /// Represents a signed pointer stored on the stack (System.IntPtr).
        /// </summary>
        Ptr,
        /// <summary>
        /// Represents an unsigned pointer stored on the stack (System.UIntPtr).
        /// </summary>
        UPtr,
        /// <summary>
        /// Represents a reference or boxed value type.
        /// </summary>
        Ref,
        /// <summary>
        /// Represents an address to a by ref element, via a <see cref="IByRef"/>.
        /// </summary>
        ByRef,
    }

    [StructLayout(LayoutKind.Explicit)]
    internal struct StackData
    {
        // Public
        [FieldOffset(0)]
        public StackType Type;      // 8 bytes

        // Access to primitive data for common operations
        [FieldOffset(8)]
        public int I32;
        [FieldOffset(8)]
        public long I64;
        [FieldOffset(8)]
        public float F32;
        [FieldOffset(8)]
        public double F64;
        [FieldOffset(8)]
        public IntPtr Ptr;      // 16 bytes

        [FieldOffset(16)]
        public object Ref;      // 24 bytes        

        // Properties
        public int Address => Ref != null ? 1 : 0;
        public bool IsByRef => Type == StackType.ByRef && Ref is IByRef;

        // Methods
        public override string ToString()
        {
            return Type switch
            {
                StackType.I32 => $"I32, {I32}",
                StackType.U32 => $"U32, {(uint)I32}",
                StackType.I64 => $"I64, {I64}",
                StackType.U64 => $"U64, {(ulong)I64}",
                StackType.F32 => $"F32, {F32}",
                StackType.F64 => $"F64, {F64}",
                StackType.Ptr => $"Ptr, {Ptr}",
                StackType.UPtr => $"UPtr, {(UIntPtr)(ulong)Ptr}",
                StackType.Ref => $"Ref, {Ref}",
                StackType.ByRef => $"ByRef, {Ref}",
                _ => "Invalid",
            };
        }

        internal bool IsPrimitiveEqual(in StackData other)
        {
            // Check type
            if (Type != other.Type)
                return false;

            switch (Type)
            {
                case StackType.I32:
                case StackType.U32:
                    return I32 == other.I32;
                case StackType.I64:
                case StackType.U64:
                    return I64 == other.I64;
                case StackType.F32:
                    return F32 == other.F32;
                case StackType.F64:
                    return F64 == other.F64;
            }
            return false;
        }

        internal bool IsAnyEqual(in StackData other)
        {
            // Check type
            if (Type != other.Type)
                return false;

            switch(Type)
            {
                default: throw new NotSupportedException(Type.ToString()); // Cannot compare by ref

                case StackType.I32:
                case StackType.U32:
                    return I32 == other.I32;
                case StackType.I64:
                case StackType.U64:
                    return I64 == other.I64;
                case StackType.F32:
                    return F32 == other.F32;
                case StackType.F64:
                    return F64 == other.F64;
                case StackType.Ptr:
                case StackType.UPtr:
                    return Ptr == other.Ptr;
                case StackType.Ref:
                    return Ref == other.Ref;
            }
        }

        public static void Wrap(CILTypeInfo typeInfo, object obj, ref StackData dst)
        {
            // Check for CLR proxy - should be passed as derived instance
            if (obj is ICLRProxy proxy)
            {
                // Pass the actual instance for this case
                Wrap(typeInfo, proxy.Instance, ref dst);
                return;
            }

            // ### Handle by ref
            if (dst.IsByRef == true)
            {
                // Get the by ref object
                IByRef byRef = (IByRef)dst.Ref;

                switch (typeInfo.TypeCode)
                {
                    default: throw new NotSupportedException();

                    case TypeCode.Boolean:
                        {
                            byRef.SetValueI1(((bool)obj) == true ? (sbyte)1 : (sbyte)0);
                            break;
                        }
                    case TypeCode.Char:
                        {
                            byRef.SetValueI2((short)(char)obj);
                            break;
                        }

                    case TypeCode.SByte:
                        {
                            byRef.SetValueI1((sbyte)obj);
                            break;
                        }
                    case TypeCode.Byte:
                        {
                            byRef.SetValueI1((sbyte)(byte)obj);
                            break;
                        }
                    case TypeCode.Int16:
                        {
                            byRef.SetValueI2((short)obj);
                            break;
                        }
                    case TypeCode.UInt16:
                        {
                            byRef.SetValueI2((short)(ushort)obj);
                            break;
                        }
                    case TypeCode.Int32:
                        {
                            byRef.SetValueI4((int)obj);
                            break;
                        }
                    case TypeCode.UInt32:
                        {
                            byRef.SetValueI4((int)(uint)obj);
                            break;
                        }
                    case TypeCode.Int64:
                        {
                            byRef.SetValueI8((long)obj);
                            break;
                        }
                    case TypeCode.UInt64:
                        {
                            byRef.SetValueI8((long)(ulong)obj);
                            break;
                        }
                    case TypeCode.Single:
                        {
                            byRef.SetValueR4((float)obj);
                            break;
                        }
                    case TypeCode.Double:
                        {
                            byRef.SetValueR8((double)obj);
                            break;
                        }

                    case TypeCode.Decimal:
                    case TypeCode.String:
                    case TypeCode.Object:
                        {
                            byRef.SetValueRef(obj);
                            break;
                        }
                }
                return;
            }


            // ### Handle enum types
            // Check for interpreted enum or as enum
            if ((typeInfo.Flags & CILTypeFlags.Enum) != 0)
            {
                // Check for interpreted
                if ((typeInfo.Flags & CILTypeFlags.Interpreted) != 0)
                {
                    // Get object as enum
                    CLREnumInstance enumInstance = (CLREnumInstance)obj;

                    // Load into dst
                    dst = enumInstance.Value;
                    return;
                }
                // Must be interop
                else
                {
                    // Get the underlying type - bit slow but this is only when calling from reflection
                    Type enumType = typeInfo.Type.GetEnumUnderlyingType();

                    // Convert to underlying type
                    object underlyingValue = Convert.ChangeType(obj, enumType);

                    // Get the type code
                    TypeCode enumTypeCode = System.Type.GetTypeCode(enumType);

                    // Select type code
                    switch (enumTypeCode)
                    {
                        case TypeCode.SByte:
                            {
                                dst.I32 = (sbyte)underlyingValue;
                                dst.Type = StackType.I32;
                                break;
                            }
                        case TypeCode.Byte:
                            {
                                dst.I32 = (byte)underlyingValue;
                                dst.Type = StackType.I32;
                                break;
                            }
                        case TypeCode.Int16:
                            {
                                dst.I32 = (short)underlyingValue;
                                dst.Type = StackType.I32;
                                break;
                            }
                        case TypeCode.UInt16:
                            {
                                dst.I32 = (ushort)underlyingValue;
                                dst.Type = StackType.I32;
                                break;
                            }
                        case TypeCode.Int32:
                            {
                                dst.I32 = (int)underlyingValue;
                                dst.Type = StackType.I32;
                                break;
                            }
                        case TypeCode.UInt32:
                            {
                                dst.I32 = (int)(uint)underlyingValue;
                                dst.Type = StackType.U32;
                                break;
                            }
                        case TypeCode.Int64:
                            {
                                dst.I64 = (long)underlyingValue;
                                dst.Type = StackType.I64;
                                break;
                            }
                        case TypeCode.UInt64:
                            {
                                dst.I64 = (long)(ulong)underlyingValue;
                                dst.Type = StackType.U64;
                                break;
                            }
                        default:
                            throw new NotSupportedException(enumTypeCode.ToString());
                    }
                    return;
                }
            }


            // ### Handle other type
            switch (typeInfo.TypeCode)
            {
                default: throw new NotSupportedException();

                case TypeCode.Boolean:
                    {
                        dst.Type = StackType.I32;
                        dst.I32 = (bool)obj == true ? 1 : 0;
                        break;
                    }
                case TypeCode.Char:
                    {
                        dst.Type = StackType.I32;
                        dst.I32 = (char)obj;
                        break;
                    }

                case TypeCode.SByte:
                    {
                        dst.Type = StackType.I32;
                        dst.I32 = (sbyte)obj;
                        break;
                    }
                case TypeCode.Byte:
                    {
                        dst.Type = StackType.I32;
                        dst.I32 = (byte)obj;
                        break;
                    }
                case TypeCode.Int16:
                    {
                        dst.Type = StackType.I32;
                        dst.I32 = (short)obj;
                        break;
                    }
                case TypeCode.UInt16:
                    {
                        dst.Type = StackType.I32;
                        dst.I32 = (ushort)obj;
                        break;
                    }
                case TypeCode.Int32:
                    {
                        dst.Type = StackType.I32;
                        dst.I32 = (int)obj;
                        break;
                    }
                case TypeCode.UInt32:
                    {
                        dst.Type = StackType.U32;
                        dst.I32 = (int)(uint)obj;
                        break;
                    }
                case TypeCode.Int64:
                    {
                        dst.Type = StackType.I64;
                        dst.I64 = (long)obj;
                        break;
                    }
                case TypeCode.UInt64:
                    {
                        dst.Type = StackType.I64;
                        dst.I64 = (long)(ulong)obj;
                        break;
                    }
                case TypeCode.Single:
                    {
                        dst.Type = StackType.F32;
                        dst.F32 = (float)obj;
                        break;
                    }
                case TypeCode.Double:
                    {
                        dst.Type = StackType.F64;
                        dst.F64 = (double)obj;
                        break;
                    }

                case TypeCode.Decimal:
                case TypeCode.String:
                case TypeCode.Object:
                case TypeCode.DateTime:
                    {
                        dst.Type = StackType.Ref;
                        dst.Ref = obj;
                        break;
                    }
            }
        }

        public static void Unwrap(CILTypeInfo typeInfo, in StackData src, ref object unwrapped)
        {
            // ### Handle by ref
            if (src.IsByRef == true)
            {
                // Get the by ref object
                IByRef byRef = (IByRef)src.Ref;

                switch (typeInfo.TypeCode)
                {
                    default: throw new NotSupportedException(typeInfo.ToString());

                    case TypeCode.Boolean: unwrapped = byRef.GetValueI1() == 1 ? true : false; break;
                    case TypeCode.Char: unwrapped = (char)byRef.GetValueI2(); break;

                    case TypeCode.SByte: unwrapped = (sbyte)byRef.GetValueI1(); break;
                    case TypeCode.Byte: unwrapped = (byte)byRef.GetValueU1(); break;
                    case TypeCode.Int16: unwrapped = (short)byRef.GetValueI2(); break;
                    case TypeCode.UInt16: unwrapped = (ushort)byRef.GetValueU2(); break;
                    case TypeCode.Int32: unwrapped = (int)byRef.GetValueI4(); break;
                    case TypeCode.UInt32: unwrapped = (uint)byRef.GetValueU4(); break;
                    case TypeCode.Int64: unwrapped = (long)byRef.GetValueI8(); break;
                    case TypeCode.UInt64: unwrapped = (ulong)byRef.GetValueU8(); break;
                    case TypeCode.Single: unwrapped = (float)byRef.GetValueR4(); break;
                    case TypeCode.Double: unwrapped = (double)byRef.GetValueR8(); break;
                    case TypeCode.Decimal:
                    case TypeCode.String:
                    case TypeCode.Object:
                    case TypeCode.DateTime:
                        {
                            unwrapped = byRef.GetValueRef();
                            break;
                        }
                }
            }
            else
            {
                // ### Handle enums
                // Check for interpreted enum or as enum
                if ((typeInfo.Flags & CILTypeFlags.Enum) != 0)
                {
                    // Check for interpreted
                    if ((typeInfo.Flags & CILTypeFlags.Interpreted) != 0)
                    {
                        // Create the enum object
                        unwrapped = new CLREnumInstance(typeInfo, src);
                        return;
                    }
                    // Must be interop
                    else
                    {
                        // Get the enum type code
                        TypeCode enumTypeCode = System.Type.GetTypeCode(typeInfo.Type.GetEnumUnderlyingType());

                        // Select type code
                        switch (enumTypeCode)
                        {
                            case TypeCode.SByte: unwrapped = Enum.ToObject(typeInfo.Type, (sbyte)src.I32); break;
                            case TypeCode.Byte: unwrapped = Enum.ToObject(typeInfo.Type, (byte)src.I32); break;
                            case TypeCode.Int16: unwrapped = Enum.ToObject(typeInfo.Type, (short)src.I32); break;
                            case TypeCode.UInt16: unwrapped = Enum.ToObject(typeInfo.Type, (ushort)src.I32); break;
                            case TypeCode.Int32: unwrapped = Enum.ToObject(typeInfo.Type, (int)src.I32); break;
                            case TypeCode.UInt32: unwrapped = Enum.ToObject(typeInfo.Type, (uint)src.I32); break;
                            case TypeCode.Int64: unwrapped = Enum.ToObject(typeInfo.Type, (long)src.I64); break;
                            case TypeCode.UInt64: unwrapped = Enum.ToObject(typeInfo.Type, (ulong)src.I64); break;
                            default:
                                throw new NotSupportedException(enumTypeCode.ToString());
                        }
                        return;
                    }
                }


                // ### Handle all other types
                switch (typeInfo.TypeCode)
                {
                    default: throw new NotSupportedException(typeInfo.ToString());

                    case TypeCode.Boolean: unwrapped = src.I32 == 1 ? true : false; break;
                    case TypeCode.Char: unwrapped = (char)src.I32; break;

                    case TypeCode.SByte: unwrapped = (sbyte)src.I32; break;
                    case TypeCode.Byte: unwrapped = (byte)src.I32; break;
                    case TypeCode.Int16: unwrapped = (short)src.I32; break;
                    case TypeCode.UInt16: unwrapped = (ushort)src.I32; break;
                    case TypeCode.Int32: unwrapped = (int)src.I32; break;
                    case TypeCode.UInt32: unwrapped = (uint)src.I32; break;
                    case TypeCode.Int64: unwrapped = (long)src.I64; break;
                    case TypeCode.UInt64: unwrapped = (ulong)src.I64; break;
                    case TypeCode.Single: unwrapped = (float)src.F32; break;
                    case TypeCode.Double: unwrapped = (double)src.F64; break;
                    case TypeCode.Decimal:
                    case TypeCode.String:
                    case TypeCode.Object:
                    case TypeCode.DateTime:
                        {
                            unwrapped = src.Ref;
                            break;
                        }
                }
            }

            // Check for ICLRInstance - should be passed as base type
            if (unwrapped is ICLRInstance clrInstance)
            {
                // Try to unbox as best suited base interop type
                unwrapped = clrInstance.UnwrapAsType(typeInfo.Type);
            }
            // Check for CLR type - should be passed as base System.Type
            else if (unwrapped is CLRType clrType)
            {
                // Select base type
                Type current = clrType.BaseType;

                // Keep descending until we reach an interop type
                while (current.IsCLRType() == true)
                    current = current.BaseType;

                // Update unwrapped
                unwrapped = current;

                // Log implicit marshalling of type - it can cause strange behaviour if not expected, but can lead to a crash if no marshalling occurs
                Debug.LineFormat("CLRType '{0}' was marshalled to interop base type '{1}'", clrType, current);
            }
        }

        public static bool TryUnwrapAs<T>(in StackData src, ref T unwrapped) where T : struct
        {
            try
            {
                // Get the type code of T to determine how to extract the value
                TypeCode typeCode = System.Type.GetTypeCode(typeof(T));

                switch (typeCode)
                {
                    case TypeCode.Boolean:
                        {
                            bool value = src.I32 == 1;
                            unwrapped = Unsafe.As<bool, T>(ref value);
                            break;
                        }
                    case TypeCode.Char:
                        {
                            char value = (char)src.I32;
                            unwrapped = Unsafe.As<char, T>(ref value);
                            break;
                        }
                    case TypeCode.SByte:
                        {
                            sbyte value = (sbyte)src.I32;
                            unwrapped = Unsafe.As<sbyte, T>(ref value);
                            break;
                        }
                    case TypeCode.Byte:
                        {
                            byte value = (byte)src.I32;
                            unwrapped = Unsafe.As<byte, T>(ref value);
                            break;
                        }
                    case TypeCode.Int16:
                        {
                            short value = (short)src.I32;
                            unwrapped = Unsafe.As<short, T>(ref value);
                            break;
                        }
                    case TypeCode.UInt16:
                        {
                            ushort value = (ushort)src.I32;
                            unwrapped = Unsafe.As<ushort, T>(ref value);
                            break;
                        }
                    case TypeCode.Int32:
                        {
                            int value = src.I32;
                            unwrapped = Unsafe.As<int, T>(ref value);
                            break;
                        }
                    case TypeCode.UInt32:
                        {
                            uint value = (uint)src.I32;
                            unwrapped = Unsafe.As<uint, T>(ref value);
                            break;
                        }
                    case TypeCode.Int64:
                        {
                            long value = src.I64;
                            unwrapped = Unsafe.As<long, T>(ref value);
                            break;
                        }
                    case TypeCode.UInt64:
                        {
                            ulong value = (ulong)src.I64;
                            unwrapped = Unsafe.As<ulong, T>(ref value);
                            break;
                        }
                    case TypeCode.Single:
                        {
                            float value = src.F32;
                            unwrapped = Unsafe.As<float, T>(ref value);
                            break;
                        }
                    case TypeCode.Double:
                        {
                            double value = src.F64;
                            unwrapped = Unsafe.As<double, T>(ref value);
                            break;
                        }
                    case TypeCode.Decimal:
                    case TypeCode.Object:
                        {
                            // Try to convert
                            unwrapped = (T)src.Ref;
                            break;
                        }
                    default:
                        {
                            // For non-primitive types or unsupported types, set to default
                            unwrapped = default;
                            return false;
                        }
                }
            }
            catch
            {
                // On any error, set to default value
                unwrapped = default;
                return false;
            }
            return true;
        }

        public static void Box(CILTypeInfo typeInfo, ref StackData obj)
        {
            // ### Handle enums
            // Check for interpreted enum or as enum
            if ((typeInfo.Flags & CILTypeFlags.Enum) != 0)
            {
                // Check for interpreted
                if ((typeInfo.Flags & CILTypeFlags.Interpreted) != 0)
                {
                    // Create the enum object
                    obj.Ref = new CLREnumInstance(typeInfo, obj);
                    return;
                }
                // Must be interop
                else
                {
                    // Get the enum type code
                    TypeCode enumTypeCode = System.Type.GetTypeCode(typeInfo.Type.GetEnumUnderlyingType());

                    // Select type code
                    switch (enumTypeCode)
                    {
                        case TypeCode.SByte: obj.Ref = Enum.ToObject(typeInfo.Type, (sbyte)obj.I32); break;
                        case TypeCode.Byte: obj.Ref = Enum.ToObject(typeInfo.Type, (byte)obj.I32); break;
                        case TypeCode.Int16: obj.Ref = Enum.ToObject(typeInfo.Type, (short)obj.I32); break;
                        case TypeCode.UInt16: obj.Ref = Enum.ToObject(typeInfo.Type, (ushort)obj.I32); break;
                        case TypeCode.Int32: obj.Ref = Enum.ToObject(typeInfo.Type, (int)obj.I32); break;
                        case TypeCode.UInt32: obj.Ref = Enum.ToObject(typeInfo.Type, (uint)obj.I32); break;
                        case TypeCode.Int64: obj.Ref = Enum.ToObject(typeInfo.Type, (long)obj.I64); break;
                        case TypeCode.UInt64: obj.Ref = Enum.ToObject(typeInfo.Type, (ulong)obj.I64); break;
                        default:
                            throw new NotSupportedException(enumTypeCode.ToString());
                    }
                    return;
                }
            }

            switch (typeInfo.TypeCode)
            {
                default: throw new NotSupportedException(typeInfo.ToString());

                case TypeCode.Boolean: obj.Ref = (bool)(obj.I32 == 1); break;
                case TypeCode.Char: obj.Ref = (char)obj.I32; break;
                case TypeCode.SByte: obj.Ref = (sbyte)obj.I32; break;
                case TypeCode.Byte: obj.Ref = (byte)obj.I32; break;
                case TypeCode.Int16: obj.Ref = (short)obj.I32; break;
                case TypeCode.UInt16: obj.Ref = (ushort)obj.I32; break;
                case TypeCode.Int32: obj.Ref = (int)obj.I32; break;
                case TypeCode.UInt32: obj.Ref = (uint)obj.I32; break;
                case TypeCode.Int64: obj.Ref = (long)obj.I64; break;
                case TypeCode.UInt64: obj.Ref = (ulong)obj.I64; break;
                case TypeCode.Single: obj.Ref = (float)obj.F32; break;
                case TypeCode.Double: obj.Ref = (double)obj.F64; break;
                case TypeCode.Decimal:
                case TypeCode.Object:
                    {
                        // Do nothing for objects, even value types are already boxed
                        break;
                    }
            }

            // Check to reference now that it is boxed
            obj.Type = StackType.Ref;
        }

        public static void Unbox(CILTypeInfo typeInfo, ref StackData obj)
        {
            // ### Handle enums
            // Check for interpreted enum or as enum
            if ((typeInfo.Flags & CILTypeFlags.Enum) != 0)
            {
                // Check for interpreted
                if ((typeInfo.Flags & CILTypeFlags.Interpreted) != 0)
                {
                    // Convert back to stack object
                    obj = ((CLREnumInstance)obj.Ref).Value;
                    return;
                }
                // Must be interop
                else
                {
                    // Get the enum type code
                    TypeCode enumTypeCode = System.Type.GetTypeCode(typeInfo.Type.GetEnumUnderlyingType());

                    // Select type code
                    switch (enumTypeCode)
                    {
                        case TypeCode.SByte:
                            {
                                obj.I32 = (sbyte)obj.Ref;
                                obj.Type = StackType.I32;
                                break;
                            }
                        case TypeCode.Byte:
                            {
                                obj.I32 = (byte)obj.Ref;
                                obj.Type = StackType.I32;
                                break;
                            }
                        case TypeCode.Int16:
                            {
                                obj.I32 = (short)obj.Ref;
                                obj.Type = StackType.I32;
                                break;
                            }
                        case TypeCode.UInt16:
                            {
                                obj.I32 = (ushort)obj.Ref;
                                obj.Type = StackType.I32;
                                break;
                            }
                        case TypeCode.Int32: 
                            {
                                obj.I32 = (int)obj.Ref;
                                obj.Type = StackType.I32;
                                break;
                            }
                        case TypeCode.UInt32:
                            {
                                obj.I32 = (int)(uint)obj.Ref;
                                obj.Type = StackType.U32;
                                break;
                            }
                        case TypeCode.Int64:
                            {
                                obj.I64 = (long)obj.Ref;
                                obj.Type = StackType.I64;
                                break;
                            }
                        case TypeCode.UInt64:
                            {
                                obj.I64 = (long)(ulong)obj.Ref;
                                obj.Type = StackType.I64;
                                break;
                            }
                        default:
                            throw new NotSupportedException(enumTypeCode.ToString());
                    }
                    return;
                }
            }

            switch (typeInfo.TypeCode)
            {
                default: throw new NotSupportedException(typeInfo.ToString());
                     
                case TypeCode.Boolean:
                    {
                        obj.I32 = ((bool)obj.Ref) == true ? 1 : 0;
                        obj.Type = StackType.I32;
                        break;
                    }
                case TypeCode.Char:
                    {
                        obj.I32 = (char)obj.Ref;
                        obj.Type = StackType.I32;
                        break;
                    }
                case TypeCode.SByte:
                    {
                        obj.I32 = (sbyte)obj.Ref;
                        obj.Type = StackType.I32;
                        break;
                    }
                case TypeCode.Byte:
                    {
                        obj.I32 = (byte)obj.Ref;
                        obj.Type = StackType.I32;
                        break;
                    }
                case TypeCode.Int16:
                    {
                        obj.I32 = (short)obj.Ref;
                        obj.Type = StackType.I32;
                        break;
                    }
                case TypeCode.UInt16:
                    {
                        obj.I32 = (ushort)obj.Ref;
                        obj.Type = StackType.I32;
                        break;
                    }
                case TypeCode.Int32:
                    {
                        obj.I32 = (int)obj.Ref;
                        obj.Type = StackType.I32;
                        break;
                    }
                case TypeCode.UInt32:
                    {
                        obj.I32 = (int)(uint)obj.Ref;
                        obj.Type = StackType.U32;
                        break;
                    }
                case TypeCode.Int64:
                    {
                        obj.I64 = (long)obj.Ref;
                        obj.Type = StackType.I64;
                        break;
                    }
                case TypeCode.UInt64:
                    {
                        obj.I64 = (long)(ulong)obj.Ref;
                        obj.Type = StackType.U64;
                        break;
                    }
                case TypeCode.Single:
                    {
                        obj.F32 = (float)obj.Ref;
                        obj.Type = StackType.F32;
                        break;
                    }
                case TypeCode.Double:
                    {
                        obj.F64 = (double)obj.Ref;
                        obj.Type = StackType.F64;
                        break;
                    }
                case TypeCode.Decimal:
                case TypeCode.Object:
                    {
                        // Do nothing for objects, even value types are already boxed
                        break;
                    }
            }
        }

        public static StackData Default(AppDomain domain, CILTypeInfo typeInfo)
        {
            // Load default into slot
            StackData dst = default;
            Default(domain, typeInfo, ref dst);

            return dst;
        }

        public static void Default(AppDomain domain, CILTypeInfo typeInfo, ref StackData dst)
        {
            // ### Handle by ref
            if (dst.IsByRef == true)
            {
                // Get the by ref object
                IByRef byRef = (IByRef)dst.Ref;

                switch (typeInfo.TypeCode)
                {
                    default: throw new NotSupportedException();

                    case TypeCode.Boolean:
                        {
                            byRef.SetValueI1(default);
                            break;
                        }
                    case TypeCode.Char:
                        {
                            byRef.SetValueI2(default);
                            break;
                        }

                    case TypeCode.SByte:
                        {
                            byRef.SetValueI1(default);
                            break;
                        }
                    case TypeCode.Byte:
                        {
                            byRef.SetValueI1(default);
                            break;
                        }
                    case TypeCode.Int16:
                        {
                            byRef.SetValueI2(default);
                            break;
                        }
                    case TypeCode.UInt16:
                        {
                            byRef.SetValueI2(default);
                            break;
                        }
                    case TypeCode.Int32:
                        {
                            byRef.SetValueI4(default);
                            break;
                        }
                    case TypeCode.UInt32:
                        {
                            byRef.SetValueI4(default);
                            break;
                        }
                    case TypeCode.Int64:
                        {
                            byRef.SetValueI8(default);
                            break;
                        }
                    case TypeCode.UInt64:
                        {
                            byRef.SetValueI8(default);
                            break;
                        }
                    case TypeCode.Single:
                        {
                            byRef.SetValueR4(default);
                            break;
                        }
                    case TypeCode.Double:
                        {
                            byRef.SetValueR8(default);
                            break;
                        }

                    case TypeCode.Decimal:
                    case TypeCode.String:
                    case TypeCode.Object:
                        {
                            // Check for value type
                            if ((typeInfo.Flags & CILTypeFlags.ValueType) != 0)
                            {
                                if ((typeInfo.Flags & CILTypeFlags.Interop) != 0)
                                {
                                    // Create default instance
                                    byRef.SetValueRef(FormatterServices.GetUninitializedObject(typeInfo.Type));
                                }
                                else
                                {
                                    // Create default value type instance
                                    byRef.SetValueRef(CLRValueTypeInstance.CreateInstance(domain, typeInfo));
                                }
                            }
                            else
                            {
                                byRef.SetValueRef(null);
                            }
                            break;
                        }
                }
                return;
            }


            // Handle simple case
            switch (typeInfo.TypeCode)
            {
                default: throw new NotSupportedException(typeInfo.ToString());

                case TypeCode.Boolean:
                    {
                        dst.Type = StackType.I32;
                        dst.I32 = default(bool) ? 1 : 0;
                        break;
                    }
                case TypeCode.Char:
                    {
                        dst.Type = StackType.I32;
                        dst.I32 = default(char);
                        break;
                    }

                case TypeCode.SByte:
                    {
                        dst.Type = StackType.I32;
                        dst.I32 = default(sbyte);
                        break;
                    }
                case TypeCode.Byte:
                    {
                        dst.Type = StackType.I32;
                        dst.I32 = default(byte);
                        break;
                    }
                case TypeCode.Int16:
                    {
                        dst.Type = StackType.I32;
                        dst.I32 = default(short);
                        break;
                    }
                case TypeCode.UInt16:
                    {
                        dst.Type = StackType.I32;
                        dst.I32 = default(ushort);
                        break;
                    }
                case TypeCode.Int32:
                    {
                        dst.Type = StackType.I32;
                        dst.I32 = default(int);
                        break;
                    }
                case TypeCode.UInt32:
                    {
                        dst.Type = StackType.U32;
                        dst.I32 = (int)default(uint);
                        break;
                    }
                case TypeCode.Int64:
                    {
                        dst.Type = StackType.I64;
                        dst.I64 = default(long);
                        break;
                    }
                case TypeCode.UInt64:
                    {
                        dst.Type = StackType.I64;
                        dst.I64 = (long)default(ulong);
                        break;
                    }
                case TypeCode.Single:
                    {
                        dst.Type = StackType.F32;
                        dst.F32 = default(float);
                        break;
                    }
                case TypeCode.Double:
                    {
                        dst.Type = StackType.F64;
                        dst.F64 = default(double);
                        break;
                    }
                case TypeCode.Decimal:
                    {
                        dst.Type = StackType.Ref;
                        dst.Ref = default(decimal);
                        break;
                    }
                case TypeCode.String:
                    {
                        dst.Type = StackType.Ref;
                        dst.Ref = default(string);
                        break;
                    }
                case TypeCode.Object:
                    {
                        // Check for value type
                        if ((typeInfo.Flags & CILTypeFlags.ValueType) != 0)
                        {
                            if ((typeInfo.Flags & CILTypeFlags.Interop) != 0)
                            {
                                // Check for nullable
                                if ((typeInfo.Flags & CILTypeFlags.NullableType) != 0)
                                {
                                    // Create default instance
                                    Type nullableType = Nullable.GetUnderlyingType(typeInfo.Type);

                                    // Get the default underlying value
                                    object defaultNullable = FormatterServices.GetUninitializedObject(nullableType);

                                    // Create instance with default value
                                    dst.Ref = Activator.CreateInstance(typeInfo.Type, new[] { defaultNullable });
                                }
                                else
                                {
                                    // Create default instance
                                    dst.Ref = FormatterServices.GetUninitializedObject(typeInfo.Type);
                                }
                            }
                            else
                            {
                                // Create default value type instance
                                dst.Ref = (CLRValueTypeInstance.CreateInstance(domain, typeInfo));
                            }
                        }
                        else
                        {
                            dst.Ref = null;
                        }
                        dst.Type = StackType.Ref;
                        break;
                    }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void Copy(CILTypeInfo typeInfo, in StackData src, ref StackData dst)
        {
            // Check for value type but not a primitive
            if ((typeInfo.Flags & CILTypeFlags.ValueType) != 0
                && (typeInfo.Flags & CILTypeFlags.PrimitiveType) == 0
                && src.Type == StackType.Ref)
            {
                // Check for clr value type
                if ((typeInfo.Flags & CILTypeFlags.Interpreted) != 0)
                {
                    // Perform value type copy for interpreted struct
                    CLRValueTypeInstance valueSrc = (CLRValueTypeInstance)src.Ref;

                    // Perform copy
                    dst.Ref = valueSrc.Copy(typeInfo);
                    dst.Type = StackType.Ref;
                }
                else
                {
                    // Perform value type copy
                    dst.Ref = __marshal.CopyInteropBoxedValueTypeSlow(typeInfo.Type, src.Ref);
                    dst.Type = StackType.Ref;
                }
            }
            else
            {
                // Simply copy is fine
                dst = src;
            }
        }
    }
}
