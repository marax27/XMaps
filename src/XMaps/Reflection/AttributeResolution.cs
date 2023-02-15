using System;
using System.Collections.Generic;
using System.Reflection;

namespace XMaps.Reflection;

internal sealed class AttributeResolution : IAttributeResolution
{
    private readonly ParameterInfo _parameterInfo;

    public AttributeResolution(ParameterInfo parameterInfo)
    {
        _parameterInfo = parameterInfo;
    }

    public IEnumerable<TAttribute> GetAll<TAttribute>() where TAttribute : Attribute
    {
        return _parameterInfo.GetCustomAttributes<TAttribute>();
    }
}
