using System;
using System.Linq;
using Egil.AngleSharp.Diffing.Core;
using Shouldly;
using Xunit;

namespace Egil.AngleSharp.Diffing
{
    public class DiffingStrategyPipelineTest : DiffingTestBase
    {
        private readonly DiffContext _dummyContext = new DiffContext(null, null);

        public DiffingStrategyPipelineTest(DiffingTestFixture fixture) : base(fixture)
        {
        }

        private FilterDecision NegateDecision(FilterDecision decision) => decision switch
        {
            FilterDecision.Keep => FilterDecision.Exclude,
            FilterDecision.Exclude => FilterDecision.Keep,
            _ => throw new InvalidOperationException()
        };

        [Fact(DisplayName = "Wen zero filter strategies have been added, true is returned")]
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

            sut.AddFilter((in ComparisonSource s, FilterDecision currentDecision) => NegateDecision(expected), isSpecializedFilter: true);
            sut.AddFilter((in ComparisonSource s, FilterDecision currentDecision) => expected, isSpecializedFilter: true);
            sut.AddFilter((in AttributeComparisonSource s, FilterDecision currentDecision) => NegateDecision(expected), isSpecializedFilter: true);
            sut.AddFilter((in AttributeComparisonSource s, FilterDecision currentDecision) => expected, isSpecializedFilter: true);

            sut.Filter(new ComparisonSource()).ShouldBe(expected);
            sut.Filter(new AttributeComparisonSource()).ShouldBe(expected);
        }

        [Theory(DisplayName = "When one or more generalized filter strategies are added, the they are executed in the opposite order they are added in")]
        [InlineData(FilterDecision.Keep)]
        [InlineData(FilterDecision.Exclude)]
        public void Test002(FilterDecision expected)
        {
            var sut = new DiffingStrategyPipeline();

            sut.AddFilter((in ComparisonSource s, FilterDecision currentDecision) => expected, isSpecializedFilter: false);
            sut.AddFilter((in ComparisonSource s, FilterDecision currentDecision) => NegateDecision(expected), isSpecializedFilter: false);
            sut.AddFilter((in AttributeComparisonSource s, FilterDecision currentDecision) => expected, isSpecializedFilter: false);
            sut.AddFilter((in AttributeComparisonSource s, FilterDecision currentDecision) => NegateDecision(expected), isSpecializedFilter: false);

            sut.Filter(new ComparisonSource()).ShouldBe(expected);
            sut.Filter(new AttributeComparisonSource()).ShouldBe(expected);
        }

        [Theory(DisplayName = "Generalized filter strategies are always executed before specialized filter strategies")]
        [InlineData(FilterDecision.Keep)]
        [InlineData(FilterDecision.Exclude)]
        public void Test003(FilterDecision expected)
        {
            var sut = new DiffingStrategyPipeline();

            sut.AddFilter((in ComparisonSource s, FilterDecision currentDecision) => expected, isSpecializedFilter: true);
            sut.AddFilter((in ComparisonSource s, FilterDecision currentDecision) => NegateDecision(expected), isSpecializedFilter: false);
            sut.AddFilter((in AttributeComparisonSource s, FilterDecision currentDecision) => expected, isSpecializedFilter: true);
            sut.AddFilter((in AttributeComparisonSource s, FilterDecision currentDecision) => NegateDecision(expected), isSpecializedFilter: false);

            sut.Filter(new ComparisonSource()).ShouldBe(expected);
            sut.Filter(new AttributeComparisonSource()).ShouldBe(expected);
        }

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        [Fact(DisplayName = "When no matcher strategy have been added, no comparisons are returned")]
        public void Test2()
        {
            var sut = new DiffingStrategyPipeline();

            sut.Match(null, null, (SourceCollection)null).ShouldBeEmpty();
            sut.Match(null, null, (SourceMap)null).ShouldBeEmpty();
        }
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

