using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System;
using System.Reflection;
using System.Runtime.Serialization;
using System.Reflection.Metadata;

namespace dotnow.Reflection
{
    internal sealed class CLRAssembly : Assembly
    {
        // Internal
        internal readonly MetadataReferenceProvider metadataProvider = null;
        internal readonly AssemblyDefinition definition = default;

        // Private       
        private readonly CLRType[] types = null;
        private readonly CLRType[] exportedTypes = null;
        private readonly string location = "";

        private readonly Lazy<AssemblyName> assemblyName = null;
        private readonly Lazy<AssemblyName[]> referenceAssemblyNames = null;

        // Properties
        internal AppDomain AppDomain => metadataProvider.AppDomain;
        internal AssemblyLoadContext AssemblyLoadContext => metadataProvider.AssemblyLoadContext;

        internal CLRType[] CLRTypes
        {
            get { return types; }
        }

        // Constructor
        internal CLRAssembly(MetadataReferenceProvider metadataProvider, string location)
        {
            this.metadataProvider = metadataProvider;
            this.definition = metadataProvider.MetadataReader.GetAssemblyDefinition();
            this.location = location;

            this.assemblyName = new (InitAssemblyName);
            this.referenceAssemblyNames = new (InitReferenceAssemblyNames);


            // Create types
            TypeDefinitionHandleCollection typeHandles = metadataProvider.MetadataReader.TypeDefinitions;

            // Create array
            types = new CLRType[typeHandles.Count];

            int index = 0;
            int exportedCount = 0;

            // Process all types
            foreach(TypeDefinitionHandle handle in typeHandles)
            {
                // Init new type
                types[index] = new CLRType(metadataProvider, handle);

                // Check for exported
                if (types[index].IsPublic == true)
                    exportedCount++;

                // Increment index
                index++;
            }

            // Create exported types
            exportedTypes = new CLRType[exportedCount];

            for (int i = 0, j = 0; i < types.Length; i++)
            {
                if (types[i].IsPublic == true)
                {
                    exportedTypes[j] = types[i];
                    j++;
                }
            }
        }

        // Methods
        // System.Reflection.Assembly
        #region Inherit
        public override Type[] GetTypes()
        {
            return types;
        }

        public override Type GetType(string name)
        {
            foreach (CLRType type in types)
            {
                // Check name
                if (string.Compare(type.Name, name) == 0)
                    return type;

                // Check full name
                if (string.Compare(type.FullName, name) == 0)
                    return type;
            }

            return null;
        }

        public override Type GetType(string name, bool throwOnError)
        {
            foreach (CLRType type in types)
            {
                // Check name
                if (string.Compare(type.Name, name) == 0)
                    return type;

                // Check full name
                if (string.Compare(type.FullName, name) == 0)
                    return type;
            }

            if (throwOnError == true)
                throw new TargetException("Failed to find type: " + name);

            return null;
        }

        public override Type GetType(string name, bool throwOnError, bool ignoreCase)
        {
            foreach (CLRType type in types)
            {
                // Check name
                if (string.Compare(type.Name, name, true) == 0)
                    return type;

                // Check full name
                if (string.Compare(type.FullName, name) == 0)
                    return type;
            }

            if (throwOnError == true)
                throw new TargetException("Failed to find type: " + name);

            return null;
        }

        public override Type[] GetExportedTypes()
        {
            return exportedTypes;
        }

        public override object CreateInstance(string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes)
        {
            throw new NotSupportedException("Use AppDomain.CreateInstance instead");
        }

        public override object[] GetCustomAttributes(bool inherit)
        {
            throw new NotSupportedException("Custom attributes are not supported");
        }

        public override object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            throw new NotSupportedException("Custom attributes are not supported");
        }

        public override bool IsDefined(Type attributeType, bool inherit)
        {
            throw new NotSupportedException("Custom attributes are not supported");
        }

        public override IList<CustomAttributeData> GetCustomAttributesData()
        {
            throw new NotSupportedException("Custom attributes are not supported");
        }

        public override Module[] GetLoadedModules(bool getResourceModules)
        {
            throw new NotSupportedException("Trivial CLR has no concept of modules");
        }

        public override Module GetModule(string name)
        {
            throw new NotSupportedException("Trivial CLR has no concept of modules");
        }

        public override Module[] GetModules(bool getResourceModules)
        {
            throw new NotSupportedException("Trivial CLR has no concept of modules");
        }

        public override Module LoadModule(string moduleName, byte[] rawModule, byte[] rawSymbolStore)
        {
            throw new NotSupportedException("Trivial CLR has no concept of modules");
        }

        public override AssemblyName GetName()
        {
            return assemblyName.Value;
        }

        public override AssemblyName GetName(bool copiedName)
        {
            if (copiedName == true)
                return new AssemblyName(GetName().FullName);

            return GetName();
        }

        public override AssemblyName[] GetReferencedAssemblies()
        {
            return referenceAssemblyNames.Value;
        }

        public override FileStream GetFile(string name)
        {
            throw new NotSupportedException("dotnow does not support encapsulated file storage");
        }

        public override FileStream[] GetFiles()
        {
            throw new NotSupportedException("dotnow does not support encapsulated file storage");
        }

        public override FileStream[] GetFiles(bool getResourceModules)
        {
            throw new NotSupportedException("dotnow does not support encapsulated file storage");
        }

        public override ManifestResourceInfo GetManifestResourceInfo(string resourceName)
        {
            throw new NotSupportedException("dotnow does not support manifest resources");
        }

        public override string[] GetManifestResourceNames()
        {
            throw new NotSupportedException("dotnow does not support manifest resources");
        }

        public override Stream GetManifestResourceStream(string name)
        {
            throw new NotSupportedException("dotnow does not support manifest resources");
        }

        public override Stream GetManifestResourceStream(Type type, string name)
        {
            throw new NotSupportedException("dotnow does not support manifest resources");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotSupportedException("Serialization is not supported");
        }

        public override Assembly GetSatelliteAssembly(CultureInfo culture)
        {
            throw new NotSupportedException("Satellite assemblies are not supported");
        }

        public override Assembly GetSatelliteAssembly(CultureInfo culture, Version version)
        {
            throw new NotSupportedException("Satellite assemblies are not supported");
        }
        #endregion

        #region LazyInit
        private AssemblyName InitAssemblyName()
        {
            return new AssemblyName(metadataProvider.MetadataReader.GetString(definition.Name));
        }

        private AssemblyName[] InitReferenceAssemblyNames()
        {
            // Get collection
            AssemblyReferenceHandleCollection referenceHandles = metadataProvider.MetadataReader.AssemblyReferences;

            // Create array
            AssemblyName[] referenceNames = new AssemblyName[referenceHandles.Count];

            // Process all
            int index = 0;
            foreach (AssemblyReferenceHandle handle in referenceHandles)
            {
                // Get the reference
                AssemblyReference reference = metadataProvider.MetadataReader.GetAssemblyReference(handle);

                // Get as assembly name
                referenceNames[index] = reference.GetAssemblyName();
                index++;
            }
            return referenceNames;
        }
        #endregion
    }
}
