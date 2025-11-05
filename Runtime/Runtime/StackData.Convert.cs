using System;

namespace dotnow.Runtime
{
    internal static class RuntimeConvert
    {
        // Methods
        public static void ToInt8(ref StackData obj)
        {
            switch (obj.Type)
            {
                // Error case
                default: throw new InvalidCastException();

                // No conversion required - all small types are promoted to Int32
                case StackType.Int32:
                    obj.Int32 = (int)(sbyte)obj.Int32;
                    break;
                case StackType.UInt32:
                    obj.Int32 = (int)(sbyte)(uint)obj.Int32;
                    break;

                case StackType.Int64:
                case StackType.UInt64:
                    obj.Int32 = (int)(sbyte)obj.Int64;
                    break;

                case StackType.Single:
                    obj.Int32 = (int)(sbyte)obj.Single;
                    break;
                case StackType.Double:
                    obj.Int32 = (int)(sbyte)obj.Double;
                    break;

                case StackType.RefBoxed:
                    obj.Int32 = (int)(sbyte)obj.UnboxAsType(TypeCode.SByte);
                    break;
            }

            obj.Type = StackType.Int32;
        }

        public static void ToInt8Checked(ref StackData obj)
        {
            checked
            {
                switch (obj.Type)
                {
                    // Error case
                    default: throw new InvalidCastException();

                    // No conversion required - all small types are promoted to Int32
                    case StackType.Int32:
                        obj.Int32 = (int)(sbyte)obj.Int32;
                        break;
                    case StackType.UInt32:
                        obj.Int32 = (int)(sbyte)(uint)obj.Int32;
                        break;

                    case StackType.Int64:
                    case StackType.UInt64:
                        obj.Int32 = (int)(sbyte)obj.Int64;
                        break;

                    case StackType.Single:
                        obj.Int32 = (int)(sbyte)obj.Single;
                        break;
                    case StackType.Double:
                        obj.Int32 = (int)(sbyte)obj.Double;
                        break;

                    case StackType.RefBoxed:
                        obj.Int32 = (int)(sbyte)obj.UnboxAsType(TypeCode.SByte);
                        break;
                }
            }

            obj.Type = StackType.Int32;
        }

        public static void ToInt16(ref StackData obj)
        {
            switch (obj.Type)
            {
                // Error case
                default: throw new InvalidCastException();

                // No conversion required - all small types are promoted to Int32
                case StackType.Int32:
                    obj.Int32 = (int)(short)obj.Int32;
                    break;
                case StackType.UInt32:
                    obj.Int32 = (int)(short)(uint)obj.Int32;
                    break;

                case StackType.Int64:
                case StackType.UInt64:
                    obj.Int32 = (int)(short)obj.Int64;
                    break;

                case StackType.Single:
                    obj.Int32 = (int)(short)obj.Single;
                    break;
                case StackType.Double:
                    obj.Int32 = (int)(short)obj.Double;
                    break;

                case StackType.RefBoxed:
                    obj.Int32 = (int)(short)obj.UnboxAsType(TypeCode.Int16);
                    break;
            }

            obj.Type = StackType.Int32;
        }

        public static void ToInt16Checked(ref StackData obj)
        {
            checked
            {
                switch (obj.Type)
                {
                    // Error case
                    default: throw new InvalidCastException();

                    // No conversion required - all small types are promoted to Int32
                    case StackType.Int32:
                        obj.Int32 = (int)(short)obj.Int32;
                        break;
                    case StackType.UInt32:
                        obj.Int32 = (int)(short)(uint)obj.Int32;
                        break;

                    case StackType.Int64:
                    case StackType.UInt64:
                        obj.Int32 = (int)(short)obj.Int64;
                        break;

                    case StackType.Single:
                        obj.Int32 = (int)(short)obj.Single;
                        break;
                    case StackType.Double:
                        obj.Int32 = (int)(short)obj.Double;
                        break;

                    case StackType.RefBoxed:
                        obj.Int32 = (int)(short)obj.UnboxAsType(TypeCode.Int16);
                        break;
                }
            }

            obj.Type = StackType.Int32;
        }

