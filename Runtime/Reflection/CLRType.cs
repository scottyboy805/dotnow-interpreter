using System;
using System.Globalization;
using System.Reflection;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Collections.Immutable;
using System.Linq;
using System.Data;
using dotnow.Runtime;
using dotnow.Runtime.CIL;

namespace dotnow.Reflection
{
    internal sealed class CLRType : Type
    {
        // Internal
        internal readonly MetadataReferenceProvider metadataProvider = null;
        internal readonly TypeDefinitionHandle handle = default;
        internal readonly TypeDefinition definition = default;

        // Private
        private static readonly List<MemberInfo> memberArrayBuilder = new List<MemberInfo>();
        private static readonly Stack<CLRFieldInfo> fieldMembersBuilder = new Stack<CLRFieldInfo>();

        private readonly Lazy<int> metadataToken = null;        
        private readonly Lazy<string> name = null;
        private readonly Lazy<string> namespaceName = null;
        private readonly Lazy<string> fullName = null;
        private readonly Lazy<string> assemblyQualifiedName = null;
        private readonly Lazy<string> toString = null;

        private readonly Lazy<Type> baseType = null;
        private readonly Lazy<Type[]> interfaceTypes = null;
        private readonly Lazy<Type[]> genericTypes = null;
        private readonly Lazy<Dictionary<object, string>> enumNames = null;

        private readonly Lazy<CLRFieldInfo[]> instanceFields = null;
        private readonly Lazy<CLRFieldInfo[]> staticFields = null;

        private bool isEnum = false;
        private bool isByRef = false;
        
        private readonly CLRType[] nestedTypes = null;
        private readonly CLRFieldInfo[] fields = null;
        //private CLRProperty[] properties = null;
        private readonly CLRConstructorInfo[] constructors = null;
        private readonly CLRMethodInfo[] methods = null;

        // Public
        public static readonly Binder Binder = new CLRBinder();

        // Properties
        internal AppDomain AppDomain => metadataProvider.AppDomain;
        internal AssemblyLoadContext AssemblyLoadContext => metadataProvider.AssemblyLoadContext;
        internal IReadOnlyList<CLRFieldInfo> CLRFields => fields;
        internal IReadOnlyList<CLRConstructorInfo> CLRConstructors => constructors;
        internal IReadOnlyList<CLRMethodInfo> CLRMethods => methods;
        internal IDictionary<object, string> CLREnumNames => enumNames.Value;

        internal CLRFieldInfo[] InstanceFields => instanceFields.Value;
        internal CLRFieldInfo[] StaticFields => staticFields.Value;

        #region TypeProperties
        public override Assembly Assembly => metadataProvider.AssemblyLoadContext.Assembly;
        public override Type UnderlyingSystemType => typeof(CLRType);

        public override int MetadataToken => metadataToken.Value;
        public override string Name => name.Value;
        public override string Namespace => namespaceName.Value;
        public override string FullName => fullName.Value;
        public override string AssemblyQualifiedName => assemblyQualifiedName.Value;      
        public override Type BaseType => baseType.Value;
        
        public override Guid GUID => throw new NotImplementedException();
        public override Module Module => throw new NotImplementedException();
        #endregion

