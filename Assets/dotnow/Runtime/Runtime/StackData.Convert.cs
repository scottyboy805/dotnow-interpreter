using System;

namespace dotnow.Runtime
{
    internal static class RuntimeConvert
    {
        // Methods
        public static void ToInt8(ref StackData obj)
        {
            switch(obj.type)
            {
                // Error case
                default: throw new InvalidCastException();

                // No conversion required (invalid IL code??)
                case StackData.ObjectType.Int8: return;
                case StackData.ObjectType.UInt8: break; // implicit conversion

                case StackData.ObjectType.Int16:
                case StackData.ObjectType.UInt16: obj.value.Int8 = (sbyte)obj.value.Int16; break;

                case StackData.ObjectType.Int32:
                case StackData.ObjectType.UInt32: obj.value.Int8 = (sbyte)obj.value.Int32; break;

                case StackData.ObjectType.Int64:
                case StackData.ObjectType.UInt64: obj.value.Int8 = (sbyte)obj.value.Int64; break;

                case StackData.ObjectType.Single: obj.value.Int8 = (sbyte)obj.value.Single; break;
                case StackData.ObjectType.Double: obj.value.Int8 = (sbyte)obj.value.Double; break;

                case StackData.ObjectType.RefBoxed: obj.value.Int8 = (sbyte)obj.UnboxAsType(TypeCode.SByte); break;
            }

            obj.type = StackData.ObjectType.Int8;
        }

        public static void ToInt8Promote(ref StackData obj)
        {
            switch (obj.type)
            {
                // Error case
                default: throw new InvalidCastException();

                // No conversion required (invalid IL code??)
                case StackData.ObjectType.Int8: return;
                case StackData.ObjectType.UInt8: obj.value.Int32 = (int)(sbyte)obj.value.Int8; break; // implicit conversion

                case StackData.ObjectType.Int16:
                case StackData.ObjectType.UInt16: obj.value.Int32 = (int)(sbyte)obj.value.Int16; break;

                case StackData.ObjectType.Int32:
                case StackData.ObjectType.UInt32: obj.value.Int32 = (int)(sbyte)obj.value.Int32; break;

                case StackData.ObjectType.Int64:
                case StackData.ObjectType.UInt64: obj.value.Int32 = (int)(sbyte)obj.value.Int64; break;

                case StackData.ObjectType.Single: obj.value.Int32 = (int)(sbyte)obj.value.Single; break;
                case StackData.ObjectType.Double: obj.value.Int32 = (int)(sbyte)obj.value.Double; break;

                case StackData.ObjectType.RefBoxed: obj.value.Int32 = (int)(sbyte)obj.UnboxAsType(TypeCode.SByte); break;
            }

            obj.type = StackData.ObjectType.Int32;
        }

        public static void ToInt8Checked(ref StackData obj)
        {
            checked
            {
                switch (obj.type)
                {
                    // Error case
                    default: throw new InvalidCastException();

                    // No conversion required (invalid IL code??)
                    case StackData.ObjectType.Int8: return;
                    case StackData.ObjectType.UInt8: break; // implicit conversion

                    case StackData.ObjectType.Int16:
                    case StackData.ObjectType.UInt16: obj.value.Int8 = (sbyte)obj.value.Int16; break;

                    case StackData.ObjectType.Int32:
                    case StackData.ObjectType.UInt32: obj.value.Int8 = (sbyte)obj.value.Int32; break;

                    case StackData.ObjectType.Int64:
                    case StackData.ObjectType.UInt64: obj.value.Int8 = (sbyte)obj.value.Int64; break;

                    case StackData.ObjectType.Single: obj.value.Int8 = (sbyte)obj.value.Single; break;
                    case StackData.ObjectType.Double: obj.value.Int8 = (sbyte)obj.value.Double; break;

                    case StackData.ObjectType.RefBoxed: obj.value.Int8 = (sbyte)obj.UnboxAsType(TypeCode.SByte); break;
                }
            }

            obj.type = StackData.ObjectType.Int8;
        }

