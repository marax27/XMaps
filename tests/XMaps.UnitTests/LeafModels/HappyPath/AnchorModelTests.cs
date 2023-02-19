using XMaps.LeafModels;

namespace XMaps.UnitTests.LeafModels.HappyPath;

public class AnchorModelTests
{
    private const string GivenHtml = """
    <html>
        <body>
            <nav>
                <a href='/'>Home</a>
                <a href='/about'>About</a>
            </nav>
            <h1>
                <a href='#'>Heading link</a>
            </h1>
            <p>
                Some text, and a <a class='highlighted' href='/about'>highlighted link</a>.
            </p>
        </body>
    </html>
    """;

    [Fact]
    public void GivenModelWithAnchorPropertyWhenMappingThenMapSuccessfully()
    {
        var act = () => HtmlMapper.Map<ModelWithAnchorProperty>(GivenHtml);

        act.Should().NotThrow();
    }

    [Fact]
    public void GivenModelWithAnchorPropertyWhenMappingThenContainExpectedValues()
    {
        var result = HtmlMapper.Map<ModelWithAnchorProperty>(GivenHtml);

        result.Anchor.Should().BeEquivalentTo(new AnchorModel("highlighted link", "/about"));
    }

    [Fact]
    public void GivenModelWithAnchorCollectionPropertyWhenMappingThenMapSuccessfully()
    {
        var act = () => HtmlMapper.Map<ModelWithAnchorCollectionProperty>(GivenHtml);

        act.Should().NotThrow();
    }

    [Fact]
    public void GivenModelWithAnchorCollectionPropertyWhenMappingThenContainExpectedValues()
    {
        var result = HtmlMapper.Map<ModelWithAnchorCollectionProperty>(GivenHtml);

        Assert.Multiple(
            () => result.Anchors[0].Should().BeEquivalentTo(new AnchorModel("Home", "/")),
            () => result.Anchors[1].Should().BeEquivalentTo(new AnchorModel("About", "/about"))
        );
    }
}

[At("//body")]
internal record ModelWithAnchorProperty(
    [At(".//a[@class='highlighted']")] AnchorModel Anchor
);

[At("//body")]
internal record ModelWithAnchorCollectionProperty(
    [At("./nav/a")] List<AnchorModel> Anchors
);
