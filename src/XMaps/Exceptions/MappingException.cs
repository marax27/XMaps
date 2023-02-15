using System;

namespace XMaps.Exceptions;

/// <summary>
/// Serves as the base class for exceptions occurring while mapping the HTML to an object.
/// </summary>
public class MappingException : XMapsException
{
    protected MappingException(string message, Type rootModelType)
        : base(message, rootModelType) { }
}
