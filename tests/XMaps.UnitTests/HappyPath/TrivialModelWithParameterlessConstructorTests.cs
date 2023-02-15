namespace XMaps.UnitTests.HappyPath;

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
        var model = new WebpageMapper<EmptyRecordWithParameterlessConstructor>();

        var result = model.Map(GivenHtml);

        result.Should().NotBeNull();
    }

    [Fact]
    public void GivenClassWithParameterlessConstructorWhenMappingThenMapSuccessfully()
    {
        var model = new WebpageMapper<EmptyClassWithParameterlessConstructor>();

        var result = model.Map(GivenHtml);

        result.Should().NotBeNull();
    }
}

internal record EmptyRecordWithParameterlessConstructor() { }

internal record EmptyClassWithParameterlessConstructor { }
