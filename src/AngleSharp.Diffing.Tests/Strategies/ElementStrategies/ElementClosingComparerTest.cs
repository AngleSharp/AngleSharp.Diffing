using System.Linq;

using AngleSharp.Diffing.Core;

using Shouldly;

using Xunit;

namespace AngleSharp.Diffing.Strategies.ElementStrategies
{
    public class ElementClosingComparerTest : DiffingTestBase
    {
        public ElementClosingComparerTest(DiffingTestFixture fixture) : base(fixture)
        {
        }

        [Fact(DisplayName = "When control and test nodes have are both self closed, the result is Same")]
        public void Test001()
        {
            var comparison = ToComparison("<v:image />", "<v:image />");

            ElementClosingComparer.Compare(comparison, CompareResult.Unknown).ShouldBe(CompareResult.Same);
        }

        [Fact(DisplayName = "When control and test nodes have are both not self closed, the result is Same")]
        public void Test002()
        {
            var comparison = ToComparison("<v:image />", "<v:image />");

            ElementClosingComparer.Compare(comparison, CompareResult.Unknown).ShouldBe(CompareResult.Same);
        }

        [Fact(DisplayName = "When either control or test node is self closed, the result is Same")]
        public void Test003()
        {
            var comparison = ToComparison("<v:image />", "<v:image>");

            ElementClosingComparer.Compare(comparison, CompareResult.Unknown).ShouldBe(CompareResult.Different);
        }
    }
}