        public static void ToInt8CheckedPromote(ref StackData obj)
        {
            checked
            {
                switch (obj.type)
                {
                    // Error case
                    default: throw new InvalidCastException();

                    // No conversion required (invalid IL code??)
                    case StackData.ObjectType.Int8: return;
                    case StackData.ObjectType.UInt8: obj.value.Int32 = (int)obj.value.Int8; break; // implicit conversion

                    case StackData.ObjectType.Int16:
                    case StackData.ObjectType.UInt16: obj.value.Int32 = (int)(sbyte)obj.value.Int16; break;

                    case StackData.ObjectType.Int32:
                    case StackData.ObjectType.UInt32: obj.value.Int32 = (int)(sbyte)obj.value.Int32; break;

                    case StackData.ObjectType.Int64:
                    case StackData.ObjectType.UInt64: obj.value.Int32 = (int)(sbyte)obj.value.Int64; break;

                    case StackData.ObjectType.Single: obj.value.Int32 = (int)(sbyte)obj.value.Single; break;
                    case StackData.ObjectType.Double: obj.value.Int32 = (int)(sbyte)obj.value.Double; break;

                    case StackData.ObjectType.RefBoxed: obj.value.Int32 = (int)(sbyte)obj.UnboxAsType(TypeCode.SByte); break;
                }
            }

            obj.type = StackData.ObjectType.Int32;
        }

        public static void ToInt16(ref StackData obj)
        {
            switch (obj.type)
            {
                // Error case
                default: throw new InvalidCastException();

                // No conversion required (invalid IL code??)
                case StackData.ObjectType.Int16: return;
                case StackData.ObjectType.UInt16: break; // implicit conversion

                case StackData.ObjectType.Int8:
                case StackData.ObjectType.UInt8: obj.value.Int16 = (short)obj.value.Int8; break;

                case StackData.ObjectType.Int32:
                case StackData.ObjectType.UInt32: obj.value.Int16 = (short)obj.value.Int32; break;

                case StackData.ObjectType.Int64:
                case StackData.ObjectType.UInt64: obj.value.Int16 = (short)obj.value.Int64; break;

                case StackData.ObjectType.Single: obj.value.Int16 = (short)obj.value.Single; break;
                case StackData.ObjectType.Double: obj.value.Int16 = (short)obj.value.Double; break;

                case StackData.ObjectType.RefBoxed: obj.value.Int16 = (short)obj.UnboxAsType(TypeCode.Int16); break;
            }

            obj.type = StackData.ObjectType.Int16;
        }

        public static void ToInt16Promote(ref StackData obj)
        {
            switch (obj.type)
            {
                // Error case
                default: throw new InvalidCastException();

                // No conversion required (invalid IL code??)
                case StackData.ObjectType.Int16: return;
                case StackData.ObjectType.UInt16: obj.value.Int32 = (int)(short)obj.value.Int16; break; // implicit conversion

                case StackData.ObjectType.Int8:
                case StackData.ObjectType.UInt8: obj.value.Int32 = (int)(short)obj.value.Int8; break;

                case StackData.ObjectType.Int32:
                case StackData.ObjectType.UInt32: obj.value.Int32 = (int)(short)obj.value.Int32; break;

                case StackData.ObjectType.Int64:
                case StackData.ObjectType.UInt64: obj.value.Int32 = (int)(short)obj.value.Int64; break;

                case StackData.ObjectType.Single: obj.value.Int32 = (int)(short)obj.value.Single; break;
                case StackData.ObjectType.Double: obj.value.Int32 = (int)(short)obj.value.Double; break;

                case StackData.ObjectType.RefBoxed: obj.value.Int32 = (int)(short)obj.UnboxAsType(TypeCode.Int16); break;
            }

            obj.type = StackData.ObjectType.Int32;
        }

        public static void ToInt16Checked(ref StackData obj)
        {
            checked
            {
                switch (obj.type)
                {
                    // Error case
                    default: throw new InvalidCastException();

                    // No conversion required (invalid IL code??)
                    case StackData.ObjectType.Int16: return;
                    case StackData.ObjectType.UInt16: break; // implicit conversion

                    case StackData.ObjectType.Int8:
                    case StackData.ObjectType.UInt8: obj.value.Int16 = (short)obj.value.Int8; break;

                    case StackData.ObjectType.Int32:
                    case StackData.ObjectType.UInt32: obj.value.Int16 = (short)obj.value.Int32; break;

                    case StackData.ObjectType.Int64:
                    case StackData.ObjectType.UInt64: obj.value.Int16 = (short)obj.value.Int64; break;

                    case StackData.ObjectType.Single: obj.value.Int16 = (short)obj.value.Single; break;
                    case StackData.ObjectType.Double: obj.value.Int16 = (short)obj.value.Double; break;

                    case StackData.ObjectType.RefBoxed: obj.value.Int16 = (short)obj.UnboxAsType(TypeCode.Int16); break;
                }
            }

            obj.type = StackData.ObjectType.Int16;
        }

