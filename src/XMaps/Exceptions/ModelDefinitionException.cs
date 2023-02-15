using System;

namespace XMaps.Exceptions;

/// <summary>
/// Serves as the base exception related to invalid model definition.
/// </summary>
public class ModelDefinitionException : XMapsException
{
    internal ModelDefinitionException(string message, Type rootModelType) 
        : base(message, rootModelType) { }
}
