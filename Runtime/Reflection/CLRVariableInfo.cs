using System;
using System.Reflection;

namespace dotnow.Reflection
{
    internal sealed class CLRVariableInfo : LocalVariableInfo
    {
        // Internal
        internal Type type = null;
        internal TypeCode typeCode = 0;

        // Private
        private int index = 0;

        // Properties
        public override Type LocalType => type;
        public override int LocalIndex => index;
        public override bool IsPinned => false;                 // Pinning is not supported

        // Public
        public CLRVariableInfo(Type type, int index)
        {
            this.type = type;
            this.typeCode = Type.GetTypeCode(type);
            this.index = index;
        }
    }
}
