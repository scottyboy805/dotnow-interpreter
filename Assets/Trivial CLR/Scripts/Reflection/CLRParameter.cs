using System;
using System.Reflection;
using Mono.Cecil;
using TrivialCLR.Runtime;
using ParameterAttributes = System.Reflection.ParameterAttributes;

namespace TrivialCLR.Reflection
{
    public sealed class CLRParameter : ParameterInfo
    {
        // Private
        private AppDomain domain = null;
        private CLRConstructor ctor = null;
        private CLRMethod method = null;
        private ParameterDefinition parameter = null;
        private Lazy<Type> parameterType = null;
        private Lazy<object> parameterDefault = null;

        // Properties
        public CLRConstructor Constructor
        {
            get { return ctor; }
        }

        public CLRMethod Method
        {
            get { return method; }
        }

        public ParameterDefinition Definition
        {
            get { return parameter; }
        }

        public override Type ParameterType
        {
            get { return parameterType.Value; }
        }

        public override object DefaultValue => base.DefaultValue;

        // Constructor
        internal CLRParameter(AppDomain domain, CLRConstructor ctor, ParameterDefinition parameter)
        {
            this.domain = domain;
            this.ctor = ctor;
            this.parameter = parameter;

            this.AttrsImpl = (ParameterAttributes)parameter.Attributes;
            this.MemberImpl = method;
            this.NameImpl = parameter.Name;
            this.PositionImpl = parameter.Sequence;

            // Lazy types
            this.parameterType = new Lazy<Type>(() => domain.ResolveType(parameter.ParameterType));
            this.parameterDefault = new Lazy<object>(() => parameter.Constant);
        }

        internal CLRParameter(AppDomain domain, CLRMethod method, ParameterDefinition parameter)
        {
            this.domain = domain;
            this.method = method;
            this.parameter = parameter;

            this.AttrsImpl = (ParameterAttributes)parameter.Attributes;
            this.MemberImpl = method;
            this.NameImpl = parameter.Name;
            this.PositionImpl = parameter.Sequence;

            // Lazy types
            this.parameterType = new Lazy<Type>(() => domain.ResolveType(parameter.ParameterType));
            this.parameterDefault = new Lazy<object>(() => parameter.Constant);
        }
    }
}
