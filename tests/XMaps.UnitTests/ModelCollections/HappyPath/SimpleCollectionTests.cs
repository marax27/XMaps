namespace XMaps.UnitTests.HappyPath;

public class SimpleCollectionTests
{
    private const string GivenHtml = """
    <html>
        <body>
            <div>
                <p>Some text</p>
                <p>More text</p>
                <p>Last paragraph</p>
            </div>
        </body>
    </html>
    """;

    private readonly string[] _expected = new[]
    {
        "Some text",
        "More text",
        "Last paragraph"
    };

    [Fact]
    public void GivenListPropertyWhenMappingThenMapSuccessfully()
    {
        TestCollectionType<List<string>>();
    }

    [Fact]
    public void GivenIReadOnlyCollectionPropertyWhenMappingThenMapSuccessfully()
    {
        TestCollectionType<IReadOnlyCollection<string>>();
    }

    [Fact]
    public void GivenIEnumerablePropertyWhenMappingThenMapSuccessfully()
    {
        TestCollectionType<IEnumerable<string>>();
    }

    [Fact]
    public void GivenIListPropertyWhenMappingThenMapSuccessfully()
    {
        TestCollectionType<IList<string>>();
    }

    private void TestCollectionType<T>()
    {
        var mapper = new WebpageMapper<SimpleCollectionModel<T>>();

        var result = mapper.Map(GivenHtml);

        result.Paragraphs.Should().BeEquivalentTo(_expected);
    }
}

[At("//body/div")]
internal record SimpleCollectionModel<T>(
    [At("./p")] T Paragraphs
);
