
namespace TestModule
{
    public class TestMisc
    {
        public static string TestNameof()
        {
            int variable = 10;

            return nameof(variable);
        }

        public static int TestSizeof()
        {
            return sizeof(int);
        }
    }
}
