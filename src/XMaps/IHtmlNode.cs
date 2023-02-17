using System.Collections.Generic;

namespace XMaps;

public interface IHtmlNode
{
    string InnerText { get; }
    string Name { get; }

    IReadOnlyDictionary<string, string> HtmlAttributes { get; }

    IHtmlNode? SelectFirstOrDefault(string xpath);
    IEnumerable<IHtmlNode> SelectAll(string xpath);
}
