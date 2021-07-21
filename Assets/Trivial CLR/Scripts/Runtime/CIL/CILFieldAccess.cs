using System;
using System.Reflection;

namespace TrivialCLR.Runtime.CIL
{
    internal sealed class CILFieldAccess
    {
        // Internal
        internal FieldInfo targetField;
        internal AppDomain.FieldDirectAccessDelegate directReadAccessDelegate;
        internal AppDomain.FieldDirectAccessDelegate directWriteAccessDelegate;
        internal CLRTypeInfo fieldTypeInfo;

        // Constructor
        public CILFieldAccess(FieldInfo targetField)
        {
            this.targetField = targetField;
        }

        // Methods
        public void SetupFieldAccess(AppDomain domain)
        {
            // Get type
            this.fieldTypeInfo = new CLRTypeInfo(targetField.FieldType);

            // Get direct access delegate
            this.directReadAccessDelegate = domain.GetDirectAccessDelegate(targetField, CLRFieldAccessMode.Read);
            this.directWriteAccessDelegate = domain.GetDirectAccessDelegate(targetField, CLRFieldAccessMode.Write);
        }

        public override string ToString()
        {
            return targetField.ToString();
        }
    }
}
