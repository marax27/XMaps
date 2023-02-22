[![.NET Build and Test](https://github.com/marax27/XMaps/actions/workflows/build-and-test.yml/badge.svg)](https://github.com/marax27/XMaps/actions/workflows/build-and-test.yml)

# About the library

_Declaratively map HTML code to a .NET type._

Transform HTML code into a structured .NET object with just a few lines of code! Whether you're dealing with a simple or a deeply-nested HTML document - simply annotate constructor parameters of your class with XPath expressions, and let XMaps do the rest.

The library relies on [HtmlAgilityPack](https://html-agility-pack.net/). XMaps accesses nodes parsed by HAP, and maps their content/attributes to properties of your class.

## Quick start

```csharp
using XMaps;

var html = @"
<html>
    <body>
        <h1>Welcome!</h1>
        <article>
            <h1>Article title</h1>
            <p>Article content.</p>
        </article>
    </body>
</html>";

var article = HtmlMapper.Map<Article>(html);
Console.WriteLine(article);
// Program output:
// Article { Title = Article title, Paragraph = Article content. }

[At("//article")]
record Article(
    [At("h1")] string Title,
    [At("p")] string Paragraph
);
```

Error handling example:

```csharp
var html = @"
<html>
    <body>
        <h1>Welcome!</h1>
        <article>
            <h1>Article's title</h1>
        </article>
    </body>
</html>";

try
{
    var result = HtmlMapper.Map<Article>(html);
}
catch (MappingException ex)
{
    Console.WriteLine("1) " + ex.Message);
    // 1) Node not found for parameter 'Paragraph' of type 'String'.
}

try
{
    var result = HtmlMapper.Map<ModelWithMissingXPath>(html);
}
catch (ModelDefinitionException ex)
{
    Console.WriteLine("2) " + ex.Message);
    // 2) Parameter 'Paragraph' contains 0 [At] attributes; exactly 1 is expected.
}

[At("//article")]
record Article(
    [At("h1")] string Title,
    [At("p")] string Paragraph
);

[At("//article")]
public record ModelWithMissingXPath(
    [At("h1")] string Title,
    string Paragraph
);
```
