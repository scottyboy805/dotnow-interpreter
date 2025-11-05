using dotnow.Interop.Runtime.Internal;
using dotnow.Reflection.Internal;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace dotnow.Reflection
{
    internal sealed class CLRArrayType : Type
    {
        // Private
        private readonly Type elementType;
        private readonly int rank;
        private readonly string arrayRankString;

        private readonly ConstructorInfo[] specialConstructors;
        private readonly MethodInfo[] specialMethods;

        // Properties
        public override Assembly Assembly => elementType.Assembly;
        public override string AssemblyQualifiedName => elementType.AssemblyQualifiedName;
        public override Type BaseType => typeof(Array);
        public override string FullName => elementType.FullName + arrayRankString;
        public override Guid GUID => Guid.Empty;
        public override Module Module => elementType.Module;
        public override string Namespace => elementType.Namespace;
        public override Type UnderlyingSystemType => typeof(CLRArrayType);
        public override string Name => elementType.Name + arrayRankString;

        // Constructor
        public CLRArrayType(Type elementType, int rank)
        {
            // Check for null
            if (elementType == null)
                throw new ArgumentNullException(nameof(elementType));
            
            this.elementType = elementType;
            this.rank = rank;

            this.arrayRankString = $"[{new string(',', rank - 1)}]";

            // Check for multi-array
            if (rank > 1)
            {
                // Inject special initializers with int and long lengths
                specialConstructors = new ConstructorInfo[]
                {
                    new CLRInternalConstructorInfo(".ctor", this, Enumerable.Repeat(typeof(int), rank).ToArray(), MethodAttributes.Public, System_Array_Internal.MultiArrayCreateInstance_Internal),
                    new CLRInternalConstructorInfo(".ctor", this, Enumerable.Repeat(typeof(long), rank).ToArray(), MethodAttributes.Public, System_Array_Internal.MultiArrayCreateInstance_Internal),
                };

                // Inject special methods with int and long indexes
                specialMethods = new MethodInfo[]
                {
                    new CLRInternalMethodInfo("Get", this, elementType, Enumerable.Repeat(typeof(int), rank).ToArray(), MethodAttributes.Public, System_Array_Internal.MultiArrayGet_Internal),
                    new CLRInternalMethodInfo("Get", this, elementType, Enumerable.Repeat(typeof(long), rank).ToArray(), MethodAttributes.Public, System_Array_Internal.MultiArrayGet_Internal),

                    new CLRInternalMethodInfo("Set", this, typeof(void), Enumerable.Repeat(typeof(int), rank).Append(elementType).ToArray(), MethodAttributes.Public, System_Array_Internal.MultiArraySet_Internal),
                    new CLRInternalMethodInfo("Set", this, typeof(void), Enumerable.Repeat(typeof(long), rank).Append(elementType).ToArray(), MethodAttributes.Public, System_Array_Internal.MultiArraySet_Internal),
                };
            }
        }

        // Methods
        public override Type GetElementType() => elementType;
        public override int GetArrayRank() => rank;
        public override bool IsSZArray => rank == 1;
        protected override bool HasElementTypeImpl() => true;
        protected override bool IsArrayImpl() => true;
        protected override bool IsByRefImpl() => elementType.IsByRef;
        protected override bool IsCOMObjectImpl() => false;
        protected override bool IsPointerImpl() => elementType.IsPointer;
        protected override bool IsPrimitiveImpl() => false;
        protected override TypeAttributes GetAttributeFlagsImpl() => elementType.Attributes;


        public override ConstructorInfo[] GetConstructors(BindingFlags bindingAttr) => typeof(Array).GetConstructors(bindingAttr);
        public override object[] GetCustomAttributes(bool inherit) => typeof(Array).GetCustomAttributes(inherit);
        public override object[] GetCustomAttributes(Type attributeType, bool inherit) => typeof(Array).GetCustomAttributes(attributeType, inherit);
        public override EventInfo GetEvent(string name, BindingFlags bindingAttr) => typeof(Array).GetEvent(name, bindingAttr);
        public override EventInfo[] GetEvents(BindingFlags bindingAttr) => typeof(Array).GetEvents(bindingAttr);
        public override FieldInfo GetField(string name, BindingFlags bindingAttr) => typeof(Array).GetField(name, bindingAttr);
        public override FieldInfo[] GetFields(BindingFlags bindingAttr) => typeof(Array).GetFields(bindingAttr);
        public override Type GetInterface(string name, bool ignoreCase) => typeof(Array).GetInterface(name, ignoreCase);
        public override Type[] GetInterfaces() => typeof(Array).GetInterfaces();
        public override MemberInfo[] GetMembers(BindingFlags bindingAttr) => typeof(Array).GetMembers(bindingAttr);
        public override MethodInfo[] GetMethods(BindingFlags bindingAttr) => typeof(Array).GetMethods(bindingAttr);
        public override Type GetNestedType(string name, BindingFlags bindingAttr) => typeof(Array).GetNestedType(name, bindingAttr);
        public override Type[] GetNestedTypes(BindingFlags bindingAttr) => typeof(Array).GetNestedTypes(bindingAttr);
        public override PropertyInfo[] GetProperties(BindingFlags bindingAttr) => typeof(Array).GetProperties(bindingAttr);
        public override object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters)
            => typeof(Array).InvokeMember(name, invokeAttr, binder, target, args, modifiers, culture, namedParameters);
        public override bool IsDefined(Type attributeType, bool inherit) => typeof(Array).IsDefined(attributeType, inherit);

        protected override ConstructorInfo GetConstructorImpl(BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
        { 
            // Try to find in array
            ConstructorInfo ctor = typeof(Array).GetConstructor(bindingAttr, binder, callConvention, types, modifiers);

            // Check for special initializers
            if(ctor == null && specialConstructors != null)
            {
                // Check for binder
                if (binder == null)
                    binder = Type.DefaultBinder;

                // Try to find special constructor match
                ctor = binder.SelectMethod(bindingAttr, specialConstructors, types, modifiers) as ConstructorInfo;
            }
            return ctor;
        }
        protected override MethodInfo GetMethodImpl(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
        { 
            // Try to find in array
            MethodInfo method = typeof(Array).GetMethod(name, bindingAttr, binder, callConvention, types, modifiers);

            // Check for special methods
            if(method == null && specialMethods != null)
            {
                // Check for binder
                if (binder == null)
                    binder = Type.DefaultBinder;

                // Try to find special method match
                method = binder.SelectMethod(bindingAttr, specialMethods, types, modifiers) as MethodInfo;
            }
            return method;
        }
        protected override PropertyInfo GetPropertyImpl(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers)
                => typeof(Array).GetProperty(name, bindingAttr, binder, returnType, types, modifiers);
    }
}
