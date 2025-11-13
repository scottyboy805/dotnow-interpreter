using System;
using System.Reflection;

namespace dotnow.Interop
{
    internal static class __delegate
    {
        // Methods
        public static Delegate CreateDelegate(Type delegateType, MethodBase targetMethod)
        {
            Func<int> d = () =>
            {
                return (int)targetMethod.Invoke(null, null);
            };
            return d;
        }

        //public Delegate CreateDelegateBinding()
        //{
        //    Func<int> d = () =>
        //    {
        //        return (int)targetMethod.Invoke(null, null);
        //    };
        //    return d;
        //}
    }
}
