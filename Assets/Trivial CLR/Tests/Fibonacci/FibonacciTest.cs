using System.Reflection;
using UnityEngine;

namespace TrivialCLR.Tests
{
    public class FibonacciTest : PerformanceTest
    {
        // Public
        public int iterations = 100;

        // Methods
        protected override void RunTest(MethodInfo method)
        {
            object result = method.Invoke(null, new object[] { iterations });

            Debug.Log("Test output: " + result);
        }
    }
}
