using System;
using System.Collections.Generic;
using System.Reflection;
using dotnow.Reflection;

namespace dotnow.Interop
{
    internal class CLRSpecialMethods
    {
        // Private
        private static ConstructorInfo _arrayRank_Initializer_Impl = null;
        private static MethodInfo _arrayRank_Get_Impl = null;
        private static MethodInfo _arrayRank_Set_Impl = null;

        // Methods
        public static ConstructorInfo GetSpecialInitializer(AppDomain domain, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
        {
            if(types.Length >= 2 && types[0] == typeof(int) && types[1] == typeof(int))
            {
                if(_arrayRank_Initializer_Impl == null)
                {
                    _arrayRank_Initializer_Impl = new CLRConstructorBindingCallSite(domain,
                        new CLRAbstractMethodInfo(".ctor", typeof(void), types, MethodAttributes.Public),
                        typeof(CLRSpecialMethods).GetMethod(nameof(_SpecialRuntime_ArrayRank_Initializer), BindingFlags.Static | BindingFlags.NonPublic));
                }
                return _arrayRank_Initializer_Impl;
            }
            return null;
        }

        public static MethodInfo GetSpecialMethod(AppDomain domain, Type type, string methodName, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
        {
            // Array rank get
            if(methodName == "Get" && types.Length >= 2 && types[0] == typeof(int) && types[1] == typeof(int))
            {
                if(_arrayRank_Get_Impl == null)
                {
                    // Create the method redirect
                    _arrayRank_Get_Impl = new CLRMethodBindingCallSite(domain,
                        new CLRAbstractMethodInfo("Get", type.GetElementType(), types, MethodAttributes.Public),
                        typeof(CLRSpecialMethods).GetMethod(nameof(_SpecialRuntime_ArrayRank_Get), BindingFlags.Static | BindingFlags.NonPublic));
                }
                return _arrayRank_Get_Impl;
            }

            // Array rank set
            if(methodName == "Set" && types.Length >= 3 && types[0] == typeof(int) && types[1] == typeof(int))
            {
                if(_arrayRank_Set_Impl == null)
                {
                    // Creaye the method redirect
                    _arrayRank_Set_Impl = new CLRMethodBindingCallSite(domain,
                        new CLRAbstractMethodInfo("Set", typeof(void), types, MethodAttributes.Public),
                        typeof(CLRSpecialMethods).GetMethod(nameof(_SpecialRuntime_ArrayRank_Set), BindingFlags.Static | BindingFlags.NonPublic));
                }
                return _arrayRank_Set_Impl;
            }
            return null;
        }

        private static object _SpecialRuntime_ArrayRank_Initializer(AppDomain domain, MethodInfo overrideMethod, object instance, object[] args)
        {
            return null;
        }

        private static object _SpecialRuntime_ArrayRank_Get(AppDomain domain, MethodInfo overrideMethod, object instance, object[] args)
        {
            // Get array
            Array a = instance as Array;

            // Get indicies
            int[] indexes = new int[args.Length];

            for (int i = 0; i < args.Length; i++)
                indexes[i] = (int)args[i];

            // Get the value
            return a.GetValue(indexes);
        }

        private static void _SpecialRuntime_ArrayRank_Set(AppDomain domain, MethodInfo overrideMethod, object instance, object[] args)
        {
            // Get array
            Array a = instance as Array;
            object value = args[args.Length - 1];

            // Get indicies
            int[] indexes = new int[args.Length - 1];

            for (int i = 0; i < indexes.Length; i++)
                indexes[i] = (int)args[i];

            // Set the value
            a.SetValue(value, indexes);
        }
    }
}
