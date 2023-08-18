

namespace TestModule
{
    public class TestForLoop
    {
        public static void TestForLoopInt32()
        {
            int a = 0;

            for (int i = 0; i < 100000; i++)
                a += 2;
        }
    }
}
