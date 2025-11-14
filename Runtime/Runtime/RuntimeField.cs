using dotnow.Interop;
using dotnow.Reflection;
using dotnow.Runtime.CIL;
using System;

namespace dotnow.Runtime
{
    internal readonly ref struct RuntimeField
    {
        // Private
        private readonly AppDomain appDomain;
        private readonly CILFieldInfo fieldInfo;

        // Constructor
        public RuntimeField(AppDomain appDomain, in CILFieldInfo fieldInfo)
        {
            // Check for null
            if (appDomain == null)
                throw new ArgumentNullException(nameof(appDomain));

            if (fieldInfo == null)
                throw new ArgumentNullException(nameof(fieldInfo));

            this.appDomain = appDomain;
            this.fieldInfo = fieldInfo;
        }

        // Methods
        public void ReflectionSetField(ThreadContext threadContext, object instance, object value)
        {
            // Check write
            CheckFieldWrite(threadContext);

            // Check for instance
            if ((fieldInfo.Flags & CILFieldFlags.This) != 0 && instance == null)
                threadContext.Throw<NullReferenceException>();

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
                SetInstanceFieldDirect(appDomain, fieldInfo, inst, ref val);
            }
            else
            {
                // Set static field
                SetStaticFieldDirect(appDomain, fieldInfo, ref val);
            }
        }

        public object ReflectionGetField(ThreadContext threadContext, object instance)
        {
            // Check for instance
            if ((fieldInfo.Flags & CILFieldFlags.This) != 0 && instance == null)
                threadContext.Throw<NullReferenceException>();

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
                GetInstanceFieldDirect(appDomain, fieldInfo, ref inst, ref val);
            }
            else
            {
                // Set static field
                GetStaticFieldDirect(appDomain, fieldInfo, ref val);
            }

            // Unwrap to managed object
            object unwrapped = null;
            StackData.Unwrap(fieldTypeInfo, val, ref unwrapped);

