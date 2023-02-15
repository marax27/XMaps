using System;

namespace XMaps.Reflection;

internal sealed record ModelConstructorParameter(
    string Name,
    Type Type,
    bool IsNullable,
    IAttributeResolution CustomAttributes
);
