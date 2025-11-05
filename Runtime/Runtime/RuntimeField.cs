using dotnow.Interop;
using dotnow.Runtime.CIL;
using System;

namespace dotnow.Runtime
{
    internal readonly ref struct RuntimeField
    {
        // Private
        private readonly ThreadContext threadContext;
        private readonly AssemblyLoadContext assemblyLoadContext;
        private readonly CILFieldInfo fieldInfo;

        // Constructor
        public RuntimeField(ThreadContext threadContext, AssemblyLoadContext assemblyLoadContext, in CILFieldInfo fieldInfo)
        {
            // Check for null
            if (threadContext == null)
                throw new ArgumentNullException(nameof(threadContext));

            if (assemblyLoadContext == null)
                throw new ArgumentNullException(nameof(assemblyLoadContext));

            this.threadContext = threadContext;
            this.assemblyLoadContext = assemblyLoadContext;
            this.fieldInfo = fieldInfo;
        }

        // Methods
        public void ReflectionSetField(object instance, object value)
        {
            // Check write
            CheckFieldWrite();

            // Check for instance
            if ((fieldInfo.Flags & CILFieldFlags.This) != 0 && instance == null)
                throw new NullReferenceException("Instance is null");

            StackData inst = default;
            StackData val = default;

            // Load instance
            if ((fieldInfo.Flags & CILFieldFlags.This) != 0)
            {
                // Get instance type handle
                CILTypeInfo declaringTypeInfo = fieldInfo.DeclaringType;

                // Wrap instance
                StackData.Wrap(declaringTypeInfo, instance, ref inst);
            }

            // Get field type handle
            CILTypeInfo fieldTypeInfo = fieldInfo.FieldType;

            // Load value
            StackData.Wrap(fieldTypeInfo, value, ref val);

            // Set the field direct
            if ((fieldInfo.Flags & CILFieldFlags.This) != 0)
            {
                // Set instance field
                SetInstanceFieldDirect(threadContext, assemblyLoadContext, fieldInfo, inst, ref val);
            }
            else
            {
                // Set static field
                SetStaticFieldDirect(threadContext, assemblyLoadContext, fieldInfo, ref val);
            }
        }

        public object ReflectionGetField(object instance)
        {
            // Check for instance
            if ((fieldInfo.Flags & CILFieldFlags.This) != 0 && instance == null)
                throw new NullReferenceException("Instance is null");

            // Check for constant
            if ((fieldInfo.Flags & CILFieldFlags.Constant) != 0)
                return fieldInfo.Field.GetRawConstantValue();

            StackData inst = default;
            StackData val = default;

            // Load instance
            if ((fieldInfo.Flags & CILFieldFlags.This) != 0)
            {
                // Get instance type handle
                CILTypeInfo declaringTypeInfo = fieldInfo.DeclaringType;

                // Wrap instance
                StackData.Wrap(declaringTypeInfo, instance, ref inst);
            }

            // Get field type handle
            CILTypeInfo fieldTypeInfo = fieldInfo.FieldType;

            // Get the field direct
            if ((fieldInfo.Flags & CILFieldFlags.This) != 0)
            {
                // Set instance field
                GetInstanceFieldDirect(threadContext, assemblyLoadContext, fieldInfo, inst, ref val);
            }
            else
            {
                // Set static field
                GetStaticFieldDirect(threadContext, assemblyLoadContext, fieldInfo, ref val);
            }

            // Unwrap to managed object
            object unwrapped = null;
            StackData.Unwrap(fieldTypeInfo, ref val, ref unwrapped);

            // Get result
            return unwrapped;
        }

        internal static void SetInstanceFieldDirect(ThreadContext threadContext, AssemblyLoadContext assemblyLoadContext, CILFieldInfo fieldInfo, in StackData instance, ref StackData value)
        {
            // Get the type handle
            CILTypeInfo fieldTypeInfo = fieldInfo.FieldType;

            // Check for interop
            if ((fieldInfo.Flags & CILFieldFlags.Interop) != 0)
            {
                // Set interop field value
                __marshal.SetFieldInterop(threadContext, assemblyLoadContext, fieldInfo, instance, ref value);
            }
            // Check for interpreted
            else if ((fieldInfo.Flags & CILFieldFlags.Interpreted) != 0)
            {
                // Get the CLR instance
                CLRTypeInstance clrInstance = (CLRTypeInstance)instance.Ref;


#warning fix this
                // Set field value
                //clrInstance.Fields[...] = value;
            }
            else
                throw new NotSupportedException(instance.Type.ToString());
        }

        internal static void SetStaticFieldDirect(ThreadContext threadContext, AssemblyLoadContext assemblyLoadContext, CILFieldInfo fieldInfo, ref StackData value)
        {
            // Get the type handle
            CILTypeInfo fieldTypeInfo = fieldInfo.FieldType;

            // Get the declaring type handle
            CILTypeInfo declaringTypeInfo = fieldInfo.DeclaringType;


            // Check for interop
            if ((fieldInfo.Flags & CILFieldFlags.Interop) != 0)
            {
                // Get interop field value
                __marshal.SetFieldInterop(threadContext, assemblyLoadContext, fieldInfo, default, ref value);
            }
            // Check for interpreted
            else if ((fieldInfo.Flags & CILFieldFlags.Interpreted) != 0)
            {
//                // Check for reference
//                if ((fieldInfo.Flags & CILFieldFlags.ReferenceType) != 0 || (fieldInfo.Flags & CILFieldFlags.ManagedValueType) != 0)
//                {
//#if DEBUG
//                    // Require reference address
//                    if (value->Type != StackTypeCode.ManagedStackClassReference && value->Type != StackTypeCode.ManagedStackValueTypeReference)
//                        throw new InvalidOperationException("Value must be a managed stack reference");
//#endif
//                    // Copy the reference
//                    declaringTypeInfo.StaticInstance.staticObjects[fieldInfo.ObjectOffset] = threadContext.managedStack[value->Register];
//                }
//                // Check for primitive
//                else if ((fieldInfo.Flags & CILFieldFlags.PrimitiveType) != 0)
//                {
//                    // Get pointer
//                    byte* instanceFieldPtr = declaringTypeInfo.StaticInstance.staticMemory + fieldInfo.MemoryOffset;

//                    // Copy primitive memory
//                    StackData.CopyPrimitiveToMemory(value, instanceFieldPtr, fieldTypeInfo.TypeCode);
//                }
//                // Must be value type
//                else
//                {
//                    // Get pointer
//                    byte* instanceFieldPtr = declaringTypeInfo.StaticInstance.staticMemory + fieldInfo.MemoryOffset;

//                    // Copy value type memory
//                    __gc.CopyMemory((byte*)value->Ptr, instanceFieldPtr, fieldTypeInfo.InstanceSize);
//                }
            }
            else
                throw new NotSupportedException(fieldInfo.ToString());
        }

        internal static void GetInstanceFieldDirect(ThreadContext threadContext, AssemblyLoadContext assemblyLoadContext, in CILFieldInfo fieldInfo, in StackData instance, ref StackData value)
        {
            // Get the type handle
            CILTypeInfo fieldTypeInfo = fieldInfo.FieldType;

            // Check for interop
            if ((fieldInfo.Flags & CILFieldFlags.Interop) != 0)
            {
                // Get interop field value
                __marshal.GetFieldInterop(threadContext, assemblyLoadContext, fieldInfo, instance, ref value);
            }
            // Check for interpreted
            else if ((fieldInfo.Flags & CILFieldFlags.Interpreted) != 0)
            {
                // Get the CLR instance
                CLRTypeInstance clrInstance = (CLRTypeInstance)instance.Ref;

#warning TODO
            }
            else
                throw new NotSupportedException(instance.Type.ToString());
        }

        internal static void GetStaticFieldDirect(ThreadContext threadContext, AssemblyLoadContext assemblyLoadContext, in CILFieldInfo fieldInfo, ref StackData value)
        {
            // Get the type handle
            CILTypeInfo fieldTypeInfo = fieldInfo.FieldType;

            // Get the declaring type handle
            CILTypeInfo declaringTypeInfo = fieldInfo.DeclaringType;


            // Check for interop
            if ((fieldInfo.Flags & CILFieldFlags.Interop) != 0)
            {
                // Get interop field value
                __marshal.GetFieldInterop(threadContext, assemblyLoadContext, fieldInfo, default, ref value);
            }
            // Check for interpreted
            else if ((fieldInfo.Flags & CILFieldFlags.Interpreted) != 0)
            {
//                // Check for reference
//                if ((fieldInfo.Flags & CILFieldFlags.ReferenceType) != 0 || (fieldInfo.Flags & CILFieldFlags.ManagedValueType) != 0)
//                {
//#if DEBUG
//                    // Require reference address
//                    if (value->Type != StackTypeCode.ManagedStackClassReference && value->Type != StackTypeCode.ManagedStackValueTypeReference)
//                        throw new InvalidOperationException("Value must be a managed stack reference");
//#endif
//                    // Copy the reference
//                    threadContext.managedStack[value->Register] = declaringTypeInfo.StaticInstance.staticObjects[fieldInfo.ObjectOffset];
//                }
//                // Check for primitive
//                else if ((fieldInfo.Flags & CILFieldFlags.PrimitiveType) != 0)
//                {
//                    // Get pointer
//                    byte* instanceFieldPtr = declaringTypeInfo.StaticInstance.staticMemory + fieldInfo.MemoryOffset;

//                    // Copy primitive memory
//                    StackData.CopyPrimitiveFromMemory(value, instanceFieldPtr, fieldTypeInfo.TypeCode);
//                }
//                // Must be value type
//                else
//                {
//                    // Get pointer
//                    byte* instanceFieldPtr = declaringTypeInfo.StaticInstance.staticMemory + fieldInfo.MemoryOffset;

//                    // Copy value type memory
//                    __gc.CopyMemory(instanceFieldPtr, (byte*)value->Ptr, fieldTypeInfo.InstanceSize);
//                }
            }
            else
                throw new NotSupportedException(fieldInfo.ToString());
        }

        private void CheckFieldWrite()
        {
            // Check for const
            if ((fieldInfo.Flags & CILFieldFlags.Constant) != 0)
                throw new InvalidOperationException("Cannot write to a constant field");
        }
    }
}