        public static void ToInt16CheckedPromote(ref StackData obj)
        {
            checked
            {
                switch (obj.type)
                {
                    // Error case
                    default: throw new InvalidCastException();

                    // No conversion required (invalid IL code??)
                    case StackData.ObjectType.Int16: return;
                    case StackData.ObjectType.UInt16: obj.value.Int32 = (int)(short)obj.value.Int16; break; // implicit conversion

                    case StackData.ObjectType.Int8:
                    case StackData.ObjectType.UInt8: obj.value.Int32 = (int)(short)obj.value.Int8; break;

                    case StackData.ObjectType.Int32:
                    case StackData.ObjectType.UInt32: obj.value.Int32 = (int)(short)obj.value.Int32; break;

                    case StackData.ObjectType.Int64:
                    case StackData.ObjectType.UInt64: obj.value.Int32 = (int)(short)obj.value.Int64; break;

                    case StackData.ObjectType.Single: obj.value.Int32 = (int)(short)obj.value.Single; break;
                    case StackData.ObjectType.Double: obj.value.Int32 = (int)(short)obj.value.Double; break;

                    case StackData.ObjectType.RefBoxed: obj.value.Int32 = (int)(short)obj.UnboxAsType(TypeCode.Int16); break;
                }
            }

            obj.type = StackData.ObjectType.Int32;
        }

        public static void ToInt32(ref StackData obj)
        {
            switch (obj.type)
            {
                // Error case
                default: throw new InvalidCastException();

                // No conversion required (invalid IL code??)
                case StackData.ObjectType.Int32: return;
                case StackData.ObjectType.UInt32: break; // implicit conversion

                case StackData.ObjectType.Int8:
                case StackData.ObjectType.UInt8: obj.value.Int32 = (int)obj.value.Int8; break;

                case StackData.ObjectType.Int16:
                case StackData.ObjectType.UInt16: obj.value.Int32 = (int)obj.value.Int16; break;

                case StackData.ObjectType.Int64:
                case StackData.ObjectType.UInt64: obj.value.Int32 = (int)obj.value.Int64; break;

                case StackData.ObjectType.Single: obj.value.Int32 = (int)obj.value.Single; break;
                case StackData.ObjectType.Double: obj.value.Int32 = (int)obj.value.Double; break;

                case StackData.ObjectType.RefBoxed: obj.value.Int32 = (int)obj.UnboxAsType(TypeCode.Int32); break;
            }

            obj.type = StackData.ObjectType.Int32;
        }

        public static void ToInt32Checked(ref StackData obj)
        {
            checked
            {
                switch (obj.type)
                {
                    // Error case
                    default: throw new InvalidCastException();

                    // No conversion required (invalid IL code??)
                    case StackData.ObjectType.Int32: return;
                    case StackData.ObjectType.UInt32: break; // implicit conversion

                    case StackData.ObjectType.Int8:
                    case StackData.ObjectType.UInt8: obj.value.Int32 = (int)obj.value.Int8; break;

                    case StackData.ObjectType.Int16:
                    case StackData.ObjectType.UInt16: obj.value.Int32 = (int)obj.value.Int16; break;

                    case StackData.ObjectType.Int64:
                    case StackData.ObjectType.UInt64: obj.value.Int32 = (int)obj.value.Int64; break;

                    case StackData.ObjectType.Single: obj.value.Int32 = (int)obj.value.Single; break;
                    case StackData.ObjectType.Double: obj.value.Int32 = (int)obj.value.Double; break;

                    case StackData.ObjectType.RefBoxed: obj.value.Int32 = (int)obj.UnboxAsType(TypeCode.Int32); break;
                }
            }

            obj.type = StackData.ObjectType.Int32;
        }

