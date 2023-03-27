using Shouldly;

namespace AngleSharp.Diffing.Strategies.AttributeStrategies;


public class AttributeComparerTest : DiffingTestBase
{
    public AttributeComparerTest(DiffingTestFixture fixture) : base(fixture)
    {
    }

    [Fact(DisplayName = "When compare is called with a current decision of Same or Skip, the current decision is returned")]
    public void Test001()
    {
        var comparison = ToAttributeComparison(@"<b foo>", "foo",
                                                "<b bar>", "bar");

        AttributeComparer.Compare(comparison, CompareResult.Same).ShouldBe(CompareResult.Same);
        AttributeComparer.Compare(comparison, CompareResult.Skip).ShouldBe(CompareResult.Skip);
    }

    [Fact(DisplayName = "When two attributes has the same name and no value, the compare result is Same")]
    public void Test002()
    {
        var comparison = ToAttributeComparison(@"<b foo>", "foo",
                                                "<b foo>", "foo");

        AttributeComparer.Compare(comparison, CompareResult.Unknown).ShouldBe(CompareResult.Same);
    }

    [Fact(DisplayName = "When two attributes does not have the same name, the compare result is Different")]
    public void Test003()
    {
        var comparison = ToAttributeComparison(@"<b foo>", "foo",
                                                "<b bar>", "bar");

        var result = AttributeComparer.Compare(comparison, CompareResult.Unknown);

        result.Decision.ShouldBe(CompareDecision.Different);
        result.Diff.ShouldBeEquivalentTo(new AttrDiff(comparison, AttrDiffKind.Name));
    }

    [Fact(DisplayName = "When two attribute values are the same, the compare result is Same")]
    public void Test004()
    {
        var comparison = ToAttributeComparison(@"<b foo=""bar"">", "foo",
                                               @"<b foo=""bar"">", "foo");

        AttributeComparer.Compare(comparison, CompareResult.Different).ShouldBe(CompareResult.Same);
    }

    [Fact(DisplayName = "When two attribute values are different, the compare result is Different")]
    public void Test005()
    {
        var comparison = ToAttributeComparison(@"<b foo=""bar"">", "foo",
                                               @"<b foo=""baz"">", "foo");

        var result = AttributeComparer.Compare(comparison, CompareResult.Unknown);

        result.Decision.ShouldBe(CompareDecision.Different);
        result.Diff.ShouldBeEquivalentTo(new AttrDiff(comparison, AttrDiffKind.Value));
    }

    [Fact(DisplayName = "When the control attribute is postfixed with :ignoreCase, " +
                         "a case insensitive comparison between control and test attributes is performed")]
    public void Test006()
    {
        var comparison = ToAttributeComparison(@"<b foo:ignoreCase=""BAR"">", "foo:ignorecase",
                                               @"<b foo=""bar"">", "foo");

        AttributeComparer.Compare(comparison, CompareResult.Unknown).ShouldBe(CompareResult.Same);
    }

    [Fact(DisplayName = "When the control attribute is postfixed with :regex, " +
                        "the control attributes value is assumed to be a regular expression and " +
                        "that is used to match against the test attributes value")]
    public void Test007()
    {
        var comparison = ToAttributeComparison(@"<b foo:regex=""foobar-\d{4}"">", "foo:regex",
                                               @"<b foo=""foobar-2000"">", "foo");

        AttributeComparer.Compare(comparison, CompareResult.Unknown).ShouldBe(CompareResult.Same);
    }

    [Theory(DisplayName = "When the control attribute is postfixed with :regex:ignoreCase " +
                         "or :ignoreCase:regex, the control attributes value is assumed " +
                         "to be a regular expression and that is used to do a case insensitive " +
                         "match against the test attributes value")]
    [InlineData(":regex:ignorecase")]
    [InlineData(":ignorecase:regex")]
    public void Test008(string attrNamePostfix)
    {
        var controlAttrName = $"foo{attrNamePostfix}";
        var comparison = ToAttributeComparison($@"<b {controlAttrName}=""foobar-\d{{4}}"">", controlAttrName,
                                               @"<b foo=""FOOBAR-2000"">", "foo");

        AttributeComparer.Compare(comparison, CompareResult.Unknown).ShouldBe(CompareResult.Same);
    }
}
