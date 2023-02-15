using System;

namespace XMaps.Exceptions;

/// <summary>
/// The exception thrown when a searched node cannot be found.
/// </summary>
public sealed class NodeNotFoundException : MappingException
{
    public Type EvaluatedModelType { get; }
    public string EvaluatedParameterName { get; }
    public string EvaluatedRelativeXPath { get; }
    public string? ParentXPath { get; }

    internal NodeNotFoundException(string message, Type rootModelType,
        Type evaluatedModelType,
        string evaluatedParameterName,
        string evaluatedRelativeXPath,
        string? parentXPath)
        : base(message, rootModelType)
    {
        EvaluatedModelType = evaluatedModelType;
        EvaluatedParameterName = evaluatedParameterName;
        EvaluatedRelativeXPath = evaluatedRelativeXPath;
        ParentXPath = parentXPath;
    }
}
