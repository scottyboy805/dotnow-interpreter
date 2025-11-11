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
        public readonly object InteropBase;

        // Properties
        public Span<StackData> Fields => fields.Span;

        // Constructor
        private CLRValueTypeInstance(AppDomain domain, CILTypeInfo typeInfo, Memory<StackData> fields)
        {
            // Check for CLR
            if ((typeInfo.Flags & CILTypeFlags.Interpreted) == 0)
                throw new ArgumentException("Only supported for interpreted value types");

            // Create proxy direct for value types, since it is not possible to derive from any other type
            ICLRProxy proxy = new Interop.CoreLib.Proxy.System_Object_Proxy();

            this.Type = (CLRType)typeInfo.Type;
            this.InteropBase = proxy;
            this.fields = fields;

            // Initialize proxy
            proxy.Initialize(domain, typeInfo.Type, this);
        }

        private CLRValueTypeInstance(CLRType type, object interopBase, Memory<StackData> fields)
        {
            if(interopBase == null)
                throw new ArgumentNullException(nameof(interopBase));

            this.Type = type;
            this.InteropBase = interopBase;
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
            return InteropBase;
        }

        public object UnwrapAsType(Type asType)
        {
            // Check for identical type
            if (Type == asType || asType == typeof(object))
                return InteropBase;

            throw new NotSupportedException("Cannot get value type instance as any type other than System.Object");
        }

        public CLRValueTypeInstance Copy(CILTypeInfo typeInfo)
        {
            // Copy the interop object
            object interopBase = __marshal.CopyInteropBoxedValueTypeSlow(InteropBase);

            // Copy the array
            StackData[] copyFields = fields.ToArray();

            // Create instance
            return new CLRValueTypeInstance(Type, interopBase, copyFields);
        }

        internal static CLRValueTypeInstance CreateInstance(AppDomain domain, CILTypeInfo typeInfo)
        {
            // Create dedicated memory - used for return to interop called where the stack would not survive
            StackData[] dedicatedFields = new StackData[typeInfo.InstanceSize];

            // Create the instance
            return new CLRValueTypeInstance(domain, typeInfo, new Memory<StackData>(dedicatedFields));
        }

        internal static CLRValueTypeInstance CreateInstance(AppDomain domain, CILTypeInfo typeInfo, StackData[] stack, int sp)
        {
            // Create the instance
            return new CLRValueTypeInstance(domain, typeInfo, new Memory<StackData>(stack, sp, typeInfo.InstanceSize));
        }
    }
}