        public static void ToInt64(ref StackData obj)
        {
            switch (obj.type)
            {
                // Error case
                default: throw new InvalidCastException();

                // No conversion required (invalid IL code??)
                case StackData.ObjectType.Int64: return;
                case StackData.ObjectType.UInt64: break; // implicit conversion

                case StackData.ObjectType.Int8:
                case StackData.ObjectType.UInt8: obj.value.Int64 = (long)obj.value.Int8; break;

                case StackData.ObjectType.Int16:
                case StackData.ObjectType.UInt16: obj.value.Int64 = (long)obj.value.Int16; break;

                case StackData.ObjectType.Int32:
                case StackData.ObjectType.UInt32: obj.value.Int64 = (long)obj.value.Int32; break;

                case StackData.ObjectType.Single: obj.value.Int64 = (long)obj.value.Single; break;
                case StackData.ObjectType.Double: obj.value.Int64 = (long)obj.value.Double; break;

                case StackData.ObjectType.RefBoxed: obj.value.Int64 = (long)obj.UnboxAsType(TypeCode.Int64); break;
            }

            obj.type = StackData.ObjectType.Int64;
        }

        public static void ToInt64Checked(ref StackData obj)
        {
            checked
            {
                switch (obj.type)
                {
                    // Error case
                    default: throw new InvalidCastException();

                    // No conversion required (invalid IL code??)
                    case StackData.ObjectType.Int64: return;
                    case StackData.ObjectType.UInt64: break; // implicit conversion

                    case StackData.ObjectType.Int8:
                    case StackData.ObjectType.UInt8: obj.value.Int64 = (long)obj.value.Int8; break;

                    case StackData.ObjectType.Int16:
                    case StackData.ObjectType.UInt16: obj.value.Int64 = (long)obj.value.Int16; break;

                    case StackData.ObjectType.Int32:
                    case StackData.ObjectType.UInt32: obj.value.Int64 = (long)obj.value.Int32; break;

                    case StackData.ObjectType.Single: obj.value.Int64 = (long)obj.value.Single; break;
                    case StackData.ObjectType.Double: obj.value.Int64 = (long)obj.value.Double; break;

                    case StackData.ObjectType.RefBoxed: obj.value.Int64 = (long)obj.UnboxAsType(TypeCode.Int64); break;
                }
            }

            obj.type = StackData.ObjectType.Int64;
        }

        public static void ToUInt8(ref StackData obj)
        {
            switch (obj.type)
            {
                // Error case
                default: throw new InvalidCastException();

                // No conversion required (invalid IL code??)
                case StackData.ObjectType.UInt8: return;
                case StackData.ObjectType.Int8: break; // implicit conversion

                case StackData.ObjectType.Int16:
                case StackData.ObjectType.UInt16: obj.value.Int8 = (sbyte)(byte)obj.value.Int16; break;

                case StackData.ObjectType.Int32:
                case StackData.ObjectType.UInt32: obj.value.Int8 = (sbyte)(byte)obj.value.Int32; break;

                case StackData.ObjectType.Int64:
                case StackData.ObjectType.UInt64: obj.value.Int8 = (sbyte)(byte)obj.value.Int64; break;

                case StackData.ObjectType.Single: obj.value.Int8 = (sbyte)(byte)obj.value.Single; break;
                case StackData.ObjectType.Double: obj.value.Int8 = (sbyte)(byte)obj.value.Double; break;

                case StackData.ObjectType.RefBoxed: obj.value.Int8 = (sbyte)(byte)obj.UnboxAsType(TypeCode.Byte); break;
            }

            obj.type = StackData.ObjectType.UInt8;
        }

        public static void ToUInt8Promote(ref StackData obj)
        {
            switch (obj.type)
            {
                // Error case
                default: throw new InvalidCastException();

                // No conversion required (invalid IL code??)
                case StackData.ObjectType.UInt8: return;
                case StackData.ObjectType.Int8: obj.value.Int32 = (int)(byte)obj.value.Int8; break; // implicit conversion

                case StackData.ObjectType.Int16:
                case StackData.ObjectType.UInt16: obj.value.Int32 = (int)(byte)obj.value.Int16; break;

                case StackData.ObjectType.Int32:
                case StackData.ObjectType.UInt32: obj.value.Int32 = (int)(byte)obj.value.Int32; break;

                case StackData.ObjectType.Int64:
                case StackData.ObjectType.UInt64: obj.value.Int32 = (int)(byte)obj.value.Int64; break;

                case StackData.ObjectType.Single: obj.value.Int32 = (int)(byte)obj.value.Single; break;
                case StackData.ObjectType.Double: obj.value.Int32 = (int)(byte)obj.value.Double; break;

                case StackData.ObjectType.RefBoxed: obj.value.Int32 = (int)(byte)obj.UnboxAsType(TypeCode.Byte); break;
            }

            obj.type = StackData.ObjectType.Int32;
        }

