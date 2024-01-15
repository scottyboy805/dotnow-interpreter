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

        // Public
        public static readonly int Size = sizeof(CLRInstance);

        // Properties
        public bool IsStackAllocated
        {
            get { return (type.flags & _CLRTypeFlags.StackAlloc) != 0; }
        }

        public int ReferenceCount
        {
            get { return *(int*)instancePointer; }
            internal set { *(int*)instancePointer = value; }
        }

        public bool IsReferenced
        {
            get { return ReferenceCount > 0; }
        }

        // Methods
        public Type GetRuntimeType(AppDomain domain)
        {
            // Try to lookup type
            return domain.ResolveType(type.typeToken);
        }

        public object Unwrap()
        {
            return __memory.GetManagedObject(*(int*)instancePointer);
        }

        public object UnwrapAs(AppDomain domain, Type asType)
        {
            // Resolve type
            Type thisType = GetRuntimeType(domain);

            // Check for identical type
            if (thisType == asType || TypeExtensions.AreAssignable(asType, thisType.BaseType) == true)
                return Unwrap();

            // Check for base type assignable
            if(asType.IsInterface == true && proxyCount > 0)
            {
                // Get interfaces
                Type[] interfaceTypes = thisType.GetInterfaces();

                // Check for assignable type
                for(int i = 0; i < proxyCount; i++)
                {
                    // Check for assignable type
                    if(TypeExtensions.AreAssignable(asType, interfaceTypes[i]) == true)
                    {
                        // Proxy was matched
                        IntPtr proxyPtr = instancePointer + (i + 1 + 1);

                        // Unwrap managed object base
                        return __memory.GetManagedObject(*(int*)proxyPtr);
                    }
                }
            }
            return null;
        }

        public T UnwrapAs<T>(AppDomain domain)
        {
            try
            {
                // Try to unwrap as T
                return (T)UnwrapAs(domain, typeof(T));
            }
            catch { }

            // Not convertible
            return default;
        }
    }
}
