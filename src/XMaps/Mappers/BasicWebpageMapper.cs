using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using XMaps.Exceptions;
using XMaps.Reflection;

namespace XMaps.Mappers;

internal sealed class BasicWebpageMapper<TModel> where TModel : class
{
    public TModel Map(IHtmlNode rootNode)
    {
        var rootXPath = TryGetRootXPath();
        var startingNode = GetStartingNode(rootNode, rootXPath);

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

        var isCustomLeafModel = false;

        for (var i = 0; i < constructorArguments.Length; ++i)
        {
            var parameter = constructorParameters[i];

            if (parameter.Type == typeof(IHtmlNode))
            {
                constructorArguments[i] = startingNode;
                isCustomLeafModel = true;
                continue;
            }

            var xpath = GetParameterXPath(parameter);

            var collectionTypeArgument = CheckForCollectionType(parameter, modelType);
            if (collectionTypeArgument is null)
            {
                var descendantNode = startingNode.SelectFirstOrDefault(xpath);
                constructorArguments[i] = EvaluateParameter(descendantNode, parameter, modelType, xpath, parentXPath);
            }
            else
            {
                var descendantNodes = startingNode.SelectAll(xpath).ToArray();
                constructorArguments[i] = EvaluateCollectionParameter(descendantNodes, collectionTypeArgument, parentXPath);
            }
        }

        try
        {
            return ReflectionUtilities.InstantiateDefensively(modelType, constructorArguments, constructorParameters);
        }
        catch (TargetInvocationException ex) when (isCustomLeafModel)
        {
            throw new LeafModelException(
                $"Failed to instantiate the '{modelType.Name}'.", typeof(TModel), ex.InnerException,
                modelType, parentXPath
            );
        }
    }

    private object EvaluateCollectionParameter(IReadOnlyList<IHtmlNode> descendantNodes, Type instanceType, string? parentXPath)
    {
        var listType = typeof(List<>).MakeGenericType(instanceType);
        var list = Activator.CreateInstance(listType)
                   ?? throw new InvalidOperationException($"Failed to instantiate a list of type '{listType.Name}'.");
        var addMethod = listType.GetMethod("Add")
                        ?? throw new InvalidOperationException($"Failed to find 'Add' method on a list of type '{listType.Name}'.");

        foreach (var descendantNode in descendantNodes)
        {
            var value = MapNodeToModel(descendantNode, instanceType, parentXPath);
            addMethod.Invoke(list, new[] { value });
        }

        return list;
    }

    private object? EvaluateParameter(IHtmlNode? descendantNode, ModelConstructorParameter parameter,
        Type modelType, string xpath, string? parentXPath)
    {
        if (descendantNode is not null) return MapNodeToModel(descendantNode, parameter.Type, xpath);

        if (parameter.IsNullable)
            return null;

        throw new Exceptions.NodeNotFoundException(
            $"Node not found for parameter '{parameter.Name}' of type '{parameter.Type.Name}'.",
            typeof(TModel),
            modelType,
            parameter.Name,
            xpath,
            parentXPath
        );
    }

    private static IHtmlNode GetStartingNode(IHtmlNode rootNode, string? rootXPath)
    {
        var startingNode = rootXPath is null
            ? rootNode
            : rootNode.SelectFirstOrDefault(rootXPath);

        if (startingNode is null)
        {
            var rootType = typeof(TModel);
            throw new StartingNodeNotFoundException($"Starting node not found for type '{rootType.Name}' (XPath: '{rootXPath}').", rootType, rootXPath);
        }

        return startingNode;
    }

    private static Type? CheckForCollectionType(ModelConstructorParameter parameter, Type modelType)
    {
        // Special case: string is a collection type (implements IEnumerable<char>),
        // but we want to map a single node to a string.
        if (parameter.Type == typeof(string))
            return null;

        var isCollectionType = parameter.Type.IsAssignableTo(typeof(System.Collections.IEnumerable));
        if (!isCollectionType)
            return null;

        if (!parameter.Type.IsGenericType)
        {
            throw new CollectionTypeDefinitionException(
                $"Property '{parameter.Name}' has a non-generic collection type '{parameter.Type.Name}'. Use 'List<T>' or an interface implemented by 'List<T>'.",
                typeof(TModel),
                parameter.Type,
                modelType
            );
        }

        var genericArgument = parameter.Type.GetGenericArguments()[0];

        var listType = typeof(List<>).MakeGenericType(genericArgument);
        var canAssignList = parameter.Type.IsAssignableFrom(listType);
        if (!canAssignList)
        {
            throw new CollectionTypeDefinitionException(
                $"Property '{parameter.Name}' has an invalid collection type '{parameter.Type.Name}'. Use 'List<T>' or an interface implemented by 'List<T>'.",
                typeof(TModel),
                parameter.Type,
                modelType
            );
        }

        return genericArgument;
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
