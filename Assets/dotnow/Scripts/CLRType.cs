using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using Mono.Cecil;
using dotnow.Interop;
using dotnow.Reflection;
using dotnow.Runtime.JIT;
using TypeAttributes = System.Reflection.TypeAttributes;

namespace dotnow
{
    public sealed class CLRType : Type, IJITOptimizable
    {
        // Types
        [Flags]
        private enum StorageAccessorMode : byte
        {
            Any = 0,
            Set = 1,
            Get = 2,            
        }

        // Private
        private static List<MemberInfo> memberArrayBuilder = new List<MemberInfo>();

        private List<CLRField> instanceFields = null;

        private AppDomain domain = null;
        private CLRModule module = null;
        private TypeDefinition type = null;
        private Type genericDefinition = null;
        private Type elementType = null;
        private int arrayRank = 0;
        private Lazy<Type> baseType = null;
        private Lazy<Type[]> interfaceTypes = null;
        private Type[] genericTypes = null;
        private CLRType[] nestedTypes = null;
        private CLRField[] fields = null;
        private CLRProperty[] properties = null;
        private CLRConstructor[] constructors = null;
        private CLRMethod[] methods = null;
        private Guid guid = Guid.Empty;
        private Lazy<CLRAttributeBuilder> attributeProvider = null;

        private MethodBase staticTypeInitializer = null;
        private bool isStaticInitializing = false;
        private bool isStaticInitialized = false;

        // Internal
        internal MethodInfo cachedToStringTarget = null;
        internal MethodInfo cachedEqualsTarget = null;
        internal MethodInfo cachedGetHashCodeTarget = null;

        // Properties
        public TypeDefinition Definition
        {
            get { return type; }
        }        

        public CLRModule RuntimeModule
        {
            get { return module; }
        }

        public override Assembly Assembly
        {
            get { throw new NotSupportedException("A CLRType is not defined in the context of a runtime assembly"); }
        }

        public override string AssemblyQualifiedName
        {
            get { return type.FullName; }
        }

        public override Type BaseType
        {
            get { return baseType.Value; }
        }

        public override string FullName
        {
            get { return type.FullName; }
        }

        public override Guid GUID
        {
            get { return guid; }
        }

        public override Module Module
        {
            get { throw new NotSupportedException("A CLRType is not defined in the context of a runtime module"); }
        }

        public override string Namespace
        {
            get { return type.Namespace; }
        }

        public override Type UnderlyingSystemType
        {
            get { return typeof(CLRType); }// throw new NotSupportedException("Cannot get underlying system type of a CLRType"); }
        }

        public override Type[] GenericTypeArguments
        {
            get
            {
                if (genericTypes == null)
                    return new Type[0];

                return genericTypes;
            }
        }

        public override bool IsGenericType
        {
            get { return genericTypes != null && genericTypes.Length > 0; }
        }

        public override string Name
        {
            get { return type.Name; }
        }

        internal IEnumerable<MemberInfo> AllMembers
        {
            get
            {
                foreach (MemberInfo member in nestedTypes)
                    yield return member;

                foreach (MemberInfo member in fields)
                    yield return member;

                foreach (MemberInfo member in properties)
                    yield return member;

                foreach (MemberInfo member in constructors)
                    yield return member;

                foreach (MemberInfo member in methods)
                    yield return member;
            }
        }

