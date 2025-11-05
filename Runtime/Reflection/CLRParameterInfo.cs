using System;
using System.Reflection;

namespace dotnow.Reflection
{
    internal sealed class CLRParameterInfo : ParameterInfo
    {
        // Private
        private object defaultValue = null;

        // Properties
        #region ParameterInfoProperties
        public override bool HasDefaultValue => defaultValue != null;
        public override object DefaultValue => defaultValue;
        #endregion

        // Constructor
        internal CLRParameterInfo(MethodBase method, Type parameterType)
        {
            base.MemberImpl = method;
            base.ClassImpl = parameterType;
        }
    }
}
