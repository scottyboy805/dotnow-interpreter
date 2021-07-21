
namespace TrivialCLR.Runtime
{
    public interface IByRef
    {
        // Methods
        StackData GetReferenceValue();
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

        void SetReferenceValue(StackData value);
        void SetReferenceValueI1(sbyte value);
        void SetReferenceValueI2(short value);
        void SetReferenceValueI4(int value);
        void SetReferenceValueI8(long value);
        void SetReferenceValueR4(float value);
        void SetReferenceValueR8(double value);        
    }
}
