using dotnow.Reflection;
using System;
using System.IO;
using System.Reflection;

namespace dotnow
{
    public static class TestUtils
    {
        // Public
        public static Assembly TestAssembly = Assembly.Load("dotnow.TestAssembly");

        // Methods
        public static Type LoadTestType(string typeName, AppDomain domain = null)
        {
            // Create domain
            if (domain == null)
                domain = new AppDomain();

            // Get module
            string modulePath = TestAssembly.Location;

            // Load module in domain
            Assembly asm = domain.LoadModule(modulePath, false, false);

            return asm.GetType(typeName);
        }

        public static MethodInfo LoadTestMethod(string typeName, string methodName, AppDomain domain = null)
        {
            // Create domain
            if (domain == null)
                domain = new AppDomain();

            // Get module
            string modulePath = TestAssembly.Location;

            // Load module in domain
            Assembly asm = domain.LoadModule(modulePath, false, false);

            return asm.GetType(typeName).GetMethod(methodName);
        }
    }
}
