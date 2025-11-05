
namespace dotnow.Runtime
{
    public struct ByRefVariable : IByRef
    {
        // Private
        private StackData[] stack;
        private int varOffset;

        // Properties
        public object Instance
        {
            get { return stack[varOffset].Ref; }
        }

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
            return (byte)stack[varOffset].Int32;
        }

        public ushort GetReferenceValueU2()
        {
            return (ushort)stack[varOffset].Int32;
        }

        public uint GetReferenceValueU4()
        {
            return (uint)stack[varOffset].Int32;
        }

        public ulong GetReferenceValueU8()
        {
            return (ulong)stack[varOffset].Int64;
        }

        public sbyte GetReferenceValueI1()
        {
            return (sbyte)stack[varOffset].Int32;
        }

        public short GetReferenceValueI2()
        {
            return (short)stack[varOffset].Int32;
        }

        public int GetReferenceValueI4()
        {
            return stack[varOffset].Int32;
        }

        public long GetReferenceValueI8()
        {
            return stack[varOffset].Int64;
        }

        public float GetReferenceValueR4()
        {
            return stack[varOffset].Single;
        }

        public double GetReferenceValueR8()
        {
            return stack[varOffset].Double;
        }

        public void SetReferenceValue(StackData value)
        {
            stack[varOffset] = value;
        }

        public void SetReferenceValueI1(sbyte value)
        {
            stack[varOffset].Int32 = value;
        }

        public void SetReferenceValueI2(short value)
        {
            stack[varOffset].Int32 = value;
        }

        public void SetReferenceValueI4(int value)
        {
            stack[varOffset].Int32 = value;
        }

        public void SetReferenceValueI8(long value)
        {
            stack[varOffset].Int64 = value;
        }

        public void SetReferenceValueR4(float value)
        {
            stack[varOffset].Single = value;
        }

        public void SetReferenceValueR8(double value)
        {
            stack[varOffset].Double = value;
        }

        public override string ToString()
        {
            return string.Format("ByRef({0})", GetReferenceValue());
        }
    }
}
