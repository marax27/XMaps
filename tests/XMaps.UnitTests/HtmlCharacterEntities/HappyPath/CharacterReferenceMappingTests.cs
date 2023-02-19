namespace XMaps.UnitTests.HtmlCharacterEntities.HappyPath;

public class CharacterReferenceMappingTests
{
    private const string GivenHtml = """
    <html>
        <body>
            <p>Some text with a &amp; character</p>
            <span>Less than sign (&#60;)</span>
        </body>
    </html>
    """;

    [Fact]
    public void GivenHtmlWithNamedCharacterEntityWhenMappingThenConvertCharacter()
    {
        var result = HtmlMapper.Map<CharacterReferenceMappingModel>(GivenHtml);

        result.Paragraph.Should().Be("Some text with a & character");
    }

    [Fact]
    public void GivenHtmlWithUnnamedCharacterEntityWhenMappingThenConvertCharacter()
    {
        var result = HtmlMapper.Map<CharacterReferenceMappingModel>(GivenHtml);

        result.Span.Should().Be("Less than sign (<)");
    }
}

[At("//body")]
internal record CharacterReferenceMappingModel(
    [At("./p")] string Paragraph,
    [At("./span")] string Span
);
