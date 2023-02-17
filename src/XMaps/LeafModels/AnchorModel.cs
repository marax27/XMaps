using System;

namespace XMaps.LeafModels;

/// <summary>
/// Represents a common <c>&lt;a&gt;</c> element.
/// It assumes the following HTML format: <c>&lt;a href="Href"&gt;InnerText&lt;/a&gt;</c>.
/// </summary>
public class AnchorModel
{
    public string InnerText { get; }
    public string Href { get; }

    public AnchorModel(IHtmlNode node)
        : this(node.InnerText.Trim(), node.HtmlAttributes["href"])
    {
        if (!node.Name.Equals("a", StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException($"Expected '<a>' node, but found '<{node.Name}>'.", nameof(node));
    }

    internal AnchorModel(string innerText, string href)
    {
        InnerText = innerText;
        Href = href;
    }
}
