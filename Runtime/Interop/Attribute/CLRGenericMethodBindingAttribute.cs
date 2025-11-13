using dotnow.Reflection;
using System;
using System.Reflection;

namespace dotnow.Interop
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class CLRGenericMethodBindingAttribute : CLRMethodBindingAttribute
    {
        // Private
        private readonly int genericArgumentCount;

        // Properties
        public int GenericArgumentCount => genericArgumentCount;

        // Constructor
        public CLRGenericMethodBindingAttribute(Type declaringType, string methodName, int genericArgumentCount, params Type[] parameterTypes)
            : base(declaringType, methodName, parameterTypes)
        {
            // Check for none
            if (genericArgumentCount <= 0)
                throw new ArgumentException("Generic argument count must be greater that zero. If the method is not generic then you should use CLRMethodBindingAttribute");

            this.genericArgumentCount = genericArgumentCount;
        }

        // Methods
        public override MethodInfo ResolveMethodBinding()
        {
            // Get parameter types
            Type[] parameters = ParameterTypes.Length == 0
                ? Type.EmptyTypes
                : ParameterTypes;

            // Get methods
            return DeclaringType.GetMethod(MethodName,
                genericArgumentCount,
                BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic,
                CLRType.Binder,
                parameters,
                null);
        }
    }
}
