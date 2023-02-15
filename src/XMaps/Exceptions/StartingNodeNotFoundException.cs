using System;

namespace XMaps.Exceptions;

/// <summary>
/// The exception thrown when a starting node path cannot be found.
/// </summary>
public sealed class StartingNodeNotFoundException : MappingException
{
    public string? StartingXPath { get; }

    internal StartingNodeNotFoundException(string message, Type rootModelType, string? startingXPath)
        : base(message, rootModelType)
    {
        StartingXPath = startingXPath;
    }
}
