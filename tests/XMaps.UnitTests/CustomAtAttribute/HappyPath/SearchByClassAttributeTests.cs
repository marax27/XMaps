namespace XMaps.UnitTests.CustomAtAttribute.HappyPath;

public class SearchByClassAttributeTests
{
    private const string GivenHtml = """
    <html>
        <body>
            <h1>Welcome</h1>
            <div class="main">
                <p>Lorem ipsum</p>
            </div>
            <div class="footer">
                <span>Bottom line</span>
            </div>
        </body>
    </html>
    """;

    [Fact]
    public void GivenValidModelWhenMappingThenMapSuccessfully()
    {
        var result = HtmlMapper.Map<SearchByClassModel>(GivenHtml);

        Assert.Multiple(
            () => result.MainContent.Trim().Should().Be("Lorem ipsum"),
            () => result.Footer.Trim().Should().Be("Bottom line")
        );
    }
}

internal sealed class ByClassAttribute : AtAttribute
{
    public ByClassAttribute(string className)
        : base($".//*[@class='{className}']") { }
}

[At("//body")]
internal sealed record SearchByClassModel(
    [ByClass("main")] string MainContent,
    [ByClass("footer")] string Footer
);
