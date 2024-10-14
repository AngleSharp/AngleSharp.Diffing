namespace AngleSharp.Diffing.Strategies.ElementStrategies;

public class IgnoreChildrenElementComparerTest : DiffingTestBase
{
    public IgnoreChildrenElementComparerTest(DiffingTestFixture fixture) : base(fixture)
    {
    }

    [Theory(DisplayName = "When a control element does not contain the 'diff:ignoreChildren' attribute or it is 'diff:ignoreChildren=false', the current decision is returned")]
    [InlineData(@"<p></p>")]
    [InlineData(@"<p diff:ignoreChildren=""false""></p>")]
    [InlineData(@"<p diff:ignoreChildren=""FALSE""></p>")]
    [InlineData(@"<p diff:ignoreChildren=""faLsE""></p>")]
    public void Test001(string controlHtml)
    {
        var comparison = ToComparison(controlHtml, "<p></p>");

        IgnoreChildrenElementComparer.Compare(comparison, CompareResult.Different).ShouldBe(CompareResult.Different);
        IgnoreChildrenElementComparer.Compare(comparison, CompareResult.Same).ShouldBe(CompareResult.Same);
        IgnoreChildrenElementComparer.Compare(comparison, CompareResult.Skip).ShouldBe(CompareResult.Skip);
    }

    [Theory(DisplayName = "When a control element has 'diff:ignoreChildren' attribute, CompareResult.SkipChildren flag is returned")]
    [InlineData(@"<p diff:ignoreChildren></p>")]
    [InlineData(@"<p diff:ignoreChildren=""true""></p>")]
    [InlineData(@"<p diff:ignoreChildren=""TRUE""></p>")]
    [InlineData(@"<p diff:ignoreChildren=""TrUe""></p>")]
    public void Test002(string controlHtml)
    {
        var comparison = ToComparison(controlHtml, "<p></p>");

        IgnoreChildrenElementComparer.Compare(comparison, CompareResult.Unknown).ShouldBe(CompareResult.SkipChildren);
        IgnoreChildrenElementComparer.Compare(comparison, CompareResult.Unknown).ShouldBe(CompareResult.SkipChildren);
    }

    [Theory(DisplayName = "When a control element has both 'diff:ignoreChildren' and a 'diff:ignoreAttributes'")]
    [InlineData("<button diff:ignoreAttributes diff:ignoreChildren></button>", @"<button id=""buttonid"" class=""somecss"">Not Ignored</button>")]
    [InlineData("<button diff:ignoreAttributes diff:ignoreChildren></button>", @"<button id=""buttonid"" class=""somecss""><span>Not Ignored</span></button>")]
    public void Test003(string controlHtml, string testHtml)
    {
        var comparison = ToComparison(controlHtml, testHtml);

        IgnoreChildrenElementComparer.Compare(comparison, CompareResult.Same).ShouldBe(CompareResult.SkipChildren);
        IgnoreChildrenElementComparer.Compare(comparison, CompareResult.Unknown).ShouldBe(CompareResult.SkipChildren);
        IgnoreChildrenElementComparer.Compare(comparison, CompareResult.Skip).ShouldBe(CompareResult.Skip);
        IgnoreChildrenElementComparer.Compare(comparison, CompareResult.SkipChildrenAndAttributes).ShouldBe(CompareResult.SkipChildrenAndAttributes);
        IgnoreChildrenElementComparer.Compare(comparison, CompareResult.SkipAttributes).ShouldBe(CompareResult.SkipChildrenAndAttributes);
        IgnoreChildrenElementComparer.Compare(comparison, CompareResult.SkipChildren).ShouldBe(CompareResult.SkipChildren);
    }
}
