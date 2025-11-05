using System;
using System.Collections.Generic;

namespace TestAssembly
{
    public class TestGeneric
    {
        public static object TestInteropGenericType()
        {
            return new List<int>
            {
                5, 6, 7
            };
        }

        public static object TestInteropGenericMethod()
        {
            int[] arr = { 5, 6, 7 };
            Array.Fill(arr, 10);

            return arr;
        }
    }
}
