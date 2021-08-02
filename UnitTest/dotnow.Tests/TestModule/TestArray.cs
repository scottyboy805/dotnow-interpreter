using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestModule
{
    public class TestArray
    {
        public static object TestArrayAllocation1()
        {
            int[] array = new int[10];

            return array.Length;
        }

        public static object TestArrayAllocation2()
        {
            int[] array =
            {
                1, 2, 3, 4, 5, 6, 7, 8, 9, 10
            };

            return array[4];
        }
    }
}
