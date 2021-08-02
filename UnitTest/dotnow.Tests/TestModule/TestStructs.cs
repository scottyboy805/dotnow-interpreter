using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestModule
{
    public class TestStructs
    {
        struct SimpleStruct
        {
            public int number;
        }

        public static int TestSetStructField()
        {
            SimpleStruct s = new SimpleStruct();
            s.number = 1234;

            return s.number;
        }

        public static int TestGetStructField()
        {
            SimpleStruct s = new SimpleStruct();
            s.number = 1234;

            int local = s.number;

            return local;
        }

        public static int TestPassStruct()
        {
            SimpleStruct s = new SimpleStruct();
            s.number = 5;

            ModifyStructValue(s);

            return s.number;
        }

        private static SimpleStruct ModifyStructValue(SimpleStruct val)
        {
            val.number = 10;
            return val;
        }
    }
}
