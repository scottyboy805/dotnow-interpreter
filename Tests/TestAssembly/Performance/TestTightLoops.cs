namespace TestAssembly
{
    public class TestTightLoops
    {
        public static double Arithmetic()
        {
            double result = 1.0;
            long iterations = 500_000;

            for (long i = 1; i <= iterations; i++)
            {
                result += i;
                result -= i;
                result *= 1.000001;
                result /= 1.000001;
            }
            
            return result; // Return the result to prevent optimization
        }
    }
}