        public static void ToInt32(ref StackData obj)
        {
            switch (obj.Type)
            {
                // Error case
                default: throw new InvalidCastException();

                // No conversion required
                case StackType.Int32: return;
                case StackType.UInt32: break; // implicit conversion

                case StackType.Int64:
                case StackType.UInt64: obj.Int32 = (int)obj.Int64; break;

                case StackType.Single: obj.Int32 = (int)obj.Single; break;
                case StackType.Double: obj.Int32 = (int)obj.Double; break;

                case StackType.RefBoxed: obj.Int32 = (int)obj.UnboxAsType(TypeCode.Int32); break;
            }

            obj.Type = StackType.Int32;
        }

        public static void ToInt32Checked(ref StackData obj)
        {
            checked
            {
                switch (obj.Type)
                {
                    // Error case
                    default: throw new InvalidCastException();

                    // No conversion required
                    case StackType.Int32: return;
                    case StackType.UInt32: break; // implicit conversion

                    case StackType.Int64:
                    case StackType.UInt64: obj.Int32 = (int)obj.Int64; break;

                    case StackType.Single: obj.Int32 = (int)obj.Single; break;
                    case StackType.Double: obj.Int32 = (int)obj.Double; break;

                    case StackType.RefBoxed: obj.Int32 = (int)obj.UnboxAsType(TypeCode.Int32); break;
                }
            }

            obj.Type = StackType.Int32;
        }

        public static void ToInt64(ref StackData obj)
        {
            switch (obj.Type)
            {
                // Error case
                default: throw new InvalidCastException();

                // No conversion required
                case StackType.Int64: return;
                case StackType.UInt64: break; // implicit conversion

                case StackType.Int32:
                case StackType.UInt32: obj.Int64 = (long)obj.Int32; break;

                case StackType.Single: obj.Int64 = (long)obj.Single; break;
                case StackType.Double: obj.Int64 = (long)obj.Double; break;

                case StackType.RefBoxed: obj.Int64 = (long)obj.UnboxAsType(TypeCode.Int64); break;
            }

            obj.Type = StackType.Int64;
        }

        public static void ToInt64Checked(ref StackData obj)
        {
            checked
            {
                switch (obj.Type)
                {
                    // Error case
                    default: throw new InvalidCastException();

                    // No conversion required
                    case StackType.Int64: return;
                    case StackType.UInt64: break; // implicit conversion

                    case StackType.Int32:
                    case StackType.UInt32: obj.Int64 = (long)obj.Int32; break;

                    case StackType.Single: obj.Int64 = (long)obj.Single; break;
                    case StackType.Double: obj.Int64 = (long)obj.Double; break;

                    case StackType.RefBoxed: obj.Int64 = (long)obj.UnboxAsType(TypeCode.Int64); break;
                }
            }

            obj.Type = StackType.Int64;
        }

        public static void ToUInt8(ref StackData obj)
        {
            switch (obj.Type)
            {
                // Error case
                default: throw new InvalidCastException();

                // No conversion required - all small types are promoted to Int32
                case StackType.Int32:
                    obj.Int32 = (int)(byte)obj.Int32;
                    break;
                case StackType.UInt32:
                    obj.Int32 = (int)(byte)(uint)obj.Int32;
                    break;

                case StackType.Int64:
                case StackType.UInt64:
                    obj.Int32 = (int)(byte)obj.Int64;
                    break;

                case StackType.Single:
                    obj.Int32 = (int)(byte)obj.Single;
                    break;
                case StackType.Double:
                    obj.Int32 = (int)(byte)obj.Double;
                    break;

                case StackType.RefBoxed:
                    obj.Int32 = (int)(byte)obj.UnboxAsType(TypeCode.Byte);
                    break;
            }

            obj.Type = StackType.Int32;
        }

