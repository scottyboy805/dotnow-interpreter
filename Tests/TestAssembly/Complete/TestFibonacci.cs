namespace TestAssembly
{
    public static class TestFibonacci
    {
        public static int Fibonacci_Iterative(int len)
        {
            int a = 0, b = 1, c = 0;
            for (int i = 2; i < len; i++)
            {
                c = a + b;
                a = b;
                b = c;
            }
            return c;
        }

        public static int Fibonacci_Recursive(int n)
        {
            if (n == 0)
            {
                return 1;
            }
            if (n == 1)
            {
                return 1;
            }
            else
            {
                return Fibonacci_Recursive(n - 2) + Fibonacci_Recursive(n - 1);
            }
        }
    }
}
