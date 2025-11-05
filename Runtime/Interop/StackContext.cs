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
    public unsafe readonly ref struct StackContext
    {
        // Internal
        internal readonly ThreadContext threadContext;
        internal readonly AssemblyLoadContext assemblyLoadContext;
        internal readonly StackData* spReturn;
        internal readonly StackData* spArg;
        internal readonly int argCount;

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
        public int ArgumentCount => argCount;
        /// <summary>
        /// Return a value indicating whether the current method context should return a value.
        /// </summary>
        public bool HasReturnValue => spReturn != null;

        // Constructor
        internal StackContext(ThreadContext threadContext, AssemblyLoadContext assemblyLoadContext, StackData* spReturn, StackData* spArg, int argCount)
        {
            // Check for null
            if(threadContext == null)
                throw new ArgumentNullException(nameof(threadContext));

            if (assemblyLoadContext == null)
                throw new ArgumentNullException(nameof(assemblyLoadContext));

            if(spArg == null)
                throw new ArgumentNullException(nameof(spArg));

            this.threadContext = threadContext;
            this.assemblyLoadContext = assemblyLoadContext;
            this.spReturn = spReturn;
            this.spArg = spArg;
            this.argCount = argCount;
        }

        // Methods
        #region Read
        public T ReadArgObject<T>(int offset) where T : class
        {
            // Check bounds
            CheckArgBounds(offset);

            // Get managed object
            return threadContext.managedStack[(spArg + offset)->Register] as T;
        }

        public T ReadArgValueType<T>(int offset) where T : struct
        {
            // Check bounds
            CheckArgBounds(offset);

            // Store temp value
            // Required so that we can perform switch on the value type
            T temp = default;

            // Try to read value type which could be managed
            ReadValueType(threadContext, ref temp, spArg + offset);

            // Get result
            return temp;
        }

        public ref T ReadArgRefValueType<T>(int offset) where T : unmanaged
        {
            // Check bounds
            CheckArgBounds(offset);

            // Get pointer at offset to the start of the memory
            T* val = (T*)(spArg + offset)->Ptr;

            // Get reference to value
            return ref *val;
        }

        public StackTypeCode ReadArgStackType(int offset)
        {
            // Check bounds
            CheckArgBounds(offset);

            // Get stack type
            return (spArg + offset)->Type;
        }

        public Type ReadArgTypeHandle(int offset)
        {
            // Get token
            int token = ReadArgValueType<int>(offset);

            // Get handle
            EntityHandle handle = MetadataTokens.EntityHandle(token);

            // Get type handle
            CILTypeHandle typeHandle = assemblyLoadContext.GetTypeHandle(handle);

            // Get meta type
            return typeHandle.MetaType;
        }

        public FieldInfo ReadArgFieldHandle(int offset)
        {
            // Get token 
            int token = ReadArgValueType<int>(offset);

            // Get handle
            EntityHandle handle = MetadataTokens.EntityHandle(token);

            // Get field handle
            CILFieldHandle fieldHandle = assemblyLoadContext.GetFieldHandle(handle);

            // Get meta field
            return fieldHandle.MetaField;
        }

        public MethodBase ReadArgMethodHandle(int offset)
        {
            // Get token
            int token = ReadArgValueType<int>(offset);

            // Get handle
            EntityHandle handle = MetadataTokens.EntityHandle(token);

            // Get method handle
            CILMethodHandle methodHandle = assemblyLoadContext.GetMethodHandle(handle);

            // Get meta method
            return methodHandle.MetaMethod;
        }
        #endregion

        #region WriteArgs
        public void WriteArgObject<T>(int offset, T val) where T : class
        {
            // Check bounds
            CheckArgBounds(offset);

            // Get the arg
            StackData* arg = (spArg + offset);

            // Check for normal value
            if (arg->IsAddress == false)
            {
#if DEBUG
                // Check for reference type
                if (arg->Type != StackTypeCode.ManagedStackClassReference)
                    throw new InvalidOperationException("Argument is not a reference type");
#endif

                // Overwrite reference type on the managed stack
                threadContext.managedStack[(spArg + offset)->Register] = val;
            }
            // Must be an address
            else
            {
                switch(arg->Type)
                {
                    default: throw new NotSupportedException(arg->Type.ToString());
                    case StackTypeCode.StackAddress:
                        {
                            // Get destination stack address
                            StackData* dst = ((StackData*)arg->Ptr);

#if DEBUG
                            // Check for reference type
                            if (dst->Type != StackTypeCode.ManagedStackClassReference)
                                throw new InvalidOperationException("Argument address is not a reference type");
#endif

                            // Overwrite reference type on the managed stack
                            threadContext.managedStack[dst->Register] = val;
                            break;
                        }
                }
            }
        }

        public void WriteArgValueType<T>(int offset, T val) where T : struct
        {
            // Check bounds
            CheckArgBounds(offset);

            // Write to argument pointer
            WriteValueType(threadContext, val, spArg + offset);
        }

        public void WriteArgWrap(int offset, Type type, object obj)
        {
            // Check null
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            // Check bounds
            CheckArgBounds(offset);

            // Get type handle
            CILTypeHandle typeHandle = type.GetHandle(AppDomain);

            // Wrap to stack
            StackData.Wrap(threadContext, typeHandle, obj, spArg + offset);
        }
        #endregion

        #region Return
        public void ReturnWrap(Type type, object obj)
        {
            // Check null
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            // Check return slot
            CheckReturn();

            // Get type handle
            CILTypeHandle typeHandle = type.GetHandle(AppDomain);

            // Wrap to stack
            StackData.Wrap(threadContext, typeHandle, obj, spReturn);
        }

        public void ReturnObject<T>(T val) where T : class
        {
            // Check return slot
            CheckReturn();

            // Write to managed stack
            threadContext.managedStack[spReturn->Register] = val;
        }

        public void ReturnValueType<T>(T val) where T : struct
        {
            // Check return slot
            CheckReturn();

            // Write to return pointer
            WriteValueType(threadContext, val, spReturn);
        }
        #endregion

        private static void ReadValueType<T>(ThreadContext threadContext, ref T valueType, StackData* sp) where T : struct
        {
            // Check for unmanaged
            if (sp->Type == StackTypeCode.UnmanagedValueType)
            {
                // Try to read value direct from the stack
                // Using unsafe instead of raw pointers allows it to compile, but would lead to unexpected behavior or crashes if T is a managed struct
                // Due to the unmanaged check though, we can pretty much guarantee that the struct is blittable in this case, otherwise something has gone very wrong
                valueType = Unsafe.Read<T>((void*)sp->Ptr);
            }
            // Check for managed
            else if (sp->Type == StackTypeCode.ManagedStackValueTypeReference)
            {
                // Read the managed stack - causes unboxing but this is the only way for structs that use managed fields
                valueType = (T)threadContext.managedStack[sp->Register];
            }
            // Must be primitive
            else
            {
                switch (valueType)
                {
                    case bool b:
                        {
                            bool val = (bool)(sp->I32 == 1);
                            valueType = Unsafe.As<bool, T>(ref val);
                            return;
                        }
                    case char c:
                        {
                            char val = (char)sp->I32;
                            valueType = Unsafe.As<char, T>(ref val);
                            return;
                        }
                    case sbyte s:
                        {
                            sbyte val = (sbyte)sp->I32;
                            valueType = Unsafe.As<sbyte, T>(ref val);
                            return;
                        }
                    case byte b:
                        {
                            byte val = (byte)sp->I32;
                            valueType = Unsafe.As<byte, T>(ref val);
                            return;
                        }
                    case short s:
                        {
                            short val = (short)sp->I32;
                            valueType = Unsafe.As<short, T>(ref val);
                            return;
                        }
                    case ushort s:
                        {
                            ushort val = (ushort)sp->I32;
                            valueType = Unsafe.As<ushort, T>(ref val);
                            return;
                        }
                    case int i:
                        {
                            int val = sp->I32;
                            valueType = Unsafe.As<int, T>(ref val);
                            return;
                        }
                    case uint u:
                        {
                            uint val = (uint)sp->I32;
                            valueType = Unsafe.As<uint, T>(ref val);
                            return;
                        }
                    case long l:
                        {
                            long val = sp->I64;
                            valueType = Unsafe.As<long, T>(ref val);
                            return;
                        }
                    case ulong ul:
                        {
                            ulong val = (ulong)sp->I64;
                            valueType = Unsafe.As<ulong, T>(ref val);
                            return;
                        }
                    case float f:
                        {
                            float val = sp->F32;
                            valueType = Unsafe.As<float, T>(ref val);
                            return;
                        }
                    case double d:
                        {
                            double val = sp->F64;
                            valueType = Unsafe.As<double, T>(ref val);
                            return;
                        }
                    case IntPtr i:
                        {
                            IntPtr val = sp->Ptr;
                            valueType = Unsafe.As<IntPtr, T>(ref val);
                            return;
                        }
                    case UIntPtr u:
                        {
                            UIntPtr val = (UIntPtr)(long)sp->Ptr;
                            valueType = Unsafe.As<UIntPtr, T>(ref val);
                            return;
                        }
                }

                // T is not supported??
                throw new NotSupportedException(typeof(T).ToString());
            }
        }

        private static void WriteValueType<T>(ThreadContext threadContext, in T valueType, StackData* sp) where T : struct
        {
            // Check for unmanaged
            if (sp->Type == StackTypeCode.UnmanagedValueType)
            {
                // Try to write value direct to the stack
                // Using unsafe instead of raw pointers allows it to compile, but would lead to unexpected behavior or crashes if T is a managed struct
                // Due to the unmanaged check though, we can pretty much guarantee that the struct is blittable in this case, otherwise something has gone very wrong
                Unsafe.Write((void*)sp->Ptr, valueType);
            }
            // Check for managed
            else if (sp->Type == StackTypeCode.ManagedStackValueTypeReference)
            {
                // Write the managed stack - causes boxing but this is the only way for structs that use managed fields
                threadContext.managedStack[sp->Register] = valueType;
            }
            // Must be primitive
            else
            {
                switch (valueType)
                {
                    case bool b: sp->I32 = b ? 1 : 0; return;
                    case char c: sp->I32 = c; return;
                    case sbyte s: sp->I32 = s; return;
                    case byte b: sp->I32 = b; return;
                    case short s: sp->I32 = s; return;
                    case ushort s: sp->I32 = s; return;
                    case int i: sp->I32 = i; return;
                    case uint u: sp->I32 = (int)u; return;
                    case long l: sp->I64 = l; return;
                    case ulong ul: sp->I64 = (long)ul; return;
                    case float f: sp->F32 = f; return;
                    case double d: sp->F64 = d; return;
                    case IntPtr i: sp->Ptr = i; return;
                    case UIntPtr u: sp->Ptr = (IntPtr)(long)u; return;
                }

                // T is not supported??
                throw new NotSupportedException(typeof(T).ToString());
            }
        }

        [Conditional("DEBUG")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void CheckReturn()
        {
            // Check for return slot provided
            if (spReturn == null)
                throw new InvalidOperationException("Method should not return a value");
        }

        [Conditional("DEBUG")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
        private void CheckArgBounds(int offset)
        {
            // Check bounds
            if (offset < 0 || offset >= argCount)
                throw new IndexOutOfRangeException(nameof(offset));
        }
    }
}
