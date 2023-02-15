namespace XMaps.UnitTests.HappyPath;

public class MapDeeplyNestedModelTests
{
    private const string GivenHtml = """
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
            <div class="footer">Some footer</div>
        </body>
    </html>
    """;

    [Fact]
    public void GivenDeeplyNestedModelWhenMappingThenMapPageLevelData()
    {
        var mapper = new WebpageMapper<PageModel>();

        var result = mapper.Map(GivenHtml);

        result.Footer.Should().Be("Some footer");
    }

    [Fact]
    public void GivenDeeplyNestedModelWhenMappingThenMapPanelLevelData()
    {
        var mapper = new WebpageMapper<PageModel>();

        var result = mapper.Map(GivenHtml);

        result.Panel.Header.Should().Be("Some title");
    }

    [Fact]
    public void GivenDeeplyNestedModelWhenMappingThenMapCardLevelData()
    {
        var mapper = new WebpageMapper<PageModel>();

        var result = mapper.Map(GivenHtml);

        result.Panel.Card.Title.Should().Be("Card header");
        result.Panel.Card.Paragraph.Should().Be("Card paragraph");
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
