using System.Collections.Specialized;
using XMaps.Exceptions;

namespace XMaps.UnitTests.SadPath;

public class UnsupportedNonGenericCollectionTypeTests
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

    [Fact]
    public void GivenNonGenericIEnumerablePropertyWhenMappingThenThrowExpectedException()
    {
        var mapper = new WebpageMapper<SimpleCollectionModel<System.Collections.IEnumerable>>();

        var act = () => mapper.Map(GivenHtml);

        act.Should().ThrowExactly<CollectionTypeDefinitionException>()
            .WithMessage("Property 'Paragraphs' has a non-generic collection type 'IEnumerable'. Use 'List<T>' or an interface implemented by 'List<T>'.");
    }

    [Fact]
    public void GivenNonGenericIEnumerablePropertyWhenMappingThenExceptionContainsExpectedValues()
    {
        var mapper = new WebpageMapper<SimpleCollectionModel<System.Collections.IEnumerable>>();

        var act = () => mapper.Map(GivenHtml);

        var exception = act.Should().ThrowExactly<CollectionTypeDefinitionException>().Which;
        Assert.Multiple(
            () => exception.RootModelType.Should().Be<SimpleCollectionModel<System.Collections.IEnumerable>>(),
            () => exception.EvaluatedPropertyType.Should().Be<System.Collections.IEnumerable>()
        );
    }

    [Fact]
    public void GivenStringCollectionPropertyWhenMappingThenThrowExpectedException()
    {
        var mapper = new WebpageMapper<SimpleCollectionModel<StringCollection>>();

        var act = () => mapper.Map(GivenHtml);

        act.Should().ThrowExactly<CollectionTypeDefinitionException>()
            .WithMessage("Property 'Paragraphs' has a non-generic collection type 'StringCollection'. Use 'List<T>' or an interface implemented by 'List<T>'.");
    }

    [Fact]
    public void GivenStringCollectionPropertyWhenMappingThenExceptionContainsExpectedValues()
    {
        var mapper = new WebpageMapper<SimpleCollectionModel<StringCollection>>();

        var act = () => mapper.Map(GivenHtml);

        var exception = act.Should().ThrowExactly<CollectionTypeDefinitionException>().Which;
        Assert.Multiple(
            () => exception.RootModelType.Should().Be<SimpleCollectionModel<StringCollection>>(),
            () => exception.EvaluatedPropertyType.Should().Be<StringCollection>()
        );
    }
}

[At("//body/div")]
internal record SimpleCollectionModel<T>(
    [At("./p")] T Paragraphs
);
