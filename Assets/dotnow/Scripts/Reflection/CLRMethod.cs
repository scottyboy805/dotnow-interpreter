using System;
using System.Collections.Generic;
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
    public sealed class CLRMethod : MethodInfo, IJITOptimizable
    {
        // Private
        private readonly AppDomain domain = null;
        private readonly CLRType declaringType = null;
        private readonly MethodDefinition method = null;
        private readonly CLRMethodBodyBase body = null;
        private ExecutionMethod executableMethod = null;
        private CLRParameter[] parameters = null;
        private Lazy<CILSignature> signature = null;
        private Lazy<CLRTypeInfo> returnType = null;
        private Lazy<CLRAttributeBuilder> attributeProvider = null;

        // Properties
        public CLRMethodBodyBase Body
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
        void IJITOptimizable.EnsureJITOptimized()
        {
            JITOptimize.EnsureJITOptimized(body);

            // Initialize executable method
            if (executableMethod == null)
                executableMethod = new ExecutionMethod(domain, signature.Value, this, body, IsStatic, false);
        }

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

            // Make sure type is initialized (Run static initializers etc)
            declaringType.StaticInitializeType();


            // Initialize executable method
            if (executableMethod == null)
                executableMethod = new ExecutionMethod(domain, signature.Value, this, body, IsStatic, false);

            // Invoke the method
            return executableMethod.ReflectionInvoke(obj, parameters);
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

        public override Delegate CreateDelegate(Type delegateType)
        {
            // Check for action
            if (delegateType != typeof(Action))
                throw new NotSupportedException("Can only support delegates of type 'System.Action' (Non-Generic)");

            // Check for instance method
            if (IsStatic == false)
                throw new InvalidOperationException("Cannot create a static delegate for an instance method. Use 'CreateDelegate(Type, object)'");

            // Create the action
            Action methodDelegate = () =>
            {
                // Initialize executable method
                if (executableMethod == null)
                    executableMethod = new ExecutionMethod(domain, signature.Value, this, body, IsStatic, false);

                // Invoke static method
                executableMethod.DelegateInvoke(null);
            };

            return methodDelegate;
        }

        public override Delegate CreateDelegate(Type delegateType, object target)
        {
            // Check for action
            if (delegateType != typeof(Action))
                throw new NotSupportedException("Can only support delegates of type 'System.Action' (Non-Generic)");

            // Check for instance method
            if (IsStatic == true)
                throw new InvalidOperationException("Cannot create an instance delegate for a static method. Use 'CreateDelegate(Type)'");

            // Create the action
            Action methodDelegate = () =>
            {
                // Initialize executable method
                if (executableMethod == null)
                    executableMethod = new ExecutionMethod(domain, signature.Value, this, body, IsStatic, false);

                // Invoke method
                executableMethod.DelegateInvoke(target);
            };

            return methodDelegate;
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
                    return CLRTypeInfo.GetTypeInfo(declaringType.GenericTypeArguments[parameter.Position]);
                }
            }

            return CLRTypeInfo.GetTypeInfo(domain.ResolveType(method.ReturnType));
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
