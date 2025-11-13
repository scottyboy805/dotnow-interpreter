
namespace TestAssembly
{
    public static class TestSort
    {
        public static int[] SortBubble(int[] values)
        {
            int temp = 0;

            for (int write = 0; write < values.Length; write++)
            {
                for (int sort = 0; sort < values.Length - 1; sort++)
                {
                    if (values[sort] > values[sort + 1])
                    {
                        temp = values[sort + 1];
                        values[sort + 1] = values[sort];
                        values[sort] = temp;
                    }
                }
            }
            return values;
        }

        public static short[] SelectionSort(short[] arr)
        {
            int n = arr.Length;
            for (int i = 0; i < n - 1; i++)
            {
                int minIndex = i;
                for (int j = i + 1; j < n; j++)
                {
                    if (arr[j] < arr[minIndex])
                    {
                        minIndex = j;
                    }
                }
                short temp = arr[i];
                arr[i] = arr[minIndex];
                arr[minIndex] = temp;
            }
            return arr;
        }
    }
}