        [Fact(DisplayName = "Specialized node matchers are executed in the reverse order they are added in")]
        public void Test61()
        {
            var sourceColllection = ToSourceCollection("<p></p><span></span>", ComparisonSourceType.Control);
            var sut = new DiffingStrategyPipeline();
            sut.AddMatcher((ctx, s, t) => new[] { new Comparison(s[0], t[0]) }, isSpecializedMatcher: true);
            sut.AddMatcher((ctx, s, t) => new[] { new Comparison(s[1], t[1]) }, isSpecializedMatcher: true);

            var result = sut.Match(_dummyContext, sourceColllection, sourceColllection).ToList();

            result[0].Control.ShouldBe(sourceColllection[1]);
            result[1].Control.ShouldBe(sourceColllection[0]);
        }

        [Fact(DisplayName = "Specialized attributes matchers are executed in the reverse order they are added in")]
        public void Test71()
        {
            var sourceMap = ToSourceMap(@"<p foo=""bar"" baz=""bum""></p>");
            var sut = new DiffingStrategyPipeline();
            sut.AddMatcher((ctx, s, t) => new[] { new AttributeComparison(s["foo"], t["foo"]) }, isSpecializedMatcher: true);
            sut.AddMatcher((ctx, s, t) => new[] { new AttributeComparison(s["baz"], t["baz"]) }, isSpecializedMatcher: true);

            var result = sut.Match(_dummyContext, sourceMap, sourceMap).ToList();

            result[0].Control.ShouldBe(sourceMap["baz"]);
            result[1].Control.ShouldBe(sourceMap["foo"]);
        }

        [Fact(DisplayName = "Generalized node matchers are executed in order they are added in")]
        public void Test62()
        {
            var sourceColllection = ToSourceCollection("<p></p><span></span>", ComparisonSourceType.Control);
            var sut = new DiffingStrategyPipeline();
            sut.AddMatcher((ctx, s, t) => new[] { new Comparison(s[0], t[0]) }, isSpecializedMatcher: false);
            sut.AddMatcher((ctx, s, t) => new[] { new Comparison(s[1], t[1]) }, isSpecializedMatcher: false);

            var result = sut.Match(_dummyContext, sourceColllection, sourceColllection).ToList();

            result[0].Control.ShouldBe(sourceColllection[0]);
            result[1].Control.ShouldBe(sourceColllection[1]);
        }

        [Fact(DisplayName = "Generalized attributes matchers are executed in the order they are added in")]
        public void Test72()
        {
            var sourceMap = ToSourceMap(@"<p foo=""bar"" baz=""bum""></p>");
            var sut = new DiffingStrategyPipeline();
            sut.AddMatcher((ctx, s, t) => new[] { new AttributeComparison(s["foo"], t["foo"]) }, isSpecializedMatcher: false);
            sut.AddMatcher((ctx, s, t) => new[] { new AttributeComparison(s["baz"], t["baz"]) }, isSpecializedMatcher: false);

            var result = sut.Match(_dummyContext, sourceMap, sourceMap).ToList();

            result[0].Control.ShouldBe(sourceMap["foo"]);
            result[1].Control.ShouldBe(sourceMap["baz"]);
        }

        [Fact(DisplayName = "Generalized node matchers always execute after specialized node matchers")]
        public void Test62123()
        {
            var sourceColllection = ToSourceCollection("<p></p><span></span>", ComparisonSourceType.Control);
            var sut = new DiffingStrategyPipeline();
            sut.AddMatcher((ctx, s, t) => new[] { new Comparison(s[0], t[0]) }, isSpecializedMatcher: false);
            sut.AddMatcher((ctx, s, t) => new[] { new Comparison(s[1], t[1]) }, isSpecializedMatcher: true);

            var result = sut.Match(_dummyContext, sourceColllection, sourceColllection).ToList();

            result[0].Control.ShouldBe(sourceColllection[1]);
            result[1].Control.ShouldBe(sourceColllection[0]);
        }

