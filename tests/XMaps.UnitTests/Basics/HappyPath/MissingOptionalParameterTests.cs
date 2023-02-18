namespace XMaps.UnitTests.Basics.HappyPath;

public class MissingOptionalParameterTests
{
    private const string GivenHtmlWithSubHeader = """
    <html>
        <body>
            <h1>Some title</h1>
            <h2>Some header</h2>
        </body>
    </html>
    """;

    private const string GivenHtmlWithoutSubHeader = """
    <html>
        <body>
            <h1>Some title</h1>
        </body>
    </html>
    """;

    [Theory]
    [InlineData(GivenHtmlWithSubHeader, "Some header")]
    [InlineData(GivenHtmlWithoutSubHeader, null)]
    public void GivenModelWithOptionalParameterWhenMappingThenMapSuccessfully(string givenHtml, string? expectedSubHeader)
    {
        var expected = new ModelWithNullableField("Some title", expectedSubHeader);
        var mapper = new WebpageMapper<ModelWithNullableField>();

        var actual = mapper.Map(givenHtml);

        actual.Should().BeEquivalentTo(expected);
    }
}

[At("//body")]
internal record ModelWithNullableField(
    [At("./h1")] string Header,
    [At("./h2")] string? OptionalSubHeader
);
