using dotnow.Reflection;
using System;
using System.Reflection;

namespace dotnow.Interop
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class CLRMethodBindingAttribute : Attribute
    {
        // Private
        private readonly Type declaringType;
        private readonly string methodName;
        private readonly Type[] parameterTypes;

        // Properties
        public Type DeclaringType => declaringType;
        public string MethodName => methodName;
        public Type[] ParameterTypes => parameterTypes;

        // Constructor
        public CLRMethodBindingAttribute(Type declaringType, string methodName, params Type[] parameterTypes)
        {
            // Check for null
            if(declaringType == null)
                throw new ArgumentNullException(nameof(declaringType));

            // Check for name
            if (string.IsNullOrEmpty(methodName) == true)
                throw new ArgumentException("Method name cannot be null or empty");

            this.declaringType = declaringType;
            this.methodName = methodName;
            this.parameterTypes = parameterTypes;
        }

        // Methods
        public virtual MethodInfo ResolveMethodBinding()
        {
            // Get parameter types
            Type[] parameters = parameterTypes.Length == 0
                ? Type.EmptyTypes
                : parameterTypes;

            // Try to find the method
            return DeclaringType.GetMethod(methodName, 
                BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, 
                CLRType.Binder,
                parameters,
                null);
        }
    }
}
