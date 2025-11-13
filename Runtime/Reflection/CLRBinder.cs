using dotnow.Interop;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace dotnow.Reflection
{
    internal sealed class CLRBinder : Binder
    {
        // Private
        private readonly Binder defaultBinder = Type.DefaultBinder;

        // Methods
        public override FieldInfo BindToField(BindingFlags bindingAttr, FieldInfo[] match, object value, CultureInfo culture)
        {
            // Delegate to default binder for field binding
            return defaultBinder.BindToField(bindingAttr, match, value, culture);
        }

        public override MethodBase BindToMethod(BindingFlags bindingAttr, MethodBase[] match, ref object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] names, out object state)
        {
            // Use default binder for method binding with argument reordering and optional parameters
            return defaultBinder.BindToMethod(bindingAttr, match, ref args, modifiers, culture, names, out state);
        }

        public override object ChangeType(object value, Type type, CultureInfo culture)
        {
            // Delegate to default binder for type conversion
            return defaultBinder.ChangeType(value, type, culture);
        }

        public override void ReorderArgumentArray(ref object[] args, object state)
        {
            // Delegate to default binder for argument reordering
            defaultBinder.ReorderArgumentArray(ref args, state);
        }

        public override MethodBase SelectMethod(BindingFlags bindingAttr, MethodBase[] match, Type[] types, ParameterModifier[] modifiers)
        {
            if (match == null || match.Length == 0)
                return null;

            if (types == null)
                types = Type.EmptyTypes;

            // First try exact matching with the default binder
            var exactMatch = defaultBinder.SelectMethod(bindingAttr, match, types, modifiers);
            if (exactMatch != null)
                return exactMatch;

            // If no exact match found, try semi-loose matching (ignore ref/out/in modifiers)
            var candidateMethods = new List<MethodBase>();

            foreach (var method in match)
            {
                var parameters = method.GetParameters();

                // Check if parameter count matches (considering optional parameters)
                if (IsParameterCountCompatible(parameters, types.Length))
                {
                    // Check if parameter types are compatible (ignoring modifiers)
                    if (AreParameterTypesCompatible(parameters, types))
                    {
                        candidateMethods.Add(method);
                    }
                }
            }

            if (candidateMethods.Count == 0)
                return null;

            // If we have candidates, use the default binder to select the best match
            // Create modified parameter types array without ref/out modifiers for comparison
            var normalizedCandidates = candidateMethods.ToArray();

            // Try to let the default binder select among normalized candidates
            var bestMatch = SelectBestMatch(normalizedCandidates, types, modifiers);

            return bestMatch ?? candidateMethods[0]; // Fallback to first candidate
        }

        public override PropertyInfo SelectProperty(BindingFlags bindingAttr, PropertyInfo[] match, Type returnType, Type[] indexes, ParameterModifier[] modifiers)
        {
            if (match == null || match.Length == 0)
                return null;

            if (indexes == null)
                indexes = Type.EmptyTypes;

            // First try exact matching with the default binder
            var exactMatch = defaultBinder.SelectProperty(bindingAttr, match, returnType, indexes, modifiers);
            if (exactMatch != null)
                return exactMatch;

            // If no exact match found, try semi-loose matching for indexed properties
            var candidateProperties = new List<PropertyInfo>();

            foreach (var property in match)
            {
                // Check return type compatibility if specified
                if (returnType != null && !IsTypeCompatible(property.PropertyType, returnType))
                    continue;

                var indexParameters = property.GetIndexParameters();

                // Check if index parameter count matches
                if (IsParameterCountCompatible(indexParameters, indexes.Length))
                {
                    // Check if index parameter types are compatible (ignoring modifiers)
                    if (AreParameterTypesCompatible(indexParameters, indexes))
                    {
                        candidateProperties.Add(property);
                    }
                }
            }

            if (candidateProperties.Count == 0)
                return null;

            // Return the best match (prioritize exact matches, then by inheritance distance)
            return SelectBestPropertyMatch(candidateProperties.ToArray(), returnType, indexes);
        }

        private bool IsParameterCountCompatible(ParameterInfo[] parameters, int providedCount)
        {
            // Must have at least as many required parameters
            var requiredCount = parameters.Count(p => !p.HasDefaultValue);
            return providedCount >= requiredCount && providedCount <= parameters.Length;
        }

        private bool AreParameterTypesCompatible(ParameterInfo[] parameters, Type[] types)
        {
            for (int i = 0; i < types.Length && i < parameters.Length; i++)
            {
                var paramType = GetUnderlyingParameterType(parameters[i]);
                var providedType = types[i];

                if (!IsTypeCompatible(paramType, providedType))
                    return false;
            }
            return true;
        }

        private Type GetUnderlyingParameterType(ParameterInfo parameter)
        {
            // Remove ref/out/in modifiers to get the underlying type
            var paramType = parameter.ParameterType;
            if (paramType.IsByRef)
                return paramType.GetElementType();
            return paramType;
        }

        private bool IsTypeCompatible(Type parameterType, Type providedType)
        {
            if (parameterType == providedType)
                return true;

            // Check for assignability (handles inheritance and implicit conversions)
            if (parameterType.IsAssignableFrom(providedType))
                return true;

            // Check for generic parameters
            if(parameterType.IsGenericParameter == true)
            {
                // Check for type generic argument
                if (parameterType.DeclaringMethod == null && providedType == typeof(__TypeGeneric))
                    return true;

                // Check for method generic argument
                if(parameterType.DeclaringMethod != null && providedType == typeof(__MethodGeneric))
                    return true;
            }

            // Check for implicit numeric conversions and other built-in conversions
            try
            {
                // Use the default binder's ChangeType to test if conversion is possible
                // This handles implicit conversions like int to long, float to double, etc.
                if (providedType != null && parameterType != typeof(void))
                {
                    var testValue = providedType.IsValueType ? Activator.CreateInstance(providedType) : null;
                    defaultBinder.ChangeType(testValue, parameterType, CultureInfo.InvariantCulture);
                    return true;
                }
            }
            catch
            {
                // Conversion not possible
            }

            return false;
        }

        private MethodBase SelectBestMatch(MethodBase[] candidates, Type[] types, ParameterModifier[] modifiers)
        {
            if (candidates.Length == 1)
                return candidates[0];

            // Score each candidate based on how well it matches
            var scoredCandidates = candidates
                .Select(method => new { Method = method, Score = ScoreMethodMatch(method, types) })
                .Where(item => item.Score >= 0)
                .OrderByDescending(item => item.Score)
                .ToArray();

            return scoredCandidates.Length > 0 ? scoredCandidates[0].Method : null;
        }

        private int ScoreMethodMatch(MethodBase method, Type[] types)
        {
            var parameters = method.GetParameters();
            int score = 0;

            for (int i = 0; i < types.Length && i < parameters.Length; i++)
            {
                var paramType = GetUnderlyingParameterType(parameters[i]);
                var providedType = types[i];

                if (paramType == providedType)
                {
                    score += 100; // Exact match
                }
                else if (paramType.IsAssignableFrom(providedType))
                {
                    score += 50; // Inheritance match
                }
                else if (IsTypeCompatible(paramType, providedType))
                {
                    score += 25; // Conversion match
                }
                else
                {
                    return -1; // Incompatible
                }
            }

            // Bonus for fewer parameters (prefer more specific overloads)
            score += (10 - parameters.Length) * 5;

            return score;
        }

        private PropertyInfo SelectBestPropertyMatch(PropertyInfo[] candidates, Type returnType, Type[] indexes)
        {
            if (candidates.Length == 1)
                return candidates[0];

            // Score each candidate property
            var scoredCandidates = candidates
                .Select(prop => new { Property = prop, Score = ScorePropertyMatch(prop, returnType, indexes) })
                .Where(item => item.Score >= 0)
                .OrderByDescending(item => item.Score)
                .ToArray();

            return scoredCandidates.Length > 0 ? scoredCandidates[0].Property : null;
        }

        private int ScorePropertyMatch(PropertyInfo property, Type returnType, Type[] indexes)
        {
            int score = 0;

            // Score return type compatibility
            if (returnType != null)
            {
                if (property.PropertyType == returnType)
                    score += 100;
                else if (returnType.IsAssignableFrom(property.PropertyType))
                    score += 50;
                else if (IsTypeCompatible(property.PropertyType, returnType))
                    score += 25;
                else
                    return -1;
            }

            // Score index parameter compatibility
            var indexParameters = property.GetIndexParameters();
            for (int i = 0; i < indexes.Length && i < indexParameters.Length; i++)
            {
                var paramType = GetUnderlyingParameterType(indexParameters[i]);
                var indexType = indexes[i];

                if (paramType == indexType)
                    score += 50;
                else if (paramType.IsAssignableFrom(indexType))
                    score += 25;
                else if (IsTypeCompatible(paramType, indexType))
                    score += 10;
                else
                    return -1;
            }

            return score;
        }
    }
}
