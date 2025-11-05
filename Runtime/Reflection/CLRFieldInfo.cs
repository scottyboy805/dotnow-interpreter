using dotnow.Runtime;
using dotnow.Runtime.CIL;
using System;
using System.Globalization;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;

namespace dotnow.Reflection
{
    internal sealed class CLRFieldInfo : FieldInfo
    {
        // Internal
        internal readonly MetadataReferenceProvider metadataProvider = null;
        internal readonly FieldDefinitionHandle handle = default;
        internal readonly FieldDefinition definition = default;

        // Private
        private readonly CLRType declaringType = null;

        private readonly Lazy<int> token = null;
        private readonly Lazy<string> name = null;
        private readonly Lazy<string> toString = null;
        private readonly Lazy<Type> fieldType = null;
        private readonly Lazy<TypeCode> fieldTypeCode = null;
        private readonly Lazy<object> fieldConstant = null;

        // Properties
        internal AppDomain AppDomain => metadataProvider.AppDomain;
        internal AssemblyLoadContext AssemblyLoadContext => metadataProvider.AssemblyLoadContext;

        #region FieldInfoProperties
        public override int MetadataToken => token.Value;
        public override string Name => name.Value;
        public override FieldAttributes Attributes => definition.Attributes;
        public override Type DeclaringType => declaringType;
        public override Type FieldType => fieldType.Value;        
        public override Type ReflectedType => typeof(CLRFieldInfo);
        public override RuntimeFieldHandle FieldHandle => throw new NotSupportedException();
        public override object GetRawConstantValue() => fieldConstant.Value;
        #endregion

        public TypeCode TypeCode => fieldTypeCode.Value;

        // Constructor
        internal CLRFieldInfo(MetadataReferenceProvider metadataProvider, CLRType declaringType, in FieldDefinitionHandle handle, in FieldDefinition definition)
        {
            this.metadataProvider = metadataProvider;
            this.declaringType = declaringType;
            this.handle = handle;
            this.definition = definition;
            
            this.token = new(InitToken);
            this.name = new(InitName);
            this.toString = new(InitToString);
            this.fieldType = new(InitFieldType);
            this.fieldTypeCode = new(InitFieldTypeCode);
            this.fieldConstant = new(InitConstant);
        }

        // Methods
        public override string ToString()
        {
            return toString.Value;
        }

        internal unsafe UnmanagedMemory<byte> GetFieldInitData()
        {
            // Get the rva
            int rva = definition.GetRelativeVirtualAddress();

            // Check for none - the field does not have init data
            if(rva == 0)
                return UnmanagedMemory<byte>.Empty;

            // Get the field type - Safe to assume it is a CLR type when dealing with field initializers with a valid rva (compiler generated)
            CLRType fieldType = (CLRType)FieldType;

            // Get the layout size
            int layoutSize = fieldType.definition.GetLayout().Size;            

            // Read the data
            PEMemoryBlock memoryBlock = metadataProvider.PEReader.GetSectionData(rva);            

            // Create the unmanaged memory
            return new UnmanagedMemory<byte>((IntPtr)memoryBlock.Pointer, layoutSize);
        }

        #region MethodInfoMethods
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

        public override object GetValue(object obj)
        {
            // Get execution context
            ThreadContext threadContext = metadataProvider.AppDomain.GetThreadContext();

            // Get field handle and declaring type handles
            CILFieldHandle fieldHandle = this.GetHandle(metadataProvider.AppDomain);


            {
                // Create runtime field
                RuntimeField runtimeField = new RuntimeField(threadContext, AssemblyLoadContext, fieldHandle);

                // Get value
                return runtimeField.ReflectionGetField(obj);
            }
        }

        public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, CultureInfo culture)
        {
            // Get execution context
            ThreadContext threadContext = metadataProvider.AppDomain.GetThreadContext();

            // Get field handle and declaring type handles
            CILFieldHandle fieldHandle = this.GetHandle(metadataProvider.AppDomain);


            {
                // Create runtime field
                RuntimeField runtimeField = new RuntimeField(threadContext, AssemblyLoadContext, fieldHandle);

                // Get value
                runtimeField.ReflectionSetField(obj, value);
            }
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
            return string.Format("{0} {1}.{2}", declaringType.FullName, FieldType.FullName, Name);
        }

        private Type InitFieldType()
        {
            // Decode signature which maps to field type
            return definition.DecodeSignature(metadataProvider, null);
        }

        private TypeCode InitFieldTypeCode()
        {
            return Type.GetTypeCode(FieldType);
        }

        private object InitConstant()
        {
            // Check for not constant
            if ((definition.Attributes & FieldAttributes.Literal) == 0)
                return null;

            // Gte the constant handle
            Constant constant = metadataProvider.MetadataReader.GetConstant(definition.GetDefaultValue());

            // Decode the value
            return GetConstantValue(metadataProvider.MetadataReader, constant);
        }
        #endregion

        private static object GetConstantValue(MetadataReader reader, in Constant constant)
        {
            // Get constant reader
            BlobReader blobReader = reader.GetBlobReader(constant.Value);
            
            // Read correct value
            return constant.TypeCode switch
            {
                ConstantTypeCode.Boolean => blobReader.ReadBoolean(),
                ConstantTypeCode.Char => (char)blobReader.ReadUInt16(),
                ConstantTypeCode.SByte => blobReader.ReadSByte(),
                ConstantTypeCode.Byte => blobReader.ReadByte(),
                ConstantTypeCode.Int16 => blobReader.ReadInt16(),
                ConstantTypeCode.UInt16 => blobReader.ReadUInt16(),
                ConstantTypeCode.Int32 => blobReader.ReadInt32(),
                ConstantTypeCode.UInt32 => blobReader.ReadUInt32(),
                ConstantTypeCode.Int64 => blobReader.ReadInt64(),
                ConstantTypeCode.UInt64 => blobReader.ReadUInt64(),
                ConstantTypeCode.Single => blobReader.ReadSingle(),
                ConstantTypeCode.Double => blobReader.ReadDouble(),
                ConstantTypeCode.String => blobReader.ReadUTF16(blobReader.Length),
                _ => null // Null means no supported constant type
            };
        }
    }
}
