namespace AngleSharp.Diffing.Strategies;

public class DiffingStrategyPipelineTest : DiffingTestBase
{
    public DiffingStrategyPipelineTest(DiffingTestFixture fixture) : base(fixture)
    {
    }

    private static FilterDecision NegateDecision(FilterDecision decision) => decision switch
    {
        FilterDecision.Keep => FilterDecision.Exclude,
        FilterDecision.Exclude => FilterDecision.Keep,
        _ => throw new InvalidOperationException()
    };

    [Fact(DisplayName = "When zero filter strategies have been added, true is returned")]
    public void Test1()
    {
        var sut = new DiffingStrategyPipeline();

        sut.Filter(new ComparisonSource()).ShouldBe(FilterDecision.Keep);
        sut.Filter(new AttributeComparisonSource()).ShouldBe(FilterDecision.Keep);
    }

    [Theory(DisplayName = "When one or more specialized filter strategies are added, " +
                          "they are executed in the order they are added in")]
    [InlineData(FilterDecision.Keep)]
    [InlineData(FilterDecision.Exclude)]
    public void Test001(FilterDecision expected)
    {
        var sut = new DiffingStrategyPipeline();

        sut.AddFilter((in ComparisonSource s, FilterDecision currentDecision) => NegateDecision(expected), StrategyType.Specialized);
        sut.AddFilter((in ComparisonSource s, FilterDecision currentDecision) => expected, StrategyType.Specialized);
        sut.AddFilter((in AttributeComparisonSource s, FilterDecision currentDecision) => NegateDecision(expected), StrategyType.Specialized);
        sut.AddFilter((in AttributeComparisonSource s, FilterDecision currentDecision) => expected, StrategyType.Specialized);

        sut.Filter(new ComparisonSource()).ShouldBe(expected);
        sut.Filter(new AttributeComparisonSource()).ShouldBe(expected);
    }

    [Theory(DisplayName = "When one or more generalized filter strategies are added, the they are executed in the opposite order they are added in")]
    [InlineData(FilterDecision.Keep)]
    [InlineData(FilterDecision.Exclude)]
    public void Test002(FilterDecision expected)
    {
        var sut = new DiffingStrategyPipeline();

        sut.AddFilter((in ComparisonSource s, FilterDecision currentDecision) => expected, StrategyType.Generalized);
        sut.AddFilter((in ComparisonSource s, FilterDecision currentDecision) => NegateDecision(expected), StrategyType.Generalized);
        sut.AddFilter((in AttributeComparisonSource s, FilterDecision currentDecision) => expected, StrategyType.Generalized);
        sut.AddFilter((in AttributeComparisonSource s, FilterDecision currentDecision) => NegateDecision(expected), StrategyType.Generalized);

        sut.Filter(new ComparisonSource()).ShouldBe(expected);
        sut.Filter(new AttributeComparisonSource()).ShouldBe(expected);
    }

    [Theory(DisplayName = "Generalized filter strategies are always executed before specialized filter strategies")]
    [InlineData(FilterDecision.Keep)]
    [InlineData(FilterDecision.Exclude)]
    public void Test003(FilterDecision expected)
    {
        var sut = new DiffingStrategyPipeline();

        sut.AddFilter((in ComparisonSource s, FilterDecision currentDecision) => expected, StrategyType.Specialized);
        sut.AddFilter((in ComparisonSource s, FilterDecision currentDecision) => NegateDecision(expected), StrategyType.Generalized);
        sut.AddFilter((in AttributeComparisonSource s, FilterDecision currentDecision) => expected, StrategyType.Specialized);
        sut.AddFilter((in AttributeComparisonSource s, FilterDecision currentDecision) => NegateDecision(expected), StrategyType.Generalized);

        sut.Filter(new ComparisonSource()).ShouldBe(expected);
        sut.Filter(new AttributeComparisonSource()).ShouldBe(expected);
    }

    [Fact(DisplayName = "When no matcher strategy have been added, no comparisons are returned")]
    public void Test2()
    {
        var sut = new DiffingStrategyPipeline();

        sut.Match(null, null, (SourceCollection)null).ShouldBeEmpty();
        sut.Match(null, null, (SourceMap)null).ShouldBeEmpty();
    }

