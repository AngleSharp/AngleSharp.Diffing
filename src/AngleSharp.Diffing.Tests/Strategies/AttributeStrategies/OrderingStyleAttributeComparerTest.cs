﻿namespace AngleSharp.Diffing.Strategies.AttributeStrategies;

public class OrderingStyleAttributeComparerTest : DiffingTestBase
{
    public OrderingStyleAttributeComparerTest(DiffingTestFixture fixture) : base(fixture)
    {
    }

    [Theory(DisplayName = "When current result is same or skip, the current decision is returned")]
    [MemberData(nameof(SameAndSkipCompareResult))]
    public void Test000(CompareResult currentResult)
    {
        var comparison = ToAttributeComparison(@"<p style=""color:red"">", "style", @"<p style=""color:red"">", "style");
        OrderingStyleAttributeComparer
            .Compare(comparison, currentResult)
            .ShouldBe(currentResult);
    }

    [Fact(DisplayName = "When attribute is not style the current decision is used")]
    public void Test001()
    {
        var comparison = ToAttributeComparison(@"<p foo=""bar"">", "foo", @"<p foo=""zab"">", "foo");
        OrderingStyleAttributeComparer.Compare(comparison, CompareResult.Different).ShouldBe(CompareResult.Different);
        OrderingStyleAttributeComparer.Compare(comparison, CompareResult.Same).ShouldBe(CompareResult.Same);
        OrderingStyleAttributeComparer.Compare(comparison, CompareResult.Skip).ShouldBe(CompareResult.Skip);
    }

    [Theory(DisplayName = "When style attributes has different values then Different is returned")]
    [InlineData(@"<p style=""color: red"">", @"<p style=""color: black"">")]
    [InlineData(@"<p style=""color: red"">", @"<p style=""text-align:center"">")]
    [InlineData(@"<p style=""color: red"">", @"<p style=""color: red;text-align:center"">")]
    [InlineData(@"<p style=""color: red;text-align:center"">", @"<p style=""color: red"">")]
    public void Test002(string control, string test)
    {
        var comparison = ToAttributeComparison(control, "style", test, "style");

        OrderingStyleAttributeComparer
            .Compare(comparison, CompareResult.Unknown)
            .ShouldBe(CompareResult.FromDiff(new AttrDiff(comparison, AttrDiffKind.Value)));
    }

    [Fact(DisplayName = "Comparer should correctly ignore insignificant whitespace")]
    public void Test003()
    {
        var comparison = ToAttributeComparison(@"<p style=""color: red"">", "style", @"<p style=""color:red"">", "style");
        OrderingStyleAttributeComparer.Compare(comparison, CompareResult.Unknown).ShouldBe(CompareResult.Same);
    }

    [Theory(DisplayName = "Comparer should ignore trailing semi colons")]
    [InlineData(@"<p style=""color:red;"">", @"<p style=""color:red"">")]
    [InlineData(@"<p style=""color:red"">", @"<p style=""color:red;"">")]
    public void Test004(string control, string test)
    {
        var comparison = ToAttributeComparison(control, "style", test, "style");
        OrderingStyleAttributeComparer.Compare(comparison, CompareResult.Unknown).ShouldBe(CompareResult.Same);
    }

    [Theory(DisplayName = "Comparer should ignore different order")]
    [InlineData(@"<p style=""alpha:0;border:0;color:red;"">", @"<p style=""color:red;border:0;alpha:0"">")]
    [InlineData(@"<p style=""alpha:0;border:0;color:red;"">", @"<p style=""border:0;color:red;alpha:0"">")]
    public void Test005(string control, string test)
    {
        var comparison = ToAttributeComparison(control, "style", test, "style");
        OrderingStyleAttributeComparer.Compare(comparison, CompareResult.Unknown).ShouldBe(CompareResult.Same);
    }

    [Theory(DisplayName = "Comparer should ignore different order inside style")]
    [InlineData(@"<p style=""border:1px solid black"">", @"<p style=""border:solid 1px black"">")]
    public void Test006(string control, string test)
    {
        var comparison = ToAttributeComparison(control, "style", test, "style");
        OrderingStyleAttributeComparer.Compare(comparison, CompareResult.Unknown).ShouldBe(CompareResult.Same);
    }
}
