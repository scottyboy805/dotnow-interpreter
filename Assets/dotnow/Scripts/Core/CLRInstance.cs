using dotnow.Runtime;
using dotnow.Runtime.Handle;
using System;

namespace dotnow
{
    public unsafe struct CLRInstance
    {
        // Internal
        internal _CLRTypeHandle type;
        internal IntPtr instancePointer;
        internal int proxyCount;
        internal int referenceCount;

        // Public
        public static readonly int Size = sizeof(CLRInstance);

        // Properties
        public bool IsStackAllocated
        {
            get { return (type.flags & _CLRTypeFlags.ValueType) != 0; }
        }

        // Methods
        //public Type GetRuntimeType()
        //{
        //    return type.get
        //}

        public object Unwrap()
        {
            return __memory.GetManagedObject(*(int*)instancePointer);
        }
    }
}