        // Constructor
        internal CLRType(AppDomain domain, CLRModule module, TypeDefinition typeDef)
        {            
            this.domain = domain;
            this.module = module;
            this.type = typeDef;

            // Base type
            this.baseType = new Lazy<Type>(() =>
            {
                // Interfaces cannot have base types so we much check for null
                if (type.BaseType != null)
                    return domain.ResolveType(type.BaseType);

                return null;
            });

            // Interface type
            this.interfaceTypes = new Lazy<Type[]>(() =>
            {
                Type[] iTypes = new Type[typeDef.Interfaces.Count];

                for (int i = 0; i < iTypes.Length; i++)
                    iTypes[i] = domain.ResolveType(typeDef.Interfaces[i].InterfaceType);

                return iTypes;
            });
            
            this.guid = Guid.NewGuid();

            // Nested types
            this.nestedTypes = new CLRType[typeDef.NestedTypes.Count];

            for (int i = 0; i < nestedTypes.Length; i++)
                nestedTypes[i] = new CLRType(domain, module, typeDef.NestedTypes[i]);


            // Field members
            this.fields = new CLRField[typeDef.Fields.Count];

            for (int i = 0; i < fields.Length; i++)
                fields[i] = new CLRField(domain, this, typeDef.Fields[i]);


            // Property members
            this.properties = new CLRProperty[typeDef.Properties.Count];

            for (int i = 0; i < properties.Length; i++)
                properties[i] = new CLRProperty(domain, this, typeDef.Properties[i]);


            int ctorCount = 0;
            int methodCount = 0;

            foreach(MethodDefinition method in typeDef.Methods)
            {
                if (method.IsConstructor == true)
                    ctorCount++;
                else
                    methodCount++;
            }

            // Constructor members
            this.constructors = new CLRConstructor[ctorCount];

            for (int index = 0, i = 0; i < typeDef.Methods.Count; i++)
            {
                if (typeDef.Methods[i].IsConstructor == true)
                {
                    constructors[index] = new CLRConstructor(domain, this, typeDef.Methods[i]);
                    index++;
                }
            }


            // Method members
            this.methods = new CLRMethod[methodCount];

            for (int index = 0, i = 0; i < typeDef.Methods.Count; i++)
            {
                if (typeDef.Methods[i].IsConstructor == false)
                {
                    methods[index] = new CLRMethod(domain, this, typeDef.Methods[i]);
                    index++;
                }
            }

            this.attributeProvider = new Lazy<CLRAttributeBuilder>(() => new CLRAttributeBuilder(domain, type.CustomAttributes));


            // Cached methods
            cachedToStringTarget = GetMethod(nameof(ToString), BindingFlags.Public | BindingFlags.Instance, null, Type.EmptyTypes, null);
            cachedEqualsTarget = GetMethod(nameof(Equals), BindingFlags.Public | BindingFlags.Instance, null, new Type[] { typeof(object) }, null);
            cachedGetHashCodeTarget = GetMethod(nameof(GetHashCode), BindingFlags.Public | BindingFlags.Instance, null, Type.EmptyTypes, null);
        }

        // Methods
        void IJITOptimizable.EnsureJITOptimized()
        {
            // Ensure all constructors are optimzed and ready to be invoked
            foreach (CLRConstructor ctor in constructors)
                JITOptimize.EnsureJITOptimized(ctor);

            // Ensure all methods are optimized and ready to be invoked
            foreach (CLRMethod method in methods)
                JITOptimize.EnsureJITOptimized(method);
        }

        public override string ToString()
        {
            return type.ToString();
        }

        public override bool Equals(object o)
        {
            if (ReferenceEquals(o, null) == true || (o is CLRType) == false)
                return false;

            // Check for equal type declarations - this will probably need more work to support generics
            return ((CLRType)o).type == this.type;
        }

        public override int GetHashCode()
        {
            return type.GetHashCode();
        }

        public List<CLRField> GetInstanceFields()
        {
            if(instanceFields == null)
            {
                instanceFields = new List<CLRField>();

                Type current = this;

                while (current != null && current.IsCLRType() == true)
                {
                    // Initialize field offsets
                    foreach (CLRField fieldSlot in (current as CLRType).fields)
                    {
                        if (fieldSlot.IsStatic == false)
                        {
                            // Add the field slot
                            instanceFields.Add(fieldSlot);
                        }
                    }

                    // Move down the hierarchy
                    current = current.BaseType;
                }                
            }

            return instanceFields;
        }

