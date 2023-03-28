namespace AngleSharp.Diffing.Strategies.AttributeStrategies;

public class BooleanAttributeComparerTest : DiffingTestBase
{
    public static readonly IEnumerable<object[]> BooleanAttributes = BooleanAttributeComparer.BooleanAttributes.Select(x => new string[] { x }).ToArray();

    public BooleanAttributeComparerTest(DiffingTestFixture fixture) : base(fixture)
    {
    }

    [Theory(DisplayName = "When current result is same or skip, the current decision is returned")]
    [MemberData(nameof(SameAndSkipCompareResult))]
    public void Test000(CompareResult currentResult)
    {
        var comparison = ToAttributeComparison(@"<b allowfullscreen=""false"">", "allowfullscreen", @"<b allowfullscreen>", "allowfullscreen");

        new BooleanAttributeComparer(BooleanAttributeComparision.Strict)
            .Compare(comparison, currentResult)
            .ShouldBe(currentResult);
    }

    [Fact(DisplayName = "When attribute names are not the same comparer returns different")]
    public void Test001()
    {
        var sut = new BooleanAttributeComparer(BooleanAttributeComparision.Strict);
        var comparison = ToAttributeComparison("<b foo>", "foo", "<b bar>", "bar");

        sut.Compare(comparison, CompareResult.Unknown)
            .ShouldBe(CompareResult.FromDiff(new AttrDiff(comparison, AttrDiffKind.Name)));
    }

    [Fact(DisplayName = "When attribute name is not an boolean attribute, its current result is returned")]
    public void Test002()
    {
        var sut = new BooleanAttributeComparer(BooleanAttributeComparision.Strict);
        var comparison = ToAttributeComparison(@"<b class="""">", "class", @"<b class="""">", "class");

        sut.Compare(comparison, CompareResult.Different).ShouldBe(CompareResult.Different);
    }

    [Theory(DisplayName = "When attributes is boolean and mode is strict, " +
                          "the attribute is considered truthy if conforms to the html5 spec and" +
                          "the result is Same if both control and test attribute are truthy")]
    [MemberData(nameof(BooleanAttributes))]
    public void Test003(string attrName)
    {
        var sut = new BooleanAttributeComparer(BooleanAttributeComparision.Strict);
        var c1 = ToAttributeComparison($@"<b {attrName}="""">", attrName, $@"<b {attrName}=""{attrName}"">", attrName);
        var c2 = ToAttributeComparison($@"<b {attrName}=""{attrName}"">", attrName, $@"<b {attrName}="""">", attrName);
        var c3 = ToAttributeComparison($@"<b {attrName}>", attrName, $@"<b {attrName}="""">", attrName);
        var c4 = ToAttributeComparison($@"<b {attrName}>", attrName, $@"<b {attrName}=""{attrName}"">", attrName);
        var c5 = ToAttributeComparison($@"<b {attrName}="""">", attrName, $@"<b {attrName}>", attrName);
        var c6 = ToAttributeComparison($@"<b {attrName}=""{attrName}"">", attrName, $@"<b {attrName}>", attrName);

        sut.Compare(c1, CompareResult.Unknown).ShouldBe(CompareResult.Same);
        sut.Compare(c2, CompareResult.Unknown).ShouldBe(CompareResult.Same);
        sut.Compare(c3, CompareResult.Unknown).ShouldBe(CompareResult.Same);
        sut.Compare(c4, CompareResult.Unknown).ShouldBe(CompareResult.Same);
        sut.Compare(c5, CompareResult.Unknown).ShouldBe(CompareResult.Same);
        sut.Compare(c6, CompareResult.Unknown).ShouldBe(CompareResult.Same);
    }

    [Theory(DisplayName = "When attributes is boolean and mode is loose, the presence of " +
        "two attributes with the same name returns compare result Same, no matter what their value is")]
    [MemberData(nameof(BooleanAttributes))]
    public void Test004(string attrName)
    {
        var sut = new BooleanAttributeComparer(BooleanAttributeComparision.Loose);
        var c1 = ToAttributeComparison($@"<b {attrName}=""foo"">", attrName, $@"<b {attrName}=""bar"">", attrName);
        var c2 = ToAttributeComparison($@"<b {attrName}=""true"">", attrName, $@"<b {attrName}=""true"">", attrName);
        var c3 = ToAttributeComparison($@"<b {attrName}=""true"">", attrName, $@"<b {attrName}=""false"">", attrName);


        sut.Compare(c1, CompareResult.Unknown).ShouldBe(CompareResult.Same);
        sut.Compare(c2, CompareResult.Unknown).ShouldBe(CompareResult.Same);
        sut.Compare(c3, CompareResult.Unknown).ShouldBe(CompareResult.Same);
    }
}
