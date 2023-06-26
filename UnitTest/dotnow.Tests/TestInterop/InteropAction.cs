using System;
using System.Collections.Generic;

namespace TestInterop
{
    public class InteropAction
    {
        public static object value = null;

        public static void ActionNoArguments()
        {
            value = 10;
        }

        public static void ActionPrimitive(int i)
        {
            value = i;
        }

        public static void ActionString(string i)
        {
            value = i;
        }

        public static void ActionBoxed(object i)
        {
            value = (int)i;
        }

        public static void ActionInteropObject(Tuple<int, int> i)
        {
            value = i.Item1 + i.Item2;
        }

        public static void ActionInteropStruct(KeyValuePair<int, int> i)
        {
            value = i.Key + i.Value;
        }
    }
}
