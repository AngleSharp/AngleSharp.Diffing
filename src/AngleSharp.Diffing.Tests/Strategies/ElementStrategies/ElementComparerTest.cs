namespace AngleSharp.Diffing.Strategies.ElementStrategies;

public class ElementComparerTest : DiffingTestBase
{
    public ElementComparerTest(DiffingTestFixture fixture) : base(fixture)
    {
    }

    [Theory(DisplayName = "When control and test nodes have the same type and name and enforceTagClosing is false, the result is Same")]
    [InlineData("<p>", "<p />")]
    [InlineData("<br>", "<br/>")]
    [InlineData("<br>", "<br>")]
    [InlineData("<p>", "<p>")]
    public void Test001(string controlHtml, string testHtml)
    {
        var comparison = ToComparison(controlHtml, testHtml);
        new ElementComparer(enforceTagClosing: false)
            .Compare(comparison, CompareResult.Unknown)
            .ShouldBe(CompareResult.Same);
    }

    [Theory(DisplayName = "When control and test nodes have the a different type and name, the result is Different")]
    [InlineData("<div>", "<p>", false)]
    [InlineData("<div>", "textnode", false)]
    [InlineData("<div>", "<!--comment-->", false)]
    [InlineData("<!--comment-->", "textnode", false)]
    [InlineData("<br>", "<br/>", true)]
    [InlineData("<input>", "<input/>", true)]
    [InlineData("<div>", "<p>", true)]
    [InlineData("<div>", "textnode", true)]
    [InlineData("<div>", "<!--comment-->", true)]
    [InlineData("<!--comment-->", "textnode", true)]
    public void Test002(string controlHtml, string testHtml, bool enforceTagClosing)
    {
        var comparison = ToComparison(controlHtml, testHtml);
        new ElementComparer(enforceTagClosing)
            .Compare(comparison, CompareResult.Unknown)
            .ShouldBe(CompareResult.Different);
    }

    [Theory(DisplayName = "When unknown node is used in comparison, but node name is equal, the result is Same")]
    [InlineData("<svg><path></path></svg>", "<path/>")]
    public void HandleUnknownNodeDuringComparison(string controlHtml, string testHtml)
    {
        var knownNode = ToNode(controlHtml).FirstChild.ToComparisonSource(0, ComparisonSourceType.Control);
        var unknownNode = ToNode(testHtml).ToComparisonSource(0, ComparisonSourceType.Test);
        var comparison = new Comparison(knownNode, unknownNode);

        new ElementComparer(enforceTagClosing: false)
            .Compare(comparison, CompareResult.Unknown)
            .ShouldBe(CompareResult.Same);
    }
}
