using System;
using System.Linq;
using Egil.AngleSharp.Diffing.Core;
using Shouldly;
using Xunit;

namespace Egil.AngleSharp.Diffing
{
    public class DiffingStrategyPipelineTest : DiffingTestBase
    {
        private DiffContext _dummyContext = new DiffContext(null, null);

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

        [Theory(DisplayName = "When one or more filter strategy is added, the last decides the outcome")]
        [InlineData(FilterDecision.Keep)]
        [InlineData(FilterDecision.Exclude)]
        public void Test5(FilterDecision expected)
        {
            var sut = new DiffingStrategyPipeline();

            sut.AddFilter((in ComparisonSource s, FilterDecision currentDecision) => NegateDecision(expected));
            sut.AddFilter((in ComparisonSource s, FilterDecision currentDecision) => expected);
            sut.AddFilter((in AttributeComparisonSource s, FilterDecision currentDecision) => NegateDecision(expected));
            sut.AddFilter((in AttributeComparisonSource s, FilterDecision currentDecision) => expected);

            sut.Filter(new ComparisonSource()).ShouldBe(expected);
            sut.Filter(new AttributeComparisonSource()).ShouldBe(expected);
        }

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        [Fact(DisplayName = "When no matcher strategy have been added, none comparisons are returned")]
        public void Test2()
        {
            var sut = new DiffingStrategyPipeline();

            sut.Match(null, null, (SourceCollection)null).ShouldBeEmpty();
            sut.Match(null, null, (SourceMap)null).ShouldBeEmpty();
        }
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

        [Fact(DisplayName = "Node Matchers are allowed to match in the order they are added in")]
        public void Test6()
        {
            var sourceColllection = ToSourceCollection("<p></p><span></span>", ComparisonSourceType.Control);
            var sut = new DiffingStrategyPipeline();
            sut.AddMatcher((ctx, s, t) => new[] { new Comparison(s[0], t[0]) });
            sut.AddMatcher((ctx, s, t) => new[] { new Comparison(s[1], t[1]) });

            var result = sut.Match(_dummyContext, sourceColllection, sourceColllection).ToList();

            result[0].Control.ShouldBe(sourceColllection[0]);
            result[1].Control.ShouldBe(sourceColllection[1]);
        }

        [Fact(DisplayName = "Attributes Matchers are allowed to match in the order they are added in")]
        public void Test7()
        {
            var sourceMap = ToSourceMap(@"<p foo=""bar"" baz=""bum""></p>");
            var sut = new DiffingStrategyPipeline();
            sut.AddMatcher((ctx, s, t) => new[] { new AttributeComparison(s["foo"], t["foo"]) });
            sut.AddMatcher((ctx, s, t) => new[] { new AttributeComparison(s["baz"], t["baz"]) });

            var result = sut.Match(_dummyContext, sourceMap, sourceMap).ToList();

            result[0].Control.ShouldBe(sourceMap["foo"]);
            result[1].Control.ShouldBe(sourceMap["baz"]);
        }

        [Fact(DisplayName = "When no compare strategy have been added, DifferentAndBreak is returned for node comparison and Different for attributes")]
        public void Test3()
        {
            var sut = new DiffingStrategyPipeline();

            sut.Compare(new Comparison()).ShouldBe(CompareResult.DifferentAndBreak);
            sut.Compare(new AttributeComparison()).ShouldBe(CompareResult.Different);
        }

        [Theory(DisplayName = "When multiple comparers are added, the last decides the outcome")]
        [InlineData(CompareResult.Different, CompareResult.Same)]
        [InlineData(CompareResult.Same, CompareResult.Different)]
        [InlineData(CompareResult.Same, CompareResult.SameAndBreak)]
        [InlineData(CompareResult.Different, CompareResult.DifferentAndBreak)]

        public void Test8(CompareResult first, CompareResult final)
        {
            var sut = new DiffingStrategyPipeline();

            sut.AddComparer((in Comparison c, CompareResult current) => first);
            sut.AddComparer((in Comparison c, CompareResult current) => final);
            sut.AddComparer((in AttributeComparison c, CompareResult current) => first);
            sut.AddComparer((in AttributeComparison c, CompareResult current) => final);

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
            sut.AddMatcher((ctx, s, t) => new[] { new Comparison(s[0], t[0]) });

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
            sut.AddMatcher((ctx, s, t) => new[] { new AttributeComparison(s["foo"], t["foo"]) });

            var result = sut.Match(context, controlSources, testSources).ToList();

            controlSources.GetUnmatched().ShouldBeEmpty();
            testSources.GetUnmatched().ShouldBeEmpty();
        }
    }
}