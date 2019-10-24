using Shouldly;
using Xunit;

namespace Egil.AngleSharp.Diffing.Core.Diffs
{
    public class UnexpectedDiffBaseTest : DiffingTestBase
    {
        protected ComparisonSource Test { get; }
        protected ComparisonSource TestOther { get; }

        public UnexpectedDiffBaseTest(DiffingTestFixture fixture) : base(fixture)
        {
            var node = ToNode("<br>");
            var text = ToNode("text node");
            Test = new ComparisonSource(node, 1, "path", ComparisonSourceType.Test);
            TestOther = new ComparisonSource(text, 2, "path/other", ComparisonSourceType.Test);
        }

        [Fact(DisplayName = "Two diffs are equal if all their properties are equal")]
        public void Test001()
        {
            var diff = new UnexpectedNodeDiff(Test);
            var otherdiff = new UnexpectedNodeDiff(Test);

            diff.Equals(otherdiff).ShouldBeTrue();
            diff.Equals((object)otherdiff).ShouldBeTrue();
            (diff == otherdiff).ShouldBeTrue();
            (diff != otherdiff).ShouldBeFalse();
            ((null as UnexpectedNodeDiff)! == (null as UnexpectedNodeDiff)!).ShouldBeTrue();
        }

        [Fact(DisplayName = "Two diffs are not equal if Control property are different")]
        public void Test002()
        {
            var diff = new UnexpectedNodeDiff(Test);
            var otherdiff = new UnexpectedNodeDiff(TestOther);

            diff.Equals(otherdiff).ShouldBeFalse();
            diff.Equals((object)otherdiff).ShouldBeFalse();
            (diff == otherdiff).ShouldBeFalse();
            (diff != otherdiff).ShouldBeTrue();
            ((null as UnexpectedNodeDiff)! == diff).ShouldBeFalse();
            (diff == (null as UnexpectedNodeDiff)!).ShouldBeFalse();
        }

        [Fact(DisplayName = "GetHashCode correctly returns same value for two equal diffs")]
        public void Test003()
        {
            var diff = new UnexpectedNodeDiff(Test);
            var otherdiff = new UnexpectedNodeDiff(Test);

            diff.GetHashCode().ShouldBe(otherdiff.GetHashCode());
        }

        [Fact(DisplayName = "GetHashCode correctly returns different value for two different diffs")]
        public void Test004()
        {
            var diff = new UnexpectedNodeDiff(Test);
            var otherdiff = new UnexpectedNodeDiff(TestOther);

            diff.GetHashCode().ShouldNotBe(otherdiff.GetHashCode());
        }
    }
}