        /// <summary>
        /// Returns the size of an instance of this type in StackData units.
        /// This method will not return the size of the instance in bytes!
        /// </summary>
        /// <returns></returns>
        public int SizeOfInstance()
        {
            // Size of in StackData units, not bytes
            return GetInstanceFields().Count;
        }

        internal int GetFieldOffset(CLRField field)
        {
            // Get instance fields
            List<CLRField> instanceFieldData = GetInstanceFields();

            // Get field index
            return instanceFieldData.IndexOf(field);
        }

        public override bool IsAssignableFrom(Type c)
        {
            return TypeExtensions.AreAssignable(this, c);
            //return base.IsAssignableFrom(c);
        }

        public override bool IsSubclassOf(Type c)
        {
            if (c == this)
                return true;

            Type current = this;

            while(current != null && current != typeof(object))
            {
                if (current == c)
                    return true;

                current = current.BaseType;
            }

            return false;
        }

        public override bool IsEquivalentTo(Type other)
        {
            return base.IsEquivalentTo(other);
        }

        public override ConstructorInfo[] GetConstructors(BindingFlags bindingAttr)
        {
            // Process each constructor
            foreach(ConstructorInfo ctor in constructors)
            {
                // Check for non public
                if ((ctor.IsPublic == true && (bindingAttr & BindingFlags.Public) == 0) ||
                    (ctor.IsPublic == false && (bindingAttr & BindingFlags.NonPublic) == 0) ||
                    (ctor.IsStatic == false && (bindingAttr & BindingFlags.Instance) == 0) ||
                    (ctor.IsStatic == true && (bindingAttr & BindingFlags.Static) == 0))
                {
                    // Skip the constructo
                    continue;
                }

                // Register the constructor
                memberArrayBuilder.Add(ctor);
            }

            // Build the result array
            return BuildMemberArray<ConstructorInfo>();
        }

        public override object[] GetCustomAttributes(bool inherit)
        {
            // Check for inherit
            if (inherit == true)
            {
                List<object> attributes = new List<object>();

                // Add attribute for this method
                attributes.AddRange(attributeProvider.Value.GetAttributeInstances());

                // Get attributes for base type
                if (baseType.Value != null)
                    attributes.AddRange(baseType.Value.GetCustomAttributes(inherit));

                return attributes.ToArray();
            }

            // Simple case
            return attributeProvider.Value.GetAttributeInstances();
        }

        public override object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            // Check for inherit
            if (inherit == true)
            {
                List<object> attributes = new List<object>();

                // Add attribute for this method
                attributes.AddRange(attributeProvider.Value.GetAttributeInstances());

                // Get attributes for base type
                if (baseType.Value != null)
                    attributes.AddRange(baseType.Value.GetCustomAttributes(attributeType, inherit));

                return attributes.ToArray();
            }

            // Simple case
            return attributeProvider.Value.GetAttributeInstancesOfType(attributeType);
        }

        public override Type GetElementType()
        {
            return elementType;
            throw new NotImplementedException();
        }

        public override EventInfo GetEvent(string name, BindingFlags bindingAttr)
        {
            throw new NotImplementedException();
        }

        public override EventInfo[] GetEvents(BindingFlags bindingAttr)
        {
            throw new NotImplementedException();
        }

        public override FieldInfo GetField(string name, BindingFlags bindingAttr)
        {
            // Check all fields
            foreach(FieldInfo field in fields)
            {
                // Check for matching field
                if (MatchFieldNameAndAttributes(field, bindingAttr, name) == true)
                    return field;
            }

            // Check base type
            if (baseType != null && (bindingAttr & BindingFlags.FlattenHierarchy) != 0)
                return baseType.Value.GetField(name, bindingAttr);

            // Field not found
            return null;
        }

        public override FieldInfo[] GetFields(BindingFlags bindingAttr)
        {
            foreach(FieldInfo field in fields)
            {
                // Check for matching field
                if (MatchFieldNameAndAttributes(field, bindingAttr, null) == true)
                    memberArrayBuilder.Add(field);
            }

            // Check base type
            if(baseType != null && (bindingAttr & BindingFlags.FlattenHierarchy) != 0)
            {
                // Get all matching child fields
                foreach (FieldInfo field in baseType.Value.GetFields(bindingAttr))
                    memberArrayBuilder.Add(field);
            }

            // Build the return array
            return BuildMemberArray<FieldInfo>();
        }

