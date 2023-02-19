using System.IO;
using System.Text;
using HtmlAgilityPack;

namespace XMaps.AgilityPack;

/// <summary>
/// Wrapper on HtmlAgilityPack's parsing ability.
/// </summary>
internal sealed class HtmlAgilityPackParser
{
    public static IHtmlNode Parse(string html)
    {
        var document = new HtmlDocument();
        document.LoadHtml(html);
        return new HtmlAgilityPackNode(document.DocumentNode);
    }

    public static IHtmlNode Parse(Stream htmlStream)
    {
        var document = new HtmlDocument();
        document.Load(htmlStream);
        return new HtmlAgilityPackNode(document.DocumentNode);
    }

    public static IHtmlNode Parse(Stream htmlStream, Encoding encoding)
    {
        var document = new HtmlDocument();
        document.Load(htmlStream, encoding);
        return new HtmlAgilityPackNode(document.DocumentNode);
    }
}
