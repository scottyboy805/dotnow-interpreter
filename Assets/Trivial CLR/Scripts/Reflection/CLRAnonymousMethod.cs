using System;
using System.Globalization;
using System.Reflection;
using TrivialCLR.Runtime;
using TrivialCLR.Runtime.CIL;

namespace TrivialCLR.Reflection
{
    internal class CLRAnonymousMethod : MethodInfo
    {
        // Private
        private AppDomain domain = null;
        private CLRAnonymousMethodBody body = null;
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
            get { return 0; }
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
            // Get the exeuction engine
            ExecutionEngine engine = domain.GetExecutionEngine();

            StackLocal[] locals = null;

            // Get locals
            if (body.InitLocals == true)
                locals = body.Locals;

            int instanceCount = (obj == null) ? 0 : 1;
            int paramCount = (parameters == null) ? 0 : parameters.Length;

            // Check parameter types
            if (paramCount > 0)
            {
                for (int i = 0; i < paramCount; i++)
                {
                    // Check if patameters should be passed by reference
                    if (this.parameters[i].ParameterType.IsByRef == true && i < parameters.Length && (parameters[i] is IByRef) == false)
                    {
                        throw new TargetInvocationException(string.Format("The argument at index '{0}' of type '{1}' must be passed by reference. You can use the ByRef type to pass external variables by reference", i, this.parameters[i].ParameterType),
                            new ArgumentException("Expected reference type for method invocation"));
                    }
                }
            }

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
            
            // Execute method body
            body.ExecuteMethodBody(engine, frame);

            // Get return object
            if (ReturnType != typeof(void))
            {
                StackData result = frame.stack[--frame.stackIndex];

                return result.UnboxAsType(returnType);
            }

            return null;
        }

        public override bool IsDefined(Type attributeType, bool inherit)
        {
            throw new NotImplementedException();
        }
    }
}
