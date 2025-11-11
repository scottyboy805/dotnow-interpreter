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
        /// </summary>
        public readonly Type InteropBaseType;
        /// <summary>
        /// For CLR types it gets the interop base type and interface types that are implemented.
        /// </summary>
        public readonly Type[] InteropImplementationTypes;
        /// <summary>
        /// The VTable used to get virtual methods from this type.
        /// </summary>
        public readonly CILVTable VTable;
        /// <summary>
        /// The memory where static field data is stored for this type.
        /// </summary>
        public readonly StackData[] StaticFields;
        /// <summary>
        /// For CLR instances it represents the number of instance fields.
        /// </summary>
        public readonly int InstanceSize;

        // Constructor
        internal CILTypeInfo(Type type)
        {
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

                // Get the type size
                GetTypeSize((CLRType)type, out this.InstanceSize, out this.StaticFields);
            }
        }

        // Methods
        public override string ToString()
        {
            return $"{Type} = {Flags}";
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

        private static void GetTypeSize(CLRType fromType, out int instanceSize, out StackData[] staticFields)
        {
            // Get instance size
            instanceSize = fromType.InstanceFields.Length;

            // Get static size
            int staticSize = fromType.StaticFields.Length;

            // Init fields
            staticFields = new StackData[staticSize];

            for (int i = 0; i < staticSize; i++)
                staticFields[i] = default;
        }
    }
}