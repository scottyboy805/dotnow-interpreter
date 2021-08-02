using System;
using System.Reflection;
using dotnow.Runtime.CIL;

namespace dotnow.Runtime
{
    public class ByRefField : IByRef
    {
        // Private
        private static readonly StackData[] directAccessStack = new StackData[2];

        private CILFieldAccess fieldAccess = null;
        private StackData instance = default;

        // Constructor
        internal ByRefField(CILFieldAccess fieldAccess, StackData instance)
        {
            this.fieldAccess = fieldAccess;
            this.instance = instance;
        }

        // Methods
        public StackData GetReferenceValue()
        {
            // Check for direct access delegate
            if(fieldAccess.directReadAccessDelegate != null)
            {
                // Push instance
                directAccessStack[0] = instance;

                fieldAccess.directReadAccessDelegate(directAccessStack, 0);
                return directAccessStack[0];
            }

            StackData value = new StackData();

            // Check for by ref
            if (instance.type == StackData.ObjectType.ByRef)
            {
                StackData.AllocTyped(ref value, fieldAccess.fieldTypeInfo, fieldAccess.targetField.GetValue(((IByRef)instance.refValue).GetReferenceValue().Box()));
                return value;
            }

            StackData.AllocTyped(ref value, fieldAccess.fieldTypeInfo, fieldAccess.targetField.GetValue(instance.Box()));

            return value;
        }

        public byte GetReferenceValueU1()
        {
            // Check for direct access delegate
            if (fieldAccess.directReadAccessDelegate != null)
            {
                // Push instance
                directAccessStack[0] = instance;

                fieldAccess.directReadAccessDelegate(directAccessStack, 0);
                return (byte)directAccessStack[0].value.Int8;
            }

            // Check for by ref
            if (instance.type == StackData.ObjectType.ByRef)
                return (byte)fieldAccess.targetField.GetValue(((IByRef)instance.refValue).GetReferenceValue().Box());

            return (byte)fieldAccess.targetField.GetValue(instance.Box());
        }

        public ushort GetReferenceValueU2()
        {
            // Check for direct access delegate
            if (fieldAccess.directReadAccessDelegate != null)
            {
                // Push instance
                directAccessStack[0] = instance;

                fieldAccess.directReadAccessDelegate(directAccessStack, 0);
                return (ushort)directAccessStack[0].value.Int16;
            }

            // Check for by ref
            if (instance.type == StackData.ObjectType.ByRef)
                return (ushort)fieldAccess.targetField.GetValue(((IByRef)instance.refValue).GetReferenceValue().Box());

            return (ushort)fieldAccess.targetField.GetValue(instance.Box());
        }

        public uint GetReferenceValueU4()
        {
            // Check for direct access delegate
            if (fieldAccess.directReadAccessDelegate != null)
            {
                // Push instance
                directAccessStack[0] = instance;

                fieldAccess.directReadAccessDelegate(directAccessStack, 0);
                return (uint)directAccessStack[0].value.Int32;
            }

            // Check for by ref
            if (instance.type == StackData.ObjectType.ByRef)
                return (uint)fieldAccess.targetField.GetValue(((IByRef)instance.refValue).GetReferenceValue().Box());

            return (uint)fieldAccess.targetField.GetValue(instance.Box());
        }

        public ulong GetReferenceValueU8()
        {
            // Check for direct access delegate
            if (fieldAccess.directReadAccessDelegate != null)
            {
                // Push instance
                directAccessStack[0] = instance;

                fieldAccess.directReadAccessDelegate(directAccessStack, 0);
                return (ulong)directAccessStack[0].value.Int64;
            }

            // Check for by ref
            if (instance.type == StackData.ObjectType.ByRef)
                return (ulong)fieldAccess.targetField.GetValue(((IByRef)instance.refValue).GetReferenceValue().Box());

            return (ulong)fieldAccess.targetField.GetValue(instance.Box());
        }

