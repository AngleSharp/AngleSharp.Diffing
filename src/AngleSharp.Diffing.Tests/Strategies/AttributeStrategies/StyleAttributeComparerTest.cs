namespace AngleSharp.Diffing.Strategies.AttributeStrategies;

public class StyleAttributeComparerTest : DiffingTestBase
{
    public StyleAttributeComparerTest(DiffingTestFixture fixture) : base(fixture)
    {
    }

    [Fact(DisplayName = "When attribute is not style the current decision is used")]
    public void Test001()
    {
        var comparison = ToAttributeComparison(@"<p foo=""bar"">", "foo", @"<p foo=""zab"">", "foo");
        StyleAttributeComparer.Compare(comparison, CompareResult.Different(null)).ShouldBe(CompareResult.Different(null));
        StyleAttributeComparer.Compare(comparison, CompareResult.Same).ShouldBe(CompareResult.Same);
        StyleAttributeComparer.Compare(comparison, CompareResult.Skip).ShouldBe(CompareResult.Skip);
    }

    [Theory(DisplayName = "When style attributes has different values then Different is returned")]
    [InlineData(@"<p style=""color: red"">", @"<p style=""color: black"">")]
    [InlineData(@"<p style=""color: red"">", @"<p style=""text-align:center"">")]
    [InlineData(@"<p style=""color: red"">", @"<p style=""color: red;text-align:center"">")]
    [InlineData(@"<p style=""color: red;text-align:center"">", @"<p style=""color: red"">")]
    public void Test002(string control, string test)
    {
        var comparison = ToAttributeComparison(control, "style", test, "style");

        var result = StyleAttributeComparer.Compare(comparison, CompareResult.Unknown);

        result.Decision.ShouldBe(CompareResultDecision.Different);
        result.Diff.ShouldBeEquivalentTo(new AttrValueDiff(comparison, AttrValueDiffKind.Styles));
    }

    [Fact(DisplayName = "Comparer should correctly ignore insignificant whitespace")]
    public void Test003()
    {
        var comparison = ToAttributeComparison(@"<p style=""color: red"">", "style", @"<p style=""color:red"">", "style");
        StyleAttributeComparer.Compare(comparison, CompareResult.Unknown).ShouldBe(CompareResult.Same);
    }

    [Theory(DisplayName = "Comparer should ignore trailing semi colons")]
    [InlineData(@"<p style=""color:red;"">", @"<p style=""color:red"">")]
    [InlineData(@"<p style=""color:red"">", @"<p style=""color:red;"">")]
    public void Test004(string control, string test)
    {
        var comparison = ToAttributeComparison(control, "style", test, "style");
        StyleAttributeComparer.Compare(comparison, CompareResult.Unknown).ShouldBe(CompareResult.Same);
    }

    [Theory(DisplayName = "Comparer should ignore different order inside style")]
    [InlineData(@"<p style=""border:1px solid black"">", @"<p style=""border:solid 1px black"">")]
    public void Test005(string control, string test)
    {
        var comparison = ToAttributeComparison(control, "style", test, "style");
        StyleAttributeComparer.Compare(comparison, CompareResult.Unknown).ShouldBe(CompareResult.Same);
    }
}
