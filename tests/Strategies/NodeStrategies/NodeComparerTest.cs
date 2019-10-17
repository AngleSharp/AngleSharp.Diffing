using Egil.AngleSharp.Diffing.Core;
using Shouldly;
using Xunit;

namespace Egil.AngleSharp.Diffing.Strategies.NodeStrategies
{
    public class NodeComparerTest : DiffingTestBase
    {
        [Fact(DisplayName = "When control and test nodes have the same type and name, the result is Same")]
        public void Test001()
        {
            var comparison = ToComparison("<p>", "<p>");
            NodeComparer.Compare(comparison, CompareResult.Different).ShouldBe(CompareResult.Same);
        }

        [Theory(DisplayName = "When control and test nodes have the a different type and name, the result is Different")]
        [InlineData("<div>", "<p>")]
        [InlineData("<div>", "textnode")]
        [InlineData("<div>", "<!--comment-->")]
        [InlineData("<!--comment-->", "textnode")]
        public void Test002(string controlHtml, string testHtml)
        {
            var comparison = ToComparison(controlHtml, testHtml);
            NodeComparer.Compare(comparison, CompareResult.Different).ShouldBe(CompareResult.Different);
        }
    }
}
