using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestModule
{
    public class TestBranching
    {
        public static bool TestGoto()
        {
            bool result = false;

            if(result == false)
                goto skip;
            result = true;
            
        skip:
            return result;
        }

        public static bool TestBranch1()
        {
            int a = 0;

            if (a == 0)
                return true;

            return false;
        }

        public static bool TestBranch2()
        {
            bool a = true;

            if (a == true)
                return true;

            return false;
        }

        public static bool TestBranch3()
        {
            int a = 1;

            if (a == 1)
                return true;

            return false;
        }

        public static bool TestBranch4()
        {
            int a = 10;

            if (a < 100)
                return true;

            return false;
        }

        public static bool TestBranch5()
        {
            int a = 10;

            if (a <= 100)
                return true;

            return false;
        }

        public static bool TestBranch6()
        {
            int a = 10;

            if (a > 0)
                return true;

            return false;
        }

        public static bool TestBranch7()
        {
            int a = 10;

            if (a >= 0)
                return true;

            return false;
        }

        public static bool TestBranch8()
        {
            object a = new object();

            if (a != null)
                return true;

            return false;
        }

        public static bool TestBranch9()
        {
            object a = null;

            if (a == null)
                return true;

            return false;
        }

        public static int TestSwitch1()
        {
            int a = 10;

            switch(a)
            {
                case 0: return 0;
                case 1: return 1;
                case 2: return 2;
                case 3: return 2;
                case 4: return 4;
                case 5: return 5;
                case 6: return 6;
                case 7: return 7;
                case 8: return 8;
                case 9: return 9;
                case 10: return 10;
            }

            return 1;
        }

        public static bool TestSwitch2()
        {
            string test = "hello";

            switch (test)
            {
                case "hi": return false;
                case "bye": return false;
                case "hello world": return false;
                case "goodbye": return false;
                case "wave": return false;
                case "hello": return true;
            }
            return false;
        }
    }
}
