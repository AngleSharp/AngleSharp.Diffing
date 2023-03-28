using AngleSharp.Diffing.Strategies.AttributeStrategies;

namespace AngleSharp.Diffing.Strategies.IgnoreStrategies;

public class IgnoreAttributeComparerTest : DiffingTestBase
{
    public IgnoreAttributeComparerTest(DiffingTestFixture fixture) : base(fixture)
    {
    }

    [Theory(DisplayName = "When current result is same or skip, the current decision is returned")]
    [MemberData(nameof(SameAndSkipCompareResult))]
    public void Test000(CompareResult currentResult)
    {
        var comparison = ToAttributeComparison(
            @"<p foo=""bar""></p>", "foo",
            @"<p foo=""bar""></p>", "foo"
        );

        IgnoreAttributeComparer
            .Compare(comparison, currentResult)
            .ShouldBe(currentResult);
    }

    [Fact(DisplayName = "When a attribute does not contain have the ':ignore' postfix, the current decision is returned")]
    public void Test003()
    {
        var comparison = ToAttributeComparison(
            @"<p foo=""bar""></p>", "foo",
            @"<p foo=""bar""></p>", "foo"
        );

        IgnoreAttributeComparer.Compare(comparison, CompareResult.Different).ShouldBe(CompareResult.Different);
        IgnoreAttributeComparer.Compare(comparison, CompareResult.Same).ShouldBe(CompareResult.Same);
    }

    [Fact(DisplayName = "When a attribute does contain have the ':ignore' postfix, Same is returned")]
    public void Test004()
    {
        var comparison = ToAttributeComparison(
            @"<p foo:ignore=""bar""></p>", "foo:ignore",
            @"<p foo=""baz""></p>", "foo"
        );

        IgnoreAttributeComparer.Compare(comparison, CompareResult.Unknown).ShouldBe(CompareResult.Same);
    }
}
