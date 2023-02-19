using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

namespace XMaps.AgilityPack;

internal sealed class HtmlAgilityPackNode : IHtmlNode
{
    private readonly HtmlNode _node;

    internal HtmlAgilityPackNode(HtmlNode node)
    {
        _node = node ?? throw new ArgumentNullException(nameof(node));
        HtmlAttributes = _node.Attributes
            .ToDictionary(x => x.Name, x => x.Value);
    }

    public string InnerText => HtmlEntity.DeEntitize(_node.InnerText);

    public string Name => _node.Name;

    public IReadOnlyDictionary<string, string> HtmlAttributes { get; }

    public IHtmlNode? SelectFirstOrDefault(string xpath)
    {
        var node = _node.SelectSingleNode(xpath);
        return node is null
            ? null
            : new HtmlAgilityPackNode(node);
    }

    public IEnumerable<IHtmlNode> SelectAll(string xpath)
    {
        var nodes = _node.SelectNodes(xpath);

        if (nodes is null) yield break;

        foreach (var node in nodes)
            yield return new HtmlAgilityPackNode(node);
    }
}
