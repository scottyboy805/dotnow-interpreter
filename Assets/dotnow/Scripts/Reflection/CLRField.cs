using System;
using System.Globalization;
using System.Reflection;
using Mono.Cecil;
using dotnow.Runtime;
using FieldAttributes = System.Reflection.FieldAttributes;

namespace dotnow.Reflection
{
    public sealed class CLRField : FieldInfo
    {
        // Private
        private AppDomain domain = null;
        private CLRType declaringType = null;
        private FieldDefinition field = null;
        private Lazy<Type> fieldType = null;
        private Lazy<CLRAttributeBuilder> attributeProvider = null;
        private object staticValue = null;

        private CLRTypeInfo fieldTypeInfo = null;

        // Properties
        public FieldDefinition Definition
        {
            get { return field; }
        }

        public override FieldAttributes Attributes
        {
            get { return (FieldAttributes)field.Attributes; }
        }

        public override Type DeclaringType
        {
            get { return declaringType; }
        }

        public override RuntimeFieldHandle FieldHandle
        {
            get { throw new NotSupportedException("A RuntimeField has no obtainable field handle"); }
        }

        public override Type FieldType
        {
            get { return fieldType.Value; }
        }

        public override string Name
        {
            get { return field.Name; }
        }

        public override Type ReflectedType
        {
            get { return declaringType; }
        }

        public byte[] InitialData
        {
            get { return field.InitialValue; }
        }

        public bool ContainsGenericParameter
        {
            get { return field.ContainsGenericParameter; }
        }

        public bool HasGenericType
        {
            get { return field.FieldType.IsGenericParameter; }
        }

        internal CLRTypeInfo FieldTypeInfo
        {
            get
            {
                if (fieldTypeInfo == null)
                    fieldTypeInfo = CLRTypeInfo.GetTypeInfo(FieldType);

                return fieldTypeInfo;
            }
        }

        // Constructor
        internal CLRField(AppDomain domain, CLRType declaringType, FieldDefinition field)
        {
            this.domain = domain;
            this.declaringType = declaringType;
            this.field = field;

            // Force read the initial value data otherwise we will get stream disposed exception when called out of loading context
            byte[] unused = field.InitialValue;

            // Intiialize field type
            this.fieldType = new Lazy<Type>(InitFieldType);

            this.attributeProvider = new Lazy<CLRAttributeBuilder>(() => new CLRAttributeBuilder(domain, field.CustomAttributes));
        }

        // Methods
        public override object[] GetCustomAttributes(bool inherit)
        {
            return attributeProvider.Value.GetAttributeInstances();
        }

        public override object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            return attributeProvider.Value.GetAttributeInstancesOfType(attributeType);
        }

        public override bool IsDefined(Type attributeType, bool inherit)
        {
            return attributeProvider.Value.IsDefined(attributeType);
        }

        public override object GetValue(object obj)
        {
            // Make sure type is initialized
            declaringType.StaticInitializeType();

            // Check for static
            if (IsStatic == false)
            {
                // Check for instance
                if (obj.IsCLRInstance() == false)
                    throw new InvalidOperationException("Cannot access field value for non CLR instance");

                // Get value from the instance
                return (obj as CLRInstance).GetFieldValue(__heapallocator.GetCurrent(), this);
            }

            // Get static value
            return staticValue;
        }

        internal void GetValueStack(in StackData obj, ref StackData value)
        {
            // Make sure type is initialized
            declaringType.StaticInitializeType();

            // Check for static
            if (IsStatic == false)
            {
                // Check for instance
                if (obj.Box().IsCLRInstance() == false)
                    throw new InvalidOperationException("Cannot access field value for non CLR instance");

                // Get value from the instance
                (obj.Box() as CLRInstance).GetFieldValueStack(this, ref value);
                return;
            }

            // Get static value
            StackData.AllocTyped(__heapallocator.GetCurrent(), ref value, fieldTypeInfo, staticValue);
        }

        public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, CultureInfo culture)
        {
            // Make sure type is initialized
            declaringType.StaticInitializeType();

            // Check for static
            if (IsStatic == false)
            {
                // Check for null
                if (obj == null)
                    throw new NullReferenceException();

                // Check for instance
                if (obj.IsCLRInstance() == false)
                    throw new InvalidOperationException("Cannot assign field value for non CLR instance");

                // Get value from the instance
                (obj as CLRInstance).SetFieldValue(__heapallocator.GetCurrent(), this, value);
            }
            else
            {
                staticValue = value;
            }
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", fieldType, Name);
        }

        internal void StaticInitialize()
        {
            if (IsStatic == true)
            {
                this.staticValue = fieldType.Value.GetDefaultValue(domain);
            }
        }

        internal int GetFieldOffset()
        {
            return declaringType.GetFieldOffset(this);
        }

        private Type InitFieldType()
        {
            // Check for generic
            if(field.ContainsGenericParameter == true)
            {
                // Get the geneirc parameter
                GenericParameter parameter = field.FieldType as GenericParameter;

                if(parameter != null)
                {
                    return declaringType.GenericTypeArguments[parameter.Position];
                }
            }

            // Default field handling
            return domain.ResolveType(field.FieldType);
        }
    }
}
