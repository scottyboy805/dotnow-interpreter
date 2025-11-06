using dotnow.Runtime;
using dotnow.Runtime.CIL;
using System;
using System.Diagnostics;
using System.Reflection;
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
        internal readonly AppDomain appDomain;
        internal readonly Span<StackData> stackArguments;
        internal readonly Span<StackData> stackReturn;

        // Properties
        /// <summary>
        /// Get the current app domain.
        /// </summary>
        public AppDomain AppDomain => appDomain;
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
        internal StackContext(AppDomain appDomain, Span<StackData> stackArguments, Span<StackData> stackReturn = default)
        {
            this.appDomain = appDomain;
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
            return ReadArgObject<Type>(offset);
        }

        public FieldInfo ReadArgFieldHandle(int offset)
        {
            return ReadArgObject<FieldInfo>(offset);
        }

        public MethodBase ReadArgMethodHandle(int offset)
        {
            return ReadArgObject<MethodBase>(offset);
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
            CILTypeInfo typeInfo = type.GetTypeInfo(appDomain);

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
            CILTypeInfo typeInfo = type.GetTypeInfo(appDomain);

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
