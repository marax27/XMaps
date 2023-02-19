using XMaps.Exceptions;

namespace XMaps.UnitTests.Basics.SadPath;

public class StartingNodeNotFoundTests
{
    private const string GivenHtml = """
    <body>
        <article>
            <h1>My Title</h1>
            <p>My Content</p>
            <footer>My Footer</footer>
        </article>
    </body>
    """;

    [Fact]
    public void GivenStartingNodeNotFoundWhenMappingThenThrowExpectedException()
    {
        var act = () => HtmlMapper.Map<StartingNodeNotFoundModel>(GivenHtml);

        act.Should().ThrowExactly<StartingNodeNotFoundException>()
            .WithMessage("Starting node not found for type 'StartingNodeNotFoundModel' (XPath: '//div[@class='main']').");
    }

    [Fact]
    public void GivenStartingNodeNotFoundWhenMappingThenExceptionContainsExpectedValues()
    {
        var act = () => HtmlMapper.Map<StartingNodeNotFoundModel>(GivenHtml);

        var exception = act.Should().ThrowExactly<StartingNodeNotFoundException>().Which;
        Assert.Multiple(
            () => exception.RootModelType.Should().Be(typeof(StartingNodeNotFoundModel)),
            () => exception.StartingXPath.Should().Be("//div[@class='main']")
        );
    }
}

[At("//div[@class='main']")]
internal record StartingNodeNotFoundModel(
    [At("./h1")] string Title
);
