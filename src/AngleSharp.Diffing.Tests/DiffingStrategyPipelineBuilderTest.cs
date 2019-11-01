using System;
using System.Linq;
using AngleSharp.Diffing.Core;
using Shouldly;
using Xunit;

namespace AngleSharp.Diffing
{
    public class DiffingStrategyPipelineBuilderTest : DiffingTestBase
    {
        public DiffingStrategyPipelineBuilderTest(DiffingTestFixture fixture) : base(fixture)
        {
        }

        [Fact(DisplayName = "Calling .Build() without adding strategies throws")]
        public void Test1()
        {
            Should.Throw<InvalidOperationException>(
                () => new DiffingStrategyPipelineBuilder().Build());
        }

        [Fact(DisplayName = "Calling .Build() without adding compare strategies throws")]
        public void Test2()
        {
            Should.Throw<InvalidOperationException>(
                () => new DiffingStrategyPipelineBuilder()
                .WithOneToOneNodeMatcher()
                .WithAttributeNameMatcher()
                .Build());
        }

        [Fact(DisplayName = "Calling .Build() without adding matcher strategies throws")]
        public void Test3()
        {
            Should.Throw<InvalidOperationException>(
                () => new DiffingStrategyPipelineBuilder()
                .WithClassAttributeComparer()
                .WithNodeNameComparer()
                .Build());
        }

        [Fact(DisplayName = "Adding custom strategies works")]
        public void Test4()
        {
            var context = new DiffContext(null, null);
            var nodeFilterCalled = false;
            var attrFilterCalled = false;
            var nodeMatcherCalled = false;
            var attrMatcherCalled = false;
            var nodeComparerCalled = false;
            var attrComparerCalled = false;

            var pipeline = new DiffingStrategyPipelineBuilder()
                .WithFilter((in ComparisonSource source, FilterDecision currentDecision) => { nodeFilterCalled = true; return currentDecision; })
                .WithFilter((in AttributeComparisonSource source, FilterDecision currentDecision) => { attrFilterCalled = true; return currentDecision; })
                .WithMatcher((ctx, ctrlSrc, testSrc) => { nodeMatcherCalled = true; return Array.Empty<Comparison>(); })
                .WithMatcher((ctx, ctrlSrc, testSrc) => { attrMatcherCalled = true; return Array.Empty<AttributeComparison>(); })
                .WithComparer((in Comparison comparison, CompareResult currentDecision) => { nodeComparerCalled = true; return currentDecision; })
                .WithComparer((in AttributeComparison comparison, CompareResult currentDecision) => { attrComparerCalled = true; return currentDecision; })
                .Build();

            pipeline.Filter(ToComparisonSource("<p>"));
            pipeline.Filter(ToAttributeComparisonSource("<p foo>", "foo"));
            pipeline.Match(context, ToSourceCollection("<p>"), ToSourceCollection("<p>")).ToList();
            pipeline.Match(context, ToSourceMap("<p foo>"), ToSourceMap("<p foo>")).ToList();
            pipeline.Compare(ToComparison("<p>","<p>"));
            pipeline.Compare(ToAttributeComparison("<p foo>", "foo", "<p foo>", "foo"));

            nodeFilterCalled.ShouldBeTrue();
            attrFilterCalled.ShouldBeTrue();
            nodeMatcherCalled.ShouldBeTrue();
            attrMatcherCalled.ShouldBeTrue();
            nodeComparerCalled.ShouldBeTrue();
            attrComparerCalled.ShouldBeTrue();
        }
    }
}