        public static void ToUInt8Checked(ref StackData obj)
        {
            checked
            {
                switch (obj.type)
                {
                    // Error case
                    default: throw new InvalidCastException();

                    // No conversion required (invalid IL code??)
                    case StackData.ObjectType.UInt8: return;
                    case StackData.ObjectType.Int8: break; // implicit conversion

                    case StackData.ObjectType.Int16:
                    case StackData.ObjectType.UInt16: obj.value.Int8 = (sbyte)(byte)obj.value.Int16; break;

                    case StackData.ObjectType.Int32:
                    case StackData.ObjectType.UInt32: obj.value.Int8 = (sbyte)(byte)obj.value.Int32; break;

                    case StackData.ObjectType.Int64:
                    case StackData.ObjectType.UInt64: obj.value.Int8 = (sbyte)(byte)obj.value.Int64; break;

                    case StackData.ObjectType.Single: obj.value.Int8 = (sbyte)(byte)obj.value.Single; break;
                    case StackData.ObjectType.Double: obj.value.Int8 = (sbyte)(byte)obj.value.Double; break;

                    case StackData.ObjectType.RefBoxed: obj.value.Int8 = (sbyte)(byte)obj.UnboxAsType(TypeCode.Byte); break;
                }
            }

            obj.type = StackData.ObjectType.UInt8;
        }

        public static void ToUInt8CheckedPromote(ref StackData obj)
        {
            checked
            {
                switch (obj.type)
                {
                    // Error case
                    default: throw new InvalidCastException();

                    // No conversion required (invalid IL code??)
                    case StackData.ObjectType.UInt8: return;
                    case StackData.ObjectType.Int8: obj.value.Int32 = (int)(byte)obj.value.Int8; break; // implicit conversion

                    case StackData.ObjectType.Int16:
                    case StackData.ObjectType.UInt16: obj.value.Int32 = (int)(byte)obj.value.Int16; break;

                    case StackData.ObjectType.Int32:
                    case StackData.ObjectType.UInt32: obj.value.Int32 = (int)(byte)obj.value.Int32; break;

                    case StackData.ObjectType.Int64:
                    case StackData.ObjectType.UInt64: obj.value.Int32 = (int)(byte)obj.value.Int64; break;

                    case StackData.ObjectType.Single: obj.value.Int32 = (int)(byte)obj.value.Single; break;
                    case StackData.ObjectType.Double: obj.value.Int32 = (int)(byte)obj.value.Double; break;

                    case StackData.ObjectType.RefBoxed: obj.value.Int32 = (int)(byte)obj.UnboxAsType(TypeCode.Byte); break;
                }
            }

            obj.type = StackData.ObjectType.Int32;
        }

        public static void ToUInt16(ref StackData obj)
        {
            switch (obj.type)
            {
                // Error case
                default: throw new InvalidCastException();

                // No conversion required (invalid IL code??)
                case StackData.ObjectType.UInt16: return;
                case StackData.ObjectType.Int16: break; // implicit conversion

                case StackData.ObjectType.Int8:
                case StackData.ObjectType.UInt8: obj.value.Int16 = (short)(ushort)obj.value.Int8; break;

                case StackData.ObjectType.Int32:
                case StackData.ObjectType.UInt32: obj.value.Int16 = (short)(ushort)obj.value.Int32; break;

                case StackData.ObjectType.Int64:
                case StackData.ObjectType.UInt64: obj.value.Int16 = (short)(ushort)obj.value.Int64; break;

                case StackData.ObjectType.Single: obj.value.Int16 = (short)(ushort)obj.value.Single; break;
                case StackData.ObjectType.Double: obj.value.Int16 = (short)(ushort)obj.value.Double; break;

                case StackData.ObjectType.RefBoxed: obj.value.Int16 = (short)(ushort)obj.UnboxAsType(TypeCode.UInt16); break;
            }

            obj.type = StackData.ObjectType.UInt16;
        }

