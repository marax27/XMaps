﻿using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using XMaps.Exceptions;
using XMaps.Reflection;

namespace XMaps;

internal interface IHtmlNode
{
    string InnerText { get; }

    IHtmlNode? SelectFirstOrDefault(string xpath);
    IEnumerable<IHtmlNode> SelectAll(string xpath);
}

internal sealed class HtmlAgilityPackNode : IHtmlNode
{
    private readonly HtmlNode _node;

    internal HtmlAgilityPackNode(HtmlNode node)
    {
        _node = node ?? throw new ArgumentNullException(nameof(node));
    }

    public string InnerText => _node.InnerText;

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

public sealed class WebpageMapper<TModel> where TModel : class
{
    /// <summary>
    ///     Map an HTML document into an instance of <c>TModel</c>.
    /// </summary>
    /// <remarks><c>TModel</c> must be a valid model type.</remarks>
    /// <param name="givenHtml">HTML in text format.</param>
    /// <returns>Initialized instance of <c>TModel</c>.</returns>
    /// <exception cref="XMapsException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public TModel Map(string givenHtml)
    {
        var document = new HtmlDocument();
        document.LoadHtml(givenHtml);
        return MapDocument(document);
    }

    private TModel MapDocument(HtmlDocument document)
    {
        var rootXPath = TryGetRootXPath();
        var startingNode = GetStartingNode(document, rootXPath);

        var result = MapNodeToModel(startingNode, typeof(TModel), rootXPath);

        return result as TModel
               ?? throw new InvalidOperationException($"Failed to instantiate the '{typeof(TModel).Name}'.");
    }

    private object MapNodeToModel(IHtmlNode startingNode, Type modelType, string? parentXPath)
    {
        if (modelType == typeof(string))
            return startingNode.InnerText;

        var constructorParameters = ReflectionUtilities.GetModelConstructorParameters(modelType, typeof(TModel));
        var constructorArguments = new object?[constructorParameters.Count];

        for (var i = 0; i < constructorArguments.Length; ++i)
        {
            var parameter = constructorParameters[i];
            var xpath = GetParameterXPath(parameter);

            var descendantNode = startingNode.SelectFirstOrDefault(xpath);
            if (descendantNode is null)
            {
                throw new Exceptions.NodeNotFoundException(
                    $"Node not found for parameter '{parameter.Name}' of type '{parameter.Type.Name}'.",
                    typeof(TModel),
                    modelType,
                    parameter.Name,
                    xpath,
                    parentXPath
                );
            }

            var value = MapNodeToModel(descendantNode, parameter.Type, xpath);

            constructorArguments[i] = value;
        }

        return ReflectionUtilities.InstantiateDefensively(modelType, constructorArguments, constructorParameters);
    }

    private static IHtmlNode GetStartingNode(HtmlDocument document, string? rootXPath)
    {
        var startingNode = rootXPath is null
            ? document.DocumentNode
            : document.DocumentNode.SelectSingleNode(rootXPath);

        if (startingNode is null)
        {
            var rootType = typeof(TModel);
            throw new StartingNodeNotFoundException($"Starting node not found for type '{rootType.Name}' (XPath: '{rootXPath}').", rootType, rootXPath);
        }

        return new HtmlAgilityPackNode(startingNode);
    }

    private static string? TryGetRootXPath()
    {
        var result = ReflectionUtilities
            .GetCustomAttributes<TModel, AtAttribute>()
            .SingleOrDefault();
        return result?.XPath;
    }

    private static string GetParameterXPath(ModelConstructorParameter parameter)
    {
        var attributes = parameter.CustomAttributes
            .GetAll<AtAttribute>()
            .ToArray();

        if (attributes.Length != 1)
        {
            throw new ModelDefinitionException(
                $"Parameter '{parameter.Name}' contains {attributes.Length} [At] attributes; exactly 1 is expected.",
                typeof(TModel));
        }

        var result = attributes[0].XPath;
        return string.IsNullOrWhiteSpace(result)
            ? throw new ModelDefinitionException($"Missing XPath for a parameter '{parameter.Name}'.", typeof(TModel))
            : result;
    }
}
