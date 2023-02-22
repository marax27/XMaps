using System;

namespace XMaps;

/// <summary>
/// It is an attribute that allows you to annotate a model and its constructor parameters with 
/// XPath expressions. This way, the mapper will know which HTML nodes to use when initializing the model.<br/>
/// The <c>AtAttribute</c> can be applied in 2 ways:
/// <list type="bullet">
///     <item>
///         <description>
///             to a model type - optional. When applied to a model type, the attribute's
///             XPath expression serves as a starting point for the model's constructor parameters.
///         </description>
///     </item>
///     <item>
///         <description>
///             to a constructor parameter - mandatory. It defines which HTML node corresponds
///             to the parameter.
///         </description>
///     </item>
/// </list>
/// </summary>
/// <remarks>
/// You can create a custom attribute that inherits from <c>AtAttribute</c>.
/// </remarks>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Parameter, AllowMultiple = false)]
public class AtAttribute : Attribute
{
    public string XPath { get; }

    public AtAttribute(string xpath) => XPath = xpath;
}
