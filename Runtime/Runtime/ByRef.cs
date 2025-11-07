using dotnow.Runtime.CIL;
using System;
using System.Reflection;

namespace dotnow.Runtime
{
    internal interface IByRef
    {
        // Type
        private readonly struct ByRefStack : IByRef
        {
            // Public
            public readonly StackData[] Stack;
            public readonly int Index;

            // Constructor
            public ByRefStack(StackData[] stack, int index)
            {
                this.Stack = stack;
                this.Index = index;
            }

            // Methods
            public override string ToString()
            {
                return $"ByRef Stack({Index}) = {Stack[Index]}";
            }

            public object GetValueRef() => Stack[Index].Ref;
            public IntPtr GetValueI() => Stack[Index].Ptr;
            public sbyte GetValueI1() => (sbyte)Stack[Index].I32;
            public short GetValueI2() => (short)Stack[Index].I32;
            public int GetValueI4() => Stack[Index].I32;
            public long GetValueI8() => Stack[Index].I64;
            public float GetValueR4() => Stack[Index].F32;
            public double GetValueR8() => Stack[Index].F64;
            public byte GetValueU1() => (byte)Stack[Index].I32;
            public ushort GetValueU2() => (ushort)Stack[Index].I32;
            public uint GetValueU4() => (uint)Stack[Index].I32;
            public ulong GetValueU8() => (ulong)Stack[Index].I32;

            public void SetValueRef(object value) => Stack[Index].Ref = value;
            public void SetValueI(IntPtr value) => Stack[Index].Ptr = value;
            public void SetValueI1(sbyte value) => Stack[Index].I32 = value;
            public void SetValueI2(short value) => Stack[Index].I32 = value;
            public void SetValueI4(int value) => Stack[Index].I32 = value;
            public void SetValueI8(long value) => Stack[Index].I64 = value;
            public void SetValueR4(float value) => Stack[Index].F32 = value;
            public void SetValueR8(double value) => Stack[Index].F64 = value;
        }

        private readonly struct ByRefElement : IByRef
        {
            // Public 
            public readonly Array Array;
            public readonly int Index;

            // Constructor
            public ByRefElement(Array array, int index)
            {
                this.Array = array;
                this.Index = index;
            }

            // Methods
            public override string ToString()
            {
                return $"ByRef Element({Index}) = {Array.GetValue(Index)}";
            }

            public object GetValueRef() => ((object[])Array)[Index];
            public IntPtr GetValueI() => ((IntPtr[])Array)[Index];
            public sbyte GetValueI1() => ((sbyte[])Array)[Index];
            public short GetValueI2() => ((short[])Array)[Index];
            public int GetValueI4() => ((int[])Array)[Index];
            public long GetValueI8() => ((long[])Array)[Index];       
            public byte GetValueU1() => ((byte[])Array)[Index];
            public ushort GetValueU2() => ((ushort[])Array)[Index];
            public uint GetValueU4() => ((uint[])Array)[Index];
            public ulong GetValueU8() => ((ulong[])Array)[Index];
            public float GetValueR4() => ((float[])Array)[Index];
            public double GetValueR8() => ((double[])Array)[Index];

            public void SetValueRef(object value) => ((object[])Array)[Index] = value;
            public void SetValueI(IntPtr value) => ((IntPtr[])Array)[Index] = value;
            public void SetValueI1(sbyte value) => ((sbyte[])Array)[Index] = value;
            public void SetValueI2(short value) => ((short[])Array)[Index] = value;
            public void SetValueI4(int value) => ((int[])Array)[Index] = value;
            public void SetValueI8(long value) => ((long[])Array)[Index] = value;
            public void SetValueR4(float value) => ((float[])Array)[Index] = value;
            public void SetValueR8(double value) => ((double[])Array)[Index] = value;
        }

