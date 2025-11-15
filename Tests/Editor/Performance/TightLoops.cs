#if DOTNOW_ENABLE_TESTS && DOTNOW_ENABLE_PERFORMANCE_TESTS
using NUnit.Framework;
using System.Reflection;
using TestAssembly;
using Unity.PerformanceTesting;

namespace dotnow.Performance
{
    [TestFixture]
    [Category("Performance Test")]
    public class TightLoops
    {
        private const int WARMUP_ITERATIONS = 5;
        private const int MEASUREMENT_ITERATIONS = 15;

        [Test]
        public void TestTightLoopsCorrectness()
        {
            // Load the dotnow method
            MethodInfo dotNowMethod = TestUtils.LoadTestMethod(nameof(TestTightLoops), nameof(TestTightLoops.Arithmetic));

            // Test for correctness
            double expected = TestTightLoops.Arithmetic();
            double actual = (double)dotNowMethod.Invoke(null, null);

            Assert.AreEqual(expected, actual, 1e-10, "dotnow and Mono/JIT should produce identical results");
        }

        [Test, Performance]
        public void TestTightLoopsMonoPerformance()
        {
            // Measure Mono/JIT performance
            Measure.Method(() => TestTightLoops.Arithmetic())
             .SampleGroup("MonoJIT")
               .WarmupCount(WARMUP_ITERATIONS)
                        .MeasurementCount(MEASUREMENT_ITERATIONS)
                   .IterationsPerMeasurement(1)
            .GC()
            .Run();
        }

        [Test, Performance]
        public void TestTightLoopsDotNowPerformance()
        {
            // Load the dotnow method
            MethodInfo dotNowMethod = TestUtils.LoadTestMethod(nameof(TestTightLoops), nameof(TestTightLoops.Arithmetic));

            // Verify correctness first
            double monoResult = TestTightLoops.Arithmetic();
            double dotNowResult = (double)dotNowMethod.Invoke(null, null);
            Assert.AreEqual(monoResult, dotNowResult, 1e-10, "Results should be identical between Mono and dotnow");

            // Measure dotnow performance
            Measure.Method(() => dotNowMethod.Invoke(null, null))
    .SampleGroup("DotNow")
            .WarmupCount(WARMUP_ITERATIONS)
           .MeasurementCount(MEASUREMENT_ITERATIONS)
              .IterationsPerMeasurement(1)
         .GC()
    .Run();
        }
    }
}
#endif