    [Fact(DisplayName = "Specialized node matchers are executed in the reverse order they are added in")]
    public void Test61()
    {
        var sourceColllection = ToSourceCollection("<p></p><span></span>", ComparisonSourceType.Control);
        var sut = new DiffingStrategyPipeline();
        sut.AddMatcher((ctx, s, t) => new[] { new Comparison(s[0], t[0]) }, StrategyType.Specialized);
        sut.AddMatcher((ctx, s, t) => new[] { new Comparison(s[1], t[1]) }, StrategyType.Specialized);

        var result = sut.Match(DummyContext, sourceColllection, sourceColllection).ToList();

        result[0].Control.ShouldBe(sourceColllection[1]);
        result[1].Control.ShouldBe(sourceColllection[0]);
    }

    [Fact(DisplayName = "Specialized attributes matchers are executed in the reverse order they are added in")]
    public void Test71()
    {
        var sourceMap = ToSourceMap(@"<p foo=""bar"" baz=""bum""></p>");
        var sut = new DiffingStrategyPipeline();
        sut.AddMatcher((ctx, s, t) => new[] { new AttributeComparison(s["foo"], t["foo"]) }, StrategyType.Specialized);
        sut.AddMatcher((ctx, s, t) => new[] { new AttributeComparison(s["baz"], t["baz"]) }, StrategyType.Specialized);

        var result = sut.Match(DummyContext, sourceMap, sourceMap).ToList();

        result[0].Control.ShouldBe(sourceMap["baz"]);
        result[1].Control.ShouldBe(sourceMap["foo"]);
    }

    [Fact(DisplayName = "Generalized node matchers are executed in order they are added in")]
    public void Test62()
    {
        var sourceColllection = ToSourceCollection("<p></p><span></span>", ComparisonSourceType.Control);
        var sut = new DiffingStrategyPipeline();
        sut.AddMatcher((ctx, s, t) => new[] { new Comparison(s[0], t[0]) }, StrategyType.Generalized);
        sut.AddMatcher((ctx, s, t) => new[] { new Comparison(s[1], t[1]) }, StrategyType.Generalized);

        var result = sut.Match(DummyContext, sourceColllection, sourceColllection).ToList();

        result[0].Control.ShouldBe(sourceColllection[0]);
        result[1].Control.ShouldBe(sourceColllection[1]);
    }

    [Fact(DisplayName = "Generalized attributes matchers are executed in the order they are added in")]
    public void Test72()
    {
        var sourceMap = ToSourceMap(@"<p foo=""bar"" baz=""bum""></p>");
        var sut = new DiffingStrategyPipeline();
        sut.AddMatcher((ctx, s, t) => new[] { new AttributeComparison(s["foo"], t["foo"]) }, StrategyType.Generalized);
        sut.AddMatcher((ctx, s, t) => new[] { new AttributeComparison(s["baz"], t["baz"]) }, StrategyType.Generalized);

        var result = sut.Match(DummyContext, sourceMap, sourceMap).ToList();

        result[0].Control.ShouldBe(sourceMap["foo"]);
        result[1].Control.ShouldBe(sourceMap["baz"]);
    }

    [Fact(DisplayName = "Generalized node matchers always execute after specialized node matchers")]
    public void Test62123()
    {
        var sourceColllection = ToSourceCollection("<p></p><span></span>", ComparisonSourceType.Control);
        var sut = new DiffingStrategyPipeline();
        sut.AddMatcher((ctx, s, t) => new[] { new Comparison(s[0], t[0]) }, StrategyType.Generalized);
        sut.AddMatcher((ctx, s, t) => new[] { new Comparison(s[1], t[1]) }, StrategyType.Specialized);

        var result = sut.Match(DummyContext, sourceColllection, sourceColllection).ToList();

        result[0].Control.ShouldBe(sourceColllection[1]);
        result[1].Control.ShouldBe(sourceColllection[0]);
    }

