using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TrivialCLR.Reflection;

namespace TrivialCLR.Runtime.JIT
{
    public interface IJITOptimizable
    {
        // Methods
        void EnsureJITOptimized();
    }

    public static class JITOptimize
    {
        // Methods
        //public static void EnsureJITOptimized(CLRModule module)
        //{

        //}

        public static void EnsureJITOptimized(Type type)
        {
            if (type is IJITOptimizable)
                (type as IJITOptimizable).EnsureJITOptimized();
        }

        public static void EnsureJITOptimized(MethodBase method)
        {
            if (method is IJITOptimizable)
                (method as IJITOptimizable).EnsureJITOptimized();
        }

        public static void EnsureJITOptimized(CLRMethodBodyBase body)
        {
            if (body is IJITOptimizable)
                (body as IJITOptimizable).EnsureJITOptimized();
        }
    }
}