        // Constructor
        internal CLRType(MetadataReferenceProvider metadataProvider, in TypeDefinitionHandle handle)
        {            
            this.metadataProvider = metadataProvider;
            this.handle = handle;
            this.definition = metadataProvider.MetadataReader.GetTypeDefinition(handle);

            // Init members
            this.metadataToken = new(InitToken);
            this.name = new(InitName);
            this.namespaceName = new(InitNamespaceName);
            this.fullName = new(InitFullName);
            this.assemblyQualifiedName = new(InitAssemblyQualifiedName);
            this.toString = new(InitToString);

            this.baseType = new(InitBaseType);
            this.interfaceTypes = new(InitInterfaceTypes);
            this.genericTypes = new(InitGenericTypes);

            //if(isEnum == true)
            this.enumNames = new(InitEnumNames);

            this.instanceFields = new(InitInstanceFields);
            this.staticFields = new(InitStaticFields);
            

            // Nested type members
            {
                // Get nested
                ImmutableArray<TypeDefinitionHandle> nestedTypes = definition.GetNestedTypes();

                // Init array
                this.nestedTypes = new CLRType[nestedTypes.Length];

                // Process all
                for (int i = 0; i < nestedTypes.Length; i++)
                {
                    // Create new nested type
                    this.nestedTypes[i] = new CLRType(metadataProvider, nestedTypes[i]);
                }
            }

            // Field members
            {
                // Get field handles
                FieldDefinitionHandleCollection fieldHandles = definition.GetFields();

                // Init fields
                this.fields = new CLRFieldInfo[fieldHandles.Count];

                // Process all
                int index = 0;
                foreach(FieldDefinitionHandle fieldHandle in fieldHandles)
                {
                    // Get the definition
                    FieldDefinition fieldDefinition = metadataProvider.MetadataReader.GetFieldDefinition(fieldHandle);

                    string name = metadataProvider.MetadataReader.GetString(fieldDefinition.Name);

                    // Create new field
                    fields[index] = new CLRFieldInfo(metadataProvider, this, fieldHandle, fieldDefinition);
                    index++;
                }
            }

            // Method and constructor members
            {
                // Get method handles;
                MethodDefinitionHandleCollection methodHandles = definition.GetMethods();

                List<(MethodDefinitionHandle, MethodDefinition)> constructorDefinitions = null;
                List<(MethodDefinitionHandle, MethodDefinition)> methodDefinitions = null;

                // Process all
                foreach(MethodDefinitionHandle methodHandle in methodHandles)
                {
                    // Get the definition
                    MethodDefinition methodDefinition = metadataProvider.MetadataReader.GetMethodDefinition(methodHandle);

                    // Check for constructor
                    string specialName = null;
                    if((methodDefinition.Attributes & MethodAttributes.RTSpecialName) != 0 &&
                        ((specialName = metadataProvider.MetadataReader.GetString(methodDefinition.Name)) == ".ctor" ||
                        (specialName = metadataProvider.MetadataReader.GetString(methodDefinition.Name)) == ".cctor"))
                    {
                        // Init collection
                        if (constructorDefinitions == null)
                            constructorDefinitions = new();

                        // Add constructor
                        constructorDefinitions.Add((methodHandle, methodDefinition));
                    }
                    // Must be a normal method
                    else
                    {
                        // Init collection
                        if (methodDefinitions == null)
                            methodDefinitions = new();

                        // Add method
                        methodDefinitions.Add((methodHandle, methodDefinition));
                    }
                }

                // Create all constructors
                {
                    // Create array
                    this.constructors = new CLRConstructorInfo[constructorDefinitions != null ? constructorDefinitions.Count : 0];

                    // Init constructors
                    for(int i = 0; i < this.constructors.Length; i++)
                    {
                        // Create new constructor member
                        this.constructors[i] = new CLRConstructorInfo(metadataProvider, this, constructorDefinitions[i].Item1, constructorDefinitions[i].Item2);
                    }
                }

                // Create all methods
                {
                    // Create array
                    this.methods = new CLRMethodInfo[methodDefinitions != null ? methodDefinitions.Count : 0];

                    // init methods
                    for(int i = 0; i < this.methods.Length; i++)
                    {                        
                        // Create new method member
                        this.methods[i] = new CLRMethodInfo(metadataProvider, this, methodDefinitions[i].Item1, methodDefinitions[i].Item2);
                    }
                }
            }
        }

        // Methods
        public override string ToString()
        {
            return toString.Value;
        }

        public int GetInstanceFieldOffset(CLRFieldInfo field)
        {
            return Array.IndexOf(InstanceFields, field);
        }

        public int GetStaticFieldOffset(CLRFieldInfo field)
        {
            return Array.IndexOf(StaticFields, field);
        }