        public static void ToUInt16Promote(ref StackData obj)
        {
            switch (obj.type)
            {
                // Error case
                default: throw new InvalidCastException();

                // No conversion required (invalid IL code??)
                case StackData.ObjectType.UInt16: return;
                case StackData.ObjectType.Int16: obj.value.Int32 = (int)obj.value.Int16; break;

                case StackData.ObjectType.Int8:
                case StackData.ObjectType.UInt8: obj.value.Int32 = (int)(ushort)obj.value.Int8; break;

                case StackData.ObjectType.Int32:
                case StackData.ObjectType.UInt32: obj.value.Int32 = (int)(ushort)obj.value.Int32; break;

                case StackData.ObjectType.Int64:
                case StackData.ObjectType.UInt64: obj.value.Int32 = (int)(ushort)obj.value.Int64; break;

                case StackData.ObjectType.Single: obj.value.Int32 = (int)(ushort)obj.value.Single; break;
                case StackData.ObjectType.Double: obj.value.Int32 = (int)(ushort)obj.value.Double; break;

                case StackData.ObjectType.RefBoxed: obj.value.Int32 = (int)(ushort)obj.UnboxAsType(TypeCode.UInt16); break;
            }

            obj.type = StackData.ObjectType.Int32;
        }

        public static void ToUInt16Checked(ref StackData obj)
        {
            checked
            {
                switch (obj.type)
                {
                    // Error case
                    default: throw new InvalidCastException();

                    // No conversion required (invalid IL code??)
                    case StackData.ObjectType.UInt16: return;
                    case StackData.ObjectType.Int16: break; // implicit conversion

                    case StackData.ObjectType.Int8:
                    case StackData.ObjectType.UInt8: obj.value.Int16 = (short)(ushort)obj.value.Int8; break;

                    case StackData.ObjectType.Int32:
                    case StackData.ObjectType.UInt32: obj.value.Int16 = (short)(ushort)obj.value.Int32; break;

                    case StackData.ObjectType.Int64:
                    case StackData.ObjectType.UInt64: obj.value.Int16 = (short)(ushort)obj.value.Int64; break;

                    case StackData.ObjectType.Single: obj.value.Int16 = (short)(ushort)obj.value.Single; break;
                    case StackData.ObjectType.Double: obj.value.Int16 = (short)(ushort)obj.value.Double; break;

                    case StackData.ObjectType.RefBoxed: obj.value.Int16 = (short)(ushort)obj.UnboxAsType(TypeCode.UInt16); break;
                }
            }

            obj.type = StackData.ObjectType.UInt16;
        }

        public static void ToUInt16CheckedPromote(ref StackData obj)
        {
            checked
            {
                switch (obj.type)
                {
                    // Error case
                    default: throw new InvalidCastException();

                    // No conversion required (invalid IL code??)
                    case StackData.ObjectType.UInt16: return;
                    case StackData.ObjectType.Int16: obj.value.Int32 = (int)(ushort)obj.value.Int16; break; // implicit conversion

                    case StackData.ObjectType.Int8:
                    case StackData.ObjectType.UInt8: obj.value.Int32 = (int)(ushort)obj.value.Int8; break;

                    case StackData.ObjectType.Int32:
                    case StackData.ObjectType.UInt32: obj.value.Int32 = (int)(ushort)obj.value.Int32; break;

                    case StackData.ObjectType.Int64:
                    case StackData.ObjectType.UInt64: obj.value.Int32 = (int)(ushort)obj.value.Int64; break;

                    case StackData.ObjectType.Single: obj.value.Int32 = (int)(ushort)obj.value.Single; break;
                    case StackData.ObjectType.Double: obj.value.Int32 = (int)(ushort)obj.value.Double; break;

                    case StackData.ObjectType.RefBoxed: obj.value.Int32 = (int)(ushort)obj.UnboxAsType(TypeCode.UInt16); break;
                }
            }

            obj.type = StackData.ObjectType.Int32;
        }

        public static void ToUInt32(ref StackData obj)
        {
            switch (obj.type)
            {
                // Error case
                default: throw new InvalidCastException();

                // No conversion required (invalid IL code??)
                case StackData.ObjectType.UInt32: return;
                case StackData.ObjectType.Int32: break; // implicit conversion

                case StackData.ObjectType.Int8:
                case StackData.ObjectType.UInt8: obj.value.Int32 = (int)(uint)obj.value.Int8; break;

                case StackData.ObjectType.Int16:
                case StackData.ObjectType.UInt16: obj.value.Int32 = (int)(uint)obj.value.Int16; break;

                case StackData.ObjectType.Int64:
                case StackData.ObjectType.UInt64: obj.value.Int32 = (int)(uint)obj.value.Int64; break;

                case StackData.ObjectType.Single: obj.value.Int32 = (int)(uint)obj.value.Single; break;
                case StackData.ObjectType.Double: obj.value.Int32 = (int)(uint)obj.value.Double; break;

                case StackData.ObjectType.RefBoxed: obj.value.Int32 = (int)(uint)obj.UnboxAsType(TypeCode.UInt32); break;
            }

            obj.type = StackData.ObjectType.UInt32;
        }

