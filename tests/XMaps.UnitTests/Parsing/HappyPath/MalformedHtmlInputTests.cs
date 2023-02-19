using System.Text;

namespace XMaps.UnitTests.Parsing.HappyPath;

public class MalformedHtmlInputTests
{
    private readonly byte[] _givenMalformedHtml = { 137, 80, 78, 71, 13, 10, 26, 10 };

    [Fact]
    public void GivenMalformedHtmlInputWhenMappingThenDoNotThrow()
    {
        var givenStream = new MemoryStream(_givenMalformedHtml);

        var act = () => HtmlMapper.Map<MalformedHtmlInputModel>(givenStream);

        act.Should().NotThrow();
    }

    [Fact]
    public void GivenMalformedHtmlInputWithExplicitEncodingWhenMappingThenDoNotThrow()
    {
        var givenStream = new MemoryStream(_givenMalformedHtml);

        var act = () => HtmlMapper.Map<MalformedHtmlInputModel>(givenStream, Encoding.Latin1);

        act.Should().NotThrow();
    }
}

internal record MalformedHtmlInputModel;
