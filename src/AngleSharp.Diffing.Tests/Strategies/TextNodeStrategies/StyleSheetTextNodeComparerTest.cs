using AngleSharp.Diffing.Core;

using Shouldly;

using Xunit;

namespace AngleSharp.Diffing.Strategies.TextNodeStrategies
{
    public class StyleSheetTextNodeComparerTest : TextNodeTestBase
    {
        public StyleSheetTextNodeComparerTest(DiffingTestFixture fixture) : base(fixture)
        {
        }

        [Fact(DisplayName = "When input node is not a IText node, comparer does not run nor change the current decision")]
        public void Test000()
        {
            var comparison = ToComparison("<p></p>", "<p></p>");

            StyleSheetTextNodeComparer.Compare(comparison, CompareResult.Different).ShouldBe(CompareResult.Different);
            StyleSheetTextNodeComparer.Compare(comparison, CompareResult.Same).ShouldBe(CompareResult.Same);
            StyleSheetTextNodeComparer.Compare(comparison, CompareResult.Skip).ShouldBe(CompareResult.Skip);
        }

        [Fact(DisplayName = "When input node is a IText node inside an element that is NOT a style tag, comparer does not run nor change the current decision")]
        public void Test0001()
        {
            var comparison = ToComparison("<p>h1{background:#000;}</p>", "<p>h1{background:#000;}</p>");

            StyleSheetTextNodeComparer.Compare(comparison, CompareResult.Different).ShouldBe(CompareResult.Different);
            StyleSheetTextNodeComparer.Compare(comparison, CompareResult.Same).ShouldBe(CompareResult.Same);
            StyleSheetTextNodeComparer.Compare(comparison, CompareResult.Skip).ShouldBe(CompareResult.Skip);
        }


        [Fact(DisplayName = "The comparer responses with Different when style information is different")]
        public void Test001()
        {
            var comparison = ToStyleComparison(@"h1{background:#000;}", @"h1{color:#000;}");

            StyleSheetTextNodeComparer.Compare(comparison, CompareResult.Different).ShouldBe(CompareResult.Different);
        }

        [Theory(DisplayName = "The comparer ignores insignificant whitespace")]
        [InlineData(" ")]
        [InlineData("  ")]
        [InlineData("\t")]
        [InlineData("\n")]
        [InlineData("\n\r")]
        public void Test003(string whitespace)
        {
            var comparison = ToStyleComparison($@"h1{whitespace}{{{whitespace}color:{whitespace}#000;{whitespace}}}", @"h1{color:#000;}");

            StyleSheetTextNodeComparer.Compare(comparison, CompareResult.Different).ShouldBe(CompareResult.Same);
        }

        private Comparison ToStyleComparison(string controlStyleText, string testStyleText)
        {
            var controlStyle = ToComparisonSource($@"<style type=""text/css"">{controlStyleText}</style>");
            var controlSource = new ComparisonSource(controlStyle.Node.FirstChild, 0, controlStyle.Path, ComparisonSourceType.Control);
            var testStyle = ToComparisonSource($@"<style type=""text/css"">{testStyleText}</style>");
            var testSource = new ComparisonSource(testStyle.Node.FirstChild, 0, testStyle.Path, ComparisonSourceType.Test);
            return new Comparison(controlSource, testSource);
        }
    }
}
