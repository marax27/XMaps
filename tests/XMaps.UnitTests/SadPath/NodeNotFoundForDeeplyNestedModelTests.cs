using XMaps.Exceptions;

namespace XMaps.UnitTests.SadPath;

public class NodeNotFoundForDeeplyNestedModelTests
{
    private const string GivenMissingPageFooterHtml = """
    <html>
        <body>
            <div class="panel">
                <h1>Some title</h1>
                <img src="/assets/image.png" />
                <div class="card">
                    <h2>Card header</h2>
                    <p>Card paragraph</p>
                </div>
            </div>
        </body>
    </html>
    """;

    private const string GivenMissingPanelHeaderHtml = """
    <html>
        <body>
            <div class="panel">
                <img src="/assets/image.png" />
                <div class="card">
                    <h2>Card header</h2>
                    <p>Card paragraph</p>
                </div>
            </div>
            <div class="footer">Some footer</div>
        </body>
    </html>
    """;

    private const string GivenMissingCardParagraphHtml = """
    <html>
        <body>
            <div class="panel">
                <h1>Some title</h1>
                <img src="/assets/image.png" />
                <div class="card">
                    <h2>Card header</h2>
                </div>
            </div>
            <div class="footer">Some footer</div>
        </body>
    </html>
    """;

    [Fact]
    public void GivenMissingFirstLevelNodeWhenMappingThenThrowExpectedException()
    {
        var mapper = new WebpageMapper<PageModel>();

        var act = () => mapper.Map(GivenMissingPageFooterHtml);

        act.Should()
            .ThrowExactly<NodeNotFoundException>()
            .WithMessage("Node not found for parameter 'Footer' of type 'String'.");
    }

    [Fact]
    public void GivenMissingFirstLevelNodeWhenMappingThenExceptionContainsExpectedData()
    {
        var mapper = new WebpageMapper<PageModel>();

        var act = () => mapper.Map(GivenMissingPageFooterHtml);

        var exception = act.Should().ThrowExactly<NodeNotFoundException>().Which;
        Assert.Multiple(
            () => exception.RootModelType.Should().Be<PageModel>(),
            () => exception.EvaluatedModelType.Should().Be<PageModel>(),
            () => exception.EvaluatedParameterName.Should().Be("Footer"),
            () => exception.EvaluatedRelativeXPath.Should().Be("./div[@class='footer']"),
            () => exception.ParentXPath.Should().Be("//body")
        );
    }

    [Fact]
    public void GivenMissingSecondLevelNodeWhenMappingThenThrowExpectedException()
    {
        var mapper = new WebpageMapper<PageModel>();

        var act = () => mapper.Map(GivenMissingPanelHeaderHtml);

        act.Should()
            .ThrowExactly<NodeNotFoundException>()
            .WithMessage("Node not found for parameter 'Header' of type 'String'.");
    }

    [Fact]
    public void GivenMissingSecondLevelNodeWhenMappingThenExceptionContainsExpectedData()
    {
        var mapper = new WebpageMapper<PageModel>();

        var act = () => mapper.Map(GivenMissingPanelHeaderHtml);

        var exception = act.Should().ThrowExactly<NodeNotFoundException>().Which;
        Assert.Multiple(
            () => exception.RootModelType.Should().Be<PageModel>(),
            () => exception.EvaluatedModelType.Should().Be<PanelModel>(),
            () => exception.EvaluatedParameterName.Should().Be("Header"),
            () => exception.EvaluatedRelativeXPath.Should().Be("./h1"),
            () => exception.ParentXPath.Should().Be("./div[@class='panel']")
        );
    }

    [Fact]
    public void GivenMissingThirdLevelNodeWhenMappingThenThrowExpectedException()
    {
        var mapper = new WebpageMapper<PageModel>();

        var act = () => mapper.Map(GivenMissingCardParagraphHtml);

        act.Should()
            .ThrowExactly<NodeNotFoundException>()
            .WithMessage("Node not found for parameter 'Paragraph' of type 'String'.");
    }

    [Fact]
    public void GivenMissingThirdLevelNodeWhenMappingThenExceptionContainsExpectedData()
    {
        var mapper = new WebpageMapper<PageModel>();

        var act = () => mapper.Map(GivenMissingCardParagraphHtml);

        var exception = act.Should().ThrowExactly<NodeNotFoundException>().Which;
        Assert.Multiple(
            () => exception.RootModelType.Should().Be<PageModel>(),
            () => exception.EvaluatedModelType.Should().Be<CardModel>(),
            () => exception.EvaluatedParameterName.Should().Be("Paragraph"),
            () => exception.EvaluatedRelativeXPath.Should().Be("./p"),
            () => exception.ParentXPath.Should().Be("./div[@class='card']")
        );
    }
}

internal record CardModel(
    [At("./h2")] string Title,
    [At("./p")] string Paragraph
);

internal record PanelModel(
    [At("./h1")] string Header,
    [At("./div[@class='card']")] CardModel Card
);

[At("//body")]
internal record PageModel(
    [At("./div[@class='panel']")] PanelModel Panel,
    [At("./div[@class='footer']")] string Footer
);