        public static void ToUInt32Checked(ref StackData obj)
        {
            checked
            {
                switch (obj.type)
                {
                    // Error case
                    default: throw new InvalidCastException();

                    // No conversion required (invalid IL code??)
                    case StackData.ObjectType.UInt32: return;
                    case StackData.ObjectType.Int32: break; // implicit conversion

                    case StackData.ObjectType.Int8:
                    case StackData.ObjectType.UInt8: obj.value.Int32 = (int)(uint)obj.value.Int8; break;

                    case StackData.ObjectType.Int16:
                    case StackData.ObjectType.UInt16: obj.value.Int32 = (int)(uint)obj.value.Int16; break;

                    case StackData.ObjectType.Int64:
                    case StackData.ObjectType.UInt64: obj.value.Int32 = (int)(uint)obj.value.Int64; break;

                    case StackData.ObjectType.Single: obj.value.Int32 = (int)(uint)obj.value.Single; break;
                    case StackData.ObjectType.Double: obj.value.Int32 = (int)(uint)obj.value.Double; break;

                    case StackData.ObjectType.RefBoxed: obj.value.Int32 = (int)(uint)obj.UnboxAsType(TypeCode.UInt32); break;
                }
            }

            obj.type = StackData.ObjectType.UInt32;
        }

        public static void ToUInt64(ref StackData obj)
        {
            switch (obj.type)
            {
                // Error case
                default: throw new InvalidCastException();

                // No conversion required (invalid IL code??)
                case StackData.ObjectType.UInt64: return;
                case StackData.ObjectType.Int64: break; // implicit conversion

                case StackData.ObjectType.Int8:
                case StackData.ObjectType.UInt8: obj.value.Int64 = (long)(ulong)obj.value.Int8; break;

                case StackData.ObjectType.Int16:
                case StackData.ObjectType.UInt16: obj.value.Int64 = (long)(ulong)obj.value.Int16; break;

                case StackData.ObjectType.Int32:
                case StackData.ObjectType.UInt32: obj.value.Int64 = (long)(ulong)obj.value.Int32; break;

                case StackData.ObjectType.Single: obj.value.Int64 = (long)(ulong)obj.value.Single; break;
                case StackData.ObjectType.Double: obj.value.Int64 = (long)(ulong)obj.value.Double; break;

                case StackData.ObjectType.RefBoxed: obj.value.Int64 = (long)(ulong)obj.UnboxAsType(TypeCode.UInt64); break;
            }

            obj.type = StackData.ObjectType.UInt64;
        }

        public static void ToUInt64Checked(ref StackData obj)
        {
            checked
            {
                switch (obj.type)
                {
                    // Error case
                    default: throw new InvalidCastException();

                    // No conversion required (invalid IL code??)
                    case StackData.ObjectType.UInt64: return;
                    case StackData.ObjectType.Int64: break; // implicit conversion

                    case StackData.ObjectType.Int8:
                    case StackData.ObjectType.UInt8: obj.value.Int64 = (long)(ulong)obj.value.Int8; break;

                    case StackData.ObjectType.Int16:
                    case StackData.ObjectType.UInt16: obj.value.Int64 = (long)(ulong)obj.value.Int16; break;

                    case StackData.ObjectType.Int32:
                    case StackData.ObjectType.UInt32: obj.value.Int64 = (long)(ulong)obj.value.Int32; break;

                    case StackData.ObjectType.Single: obj.value.Int64 = (long)(ulong)obj.value.Single; break;
                    case StackData.ObjectType.Double: obj.value.Int64 = (long)(ulong)obj.value.Double; break;

                    case StackData.ObjectType.RefBoxed: obj.value.Int64 = (long)(ulong)obj.UnboxAsType(TypeCode.UInt64); break;
                }
            }

            obj.type = StackData.ObjectType.UInt64;
        }

