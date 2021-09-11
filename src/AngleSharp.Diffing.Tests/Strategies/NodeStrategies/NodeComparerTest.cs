using AngleSharp.Diffing.Core;
using AngleSharp.Diffing.Strategies.ElementStrategies;

using Shouldly;

using Xunit;

namespace AngleSharp.Diffing.Strategies.NodeStrategies
{
    public class NodeComparerTest : DiffingTestBase
    {
        public NodeComparerTest(DiffingTestFixture fixture) : base(fixture)
        {
        }

        [Fact(DisplayName = "When control and test nodes have the same type and name, the result is Same")]
        public void Test001()
        {
            var comparison = ToComparison("<p>", "<p>");
            ElementComparer.Compare(comparison, CompareResult.Unknown).ShouldBe(CompareResult.Same);
        }

        [Theory(DisplayName = "When control and test nodes have the a different type and name, the result is Different")]
        [InlineData("<div>", "<p>")]
        [InlineData("<div>", "textnode")]
        [InlineData("<div>", "<!--comment-->")]
        [InlineData("<!--comment-->", "textnode")]
        public void Test002(string controlHtml, string testHtml)
        {
            var comparison = ToComparison(controlHtml, testHtml);
            ElementComparer.Compare(comparison, CompareResult.Unknown).ShouldBe(CompareResult.Different);
        }
    }
}
