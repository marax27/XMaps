namespace XMaps.UnitTests.Basics.HappyPath;

public class TrivialModelWithParameterlessConstructorTests
{
    private const string GivenHtml = """
    <html>
        <body>
            <div id="div1">
                <span id="span1">Hello</span>
            </div>
        </body>
    </html>
    """;

    [Fact]
    public void GivenRecordWithParameterlessConstructorWhenMappingThenMapSuccessfully()
    {
        var result = HtmlMapper.Map<EmptyRecordWithParameterlessConstructor>(GivenHtml);

        result.Should().NotBeNull();
    }

    [Fact]
    public void GivenClassWithParameterlessConstructorWhenMappingThenMapSuccessfully()
    {
        var result = HtmlMapper.Map<EmptyClassWithParameterlessConstructor>(GivenHtml);

        result.Should().NotBeNull();
    }
}

internal record EmptyRecordWithParameterlessConstructor() { }

internal record EmptyClassWithParameterlessConstructor { }
