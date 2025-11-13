using dotnow.Reflection;
using System;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;

namespace dotnow.Runtime.CIL
{
    internal static class CILExtensions
    {
        // Methods
        public static AssemblyLoadContext GetLoadContext(this MemberInfo metadataMember)
        {
            // Get the type
            Type metadataType = metadataMember.DeclaringType != null
                ? metadataMember.DeclaringType
                : metadataMember as Type;

            // Check for CLR
            if (metadataType is CLRType clrType)
                return clrType.AssemblyLoadContext;

            // Must be an interop type and they do not have a load context
            return null;
        }

        /// <summary>
        /// Get the type handle for the specified metadata type.
        /// This will load the type into memory if it has not already been loaded.
        /// </summary>
        /// <param name="metaType"></param>
        /// <param name="assemblyLoadContext"></param>
        /// <returns></returns>
        /// <exception cref="TypeLoadException">Type could not be loaded</exception>
        public static CILTypeInfo GetTypeInfo(this Type metaType, AppDomain appDomain)
        {
            // Check for CLR type
            if (metaType is CLRType clrType)
            {
                // Get entity handle
                EntityHandle handle = MetadataTokens.EntityHandle(metaType.MetadataToken);

                // Resolve type - needs to be resolved before it can be used
                clrType.AssemblyLoadContext.ResolveType(handle);

                // Finally get the type handle
                return clrType.AssemblyLoadContext.GetTypeHandle(handle);
            }
            else
            {
                // Get hash
                int hash = appDomain.ResolveInteropTypeHandle(metaType);

                // Get the interop handle
                return appDomain.interopTypeHandles[hash];
            }
        }

        public static CILFieldInfo GetFieldInfo(this FieldInfo metaField, AppDomain appDomain)
        {
            // Ensure that declaring type has been loaded
            metaField.DeclaringType.GetTypeInfo(appDomain);

            // Check for CLR field
            if (metaField is CLRFieldInfo clrField)
            {
                // Get entity handle
                EntityHandle handle = MetadataTokens.EntityHandle(metaField.MetadataToken);

                // Get the field handle direct - It will be resolved when the parent type is loaded
                return clrField.AssemblyLoadContext.GetFieldHandle(handle);
            }
            else
            {
                // Get hash
                int hash = metaField.GetHashCode();

                // Get the interop handle direct - Field should be resolved when parent type is loaded
                return appDomain.interopFieldHandles[hash];
            }
        }

        public static CILMethodInfo GetMethodInfo(this MethodBase metaMethod, AppDomain appDomain)
        {
            // Ensure that declaring type has been loaded
            metaMethod.DeclaringType.GetTypeInfo(appDomain);

            // Check for CLR method
            if (metaMethod is CLRMethodInfo clrMethod)
            {
                // Get entity handle
                EntityHandle handle = MetadataTokens.EntityHandle(metaMethod.MetadataToken);

                // Resolve the method handle
                clrMethod.AssemblyLoadContext.ResolveMethod(handle);

                // Finally get the method handle
                return clrMethod.AssemblyLoadContext.GetMethodHandle(handle);
            }
            else if (metaMethod is CLRConstructorInfo clrConstructor)
            {
                // Get load context
                AssemblyLoadContext assemblyLoadContext = clrConstructor.AssemblyLoadContext;

                // Get entity handle
                EntityHandle handle = MetadataTokens.EntityHandle(metaMethod.MetadataToken);

                // Resolve the method handle
                assemblyLoadContext.ResolveMethod(handle);

                // Finally get the method handle
                return assemblyLoadContext.GetMethodHandle(handle);
            }
            else
            {
                // Get hash
                int hash = appDomain.ResolveInteropMethodHandle(metaMethod);

                // Get the interop handle
                return appDomain.interopMethodHandles[hash];
            }
        }
    }
}