        public override ConstructorInfo[] GetConstructors(BindingFlags bindingAttr)
        {
            throw new NotImplementedException();
        }

        public override object[] GetCustomAttributes(bool inherit)
        {
            throw new NotImplementedException();
        }

        public override object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            throw new NotImplementedException();
        }

        public override Type GetElementType()
        {
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
            throw new NotImplementedException();
        }

        public override FieldInfo[] GetFields(BindingFlags bindingAttr)
        {
            foreach (FieldInfo field in fields)
            {
                // Check for matching field
                if (MatchFieldNameAndAttributes(field, bindingAttr, null) == true)
                    memberArrayBuilder.Add(field);
            }

            // Check base type
            if (baseType != null && (bindingAttr & BindingFlags.FlattenHierarchy) != 0)
            {
                // Get all matching child fields
                foreach (FieldInfo field in BaseType.GetFields(bindingAttr))
                    memberArrayBuilder.Add(field);
            }

            // Build the return array
            return BuildMemberArray<FieldInfo>();
        }

        public override Type GetInterface(string name, bool ignoreCase)
        {
            throw new NotImplementedException();
        }

        public override Type[] GetInterfaces()
        {
            return interfaceTypes.Value;
        }

        public override MemberInfo[] GetMembers(BindingFlags bindingAttr)
        {
            // Select types
            IEnumerable<MemberInfo> typeMembers = nestedTypes.Where(t => MatchTypeAttributes(t, bindingAttr));

            // Select fields
            IEnumerable<MemberInfo> fieldMembers = fields.Where(f => MatchFieldAttributes(f, bindingAttr));

            // Select constructors
            IEnumerable<ConstructorInfo> constructorMembers = constructors.Where(c => MatchMethodAttributes(c, bindingAttr));

            // Select methods
            IEnumerable<MemberInfo> methodMembers = methods.Where(m => MatchMethodAttributes(m, bindingAttr));

            // Get all members
            return typeMembers
                .Concat(fieldMembers)
                .Concat(constructorMembers)
                .Concat(methodMembers)
                .ToArray();
        }

        public override MethodInfo[] GetMethods(BindingFlags bindingAttr)
        {
            throw new NotImplementedException();
        }

        public override Type GetNestedType(string name, BindingFlags bindingAttr)
        {
            throw new NotImplementedException();
        }

        public override Type[] GetNestedTypes(BindingFlags bindingAttr)
        {
            throw new NotImplementedException();
        }

        public override PropertyInfo[] GetProperties(BindingFlags bindingAttr)
        {
            throw new NotImplementedException();
        }

