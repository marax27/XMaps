namespace XMaps.UnitTests.Basics.HappyPath;

public class ModelWithOnePublicConstructorTests
{
    private const string GivenHtml = """
    <html>
        <body>
            <article>
                <a href="#">Navigation link</a>
                <span>Text</span>
                <span class="label">Label under nav link.</span>
            </article>
        </body>
    </html>
    """;

    [Fact]
    public void GivenRecordWithOnePublicConstructorWhenMappingThenMapSuccessfully()
    {
        var mapper = new WebpageMapper<RecordWithNonPublicConstructors>();

        var result = mapper.Map(GivenHtml);

        result.Should().BeEquivalentTo(new RecordWithNonPublicConstructors("Navigation link", "Label under nav link."));
    }

    [Fact]
    public void GivenClassWithOnePublicConstructorWhenMappingThenMapSuccessfully()
    {
        var mapper = new WebpageMapper<ClassWithNonPublicConstructors>();

        var result = mapper.Map(GivenHtml);

        result.Should().BeEquivalentTo(new RecordWithNonPublicConstructors("Navigation link", "Label under nav link."));
    }
}

[At("//article")]
internal record RecordWithNonPublicConstructors(
    [At("./a")] string NavigationLink,
    [At("./span[@class='label']")] string Label
)
{
    protected RecordWithNonPublicConstructors() : this("") { }

    private RecordWithNonPublicConstructors(string text) : this(text, text) { }
}

[At("//article")]
public class ClassWithNonPublicConstructors
{
    public string NavigationLink { get; }
    public string Label { get; }

    public ClassWithNonPublicConstructors([At("./a")] string navigationLink, [At("./span[@class='label']")] string label)
    {
        NavigationLink = navigationLink;
        Label = label;
    }

    internal ClassWithNonPublicConstructors() : this("navigation", "label") { }

    protected ClassWithNonPublicConstructors(string x) : this(x, x) { }
}
