using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestModule
{
    public class TestExceptions
    {
        public static object TestThrowException()
        {
            throw new Exception();
        }

        public static object TestRethrowException()
        {
            try
            {
                throw new Exception();
            }
            catch
            {
                throw;
            }
        }

        public static object TestFinally()
        {
            try
            {
                throw new Exception();
            }
            finally
            {
                Exception e = new Exception();
            }
        }
    }
}
