using Egil.AngleSharp.Diffing.Core;
using Shouldly;
using Xunit;

namespace Egil.AngleSharp.Diffing.Strategies.TextNodeStrategies
{
    public class TextNodeComparerTest : TextnodeStrategyTestBase
    {
        [Theory(DisplayName = "When option is Preserve or RemoveWhitespaceNodes, comparer does not run nor change the current decision")]
        [InlineData(WhitespaceOption.Preserve)]
        [InlineData(WhitespaceOption.RemoveWhitespaceNodes)]
        public void Test5(WhitespaceOption whitespaceOption)
        {
            var comparison = new Comparison(ToComparisonSource("hello world", ComparisonSourceType.Control), ToComparisonSource("  hello   world  ", ComparisonSourceType.Test));
            var sut = new TextNodeComparer(whitespaceOption);

            sut.Compare(comparison, CompareResult.Different).ShouldBe(CompareResult.Different);
            sut.Compare(comparison, CompareResult.DifferentAndBreak).ShouldBe(CompareResult.DifferentAndBreak);
            sut.Compare(comparison, CompareResult.Same).ShouldBe(CompareResult.Same);
            sut.Compare(comparison, CompareResult.SameAndBreak).ShouldBe(CompareResult.SameAndBreak);
        }

        [Fact(DisplayName = "When option is Normalize and current decision is Same or SameAndBreak, compare uses the current decision")]
        public void Test55()
        {
            var comparison = new Comparison();
            var sut = new TextNodeComparer(WhitespaceOption.Normalize);
            sut.Compare(comparison, CompareResult.Same).ShouldBe(CompareResult.Same);
            sut.Compare(comparison, CompareResult.SameAndBreak).ShouldBe(CompareResult.SameAndBreak);
        }

        [Theory(DisplayName = "When option is Normalize, any whitespace before and after a text node is removed before comparison")]
        [MemberData(nameof(WhitespaceCharStrings))]
        public void Test7(string whitespace)
        {
            var sut = new TextNodeComparer(WhitespaceOption.Normalize);
            var normalText = "text";
            var whitespaceText = $"{whitespace}text{whitespace}";
            var c1 = new Comparison(ToComparisonSource(normalText, ComparisonSourceType.Control), ToComparisonSource(normalText, ComparisonSourceType.Test));
            var c2 = new Comparison(ToComparisonSource(normalText, ComparisonSourceType.Control), ToComparisonSource(whitespaceText, ComparisonSourceType.Test));
            var c3 = new Comparison(ToComparisonSource(whitespaceText, ComparisonSourceType.Control), ToComparisonSource(normalText, ComparisonSourceType.Test));
            var c4 = new Comparison(ToComparisonSource(whitespaceText, ComparisonSourceType.Control), ToComparisonSource(whitespaceText, ComparisonSourceType.Test));

            sut.Compare(c1, CompareResult.Different).ShouldBe(CompareResult.Same);
            sut.Compare(c2, CompareResult.Different).ShouldBe(CompareResult.Same);
            sut.Compare(c3, CompareResult.Different).ShouldBe(CompareResult.Same);
            sut.Compare(c4, CompareResult.Different).ShouldBe(CompareResult.Same);
        }

        [Theory(DisplayName = "When option is Normalize, any consecutive whitespace characters are collapsed into one before comparison")]
        [MemberData(nameof(WhitespaceCharStrings))]
        public void Test9(string whitespace)
        {
            var sut = new TextNodeComparer(WhitespaceOption.Normalize);
            var normalText = "hello world";
            var whitespaceText = $"{whitespace}hello{whitespace}{whitespace}world{whitespace}";
            var c1 = new Comparison(ToComparisonSource(normalText, ComparisonSourceType.Control), ToComparisonSource(normalText, ComparisonSourceType.Test));
            var c2 = new Comparison(ToComparisonSource(normalText, ComparisonSourceType.Control), ToComparisonSource(whitespaceText, ComparisonSourceType.Test));
            var c3 = new Comparison(ToComparisonSource(whitespaceText, ComparisonSourceType.Control), ToComparisonSource(normalText, ComparisonSourceType.Test));
            var c4 = new Comparison(ToComparisonSource(whitespaceText, ComparisonSourceType.Control), ToComparisonSource(whitespaceText, ComparisonSourceType.Test));

            sut.Compare(c1, CompareResult.Different).ShouldBe(CompareResult.Same);
            sut.Compare(c2, CompareResult.Different).ShouldBe(CompareResult.Same);
            sut.Compare(c3, CompareResult.Different).ShouldBe(CompareResult.Same);
            sut.Compare(c4, CompareResult.Different).ShouldBe(CompareResult.Same);
        }

        // When a parent node has overridden the global whitespace option, that overridden option is used
        // When whitespace option is Preserve or RemoveWhitespaceNodes, a string ordinal comparison is performed
        // When whitespace option is Preserve or RemoveWhitespaceNodes and IgnoreCase is true, a string ordinal ignore case comparison is performed
        // When IgnoreCase is true, a case insensitve comparison is performed
        // When the parent element is <pre>, the is implicitly set to Preserve, unless explicitly overridden on the element
        // When diff:regex attribute is found on the containing element, the control text is expected to a regex and that used when comparing to the test text node.
    }
}