        public override Type GetInterface(string name, bool ignoreCase)
        {
            // Check all interfaces
            foreach (Type interfaceType in interfaceTypes.Value)
            {
                // Check for matching interface
                if (MatchMemberName(interfaceType, BindingFlags.Default, name) == true)
                    return interfaceType;
            }

            // Interface not found
            return null;
        }

        public override Type[] GetInterfaces()
        {
            // Create a clone array
            return interfaceTypes.Value;
        }

        public override MemberInfo[] GetMembers(BindingFlags bindingAttr)
        {
            // Add nested types
            foreach(Type type in nestedTypes)
            {
                // Check for matching types
                if (MatchTypeNameAndAttributes(type, bindingAttr, null) == true)
                    memberArrayBuilder.Add(type);
            }

            // Add fields
            foreach(FieldInfo field in fields)
            {
                // Check for matching fields
                if (MatchFieldNameAndAttributes(field, bindingAttr, null) == true)
                    memberArrayBuilder.Add(field);
            }

            // Add properties
            foreach(PropertyInfo property in properties)
            {
                // Check for matching properties
                if (MatchPropertyNameAndAttributes(property, bindingAttr, null) == true)
                    memberArrayBuilder.Add(property);
            }

            // Add constructors
            foreach(ConstructorInfo constructor in constructors)
            {
                // Check for matching constructor
                if (MatchMethodNameAndAttributes(constructor, bindingAttr, null) == true)
                    memberArrayBuilder.Add(constructor);
            }

            // Add methods
            foreach (MethodInfo method in methods)
            {
                // Check for matching methods
                if (MatchMethodNameAndAttributes(method, bindingAttr, null) == true)
                    memberArrayBuilder.Add(method);
            }

            // Build the result array
            return BuildMemberArray<MemberInfo>();
        }

        public override MethodInfo[] GetMethods(BindingFlags bindingAttr)
        {
            foreach (MethodInfo method in methods)
            {
                // Check for matching methods
                if (MatchMethodNameAndAttributes(method, bindingAttr, null) == true)
                    memberArrayBuilder.Add(method);
            }

            // Check base type
            if (baseType != null && (bindingAttr & BindingFlags.FlattenHierarchy) != 0)
            {
                // Get all matching child methods
                foreach (MethodInfo method in baseType.Value.GetMethods(bindingAttr))
                    memberArrayBuilder.Add(method);
            }

            // Build the return array
            return BuildMemberArray<MethodInfo>();
        }

        public override Type GetNestedType(string name, BindingFlags bindingAttr)
        {
            foreach (Type type in nestedTypes)
            {
                // Check for matching nested types
                if (MatchTypeNameAndAttributes(type, bindingAttr, name) == true)
                    return type;
            }

            // No type found
            return null;
        }

        public override Type[] GetNestedTypes(BindingFlags bindingAttr)
        {
            foreach (Type type in nestedTypes)
            {
                // Check for matching nested types
                if (MatchTypeNameAndAttributes(type, bindingAttr, null) == true)
                    memberArrayBuilder.Add(type);
            }

            // Build the return array
            return BuildMemberArray<Type>();
        }

        public override PropertyInfo[] GetProperties(BindingFlags bindingAttr)
        {
            foreach(PropertyInfo property in properties)
            {
                // Check for matching properties
                if (MatchPropertyNameAndAttributes(property, bindingAttr, null) == true)
                    memberArrayBuilder.Add(property);
            }

            // Build the result array
            return BuildMemberArray<PropertyInfo>();
        }

