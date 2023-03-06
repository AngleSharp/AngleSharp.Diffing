namespace AngleSharp.Diffing.Strategies.TextNodeStrategies;

public class TextNodeFilterTest : TextNodeTestBase
{
    public TextNodeFilterTest(DiffingTestFixture fixture) : base(fixture)
    {
    }

    [Theory(DisplayName = "When whitespace option is Preserve, the provided decision is not changed by the filter for whitespace only text nodes")]
    [MemberData(nameof(WhitespaceCharStrings))]
    public void Test1(string whitespace)
    {
        var sut = new TextNodeFilter(WhitespaceOption.Preserve);
        var source = ToComparisonSource(whitespace);

        sut.Filter(source, FilterDecision.Keep).ShouldBe(FilterDecision.Keep);
        sut.Filter(source, FilterDecision.Exclude).ShouldBe(FilterDecision.Exclude);
    }

    [Theory(DisplayName = "When whitespace option is RemoveWhitespaceNodes, whitespace only text nodes are excluded during filtering")]
    [MemberData(nameof(WhitespaceCharStrings))]
    public void Test2(string whitespace)
    {
        var sut = new TextNodeFilter(WhitespaceOption.RemoveWhitespaceNodes);
        var source = ToComparisonSource(whitespace);

        sut.Filter(source, FilterDecision.Keep).ShouldBe(FilterDecision.Exclude);
    }

    [Theory(DisplayName = "When whitespace option is Normalize, whitespace only text nodes are excluded during filtering")]
    [MemberData(nameof(WhitespaceCharStrings))]
    public void Test3(string whitespace)
    {
        var sut = new TextNodeFilter(WhitespaceOption.Normalize);
        var source = ToComparisonSource(whitespace);

        sut.Filter(source, FilterDecision.Keep).ShouldBe(FilterDecision.Exclude);
    }

    [Theory(DisplayName = "Filter method doesn't change the decision of non-whitespace nodes or non-text nodes")]
    [InlineData("hello world")]
    [InlineData("<p>hello world</p>")]
    public void Test4x(string html)
    {
        var sut = new TextNodeFilter(WhitespaceOption.Normalize);
        var source = ToComparisonSource(html);

        sut.Filter(source, FilterDecision.Keep).ShouldBe(FilterDecision.Keep);
        sut.Filter(source, FilterDecision.Exclude).ShouldBe(FilterDecision.Exclude);
    }

    [Theory(DisplayName = "If parent node is <pre>, <script>, or <style> element, the implicit option is Preserved")]
    [InlineData("pre")]
    [InlineData("style")]
    [InlineData("script")]
    public void Test5(string tag)
    {
        var sut = new TextNodeFilter(WhitespaceOption.Normalize);
        var pre = ToNode($"<{tag}> \n\t </{tag}>");
        var source = pre.FirstChild.ToComparisonSource(0, ComparisonSourceType.Control);

        sut.Filter(source, FilterDecision.Keep).ShouldBe(FilterDecision.Keep);
    }

    [Theory(DisplayName = "If parent node is <pre>, <script>, or <style> element with a diff:whitespace, the option is take from the attribute")]
    [InlineData("pre", WhitespaceOption.RemoveWhitespaceNodes)]
    [InlineData("style", WhitespaceOption.RemoveWhitespaceNodes)]
    [InlineData("script", WhitespaceOption.RemoveWhitespaceNodes)]
    [InlineData("pre", WhitespaceOption.Normalize)]
    [InlineData("style", WhitespaceOption.Normalize)]
    [InlineData("script", WhitespaceOption.Normalize)]
    public void Test51(string tag, WhitespaceOption whitespaceOption)
    {
        var sut = new TextNodeFilter(WhitespaceOption.Normalize);
        var pre = ToNode($"<{tag} diff:whitespace=\"{whitespaceOption.ToString()}\">\n\t</{tag}>");
        var source = pre.FirstChild.ToComparisonSource(0, ComparisonSourceType.Control);

        sut.Filter(source, FilterDecision.Keep).ShouldBe(FilterDecision.Exclude);
    }

    [Theory(DisplayName = "When a parent node has overridden the global whitespace option, that overridden option is used")]
    [InlineData(@"<header><h1><em diff:whitespace=""preserve"">  </em></h1></header>")]
    [InlineData(@"<header><h1 diff:whitespace=""preserve""><em>  </em></h1></header>")]
    [InlineData(@"<header diff:whitespace=""preserve""><h1><em>  </em></h1></header>")]
    public void Tes76(string html)
    {
        var sut = new TextNodeFilter(WhitespaceOption.Normalize);
        var root = ToNode(html);
        var source = new ComparisonSource(root.FirstChild.FirstChild.FirstChild, 0, "dummypath", ComparisonSourceType.Control);

        sut.Filter(source, FilterDecision.Keep).ShouldBe(FilterDecision.Keep);
    }
}