        private readonly struct ByRefField : IByRef
        {
            // Public             
            public readonly AppDomain AppDomain;
            public readonly CILFieldInfo Field;
            public readonly StackData Instance;

            // Constructor
            public ByRefField(AppDomain appDomain, CILFieldInfo field, StackData instance)
            {
                AppDomain = appDomain;
                this.Field = field;
                this.Instance = instance;
            }

            // Methods
            public override string ToString()
            {
                return $"ByRef Field({Field}) = {ReadField()}";
            }

            public object GetValueRef() => ReadField().Ref;
            public IntPtr GetValueI() => ReadField().Ptr;
            public sbyte GetValueI1() => (sbyte)ReadField().I32;
            public short GetValueI2() => (short)ReadField().I32;
            public int GetValueI4() => ReadField().I32;
            public long GetValueI8() => ReadField().I64;
            public byte GetValueU1() => (byte)ReadField().I32;
            public ushort GetValueU2() => (ushort)ReadField().I32;
            public uint GetValueU4() => (uint)ReadField().I32;
            public ulong GetValueU8() => (ulong)ReadField().I64;
            public float GetValueR4() => ReadField().F32;
            public double GetValueR8() => ReadField().F64;

            public void SetValueRef(object value) => SetFieldRef(new StackData { Type = StackType.Ref, Ref = value });
            public void SetValueI(IntPtr value) => SetFieldRef(new StackData { Type = StackType.Ptr, Ptr = value });
            public void SetValueI1(sbyte value) => SetFieldRef(new StackData { Type = StackType.I32, I32 = value });
            public void SetValueI2(short value) => SetFieldRef(new StackData { Type = StackType.I32, I32 = value });
            public void SetValueI4(int value) => SetFieldRef(new StackData { Type = StackType.I32, I32 = value });
            public void SetValueI8(long value) => SetFieldRef(new StackData { Type = StackType.I64, I64 = value });
            public void SetValueR4(float value) => SetFieldRef(new StackData { Type = StackType.F32, F32 = value });
            public void SetValueR8(double value) => SetFieldRef(new StackData { Type = StackType.F64, F64 = value });

            private StackData ReadField()
            {
                StackData val = default;
                
                // Check for static or instance
                if((Field.Flags & CILFieldFlags.This) != 0)
                {
                    // Read the instance field value
                    RuntimeField.GetInstanceFieldDirect(AppDomain, Field, Instance, ref val);
                }
                else
                {
                    // Read the static value
                    RuntimeField.GetStaticFieldDirect(AppDomain, Field, ref val);
                }

                return val;
            }

            private void SetFieldRef(StackData val)
            {
                // Check for static or instance
                if ((Field.Flags & CILFieldFlags.This) != 0)
                {
                    // Read the instance field value
                    RuntimeField.SetInstanceFieldDirect(AppDomain, Field, Instance, ref val);
                }
                else
                {
                    // Read the static value
                    RuntimeField.SetStaticFieldDirect(AppDomain, Field, ref val);
                }
            }
        }

        #region GetValue
        object GetValueRef();
        IntPtr GetValueI();
        sbyte GetValueI1();
        short GetValueI2();
        int GetValueI4();
        byte GetValueU1();
        ushort GetValueU2();
        uint GetValueU4();
        ulong GetValueU8();
        long GetValueI8();
        float GetValueR4();
        double GetValueR8();
        #endregion

        #region SetValue
        void SetValueRef(object value);
        void SetValueI(IntPtr value);
        void SetValueI1(sbyte value);
        void SetValueI2(short value);
        void SetValueI4(int value);
        void SetValueI8(long value);
        void SetValueR4(float value);
        void SetValueR8(double value);
        #endregion

        public static IByRef MakeByRefStack(StackData[] stack, int index)
        {
            return new ByRefStack(stack, index);
        }

        public static IByRef MakeByRefElement(Array array, int index)
        {
            return new ByRefElement(array, index);
        }

        public static IByRef MakeByRefInstanceField(AppDomain appDomain, CILFieldInfo field, StackData instance)
        {
            return new ByRefField(appDomain, field, instance);
        }

        public static IByRef MakeByRefStaticField(AppDomain appDomain, CILFieldInfo field)
        {
            return new ByRefField(appDomain, field, default);
        }
    }
}
