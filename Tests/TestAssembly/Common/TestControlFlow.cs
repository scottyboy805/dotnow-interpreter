namespace TestAssembly
{
    public static class TestControlFlow
    {
        // Simple if-else tests
        public static object[] TestIfElse()
        {
            int a = 10;
            int b = 20;
            object result1 = null;
            object result2 = null;
            object result3 = null;

            if (a > b)
                result1 = "a is greater";
            else
                result1 = "b is greater or equal";

            if (a == 10)
                result2 = "a is ten";

            if (a > 0)
                result3 = "positive";
            else if (a < 0)
                result3 = "negative";
            else
                result3 = "zero";

            return new object[] { result1, result2, result3 };
        }

        // Switch statement tests
        public static object[] TestSwitch()
        {
            object result1 = null;
            object result2 = null;
            object result3 = null;

            int value1 = 1;
            int value2 = 2;
            int value3 = 99;

            switch (value1)
            {
                case 1:
                    result1 = "one";
                    break;
                case 2:
                    result1 = "two";
                    break;
                default:
                    result1 = "other";
                    break;
            }

            switch (value2)
            {
                case 1:
                    result2 = "one";
                    break;
                case 2:
                    result2 = "two";
                    break;
                default:
                    result2 = "other";
                    break;
            }

            switch (value3)
            {
                case 1:
                    result3 = "one";
                    break;
                case 2:
                    result3 = "two";
                    break;
                default:
                    result3 = "other";
                    break;
            }

            return new object[] { result1, result2, result3 };
        }

        // For loop tests
        public static object[] TestForLoop()
        {
            int sum = 0;
            int count = 0;

            for (int i = 0; i < 5; i++)
            {
                sum += i;
                count++;
            }

            int sum2 = 0;
            for (int i = 1; i <= 3; i++)
            {
                sum2 += i * 2;
            }

            return new object[] { sum, count, sum2 };
        }

        // While loop tests
        public static object[] TestWhileLoop()
        {
            int sum = 0;
            int i = 0;

            while (i < 5)
            {
                sum += i;
                i++;
            }

            int factorial = 1;
            int n = 4;
            while (n > 0)
            {
                factorial *= n;
                n--;
            }

            return new object[] { sum, factorial };
        }

        // Do-while loop tests
        public static object[] TestDoWhileLoop()
        {
            int sum = 0;
            int i = 0;

            do
            {
                sum += i;
                i++;
            } while (i < 3);

            int count = 0;
            int j = 5;
            do
            {
                count++;
                j--;
            } while (j > 0);

            return new object[] { sum, count };
        }

        // Break and continue tests
        public static object[] TestBreakContinue()
        {
            int sum1 = 0;
            int sum2 = 0;

            // Test break
            for (int i = 0; i < 10; i++)
            {
                if (i == 5)
                    break;
                sum1 += i;
            }

            // Test continue
            for (int i = 0; i < 10; i++)
            {
                if (i % 2 == 0)
                    continue;
                sum2 += i;
            }

            return new object[] { sum1, sum2 };
        }

        // Nested loops test
        public static object[] TestNestedLoops()
        {
            int sum = 0;

            for (int i = 1; i <= 3; i++)
            {
                for (int j = 1; j <= 2; j++)
                {
                    sum += i * j;
                }
            }

            return new object[] { sum };
        }
    }
}