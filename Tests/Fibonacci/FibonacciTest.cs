#if !UNITY_DISABLE
#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_IOS || UNITY_ANDROID || UNITY_WSA || UNITY_WEBGL || UNITY_SWITCH)
using System.Reflection;
using UnityEngine;

namespace dotnow.Tests
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
#endif
#endif