        public override object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters)
        {
            throw new NotImplementedException();
        }

        public override bool IsDefined(Type attributeType, bool inherit)
        {
            throw new NotImplementedException();
        }

        protected override TypeAttributes GetAttributeFlagsImpl()
        {
            return definition.Attributes;
        }

        protected override ConstructorInfo GetConstructorImpl(BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
        {
            // Check all constructors
            foreach (MethodBase ctor in constructors)
            {
                // Check for matching name
                if(MatchMethodAttributes(ctor, bindingAttr) == true)
                    memberArrayBuilder.Add(ctor);
            }

            // Check for any
            if (memberArrayBuilder.Count == 0)
                return null;


            // Build the match array
            ConstructorInfo[] matchedConstructors = BuildMemberArray<ConstructorInfo>();

            // Check for modifiers
            if (modifiers == null)
                modifiers = new ParameterModifier[0];

            // Select binder
            if (binder == null)
                binder = DefaultBinder;

            // Select the correct method
            MethodBase matchedMethod = binder.SelectMethod(bindingAttr, matchedConstructors, types, modifiers);

            // Get constructor result
            return matchedMethod as ConstructorInfo;

        }

        protected override MethodInfo GetMethodImpl(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
        {
            // Check all methods
            foreach (MethodInfo method in methods)
            {
                // Check for matching name
                if (MatchMemberName(method, bindingAttr, name) == true)
                    memberArrayBuilder.Add(method);
            }

            // Check base methods
            if (baseType != null && (bindingAttr & BindingFlags.FlattenHierarchy) != 0)
            {
                MethodInfo method = BaseType.GetMethod(name, bindingAttr, binder, callConvention, types, modifiers);

                if (method != null)
                    memberArrayBuilder.Add(method);
            }

            // Build the match array
            MethodInfo[] matchedMethods = BuildMemberArray<MethodInfo>();

            // Check for no types specified and single match
            if (types == null && matchedMethods.Length <= 1)
            {
                // Get the first matched method
                if (matchedMethods.Length > 0)
                    return matchedMethods[0];

                return null;
            }

            //// Check for special methods
            //MethodInfo specialMethod = CLRSpecialMethods.GetSpecialMethod(domain, this, name, bindingAttr, binder, callConvention, types, modifiers);

            //if (specialMethod != null)
            //    return specialMethod;

            // Check for no matches
            if (matchedMethods.Length == 0)
                return null;

            // Check for modifiers
            if (modifiers == null)
                modifiers = new ParameterModifier[0];

            // Select binder
            if (binder == null)
                binder = DefaultBinder;

            // Select types
            if (types == null)
                types = Type.EmptyTypes;

            // Select the correct method
            MethodBase matchedMethod = binder.SelectMethod(bindingAttr, matchedMethods, types, modifiers);

            // Get constructor result
            return matchedMethod as MethodInfo;
        }

        protected override PropertyInfo GetPropertyImpl(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers)
        {
            throw new NotImplementedException();
        }

        protected override bool HasElementTypeImpl()
        {
            throw new NotImplementedException();
        }

        protected override bool IsArrayImpl()
        {
            return false;
        }

        protected override bool IsByRefImpl()
        {
            return isByRef;
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

        protected override bool IsValueTypeImpl()
        {
            // Check for value type base
            return BaseType == typeof(ValueType);
        }

        public override Type MakeArrayType()
        {
            return new CLRArrayType(this, 1);
        }

        public override Type MakeArrayType(int rank)
        {
            // Check rank bounds
            if (rank < 1 || rank > 32)
                throw new ArgumentException("Array rank is not valid");

            // Create the CLR array
            return new CLRArrayType(this, rank);
        }

        public override Type MakeByRefType()
        {
            // Create a copy type
            CLRType byRefType = new CLRType(metadataProvider, handle);

            // Set by ref flag
            byRefType.isByRef = true;

            return byRefType;
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
            return MatchTypeAttributes(type, bindingAttr);
        }

        private bool MatchTypeAttributes(Type type, BindingFlags bindingAttr)
        {
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
            return MatchFieldAttributes(field, bindingAttr);
        }

        private bool MatchFieldAttributes(FieldInfo field, BindingFlags bindingAttr)
        {
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

        //private bool MatchPropertyNameAndAttributes(PropertyInfo property, BindingFlags bindingAttr, string name)
        //{
        //    // Check for name specified
        //    if (name != null)
        //    {
        //        // Check for matching name
        //        if (MatchMemberName(property, bindingAttr, name) == false)
        //            return false;
        //    }

        //    StorageAccessorMode accessorMode = StorageAccessorMode.Any;

        //    // Check for explicit set
        //    if ((bindingAttr & BindingFlags.SetProperty) != 0)
        //        accessorMode |= StorageAccessorMode.Set;

        //    // Check for explicit get
        //    if ((bindingAttr & BindingFlags.GetProperty) != 0)
        //        accessorMode |= StorageAccessorMode.Get;

        //    // Check for read write property
        //    if ((property.CanRead == false && (accessorMode & StorageAccessorMode.Get) != 0) ||
        //        property.CanWrite == false && (accessorMode & StorageAccessorMode.Set) != 0)
        //    {
        //        // Skip this property
        //        return false;
        //    }

        //    // Try to get accessor
        //    MethodInfo accessor = (property.CanRead == true) ?
        //        property.GetGetMethod(true) :
        //        (property.CanWrite == true) ?
        //        property.GetSetMethod(true) :
        //        null;

        //    // Check for accessor
        //    if (accessor == null)
        //        return false;

        //    // Try to match the accessor
        //    return MatchMethodNameAndAttributes(accessor, bindingAttr, null);
        //}

        private bool MatchMethodNameAndAttributes(MethodBase method, BindingFlags bindingAttr, string name)
        {
            // Check for name specified
            if (name != null)
            {
                // Check for matching name
                if (MatchMemberName(method, bindingAttr, name) == false)
                    return false;
            }

            // Check for attributes
            return MatchMethodAttributes(method, bindingAttr);
        }

        private bool MatchMethodAttributes(MethodBase method, BindingFlags bindingAttr)
        {
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
                throw new ArgumentNullException(nameof(member));

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

        #region LazyInit
        private int InitToken()
        {
            return metadataProvider.MetadataReader.GetToken(handle);
        }

        private string InitName()
        {
            return metadataProvider.MetadataReader.GetString(definition.Name);
        }

        private string InitNamespaceName()
        {
            return metadataProvider.MetadataReader.GetString(definition.Namespace);
        }

        private string InitFullName()
        {
            // Check for namespace
            if (definition.Namespace.IsNil == false)
            {
                return string.Format("{0}.{1}", metadataProvider.MetadataReader.GetString(definition.Namespace),
                    metadataProvider.MetadataReader.GetString(definition.Name));
            }

            return Name;
        }

        private string InitAssemblyQualifiedName()
        {
            return string.Format("{0}, {1}", FullName, metadataProvider.AssemblyLoadContext.Assembly.FullName);
        }

        private string InitToString()
        {
            return FullName;
        }

        private Type InitBaseType()
        {
            // Check for nil
            if (definition.BaseType.IsNil == true)
                return null;

            // Resolve type reference
            return metadataProvider.ResolveMetadataType(definition.BaseType);
        }

        private Type[] InitInterfaceTypes()
        {
            // Get interface handles
            InterfaceImplementationHandleCollection interfaceHandles = definition.GetInterfaceImplementations();

            // Init array
            Type[] interfaces = new Type[interfaceHandles.Count];

            // Process all
            int index = 0;
            foreach (InterfaceImplementationHandle handle in interfaceHandles)
            {
                // Get interface implementation
                InterfaceImplementation implementation = metadataProvider.MetadataReader.GetInterfaceImplementation(handle);

                // Insert resolved
                interfaces[index] = metadataProvider.ResolveMetadataType(implementation.Interface);
                index++;
            }
            return interfaces;
        }

        private Type[] InitGenericTypes()
        {
            return null;
        }

        private Dictionary<object, string> InitEnumNames()
        {
            // Create result
            Dictionary<object, string> enumFields = new();

            // Get all enum fields
            foreach(FieldInfo enumField in GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                // Get raw value
                object value = enumField.GetRawConstantValue();

                // Push named value
                enumFields[value] = enumField.Name;
            }
            return enumFields;
        }

        private CLRFieldInfo[] InitInstanceFields()
        {
            return EnumerateFields(this)
                .Where(f => f.IsStatic == false)
                .ToArray();
        }

        private CLRFieldInfo[] InitStaticFields()
        {
            return EnumerateFields(this)
                .Where(f => f.IsStatic == true)
                .ToArray();
        }

        private IEnumerable<CLRFieldInfo> EnumerateFields(CLRType type)
        {
            if (type.BaseType is CLRType baseTypeCLR)
            {
                // Get base fields first
                foreach (CLRFieldInfo baseField in EnumerateFields(baseTypeCLR))
                    yield return baseField;
            }

            // Get this fields
            foreach (CLRFieldInfo field in type.fields)
                yield return field;
        }
        #endregion
    }
}