    [Fact(DisplayName = "Generalized attributes matchers always execute after specialized attribute matchers")]
    public void Test74342()
    {
        var sourceMap = ToSourceMap(@"<p foo=""bar"" baz=""bum""></p>");
        var sut = new DiffingStrategyPipeline();
        sut.AddMatcher((ctx, s, t) => new[] { new AttributeComparison(s["foo"], t["foo"]) }, StrategyType.Generalized);
        sut.AddMatcher((ctx, s, t) => new[] { new AttributeComparison(s["baz"], t["baz"]) }, StrategyType.Specialized);

        var result = sut.Match(DummyContext, sourceMap, sourceMap).ToList();

        result[0].Control.ShouldBe(sourceMap["baz"]);
        result[1].Control.ShouldBe(sourceMap["foo"]);
    }


    [Fact(DisplayName = "When no compare strategy have been added, Different is returned for node and attribute comparisons")]
    public void Test3()
    {
        var sut = new DiffingStrategyPipeline();

        sut.Compare(new Comparison()).ShouldBe(CompareResult.Different);
        sut.Compare(new AttributeComparison()).ShouldBe(CompareResult.Different);
    }

    [Theory(DisplayName = "Specialized comparers are executed in the order they are added in")]
    [InlineData(CompareDecision.Different, CompareDecision.Same)]
    [InlineData(CompareDecision.Same, CompareDecision.Different)]
    public void Test8(CompareDecision first, CompareDecision final)
    {
        var sut = new DiffingStrategyPipeline();

        sut.AddComparer((in Comparison c, CompareResult current) => new CompareResult(first), StrategyType.Specialized);
        sut.AddComparer((in Comparison c, CompareResult current) => new CompareResult(final), StrategyType.Specialized);
        sut.AddComparer((in AttributeComparison c, CompareResult current) => new CompareResult(first), StrategyType.Specialized);
        sut.AddComparer((in AttributeComparison c, CompareResult current) => new CompareResult(final), StrategyType.Specialized);

        sut.Compare(new Comparison()).ShouldBe(new CompareResult(final));
        sut.Compare(new AttributeComparison()).ShouldBe(new CompareResult(final));
    }

    [Theory(DisplayName = "Generalized comparers are executed in the reverse order they are added in")]
    [InlineData(CompareDecision.Different, CompareDecision.Same)]
    [InlineData(CompareDecision.Same, CompareDecision.Different)]
    public void Test12321(CompareDecision first, CompareDecision final)
    {
        var sut = new DiffingStrategyPipeline();

        sut.AddComparer((in Comparison c, CompareResult current) => new CompareResult(final), StrategyType.Generalized);
        sut.AddComparer((in Comparison c, CompareResult current) => new CompareResult(first), StrategyType.Generalized);
        sut.AddComparer((in AttributeComparison c, CompareResult current) => new CompareResult(final), StrategyType.Generalized);
        sut.AddComparer((in AttributeComparison c, CompareResult current) => new CompareResult(first), StrategyType.Generalized);

        sut.Compare(new Comparison()).ShouldBe(new CompareResult(final));
        sut.Compare(new AttributeComparison()).ShouldBe(new CompareResult(final));
    }

    [Theory(DisplayName = "Generalized comparers are always executed before specialized comparers")]
    [InlineData(CompareDecision.Different, CompareDecision.Same)]
    [InlineData(CompareDecision.Same, CompareDecision.Different)]
    public void Test8314(CompareDecision first, CompareDecision final)
    {
        var sut = new DiffingStrategyPipeline();

        sut.AddComparer((in Comparison c, CompareResult current) => new CompareResult(first), StrategyType.Generalized);
        sut.AddComparer((in Comparison c, CompareResult current) => new CompareResult(final), StrategyType.Specialized);
        sut.AddComparer((in AttributeComparison c, CompareResult current) => new CompareResult(first), StrategyType.Generalized);
        sut.AddComparer((in AttributeComparison c, CompareResult current) => new CompareResult(final), StrategyType.Specialized);

        sut.Compare(new Comparison()).ShouldBe(new CompareResult(final));
        sut.Compare(new AttributeComparison()).ShouldBe(new CompareResult(final));
    }

