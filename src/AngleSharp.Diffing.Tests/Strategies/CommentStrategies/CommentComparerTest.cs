namespace AngleSharp.Diffing.Strategies.CommentStrategies;

public class CommentComparerTest : DiffingTestBase
{
    public CommentComparerTest(DiffingTestFixture fixture) : base(fixture)
    {
    }

    [Theory(DisplayName = "When current result is same or skip, current result is returned")]
    [MemberData(nameof(SameAndSkipCompareResult))]
    public void Test000(CompareResult currentResult)
    {
        var comparison = ToComparison("<!--foo-->", "<!--bar-->");
        CommentComparer
            .Compare(comparison, currentResult)
            .ShouldBe(currentResult);
    }

    [Theory(DisplayName = "When control and test are comment with equal content, the result is Same")]
    [InlineData("<!---->", "<!---->")]
    [InlineData("<!--foo-->", "<!--foo-->")]
    public void Test001(string controlHtml, string testHtml)
    {
        var comparison = ToComparison(controlHtml, testHtml);
        CommentComparer
            .Compare(comparison, CompareResult.Unknown)
            .ShouldBe(CompareResult.Same);
    }

    [Theory(DisplayName = "When control and test are comment with unequal content, the result is Different")]
    [InlineData("<!---->", "<!--foo-->")]
    [InlineData("<!--foo-->", "<!---->")]
    [InlineData("<!--foo-->", "<!--bar-->")]
    public void Test002(string controlHtml, string testHtml)
    {
        var comparison = ToComparison(controlHtml, testHtml);
        CommentComparer
            .Compare(comparison, CompareResult.Unknown)
            .ShouldBe(CompareResult.FromDiff(new CommentDiff(comparison)));
    }

    [Theory(DisplayName = "When input node is not a IComment node, comparer does not run nor change the current decision")]
    [InlineData("foo", "bar")]
    [InlineData("<p>", "<div>")]
    public void Test003(string controlHtml, string testHtml)
    {
        var comparison = ToComparison(controlHtml, testHtml);

        CommentComparer.Compare(comparison, CompareResult.Different).ShouldBe(CompareResult.Different);
        CommentComparer.Compare(comparison, CompareResult.Same).ShouldBe(CompareResult.Same);
        CommentComparer.Compare(comparison, CompareResult.Skip).ShouldBe(CompareResult.Skip);
    }
}
