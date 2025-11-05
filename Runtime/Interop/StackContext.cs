using dotnow.Runtime;
using dotnow.Runtime.CIL;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;

namespace dotnow.Interop
{
    /// <summary>
    /// Represents the current state of the evaluation stack at the time of method execution.
    /// Allows interop and binding methods to read and write to the stack in a safe performant way.
    /// </summary>
    public readonly ref struct StackContext
    {
        // Internal
        internal readonly AssemblyLoadContext assemblyLoadContext;
        internal readonly Span<StackData> stackArguments;
        internal readonly Span<StackData> stackReturn;

        // Properties
        /// <summary>
        /// Get the <see cref="AppDomain"/> for the current method context.
        /// </summary>
        public AppDomain AppDomain => assemblyLoadContext.AppDomain;
        /// <summary>
        /// Get the <see cref="AssemblyLoadContext"/> for the current method context.
        /// </summary>
        public AssemblyLoadContext AssemblyLoadContext => assemblyLoadContext;

        /// <summary>
        /// Get the number of arguments for the current method context.
        /// Note that the instance is always passed as the first argument for non-static methods.
        /// </summary>
        public int ArgumentCount => stackArguments.Length;
        /// <summary>
        /// Return a value indicating whether the current method context should return a value.
        /// </summary>
        public bool HasReturnValue => stackReturn.Length > 0;

        // Constructor
        internal StackContext(AssemblyLoadContext assemblyLoadContext, Span<StackData> stackArguments, Span<StackData> stackReturn = default)
        {
            if (assemblyLoadContext == null)
                throw new ArgumentNullException(nameof(assemblyLoadContext));

            this.assemblyLoadContext = assemblyLoadContext;
            this.stackArguments = stackArguments;
            this.stackReturn = stackReturn;
        }

        // Methods
        #region Read
        public T ReadArgObject<T>(int offset) where T : class
        {
            // Check bounds
            CheckArgBounds(offset);

            // Get managed object
            return stackArguments[offset].Ref as T;
        }

        public T ReadArgValueType<T>(int offset) where T : struct
        {
            // Check bounds
            CheckArgBounds(offset);

            // Get managed object
            return (T)stackArguments[offset].Ref;
        }

        public StackType ReadArgStackType(int offset)
        {
            // Check bounds
            CheckArgBounds(offset);

            // Get stack type
            return stackArguments[offset].Type;
        }

        public Type ReadArgTypeHandle(int offset)
        {
            // Get token
            int token = ReadArgValueType<int>(offset);

            // Get handle
            EntityHandle handle = MetadataTokens.EntityHandle(token);

            // Get type handle
            CILTypeInfo typeInfo = assemblyLoadContext.GetTypeHandle(handle);

            // Get meta type
            return typeInfo.Type;
        }

        public FieldInfo ReadArgFieldHandle(int offset)
        {
            // Get token 
            int token = ReadArgValueType<int>(offset);

            // Get handle
            EntityHandle handle = MetadataTokens.EntityHandle(token);

            // Get field handle
            CILFieldInfo fieldInfo = assemblyLoadContext.GetFieldHandle(handle);

            // Get meta field
            return fieldInfo.Field;
        }

        public MethodBase ReadArgMethodHandle(int offset)
        {
            // Get token
            int token = ReadArgValueType<int>(offset);

            // Get handle
            EntityHandle handle = MetadataTokens.EntityHandle(token);

            // Get method handle
            CILMethodInfo methodInfo = assemblyLoadContext.GetMethodHandle(handle);

            // Get meta method
            return methodInfo.Method;
        }
        #endregion

        #region WriteArgs
        public void WriteArgObject<T>(int offset, T val) where T : class
        {
            // Check bounds
            CheckArgBounds(offset);

            // Write to argument slot
            WriteArgWrap(offset, typeof(T), val);
        }

        public void WriteArgValueType<T>(int offset, T val) where T : struct
        {
            // Check bounds
            CheckArgBounds(offset);

            // Write to argument slot
            WriteArgWrap(offset, typeof(T), val);
        }

        internal void WriteArgWrap(int offset, Type type, object obj)
        {
            // Check null
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            // Check bounds
            CheckArgBounds(offset);

            // Get type handle
            CILTypeInfo typeInfo = type.GetTypeInfo(AppDomain);

            StackData val = default;

            // Wrap to stack
            StackData.Wrap(typeInfo, obj, ref val);

            // Update the argument value
            stackArguments[offset] = val;
        }
        #endregion

        #region Return
        public void ReturnObject<T>(T val) where T : class
        {
            // Check return slot
            CheckReturn();

            // Write to return slot
            ReturnWrap(typeof(T), val);
        }

        public void ReturnValueType<T>(T val) where T : struct
        {
            // Check return slot
            CheckReturn();

            // Write to return slot
            ReturnWrap(typeof(T), val);
        }

        internal void ReturnWrap(Type type, object obj)
        {
            // Check null
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            // Check return slot
            CheckReturn();

            // Get type handle
            CILTypeInfo typeInfo = type.GetTypeInfo(AppDomain);

            StackData dst = default;

            // Wrap to stack
            StackData.Wrap(typeInfo, obj, ref dst);

            // Assign to return slot
            stackReturn[0] = dst;
        }
        #endregion

        [Conditional("DEBUG")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void CheckReturn()
        {
            // Check for return slot provided
            if (HasReturnValue == false)
                throw new InvalidOperationException("Method should not return a value");
        }

        [Conditional("DEBUG")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
        private void CheckArgBounds(int offset)
        {
            // Check bounds
            if (offset < 0 || offset >= stackArguments.Length)
                throw new IndexOutOfRangeException(nameof(offset));
        }
    }
}
