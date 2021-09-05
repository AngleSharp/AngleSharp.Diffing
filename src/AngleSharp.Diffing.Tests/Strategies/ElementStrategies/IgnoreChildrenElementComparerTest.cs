using System.Linq;

using AngleSharp.Diffing.Core;

using Shouldly;

using Xunit;

namespace AngleSharp.Diffing.Strategies.ElementStrategies
{
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
            var comparison = ToComparison(controlHtml, "<p><em></em></p>");

            IgnoreChildrenElementComparer.Compare(comparison, CompareResult.Different).ShouldBe(CompareResult.Different);
            IgnoreChildrenElementComparer.Compare(comparison, CompareResult.Same).ShouldBe(CompareResult.Same);
            IgnoreChildrenElementComparer.Compare(comparison, CompareResult.Skip).ShouldBe(CompareResult.Skip);
        }

        [Theory(DisplayName = "When a control element has 'diff:ignoreChildren' attribute, SameAndBreak is returned")]
        [InlineData(@"<p diff:ignoreChildren></p>")]
        [InlineData(@"<p diff:ignoreChildren=""true""></p>")]
        [InlineData(@"<p diff:ignoreChildren=""TRUE""></p>")]
        [InlineData(@"<p diff:ignoreChildren=""TrUe""></p>")]
        public void Test002(string controlHtml)
        {
            var comparison = ToComparison(controlHtml, "<p><em></em></p>");

            IgnoreChildrenElementComparer.Compare(comparison, CompareResult.Same).ShouldBe(CompareResult.Same | CompareResult.SkipChildren);
            IgnoreChildrenElementComparer.Compare(comparison, CompareResult.Different).ShouldBe(CompareResult.Different | CompareResult.SkipChildren);
        }

        [Fact(DisplayName = "When a control element has 'diff:ignoreChildren', calling Build() with DefaultOptions() returns expected diffs")]
        public void Test003()
        {
            var control = @"<p attr=""foo"" missing diff:ignoreChildren>hello <em>world</em></p>";
            var test    = @"<p attr=""bar"" unexpected>world says <strong>hello</strong></p>";

            var diffs = DiffBuilder
                .Compare(control)
                .WithTest(test)
                .Build()
                .ToList();

            diffs.Count.ShouldBe(3);
            diffs.SingleOrDefault(x => x is AttrDiff).ShouldNotBeNull();
            diffs.SingleOrDefault(x => x is MissingAttrDiff).ShouldNotBeNull();
            diffs.SingleOrDefault(x => x is UnexpectedAttrDiff).ShouldNotBeNull();
        }

        [Theory(DisplayName = "When a control element has 'diff:ignoreChildren', calling Build() with DefaultOptions() returns empty diffs")]
        [InlineData(@"<p attr=""foo"" diff:ignoreChildren>hello <em>world</em></p>",
                        @"<p attr=""foo"">world says <strong>hello</strong></p>")]
        [InlineData(@"<p attr=""foo"" expected diff:ignoreChildren>hello <em>world</em></p>",
                        @"<p attr=""foo"" expected>world says <strong>hello</strong></p>")]
        public void Test004(string control, string test)
        {
            var diffs = DiffBuilder
                .Compare(control)
                .WithTest(test)
                .Build()
                .ToList();

            diffs.ShouldBeEmpty();
        }
    }
}
