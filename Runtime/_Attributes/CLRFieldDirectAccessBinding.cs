using System;

namespace dotnow
{
    public enum CLRFieldAccessMode
    {
        Read,
        Write,
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class CLRFieldDirectAccessBindingAttribute : Attribute
    {
        // Private
        private Type declaringType;
        private string fieldName;
        private CLRFieldAccessMode fieldAccessMode;

        // Properties
        public Type DeclaringType
        {
            get { return declaringType; }
        }

        public string FieldName
        {
            get { return fieldName; }
        }

        public CLRFieldAccessMode FieldAccessMode
        {
            get { return fieldAccessMode; }
        }

        // Constructor
        public CLRFieldDirectAccessBindingAttribute(Type declaringType, string fieldName, CLRFieldAccessMode fieldAccessMode)
        {
            this.declaringType = declaringType;
            this.fieldName = fieldName;
            this.fieldAccessMode = fieldAccessMode;
        }
    }
}
