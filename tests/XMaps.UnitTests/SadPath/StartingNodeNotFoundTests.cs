using XMaps.Exceptions;

namespace XMaps.UnitTests.SadPath;

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
        var mapper = new WebpageMapper<StartingNodeNotFoundModel>();

        var act = () => mapper.Map(GivenHtml);

        act.Should().ThrowExactly<StartingNodeNotFoundException>()
            .WithMessage("Starting node not found for type 'StartingNodeNotFoundModel' (XPath: '//div[@class='main']').");
    }

    [Fact]
    public void GivenStartingNodeNotFoundWhenMappingThenExceptionContainsExpectedValues()
    {
        var mapper = new WebpageMapper<StartingNodeNotFoundModel>();

        var exception = Assert.Throws<StartingNodeNotFoundException>(() => mapper.Map(GivenHtml));

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
