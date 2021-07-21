using System;

namespace TrivialCLR.Runtime
{
    public struct ByRefElement : IByRef
    {
        public static bool longIndex = false;

        // Private
        private Array array;
        private long index;

        // Constructor
        public ByRefElement(Array array, int index)
        {
            this.array = array;
            this.index = index;
        }

        public ByRefElement(Array array, long index)
        {
            this.array = array;
            this.index = index;
        }

        // Methods
        public StackData GetReferenceValue()
        {
            // Get element type code
            TypeCode elementTypeCode = Type.GetTypeCode(array.GetType().GetElementType());

            StackData elem = new StackData();

            if (longIndex == false)
            {
                StackData.AllocTyped(ref elem, elementTypeCode, array.GetValue((int)index));
                return elem;
            }

            //return StackObject.AllocTyped(elementTypeCode, array.GetValue((int)index));

            StackData.AllocTyped(ref elem, elementTypeCode, array.GetValue(index));
            return elem;
            //return StackObject.AllocTyped(elementTypeCode, array.GetValue(index));
        }

        public byte GetReferenceValueU1()
        {
            return ((byte[])array)[longIndex == true ? index : (int)index];
        }

        public ushort GetReferenceValueU2()
        {
            return ((ushort[])array)[longIndex == true ? index : (int)index];
        }

        public uint GetReferenceValueU4()
        {
            return ((uint[])array)[longIndex == true ? index : (int)index];
        }

        public ulong GetReferenceValueU8()
        {
            return ((ulong[])array)[longIndex == true ? index : (int)index];
        }

        public sbyte GetReferenceValueI1()
        {
            return ((sbyte[])array)[longIndex == true ? index : (int)index];
        }

        public short GetReferenceValueI2()
        {
            return ((short[])array)[longIndex == true ? index : (int)index];
        }

        public int GetReferenceValueI4()
        {
            return ((int[])array)[longIndex == true ? index : (int)index];
        }

        public long GetReferenceValueI8()
        {
            return ((long[])array)[longIndex == true ? index : (int)index];
        }

        public float GetReferenceValueR4()
        {
            return ((float[])array)[longIndex == true ? index : (int)index];
        }

        public double GetReferenceValueR8()
        {
            return ((double[])array)[longIndex == true ? index : (int)index];
        }

        public void SetReferenceValue(StackData value)
        {
            // Get element type code
            TypeCode elementTypeCode = Type.GetTypeCode(array.GetType().GetElementType());

            if (longIndex == false)
            {
                array.SetValue(value.UnboxAsType(elementTypeCode), (int)index);
                return;
            }

            array.SetValue(value.UnboxAsType(elementTypeCode), index);
        }

        public void SetReferenceValueI1(sbyte value)
        {
            ((sbyte[])array)[longIndex == true ? index : (int)index] = value;
        }

        public void SetReferenceValueI2(short value)
        {
            ((short[])array)[longIndex == true ? index : (int)index] = value;
        }

        public void SetReferenceValueI4(int value)
        {
            ((int[])array)[longIndex == true ? index : (int)index] = value;
        }

        public void SetReferenceValueI8(long value)
        {
            ((long[])array)[longIndex == true ? index : (int)index] = value;
        }

        public void SetReferenceValueR4(float value)
        {
            ((float[])array)[longIndex == true ? index : (int)index] = value;
        }

        public void SetReferenceValueR8(double value)
        {
            ((double[])array)[longIndex == true ? index : (int)index] = value;
        }
    }
}
