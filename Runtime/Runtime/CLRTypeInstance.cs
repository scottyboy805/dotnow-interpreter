using dotnow.Interop;
using dotnow.Reflection;
using System;

namespace dotnow.Runtime
{
    internal sealed class CLRTypeInstance : ICLRInstance
    {
        // Public
        public readonly CLRType Type;
        public readonly object[] Interop;
        public readonly StackData[] Fields;

        // Constructor
        private CLRTypeInstance(CILTypeInfo typeInfo)
        {
            // Check for CLR
            if ((typeInfo.Flags & CILTypeFlags.Interpreted) == 0)
                throw new ArgumentException("Only supported for interpreted types");

            this.Type = (CLRType)typeInfo.Meta;
            this.Interop = new object[typeInfo.InteropTypes.Length];
            this.Fields = new StackData[typeInfo.InstanceSize];
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
            return Interop[0];
        }

        public object UnwrapAsType(Type asType)
        {
            throw new NotImplementedException();
        }

        // Methods
        internal static CLRTypeInstance CreateInstance(CILTypeInfo typeInfo)
        {

        }
    }
}
