using dotnow.Interop;
using dotnow.Reflection;
using System;
using System.Runtime.InteropServices;

namespace dotnow.Runtime
{
    public enum StackTypeCode : uint
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
        IntPtr,
        /// <summary>
        /// Represents an unsigned pointer stored on the stack (System.UIntPtr).
        /// </summary>
        UIntPtr,

        Ref,
        ByRef,
    }

    [StructLayout(LayoutKind.Explicit)]
    internal struct StackData
    {
        // Public
        [FieldOffset(0)]
        public StackTypeCode Type;      // 8 bytes

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
        public IntPtr Ptr;
        [FieldOffset(8)]
        public object Ref; // 16 bytes        

        // Methods
        public static void Wrap(CILTypeInfo typeInfo, object obj, ref StackData dst)
        {
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
                    Type enumType = typeInfo.Meta.GetEnumUnderlyingType();

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
                                dst.Type = StackTypeCode.I32;
                                break;
                            }
                        case TypeCode.Byte:
                            {
                                dst.I32 = (sbyte)underlyingValue;
                                dst.Type = StackTypeCode.I32;
                                break;
                            }
                        case TypeCode.Int16:
                            {
                                dst.I32 = (sbyte)underlyingValue;
                                dst.Type = StackTypeCode.I32;
                                break;
                            }
                        case TypeCode.UInt16:
                            {
                                dst.I32 = (sbyte)underlyingValue;
                                dst.Type = StackTypeCode.I32;
                                break;
                            }
                        case TypeCode.Int32:
                            {
                                dst.I32 = (sbyte)underlyingValue;
                                dst.Type = StackTypeCode.I32;
                                break;
                            }
                        case TypeCode.UInt32:
                            {
                                dst.I32 = (sbyte)underlyingValue;
                                dst.Type = StackTypeCode.U32;
                                break;
                            }
                        case TypeCode.Int64:
                            {
                                dst.I32 = (sbyte)underlyingValue;
                                dst.Type = StackTypeCode.I64;
                                break;
                            }
                        case TypeCode.UInt64:
                            {
                                dst.I32 = (sbyte)underlyingValue;
                                dst.Type = StackTypeCode.U64;
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
                        dst.Type = StackTypeCode.I32;
                        dst.I32 = (bool)obj == true ? 1 : 0;
                        break;
                    }
                case TypeCode.Char:
                    {
                        dst.Type = StackTypeCode.I32;
                        dst.I32 = (char)obj;
                        break;
                    }

                case TypeCode.SByte:
                    {
                        dst.Type = StackTypeCode.I32;
                        dst.I32 = (sbyte)obj;
                        break;
                    }
                case TypeCode.Byte:
                    {
                        dst.Type = StackTypeCode.I32;
                        dst.I32 = (byte)obj;
                        break;
                    }
                case TypeCode.Int16:
                    {
                        dst.Type = StackTypeCode.I32;
                        dst.I32 = (short)obj;
                        break;
                    }
                case TypeCode.UInt16:
                    {
                        dst.Type = StackTypeCode.I32;
                        dst.I32 = (ushort)obj;
                        break;
                    }
                case TypeCode.Int32:
                    {
                        dst.Type = StackTypeCode.I32;
                        dst.I32 = (int)obj;
                        break;
                    }
                case TypeCode.UInt32:
                    {
                        dst.Type = StackTypeCode.U32;
                        dst.I32 = (int)(uint)obj;
                        break;
                    }
                case TypeCode.Int64:
                    {
                        dst.Type = StackTypeCode.I64;
                        dst.I64 = (long)obj;
                        break;
                    }
                case TypeCode.UInt64:
                    {
                        dst.Type = StackTypeCode.I64;
                        dst.I64 = (long)(ulong)obj;
                        break;
                    }
                case TypeCode.Single:
                    {
                        dst.Type = StackTypeCode.F32;
                        dst.F32 = (float)obj;
                        break;
                    }
                case TypeCode.Double:
                    {
                        dst.Type = StackTypeCode.F64;
                        dst.F64 = (double)obj;
                        break;
                    }

                case TypeCode.Decimal:
                case TypeCode.String:
                case TypeCode.Object:
                    {
                        dst.Type = StackTypeCode.Ref;
                        dst.Ref = obj;
                        break;
                    }
            }
        }

        public static void Unwrap(CILTypeInfo typeInfo, ref StackData src, ref object unwrapped)
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
                    TypeCode enumTypeCode = System.Type.GetTypeCode(typeInfo.Meta.GetEnumUnderlyingType());

                    // Select type code
                    switch (enumTypeCode)
                    {
                        case TypeCode.SByte: unwrapped = Enum.ToObject(typeInfo.Meta, (sbyte)src.I32); break;
                        case TypeCode.Byte: unwrapped = Enum.ToObject(typeInfo.Meta, (byte)src.I32); break;
                        case TypeCode.Int16: unwrapped = Enum.ToObject(typeInfo.Meta, (short)src.I32); break;
                        case TypeCode.UInt16: unwrapped = Enum.ToObject(typeInfo.Meta, (ushort)src.I32); break;
                        case TypeCode.Int32: unwrapped = Enum.ToObject(typeInfo.Meta, (int)src.I32); break;
                        case TypeCode.UInt32: unwrapped = Enum.ToObject(typeInfo.Meta, (uint)src.I32); break;
                        case TypeCode.Int64: unwrapped = Enum.ToObject(typeInfo.Meta, (long)src.I32); break;
                        case TypeCode.UInt64: unwrapped = Enum.ToObject(typeInfo.Meta, (ulong)src.I32); break;
                        default:
                            throw new NotSupportedException(enumTypeCode.ToString());
                    }
                    return;
                }
            }


            // ### Handle all other types
            switch (typeInfo.TypeCode)
            {
                default: throw new NotSupportedException();

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
                    {
                        unwrapped = src.Ref;
                        break;
                    }
            }

            // Check for ICLRInstance - should be passed as base type
            if (unwrapped is ICLRInstance clrInstance)
            {
                // Try to unbox as best suited base interop type
                unwrapped = clrInstance.UnwrapAsType(typeInfo.Meta);
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
    }
}
