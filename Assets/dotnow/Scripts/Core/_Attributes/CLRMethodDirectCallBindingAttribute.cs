using System;

namespace dotnow
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class CLRMethodDirectCallBindingAttribute : Attribute
    {
        // Private
        private Type declaringType;
        private string methodName;
        private Type[] parameterTypes;
        private bool isGenericMethod;
        private Type[] genericTypes;
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

        public bool IsGeneric
        {
            get { return isGenericMethod; }
        }
        public Type[] GenericTypes
        {
            get { return genericTypes; }
        }
        // Constructor
        public CLRMethodDirectCallBindingAttribute(Type declaringType, string methodName, bool isGenericMethod = false, Type[] genericTypes = null, Type[] parameterTypes = null)
        {
            this.declaringType = declaringType;
            this.methodName = methodName;
            this.parameterTypes = parameterTypes;
            this.isGenericMethod = isGenericMethod;
            this.genericTypes = genericTypes;
            if (parameterTypes == null || parameterTypes.Length == 0)
                this.parameterTypes = Type.EmptyTypes;
        }
        
        public CLRMethodDirectCallBindingAttribute(Type declaringType, string methodName, params Type[] parameterTypes)
        {
            this.declaringType = declaringType;
            this.methodName = methodName;
            this.parameterTypes = parameterTypes;
            this.isGenericMethod = false;
            this.genericTypes = null;
            if (parameterTypes == null || parameterTypes.Length == 0)
                this.parameterTypes = Type.EmptyTypes;
        }
    }
}