        [Fact(DisplayName = "Generalized attributes matchers always execute after specialized attribute matchers")]
        public void Test74342()
        {
            var sourceMap = ToSourceMap(@"<p foo=""bar"" baz=""bum""></p>");
            var sut = new DiffingStrategyPipeline();
            sut.AddMatcher((ctx, s, t) => new[] { new AttributeComparison(s["foo"], t["foo"]) }, isSpecializedMatcher: false);
            sut.AddMatcher((ctx, s, t) => new[] { new AttributeComparison(s["baz"], t["baz"]) }, isSpecializedMatcher: true);

            var result = sut.Match(_dummyContext, sourceMap, sourceMap).ToList();

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
        [InlineData(CompareResult.Different, CompareResult.Same)]
        [InlineData(CompareResult.Same, CompareResult.Different)]
        public void Test8(CompareResult first, CompareResult final)
        {
            var sut = new DiffingStrategyPipeline();

            sut.AddComparer((in Comparison c, CompareResult current) => first, isSpecializedComparer: true);
            sut.AddComparer((in Comparison c, CompareResult current) => final, isSpecializedComparer: true);
            sut.AddComparer((in AttributeComparison c, CompareResult current) => first, isSpecializedComparer: true);
            sut.AddComparer((in AttributeComparison c, CompareResult current) => final, isSpecializedComparer: true);

            sut.Compare(new Comparison()).ShouldBe(final);
            sut.Compare(new AttributeComparison()).ShouldBe(final);
        }

        [Theory(DisplayName = "Generalized comparers are executed in the reverse order they are added in")]
        [InlineData(CompareResult.Different, CompareResult.Same)]
        [InlineData(CompareResult.Same, CompareResult.Different)]
        public void Test12321(CompareResult first, CompareResult final)
        {
            var sut = new DiffingStrategyPipeline();

            sut.AddComparer((in Comparison c, CompareResult current) => final, isSpecializedComparer: false);
            sut.AddComparer((in Comparison c, CompareResult current) => first, isSpecializedComparer: false);
            sut.AddComparer((in AttributeComparison c, CompareResult current) => final, isSpecializedComparer: false);
            sut.AddComparer((in AttributeComparison c, CompareResult current) => first, isSpecializedComparer: false);

            sut.Compare(new Comparison()).ShouldBe(final);
            sut.Compare(new AttributeComparison()).ShouldBe(final);
        }

        [Theory(DisplayName = "Generalized comparers are always executed before specialized comparers")]
        [InlineData(CompareResult.Different, CompareResult.Same)]
        [InlineData(CompareResult.Same, CompareResult.Different)]
        public void Test8314(CompareResult first, CompareResult final)
        {
            var sut = new DiffingStrategyPipeline();

            sut.AddComparer((in Comparison c, CompareResult current) => first, isSpecializedComparer: false);
            sut.AddComparer((in Comparison c, CompareResult current) => final, isSpecializedComparer: true);
            sut.AddComparer((in AttributeComparison c, CompareResult current) => first, isSpecializedComparer: false);
            sut.AddComparer((in AttributeComparison c, CompareResult current) => final, isSpecializedComparer: true);

            sut.Compare(new Comparison()).ShouldBe(final);
            sut.Compare(new AttributeComparison()).ShouldBe(final);
        }

        [Fact(DisplayName = "After two nodes has been matched, they are marked as matched in the source collection")]
        public void Test101()
        {
            var context = new DiffContext(null, null);
            var controlSources = ToSourceCollection("<p></p>", ComparisonSourceType.Control);
            var testSources = ToSourceCollection("<p></p>", ComparisonSourceType.Test);
            var sut = new DiffingStrategyPipeline();
            sut.AddMatcher((ctx, s, t) => new[] { new Comparison(s[0], t[0]) }, isSpecializedMatcher: true);

            var result = sut.Match(context, controlSources, testSources).ToList();

            controlSources.GetUnmatched().ShouldBeEmpty();
            testSources.GetUnmatched().ShouldBeEmpty();
        }

        [Fact(DisplayName = "After two attributes has been matched, they are marked as matched in the source map")]
        public void Test102()
        {
            var context = new DiffContext(null, null);
            var controlSources = ToSourceMap(@"<p foo=""bar""></p>", ComparisonSourceType.Control);
            var testSources = ToSourceMap(@"<p foo=""bar""></p>", ComparisonSourceType.Test);
            var sut = new DiffingStrategyPipeline();
            sut.AddMatcher((ctx, s, t) => new[] { new AttributeComparison(s["foo"], t["foo"]) }, isSpecializedMatcher: true);

            var result = sut.Match(context, controlSources, testSources).ToList();

            controlSources.GetUnmatched().ShouldBeEmpty();
            testSources.GetUnmatched().ShouldBeEmpty();
        }
    }
}