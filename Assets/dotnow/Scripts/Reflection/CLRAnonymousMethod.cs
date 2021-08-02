using System;
using System.Globalization;
using System.Reflection;
using dotnow.Runtime;
using dotnow.Runtime.CIL;
using dotnow.Runtime.JIT;

namespace dotnow.Reflection
{
    internal class CLRAnonymousMethod : MethodInfo, IJITOptimizable
    {
        // Private
        private readonly AppDomain domain = null;
        private readonly CLRAnonymousMethodBody body = null;
        private ExecutionMethod executableMethod = null;
        private CLRTypeInfo returnType = default;
        private CLRAnonymousParameter[] parameters = null;
        private Lazy<CILSignature> signature = null;

        // Properties
        public override ICustomAttributeProvider ReturnTypeCustomAttributes
        {
            get { return null; }
        }

        public override MethodAttributes Attributes
        {
            get { return MethodAttributes.Static; }
        }

        public override RuntimeMethodHandle MethodHandle
        {
            get { return default(RuntimeMethodHandle); }
        }

        public override Type DeclaringType
        {
            get { return null; }
        }

        public override string Name
        {
            get { return "Anonymous"; }
        }

        public override Type ReflectedType
        {
            get { return typeof(CLRAnonymousMethod); }
        }

        public override Type ReturnType
        {
            get { return returnType.type; }
        }

        // Constructor
        internal CLRAnonymousMethod(AppDomain domain, CILOperation[] instructions, bool initLocals, int maxStack, Type[] localTypes, Type returnType, Type[] parameterTypes)
        {
            this.domain = domain;
            this.body = new CLRAnonymousMethodBody(domain, this, instructions, initLocals, maxStack, localTypes);
            this.returnType = new CLRTypeInfo(returnType);
            this.parameters = new CLRAnonymousParameter[parameterTypes.Length];

            for (int i = 0; i < parameterTypes.Length; i++)
                this.parameters[i] = new CLRAnonymousParameter(parameterTypes[i], i);

            // Lazy types
            this.signature = new Lazy<CILSignature>(() => domain.GetMethodSignature(this));
        }

        // Methods
        void IJITOptimizable.EnsureJITOptimized()
        {
            JITOptimize.EnsureJITOptimized(body);

            // Initialize executable method
            if (executableMethod == null)
                executableMethod = new ExecutionMethod(domain, signature.Value, this, body, true, false);
        }

        public override MethodInfo GetBaseDefinition()
        {
            throw new NotImplementedException();
        }

        public override object[] GetCustomAttributes(bool inherit)
        {
            throw new NotImplementedException();
        }

        public override object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            throw new NotImplementedException();
        }

        public override MethodImplAttributes GetMethodImplementationFlags()
        {
            throw new NotImplementedException();
        }

        public override ParameterInfo[] GetParameters()
        {
            return (ParameterInfo[])parameters.Clone();
        }

        public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
        {
            // Initialize executable method
            if (executableMethod == null)
                executableMethod = new ExecutionMethod(domain, signature.Value, this, body, IsStatic, false);

            // Invoke the method
            return executableMethod.ReflectionInvoke(obj, parameters);
        }

        public override bool IsDefined(Type attributeType, bool inherit)
        {
            throw new NotImplementedException();
        }
    }
}
