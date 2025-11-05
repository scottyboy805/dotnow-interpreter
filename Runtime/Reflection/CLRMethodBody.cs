using System;
using System.Reflection;
using System.Collections.Generic;
using dotnow.Runtime;
using System.Reflection.Metadata;
using System.Collections.Immutable;

namespace dotnow.Reflection
{
    internal sealed class CLRMethodBody : MethodBody
    {
        // Internal
        internal readonly MetadataReferenceProvider metadataProvider = null;
        internal readonly MethodBodyBlock bodyBlock = null;

        // Private
        private readonly MethodBase method = null;
        private readonly Lazy<CLRVariableInfo[]> locals = null;

        // Properties
        #region MethodBodyProperties
        public override IList<LocalVariableInfo> LocalVariables => locals.Value;
        public override int MaxStackSize => bodyBlock.MaxStack;
        public override bool InitLocals => bodyBlock.LocalVariablesInitialized;
        #endregion

        // Constructor
        internal CLRMethodBody(MetadataReferenceProvider metadataProvider, MethodBase method, MethodBodyBlock bodyBlock)
        {
            this.metadataProvider = metadataProvider;
            this.method = method;
            this.bodyBlock = bodyBlock;

            // Initialize the locals
            this.locals = new(InitLocalVariables);
        }

        // Methods
        internal unsafe UnmanagedMemory<byte> GetInstructionSet()
        {
            // Get the memory reader
            BlobReader reader = bodyBlock.GetILReader();

            // Create unmanaged memory
            return new UnmanagedMemory<byte>((IntPtr)reader.StartPointer, reader.Length);
        }

        private CLRVariableInfo[] InitLocalVariables()
        {
            // Check for no locals - locals are initialized fully by instructions - no need to do it here
            if (bodyBlock.LocalSignature.IsNil == true)
                return Array.Empty<CLRVariableInfo>();

            // Resolve local signature
            StandaloneSignature localSignature = metadataProvider.MetadataReader.GetStandaloneSignature(bodyBlock.LocalSignature);

            // Decode signature
            ImmutableArray<Type> localTypes = localSignature.DecodeLocalSignature(metadataProvider, null);

            // Init array
            CLRVariableInfo[] locals = new CLRVariableInfo[localTypes.Length];

            // Initialize all locals
            for (int i = 0; i < locals.Length; i++)
            {
                // Create the local
                locals[i] = new CLRVariableInfo(localTypes[i], i);
            }
            return locals;
        }
    }
}
