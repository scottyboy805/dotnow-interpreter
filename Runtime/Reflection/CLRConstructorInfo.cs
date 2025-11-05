using dotnow.Runtime;
using dotnow.Runtime.CIL;
using System;
using System.Globalization;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace dotnow.Reflection
{
    internal sealed class CLRConstructorInfo : ConstructorInfo
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
        private readonly Lazy<MetadataDebugInformation> debugInformation = null;

        // Properties
        internal AppDomain AppDomain => metadataProvider.AppDomain;
        internal AssemblyLoadContext AssemblyLoadContext => metadataProvider.AssemblyLoadContext;

        #region ConstructorInfoProperties
        public override int MetadataToken => token.Value;
        public override string Name => name.Value;
        public override Type DeclaringType => declaringType;
        public override MethodAttributes Attributes => definition.Attributes;
        public override Type ReflectedType => typeof(CLRMethodInfo);
        public override RuntimeMethodHandle MethodHandle => throw new NotSupportedException();
        #endregion

        public MetadataDebugInformation DebugInformation => debugInformation.Value;

        // Constructor
        internal CLRConstructorInfo(MetadataReferenceProvider metadataProvider, CLRType declaringType, in MethodDefinitionHandle handle, in MethodDefinition definition)
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
            this.debugInformation = new(InitDebugInformation);

            // Check for body
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

        #region ConstructorInfoMethods
        public override MethodBody GetMethodBody()
        {
            return body;
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
            // Get execution context
            ThreadContext threadContext = metadataProvider.AppDomain.GetThreadContext();

            // Get method handle
            CILMethodInfo methodInfo = this.GetMethodInfo(metadataProvider.AppDomain);

            // Create the runtime method
            RuntimeMethod runtimeMethod = new RuntimeMethod(threadContext, AssemblyLoadContext, methodInfo);

            // Perform reflection invoke
            return runtimeMethod.ReflectionInvoke(obj, parameters);

            //// Check for body
            //if ((handle.Flags & CILMethodFlags.Body) == 0 && (handle.Flags & CILMethodFlags.Native) == 0)
            //    throw new InvalidOperationException("Cannot invoke a method with no body");

            //// Load instance onto stack
            //if ((handle.Flags & CILMethodFlags.This) != 0)
            //{

            //}

            //// Load args onto stack
            //for (int i = 0; i < handle.Signature.ArgCount; i++)
            //{
            //    // Get parameter type
            //    CILTypeHandle parameterTypeHandle = handle.Signature.Parameters[i].ParameterTypeToken.GetTypeHandle(AssemblyLoadContext);

            //    // Load arg
            //    threadContext.LoadOntoStack(parameterTypeHandle, i, parameters[i]);
            //}

            //// Call the method
            //CILInterpreter.ExecuteMethodHandle(threadContext, AssemblyLoadContext, handle);
            //return null;
        }

        public override object Invoke(BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region LazyInit
        private int InitToken()
        {
            return metadataProvider.MetadataReader.GetToken(handle);
        }

        private string InitName()
        {
            return IsStatic == true ? "..ctor" : ".ctor";
        }

        private string InitToString()
        {
            StringBuilder builder = new StringBuilder();

            // Type name
            builder.Append(DeclaringType);

            // Constructor name
            builder.Append(Name);

            // Constructor arguments
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

            // Setup parameters
            for (int i = 0; i < parameters.Length; i++)
            {
                // Create parameter
                parameters[i] = new CLRParameterInfo(this, signature.ParameterTypes[i]);
            }

            // Get parameters
            return parameters;
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