        public sbyte GetReferenceValueI1()
        {
            // Check for direct access delegate
            if (fieldAccess.directReadAccessDelegate != null)
            {
                // Push instance
                directAccessStack[0] = instance;

                fieldAccess.directReadAccessDelegate(directAccessStack, 0);
                return directAccessStack[0].value.Int8;
            }

            // Check for by ref
            if (instance.type == StackData.ObjectType.ByRef)
                return (sbyte)fieldAccess.targetField.GetValue(((IByRef)instance.refValue).GetReferenceValue().Box());

            return (sbyte)fieldAccess.targetField.GetValue(instance.Box());
        }

        public short GetReferenceValueI2()
        {
            // Check for direct access delegate
            if (fieldAccess.directReadAccessDelegate != null)
            {
                // Push instance
                directAccessStack[0] = instance;

                fieldAccess.directReadAccessDelegate(directAccessStack, 0);
                return directAccessStack[0].value.Int16;
            }

            // Check for by ref
            if (instance.type == StackData.ObjectType.ByRef)
                return (short)fieldAccess.targetField.GetValue(((IByRef)instance.refValue).GetReferenceValue().Box());

            return (short)fieldAccess.targetField.GetValue(instance.Box());
        }

        public int GetReferenceValueI4()
        {
            // Check for direct access delegate
            if (fieldAccess.directReadAccessDelegate != null)
            {
                // Push instance
                directAccessStack[0] = instance;

                fieldAccess.directReadAccessDelegate(directAccessStack, 0);
                return directAccessStack[0].value.Int32;
            }

            // Check for by ref
            if (instance.type == StackData.ObjectType.ByRef)
                return (int)fieldAccess.targetField.GetValue(((IByRef)instance.refValue).GetReferenceValue().Box());

            return (int)fieldAccess.targetField.GetValue(instance.Box());
        }

        public long GetReferenceValueI8()
        {
            // Check for direct access delegate
            if (fieldAccess.directReadAccessDelegate != null)
            {
                // Push instance
                directAccessStack[0] = instance;

                fieldAccess.directReadAccessDelegate(directAccessStack, 0);
                return directAccessStack[0].value.Int64;
            }

            // Check for by ref
            if (instance.type == StackData.ObjectType.ByRef)
                return (long)fieldAccess.targetField.GetValue(((IByRef)instance.refValue).GetReferenceValue().Box());

            return (long)fieldAccess.targetField.GetValue(instance.Box());
        }

        public float GetReferenceValueR4()
        {
            // Check for direct access delegate
            if (fieldAccess.directReadAccessDelegate != null)
            {
                // Push instance
                directAccessStack[0] = instance;

                fieldAccess.directReadAccessDelegate(directAccessStack, 0);
                return directAccessStack[0].value.Single;
            }

            // Check for by ref
            if (instance.type == StackData.ObjectType.ByRef)
                return (float)fieldAccess.targetField.GetValue(((IByRef)instance.refValue).GetReferenceValue().Box());

            return (float)fieldAccess.targetField.GetValue(instance.Box());
        }

        public double GetReferenceValueR8()
        {
            // Check for direct access delegate
            if (fieldAccess.directReadAccessDelegate != null)
            {
                // Push instance
                directAccessStack[0] = instance;

                fieldAccess.directReadAccessDelegate(directAccessStack, 0);
                return directAccessStack[0].value.Double;
            }

            // Check for by ref
            if (instance.type == StackData.ObjectType.ByRef)
                return (double)fieldAccess.targetField.GetValue(((IByRef)instance.refValue).GetReferenceValue().Box());

            return (double)fieldAccess.targetField.GetValue(instance.Box());
        }

        public void SetReferenceValue(StackData value)
        {
            // Check for direct access delegate
            if(fieldAccess.directWriteAccessDelegate != null)
            {
                // Push instance and value
                directAccessStack[0] = instance;
                directAccessStack[1] = value;

                fieldAccess.directWriteAccessDelegate(directAccessStack, 0);
                return;
            }

            // Check for by ref
            if(instance.type == StackData.ObjectType.ByRef)
            {
                fieldAccess.targetField.SetValue(((IByRef)instance.refValue).GetReferenceValue().Box(), value.UnboxAsType(fieldAccess.fieldTypeInfo));
                return;
            }

            fieldAccess.targetField.SetValue(instance.Box(), value.UnboxAsType(fieldAccess.fieldTypeInfo));
        }

