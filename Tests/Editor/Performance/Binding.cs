using NUnit.Framework;
using System.Reflection;
using TestAssembly;
using Unity.PerformanceTesting;
using UnityEngine;

namespace dotnow.Performance
{
    [TestFixture]
    [Category("Performance Test")]
    public class Binding
    {
        private const int WARMUP_ITERATIONS = 5;
        private const int MEASUREMENT_ITERATIONS = 15;

        [Test, Performance]
        public void TestMonoPerformance()
        {
            GameObject go = new();

            // Measure Mono/JIT performance
            Measure.Method(() => TestBinding.TestTransformBinding(go.transform))
             .SampleGroup("MonoJIT")
               .WarmupCount(WARMUP_ITERATIONS)
                        .MeasurementCount(MEASUREMENT_ITERATIONS)
                   .IterationsPerMeasurement(1)
            .GC()
            .Run();
        }

        [Test, Performance]
        public void TestDotNowReflectionPerformance()
        {
            // Disable bindings for this test
            AppDomain domain = new AppDomain(AppDomainOptions.DisableDirectCallBindings);

            GameObject go = new();

            // Load the dotnow method
            MethodInfo dotNowMethod = TestUtils.LoadTestMethod(nameof(TestBinding), nameof(TestBinding.TestTransformBinding), domain);

            // Measure dotnow performance
            Measure.Method(() => dotNowMethod.Invoke(null, new object[] { go.transform }))
                .SampleGroup("DotNow")
                .WarmupCount(WARMUP_ITERATIONS)
                .MeasurementCount(MEASUREMENT_ITERATIONS)
                .IterationsPerMeasurement(1)
                .GC()
                .Run();
        }

        [Test, Performance]
        public void TestDotNowDirectCallBindingPerformance()
        {
            // Do not disable bindings for this test
            AppDomain domain = new AppDomain();

            GameObject go = new();

            // Load the dotnow method
            MethodInfo dotNowMethod = TestUtils.LoadTestMethod(nameof(TestBinding), nameof(TestBinding.TestTransformBinding), domain);

            // Measure dotnow performance
            Measure.Method(() => dotNowMethod.Invoke(null, new object[] { go.transform }))
                .SampleGroup("DotNow")
                .WarmupCount(WARMUP_ITERATIONS)
                .MeasurementCount(MEASUREMENT_ITERATIONS)
                .IterationsPerMeasurement(1)
                .GC()
                .Run();
        }
    }
}
