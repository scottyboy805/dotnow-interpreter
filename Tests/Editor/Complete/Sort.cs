#if DOTNOW_ENABLE_TESTS
using NUnit.Framework;
using System.Reflection;
using TestAssembly;

namespace dotnow.Complete
{
    [TestFixture]
    [Category("Complete Test")]
    public class Sort
    {
        [TestCase(new int[] { 800, 11, 50, 771, 649, 770, 240, 9 }, TestName = "Bubble Sort (8)")]
        [TestCase(new int[] { 456, 34, 567, 3, 36, 56, 5678, 2, 57532, 343 }, TestName = "Bubble Sort (10)")]
        public void TestBubbleSort(int[] values)
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestSort), nameof(TestSort.SortBubble));

            int[] actual = (int[])method.Invoke(null, new object[] { values });
            int[] expected = TestSort.SortBubble(values);

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestCase(new short[] { 800, 11, 50, 771, 649, 770, 240, 9 }, TestName = "Selection Sort (8)")]
        [TestCase(new short[] { 456, 34, 567, 3, 36, 56, 5678, 2, 5752, 343 }, TestName = "Selection Sort (10)")]
        public void TestSelectionSort(short[] values)
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestSort), nameof(TestSort.SelectionSort));

            short[] actual = (short[])method.Invoke(null, new object[] { values });
            short[] expected = TestSort.SelectionSort(values);

            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
#endif