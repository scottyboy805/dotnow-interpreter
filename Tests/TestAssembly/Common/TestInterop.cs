
namespace TestAssembly
{
    public class TestInterop
    {
        public static bool UnmanagedPrimitive_Argument()
        {
            return float.IsNegative(1f);
        }

        public static decimal UnmanagedStruct_Argument()
        {
            decimal d = 23;
            decimal.Negate(d);

            return d;
        }
    }
}
