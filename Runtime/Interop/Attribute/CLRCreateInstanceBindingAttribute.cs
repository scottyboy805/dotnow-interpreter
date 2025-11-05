using dotnow.Reflection;
using System;
using System.Reflection;

namespace dotnow.Interop
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class CLRCreateInstanceBindingAttribute : Attribute
    {
        // Private
        private readonly Type declaringType;
        private readonly Type[] parameterTypes;

        // Properties
        public Type DeclaringType => declaringType;
        public Type[] ParameterTypes => parameterTypes;

        // Constructor
        public CLRCreateInstanceBindingAttribute(Type declaringType, params Type[] parameterTypes)
        {
            // Check for null
            if (declaringType == null)
                throw new ArgumentNullException(nameof(declaringType));

            this.declaringType = declaringType;
            this.parameterTypes = parameterTypes;
        }

        // Methods
        public virtual ConstructorInfo ResolveInitializerBinding()
        {
            // Get parameter types
            Type[] parameters = parameterTypes.Length == 0
                ? Type.EmptyTypes
                : parameterTypes;

            // Try to find the method
            return DeclaringType.GetConstructor(
                BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic,
                CLRType.Binder,
                parameters,
                null);
        }
    }
}