        public static void ToUInt8Checked(ref StackData obj)
        {
            checked
            {
                switch (obj.Type)
                {
                    // Error case
                    default: throw new InvalidCastException();

                    // No conversion required - all small types are promoted to Int32
                    case StackType.Int32:
                        obj.Int32 = (int)(byte)obj.Int32;
                        break;
                    case StackType.UInt32:
                        obj.Int32 = (int)(byte)(uint)obj.Int32;
                        break;

                    case StackType.Int64:
                    case StackType.UInt64:
                        obj.Int32 = (int)(byte)obj.Int64;
                        break;

                    case StackType.Single:
                        obj.Int32 = (int)(byte)obj.Single;
                        break;
                    case StackType.Double:
                        obj.Int32 = (int)(byte)obj.Double;
                        break;

                    case StackType.RefBoxed:
                        obj.Int32 = (int)(byte)obj.UnboxAsType(TypeCode.Byte);
                        break;
                }
            }

            obj.Type = StackType.Int32;
        }

        public static void ToUInt16(ref StackData obj)
        {
            switch (obj.Type)
            {
                // Error case
                default: throw new InvalidCastException();

                // No conversion required - all small types are promoted to Int32
                case StackType.Int32:
                    obj.Int32 = (int)(ushort)obj.Int32;
                    break;
                case StackType.UInt32:
                    obj.Int32 = (int)(ushort)(uint)obj.Int32;
                    break;

                case StackType.Int64:
                case StackType.UInt64:
                    obj.Int32 = (int)(ushort)obj.Int64;
                    break;

                case StackType.Single:
                    obj.Int32 = (int)(ushort)obj.Single;
                    break;
                case StackType.Double:
                    obj.Int32 = (int)(ushort)obj.Double;
                    break;

                case StackType.RefBoxed:
                    obj.Int32 = (int)(ushort)obj.UnboxAsType(TypeCode.UInt16);
                    break;
            }

            obj.Type = StackType.Int32;
        }

        public static void ToUInt16Checked(ref StackData obj)
        {
            checked
            {
                switch (obj.Type)
                {
                    // Error case
                    default: throw new InvalidCastException();

                    // No conversion required - all small types are promoted to Int32
                    case StackType.Int32:
                        obj.Int32 = (int)(ushort)obj.Int32;
                        break;
                    case StackType.UInt32:
                        obj.Int32 = (int)(ushort)(uint)obj.Int32;
                        break;

                    case StackType.Int64:
                    case StackType.UInt64:
                        obj.Int32 = (int)(ushort)obj.Int64;
                        break;

                    case StackType.Single:
                        obj.Int32 = (int)(ushort)obj.Single;
                        break;
                    case StackType.Double:
                        obj.Int32 = (int)(ushort)obj.Double;
                        break;

                    case StackType.RefBoxed:
                        obj.Int32 = (int)(ushort)obj.UnboxAsType(TypeCode.UInt16);
                        break;
                }
            }

            obj.Type = StackType.Int32;
        }

        public static void ToUInt32(ref StackData obj)
        {
            switch (obj.Type)
            {
                // Error case
                default: throw new InvalidCastException();

                // No conversion required
                case StackType.UInt32: return;
                case StackType.Int32: break; // implicit conversion

                case StackType.Int64:
                case StackType.UInt64: obj.Int32 = (int)(uint)obj.Int64; break;

                case StackType.Single: obj.Int32 = (int)(uint)obj.Single; break;
                case StackType.Double: obj.Int32 = (int)(uint)obj.Double; break;

                case StackType.RefBoxed: obj.Int32 = (int)(uint)obj.UnboxAsType(TypeCode.UInt32); break;
            }

            obj.Type = StackType.UInt32;
        }

        public static void ToUInt32Checked(ref StackData obj)
        {
            checked
            {
                switch (obj.Type)
                {
                    // Error case
                    default: throw new InvalidCastException();

                    // No conversion required
                    case StackType.UInt32: return;
                    case StackType.Int32: break; // implicit conversion

                    case StackType.Int64:
                    case StackType.UInt64: obj.Int32 = (int)(uint)obj.Int64; break;

                    case StackType.Single: obj.Int32 = (int)(uint)obj.Single; break;
                    case StackType.Double: obj.Int32 = (int)(uint)obj.Double; break;

                    case StackType.RefBoxed: obj.Int32 = (int)(uint)obj.UnboxAsType(TypeCode.UInt32); break;
                }
            }

            obj.Type = StackType.UInt32;
        }

