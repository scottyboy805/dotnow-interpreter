using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestModule
{
    public class TestVariables
    {
        public class TestInstance
        {
            public int a;
        }

        public static int TestValArgument()
        {
            int value = 5;

            ModifyIntVal(value);

            return value;
        }

        public static int TestRefArgument()
        {
            int value = 5;

            ModifyIntRef(ref value);

            return value;
        }

        public static int TestRefArgumentMultiple()
        {
            int value = 5;

            RefPassThrough(ref value);

            return value;
        }

        public static int TestRefArgumentExternal(ref int value)
        {
            value = 5;

            ModifyIntRef(ref value);

            return value;
        }

        public static int TestRefField()
        {
            TestInstance instance = new TestInstance();
            instance.a = 5;

            ModifyIntRef(ref instance.a);

            return instance.a;
        }

        private static void RefPassThrough(ref int value)
        {
            ModifyIntRef(ref value);
        }

        private static void ModifyIntVal(int value)
        {
            value = 10;
        }

        private static void ModifyIntRef(ref int value)
        {
            int test = value;

            value = 10;
        }
    }
}
