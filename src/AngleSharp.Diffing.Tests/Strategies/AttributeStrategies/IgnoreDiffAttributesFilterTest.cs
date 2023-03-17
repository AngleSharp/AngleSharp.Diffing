namespace AngleSharp.Diffing.Strategies.AttributeStrategies;

public class IgnoreDiffAttributesFilterTest : DiffingTestBase
{
    public IgnoreDiffAttributesFilterTest(DiffingTestFixture fixture) : base(fixture)
    {
    }

    [Theory(DisplayName = "When an attribute starts with 'diff:' it is filtered out")]
    [InlineData(@"<p diff:whitespace=""Normalize"">", "diff:whitespace")]
    [InlineData(@"<p diff:ignore=""true"">", "diff:ignore")]
    public void Test1(string elementHtml, string diffAttrName)
    {
        var source = ToAttributeComparisonSource(elementHtml, diffAttrName);

        IgnoreDiffAttributesFilter.Filter(source, FilterDecision.Keep).ShouldBe(FilterDecision.Exclude);
    }

    [Theory(DisplayName = "When an attribute does not starts with 'diff:' its current decision is used")]
    [InlineData(@"<p lang=""csharp"">", "lang")]
    [InlineData(@"<p diff=""foo"">", "diff")]
    [InlineData(@"<p diffx=""foo"">", "diffx")]
    public void Test2(string elementHtml, string diffAttrName)
    {
        var source = ToAttributeComparisonSource(elementHtml, diffAttrName);

        IgnoreDiffAttributesFilter.Filter(source, FilterDecision.Keep).ShouldBe(FilterDecision.Keep);
    }
}
