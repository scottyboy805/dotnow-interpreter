using dotnow.Reflection;
using System;
using System.Collections.Generic;
using System.Reflection;

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
    }

    internal sealed class CILTypeInfo
    {
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
        /// For CLR types it gets the interop base type and interface types that are implemented.
        /// </summary>
        public readonly Type[] InteropTypes;
        /// <summary>
        /// For CLR instances it represents the number of instance fields.
        /// </summary>
        public readonly int InstanceSize;
        /// <summary>
        /// For CLR types it represents the number of static fields.
        /// </summary>
        public readonly int StaticSize;

        // Constructor
        internal CILTypeInfo(Type type)
        {
            this.Type = type;
            this.Flags = GetFlags(type);
            this.TypeCode = Type.GetTypeCode(type);

            // Check for CLR
            if((Flags & CILTypeFlags.Interpreted) != 0)
            {
                // Get the interop types
                this.InteropTypes = GetInteropTypes(type);

                // Get the type size
                GetTypeSize(type, out this.InstanceSize, out this.StaticSize);
            }
        }

        // Methods
        public override string ToString()
        {
            return $"{Type} = {Flags}";
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

                // Interop
                if (isCLr == false) flags |= CILTypeFlags.Interop;

                // Interpreted
                if (isCLr == true) flags |= CILTypeFlags.Interpreted;
            }
            return flags;
        }

        private static Type[] GetInteropTypes(Type fromType)
        {
            List<Type> implemented = new(4);

            // Add immediate base
            implemented.Add(fromType);

            // Add interfaces
            implemented.AddRange(fromType.GetInterfaces());

            // Get the array
            return implemented.ToArray();
        }

        private static void GetTypeSize(Type fromType, out int instanceSize, out int staticSize)
        {
            int i = 0, s = 0;

            Type current = fromType;

            while(current != null && current is CLRType)
            {
                // Get fields
                foreach(FieldInfo field in current.GetFields())
                {
                    // Check for instance
                    if(field.IsStatic == false)
                    {
                        // Increase instance size
                        i++;
                    }
                    else
                    {
                        // Increase static size
                        s++;
                    }
                }

                // Get base
                current = current.BaseType;
            }

            instanceSize = i;
            staticSize = s;
        }
    }
}