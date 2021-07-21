using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using Mono.Cecil;
using PropertyAttributes = System.Reflection.PropertyAttributes;

namespace TrivialCLR.Reflection
{
    public sealed class CLRProperty : PropertyInfo
    {
        // Private
        private AppDomain domain = null;
        private CLRType declaringType = null;
        private PropertyDefinition property = null;
        private Lazy<CLRMethod> getMethod = null;
        private Lazy<CLRMethod> setMethod = null;
        private Lazy<Type> propertyType = null;
        private Lazy<CLRAttributeBuilder> attributeProvider = null;

        // Properties
        public PropertyDefinition Definition
        {
            get { return property; }
        }

        public override PropertyAttributes Attributes
        {
            get { return (PropertyAttributes)property.Attributes; }
        }

        public override bool CanRead
        {
            get { return property.GetMethod != null; }
        }

        public override bool CanWrite
        {
            get { return property.SetMethod != null; }
        }

        public override Type PropertyType
        {
            get { return propertyType.Value; }
        }

        public override Type DeclaringType
        {
            get { return declaringType; }
        }

        public override string Name
        {
            get { return property.Name; }
        }

        public override Type ReflectedType
        {
            get { return declaringType; }
        }

        // Constructor
        internal CLRProperty(AppDomain domain, CLRType declaringType, PropertyDefinition property)
        {
            this.domain = domain;
            this.declaringType = declaringType;
            this.property = property;

            // Lazy types
            this.propertyType = new Lazy<Type>(() => domain.ResolveType(property.PropertyType));
            this.getMethod = new Lazy<CLRMethod>(() => domain.ResolveMethod(property.GetMethod) as CLRMethod);
            this.setMethod = new Lazy<CLRMethod>(() => domain.ResolveMethod(property.SetMethod) as CLRMethod);

            this.attributeProvider = new Lazy<CLRAttributeBuilder>(() => new CLRAttributeBuilder(domain, property.CustomAttributes));
        }

        // Methods
        public override MethodInfo[] GetAccessors(bool nonPublic)
        {
            // Try to get accessors
            MethodInfo getter = GetGetMethod(nonPublic);
            MethodInfo setter = GetSetMethod(nonPublic);

            // Check for both accessors
            if (getter != null && setter != null)
                return new MethodInfo[] { getter, setter };

            // Check for getter only
            if (getter != null)
                return new MethodInfo[] { getter };

            // Check for setter only
            if (setter != null)
                return new MethodInfo[] { setter };

            // Empry array
            return new MethodInfo[0];
        }

        public override object[] GetCustomAttributes(bool inherit)
        {
            return attributeProvider.Value.GetAttributeInstances();
        }

        public override object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            return attributeProvider.Value.GetAttributeInstancesOfType(attributeType);
        }

        public override MethodInfo GetGetMethod(bool nonPublic)
        {
            MethodInfo getter = getMethod.Value;

            // Check for null
            if(getter != null)
            {
                if(nonPublic == false)
                {
                    if (getter.IsPublic == false)
                        getter = null;
                }
            }

            // No getter
            return getter;
        }

        public override ParameterInfo[] GetIndexParameters()
        {
            throw new NotImplementedException();
        }

        public override MethodInfo GetSetMethod(bool nonPublic)
        {
            MethodInfo setter = setMethod.Value;

            // Check for null
            if(setter != null)
            {
                if(nonPublic == false)
                {
                    if (setter.IsPublic == false)
                        setter = null;
                }
            }
            // No setter
            return setter;
        }

        public override object GetValue(object obj, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture)
        {
            // Make sure type is initialized
            declaringType.StaticInitializeType();

            // Check get method
            MethodInfo method = GetGetMethod((invokeAttr & BindingFlags.NonPublic) != 0);

            if (method != null)
            {
                return method.Invoke(obj, null);
            }
            else
                throw new MissingMethodException(string.Format("The property '{0}' does not define a set accessor", this));
        }

        public override bool IsDefined(Type attributeType, bool inherit)
        {
            return attributeProvider.Value.IsDefined(attributeType);
        }

        public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture)
        {
            // Make sure type is initialized
            declaringType.StaticInitializeType();

            // Check get method
            MethodInfo method = GetSetMethod((invokeAttr & BindingFlags.NonPublic) != 0);

            if (method != null)
            {
                method.Invoke(obj, new object[] { value });
            }
            else
                throw new MissingMethodException(string.Format("The property '{0}' does not define a set accessor", this));
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", propertyType, Name);
        }
    }
}
