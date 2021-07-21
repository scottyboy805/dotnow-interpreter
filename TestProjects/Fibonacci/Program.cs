
namespace Fibonacci
{
    public class Program
    {
        public static int Main(int n)
        {
            int a = 0;
            int b = 1;
            // In N steps, compute Fibonacci sequence iteratively.
            for (int i = 0; i < n; i++)
            {
                int temp = a;
                a = b;
                b = temp + b;
            }
            return a;
        }
    }
}
