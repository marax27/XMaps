using System.IO;
using System.Text;
using XMaps.AgilityPack;
using XMaps.Exceptions;
using XMaps.Mappers;

namespace XMaps;

/// <summary>
/// Provides methods for mapping HTML code to a .NET object.
/// </summary>
public static class HtmlMapper
{
    /// <summary>
    /// Maps the input HTML into an object of <c>TModel</c> type.
    /// </summary>
    /// <typeparam name="TModel">Model type. See documentation for model requirements.</typeparam>
    /// <returns><c>TModel</c> object, initialized using its public constructor.</returns>
    /// <exception cref="XMapsException"></exception>
    public static TModel Map<TModel>(string html) where TModel : class
    {
        var mapper = new BasicWebpageMapper<TModel>();
        var rootNode = HtmlAgilityPackParser.Parse(html);
        return mapper.Map(rootNode);
    }

    /// <summary>
    /// Maps the input HTML stream into an object of <c>TModel</c> type.
    /// </summary>
    /// <typeparam name="TModel">Model type. See documentation for model requirements.</typeparam>
    /// <returns><c>TModel</c> object, initialized using its public constructor.</returns>
    /// <exception cref="XMapsException"></exception>
    public static TModel Map<TModel>(Stream stream) where TModel : class
    {
        var mapper = new BasicWebpageMapper<TModel>();
        var rootNode = HtmlAgilityPackParser.Parse(stream);
        return mapper.Map(rootNode);
    }

    /// <summary>
    /// Maps the input HTML stream into an object of <c>TModel</c> type.
    /// </summary>
    /// <typeparam name="TModel">Model type. See documentation for model requirements.</typeparam>
    /// <returns><c>TModel</c> object, initialized using its public constructor.</returns>
    /// <exception cref="XMapsException"></exception>
    public static TModel Map<TModel>(Stream stream, Encoding encoding) where TModel : class
    {
        var mapper = new BasicWebpageMapper<TModel>();
        var rootNode = HtmlAgilityPackParser.Parse(stream, encoding);
        return mapper.Map(rootNode);
    }
}
