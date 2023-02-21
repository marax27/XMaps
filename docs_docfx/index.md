# XMaps Homepage

_Declaratively map HTML code to a .NET type._

Transform HTML code into a structured .NET object with just a few lines of code! Whether you're dealing with a simple or a deeply-nested HTML document - simply annotate constructor parameters of your class with XPath expressions, and let XMaps do the rest.

## Getting started

XMaps requires two things to work: **input HTML** and a **model type**. The model can be a regular class or a [record](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/record) (C# 9+). In fact, XMaps takes advantage of the new record syntax to make mapping even more concise. Here's an example of a model record:

```php
[At("//article")]                                 /* Entry node: first <article> node in the document */
record ArticleModel(
    [At("h1")] string Title,                      /* map first <h1> child of <article> */
    [At("span[@class='author']")] string Author,  /* map first <span class='author'> child of <article> */
    [At("span[@class='date']")] string Date       /* map first <span class='date'> child of <article> */
);
```

which could be used like this:

```csharp
using XMaps;

var html = @"
<html>
  <body>
    <article>
      <h1>Welcome!</h1>
      <span class=""author"">John Doe</span>
      <span class=""date"">2021-01-01</span>
    </article>
  </body>
</html>";

var article = HtmlMapper.Map<ArticleModel>(html);
Console.WriteLine(article);
```

Keep in mind that:

- Model's `string` properties will be automatically initialized with an [inner text](https://developer.mozilla.org/en-US/docs/Web/API/HTMLElement/innerText) of a matching node.
- If a node cannot be found for a non-nullable property, a [NodeNotFoundException](/api/XMaps.Exceptions.NodeNotFoundException.html) will be thrown.

## Model definition

The `string` type isn't the only type that can be used. Model's property can be: a nullable type, a collection of models or another model. See below:

```php
[At("//article")]
record ArticleModel(
    [At("h1")] string Title,                       /* mandatory - will throw if the node is not found */
    [At("span[@class='author']")] string? Author,  /* optional - null if the node is not found */
    [At("p")] List<string> Paragraphs,             /* empty if no nodes are found */
);
```

The above model could be used to map the following HTML:

```html
<html>
  <body>
    <article>
      <h1>Welcome!</h1>
      <!-- <span class="author">John Doe</span> -->
      <p>Lorem ipsum dolor sit amet...</p>
      <p>Nec nam aliquam sem et tortor...</p>
      <p>In nulla posuere sollicitudin aliquam...</p>
    </article>
  </body>
</html>
```
