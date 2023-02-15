using System;

namespace XMaps.Exceptions;

/// <summary>
/// Serves as the base class for XMaps-specific exceptions.
/// </summary>
/// <remarks>
/// If XMaps throws something that isn't an <see cref="XMapsException"/> (e.g. <see cref="InvalidOperationException"/>), it may be a bug.
/// </remarks>
public abstract class XMapsException : Exception
{
    public Type RootModelType { get; }

    protected XMapsException(string message, Type rootModelType) : base(message)
    {
        RootModelType = rootModelType;
    }
}
