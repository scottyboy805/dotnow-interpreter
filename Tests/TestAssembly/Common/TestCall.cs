using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAssembly
{
    public static class TestCall
    {
        public class ClassC
        {
            public virtual int GetNumber()
            {
                return 100;
            }
        }

        public class ClassB : ClassC
        {
            public override int GetNumber()
            {
                return 200;
            }
        }

        public class ClassA : ClassB
        {
            public override int GetNumber()
            {
                return 300;
            }
        }

        public static int CallSimple(int value)
        {
            return CallConstant(value);
        }

        public static int CallConstant(int value)
        {
            return value + value;
        }

        public static string CallObject(object value)
        {
            return value.ToString();
        }

        public static object[] CallVirtual()
        {
            return new object[]
            {
                new ClassC().GetNumber(),
                new ClassB().GetNumber(),
                new ClassA().GetNumber(),
            };
        }

        public static int TestStaticDelegate()
        {
            return 123;
        }
    }
}
