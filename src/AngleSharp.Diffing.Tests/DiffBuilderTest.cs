using AngleSharp.Diffing.Strategies;
using AngleSharp.Diffing.Strategies.AttributeStrategies;
using AngleSharp.Diffing.Strategies.TextNodeStrategies;
using AngleSharp.Diffing.TestData;

namespace AngleSharp.Diffing;


public class DiffBuilderTest
{
    [Fact(DisplayName = "Control and test html are set correctly")]
    public void Test001()
    {
        var control = "<p>control</p>";
        var test = "<p>test</p>";

        var sut = DiffBuilder
            .Compare(control)
            .WithTest(test);

        sut.Control.ShouldBe(control);
        sut.Test.ShouldBe(test);
    }

    [Fact(DisplayName = "Builder throws if null is passed to control and test")]
    public void Test002()
    {
        Should.Throw<ArgumentNullException>(() => DiffBuilder.Compare(null!)).ParamName.ShouldBe("value");
        Should.Throw<ArgumentNullException>(() => DiffBuilder.Compare("").WithTest(null!)).ParamName.ShouldBe("value");
    }

    [Fact(DisplayName = "Calling Build() with DefaultOptions() returns expected diffs")]
    public void Test003()
    {
        var control = @"<p attr=""foo"" missing>hello <em>world</em></p>";
        var test = @"<p attr=""bar"" unexpected>world says <strong>hello</strong></p>";

        var diffs = DiffBuilder
            .Compare(control)
            .WithTest(test)
            .Build()
            .ToList();

        diffs.Count.ShouldBe(6);
        diffs.SingleOrDefault(x => x is AttrDiff).ShouldNotBeNull();
        diffs.SingleOrDefault(x => x is MissingAttrDiff).ShouldNotBeNull();
        diffs.SingleOrDefault(x => x is UnexpectedAttrDiff).ShouldNotBeNull();
        diffs.SingleOrDefault(x => x is NodeDiff).ShouldNotBeNull();
        diffs.SingleOrDefault(x => x is MissingNodeDiff).ShouldNotBeNull();
        diffs.SingleOrDefault(x => x is UnexpectedNodeDiff).ShouldNotBeNull();
    }

    [Fact(DisplayName = "Setting options works")]
    public void Test004()
    {
        var control = "<p foo>hello world</p>";
        var test = "<p foo>hello world</p>";

        var nodeFilterCalled = false;
        var attrFilterCalled = false;
        var nodeMatcherCalled = false;
        var attrMatcherCalled = false;
        var nodeComparerCalled = false;
        var attrComparerCalled = false;

        var diffs = DiffBuilder
            .Compare(control)
            .WithTest(test)
            .WithOptions(options => options
                .AddDefaultOptions()
                .AddFilter((in ComparisonSource source, FilterDecision currentDecision) => { nodeFilterCalled = true; return currentDecision; })
                .AddFilter((in AttributeComparisonSource source, FilterDecision currentDecision) => { attrFilterCalled = true; return currentDecision; })
                .AddMatcher((ctx, ctrlSrc, testSrc) => { nodeMatcherCalled = true; return Array.Empty<Comparison>(); })
                .AddMatcher((ctx, ctrlSrc, testSrc) => { attrMatcherCalled = true; return Array.Empty<AttributeComparison>(); })
                .AddComparer((in Comparison comparison, CompareResult currentDecision) => { nodeComparerCalled = true; return currentDecision; })
                .AddComparer((in AttributeComparison comparison, CompareResult currentDecision) => { attrComparerCalled = true; return currentDecision; })
            )
            .Build()
            .ToList();

        nodeFilterCalled.ShouldBeTrue();
        attrFilterCalled.ShouldBeTrue();
        nodeMatcherCalled.ShouldBeTrue();
        attrMatcherCalled.ShouldBeTrue();
        nodeComparerCalled.ShouldBeTrue();
        attrComparerCalled.ShouldBeTrue();
    }

    [Theory(DisplayName = "When a control element has 'diff:ignoreChildren', calling Build() with DefaultOptions() returns empty diffs")]
    [InlineData(@"<p diff:ignoreChildren>hello <em>world</em></p>",
                     @"<p>world says <strong>hello</strong></p>")]
    [InlineData(@"<p diff:ignoreChildren>hello</p>",
                     @"<p>world says <strong>hello</strong></p>")]
    [InlineData(@"<p diff:ignoreChildren>hello <em>world</em></p>",
                     @"<p>world says</p>")]
    public void Test005(string control, string test)
    {
        var diffs = DiffBuilder
            .Compare(control)
            .WithTest(test)
            .Build()
            .ToList();

        diffs.ShouldBeEmpty();
    }

    [Theory(DisplayName = "When a control element has 'diff:ignoreAttributes', calling Build() with DefaultOptions() returns empty diffs")]
    [InlineData(@"<p id=""foo"" diff:ignoreAttributes></p>",
                     @"<p id=""bar""></p>")]
    [InlineData(@"<p diff:ignoreAttributes></p>",
                     @"<p unexpected></p>")]
    [InlineData(@"<p id=""foo"" diff:ignoreAttributes></p>",
                     @"<p></p>")]
    public void Test006(string control, string test)
    {
        var diffs = DiffBuilder
            .Compare(control)
            .WithTest(test)
            .Build()
            .ToList();

        diffs.ShouldBeEmpty();
    }

    [Theory(DisplayName =
        "When a control element has ':ignore', elements with and without that attribute should return empty diffs")]
    [MemberData(nameof(IgnoreAttributeTestData.ControlAndHtmlData), MemberType = typeof(IgnoreAttributeTestData))]
    public void Test007(string controlHtml, string testHtml)
    {
        var diffs = DiffBuilder.Compare(controlHtml).WithTest(testHtml).Build();
        Assert.Empty(diffs);
    }

    [Theory(DisplayName =
        "When a control element has ':ignore', but IgnoreAttributeComparer is not active, diffs should be found")]
    [MemberData(nameof(IgnoreAttributeTestData.ControlHtmlAndDiffData), MemberType = typeof(IgnoreAttributeTestData))]
    public void Test008(string controlHtml, string testHtml, DiffResult expectedDiffResult)
    {
        var diffs = DiffBuilder
            .Compare(controlHtml)
            .WithTest(testHtml)
            .WithOptions(a => a // Most important thing to note here is we do not have a ignore attribute comparer
                .AddSearchingNodeMatcher()
                .AddMatcher(AttributeNameMatcher.Match, StrategyType.Generalized)
                .AddElementComparer(enforceTagClosing: false)
                .AddMatcher(PostfixedAttributeMatcher.Match, StrategyType.Specialized)
                .AddComparer(AttributeComparer.Compare, StrategyType.Generalized)
                .AddClassAttributeComparer()
                .AddBooleanAttributeComparer(BooleanAttributeComparision.Strict)
                .AddStyleAttributeComparer())
            .Build()
            .ToList();

        Assert.Single(diffs);
        Assert.Equal(DiffTarget.Attribute, diffs[0].Target);
        Assert.Equal(expectedDiffResult, diffs[0].Result);
    }
}