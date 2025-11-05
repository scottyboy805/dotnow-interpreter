
namespace TestAssembly
{
    public class TestArray
    {
        public static object TestUnmanagedValueTypeArray()
        {
            decimal[] arr =
            {
                10, 20, 30, 40,
            };
            return arr;
        }

        public static object TestPrimitiveTypeArray()
        {
            return new int[]
            {
                5, 10, 15, 20,
            };
        }

        public static object TestMultidimensionalPrimitiveTypeArray()
        {
            return new int[,]
            {
                { 5, 10 },
                { 15, 20 }
            };
        }
    }
}