        public static void ToUInt64(ref StackData obj)
        {
            switch (obj.Type)
            {
                // Error case
                default: throw new InvalidCastException();

                // No conversion required
                case StackType.UInt64: return;
                case StackType.Int64: break; // implicit conversion

                case StackType.Int32:
                case StackType.UInt32: obj.Int64 = (long)(ulong)obj.Int32; break;

                case StackType.Single: obj.Int64 = (long)(ulong)obj.Single; break;
                case StackType.Double: obj.Int64 = (long)(ulong)obj.Double; break;

                case StackType.RefBoxed: obj.Int64 = (long)(ulong)obj.UnboxAsType(TypeCode.UInt64); break;
            }

            obj.Type = StackType.UInt64;
        }

        public static void ToUInt64Checked(ref StackData obj)
        {
            checked
            {
                switch (obj.Type)
                {
                    // Error case
                    default: throw new InvalidCastException();

                    // No conversion required
                    case StackType.UInt64: return;
                    case StackType.Int64: break; // implicit conversion

                    case StackType.Int32:
                    case StackType.UInt32: obj.Int64 = (long)(ulong)obj.Int32; break;

                    case StackType.Single: obj.Int64 = (long)(ulong)obj.Single; break;
                    case StackType.Double: obj.Int64 = (long)(ulong)obj.Double; break;

                    case StackType.RefBoxed: obj.Int64 = (long)(ulong)obj.UnboxAsType(TypeCode.UInt64); break;
                }
            }

            obj.Type = StackType.UInt64;
        }

        public static void ToSingle(ref StackData obj)
        {
            switch (obj.Type)
            {
                // Error case
                default: throw new InvalidCastException();

                // No conversion required
                case StackType.Single: return;

                case StackType.Int32:
                case StackType.UInt32: obj.Single = (float)obj.Int32; break;

                case StackType.Int64:
                case StackType.UInt64: obj.Single = (float)obj.Int64; break;

                case StackType.Double: obj.Single = (float)obj.Double; break;

                case StackType.RefBoxed: obj.Single = (float)obj.UnboxAsType(TypeCode.Single); break;
            }

            obj.Type = StackType.Single;
        }

        public static void ToSingleChecked(ref StackData obj)
        {
            checked
            {
                switch (obj.Type)
                {
                    // Error case
                    default: throw new InvalidCastException();

                    // No conversion required
                    case StackType.Single: return;

                    case StackType.Int32:
                    case StackType.UInt32: obj.Single = (float)obj.Int32; break;

                    case StackType.Int64:
                    case StackType.UInt64: obj.Single = (float)obj.Int64; break;

                    case StackType.Double: obj.Single = (float)obj.Double; break;

                    case StackType.RefBoxed: obj.Single = (float)obj.UnboxAsType(TypeCode.Single); break;
                }
            }

            obj.Type = StackType.Single;
        }

        public static void ToDouble(ref StackData obj)
        {
            switch (obj.Type)
            {
                // Error case
                default: throw new InvalidCastException();

                // No conversion required
                case StackType.Double: return;

                case StackType.Int32:
                case StackType.UInt32: obj.Double = (double)obj.Int32; break;

                case StackType.Int64:
                case StackType.UInt64: obj.Double = (double)obj.Int64; break;

                case StackType.Single: obj.Double = (double)obj.Single; break;

                case StackType.RefBoxed: obj.Double = (double)obj.UnboxAsType(TypeCode.Double); break;
            }

            obj.Type = StackType.Double;
        }

        public static void ToDoubleChecked(ref StackData obj)
        {
            checked
            {
                switch (obj.Type)
                {
                    // Error case
                    default: throw new InvalidCastException();

                    // No conversion required
                    case StackType.Double: return;

                    case StackType.Int32:
                    case StackType.UInt32: obj.Double = (double)obj.Int32; break;

                    case StackType.Int64:
                    case StackType.UInt64: obj.Double = (double)obj.Int64; break;

                    case StackType.Single: obj.Double = (double)obj.Single; break;

                    case StackType.RefBoxed: obj.Double = (double)obj.UnboxAsType(TypeCode.Double); break;
                }
            }

            obj.Type = StackType.Double;
        }
    }
}
