namespace XMaps.UnitTests.ModelCollections.HappyPath;

public class EmptySimpleCollectionTests
{
    private const string GivenHtml = """
    <html>
        <body>
            <div>
                <h1>No paragraphs here.</h1>
            </div>
        </body>
    </html>
    """;

    [Fact]
    public void GivenSimpleCollectionWhenMappingThenReturnEmptyCollection()
    {
        var result = HtmlMapper.Map<SimpleCollectionModel<List<string>>>(GivenHtml);

        result.Paragraphs.Should().BeEmpty();
    }

    [Fact]
    public void GivenNullableSimpleCollectionWhenMappingThenReturnEmptyCollection()
    {
        var result = HtmlMapper.Map<SimpleCollectionModel<List<string>?>>(GivenHtml);

        result.Paragraphs.Should().BeEmpty();
    }
}
