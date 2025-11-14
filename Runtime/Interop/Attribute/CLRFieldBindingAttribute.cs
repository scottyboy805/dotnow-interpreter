using System;
using System.Reflection;

namespace dotnow.Interop
{
    public enum CLRFieldAccess
    {
        Read,
        Write,
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class CLRFieldBindingAttribute : Attribute
    {
        // Private
        private readonly Type declaringType;
        private readonly string fieldName;
        private readonly CLRFieldAccess fieldAccess;

        // Properties
        public Type DeclaringType => declaringType;
        public string FieldName => fieldName;
        public CLRFieldAccess FieldAccess => fieldAccess;

        // Constructor
        public CLRFieldBindingAttribute(Type declaringType, string fieldName, CLRFieldAccess fieldAccess)
        {
            // Check for null
            if (declaringType == null)
                throw new ArgumentNullException(nameof(declaringType));

            // Check for name
            if (string.IsNullOrEmpty(fieldName) == true)
                throw new ArgumentException("Field name cannot be null or empty");

            this.declaringType = declaringType;
            this.fieldName = fieldName;
            this.fieldAccess = fieldAccess;
        }

        // Methods
        public FieldInfo ResolveFieldBinding()
        {
            // Try to find the method
            return DeclaringType.GetField(fieldName,
                BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
        }
    }
}
