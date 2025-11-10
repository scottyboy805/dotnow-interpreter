using dotnow.Reflection;
using dotnow.Runtime;
using dotnow.Runtime.CIL;
using System;
using System.Linq;

namespace dotnow.Interop.CoreLib
{
    internal static class System_Enum
    {
        // Methods
        [Preserve]
        [CLRMethodBinding(typeof(Enum), nameof(Enum.GetName), typeof(Type), typeof(object))]
        public static void GetName_Override(StackContext context)
        {
            // Get the type and value
            Type metaType = context.ReadArgTypeHandle(0);
            object enumValue = context.ReadArgObject<object>(1);

            // The name of the enum
            string enumName = null;

            // Check for enum
            if(enumValue is CLREnumInstance clrEnum)
            {
                // Use ToString which gives the correct name
                enumName = enumValue.ToString();
            }
            // Check for CLR type
            else if(metaType is CLRType clrType)
            {
                // Try to get the enum name
                clrType.CLREnumNames.TryGetValue(enumValue, out enumName);
            }
            // Must be interop
            else
            {
                // Get enum name
                enumName = Enum.GetName(metaType, enumValue);
            }

            // Write to stack
            context.ReturnObject(enumName);
        }

        [Preserve]
        [CLRMethodBinding(typeof(Enum), nameof(Enum.GetNames), typeof(Type))]
        public static void GetNames_Override(StackContext context)
        {
            // Get the type
            Type metaType = context.ReadArgTypeHandle(0);

            // Fetch all enum value names
            string[] enumNames = (metaType is CLRType clrType)
                ? clrType.CLREnumNames.Values.ToArray()
                : Enum.GetNames(metaType);

            // Write to stack
            context.ReturnObject(enumNames);
        }

        [Preserve]
        [CLRMethodBinding(typeof(Enum), nameof(Enum.Parse), typeof(Type), typeof(string))]
        public static void Parse_Override(StackContext context)
        {
            // Get the type and value
            Type metaType = context.ReadArgTypeHandle(0);
            string enumString = context.ReadArgObject<string>(1);

            // The name of the enum
            object enumValue = null;

            // Check for CLR type
            if (metaType is CLRType clrType)
            {
                foreach(var valueNamePair in clrType.CLREnumNames)
                {
                    if(valueNamePair.Value == enumString)
                    {
                        // Create a boxed representation of the enum
                        enumValue = new CLREnumInstance(clrType, valueNamePair.Key);
                        break;
                    }
                }
            }
            // Must be interop
            else
            {
                // Parse enum value
                enumValue = Enum.Parse(metaType, enumString);
            }

            // Write to stack
            context.ReturnObject(enumValue);
        }

        [Preserve]
        [CLRGenericMethodBinding(typeof(Enum), nameof(Enum.Parse), 1, typeof(string))]
        public static void ParseGeneric_Override(StackContext context, Type[] genericArguments)
        {
            // Get the string value
            string enumString = context.ReadArgObject<string>(0);

            // The parsed enum value
            object enumValue = null;

            // Check for CLR type
            if (genericArguments[0] is CLRType clrType)
            {
                foreach (var valueNamePair in clrType.CLREnumNames)
                {
                    if (valueNamePair.Value == enumString)
                    {
                        // Create a boxed representation of the enum
                        enumValue = new CLREnumInstance(clrType, valueNamePair.Key);
                        break;
                    }
                }
            }
            // Must be interop
            else
            {
                // Parse enum value
                enumValue = Enum.Parse(genericArguments[0], enumString);
            }

            // Write to stack - unbox the value onto the stack as type T
            context.ReturnWrap(genericArguments[0], enumValue);
        }

