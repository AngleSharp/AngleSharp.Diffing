using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Dom;
using Egil.AngleSharp.Diffing.Core;
using Shouldly;
using Xunit;

namespace Egil.AngleSharp.Diffing
{
    public class DiffingStrategyPipelineTest : DiffingTestBase
    {
        [Fact(DisplayName = "Wen zero filter strategies have been added, true is returned")]
        public void Test1()
        {
            var sut = new DiffingStrategyPipeline();

            sut.Filter(new ComparisonSource()).ShouldBeTrue();
            sut.Filter(new AttributeComparisonSource()).ShouldBeTrue();
        }

        [Theory(DisplayName = "When one or more filter strategy is added, the last decides the outcome")]
        [InlineData(true)]
        [InlineData(false)]
        public void Test5(bool expected)
        {
            var sut = new DiffingStrategyPipeline();

            sut.AddFilter((in ComparisonSource s, bool currentDecision) => !expected);
            sut.AddFilter((in ComparisonSource s, bool currentDecision) => expected);
            sut.AddFilter((in AttributeComparisonSource s, bool currentDecision) => !expected);
            sut.AddFilter((in AttributeComparisonSource s, bool currentDecision) => expected);

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
            var sources = ToComparisonSourceList("<p></p><span></span>").ToList();
            var context = new DiffContext((IElement)sources[0].Node, (IElement)sources[0].Node);
            var sourceColllection = new SourceCollection(ComparisonSourceType.Control, sources);
            var sut = new DiffingStrategyPipeline();
            sut.AddMatcher((ctx, s, t) => new[] { new Comparison(s[0], t[0]) });
            sut.AddMatcher((ctx, s, t) => new[] { new Comparison(s[1], t[1]) });

            var result = sut.Match(context, sourceColllection, sourceColllection).ToList();

            result[0].Control.ShouldBe(sources[0]);
            result[1].Control.ShouldBe(sources[1]);
        }

        [Fact(DisplayName = "Attributes Matchers are allowed to match in the order they are added in")]
        public void Test7()
        {
            var source = ToComparisonSource(@"<p foo=""bar"" baz=""bum""></p>");
            var context = new DiffContext((IElement)source.Node, (IElement)source.Node);
            var sourceMap = new SourceMap(source);
            var sut = new DiffingStrategyPipeline();
            sut.AddMatcher((ctx, s, t) => new[] { new AttributeComparison(s["foo"], t["foo"]) });
            sut.AddMatcher((ctx, s, t) => new[] { new AttributeComparison(s["baz"], t["baz"]) });

            var result = sut.Match(context, sourceMap, sourceMap).ToList();

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

    }
}