        public static void ToSingle(ref StackData obj)
        {
            switch (obj.type)
            {
                // Error case
                default: throw new InvalidCastException();

                // No conversion required (invalid IL code??)
                case StackData.ObjectType.Single: return;

                case StackData.ObjectType.Int8:
                case StackData.ObjectType.UInt8: obj.value.Single = (float)obj.value.Int8; break;

                case StackData.ObjectType.Int16:
                case StackData.ObjectType.UInt16: obj.value.Single = (float)obj.value.Int16; break;

                case StackData.ObjectType.Int32:
                case StackData.ObjectType.UInt32: obj.value.Single = (float)obj.value.Int32; break;

                case StackData.ObjectType.Int64:
                case StackData.ObjectType.UInt64: obj.value.Single = (float)obj.value.Int64; break;

                case StackData.ObjectType.Double: obj.value.Single = (float)obj.value.Double; break;

                case StackData.ObjectType.RefBoxed: obj.value.Single = (float)obj.UnboxAsType(TypeCode.Single); break;
            }

            obj.type = StackData.ObjectType.Single;
        }

        public static void ToSingleChecked(ref StackData obj)
        {
            checked
            {
                switch (obj.type)
                {
                    // Error case
                    default: throw new InvalidCastException();

                    // No conversion required (invalid IL code??)
                    case StackData.ObjectType.Single: return;

                    case StackData.ObjectType.Int8:
                    case StackData.ObjectType.UInt8: obj.value.Single = (float)obj.value.Int8; break;

                    case StackData.ObjectType.Int16:
                    case StackData.ObjectType.UInt16: obj.value.Single = (float)obj.value.Int16; break;

                    case StackData.ObjectType.Int32:
                    case StackData.ObjectType.UInt32: obj.value.Single = (float)obj.value.Int32; break;

                    case StackData.ObjectType.Int64:
                    case StackData.ObjectType.UInt64: obj.value.Single = (float)obj.value.Int64; break;

                    case StackData.ObjectType.Double: obj.value.Single = (float)obj.value.Double; break;

                    case StackData.ObjectType.RefBoxed: obj.value.Single = (float)obj.UnboxAsType(TypeCode.Single); break;
                }
            }

            obj.type = StackData.ObjectType.Single;
        }

        public static void ToDouble(ref StackData obj)
        {
            switch (obj.type)
            {
                // Error case
                default: throw new InvalidCastException();

                // No conversion required (invalid IL code??)
                case StackData.ObjectType.Double: return;

                case StackData.ObjectType.Int8:
                case StackData.ObjectType.UInt8: obj.value.Double = (double)obj.value.Int8; break;

                case StackData.ObjectType.Int16:
                case StackData.ObjectType.UInt16: obj.value.Double = (double)obj.value.Int16; break;

                case StackData.ObjectType.Int32:
                case StackData.ObjectType.UInt32: obj.value.Double = (double)obj.value.Int32; break;

                case StackData.ObjectType.Int64:
                case StackData.ObjectType.UInt64: obj.value.Double = (double)obj.value.Int64; break;

                case StackData.ObjectType.Single: obj.value.Double = (double)obj.value.Single; break;

                case StackData.ObjectType.RefBoxed: obj.value.Double = (double)obj.UnboxAsType(TypeCode.Double); break;
            }

            obj.type = StackData.ObjectType.Double;
        }

        public static void ToDoubleChecked(ref StackData obj)
        {
            checked
            {
                switch (obj.type)
                {
                    // Error case
                    default: throw new InvalidCastException();

                    // No conversion required (invalid IL code??)
                    case StackData.ObjectType.Double: return;

                    case StackData.ObjectType.Int8:
                    case StackData.ObjectType.UInt8: obj.value.Double = (double)obj.value.Int8; break;

                    case StackData.ObjectType.Int16:
                    case StackData.ObjectType.UInt16: obj.value.Double = (double)obj.value.Int16; break;

                    case StackData.ObjectType.Int32:
                    case StackData.ObjectType.UInt32: obj.value.Double = (double)obj.value.Int32; break;

                    case StackData.ObjectType.Int64:
                    case StackData.ObjectType.UInt64: obj.value.Double = (double)obj.value.Int64; break;

                    case StackData.ObjectType.Single: obj.value.Double = (double)obj.value.Single; break;

                    case StackData.ObjectType.RefBoxed: obj.value.Double = (double)obj.UnboxAsType(TypeCode.Double); break;
                }
            }

            obj.type = StackData.ObjectType.Double;
        }
    }
}
