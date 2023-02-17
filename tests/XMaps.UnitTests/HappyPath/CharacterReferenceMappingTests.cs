namespace XMaps.UnitTests.HappyPath;

public class CharacterReferenceMappingTests
{
    private const string GivenHtml = """
    <html>
        <body>
            <p>Some text with a &amp; character</p>
        </body>
    </html>
    """;

    [Fact]
    public void GivenHtmlWithCharacterReferencesWhenMappingThenConvertCharacter()
    {
        var mapper = new WebpageMapper<CharacterReferenceMappingModel>();

        var result = mapper.Map(GivenHtml);

        result.Text.Should().Be("Some text with a & character");
    }
}

[At("//body")]
internal record CharacterReferenceMappingModel([At("./p")] string Text);
