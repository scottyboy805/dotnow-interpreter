using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using dotnow;
using dotnow.Reflection;

public static class GetConstructorHelper
{
    public static CLRConstructor GetRuntimeCLRCtor(dotnow.AppDomain appDomain, CLRType type, object[] args)
    {
        var ctorDefinitions = type.Definition.Methods.Where(m => m.IsConstructor);

        if (args == null || args.Length == 0)
        {
            var parameterlessCtor = ctorDefinitions.FirstOrDefault(c => c.Parameters.Count == 0);
            return parameterlessCtor != null ? new CLRConstructor(appDomain, type, parameterlessCtor) : null;
        }

        CLRConstructor bestMatch = null;
        int bestMatchScore = -1;

        foreach (var ctorDef in ctorDefinitions)
        {
            if (ctorDef.Parameters.Count != args.Length) continue;

            int score = 0;
            bool isValidMatch = true;

            for (int i = 0; i < args.Length; i++)
            {
                var paramType = appDomain.ResolveType(ctorDef.Parameters[i].ParameterType);
                var argType = args[i]?.GetType();

                int paramScore = CalculateTypeMatchScore(paramType, argType);
                if (paramScore == -1)
                {

                    isValidMatch = false;
                    break;
                }
                score += paramScore;
            }

            if (isValidMatch && score > bestMatchScore)
            {
                bestMatch = new CLRConstructor(appDomain, type, ctorDef);
                bestMatchScore = score;
            }
        }

        return bestMatch;
    }
    
    private static int CalculateTypeMatchScore(Type paramType, Type argType)
    {
        if (argType == null)
        {
            return paramType.IsValueType ? -1 : 0;
        }

        if (paramType == argType)
        {
            return 100;
        }

        if (paramType.IsAssignableFrom(argType))
        {
            return 75;
        }

        if (paramType.IsGenericParameter)
        {
            return 50;
        }

        if (IsNumericType(paramType) && IsNumericType(argType))
        {
            return 25;
        }

        if (paramType.IsInterface && argType.GetInterfaces().Contains(paramType))
        {
            return 60;
        }

        if (HasImplicitConversion(argType, paramType))
        {
            return 40;
        }

        return -1;
    }

    private static bool IsNumericType(Type type)
    {
        switch (Type.GetTypeCode(type))
        {
            case TypeCode.Byte:
            case TypeCode.SByte:
            case TypeCode.UInt16:
            case TypeCode.UInt32:
            case TypeCode.UInt64:
            case TypeCode.Int16:
            case TypeCode.Int32:
            case TypeCode.Int64:
            case TypeCode.Decimal:
            case TypeCode.Double:
            case TypeCode.Single:
                return true;
            default:
                return false;
        }
    }

    private static bool HasImplicitConversion(Type fromType, Type toType)
    {
        return fromType.GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Where(m => m.Name == "op_Implicit" && m.ReturnType == toType)
            .Any(m => m.GetParameters().Length == 1 && m.GetParameters()[0].ParameterType.IsAssignableFrom(fromType));
    }
    
    
    public class ConstructorMatch
    {
        public ConstructorInfo Constructor { get; set; }
        public object[] Arguments { get; set; }
    }

    public static ConstructorMatch FindBaseConstructor(Type baseType, object[] childArguments)
    {
        var constructors = baseType.GetConstructors(BindingFlags.Public | BindingFlags.Instance);

        foreach (var constructor in constructors)
        {
            var parameters = constructor.GetParameters();
            var matchedArguments = new List<object>();
            bool isMatch = true;
            int childArgIndex = 0;

            foreach (var parameter in parameters)
            {
                bool parameterMatched = false;

                while (childArgIndex < childArguments.Length)
                {
                    var arg = childArguments[childArgIndex];
                    if (arg != null && parameter.ParameterType.IsAssignableFrom(arg.GetType()))
                    {
                        matchedArguments.Add(arg);
                        parameterMatched = true;
                        childArgIndex++;
                        break;
                    }
                    childArgIndex++;
                }

                if (!parameterMatched)
                {
                    isMatch = false;
                    break;
                }
            }

            if (isMatch && matchedArguments.Count == parameters.Length)
            {
                return new ConstructorMatch
                {
                    Constructor = constructor,
                    Arguments = matchedArguments.ToArray()
                };
            }
        }

        return null; // No matching constructor found
    }
}
