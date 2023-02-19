using XMaps.Exceptions;
using XMaps.LeafModels;

namespace XMaps.UnitTests.SadPath.LeafModels;

public class NonAnchorNodeMappedToAnchorModelTests
{
    private const string GivenHtml = """
    <html>
        <body>
            <nav>
                <h1 href="/home">Home</h1> <!-- This is not a valid anchor element -->
            </nav>
            <article>
                <h2>Heading</h2>
                <p>Some text.</p>
            </article>
        </body>
    </html>
    """;

    [Fact]
    public void GivenModelWithAnchorPropertyWhenMappingNonAnchorNodeThenThrowExpectedException()
    {
        var act = () => HtmlMapper.Map<NonAnchorNodeMappedToAnchorModel>(GivenHtml);

        act.Should().ThrowExactly<LeafModelException>()
            .WithMessage("Failed to instantiate the 'AnchorModel'. Expected '<a>' node, but found '<h1>'. (Parameter 'node')");
    }

    [Fact]
    public void GivenModelWithAnchorPropertyWhenMappingNonAnchorNodeThenExceptionContainsExpectedValues()
    {
        var act = () => HtmlMapper.Map<NonAnchorNodeMappedToAnchorModel>(GivenHtml);

        var exception = act.Should().ThrowExactly<LeafModelException>().Which;
        Assert.Multiple(
            () => exception.RootModelType.Should().Be<NonAnchorNodeMappedToAnchorModel>(),
            () => exception.EvaluatedModelType.Should().Be<AnchorModel>(),
            () => exception.XPath.Should().Be(".//h1")
        );
    }
}

[At("//body")]
internal record NonAnchorNodeMappedToAnchorModel(
    [At(".//h1")] AnchorModel Anchor
);
