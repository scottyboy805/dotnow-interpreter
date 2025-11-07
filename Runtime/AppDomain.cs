using dotnow.Interop;
using dotnow.Reflection;
using dotnow.Runtime;
using dotnow.Runtime.CIL;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Threading;

[assembly: InternalsVisibleTo("dotnow.Tests")]

namespace dotnow
{
    // Delegate
    internal delegate void VoidCall();

    public delegate void DirectInstance(StackContext stack, Type instanceType);         // Used to create instance of interop type directly
    public delegate void DirectCall(StackContext stack);                                // Used to call interop methods directly
    public delegate void DirectCallGeneric(StackContext stack, Type[] genericTypes);    // Used to call interop generic methods directly
    public delegate void DirectAccess(StackContext stack);                              // Used to access interop fields directly

    public sealed class AppDomain : IDisposable
    {
        // Internal
        internal readonly ConcurrentDictionary<Thread, ThreadContext> threadContexts = new();
        internal readonly ConcurrentDictionary<Assembly, AssemblyLoadContext> assemblyLoadContexts = new();

        /// <summary>
        /// Stores handles for interop members that are part of the calling app (For things like System.Int, System.String, etc.).
        /// The key is the hash of the system type.
        /// </summary>
        internal readonly ConcurrentDictionary<int, CILTypeInfo> interopTypeHandles = new();
        internal readonly ConcurrentDictionary<int, CILFieldInfo> interopFieldHandles = new();
        internal readonly ConcurrentDictionary<int, CILMethodInfo> interopMethodHandles = new();

        // Public
#if !UNITY_EDITOR && ENABLE_IL2CPP
        public const bool IsJITAvailable = false;
#else
        public const bool IsJITAvailable = true;
#endif

        // Constructor
        static AppDomain()
        {
            if (IsJITAvailable == true)
            {
                // Ensure interpreter methods are jit compiled here
                foreach (MethodInfo method in typeof(CILInterpreter).GetMethods())
                    RuntimeHelpers.PrepareMethod(method.MethodHandle);
            }
        }

        public AppDomain()
        {
            // Initialize built in types
            ResolveInteropTypeHandle(typeof(void));
            ResolveInteropTypeHandle(typeof(sbyte));
            ResolveInteropTypeHandle(typeof(short));
            ResolveInteropTypeHandle(typeof(int));
            ResolveInteropTypeHandle(typeof(long));
            ResolveInteropTypeHandle(typeof(byte));
            ResolveInteropTypeHandle(typeof(ushort));
            ResolveInteropTypeHandle(typeof(uint));
            ResolveInteropTypeHandle(typeof(ulong));
            ResolveInteropTypeHandle(typeof(float));
            ResolveInteropTypeHandle(typeof(double));
            ResolveInteropTypeHandle(typeof(decimal));
            ResolveInteropTypeHandle(typeof(char));
            ResolveInteropTypeHandle(typeof(bool));
            ResolveInteropTypeHandle(typeof(string));
            ResolveInteropTypeHandle(typeof(object));
        }
        
        // Methods
        public void Dispose()
        {
            // TODO - support unloading domain
        }

        internal ThreadContext GetThreadContext(int stackSize = 4096)
        {
            // Try to get context for thread
            ThreadContext context;
            if (threadContexts.TryGetValue(Thread.CurrentThread, out context) == true)
                return context;

            // Need to create context
            context = new ThreadContext(stackSize);

            // Add context
            threadContexts[Thread.CurrentThread] = context;
            return context;
        }

        /// <summary>
        /// Attempts to load the method executable and resolve all metadata references associated.
        /// In normal conditions the method is lazy loaded when it will be called by another method or via reflection, this offers more control over preloading.
        /// Note that this also supports interop methods when JIT backend is available, otherwise it will do nothing.
        /// </summary>
        /// <param name="method"></param>
        public void PrepareMethod(MethodBase method)
        {
            // Check for CLR
            if(method.IsCLRMethod() == true)
            {
                // Get the handle which will force JIT analyze
                method.GetMethodInfo(this);
            }
            else
            {
                // Prepare the method
                if (IsJITAvailable == true)
                    RuntimeHelpers.PrepareMethod(method.MethodHandle);
            }
        }

        #region CreateInstance
        public object CreateUninitializedInstance(Type type)
        {
            // Check for null
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            // Check for clr
            if(type is CLRType clrType)
            {         
                // Create clr instance - don't run the constructor
                return CreateCLRInstance(clrType, null, null);
            }
            else
            {
                // Use interop
                return FormatterServices.GetUninitializedObject(type);
            }
        }

        public object CreateInstance(Type type, params object[] args)
        {
            // Check for null
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            // Check for clr
            if (type is CLRType clrType)
            {
                // Get constructor
                MethodBase ctor = null;

                // Check for any arguments
                if (args != null && args.Length > 0)
                {
                    // Try to get the constructor
                    Type[] parameterList = __marshal.GetParameterTypeList(args.Length);

                    // Initialize types
                    for(int i = 0; i < args.Length; i++)
                    {
                        // Check for null
                        if (args[i] != null)
                            parameterList[i] = args[i].GetInterpretedType();
                    }

                    // Try to get the constructor
                    ctor = clrType.GetConstructor(parameterList);
                }
                else
                {
                    // Try to get default constructor
                    ctor = clrType.GetConstructor(Type.EmptyTypes);
                }

                // Check for any constructor
                if (ctor == null)
                    throw new MissingMethodException("No suitable constructor found");

                // Create clr instance - don't run the constructor
                return CreateCLRInstance(clrType, ctor, args);
            }
            else
            {
                // Use interop
                return Activator.CreateInstance(type, args);
            }
        }