        public void SetReferenceValueI1(sbyte value)
        {
            // Check for direct access delegate
            if (fieldAccess.directWriteAccessDelegate != null)
            {
                // Push instance and value
                directAccessStack[0] = instance;
                StackData.AllocTyped(ref directAccessStack[1], TypeCode.SByte, value);

                fieldAccess.directWriteAccessDelegate(directAccessStack, 0);
                return;
            }

            // Check for by ref
            if (instance.type == StackData.ObjectType.ByRef)
            {
                fieldAccess.targetField.SetValue(((IByRef)instance.refValue).GetReferenceValue().Box(), value);
                return;
            }

            fieldAccess.targetField.SetValue(instance.Box(), value);
        }

        public void SetReferenceValueI2(short value)
        {
            // Check for direct access delegate
            if (fieldAccess.directWriteAccessDelegate != null)
            {
                // Push instance and value
                directAccessStack[0] = instance;
                StackData.AllocTyped(ref directAccessStack[1], TypeCode.Int16, value);

                fieldAccess.directWriteAccessDelegate(directAccessStack, 0);
                return;
            }

            // Check for by ref
            if (instance.type == StackData.ObjectType.ByRef)
            {
                fieldAccess.targetField.SetValue(((IByRef)instance.refValue).GetReferenceValue().Box(), value);
                return;
            }

            fieldAccess.targetField.SetValue(instance.Box(), value);
        }

        public void SetReferenceValueI4(int value)
        {
            // Check for direct access delegate
            if (fieldAccess.directWriteAccessDelegate != null)
            {
                // Push instance and value
                directAccessStack[0] = instance;
                StackData.AllocTyped(ref directAccessStack[1], TypeCode.Int32, value);

                fieldAccess.directWriteAccessDelegate(directAccessStack, 0);
                return;
            }

            // Check for by ref
            if (instance.type == StackData.ObjectType.ByRef)
            {
                fieldAccess.targetField.SetValue(((IByRef)instance.refValue).GetReferenceValue().Box(), value);
                return;
            }

            fieldAccess.targetField.SetValue(instance.Box(), value);
        }

        public void SetReferenceValueI8(long value)
        {
            // Check for direct access delegate
            if (fieldAccess.directWriteAccessDelegate != null)
            {
                // Push instance and value
                directAccessStack[0] = instance;
                StackData.AllocTyped(ref directAccessStack[1], TypeCode.Int64, value);

                fieldAccess.directWriteAccessDelegate(directAccessStack, 0);
                return;
            }

            // Check for by ref
            if (instance.type == StackData.ObjectType.ByRef)
            {
                fieldAccess.targetField.SetValue(((IByRef)instance.refValue).GetReferenceValue().Box(), value);
                return;
            }

            fieldAccess.targetField.SetValue(instance.Box(), value);
        }

        public void SetReferenceValueR4(float value)
        {
            // Check for direct access delegate
            if (fieldAccess.directWriteAccessDelegate != null)
            {
                // Push instance and value
                directAccessStack[0] = instance;
                StackData.AllocTyped(ref directAccessStack[1], TypeCode.Single, value);

                fieldAccess.directWriteAccessDelegate(directAccessStack, 0);
                return;
            }

            // Check for by ref
            if (instance.type == StackData.ObjectType.ByRef)
            {
                fieldAccess.targetField.SetValue(((IByRef)instance.refValue).GetReferenceValue().Box(), value);
                return;
            }

            fieldAccess.targetField.SetValue(instance.Box(), value);
        }

        public void SetReferenceValueR8(double value)
        {
            // Check for direct access delegate
            if (fieldAccess.directWriteAccessDelegate != null)
            {
                // Push instance and value
                directAccessStack[0] = instance;
                StackData.AllocTyped(ref directAccessStack[1], TypeCode.Double, value);

                fieldAccess.directWriteAccessDelegate(directAccessStack, 0);
                return;
            }

            // Check for by ref
            if (instance.type == StackData.ObjectType.ByRef)
            {
                fieldAccess.targetField.SetValue(((IByRef)instance.refValue).GetReferenceValue().Box(), value);
                return;
            }

            fieldAccess.targetField.SetValue(instance.Box(), value);
        }

        public override string ToString()
        {
            return string.Format("ByRef({0})", GetReferenceValue());
        }
    }
}
