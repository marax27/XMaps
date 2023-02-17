using System;

namespace XMaps.Exceptions;

/// <summary>
/// The exception thrown when there is a problem with a model's collection property.
/// </summary>
public sealed class CollectionTypeDefinitionException : ModelDefinitionException
{
    public Type EvaluatedPropertyType { get; }
    public Type EvaluatedModelType { get; }

    internal CollectionTypeDefinitionException(string message, Type rootModelType,
        Type evaluatedPropertyType, Type evaluatedModelType)
        : base(message, rootModelType)
    {
        EvaluatedPropertyType = evaluatedPropertyType;
        EvaluatedModelType = evaluatedModelType;
    }
}
