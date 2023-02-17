namespace XMaps.UnitTests.HappyPath;

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
        var mapper = new WebpageMapper<SimpleCollectionModel<List<string>>>();

        var result = mapper.Map(GivenHtml);

        result.Paragraphs.Should().BeEmpty();
    }

    [Fact]
    public void GivenNullableSimpleCollectionWhenMappingThenReturnEmptyCollection()
    {
        var mapper = new WebpageMapper<SimpleCollectionModel<List<string>?>>();

        var result = mapper.Map(GivenHtml);

        result.Paragraphs.Should().BeEmpty();
    }
}
