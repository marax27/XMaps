using XMaps.Exceptions;

namespace XMaps.UnitTests.Basics.SadPath;

public class ModelConstructorTests
{
    private const string GivenHtml = """
    <html>
        <body>
            <article>
                <h1>Some title</h1>
                <p>Some paragraph</p>
                <h2>Some header</h2>
                <footer>Some footer</footer>
            </article>
        </body>
    </html>
    """;

    [Fact]
    public void GivenModelWithMultiplePublicConstructorsThenThrowExpectedException()
    {
        var mapper = new WebpageMapper<MultipleConstructorsModel>();

        var act = () => mapper.Map(GivenHtml);

        act.Should().ThrowExactly<ModelConstructorException>()
            .WithMessage("Ambiguous model construction for 'MultipleConstructorsModel': found 3 public constructors, expected 1.");
    }

    [Fact]
    public void GivenModelWithNoPublicConstructorsThenThrowExpectedException()
    {
        var mapper = new WebpageMapper<NoPublicConstructorModel>();

        var act = () => mapper.Map(GivenHtml);

        act.Should().ThrowExactly<ModelConstructorException>()
            .WithMessage("Failed to find a public constructor of type 'NoPublicConstructorModel'.");
    }
}

internal class MultipleConstructorsModel
{
    public MultipleConstructorsModel([At("//p")] string paragraph) { }

    public MultipleConstructorsModel([At("//p")] string paragraph, [At("//h2")] string header) { }

    public MultipleConstructorsModel([At("//p")] string paragraph, [At("//h2")] string header, [At("//footer")] string footer) { }
}

internal class NoPublicConstructorModel
{
    internal NoPublicConstructorModel() { }
}
