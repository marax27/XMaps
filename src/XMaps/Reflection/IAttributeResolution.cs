using System;
using System.Collections.Generic;

namespace XMaps.Reflection;

internal interface IAttributeResolution
{
    public IEnumerable<TAttribute> GetAll<TAttribute>() where TAttribute : Attribute;
}
