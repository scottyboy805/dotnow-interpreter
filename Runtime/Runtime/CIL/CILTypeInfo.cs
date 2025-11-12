using dotnow.Reflection;
using System;

namespace dotnow.Runtime.CIL
{
    [Flags]
    internal enum CILTypeFlags : ushort
    {
        None = 0,
        Abstract = 1 << 0,
        Enum = 1 << 2,
        Array = 1 << 3,
        MultiArray = 1 << 4,
        Interop = 1 << 5,
        Interpreted = 1 << 6,

        PrimitiveType = 1 << 8,
        ReferenceType = 1 << 9,
        ValueType = 1 << 10,
        NullableType = 1 << 11,
    }

    internal sealed class CILTypeInfo
    {
        // Private
        private bool staticInit = false;
        private AppDomain domain = null;

        private readonly Lazy<CILTypeInfo[]> instanceFields;
        private readonly Lazy<StackData[]> staticFields;

        // Public
        /// <summary>
        /// The associated metadata type.
        /// </summary>
        public readonly Type Type;        
        /// <summary>
        /// The type flags for this handle which specify how the type should be used.
        /// </summary>
        public readonly CILTypeFlags Flags;
        /// <summary>
        /// The type code of this type handle for interoperability.
        /// </summary>
        public readonly TypeCode TypeCode;
        /// <summary>
        /// For CLR types it represents the first base type that is an interop type (not interpreted).
        /// Only available for interpreted types.
        /// </summary>
        public readonly Type InteropBaseType;
        /// <summary>
        /// For CLR types it gets the interop base type and interface types that are implemented.
        /// Only available for interpreted types.
        /// </summary>
        public readonly Type[] InteropImplementationTypes;
        /// <summary>
        /// The VTable used to get virtual methods from this type.
        /// Only available for interpreted types which are not marked as sealed.
        /// </summary>
        public readonly CILVTable VTable;

        // Properties
        /// <summary>
        /// The default field memory for an instance of this type, with each field initialized to default value.
        /// Should be copied for each instance to reserve a unique memory for an instance.
        /// Only available for interpreted types.
        /// </summary>
        public CILTypeInfo[] InstanceFields => instanceFields.Value;
        /// <summary>
        /// The memory where static field data is stored for this type.
        /// Only available for interpreted types.
        /// </summary>
        public StackData[] StaticFields => staticFields.Value;

        // Constructor
        internal CILTypeInfo(AppDomain domain, Type type)
        {
            this.domain = domain;
            this.Type = type;
            this.Flags = GetFlags(type);
            this.TypeCode = Type.GetTypeCode(type);

            // Check for CLR
            if((Flags & CILTypeFlags.Interpreted) != 0)
            {
                // Create VTable
                if (type.IsSealed == false)
                    this.VTable = new CILVTable(type);

                // Get the base type
                this.InteropBaseType = GetInteropBaseType(type);

                // Get the interop implementation types
                this.InteropImplementationTypes = type.GetInterfaces();

                // Initialize instance and static layout on demand
                this.instanceFields = new(InitInstanceFields);
                this.staticFields = new(InitStaticFields);
            }
        }

        // Methods
        public override string ToString()
        {
            return $"{Type} = {Flags}";
        }

        private CILTypeInfo[] InitInstanceFields()
        {
            // Get instance size
            int instanceSize = ((CLRType)Type).InstanceFields.Length;

            // Init fields
            CILTypeInfo[] instanceFields = new CILTypeInfo[instanceSize];

            // Initialize fields
            for (int i = 0; i < instanceSize; i++)
            {
                // Get the field type handle
                instanceFields[i] = ((CLRType)Type).InstanceFields[i].FieldType.GetTypeInfo(domain);
            }
            return instanceFields;
        }

        private StackData[] InitStaticFields()
        {
            // Get static size
            int staticSize = ((CLRType)Type).StaticFields.Length;

            // Init fields
            StackData[] staticFields = new StackData[staticSize];

            for (int i = 0; i < staticSize; i++)
            {
                // Get the field type handle
                CILTypeInfo fieldTypeInfo = ((CLRType)Type).StaticFields[i].FieldType.GetTypeInfo(domain);

                // Init to default
                staticFields[i] = StackData.Default(domain, fieldTypeInfo);
            }
            return staticFields;
        }

        public void StaticInitialize()
        {
            // Run static initializer
            if (staticInit == false && Type.TypeInitializer != null)
            {
                // Only run initializer once
                staticInit = true;
                Type.TypeInitializer.Invoke(null);
            }
        }

        private static CILTypeFlags GetFlags(Type fromType)
        {
            // Init flags
            CILTypeFlags flags = 0;
            {
                // Get attributes
                bool isCLr = fromType is CLRType;

                // Abstract
                if (fromType.IsAbstract == true) flags |= CILTypeFlags.Abstract;

                // Enum
                if (fromType.IsEnum == true) flags |= CILTypeFlags.Enum;

                // Array
                if (fromType.IsArray == true) flags |= CILTypeFlags.Array;
                if (fromType.IsArray == true && fromType.GetArrayRank() > 1) flags |= CILTypeFlags.MultiArray;

                // Primitive
                if (fromType.IsPrimitive == true) flags |= CILTypeFlags.PrimitiveType;

                // Reference type
                if (fromType.IsClass == true) flags |= CILTypeFlags.ReferenceType;

                // Value type
                if ((flags & CILTypeFlags.ReferenceType) == 0 && (flags & CILTypeFlags.PrimitiveType) == 0 && fromType.IsValueType == true) flags |= CILTypeFlags.ValueType;

                // Nullable type
                if (Nullable.GetUnderlyingType(fromType) != null) flags |= CILTypeFlags.NullableType;

                // Interop
                if (isCLr == false) flags |= CILTypeFlags.Interop;

                // Interpreted
                if (isCLr == true) flags |= CILTypeFlags.Interpreted;
            }
            return flags;
        }

        private static Type GetInteropBaseType(Type type)
        {
            // Check for value type
            if (type.IsValueType == true)
                return typeof(object);

            Type current = type.BaseType;

            // Move down the hierarchy until we have a valid system base type
            while (current.IsCLRType() == true)
                current = current.BaseType;

            // Check for none
            if (current == null)
                current = typeof(object);

            return current;
        }

        private static void GetTypeLayout(AppDomain domain, CLRType fromType, out StackData[] instanceFields, out StackData[] staticFields)
        {
            // Get instance size
            int instanceSize = fromType.InstanceFields.Length;

            // Init fields
            instanceFields = new StackData[instanceSize];

            // Initialize fields
            for (int i = 0; i < instanceSize; i++)
            {
                // Get the field type handle
                CILTypeInfo fieldTypeInfo = fromType.InstanceFields[i].FieldType.GetTypeInfo(domain);
                instanceFields[i] = StackData.Default(domain, fieldTypeInfo);
            }

            // Get static size
            int staticSize = fromType.StaticFields.Length;

            // Init fields
            staticFields = new StackData[staticSize];

            for (int i = 0; i < staticSize; i++)
            {
                // Get the field type handle
                CILTypeInfo fieldTypeInfo = fromType.StaticFields[i].FieldType.GetTypeInfo(domain);
                staticFields[i] = StackData.Default(domain, fieldTypeInfo);
            }
        }
    }
}