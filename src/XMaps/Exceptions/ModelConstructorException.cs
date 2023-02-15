using System;

namespace XMaps.Exceptions;

/// <summary>
/// The exception thrown when there is a problem with a model's constructor.
/// </summary>
public sealed class ModelConstructorException : ModelDefinitionException
{
    internal ModelConstructorException(string message, Type rootModelType)
        : base(message, rootModelType) { }
}
