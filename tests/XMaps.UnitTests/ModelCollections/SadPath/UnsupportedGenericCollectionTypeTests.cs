using XMaps.Exceptions;
using XMaps.UnitTests.SadPath;

namespace XMaps.UnitTests.ModelCollections.SadPath;

public class UnsupportedGenericCollectionTypeTests
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
    public void GivenHashSetPropertyWhenMappingThenThrowExpectedException()
    {
        var mapper = new WebpageMapper<SimpleCollectionModel<HashSet<string>>>();

        var act = () => mapper.Map(GivenHtml);

        act.Should().ThrowExactly<CollectionTypeDefinitionException>()
            .WithMessage("Property 'Paragraphs' has an invalid collection type 'HashSet`1'. Use 'List<T>' or an interface implemented by 'List<T>'.");
    }

    [Fact]
    public void GivenHashSetPropertyWhenMappingThenExceptionContainsExpectedValues()
    {
        var mapper = new WebpageMapper<SimpleCollectionModel<HashSet<string>>>();

        var act = () => mapper.Map(GivenHtml);

        var exception = act.Should().ThrowExactly<CollectionTypeDefinitionException>().Which;
        Assert.Multiple(
            () => exception.RootModelType.Should().Be<SimpleCollectionModel<HashSet<string>>>(),
            () => exception.EvaluatedPropertyType.Should().Be<HashSet<string>>()
        );
    }

    [Fact]
    public void GivenISetPropertyWhenMappingThenThrowExpectedException()
    {
        var mapper = new WebpageMapper<SimpleCollectionModel<ISet<string>>>();

        var act = () => mapper.Map(GivenHtml);

        act.Should().ThrowExactly<CollectionTypeDefinitionException>()
            .WithMessage("Property 'Paragraphs' has an invalid collection type 'ISet`1'. Use 'List<T>' or an interface implemented by 'List<T>'.");
    }

    [Fact]
    public void GivenISetPropertyWhenMappingThenExceptionContainsExpectedValues()
    {
        var mapper = new WebpageMapper<SimpleCollectionModel<ISet<string>>>();

        var act = () => mapper.Map(GivenHtml);

        var exception = act.Should().ThrowExactly<CollectionTypeDefinitionException>().Which;
        Assert.Multiple(
            () => exception.RootModelType.Should().Be<SimpleCollectionModel<ISet<string>>>(),
            () => exception.EvaluatedPropertyType.Should().Be<ISet<string>>()
        );
    }
}