            // Get result
            return unwrapped;
        }

        internal static void SetInstanceFieldDirect(AppDomain appDomain, CILFieldInfo fieldInfo, in StackData instance, ref StackData value)
        {
            // Get the type handle
            CILTypeInfo fieldTypeInfo = fieldInfo.FieldType;

            // Check for interop
            if ((fieldInfo.Flags & CILFieldFlags.Interop) != 0)
            {
                // Set interop field value
                __marshal.SetFieldInterop(appDomain, fieldInfo, instance, ref value);
            }
            // Check for interpreted
            else if ((fieldInfo.Flags & CILFieldFlags.Interpreted) != 0)
            {
                // Require CLR field
                if (fieldInfo.Field is not CLRFieldInfo clrField)
                    throw new FieldAccessException("Field must be a CLR field");

                // Check for by ref
                if ((fieldInfo.DeclaringType.Flags & CILTypeFlags.ValueType) != 0) //instance.IsByRef == true)
                {
                    // Get the value type instance
                    CLRValueTypeInstance clrValueInstance = instance.IsByRef == true
                        ? (CLRValueTypeInstance)((IByRef)instance.Ref).GetValueRef()
                        : (CLRValueTypeInstance)instance.Ref;

                    // Get the field offset
                    int offset = clrValueInstance.Type.GetInstanceFieldOffset(clrField);

                    // Write the field
                    clrValueInstance.Fields[offset] = value;
                }
                else
                {
                    // Get the CLR instance
                    CLRTypeInstance clrInstance = (CLRTypeInstance)instance.Ref;

                    // Get the field offset
                    int offset = clrInstance.Type.GetInstanceFieldOffset(clrField);

                    // Write the field
                    clrInstance.Fields[offset] = value;
                }
            }
            else
                throw new NotSupportedException(instance.Type.ToString());
        }

        internal static void SetStaticFieldDirect(AppDomain appDomain, CILFieldInfo fieldInfo, ref StackData value)
        {
            // Get the type handle
            CILTypeInfo fieldTypeInfo = fieldInfo.FieldType;

            // Get the declaring type handle
            CILTypeInfo declaringTypeInfo = fieldInfo.DeclaringType;


            // Check for interop
            if ((fieldInfo.Flags & CILFieldFlags.Interop) != 0)
            {
                // Get interop field value
                __marshal.SetFieldInterop(appDomain, fieldInfo, default, ref value);
            }
            // Check for interpreted
            else if ((fieldInfo.Flags & CILFieldFlags.Interpreted) != 0)
            {
                // Require CLR field
                if (fieldInfo.Field is not CLRFieldInfo clrField)
                    throw new FieldAccessException("Field must be a CLR field");

                // Get the field offset
                int fieldOffset = ((CLRType)declaringTypeInfo.Type).GetStaticFieldOffset(clrField);

                // Write the field
                declaringTypeInfo.StaticFields[fieldOffset] = value;
            }
            else
                throw new NotSupportedException(fieldInfo.ToString());
        }

        internal static void GetInstanceFieldDirect(AppDomain appDomain, in CILFieldInfo fieldInfo, ref StackData instance, ref StackData value)
        {
            // Get the type handle
            CILTypeInfo fieldTypeInfo = fieldInfo.FieldType;

            // Check for interop
            if ((fieldInfo.Flags & CILFieldFlags.Interop) != 0)
            {
                // Get interop field value
                __marshal.GetFieldInterop(appDomain, fieldInfo, ref instance, ref value);
            }
            // Check for interpreted
            else if ((fieldInfo.Flags & CILFieldFlags.Interpreted) != 0)
            {
                // Require CLR field
                if (fieldInfo.Field is not CLRFieldInfo clrField)
                    throw new FieldAccessException("Field must be a CLR field");

                // Check for value type
                if ((fieldInfo.DeclaringType.Flags & CILTypeFlags.ValueType) != 0)// instance.IsByRef == true)
                {
                    // Get the value type instance
                    CLRValueTypeInstance clrValueInstance = instance.IsByRef == true
                        ? (CLRValueTypeInstance)((IByRef)instance.Ref).GetValueRef()
                        : (CLRValueTypeInstance)instance.Ref;

                    // Get the field offset
                    int offset = clrValueInstance.Type.GetInstanceFieldOffset(clrField);

                    // Read the field
                    value = clrValueInstance.Fields[offset];
                }
                else
                {
                    // Get the CLR instance
                    CLRTypeInstance clrInstance = (CLRTypeInstance)instance.Ref;

                    // Get the field offset
                    int offset = clrInstance.Type.GetInstanceFieldOffset(clrField);

                    // Read the field
                    value = clrInstance.Fields[offset];
                }
            }
            else
                throw new NotSupportedException(instance.Type.ToString());
        }

        internal static void GetStaticFieldDirect(AppDomain appDomain, in CILFieldInfo fieldInfo, ref StackData value)
        {
            // Get the type handle
            CILTypeInfo fieldTypeInfo = fieldInfo.FieldType;

            // Get the declaring type handle
            CILTypeInfo declaringTypeInfo = fieldInfo.DeclaringType;


            // Check for interop
            if ((fieldInfo.Flags & CILFieldFlags.Interop) != 0)
            {
                // Get interop field value
                StackData inst = default;
                __marshal.GetFieldInterop(appDomain, fieldInfo, ref inst, ref value);
            }
            // Check for interpreted
            else if ((fieldInfo.Flags & CILFieldFlags.Interpreted) != 0)
            {
                // Require CLR field
                if (fieldInfo.Field is not CLRFieldInfo clrField)
                    throw new FieldAccessException("Field must be a CLR field");

                // Get the field offset
                int fieldOffset = ((CLRType)declaringTypeInfo.Type).GetStaticFieldOffset(clrField);

                // Read the field
                value = declaringTypeInfo.StaticFields[fieldOffset];
            }
            else
                throw new NotSupportedException(fieldInfo.ToString());
        }

        private void CheckFieldWrite(ThreadContext threadContext)
        {
            // Check for const
            if ((fieldInfo.Flags & CILFieldFlags.Constant) != 0)
                threadContext.Throw(new InvalidOperationException("Cannot write to a constant field"));
        }
    }
}