        [Preserve]
        [CLRMethodBinding(typeof(Enum), nameof(Enum.TryParse), typeof(Type), typeof(string), typeof(object))]
        public static void TryParse_Override(StackContext context)
        {
            // Get the type and value
            Type metaType = context.ReadArgTypeHandle(0);
            string enumString = context.ReadArgObject<string>(1);

            // Check for CLR type
            if (metaType is CLRType clrType)
            {
                foreach (var valueNamePair in clrType.CLREnumNames)
                {
                    if (valueNamePair.Value == enumString)
                    {
                        // Write to out value
                        context.WriteArgAny(2, metaType, new CLREnumInstance(clrType, valueNamePair.Key));

                        // Write result
                        context.ReturnValueType(true);
                        return;
                    }
                }

                // Write to out value with the default value for the enum
                context.WriteArgAny(2, metaType, new CLREnumInstance(clrType, StackData.Default(clrType.GetTypeInfo(context.AppDomain))));

                // Write result
                context.ReturnValueType(false);
            }
            // Must be interop
            else
            {
                // Call interop
                bool success = Enum.TryParse(metaType, enumString, out object result);

                // Write to out value
                context.WriteArgAny(2, metaType, result);

                // Write result
                context.ReturnValueType(success);
            }
        }

        [Preserve]
        [CLRGenericMethodBinding(typeof(Enum), nameof(Enum.TryParse), 1, typeof(string), typeof(__MethodGeneric))]
        public static void TryParseGeneric_Override(StackContext context, Type[] genericTypes)
        {
            // Get the value
            string enumString = context.ReadArgObject<string>(0);

            // Check for CLR type
            if (genericTypes[0] is CLRType clrType)
            {
                foreach (var valueNamePair in clrType.CLREnumNames)
                {
                    if (valueNamePair.Value == enumString)
                    {
                        // Write to out value
                        context.WriteArgAny(1, clrType, new CLREnumInstance(clrType, valueNamePair.Key));

                        // Write result
                        context.ReturnValueType(true);
                        return;
                    }
                }

                // Write to out value with the default value for the enum
                context.WriteArgAny(1, clrType, new CLREnumInstance(clrType, StackData.Default(clrType.GetTypeInfo(context.AppDomain))));

                // Write result
                context.ReturnValueType(false);
            }
            // Must be interop
            else
            {
                // Call interop
                bool success = Enum.TryParse(genericTypes[0], enumString, out object result);

                // Write to out value as enm
                context.WriteArgAny(1, genericTypes[0], result);

                // Write result
                context.ReturnValueType(success);
            }
        }

        [Preserve]
        [CLRMethodBinding(typeof(Enum), nameof(Enum.GetUnderlyingType), typeof(Type))]
        public static void GetUnderlyingType_Override(StackContext context)
        {
            // Get the type
            Type enumType = context.ReadArgObject<Type>(0);

            Type underlyingType = null;

            // Check for clr
            if(enumType is CLRType clrType)
            {
                // Get the underlying type
                underlyingType = clrType.GetEnumUnderlyingType();
            }
            // Must be interop
            else
            {
                // Get underlying type default
                underlyingType = Enum.GetUnderlyingType(enumType);
            }

            // Write to stack
            context.ReturnObject(underlyingType);
        }

        [Preserve]
        [CLRMethodBinding(typeof(Enum), nameof(Enum.GetValues), typeof(Type))]
        public static void GetValues_Override(StackContext context)
        {
            // Get the type
            Type enumType = context.ReadArgObject<Type>(0);

            Array enumValues = null;

            // Check for clr
            if (enumType is CLRType clrType)
            {
                // Create array
                enumValues = clrType.CLREnumNames.Keys
                    .Select(e => new CLREnumInstance(clrType, e))
                    .ToArray();
            }
            // Must be interop
            else
            {
                // Get values direct
                enumValues = Enum.GetValues(enumType);
            }

            // Write array to stack
            context.ReturnObject(enumValues);
        }

        [Preserve]
        [CLRMethodBinding(typeof(Enum), nameof(Enum.Format), typeof(Type), typeof(object), typeof(string))]
        public static void Format_Override(StackContext context)
        {
            // Read the type value and format
            Type enumType = context.ReadArgObject<Type>(0);
            object enumValue = context.ReadArgObject<object>(1);
            string format = context.ReadArgObject<string>(2);

            string formattedEnum = null;

            // Check for clr
            if (enumType is CLRType clrType)
            {
                switch(format)
                {
                    default: throw new NotSupportedException(format);
                    case "g":
                    case "G":
                        {
                            // Just get enum string
                            formattedEnum = enumValue.ToString();
                            break;
                        }
                }
            }
            // Must be interop
            else
            {
                // Get formatted value
                formattedEnum = Enum.Format(enumType, enumValue, format);
            }

            // Write format string to stack
            context.ReturnObject(formattedEnum);
        }
    }
}
