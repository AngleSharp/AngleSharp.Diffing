using System.Linq;

using AngleSharp.Diffing.Core;

using Shouldly;

using Xunit;

namespace AngleSharp.Diffing.Strategies.ElementStrategies
{
    public class IgnoreAttributesElementComparerTest : DiffingTestBase
    {
        public IgnoreAttributesElementComparerTest(DiffingTestFixture fixture) : base(fixture)
        {
        }

        [Theory(DisplayName = "When a control element does not contain the 'diff:ignoreAttributes' attribute or it is 'diff:ignoreAttributes=false', the current decision is returned")]
        [InlineData(@"<p></p>")]
        [InlineData(@"<p diff:ignoreAttributes=""false""></p>")]
        [InlineData(@"<p diff:ignoreAttributes=""FALSE""></p>")]
        [InlineData(@"<p diff:ignoreAttributes=""faLsE""></p>")]
        public void Test001(string controlHtml)
        {
            var comparison = ToComparison(controlHtml, "<p></p>");

            IgnoreAttributesElementComparer.Compare(comparison, CompareResult.Different).ShouldBe(CompareResult.Different);
            IgnoreAttributesElementComparer.Compare(comparison, CompareResult.Same).ShouldBe(CompareResult.Same);
            IgnoreAttributesElementComparer.Compare(comparison, CompareResult.Skip).ShouldBe(CompareResult.Skip);
        }

        [Theory(DisplayName = "When a control element has 'diff:ignoreAttributes' attribute, CompareResult.SkipAttributes flag is returned")]
        [InlineData(@"<p diff:ignoreAttributes></p>")]
        [InlineData(@"<p diff:ignoreAttributes=""true""></p>")]
        [InlineData(@"<p diff:ignoreAttributes=""TRUE""></p>")]
        [InlineData(@"<p diff:ignoreAttributes=""TrUe""></p>")]
        public void Test002(string controlHtml)
        {
            var comparison = ToComparison(controlHtml, "<p></p>");

            IgnoreAttributesElementComparer.Compare(comparison, CompareResult.Same).ShouldBe(CompareResult.Same | CompareResult.SkipAttributes);
            IgnoreAttributesElementComparer.Compare(comparison, CompareResult.Different).ShouldBe(CompareResult.Different | CompareResult.SkipAttributes);
        }
    }
}