        public override object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters)
        {
            throw new NotImplementedException();
        }

        public override bool IsDefined(Type attributeType, bool inherit)
        {
            if (inherit == true)
            {
                // Check for defined on this method
                if (attributeProvider.Value.IsDefined(attributeType) == true)
                    return true;

                // Get attributes for base type
                if (baseType.Value != null && baseType.Value.IsDefined(attributeType, inherit) == true)
                    return true;

                return false;
            }

            // Simple case
            return attributeProvider.Value.IsDefined(attributeType);
        }

        protected override TypeAttributes GetAttributeFlagsImpl()
        {
            return (TypeAttributes)type.Attributes;
        }

        protected override ConstructorInfo GetConstructorImpl(BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
        {
            foreach(ConstructorInfo ctor in constructors)
            {
                // Check for matching constructors
                if (MatchMethodNameAndAttributes(ctor, bindingAttr, null) == true)
                    memberArrayBuilder.Add(ctor);
            }

            // Get the matching array
            ConstructorInfo[] matchedConstructors = BuildMemberArray<ConstructorInfo>();

            // Check for no types specified
            if (types == null)
            {
                // Get the first matched method
                if (matchedConstructors.Length > 0)
                    return matchedConstructors[0];

                return null;
            }

            // Check for special methods
            ConstructorInfo specialInitializer = CLRSpecialMethods.GetSpecialInitializer(domain, bindingAttr, binder, callConvention, types, modifiers);

            if (specialInitializer != null)
                return specialInitializer;

            // Check for no constructor
            if (matchedConstructors.Length == 0)
                return null;

            // Check for modifiers
            if (modifiers == null)
                modifiers = new ParameterModifier[0];

            // Select binder
            if (binder == null)
                binder = DefaultBinder;

            // Select the correct constructor
            MethodBase matchedCtor = binder.SelectMethod(bindingAttr, matchedConstructors, types, modifiers);

            // Get constructor result
            return matchedCtor as ConstructorInfo;
        }

        protected override MethodInfo GetMethodImpl(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
        {
            if (types == null)
                types = Type.EmptyTypes;

            // Check all methods
            foreach (MethodInfo method in methods)
            {
                // Check for matching name
                if (MatchMemberName(method, bindingAttr, name) == true)
                    memberArrayBuilder.Add(method);
            }

            // Check base methods
            if(baseType != null && (bindingAttr & BindingFlags.FlattenHierarchy) != 0)
            {
                MethodInfo method = baseType.Value.GetMethod(name, bindingAttr, binder, callConvention, types, modifiers);

                if (method != null)
                    memberArrayBuilder.Add(method);
            }

            // Build the match array
            MethodInfo[] matchedMethods = BuildMemberArray<MethodInfo>();

            // Check for no types specified
            if (types == null || types.Length == 0)
            {
                // Get the first matched method
                if (matchedMethods.Length > 0)
                    return matchedMethods[0];

                return null;
            }

            //if(name == "Get" && baseType.Value == typeof(Enum))
            //{
            //    Array.Resize(ref matchedMethods, matchedMethods.Length + 1);
            //    matchedMethods[matchedMethods.Length - 1] = typeof(CLRType).GetMethod("Get", BindingFlags.Static | BindingFlags.NonPublic);
            //}

            //if (name == "Set" && baseType.Value == typeof(Enum))
            //{
            //    Array.Resize(ref matchedMethods, matchedMethods.Length + 1);
            //    matchedMethods[matchedMethods.Length - 1] = typeof(CLRType).GetMethod("Set", BindingFlags.Static | BindingFlags.NonPublic);
            //}
            // Check for special methods
            MethodInfo specialMethod = CLRSpecialMethods.GetSpecialMethod(domain, this, name, bindingAttr, binder, callConvention, types, modifiers);

            if (specialMethod != null)
                return specialMethod;

            // Check for no matches
            if (matchedMethods.Length == 0)
                return null;

            // Check for modifiers
            if (modifiers == null)
                modifiers = new ParameterModifier[0];

            // Select binder
            if (binder == null)
                binder = DefaultBinder;

            // Select the correct method
            MethodBase matchedMethod = binder.SelectMethod(bindingAttr, matchedMethods, types, modifiers);

            // Get constructor result
            return matchedMethod as MethodInfo;
        }

        protected override PropertyInfo GetPropertyImpl(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers)
        {
            // Check all properties
            foreach(PropertyInfo property in properties)
            {
                // Check for matching name
                if(MatchMemberName(property, bindingAttr, name) == true)
                    memberArrayBuilder.Add(property);
            }

            // Build the match array
            PropertyInfo[] matchedProperties = BuildMemberArray<PropertyInfo>();

            // Check for no types specified
            if (returnType == null || types == null)
            {
                // Get the first matched method
                if (matchedProperties.Length > 0)
                    return matchedProperties[0];

                return null;
            }

            // Check for modifiers
            if (modifiers == null)
                modifiers = new ParameterModifier[0];

            // Select binder
            if (binder == null)
                binder = DefaultBinder;

            // Select the correct constructor
            return binder.SelectProperty(bindingAttr, matchedProperties, returnType, types, modifiers);
        }

        protected override bool HasElementTypeImpl()
        {
            return elementType != null;
        }

        protected override bool IsArrayImpl()
        {
            return elementType != null;
        }

        public override int GetArrayRank()
        {
            return arrayRank;
        }

        protected override bool IsByRefImpl()
        {
            return false;
        }

        protected override bool IsCOMObjectImpl()
        {
            return false;
        }

        protected override bool IsPointerImpl()
        {
            return false;
        }

        protected override bool IsPrimitiveImpl()
        {
            return false;
        }

        public override Type MakeGenericType(params Type[] typeArguments)
        {
            // Create the return type
            CLRType genericType = new CLRType(domain, module, type);

            // Set generic definition
            genericType.genericDefinition = this;

            // Setup generics
            genericType.genericTypes = new Type[typeArguments.Length];

            // Allocate types
            for(int i = 0; i < typeArguments.Length; i++)
            {
                genericType.genericTypes[i] = typeArguments[i];
            }

            return genericType;
        }

        public override Type MakeArrayType(int rank)
        {
            // Create the return type
            CLRType arrayType = new CLRType(domain, module, type);

            // Set array definition
            arrayType.elementType = this;
            arrayType.arrayRank = rank;

            return arrayType;
        }

        public override Type GetGenericTypeDefinition()
        {
            return genericDefinition;
        }

        private bool MatchTypeNameAndAttributes(Type type, BindingFlags bindingAttr, string name)
        {
            // Check for name specified
            if (name != null)
            {
                // Check for matching name
                if (MatchMemberName(type, bindingAttr, name) == false)
                    return false;
            }

            // Check for attributes
            if ((type.IsPublic == true && (bindingAttr & BindingFlags.Public) == 0) ||
                (type.IsPublic == false && (bindingAttr & BindingFlags.NonPublic) == 0))
            {
                // Skip this type
                return false;
            }

            // Type match
            return true;
        }

        private bool MatchFieldNameAndAttributes(FieldInfo field, BindingFlags bindingAttr, string name)
        {
            // Check for name specified
            if (name != null)
            {
                // Check for matching name
                if (MatchMemberName(field, bindingAttr, name) == false)
                    return false;
            }

            // Check for attributes
            if ((field.IsPublic == true && (bindingAttr & BindingFlags.Public) == 0) ||
                (field.IsPublic == false && (bindingAttr & BindingFlags.NonPublic) == 0) ||
                (field.IsStatic == false && (bindingAttr & BindingFlags.Instance) == 0) ||
                (field.IsStatic == true && (bindingAttr & BindingFlags.Static) == 0))
            {
                // Skip this field
                return false;
            }

            // Field match
            return true;
        }

        private bool MatchPropertyNameAndAttributes(PropertyInfo property, BindingFlags bindingAttr, string name)
        {
            // Check for name specified
            if (name != null)
            {
                // Check for matching name
                if (MatchMemberName(property, bindingAttr, name) == false)
                    return false;
            }

            StorageAccessorMode accessorMode = StorageAccessorMode.Any;

            // Check for explicit set
            if ((bindingAttr & BindingFlags.SetProperty) != 0)
                accessorMode |= StorageAccessorMode.Set;

            // Check for explicit get
            if ((bindingAttr & BindingFlags.GetProperty) != 0)
                accessorMode |= StorageAccessorMode.Get;

            // Check for read write property
            if ((property.CanRead == false && (accessorMode & StorageAccessorMode.Get) != 0) ||
                property.CanWrite == false && (accessorMode & StorageAccessorMode.Set) != 0)
            {
                // Skip this property
                return false;
            }

            // Try to get accessor
            MethodInfo accessor = (property.CanRead == true) ?
                property.GetGetMethod(true) :
                (property.CanWrite == true) ?
                property.GetSetMethod(true) :
                null;

            // Check for accessor
            if (accessor == null)
                return false;

            // Try to match the accessor
            return MatchMethodNameAndAttributes(accessor, bindingAttr, null);
        }

        private bool MatchMethodNameAndAttributes(MethodBase method, BindingFlags bindingAttr, string name)
        {
            // Check for name specified
            if(name != null)
            {
                // Check for matching name
                if (MatchMemberName(method, bindingAttr, name) == false)
                    return false;
            }

            // Check for attributes
            if ((method.IsPublic == true && (bindingAttr & BindingFlags.Public) == 0) ||
                (method.IsPublic == false && (bindingAttr & BindingFlags.NonPublic) == 0) ||
                (method.IsStatic == false && (bindingAttr & BindingFlags.Instance) == 0) ||
                (method.IsStatic == true && (bindingAttr & BindingFlags.Static) == 0))
            {
                // Skip this method
                return false;
            }

            // Method match
            return true;
        }

        private bool MatchMemberName(MemberInfo member, BindingFlags bindingAttr, string name)
        {
            // Check for null
            if (member == null)
                throw new ArgumentNullException("member");

            // Check for case sensitive
            bool ignoreCase = (bindingAttr & BindingFlags.IgnoreCase) != 0;

            // Compare names
            return string.Compare(member.Name, name, ignoreCase) == 0;
        }

        private T[] BuildMemberArray<T>() where T : MemberInfo
        {
            // Allocate new array
            T[] resultArray = new T[memberArrayBuilder.Count];

            // Copy elements
            for (int i = 0; i < memberArrayBuilder.Count; i++)
                resultArray[i] = memberArrayBuilder[i] as T;

            // Reset shared list
            memberArrayBuilder.Clear();

            return resultArray;
        }

        internal void StaticInitializeType()
        {
            // Prevent reccursive loops
            if (isStaticInitializing == true)
                return;

            if (isStaticInitialized == false)
            {
                // Get the initializer info
                isStaticInitializing = true;
                staticTypeInitializer = TypeInitializer;

                try
                {
                    // Initialize static fields
                    foreach (CLRField field in fields)
                        field.StaticInitialize();

                    // Invoke static constructor if available
                    if (staticTypeInitializer != null)
                        staticTypeInitializer.Invoke(null, null);
                }
                catch (Exception e)
                {
                    throw new TypeInitializationException(ToString(), e);
                }

                // Set initialized flag
                isStaticInitialized = true;
                isStaticInitializing = false;
            }
        }

        public static bool operator==(CLRType a, CLRType b)
        {
            // Handle null cases
            if (ReferenceEquals(a, null) == true || ReferenceEquals(b, null) == true)
                return ReferenceEquals(a, b) == true;

            // Check type equality
            return a.type == b.type;
        }

        public static bool operator!=(CLRType a, CLRType b)
        {
            // Handle null cases
            if (ReferenceEquals(a, null) == true || ReferenceEquals(b, null) == true)
                return ReferenceEquals(a, b) == false;

            // Check type equality
            return a.type != b.type;
        }
    }
}
