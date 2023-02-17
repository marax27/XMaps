# XMaps Design Decisions

## About

XMaps library can be described as the **HTML document mapper**. XMaps relies on a pre-existing HTML parser in order to construct a **model** based on HTML nodes.

## Model requirements

A model is any type whose instance can be created by an XMaps mapper.

The main requirement for a model type is **to have exactly one public constructor.** A trivial example of a valid model would be a class with an empty public constructor.

Hierarchy of model types:

1. **Leaf models** - they do not require other model instances to be created.
   - `String` - by default, XMaps will supply a node's inner text into a `string` parameter.
   - `public new(IHtmlNode)` - a class with a **single public constructor** which takes a **single `IHtmlNode` parameter.**
   - `public new()` - a special case.
   - (?) `ILeafModel` - utilize static virtual methods from .NET 7.
2. **Composite models** - the mapper must recursively instantiate sub-models in order to create an instance of the composite model.
   - `public new(Model1 model1, ..., ModelN modelN)` - a class with a **single public constructor,** whose **all parameters are valid models**.

## Handling nodes that were not found

- `T PropertyName` - if a node is not found, then the mapper throws an exception.
- `T? PropertyName` - if a node is not found, then the result model property is `null`.

## Implicit transformation of character references

`&amp; -> &` etc.

## Targeting .NET 6

XMaps targets the latest LTS version of the .NET framework.

## Preview features

XMaps strives for minimalism and stability. However, some preview features may be incorporated.
