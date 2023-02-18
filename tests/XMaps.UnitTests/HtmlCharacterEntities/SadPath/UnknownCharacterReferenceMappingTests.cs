namespace XMaps.UnitTests.HtmlCharacterEntities.SadPath;

public class UnknownCharacterReferenceMappingTests
{
    private const string GivenHtml = """
    <html>
        <body>
            <div id="div1">
                <p>&apos;Cited text&apos; and a character &invalid;</p>
            </div>
        </body>
    </html>
    """;

    [Fact]
    public void GivenInvalidCharacterEntityInHtmlWhenMappingThenMapSuccessfully()
    {
        var mapper = new WebpageMapper<UnknownCharacterReferenceModel>();

        var result = mapper.Map(GivenHtml);

        result.Paragraph.Should().Be("'Cited text' and a character &invalid;");
    }
}

[At("//div[@id='div1']")]
internal record UnknownCharacterReferenceModel([At("./p")] string Paragraph);
