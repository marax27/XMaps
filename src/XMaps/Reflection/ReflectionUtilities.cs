using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using XMaps.Exceptions;

namespace XMaps.Reflection;

/// <summary>
/// Reflection-related utilities.
/// </summary>
internal static class ReflectionUtilities
{
    public static bool IsNullable(ParameterInfo info)
    {
        var nullabilityContext = new NullabilityInfoContext();
        var nullabilityInfo = nullabilityContext.Create(info);
        return nullabilityInfo.WriteState == NullabilityState.Nullable;
    }

    public static IEnumerable<TAttribute> GetCustomAttributes<TType, TAttribute>()
        where TAttribute : Attribute
    {
        return typeof(TType).GetCustomAttributes<TAttribute>();
    }

    public static IReadOnlyList<ModelConstructorParameter> GetModelConstructorParameters(Type modelType, Type rootModelType)
    {
        return GetConstructorParameterInfos(modelType, rootModelType)
            .Select(parameter => new ModelConstructorParameter(
                parameter.Name ?? throw new InvalidOperationException($"Unexpected unnamed parameter of type '{parameter.ParameterType.Name}'."),
                parameter.ParameterType,
                IsNullable(parameter),
                new AttributeResolution(parameter)
            ))
            .ToArray();
    }

    public static object InstantiateDefensively(Type instantiatedType,
        object?[] constructorArguments,
        IReadOnlyList<ModelConstructorParameter> constructorParameters)
    {
        // Compare constructor arguments' types with constructor's parameter types.

        if (constructorArguments.Length != constructorParameters.Count)
            FailInstantiation(instantiatedType, $"expecting {constructorParameters.Count} arguments, but got {constructorArguments.Length}");

        for (var i = 0; i < constructorArguments.Length; ++i)
        {
            var expectedType = constructorParameters[i].Type;
            var actualType = constructorArguments[i]?.GetType();

            if (actualType is { })
            {
                if (expectedType != actualType && !actualType.IsAssignableTo(expectedType))
                    FailInstantiation(instantiatedType, $"argument {i} is of type '{actualType.Name}', but expected '{expectedType.Name}'");
            }
            else if (!constructorParameters[i].IsNullable)
            {
                FailInstantiation(instantiatedType, $"argument {i} is null, but expected non-nullable '{expectedType.Name}'");
            }
        }

        // Instantiate the desired object.

        var result = Activator.CreateInstance(instantiatedType, constructorArguments);
        return result
               ?? throw new InvalidOperationException($"Failed to instantiate '{instantiatedType.Name}'.");
    }

    private static void FailInstantiation(Type instantiatedType, string message)
    {
        throw new InvalidOperationException($"Failed to instantiate '{instantiatedType.Name}': {message}");
    }

    private static ParameterInfo[] GetConstructorParameterInfos(Type type, Type modelType)
    {
        var constructors = type.GetConstructors();

        if (constructors.Length == 0)
            throw new ModelConstructorException($"Failed to find a public constructor of type '{type.Name}'.", modelType);
        else if (constructors.Length > 1)
            throw new ModelConstructorException($"Ambiguous model construction for '{type.Name}': found {constructors.Length} public constructors, expected 1.", modelType);

        return constructors[0].GetParameters();
    }
}
