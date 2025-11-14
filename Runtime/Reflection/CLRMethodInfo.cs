using dotnow.Interop;
using dotnow.Runtime;
using dotnow.Runtime.CIL;
using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace dotnow.Reflection
{
    internal sealed class CLRMethodInfo : MethodInfo
    {
        // Internal
        internal readonly MetadataReferenceProvider metadataProvider = null;
        internal readonly MethodDefinitionHandle handle = default;
        internal readonly MethodDefinition definition = default;

        // Private
        private readonly CLRType declaringType = null;
        private readonly CLRMethodBody body = null;

        private readonly Lazy<int> token = null;
        private readonly Lazy<string> name = null;
        private readonly Lazy<string> toString = null;
        private readonly Lazy<MethodSignature<Type>> signature = null;
        private readonly Lazy<CLRParameterInfo[]> parameters = null;
        private readonly Lazy<CLRParameterInfo> returnParameter = null;
        private readonly Lazy<MetadataDebugInformation> debugInformation = null;

        // Properties
        internal AppDomain AppDomain => metadataProvider.AppDomain;
        internal AssemblyLoadContext AssemblyLoadContext => metadataProvider.AssemblyLoadContext;

        #region MethodInfoProperties
        public override int MetadataToken => token.Value;
        public override string Name => name.Value;
        public override Type DeclaringType => declaringType;
        public override Type ReturnType => returnParameter.Value.ParameterType;
        public override ParameterInfo ReturnParameter => returnParameter.Value;
        public override MethodAttributes Attributes => definition.Attributes;
        public override ICustomAttributeProvider ReturnTypeCustomAttributes => throw new NotImplementedException();
        public override Type ReflectedType => declaringType;
        public override RuntimeMethodHandle MethodHandle => throw new NotSupportedException();
        #endregion

        public MetadataDebugInformation DebugInformation => debugInformation.Value;

        // Constructor
        internal CLRMethodInfo(MetadataReferenceProvider metadataProvider, CLRType declaringType, in MethodDefinitionHandle handle, in MethodDefinition definition)
        {
            this.metadataProvider = metadataProvider;
            this.declaringType = declaringType;
            this.handle = handle;
            this.definition = definition;

            this.token = new(InitToken);
            this.name = new(InitName);
            this.toString = new(InitToString);
            this.signature = new(InitSignature);
            this.parameters = new(InitParameters);
            this.returnParameter = new(InitReturnParameter);
            this.debugInformation = new(InitDebugInformation);

            // Check for body - virtual address must be valid or no body is present
            if (definition.RelativeVirtualAddress != 0)
            {
                // Get the method body block
                MethodBodyBlock bodyBlock = metadataProvider.PEReader.GetMethodBody(definition.RelativeVirtualAddress);

                // Create body
                this.body = new CLRMethodBody(metadataProvider, this, bodyBlock);
            }
        }

        // Methods
        public override string ToString()
        {
            return toString.Value;
        }

        #region MethodInfoMethods
        public override MethodBody GetMethodBody()
        {
            return body;
        }

        public override MethodInfo GetBaseDefinition()
        {
            throw new NotImplementedException();
        }

        public override bool IsDefined(Type attributeType, bool inherit)
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
            return parameters.Value;
        }

        public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
        {
            // Ensure that the method is resolved
            metadataProvider.AssemblyLoadContext.ResolveMethod(handle);


            // Get execution context
            ThreadContext threadContext = metadataProvider.AppDomain.GetThreadContext();

            // Get method handle that we can actually invoke
            CILMethodInfo methodInfo = this.GetMethodInfo(AssemblyLoadContext.AppDomain);

            // Create the runtime method
            RuntimeMethod runtimeMethod = new RuntimeMethod(threadContext, AssemblyLoadContext, methodInfo);

            // Perform reflection invoke
            return runtimeMethod.ReflectionInvoke(obj, parameters);
        }

        public override Delegate CreateDelegate(Type delegateType)
        {
            return __delegate.CreateDelegate(delegateType, this);
        }

        public override Delegate CreateDelegate(Type delegateType, object target)
        {
            return Delegate.CreateDelegate(delegateType, target, this);
        }
        #endregion

        #region LazyInit
        private int InitToken()
        {
            return metadataProvider.MetadataReader.GetToken(handle);
        }

        private string InitName()
        {
            return metadataProvider.MetadataReader.GetString(definition.Name);
        }

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
                for(int i = 0; i < parameters.Value.Length; i++)
                {
                    // Append parameter type
                    builder.Append(parameters.Value[i].ParameterType);

                    // Check for more parameters to come
                    if(i < parameters.Value.Length - 1)
                        builder.Append(", ");
                }
            }
            builder.Append(')');
            
            return builder.ToString();
        }

        private MethodSignature<Type> InitSignature()
        {
            // Decode signature
            return definition.DecodeSignature(metadataProvider, null);
        }

        private CLRParameterInfo[] InitParameters()
        {
            // Resolve signature
            MethodSignature<Type> signature = this.signature.Value;

            // Init parameters
            CLRParameterInfo[] parameters = new CLRParameterInfo[signature.ParameterTypes.Length];

            // Get metadata parameters
            Parameter[] metaParameters = definition.GetParameters()
                .Select(p => metadataProvider.MetadataReader.GetParameter(p))
                .ToArray();

            // Setup parameters
            for (int i = 0; i < parameters.Length; i++)
            {
                // Get the parameter
                Parameter parameter = metaParameters[i];

                // Get name
                string parameterName = metadataProvider.MetadataReader.GetString(parameter.Name);

                // Create parameter
                parameters[i] = new CLRParameterInfo(this,
                    parameterName,
                    parameter.SequenceNumber,
                    parameter.Attributes,
                    signature.ParameterTypes[i]);
            }

            // Get parameters
            return parameters;
        }

        private CLRParameterInfo InitReturnParameter()
        {
            // Resolve signature
            MethodSignature<Type> signature = this.signature.Value;

            // Create return param
            return new CLRParameterInfo(this, "Return", 0, 0, signature.ReturnType);   
        }

        private MetadataDebugInformation InitDebugInformation()
        {
            // Check for debug symbols
            if (metadataProvider.HasDebugSymbols == true)
                return new MetadataDebugInformation(metadataProvider, handle);

            return null;
        }
        #endregion
    }
}
