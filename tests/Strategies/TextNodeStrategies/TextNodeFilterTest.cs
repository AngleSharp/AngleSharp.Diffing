using Shouldly;
using Xunit;

namespace Egil.AngleSharp.Diffing.Strategies.TextNodeStrategies
{
    public class TextNodeFilterTest : TextnodeStrategyTestBase
    {
        [Theory(DisplayName = "When whitespace option is Preserve, the provided decision is not changed by the filter for whitespace only text nodes")]
        [MemberData(nameof(WhitespaceCharStrings))]
        public void Test1(string whitespace)
        {
            var sut = new TextNodeFilter(WhitespaceOption.Preserve);
            var source = ToComparisonSource(whitespace);

            sut.Filter(source, FilterDecision.Keep).ShouldBe(FilterDecision.Keep);
            sut.Filter(source, FilterDecision.Exclude).ShouldBe(FilterDecision.Exclude);
        }

        [Theory(DisplayName = "When whitespace option is RemoveWhitespaceNodes, whitespace only text nodes are excluded during filtering")]
        [MemberData(nameof(WhitespaceCharStrings))]
        public void Test2(string whitespace)
        {
            var sut = new TextNodeFilter(WhitespaceOption.RemoveWhitespaceNodes);
            var source = ToComparisonSource(whitespace);

            sut.Filter(source, FilterDecision.Keep).ShouldBe(FilterDecision.Exclude);
        }

        [Theory(DisplayName = "When whitespace option is Normalize, whitespace only text nodes are excluded during filtering")]
        [MemberData(nameof(WhitespaceCharStrings))]
        public void Test3(string whitespace)
        {
            var sut = new TextNodeFilter(WhitespaceOption.Normalize);
            var source = ToComparisonSource(whitespace);

            sut.Filter(source, FilterDecision.Keep).ShouldBe(FilterDecision.Exclude);
        }

        [Theory(DisplayName = "Filter method doesn't change the decision of non-whitespace nodes or non-text nodes")]
        [InlineData("hello world")]
        [InlineData("<p>hello world</p>")]
        public void Test4x(string html)
        {
            var sut = new TextNodeFilter(WhitespaceOption.Normalize);
            var source = ToComparisonSource(html);

            sut.Filter(source, FilterDecision.Keep).ShouldBe(FilterDecision.Keep);
            sut.Filter(source, FilterDecision.Exclude).ShouldBe(FilterDecision.Exclude);
        }

        // When a parent node has overridden the global whitespace option, that overridden option is used
    }
}


