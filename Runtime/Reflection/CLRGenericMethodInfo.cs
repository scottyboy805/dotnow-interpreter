using System;
using System.Globalization;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;

namespace dotnow.Reflection
{
    internal sealed class CLRGenericMethodInfo : MethodInfo
    {
        // Private
        private readonly MethodInfo genericDefinition;
        private readonly Type[] genericArguments;

        private readonly Lazy<string> toString = null;
        private readonly Lazy<CLRParameterInfo[]> parameters = null;
        private readonly Lazy<CLRParameterInfo> returnParameter = null;

        // Properties
        public override bool IsGenericMethod => true;
        public override bool IsConstructedGenericMethod => true;
        public override bool IsGenericMethodDefinition => false;
        public override bool ContainsGenericParameters => false;

        #region MethodInfoProperties
        public override string Name => genericDefinition.Name;
        public override Type DeclaringType => genericDefinition.DeclaringType;
        public override Type ReturnType => returnParameter.Value.ParameterType;
        public override ParameterInfo ReturnParameter => returnParameter.Value;
        public override MethodAttributes Attributes => genericDefinition.Attributes;
        public override ICustomAttributeProvider ReturnTypeCustomAttributes => throw new NotImplementedException();
        public override Type ReflectedType => typeof(CLRGenericMethodInfo);
        public override RuntimeMethodHandle MethodHandle => throw new NotSupportedException();
        #endregion

        // Constructor
        internal CLRGenericMethodInfo(MethodInfo genericDefinition,  Type[] genericArguments)
        {
            this.genericDefinition = genericDefinition;
            this.genericArguments = genericArguments;

            this.toString = new(InitToString);
            this.parameters = new(InitParameters);
            this.returnParameter = new(InitReturnParameter);
        }

        // Methods
        public override string ToString()
        {
            return toString.Value;
        }

        #region MethodInfoMethods
        public override MethodBody GetMethodBody()
        {
            // No body available
            return null;
        }

        public override MethodInfo GetBaseDefinition()
        {
            return genericDefinition;
        }

        public override MethodInfo GetGenericMethodDefinition()
        {
            return genericDefinition;
        }

        public override Type[] GetGenericArguments()
        {
            return genericArguments;
        }

        public override bool IsDefined(Type attributeType, bool inherit)
        {
            return genericDefinition.IsDefined(attributeType, inherit);
        }

        public override object[] GetCustomAttributes(bool inherit)
        {
            return genericDefinition.GetCustomAttributes(inherit);
        }

        public override object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            return genericDefinition.GetCustomAttributes(attributeType, inherit);
        }

        public override MethodImplAttributes GetMethodImplementationFlags()
        {
            throw new NotImplementedException();
        }

        public override ParameterInfo[] GetParameters()
        {
            return parameters.Value;
        }

        public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
        #endregion

        #region LazyInit
        private string InitToString()
        {
            StringBuilder builder = new StringBuilder();

            // Return type
            builder.Append(ReturnType);
            builder.Append(' ');

            // Type name
            builder.Append(DeclaringType);
            builder.Append('.');

            // Method name
            builder.Append(Name);

            // Method arguments
            builder.Append('(');
            {
                // Add parameters
                for (int i = 0; i < parameters.Value.Length; i++)
                {
                    // Append parameter type
                    builder.Append(parameters.Value[i].ParameterType);

                    // Check for more parameters to come
                    if (i < parameters.Value.Length - 1)
                        builder.Append(", ");
                }
            }
            builder.Append(')');

            return builder.ToString();
        }

        private CLRParameterInfo[] InitParameters()
        {
            Type[] declaringTypeGenericsArguments = null;

            // Check for generic type
            if (DeclaringType.IsConstructedGenericType == true)
                declaringTypeGenericsArguments = DeclaringType.GetGenericArguments();

            // Get parameters from base type
            ParameterInfo[] baseParameters = genericDefinition.GetParameters();

            // Init parameters
            CLRParameterInfo[] parameters = new CLRParameterInfo[baseParameters.Length];

            // Setup parameters
            for (int i = 0; i < parameters.Length; i++)
            {
                // Select the parameter type
                Type parameterType = baseParameters[i].ParameterType;

                // Check generic parameter
                if (parameterType.IsGenericParameter == true)
                {
                    // Check for type `!0`
                    if(parameterType.DeclaringMethod == null)
                    {
                        // Select the parameter from the type
                        parameterType = declaringTypeGenericsArguments[parameterType.GenericParameterPosition];
                    }
                    // Must be method `!!0`
                    else
                    {
                        // Select the parameter from the method arguments
                        parameterType = genericArguments[parameterType.GenericParameterPosition];
                    }
                }

                // Create parameter
                parameters[i] = new CLRParameterInfo(this,
                    baseParameters[i].Name,
                    baseParameters[i].Position,
                    baseParameters[i].Attributes,
                    parameterType);
            }

            // Get parameters
            return parameters;
        }

        private CLRParameterInfo InitReturnParameter()
        {
            // Get return type
            Type returnType = genericDefinition.ReturnType;

            // Check for generic parameter
            if(returnType.IsGenericParameter == true)
            {
                // Check for type `!0`
                if(returnType.DeclaringMethod == null)
                {
                    Type[] declaringTypeGenericsArguments = null;

                    // Check for generic type
                    if (DeclaringType.IsConstructedGenericType == true)
                        declaringTypeGenericsArguments = DeclaringType.GetGenericArguments();

                    // Select the parameter from the type
                    returnType = declaringTypeGenericsArguments[returnType.GenericParameterPosition];
                }
                // Must be method `!!0`
                else
                {
                    // Select the parameter from the method arguments
                    returnType = genericArguments[returnType.GenericParameterPosition];
                }
            }

            // Create return param
            return new CLRParameterInfo(this, "Return", 0, 0, returnType);
        }
        #endregion
    }
}