    [Fact(DisplayName = "After two nodes has been matched, they are marked as matched in the source collection")]
    public void Test101()
    {
        var controlSources = ToSourceCollection("<p></p>", ComparisonSourceType.Control);
        var testSources = ToSourceCollection("<p></p>", ComparisonSourceType.Test);
        var sut = new DiffingStrategyPipeline();
        sut.AddMatcher((ctx, s, t) => new[] { new Comparison(s[0], t[0]) }, StrategyType.Specialized);

        var result = sut.Match(DummyContext, controlSources, testSources).ToList();

        controlSources.GetUnmatched().ShouldBeEmpty();
        testSources.GetUnmatched().ShouldBeEmpty();
    }

    [Fact(DisplayName = "After two attributes has been matched, they are marked as matched in the source map")]
    public void Test102()
    {
        var controlSources = ToSourceMap(@"<p foo=""bar""></p>", ComparisonSourceType.Control);
        var testSources = ToSourceMap(@"<p foo=""bar""></p>", ComparisonSourceType.Test);
        var sut = new DiffingStrategyPipeline();
        sut.AddMatcher((ctx, s, t) => new[] { new AttributeComparison(s["foo"], t["foo"]) }, StrategyType.Specialized);

        var result = sut.Match(DummyContext, controlSources, testSources).ToList();

        controlSources.GetUnmatched().ShouldBeEmpty();
        testSources.GetUnmatched().ShouldBeEmpty();
    }

    [Fact(DisplayName = "When no matchers have been added, HasMatchers returns false")]
    public void Test201()
    {
        var sut = new DiffingStrategyPipeline();
        sut.HasMatchers.ShouldBeFalse();
    }

    [Fact(DisplayName = "When either a node or attribute matchers are missing, HasMatchers returns false")]
    public void Test202()
    {
        var sutWithAttrMatcher = new DiffingStrategyPipeline();
        sutWithAttrMatcher.AddMatcher((ctx, s, t) => Array.Empty<AttributeComparison>(), StrategyType.Generalized);
        sutWithAttrMatcher.HasMatchers.ShouldBeFalse();

        var sutWithNodeMatcher = new DiffingStrategyPipeline();
        sutWithNodeMatcher.AddMatcher((ctx, s, t) => Array.Empty<Comparison>(), StrategyType.Generalized);
        sutWithNodeMatcher.HasMatchers.ShouldBeFalse();
    }

    [Fact(DisplayName = "When at least one node and attribute matchers are added, HasMatchers returns true")]
    public void Test203()
    {
        var sut = new DiffingStrategyPipeline();
        sut.AddMatcher((ctx, s, t) => Array.Empty<AttributeComparison>(), StrategyType.Generalized);
        sut.AddMatcher((ctx, s, t) => Array.Empty<Comparison>(), StrategyType.Generalized);
        sut.HasMatchers.ShouldBeTrue();
    }

    [Fact(DisplayName = "When no comparer have been added, HasComparers returns false")]
    public void Test301()
    {
        var sut = new DiffingStrategyPipeline();
        sut.HasComparers.ShouldBeFalse();
    }

    [Fact(DisplayName = "When either a node or attribute comparer are missing, HasComparers returns false")]
    public void Test302()
    {
        var sutWithAttrComparer = new DiffingStrategyPipeline();
        sutWithAttrComparer.AddComparer((in AttributeComparison c, CompareResult current) => current, StrategyType.Generalized);
        sutWithAttrComparer.HasComparers.ShouldBeFalse();

        var sutWithNodeComparer = new DiffingStrategyPipeline();
        sutWithNodeComparer.AddComparer((in Comparison c, CompareResult current) => current, StrategyType.Generalized);
        sutWithNodeComparer.HasComparers.ShouldBeFalse();
    }

    [Fact(DisplayName = "When at least one node and attribute comparer are added, HasComparers returns true")]
    public void Test303()
    {
        var sut = new DiffingStrategyPipeline();
        sut.AddComparer((in AttributeComparison c, CompareResult current) => current, StrategyType.Generalized);
        sut.AddComparer((in Comparison c, CompareResult current) => current, StrategyType.Generalized);
        sut.HasComparers.ShouldBeTrue();
    }
}