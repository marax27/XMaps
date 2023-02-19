namespace XMaps.UnitTests.ModelCollections.HappyPath;

public class NestedCollectionTypePropertyTests
{
    private const string GivenHtml = """
    <html>
        <body>
            <div class="site">
                <div class="card main-card">
                    <h1>Main header</h1>
                    <p>Main card description.</p>
                </div>

                <div class="row">
                    <div class="card">
                        <h1>Card header</h1>
                        <p>Sample text.</p>
                        <p>Second paragraph.</p>
                        <p>Third paragraph.</p>
                    </div>

                    <div class="card">
                        <h1>Last header</h1>
                        <p>Example text.</p>
                    </div>
                </div>
            </div>
        </body>
    </html>
    """;

    [Fact]
    public void GivenCollectionPropertyInNestedModelWhenMappingThenHasExpectedNumberOfCards()
    {
        var result = HtmlMapper.Map<NestedCollectionModel>(GivenHtml);

        result.Cards.Should().HaveCount(3);
    }

    [Fact]
    public void GivenCollectionPropertyInNestedModelWhenMappingThenCardsHaveExpectedValues()
    {
        var result = HtmlMapper.Map<NestedCollectionModel>(GivenHtml);

        result.Cards.Should().BeEquivalentTo(new NestedCollectionCardModel[]
        {
            new ("Main header", new []{ "Main card description." }),
            new ("Card header", new []
            {
                "Sample text.",
                "Second paragraph.",
                "Third paragraph."
            }),
            new ("Last header", new []{ "Example text." })
        });
    }
}

internal record NestedCollectionCardModel(
    [At("h1")] string Header,
    [At("p")] IReadOnlyCollection<string> Paragraphs
);

[At("//div[@class='site']")]
internal record NestedCollectionModel(
    [At(".//div[contains(@class, 'card')]")] IReadOnlyList<NestedCollectionCardModel> Cards
);
