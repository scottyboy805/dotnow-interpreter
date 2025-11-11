using dotnow.Interop;
using dotnow.Reflection;
using dotnow.Runtime.CIL;
using System;

namespace dotnow.Runtime
{
    internal readonly struct CLRValueTypeInstance : ICLRInstance
    {
        // Private
        private readonly Memory<StackData> fields;

        // Public
        public readonly CLRType Type;

        // Properties
        public Span<StackData> Fields => fields.Span;

        // Constructor
        private CLRValueTypeInstance(CILTypeInfo typeInfo, Memory<StackData> fields)
        {
            // Check for CLR
            if ((typeInfo.Flags & CILTypeFlags.Interpreted) == 0)
                throw new ArgumentException("Only supported for interpreted value types");

            this.Type = (CLRType)typeInfo.Type;
            this.fields = fields;
        }

        // Methods
        public bool Equals(ICLRInstance otherInstance)
        {
            return ReferenceEquals(this, otherInstance);
        }

        public Type GetInterpretedType()
        {
            return Type;
        }

        public object Unwrap()
        {
            throw new NotSupportedException("Not supported for value types");
        }

        public object UnwrapAsType(Type asType)
        {
            throw new NotSupportedException("Not supported for value types");
        }

        public CLRValueTypeInstance Copy(CILTypeInfo typeInfo)
        {
            // Copy the array
            StackData[] copyFields = fields.ToArray();

            // Create instance
            return new CLRValueTypeInstance(typeInfo, new Memory<StackData>(copyFields));
        }

        internal static CLRValueTypeInstance CreateInstance(CILTypeInfo typeInfo)
        {
            // Create dedicated memory - used for return to interop called where the stack would not survive
            StackData[] dedicatedFields = new StackData[typeInfo.InstanceSize];

            // Create the instance
            return new CLRValueTypeInstance(typeInfo, new Memory<StackData>(dedicatedFields));
        }

        internal static CLRValueTypeInstance CreateInstance(CILTypeInfo typeInfo, StackData[] stack, int sp)
        {
            // Create the instance
            return new CLRValueTypeInstance(typeInfo, new Memory<StackData>(stack, sp, typeInfo.InstanceSize));
        }
    }
}
