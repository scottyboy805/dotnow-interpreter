using System;
using System.Reflection;
using TrivialCLR.Reflection;

namespace TrivialCLR.Runtime.CIL
{
    internal sealed class CILMethodInvocation
    {
        // Private
        
        // Internal
        internal MethodBase targetMethod;
        internal AppDomain.MethodDirectCallDelegate directCallDelegate;
        internal CILSignature signature;
        internal VTable vTable;
        internal bool isCLRMethod;
        internal bool isCtor;
        internal bool isStatic;
        internal MethodAttributes attributes = 0;

        internal object[] cachedArgumentList;
        internal StackData[] cachedArgumentListDirectCall;

        // Constructor
        public CILMethodInvocation(MethodBase targetMethod)
        {
            this.targetMethod = targetMethod;
            this.isCLRMethod = targetMethod is CLRMethod || targetMethod is CLRConstructor;
            this.isCtor = targetMethod is ConstructorInfo;
            this.isStatic = targetMethod.IsStatic;
            this.attributes = targetMethod.Attributes;
        }

        // Methods
        public void SetupMethodCall(AppDomain domain)
        {
            // Get argument list
            this.signature = domain.GetMethodSignature(targetMethod);

            // Check for non-ctor method
            if (targetMethod is MethodInfo)
            {
                //MethodInfo current = (targetMethod as MethodInfo).GetBaseDefinition();

                // Get direct call
                this.directCallDelegate = domain.GetDirectCallDelegate(targetMethod);

                // Get vtable
                if (targetMethod.ContainsGenericParameters == false)
                    this.vTable = domain.GetMethodVTableForType(targetMethod.DeclaringType);
            }

            // Alloc arrays
            if (this.directCallDelegate == null)
            {
                this.cachedArgumentList = new object[signature.argumentCount];
            }
            else
            {
                this.cachedArgumentListDirectCall = new StackData[signature.argumentCount];
            }
        }

        public override string ToString()
        {
            return targetMethod.ToString();
        }
    }
}
