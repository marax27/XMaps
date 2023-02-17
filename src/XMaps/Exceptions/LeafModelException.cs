using System;

namespace XMaps.Exceptions;

public sealed class LeafModelException : MappingException
{
    public Type EvaluatedModelType { get; }
    public string? XPath { get; }

    public LeafModelException(string message, Type rootModelType, Exception? innerException,
        Type evaluatedModelType,
        string? xpath)
        : base($"{message} {innerException?.Message}", rootModelType, innerException)
    {
        EvaluatedModelType = evaluatedModelType;
        XPath = xpath;
    }
}
