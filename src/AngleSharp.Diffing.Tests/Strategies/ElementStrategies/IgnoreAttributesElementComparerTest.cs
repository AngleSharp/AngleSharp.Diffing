namespace AngleSharp.Diffing.Strategies.ElementStrategies;

public class IgnoreAttributesElementComparerTest : DiffingTestBase
{
    public IgnoreAttributesElementComparerTest(DiffingTestFixture fixture) : base(fixture)
    {
    }

    [Theory(DisplayName = "When a control element does not contain the 'diff:ignoreAttributes' attribute or it is 'diff:ignoreAttributes=false', the current decision is returned")]
    [InlineData(@"<p></p>")]
    [InlineData(@"<p diff:ignoreAttributes=""false""></p>")]
    [InlineData(@"<p diff:ignoreAttributes=""FALSE""></p>")]
    [InlineData(@"<p diff:ignoreAttributes=""faLsE""></p>")]
    public void Test001(string controlHtml)
    {
        var comparison = ToComparison(controlHtml, "<p></p>");

        IgnoreAttributesElementComparer.Compare(comparison, CompareResult.Different).ShouldBe(CompareResult.Different);
        IgnoreAttributesElementComparer.Compare(comparison, CompareResult.Same).ShouldBe(CompareResult.Same);
        IgnoreAttributesElementComparer.Compare(comparison, CompareResult.Skip).ShouldBe(CompareResult.Skip);
    }

    [Theory(DisplayName = "When a control element has 'diff:ignoreAttributes' attribute, CompareResult.SkipAttributes flag is returned")]
    [InlineData(@"<p diff:ignoreAttributes></p>")]
    [InlineData(@"<p diff:ignoreAttributes=""true""></p>")]
    [InlineData(@"<p diff:ignoreAttributes=""TRUE""></p>")]
    [InlineData(@"<p diff:ignoreAttributes=""TrUe""></p>")]
    public void Test002(string controlHtml)
    {
        var comparison = ToComparison(controlHtml, "<p></p>");

        IgnoreAttributesElementComparer.Compare(comparison, CompareResult.Unknown).ShouldBe(CompareResult.SkipAttributes);
        IgnoreAttributesElementComparer.Compare(comparison, CompareResult.Unknown).ShouldBe(CompareResult.SkipAttributes);
    }

    [Theory(DisplayName = "When a control element has both 'diff:ignoreChildren' and a 'diff:ignoreAttributes'")]
    [InlineData("<button diff:ignoreAttributes diff:ignoreChildren></button>", @"<button id=""buttonid"" class=""somecss"">Not Ignored</button>")]
    [InlineData("<button diff:ignoreAttributes diff:ignoreChildren></button>", @"<button id=""buttonid"" class=""somecss""><span>Not Ignored</span></button>")]
    public void Test003(string controlHtml, string testHtml)
    {
        var comparison = ToComparison(controlHtml, testHtml);

        IgnoreAttributesElementComparer.Compare(comparison, CompareResult.Same).ShouldBe(CompareResult.SkipAttributes);
        IgnoreAttributesElementComparer.Compare(comparison, CompareResult.Unknown).ShouldBe(CompareResult.SkipAttributes);
        IgnoreAttributesElementComparer.Compare(comparison, CompareResult.Skip).ShouldBe(CompareResult.Skip);
        IgnoreAttributesElementComparer.Compare(comparison, CompareResult.SkipChildrenAndAttributes).ShouldBe(CompareResult.SkipChildrenAndAttributes);
        IgnoreAttributesElementComparer.Compare(comparison, CompareResult.SkipChildren).ShouldBe(CompareResult.SkipChildrenAndAttributes);
        IgnoreAttributesElementComparer.Compare(comparison, CompareResult.SkipAttributes).ShouldBe(CompareResult.SkipAttributes);
    }
}
