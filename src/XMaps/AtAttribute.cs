using System;

namespace XMaps;

/// <summary>
/// Responsible for mapping parts of HTML into a model.
/// The <c>AtAttribute</c> can be applied in 2 ways:
/// 1. On a class, to define mapping's entry point.
/// 2. On a field, to map node(s) to a field.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Parameter, AllowMultiple = false)]
public sealed class AtAttribute : Attribute
{
    public string XPath { get; }

    public AtAttribute(string xpath) => XPath = xpath;
}
