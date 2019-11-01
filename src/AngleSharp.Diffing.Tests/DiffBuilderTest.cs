using System;
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


        [Fact(DisplayName = "Calling .Build() without adding strategies throws")]
        public void Test3()
        {
            var control = "<p>control</p>";
            var test = "<p>test</p>";

            Should.Throw<InvalidOperationException>(() => DiffBuilder.Compare(control).WithTest(test).Build());
        }

        [Fact(DisplayName = "Calling .Build() without adding strategies throws")]
        public void Test4()
        {
            var control = "<p>control</p>";
            var test = "<p>test</p>";

            Should.Throw<InvalidOperationException>(
                () => DiffBuilder.Compare(control)
                .WithTest(test)
                .Build());
        }

        [Fact(DisplayName = "Calling .Build() without adding strategies throws")]
        public void Test5()
        {
            var control = "<p>control</p>";
            var test = "<p>test</p>";

            Should.Throw<InvalidOperationException>(
                () => DiffBuilder.Compare(control)
                .WithTest(test)
                .WithOneToOneNodeMatcher()
                .WithAttributeNameMatcher()
                .Build());
        }


        [Fact(DisplayName = "Calling Build() with DefaultOptions() returns expected diffs")]
        public void Test6()
        {
            var control = "<p>hello <em>world</em></p>";
            var test = "<p>world says <strong>hello</strong></p>";

            var diffs = DiffBuilder.Compare(control)
                .WithTest(test)
                .WithDefaultOptions()
                .Build();

            diffs.ShouldNotBeEmpty();
        }

        [Fact(DisplayName = "Adding custom strategies works")]
        public void Test7()
        {
            var nodeFilterCalled = false;
            var attrFilterCalled = false;
            var nodeMatcherCalled = false;
            var attrMatcherCalled = false;
            var nodeComparerCalled = false;
            var attrComparerCalled = false;
            var control = @"<p foo=""bar"">hello <em>world</em></p>";
            var test = @"<p foo=""bar"">world says <strong>hello</strong></p>";

            DiffBuilder.Compare(control)
                .WithTest(test)
                .WithDefaultOptions()
                .WithFilter((in ComparisonSource source, FilterDecision currentDecision) => { nodeFilterCalled = true; return currentDecision; })
                .WithFilter((in AttributeComparisonSource source, FilterDecision currentDecision) => { attrFilterCalled = true; return currentDecision; })
                .WithMatcher((ctx, ctrlSrc, testSrc) => { nodeMatcherCalled = true; return Array.Empty<Comparison>(); })
                .WithMatcher((ctx, ctrlSrc, testSrc) => { attrMatcherCalled = true; return Array.Empty<AttributeComparison>(); })
                .WithComparer((in Comparison comparison, CompareResult currentDecision) => { nodeComparerCalled = true; return currentDecision; })
                .WithComparer((in AttributeComparison comparison, CompareResult currentDecision) => { attrComparerCalled = true; return currentDecision; })
                .Build();

            nodeFilterCalled.ShouldBeTrue();
            attrFilterCalled.ShouldBeTrue();
            nodeMatcherCalled.ShouldBeTrue();
            attrMatcherCalled.ShouldBeTrue();
            nodeComparerCalled.ShouldBeTrue();
            attrComparerCalled.ShouldBeTrue();
        }
    }
}
