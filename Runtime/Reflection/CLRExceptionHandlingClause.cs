using System;
using System.Reflection;
using System.Reflection.Metadata;

namespace dotnow.Reflection
{
    internal sealed class CLRExceptionHandlingClause : ExceptionHandlingClause
    {
        // Internal
        internal readonly MetadataReferenceProvider metadataProvider = null;
        internal readonly ExceptionRegion region = default;

        // Private
        private readonly Lazy<Type> catchType;

        public override Type CatchType => catchType.Value;
        public override int TryOffset => region.TryOffset;
        public override int TryLength => region.TryOffset;
        public override int HandlerOffset => region.HandlerOffset;
        public override int HandlerLength => region.HandlerLength;
        public override ExceptionHandlingClauseOptions Flags => (ExceptionHandlingClauseOptions)region.Kind;

        // Constructor
        public CLRExceptionHandlingClause(MetadataReferenceProvider metadataProvider, ExceptionRegion region)
        {
            this.metadataProvider = metadataProvider;
            this.region = region;

            // Initialize handler type
            this.catchType = new(InitCatchType);
        }

        private Type InitCatchType()
        {
            // Just resolve the type
            return metadataProvider.ResolveMetadataType(region.CatchType);
        }
    }
}
