using XMaps.Exceptions;

namespace XMaps.UnitTests.Basics.SadPath;

public class InvalidAtAttributeTests
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
    public void GivenParameterWithoutAtAttributeWhenMappingThenThrowExpectedException()
    {
        var act = () => HtmlMapper.Map<MissingAtAttributeModel>(GivenHtml);

        act.Should().ThrowExactly<ModelDefinitionException>()
            .WithMessage("Parameter 'Content' contains 0 [At] attributes; exactly 1 is expected.")
            .Which.RootModelType.Should().Be<MissingAtAttributeModel>();
    }

    [Fact]
    public void GivenParameterWithEmptyXPathWhenMappingThenThrowExpectedException()
    {
        var act = () => HtmlMapper.Map<EmptyXPathModel>(GivenHtml);

        act.Should().ThrowExactly<ModelDefinitionException>()
            .WithMessage("Missing XPath for a parameter 'Title'.")
            .Which.RootModelType.Should().Be<EmptyXPathModel>();
    }

    [Fact]
    public void GivenParameterWithWhitespaceXPathWhenMappingThenThrowExpectedException()
    {
        var act = () => HtmlMapper.Map<WhitespaceXPathModel>(GivenHtml);

        act.Should().ThrowExactly<ModelDefinitionException>()
            .WithMessage("Missing XPath for a parameter 'Content'.")
            .Which.RootModelType.Should().Be<WhitespaceXPathModel>();
    }

    [Fact]
    public void GivenParameterWithNullXPathWhenMappingThenThrowExpectedException()
    {
        var act = () => HtmlMapper.Map<NullXPathModel>(GivenHtml);

        act.Should().ThrowExactly<ModelDefinitionException>()
            .WithMessage("Missing XPath for a parameter 'Footer'.")
            .Which.RootModelType.Should().Be<NullXPathModel>();
    }
}

[At("//article")]
internal record MissingAtAttributeModel(
    [At("./h1")] string Title,
    string Content,
    [At("./footer")] string Footer
);

[At("//article")]
internal record EmptyXPathModel(
    [At("")] string Title,
    [At("./p")] string Content,
    [At("./footer")] string Footer
);

[At("//article")]
internal record WhitespaceXPathModel(
    [At("./h1")] string Title,
    [At("    \r\n")] string Content,
    [At("./footer")] string Footer
);

[At("//article")]
internal record NullXPathModel(
    [At("./h1")] string Title,
    [At("./p")] string Content,
    [At(null!)] string Footer
);
