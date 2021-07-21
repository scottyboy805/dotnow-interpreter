using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using TrivialCLR.Reflection;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace TrivialCLR.Tests
{
    public class PerformanceTest : MonoBehaviour
    {
        // Private
        private AppDomain domain = null;

        // Public
        public TextAsset assemblyImage;
        [Tooltip("Note that tests may run faster the second time around due to JIT optimization statges for interpreter and JIT compilation for mono")]
        public int testIterations = 1;

        // Properties
        public virtual string TestName
        {
            get { return GetType().Name; }
        }

        public AppDomain InterpretedAppDomain
        {
            get { return domain; }
        }

        // Methods
        private void Start()
        {
            // Check for assembly image
            if (assemblyImage == null)
            {
                Debug.LogError("Assembly image has not been assigned!");
                return;
            }

            // Create domain
            domain = new AppDomain();

            // Trigger on before
            OnBeforeRunTest();

            // Run tests
            for (int i = 0; i < testIterations; i++)
            {
                Debug.Log("Starting tests: [" + TestName + "]" + ((testIterations > 1) ? (" test iteration: " + (i + 1)) : ""));
                RunTestInterpreted();
                RunTestJitted();
            }
        }

        private void RunTestInterpreted()
        {
            // Load module
            CLRModule module = domain.LoadModuleStream(new MemoryStream(assemblyImage.bytes), false);

            // Get target type
            Type mainType = module.GetTypes()
                .Where(t => t.Name == "Program")
                .FirstOrDefault();

            // Check for error
            if (mainType == null)
            {
                Debug.LogError("Failed to load main type: Program");
                return;
            }

            // Get main method
            MethodInfo mainMethod = mainType.GetMethod("Main", BindingFlags.Public | BindingFlags.Static);

            if (mainMethod == null)
            {
                Debug.LogError("Failed to load main method: Main");
                return;
            }

            // Run the test
            RunTestTimed(mainMethod, true);
        }

        private void RunTestJitted()
        {
            // Load using mono
            Assembly jitAssembly = Assembly.Load(assemblyImage.bytes);

            // Get target type
            Type mainType = jitAssembly.GetTypes()
                .Where(t => t.Name == "Program")
                .FirstOrDefault();

            // Check for error
            if (mainType == null)
            {
                Debug.LogError("Failed to load main type: Program");
                return;
            }

            // Get main method
            MethodInfo mainMethod = mainType.GetMethod("Main", BindingFlags.Public | BindingFlags.Static);

            if (mainMethod == null)
            {
                Debug.LogError("Failed to load main method: Main");
                return;
            }

            // Run the test
            RunTestTimed(mainMethod, false);
        }

        private void RunTestTimed(MethodInfo targetMethod, bool interpreted)
        {
            Stopwatch timer = Stopwatch.StartNew();

            // Invoke the test method
            RunTest(targetMethod);

            timer.Stop();
            Debug.Log(((interpreted == true) ? "(Interpreted): " : "(Jitted): ") + "Test completed in: " + timer.ElapsedMilliseconds + "ms");
        }

        protected virtual void OnBeforeRunTest() { }

        protected virtual void RunTest(MethodInfo method)
        {
            method.Invoke(null, null);
        }
    }
}
