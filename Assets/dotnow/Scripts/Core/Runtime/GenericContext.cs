using System;

namespace dotnow.Runtime
{
    internal struct GenericContext : IDisposable
    {
        // Private
        private static Type typeContext = null;

        // Properties
        public static Type TypeContext
        {
            get { return typeContext; }
        }

        // Constructor
        public GenericContext(Type type)
        {
            typeContext = type;
        }

        // Methods
        public void Dispose()
        {
            typeContext = null;
        }
    }
}
