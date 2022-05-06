
namespace dotnow.Runtime
{
    public struct ByRefVariable : IByRef
    {
        // Private
        private StackData[] stack;
        private int varOffset;

        // Constructor
        public ByRefVariable(StackData[] stack, int varOffset)
        {
            this.stack = stack;
            this.varOffset = varOffset;
        }

        // Methods
        public StackData GetReferenceValue()
        {
            return stack[varOffset];
        }

        public byte GetReferenceValueU1()
        {
            return (byte)stack[varOffset].value.Int8;
        }

        public ushort GetReferenceValueU2()
        {
            return (ushort)stack[varOffset].value.Int16;
        }

        public uint GetReferenceValueU4()
        {
            return (uint)stack[varOffset].value.Int32;
        }

        public ulong GetReferenceValueU8()
        {
            return (ulong)stack[varOffset].value.Int64;
        }

        public sbyte GetReferenceValueI1()
        {
            return stack[varOffset].value.Int8;
        }

        public short GetReferenceValueI2()
        {
            return stack[varOffset].value.Int16;
        }

        public int GetReferenceValueI4()
        {
            return stack[varOffset].value.Int32;
        }

        public long GetReferenceValueI8()
        {
            return stack[varOffset].value.Int64;
        }

        public float GetReferenceValueR4()
        {
            return stack[varOffset].value.Single;
        }

        public double GetReferenceValueR8()
        {
            return stack[varOffset].value.Double;
        }

        public void SetReferenceValue(StackData value)
        {
            stack[varOffset] = value;
        }

        public void SetReferenceValueI1(sbyte value)
        {
            stack[varOffset].value.Int8 = value;
        }

        public void SetReferenceValueI2(short value)
        {
            stack[varOffset].value.Int16 = value;
        }

        public void SetReferenceValueI4(int value)
        {
            stack[varOffset].value.Int32 = value;
        }

        public void SetReferenceValueI8(long value)
        {
            stack[varOffset].value.Int64 = value;
        }

        public void SetReferenceValueR4(float value)
        {
            stack[varOffset].value.Single = value;
        }

        public void SetReferenceValueR8(double value)
        {
            stack[varOffset].value.Double = value;
        }

        public override string ToString()
        {
            return string.Format("ByRef({0})", GetReferenceValue());
        }
    }
}
