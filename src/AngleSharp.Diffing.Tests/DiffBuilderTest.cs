using System;
using System.Linq;

using AngleSharp.Diffing.Core;

using Shouldly;

using Xunit;

namespace AngleSharp.Diffing
{

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
            Should.Throw<ArgumentNullException>(() => DiffBuilder.Compare(null!)).ParamName.ShouldBe(nameof(DiffBuilder.Control));
            Should.Throw<ArgumentNullException>(() => DiffBuilder.Compare("").WithTest(null!)).ParamName.ShouldBe(nameof(DiffBuilder.Test));
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
    }
}
