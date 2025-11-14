using dotnow.Interop;
using dotnow.Reflection;
using System;
using System.Reflection;

namespace dotnow.Runtime.CIL
{
    [Flags]
    internal enum CILFieldFlags : ushort
    {
        None = 0,
        This = 1 << 1,
        Interop = 1 << 2,
        Interpreted = 1 << 3,
        Constant = 1 << 4,

        DirectReadDelegate = 1 << 10,
        DirectWriteDelegate = 1 << 11,
    }

    internal sealed class CILFieldInfo
    {
        // Public
        /// <summary>
        /// The associated metadata field.
        /// </summary>
        public readonly FieldInfo Field;
        /// <summary>
        /// The associated type info for the declaring type.
        /// </summary>
        public readonly CILTypeInfo DeclaringType;
        /// <summary>
        /// The associated type info for the field type.
        /// </summary>
        public readonly CILTypeInfo FieldType;
        /// <summary>
        /// The method flags which specify how the field should be used.
        /// </summary>
        public readonly CILFieldFlags Flags;
        /// <summary>
        /// The optional delegate if an interop read binding is associated with this field.
        /// </summary>
        public readonly Delegate InteropReadAccess;
        /// <summary>
        /// The optional delegate if an interop write binding is associated with this field.
        /// </summary>
        public readonly Delegate InteropWriteAccess;

        // Constructor
        internal CILFieldInfo(AppDomain domain, FieldInfo field)
        {
            this.Field = field;
            this.DeclaringType = field.DeclaringType.GetTypeInfo(domain);
            this.FieldType = field.FieldType.GetTypeInfo(domain);
            this.Flags = GetFlags(field, out this.InteropReadAccess, out this.InteropWriteAccess);
        }

        // Methods
        public override string ToString()
        {
            return $"{Field} = {Flags}";
        }

        private static CILFieldFlags GetFlags(FieldInfo field, out Delegate interopReadAccess, out Delegate interopWriteAccess)
        {
            interopReadAccess = null;
            interopWriteAccess = null;

            // Init flags
            CILFieldFlags flags = 0;
            {
                bool isClr = field is CLRFieldInfo;

                // Instance field
                if (field.IsStatic == false) flags |= CILFieldFlags.This;

                // Check interop
                if (isClr == false) flags |= CILFieldFlags.Interop;

                // Check for interpreted
                if (isClr == true) flags |= CILFieldFlags.Interpreted;

                // Check for constant
                if ((field.Attributes & FieldAttributes.Literal) != 0) flags |= CILFieldFlags.Constant;
            }

            // Check for read binding
            if(__bindings.TryGetDirectReadBinding(field, out DirectAccess read) == true)
            {
                flags |= CILFieldFlags.DirectReadDelegate;
                interopReadAccess = read;
            }
            // Check for write binding
            else if(__bindings.TryGetDirectWriteBinding(field, out DirectAccess write) == true)
            {
                flags |= CILFieldFlags.DirectWriteDelegate;
                interopWriteAccess = write;
            }

            return flags;
        }
    }
}
