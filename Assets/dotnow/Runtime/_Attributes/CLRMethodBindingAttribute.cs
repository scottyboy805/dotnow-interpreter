using System;

namespace dotnow
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class CLRMethodBindingAttribute : Attribute
    {
        // Private
        private Type declaringType;
        private string methodName;
        private Type[] parameterTypes;

        // Properties
        public Type DeclaringType
        {
            get { return declaringType; }
        }

        public string MethodName
        {
            get { return methodName; }
        }

        public Type[] ParameterTypes
        {
            get { return parameterTypes; }
        }

        // Constructor
        public CLRMethodBindingAttribute(Type declaringType, string methodName, params Type[] parameterTypes)
        {
            this.declaringType = declaringType;
            this.methodName = methodName;
            this.parameterTypes = Bindings.GetBindingParameters(parameterTypes);

            if (parameterTypes == null || parameterTypes.Length == 0)
                this.parameterTypes = Type.EmptyTypes;
        }
    }
}
