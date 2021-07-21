using System;
using System.Globalization;
using System.Reflection;
using Mono.Cecil;
using TrivialCLR.Runtime;
using TrivialCLR.Runtime.CIL;
using MethodAttributes = System.Reflection.MethodAttributes;
using MethodImplAttributes = System.Reflection.MethodImplAttributes;

namespace TrivialCLR.Reflection
{
    public sealed class CLRConstructor : ConstructorInfo
    {
        // Private
        private AppDomain domain = null;
        private CLRType declaringType = null;
        private MethodDefinition method = null;
        private CLRMethodBody body = null;
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

            // Get the exeuction engine
            ExecutionEngine engine = domain.GetExecutionEngine();

            StackLocal[] locals = null;

            // Get locals
            if (body.InitLocals == true)
                locals = body.Locals;

            int instanceCount = (obj == null) ? 0 : 1;
            int paramCount = (parameters == null) ? 0 : parameters.Length;

            // Create the method frame
            ExecutionFrame frame;//= new ExecutionFrame(engine, this, body.MaxStack + paramCount + instanceCount, paramCount, locals);
            frame = new ExecutionFrame(domain, engine, null, this, body.MaxStack, paramCount, locals);

            // Push instance
            if (instanceCount > 0)
            {
                frame.stack[frame.stackIndex].refValue = obj;
                frame.stack[frame.stackIndex++].type = StackData.ObjectType.Ref;
            }

            // Push parameters
            for (int i = 0; i < paramCount; i++)
                StackData.AllocTyped(ref frame.stack[frame.stackIndex++], signature.Value.parameterTypeInfos[i], parameters[i]);
                //frame.stack[frame.stackIndex++] = StackObject.AllocTyped(this.parameters[i].ParameterTypeCode, parameters[i]);

            // Execute method body
            body.ExecuteMethodBody(engine, frame);

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
