using XMaps.Exceptions;
using XMaps.UnitTests.CustomAtAttribute.HappyPath;

namespace XMaps.UnitTests.CustomAtAttribute.SadPath;

public class MultipleAttributesForOnePropertyTests
{
    private const string GivenHtml = """
    <html>
        <body>
            <div class="main">
                <h1>Header</h1>
            </div>
        </body>
    </html>
    """;

    [Fact]
    public void GivenMultipleAttributesForOnePropertyWhenMappingThenThrowModelDefinitionException()
    {
        var act = () => HtmlMapper.Map<MultipleAttributesForOnePropertyModel>(GivenHtml);

        act.Should().ThrowExactly<ModelDefinitionException>()
            .WithMessage("Parameter 'Header' contains 2 [At] attributes; exactly 1 is expected.");
    }
}

[At("//body")]
record MultipleAttributesForOnePropertyModel(
    [At("./div[@class='main']")]
    [ByClass("h1")]
    string Header
);
