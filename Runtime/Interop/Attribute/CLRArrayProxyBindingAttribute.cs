using System;

namespace dotnow.Interop
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class CLRArrayProxyBindingAttribute : Attribute
    {
        // Private
        private readonly Type forArrayType;
        private readonly int rank;

        // Properties
        public Type ForArrayType => forArrayType;
        public int ArrayRank => rank;

        // Constructor
        public CLRArrayProxyBindingAttribute(Type forArrayType, int arrayRank)
        {
            this.forArrayType = forArrayType;
            this.rank = arrayRank;
        }
    }
}