        public ICLRInstance CreateInstanceFromProxy(Type type, ICLRProxy proxy)
        {
            // Check for null
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            if (proxy == null)
                throw new ArgumentNullException(nameof(proxy));

            // Check for interop type
            if (type is not CLRType)
                throw new ArgumentException("Type must be a CLR type");

            // Get the type handle
            CILTypeInfo typeInfo = type.GetTypeInfo(this);

            // Create instance
            ICLRInstance instance = CLRTypeInstance.CreateInstanceFromProxy(this, typeInfo, proxy);

            // Try to find default constructor
            MethodBase ctor = type.GetConstructor(Type.EmptyTypes);

            // Run default ctor
            RunInitializer(instance, ctor, null);

            return instance;
        }

        private object CreateCLRInstance(CLRType clrType, MethodBase ctor, object[] args)
        {
            // Check for abstract
            if (clrType.IsAbstract == true)
                throw new InvalidOperationException("Cannot create instance of abstract type");

            // Check for array
            if(clrType.IsArray == true)
            {

            }
            // Check for enum
            else if(clrType.IsEnum == true)
            {

            }
            // Create normal instance or boxed instance
            else
            {
                // Get type handle
                CILTypeInfo typeInfo = clrType.GetTypeInfo(this);

                // Create type instance
                CLRTypeInstance instance = CLRTypeInstance.CreateInstance(this, typeInfo);

                // Run constructor
                RunInitializer(instance, ctor, args);

                // Get instance
                return instance;
            }

            throw new NotSupportedException(clrType.ToString());
        }

        private void RunInitializer(ICLRInstance instance, MethodBase ctor, object[] args)
        {
            // Check for none
            if (ctor == null)
                return;

            // Invoke the constructor
            ctor.Invoke(instance, args);
        }        
        #endregion

        #region LoadAssembly
        public Assembly LoadAssemblyFrom(string assemblyPath, bool keepOpen = true)
        {
            // Try to open the file
            Stream peStream = File.OpenRead(assemblyPath);
            Stream pdbStream = null;

            // Check for pdb
            string pdbPath = Path.ChangeExtension(assemblyPath, ".pdb");

            // Check for any symbols
            if(File.Exists(pdbPath) == true)
                pdbStream = File.OpenRead(pdbPath);

            // Load from stream
            return LoadAssemblyStream(peStream, pdbStream, keepOpen);
        }

        public Assembly LoadAssemblyStream(Stream asmStream, Stream pdbStream = null, bool keepOpen = true)
        {
            Stopwatch timer = Stopwatch.StartNew();

            PEStreamOptions peOptions = PEStreamOptions.PrefetchMetadata | PEStreamOptions.PrefetchEntireImage;
            MetadataStreamOptions pdbOptions = MetadataStreamOptions.PrefetchMetadata;

            // Check for keep open
            if (keepOpen == true)
            {
                peOptions |= PEStreamOptions.LeaveOpen;
                pdbOptions |= MetadataStreamOptions.LeaveOpen;
            }

            // Get hint load location
            string location = "";

            if (asmStream is FileStream)
                location = (asmStream as FileStream).Name;


            // Create reader
            PEReader peReader = new PEReader(asmStream, peOptions);

            // Get metadata reader
            MetadataReader metadataReader = peReader.GetMetadataReader();
            MetadataReader pdbMetadata = null;

            // Check for symbols
            if (pdbStream != null)
            {
                // Get the pdb metadata
                using (MetadataReaderProvider pdbReader = MetadataReaderProvider.FromPortablePdbStream(pdbStream, pdbOptions))
                    pdbMetadata = pdbReader.GetMetadataReader();
            }

            // Create context
            AssemblyLoadContext loadContext = new AssemblyLoadContext(this, peReader, metadataReader, pdbMetadata, location);

            // Add context
            assemblyLoadContexts[loadContext.Assembly] = loadContext;

            // Release the stream
            if (keepOpen == false)
                asmStream.Dispose();

            Debug.Timer("Load Assembly", timer);

            // Get the loaded assembly
            return loadContext.Assembly;
        }
        #endregion

        internal int ResolveInteropTypeHandle(Type type)
        {
            // Get hash code
            int hash = type.GetHashCode();

            // Check for already added
            if (interopTypeHandles.ContainsKey(hash) == false)
            {
                // Create handle
                CILTypeInfo handle = new CILTypeInfo(type);

                // Add to cache
                interopTypeHandles[hash] = handle;
            }
            return hash;
        }

        internal int ResolveInteropFieldHandle(FieldInfo field)
        {
            // Get hash code
            int hash = field.GetHashCode();

            // Check for already added
            if (interopFieldHandles.ContainsKey(hash) == false)
            {
                // Ensure the field type is resolved
                field.FieldType.GetTypeInfo(this);

                // Create handle
                CILFieldInfo handle = new CILFieldInfo(this, field);

                // Add to cache
                interopFieldHandles[hash] = handle;
            }
            return hash;
        }

        internal int ResolveInteropMethodHandle(MethodBase method)
        {
            // Get hash code
            int hash = method.GetHashCode();

            // Check for already added
            if (interopMethodHandles.ContainsKey(hash) == false)
            {
                // Create handle
                CILMethodInfo handle = new CILMethodInfo(this, method);

                // Add to cache
                interopMethodHandles[hash] = handle;
            }
            return hash;
        }
    }
}
