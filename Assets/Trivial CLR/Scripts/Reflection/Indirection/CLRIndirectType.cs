using System;
using System.Globalization;
using System.Reflection;

namespace TrivialCLR.Reflection.Indirection
{
    internal sealed class CLRIndirectType : Type
    {
        // Private
        private Type indirectType = null;
        private Type indirectTypeGenericDefintiion = null;

        // Properties
        public Type IndirectType
        {
            get { return indirectType; }
        }

        public override Assembly Assembly
        {
            get { return indirectType.Assembly; }
        }

        public override string AssemblyQualifiedName
        {
            get { return indirectType.AssemblyQualifiedName; }
        }

        public override Type BaseType
        {
            get { return indirectType.BaseType; }
        }

        public override string FullName
        {
            get { return indirectType.FullName; }
        }

        public override Guid GUID
        {
            get { return indirectType.GUID; }
        }

        public override Module Module
        {
            get { return indirectType.Module; }
        }

        public override string Namespace
        {
            get { return indirectType.Namespace; }
        }

        public override Type UnderlyingSystemType
        {
            get { return indirectType.UnderlyingSystemType; }
        }

        public override string Name
        {
            get { return indirectType.Name; }
        }

        public override bool ContainsGenericParameters
        {
            get { return indirectType.ContainsGenericParameters; }
        }

        // Constructor
        public CLRIndirectType(Type indirectType)
        {
            this.indirectType = indirectType;
            this.indirectTypeGenericDefintiion = indirectType;

            if (indirectType.IsGenericType == true)
                this.indirectTypeGenericDefintiion = indirectType.GetGenericTypeDefinition();
        }

        // Methods
        public override ConstructorInfo[] GetConstructors(BindingFlags bindingAttr)
        {
            return indirectType.GetConstructors(bindingAttr);
        }

        public override object[] GetCustomAttributes(bool inherit)
        {
            return indirectType.GetCustomAttributes(inherit);
        }

        public override object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            return indirectType.GetCustomAttributes(attributeType, inherit);
        }

        public override Type GetElementType()
        {
            return indirectType.GetElementType();
        }

        public override EventInfo GetEvent(string name, BindingFlags bindingAttr)
        {
            return indirectType.GetEvent(name, bindingAttr);
        }

        public override EventInfo[] GetEvents(BindingFlags bindingAttr)
        {
            return indirectType.GetEvents(bindingAttr);
        }

        public override FieldInfo GetField(string name, BindingFlags bindingAttr)
        {
            FieldInfo result = indirectType.GetField(name, bindingAttr);

            if (result != null)
                result = new CLRIndirectFieldInvocation(result);

            return result;
        }

        public override FieldInfo[] GetFields(BindingFlags bindingAttr)
        {
            FieldInfo[] results = indirectType.GetFields(bindingAttr);

            for (int i = 0; i < results.Length; i++)
                results[i] = new CLRIndirectFieldInvocation(results[i]);

            return results;
        }

        public override Type GetInterface(string name, bool ignoreCase)
        {
            return indirectType.GetInterface(name, ignoreCase);
        }

        public override Type[] GetInterfaces()
        {
            return indirectType.GetInterfaces();
        }

        public override MemberInfo[] GetMembers(BindingFlags bindingAttr)
        {
            return indirectType.GetMembers(bindingAttr);
        }

        public override MethodInfo[] GetMethods(BindingFlags bindingAttr)
        {
            return indirectType.GetMethods(bindingAttr);
        }

        public override Type GetNestedType(string name, BindingFlags bindingAttr)
        {
            return indirectType.GetNestedType(name, bindingAttr);
        }

        public override Type[] GetNestedTypes(BindingFlags bindingAttr)
        {
            return indirectType.GetNestedTypes(bindingAttr);
        }

        public override PropertyInfo[] GetProperties(BindingFlags bindingAttr)
        {
            return indirectType.GetProperties(bindingAttr);
        }

        public override object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters)
        {
            return indirectType.InvokeMember(name, invokeAttr, binder, target, args, modifiers, culture, namedParameters);
        }

        public override bool IsDefined(Type attributeType, bool inherit)
        {
            return indirectType.IsDefined(attributeType, inherit);
        }

        protected override TypeAttributes GetAttributeFlagsImpl()
        {
            return indirectType.Attributes;
        }

        protected override ConstructorInfo GetConstructorImpl(BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
        {
            return indirectTypeGenericDefintiion.GetConstructor(bindingAttr, binder, callConvention, types, modifiers);
        }

        protected override MethodInfo GetMethodImpl(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
        {
            return indirectTypeGenericDefintiion.GetMethod(name, bindingAttr, binder, callConvention, types, modifiers);
        }

        protected override PropertyInfo GetPropertyImpl(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers)
        {
            return indirectTypeGenericDefintiion.GetProperty(name, bindingAttr, binder, returnType, types, modifiers);
        }

        protected override bool HasElementTypeImpl()
        {
            return indirectType.HasElementType;
        }

        protected override bool IsArrayImpl()
        {
            return indirectType.IsArray;
        }

        protected override bool IsByRefImpl()
        {
            return indirectType.IsByRef;
        }

        protected override bool IsCOMObjectImpl()
        {
            return indirectType.IsCOMObject;
        }

        protected override bool IsPointerImpl()
        {
            return indirectType.IsPointer;
        }

        protected override bool IsPrimitiveImpl()
        {
            return indirectType.IsPrimitive;
        }

        public override Type[] GetGenericArguments()
        {
            return indirectType.GetGenericArguments();
        }

        public override Type MakeGenericType(params Type[] typeArguments)
        {
            return indirectType.MakeGenericType(typeArguments);
        }
    }
}
