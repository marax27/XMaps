using System;

namespace XMaps;

/// <summary>
/// Responsible for mapping parts of HTML into a model.
/// The <c>AtAttribute</c> can be applied in 2 ways:
/// 1. Applied to a class, in order to define mapping's entry point.
/// 2. Applied to a constructor parameter in order to map node(s) to this parameter.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Parameter, AllowMultiple = false)]
public sealed class AtAttribute : Attribute
{
    public string XPath { get; }

    public AtAttribute(string xpath) => XPath = xpath;
}
