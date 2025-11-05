using System;
using System.Globalization;
using System.Reflection;

namespace dotnow.Reflection
{
    internal sealed class CLRGenericParameterType : Type
    {
        // Private
        private readonly int position;

        // Properties
        public override Assembly Assembly => null;
        public override string AssemblyQualifiedName => Name;
        public override Type BaseType => null;
        public override string FullName => Name;
        public override Guid GUID => Guid.Empty;
        public override Module Module => null;
        public override string Namespace => null;
        public override Type UnderlyingSystemType => typeof(CLRGenericParameterType);
        public override string Name => $"!{position}";

        public override MethodBase DeclaringMethod => null;
        public override Type DeclaringType => null;
        public override int GenericParameterPosition => position;
        public override bool IsGenericParameter => true;

        // Constructor
        public CLRGenericParameterType(int position)
        {
            this.position = position;
        }

        // Methods
        protected override TypeAttributes GetAttributeFlagsImpl() => 0;
        protected override bool HasElementTypeImpl() => false;
        protected override bool IsArrayImpl() => false;
        protected override bool IsByRefImpl() => false;
        protected override bool IsCOMObjectImpl() => false;
        protected override bool IsPointerImpl() => false;
        protected override bool IsPrimitiveImpl() => false;

        public override ConstructorInfo[] GetConstructors(BindingFlags bindingAttr) => throw new NotSupportedException();
        public override object[] GetCustomAttributes(bool inherit) => throw new NotSupportedException();
        public override object[] GetCustomAttributes(Type attributeType, bool inherit) => throw new NotSupportedException();
        public override Type GetElementType() => throw new NotSupportedException();
        public override EventInfo GetEvent(string name, BindingFlags bindingAttr) => throw new NotSupportedException();
        public override EventInfo[] GetEvents(BindingFlags bindingAttr) => throw new NotSupportedException();
        public override FieldInfo GetField(string name, BindingFlags bindingAttr) => throw new NotSupportedException();
        public override FieldInfo[] GetFields(BindingFlags bindingAttr) => throw new NotSupportedException();
        public override Type GetInterface(string name, bool ignoreCase) => throw new NotSupportedException();
        public override Type[] GetInterfaces() => throw new NotSupportedException();
        public override MemberInfo[] GetMembers(BindingFlags bindingAttr) => throw new NotSupportedException();
        public override MethodInfo[] GetMethods(BindingFlags bindingAttr) => throw new NotSupportedException();
        public override Type GetNestedType(string name, BindingFlags bindingAttr) => throw new NotSupportedException();
        public override Type[] GetNestedTypes(BindingFlags bindingAttr) => throw new NotSupportedException();
        public override PropertyInfo[] GetProperties(BindingFlags bindingAttr) => throw new NotSupportedException();
        public override object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters) => throw new NotSupportedException();
        public override bool IsDefined(Type attributeType, bool inherit) => throw new NotSupportedException();
        protected override ConstructorInfo GetConstructorImpl(BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers) => throw new NotSupportedException();
        protected override MethodInfo GetMethodImpl(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers) => throw new NotSupportedException();
        protected override PropertyInfo GetPropertyImpl(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers) => throw new NotSupportedException();
    }
}
