using System;
using System.Reflection;

namespace TestAssembly
{
    public class TestMethodInfo
    {
        // Test various method types for reflection
        public static void StaticVoidMethod() { }

        public static int StaticIntMethod() => 42;

        public static string StaticStringMethod(string input) => input;

        public static T StaticGenericMethod<T>(T input) => input;

        public void InstanceVoidMethod() { }

        public int InstanceIntMethod() => 123;

        public string InstanceStringMethod(string input) => input;

        public virtual string VirtualMethod() => "virtual";

        protected void ProtectedMethod() { }

        private void PrivateMethod() { }

        internal void InternalMethod() { }

        public static void MethodWithMultipleParameters(int a, string b, bool c, double d) { }

        public static void MethodWithRefParameter(ref int value) { value = 999; }

        public static void MethodWithOutParameter(out string value) { value = "out"; }

        public static void MethodWithInParameter(in DateTime value) { }

        public static void MethodWithParamsParameter(params object[] values) { }

        public static void OverloadedMethod() { }

        public static void OverloadedMethod(int value) { }

        public static void OverloadedMethod(string value) { }

        // Constructor for testing
        public TestMethodInfo() { }

        public TestMethodInfo(int value) { }

        // Properties for testing
        public string TestProperty { get; set; }

        public int ReadOnlyProperty { get; private set; }
    }

    public abstract class AbstractTestClass
    {
        public abstract string AbstractMethod();
    }

    public class ConcreteTestClass : AbstractTestClass
    {
        public override string AbstractMethod() => "implemented";

        public virtual string VirtualMethod() => "base";
    }

    public class DerivedTestClass : ConcreteTestClass
    {
        public override string VirtualMethod() => "overridden";
    }
}
