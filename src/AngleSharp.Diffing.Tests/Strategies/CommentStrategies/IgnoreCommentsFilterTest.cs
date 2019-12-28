using AngleSharp.Diffing.Core;

using Shouldly;

using Xunit;

namespace AngleSharp.Diffing.Strategies.CommentStrategies
{
    public class IgnoreCommentsFilterTest : DiffingTestBase
    {
        public IgnoreCommentsFilterTest(DiffingTestFixture fixture) : base(fixture)
        {
        }

        [Theory(DisplayName = "Comment nodes and their child node are filtered out")]
        [InlineData("<!---->")]
        [InlineData("<!-- comment -->")]
        public void Test1(string html)
        {
            var commentSource = ToComparisonSource(html);

            IgnoreCommentsFilter.Filter(commentSource, FilterDecision.Keep).ShouldBe(FilterDecision.Exclude);
        }

        [Theory(DisplayName = "When called with a none-comment node, the current decision is returned")]
        [InlineData("Textnode")]
        [InlineData("<p></p>")]
        public void Test2(string html)
        {
            var source = ToComparisonSource(html);

            IgnoreCommentsFilter.Filter(source, FilterDecision.Keep).ShouldBe(FilterDecision.Keep);
        }
    }
}
