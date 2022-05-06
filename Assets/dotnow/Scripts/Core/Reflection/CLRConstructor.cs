using System;
using System.Globalization;
using System.Reflection;
using Mono.Cecil;
using dotnow.Runtime;
using dotnow.Runtime.CIL;
using dotnow.Runtime.JIT;
using MethodAttributes = System.Reflection.MethodAttributes;
using MethodImplAttributes = System.Reflection.MethodImplAttributes;

namespace dotnow.Reflection
{
    public sealed class CLRConstructor : ConstructorInfo, IJITOptimizable
    {
        // Private
        private readonly AppDomain domain = null;
        private readonly CLRType declaringType = null;
        private readonly MethodDefinition method = null;
        private readonly CLRMethodBodyBase body = null;
        private ExecutionMethod executableCtor = null;
        private CLRParameter[] parameters = null;
        private Lazy<CILSignature> signature = null;
        private Lazy<CLRAttributeBuilder> attributeProvider = null;

        // Properties
        public MethodDefinition Definition
        {
            get { return method; }
        }

        public override RuntimeMethodHandle MethodHandle
        {
            get { throw new NotSupportedException("A RuntimeConstructor has no obtainable method handle"); }
        }

        public override MethodAttributes Attributes
        {
            get { return (MethodAttributes)method.Attributes; }
        }

        public override Type DeclaringType
        {
            get { return declaringType; }
        }

        public override string Name
        {
            get { return method.Name; }
        }

        public override Type ReflectedType
        {
            get { return declaringType; }
        }

        // Constructor
        internal CLRConstructor(AppDomain domain, CLRType declaringType, MethodDefinition method)
        {
            this.domain = domain;
            this.declaringType = declaringType;
            this.method = method;

            // Create parameters
            parameters = new CLRParameter[method.Parameters.Count];

            for(int i = 0; i < parameters.Length; i++)
            {
                parameters[i] = new CLRParameter(domain, this, method.Parameters[i]);
            }

            if (method.HasBody == true)
                this.body = new CLRMethodBody(this.domain, this, method.Body);

            // Lazy types
            signature = new Lazy<CILSignature>(() => domain.GetMethodSignature(this));
            attributeProvider = new Lazy<CLRAttributeBuilder>(() => new CLRAttributeBuilder(domain, method.CustomAttributes));
        }

        // Methods
        void IJITOptimizable.EnsureJITOptimized()
        {
            JITOptimize.EnsureJITOptimized(body);

            // Initialize executable method
            if (executableCtor == null)
                executableCtor = new ExecutionMethod(domain, signature.Value, this, body, false, true);
        }

        public override object[] GetCustomAttributes(bool inherit)
        {
            return attributeProvider.Value.GetAttributeInstances();
        }

        public override object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            return attributeProvider.Value.GetAttributeInstancesOfType(attributeType);
        }

        public override MethodImplAttributes GetMethodImplementationFlags()
        {
            return (MethodImplAttributes)method.ImplAttributes;
        }

        public override ParameterInfo[] GetParameters()
        {
            return parameters;
        }

        public override object Invoke(BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
        {
            //throw new NotImplementedException("Static constructors are not yet supported");
            return Invoke(null, invokeAttr, binder, parameters, culture);
        }

        public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
        {
            // Check if method can be invoked
            if (body == null)
                throw new TargetInvocationException("Cannot invoke a method which does not define a method body: " + method, new Exception("No executable method body found"));

            // Check for instance
            if (obj == null && IsStatic == false)
                throw new TargetInvocationException("An instance must be provided in order to invoke a non static object initializer", new Exception("No instance provided"));

            if (obj != null && IsStatic == true)
                obj = null;

            // Make sure type is initialized
            declaringType.StaticInitializeType();


            // Initialize executable method
            if (executableCtor == null)
                executableCtor = new ExecutionMethod(domain, signature.Value, this, body, false, true);

            // Invoke the method
            executableCtor.ReflectionInvoke(obj, parameters);

            return obj;
        }

        public override bool IsDefined(Type attributeType, bool inherit)
        {
            return attributeProvider.Value.IsDefined(attributeType);
        }

        public override string ToString()
        {
            return method.ToString();
        }
    }
}
