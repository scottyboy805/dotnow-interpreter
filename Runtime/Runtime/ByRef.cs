using System;

namespace dotnow.Runtime
{
    internal interface IByRef
    {
        // Type
        private readonly struct ByRefStack : IByRef
        {
            // Private
            private readonly StackData[] Stack;
            private readonly int Index;

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

            public object GetReferenceValue() => Stack[Index].Ref;
            public sbyte GetReferenceValueI1() => (sbyte)Stack[Index].I32;
            public short GetReferenceValueI2() => (short)Stack[Index].I32;
            public int GetReferenceValueI4() => Stack[Index].I32;
            public long GetReferenceValueI8() => Stack[Index].I64;
            public float GetReferenceValueR4() => Stack[Index].F32;
            public double GetReferenceValueR8() => Stack[Index].F64;
            public byte GetReferenceValueU1() => (byte)Stack[Index].I32;
            public ushort GetReferenceValueU2() => (ushort)Stack[Index].I32;
            public uint GetReferenceValueU4() => (uint)Stack[Index].I32;
            public ulong GetReferenceValueU8() => (ulong)Stack[Index].I32;

            public void SetReferenceValue(object value) => Stack[Index].Ref = value;
            public void SetReferenceValueI1(sbyte value) => Stack[Index].I32 = value;
            public void SetReferenceValueI2(short value) => Stack[Index].I32 = value;
            public void SetReferenceValueI4(int value) => Stack[Index].I32 = value;
            public void SetReferenceValueI8(long value) => Stack[Index].I64 = value;
            public void SetReferenceValueR4(float value) => Stack[Index].F32 = value;
            public void SetReferenceValueR8(double value) => Stack[Index].F64 = value;
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

            public object GetReferenceValue() => ((object[])Array)[Index];
            public sbyte GetReferenceValueI1() => ((sbyte[])Array)[Index];
            public short GetReferenceValueI2() => ((short[])Array)[Index];
            public int GetReferenceValueI4() => ((int[])Array)[Index];
            public long GetReferenceValueI8() => ((long[])Array)[Index];
            public float GetReferenceValueR4() => ((float[])Array)[Index];
            public double GetReferenceValueR8() => ((double[])Array)[Index];
            public byte GetReferenceValueU1() => ((byte[])Array)[Index];
            public ushort GetReferenceValueU2() => ((ushort[])Array)[Index];
            public uint GetReferenceValueU4() => ((uint[])Array)[Index];
            public ulong GetReferenceValueU8() => ((ulong[])Array)[Index];

            public void SetReferenceValue(object value) => ((object[])Array)[Index] = value;
            public void SetReferenceValueI1(sbyte value) => ((sbyte[])Array)[Index] = value;
            public void SetReferenceValueI2(short value) => ((short[])Array)[Index] = value;
            public void SetReferenceValueI4(int value) => ((int[])Array)[Index] = value;
            public void SetReferenceValueI8(long value) => ((long[])Array)[Index] = value;
            public void SetReferenceValueR4(float value) => ((float[])Array)[Index] = value;
            public void SetReferenceValueR8(double value) => ((double[])Array)[Index] = value;
        }

        #region GetValue
        object GetReferenceValue();
        byte GetReferenceValueU1();
        ushort GetReferenceValueU2();
        uint GetReferenceValueU4();
        ulong GetReferenceValueU8();
        sbyte GetReferenceValueI1();
        short GetReferenceValueI2();
        int GetReferenceValueI4();
        long GetReferenceValueI8();
        float GetReferenceValueR4();
        double GetReferenceValueR8();
        #endregion

        #region SetValue
        void SetReferenceValue(object value);
        void SetReferenceValueI1(sbyte value);
        void SetReferenceValueI2(short value);
        void SetReferenceValueI4(int value);
        void SetReferenceValueI8(long value);
        void SetReferenceValueR4(float value);
        void SetReferenceValueR8(double value);
        #endregion

        public static IByRef MakeByRefStack(StackData[] stack, int index)
        {
            return new ByRefStack(stack, index);
        }

        public static IByRef MakeByRefElement(Array array, int index)
        {
            return new ByRefElement(array, index);
        }
    }
}
