using System;
using System.IO;
using System.Reflection;
using dotnow.Reflection;

namespace dotnow.Tests
{
    public class TestUtils
    {
        public static CLRType LoadTestType(string typeName)
        {
            AppDomain domain;
            return LoadTestType(typeName, out domain);
        }

        public static CLRType LoadTestType(string typeName, out AppDomain domain)
        {
            domain = new AppDomain();

            // Load the stream
            Stream moduleStream = File.OpenRead("../../../TestModule/bin/Debug/TestModule.dll");

            // Load the module
            CLRModule module = domain.LoadModuleStream(moduleStream, true);

            // Get type
            return module.GetType(typeName) as CLRType;
        }

        public static CLRMethod LoadTestMethod(string typeName, string methodName, BindingFlags flags = BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
        {
            AppDomain domain;
            return LoadTestMethod(typeName, methodName, out domain, flags);
        }

        public static CLRMethod LoadTestMethod(string typeName, string methodName, out AppDomain domain, BindingFlags flags = BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
        {
            CLRType type = LoadTestType(typeName, out domain);

            if (type != null)
            {
                CLRMethod method = type.GetMethod(methodName, flags) as CLRMethod;

                if (method != null)
                    return method;
            }

            throw new MissingMethodException(string.Format("Target method '{0}' was not found", methodName));
        }
    }
}
