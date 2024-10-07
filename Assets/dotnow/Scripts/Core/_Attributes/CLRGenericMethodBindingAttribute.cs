using System;

namespace dotnow
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class CLRGenericMethodBindingAttribute : CLRMethodBindingAttribute
    {
        // Private
        private int genericArgumentCount = 1;

        // Properties
        public int GenericArgumentCount
        {
            get { return genericArgumentCount; }
        }

        // Constructor
        public CLRGenericMethodBindingAttribute(Type declaringType, string methodName, int genericArgumentCount, params Type[] parameterTypes)
            : base(declaringType, methodName, parameterTypes)
        {
            // Check for none
            if (genericArgumentCount < 1)
                throw new ArgumentException("Generic argument count must be 1 or more");

            this.genericArgumentCount = genericArgumentCount;
        }
    }
}
