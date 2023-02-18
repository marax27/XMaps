using XMaps.Exceptions;

namespace XMaps.UnitTests.SadPath;

public class NodeNotFoundTests
{
    private const string GivenSampleHtml = """
    <html>
        <body>
            <article>
                <h1>First Article</h1>
                <img src="/img1.png" alt="Image 1"/>
                <p>My Content</p>
                <footer>My Footer</footer>
            </article>
            <article>
                <h1>Second Article</h1>
                <img src="/img2.jpg" alt="Image 2"/>
                <p>My Content</p>
            </article>
        </body>
    </html>
    """;

    [Fact]
    public void GivenModelWithMissingNodeWhenMappingThenThrowExpectedException()
    {
        var mapper = new WebpageMapper<ArticleWithoutFooterModel>();

        var act = () => mapper.Map(GivenSampleHtml);

        var exception = act.Should()
            .ThrowExactly<NodeNotFoundException>()
            .WithMessage("Node not found for parameter 'Footer' of type 'String'.");
    }

    [Fact]
    public void GivenModelWithMissingNodeWhenMappingThenExceptionContainsExpectedData()
    {
        var mapper = new WebpageMapper<ArticleWithoutFooterModel>();

        var act = () => mapper.Map(GivenSampleHtml);

        var exception = act.Should().ThrowExactly<NodeNotFoundException>().Which;
        Assert.Multiple(
            () => exception.RootModelType.Should().Be<ArticleWithoutFooterModel>(),
            () => exception.EvaluatedModelType.Should().Be<ArticleWithoutFooterModel>(),
            () => exception.EvaluatedParameterName.Should().Be("Footer"),
            () => exception.EvaluatedRelativeXPath.Should().Be("footer"),
            () => exception.ParentXPath.Should().Be("//body/article[2]")
        );
    }
}

[At("//body/article[2]")]
internal record ArticleWithoutFooterModel(
    [At("h1")] string Title,
    [At("footer")] string Footer
);
