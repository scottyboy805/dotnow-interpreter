using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using Mono.Cecil;
using TrivialCLR.Runtime;
using TrivialCLR.Runtime.CIL;
using MethodAttributes = System.Reflection.MethodAttributes;
using MethodImplAttributes = System.Reflection.MethodImplAttributes;

namespace TrivialCLR.Reflection
{
    public sealed class CLRMethod : MethodInfo
    {
        // Private
        private readonly AppDomain domain = null;
        private CLRType declaringType = null;
        private MethodDefinition method = null;
        private CLRMethodBody body = null;
        private CLRParameter[] parameters = null;
        private Lazy<CILSignature> signature = null;
        private Lazy<CLRTypeInfo> returnType = null;
        private Lazy<CLRAttributeBuilder> attributeProvider = null;

        // Properties
        public CLRMethodBody Body
        {
            get { return body; }
        }

        public MethodDefinition Definition
        {
            get { return method; }
        }

        public override string Name
        {
            get { return method.Name; }
        }

        public override Type DeclaringType
        {
            get { return declaringType; }
        }

        public override RuntimeMethodHandle MethodHandle
        {
            get { throw new NotSupportedException("A RuntimeMethod has no obtainable method hamdle"); }
        }

        public override MethodAttributes Attributes
        {
            get { return (MethodAttributes)method.Attributes; }
        }
        
        public override MemberTypes MemberType
        {
            get { return MemberTypes.Method; }
        }

        public override Type ReflectedType
        {
            get { return declaringType; }
        }

        public override Type ReturnType
        {
            get { return returnType.Value.type; }
        }

        public override bool ContainsGenericParameters
        {
            get { return method.ContainsGenericParameter; }
        }

        public override System.Reflection.ICustomAttributeProvider ReturnTypeCustomAttributes
        {
            get { throw new NotImplementedException(); }
        }

        // Constructor
        internal CLRMethod(AppDomain domain, CLRType declaringType, MethodDefinition method)           
        {
            this.domain = domain;
            this.declaringType = declaringType;
            this.method = method;

            // Create parameters
            parameters = new CLRParameter[method.Parameters.Count];

            for (int i = 0; i < parameters.Length; i++)
            {
                parameters[i] = new CLRParameter(domain, this, method.Parameters[i]);
            }

            if (method.HasBody == true)
                this.body = new CLRMethodBody(domain, this, method.Body);

            // Lazy types
            signature = new Lazy<CILSignature>(InitSignature);
            returnType = new Lazy<CLRTypeInfo>(InitReturnType);
            attributeProvider = new Lazy<CLRAttributeBuilder>(InitAttributeProvider);
        }

        // Methods
        public override MethodInfo GetBaseDefinition()
        {
            // Check for interface
            if (declaringType.IsInterface == true)
                return this;

            if (method.HasOverrides == true)
                return declaringType.BaseType.GetMethod(method.Name);

            return null;
        }

        public override object[] GetCustomAttributes(bool inherit)
        {
            // Check for inherit
            if(inherit == true)
            {
                List<object> attributes = new List<object>();

                // Build type arg list
                Type[] argTypes = new Type[parameters.Length];

                for (int i = 0; i < argTypes.Length; i++)
                    argTypes[i] = parameters[i].ParameterType;

                // Add attribute for this method
                attributes.AddRange(attributeProvider.Value.GetAttributeInstances());

                // Try to find base method
                MethodInfo baseMethod = declaringType.BaseType.GetMethod(Name, argTypes);

                // Get attributes for base method
                if (baseMethod != null)
                    attributes.AddRange(baseMethod.GetCustomAttributes(inherit));

                return attributes.ToArray();
            }

            // Simple case
            return attributeProvider.Value.GetAttributeInstances();
        }

        public override object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            // Check for inherit
            if (inherit == true)
            {
                List<object> attributes = new List<object>();

                // Build type arg list
                Type[] argTypes = new Type[parameters.Length];

                for (int i = 0; i < argTypes.Length; i++)
                    argTypes[i] = parameters[i].ParameterType;

                // Add attribute for this method
                attributes.AddRange(attributeProvider.Value.GetAttributeInstances());

                // Try to find base method
                MethodInfo baseMethod = declaringType.BaseType.GetMethod(Name, argTypes);

                // Get attributes for base method
                if (baseMethod != null)
                    attributes.AddRange(baseMethod.GetCustomAttributes(attributeType, inherit));

                return attributes.ToArray();
            }

            // Simple case
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

        public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
        {
            // Check if method can be invoked
            if (body == null)
                throw new TargetInvocationException("Cannot invoke a method which does not define a method body: " + method, new Exception("No executable method body found"));

            // Check for instance
            if (obj == null && IsStatic == false)
                throw new TargetInvocationException("An instance must be provided in order to invoke a non static method", new Exception("No instance provided"));

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

            // Check parameter types
            if(paramCount > 0)
            {
                for(int i = 0; i < paramCount; i++)
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
                //frame.stack[frame.stackIndex++] = StackObject.AllocTyped(this.parameters[i].ParameterTypeCode, parameters[i]);

            // Execute method body
            body.ExecuteMethodBody(engine, frame);

            // Get return object
            if (ReturnType != typeof(void))
            {
                StackData result = frame.stack[--frame.stackIndex];

                return result.UnboxAsType(returnType.Value);
            }

            return null;
        }

        public override bool IsDefined(Type attributeType, bool inherit)
        {
            if(inherit == true)
            {
                // Check for defined on this method
                if (attributeProvider.Value.IsDefined(attributeType) == true)
                    return true;

                // Check for defined on base method
                Type[] argTypes = new Type[parameters.Length];

                for (int i = 0; i < argTypes.Length; i++)
                    argTypes[i] = parameters[i].ParameterType;

                // Try to find base method
                MethodInfo baseMethod = declaringType.BaseType.GetMethod(Name, argTypes);

                // Get attributes for base method
                if (baseMethod != null && baseMethod.IsDefined(attributeType, inherit) == true)
                    return true;

                return false;
            }

            // Simple case
            return attributeProvider.Value.IsDefined(attributeType);
        }

        public override string ToString()
        {
            return method.ToString();
        }

        private CLRTypeInfo InitReturnType()
        {
            if(method.ContainsGenericParameter == true)
            {
                GenericParameter parameter = method.ReturnType as GenericParameter;

                if(parameter != null)
                {
                    return new CLRTypeInfo(declaringType.GenericTypeArguments[parameter.Position]);
                }
            }

            return new CLRTypeInfo(domain.ResolveType(method.ReturnType));
        }

        private CILSignature InitSignature()
        {
            return domain.GetMethodSignature(this);
        }

        private CLRAttributeBuilder InitAttributeProvider()
        {
            return new CLRAttributeBuilder(domain, method.CustomAttributes);
        }
    }
}
