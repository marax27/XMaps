namespace XMaps.UnitTests.HappyPath;

public class MappingToStringTests
{
    private const string GivenHtml = """
    <html>
        <head></head>
        <body>
            <header>
                <svg>Logo</svg>
                <p>Title</p>
                <p>About</p>
                <p>Contact</p>
            </header>
            <div id="main">
                <h1>Welcome!</h1>
                <p>Lorem ipsum</p>
            </div>
            <script src="/script.js"></script>
        </body>
    </html>
    """;

    [Fact]
    public void GivenSimpleTestModelWhenMappingInnerTextToStringThenContainsExpectedValues()
    {
        var mapper = new WebpageMapper<SimpleTestModel>();

        var result = mapper.Map(GivenHtml);

        result.Should()
            .BeEquivalentTo(new SimpleTestModel("Welcome!", "Lorem ipsum"));
    }
}

[At("//div[@id='main']")]
internal record SimpleTestModel(
    [At("h1")] string Header,
    [At("./p")] string Paragraph
);
