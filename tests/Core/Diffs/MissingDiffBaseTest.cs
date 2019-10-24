using Shouldly;
using Xunit;

namespace Egil.AngleSharp.Diffing.Core.Diffs
{
    public class MissingDiffBaseTest : DiffingTestBase
    {
        protected ComparisonSource Control { get; }
        protected ComparisonSource ControlOther { get; }

        public MissingDiffBaseTest(DiffingTestFixture fixture) : base(fixture)
        {
            var node = ToNode("<br>");
            var text = ToNode("text node");
            Control = new ComparisonSource(node, 1, "path", ComparisonSourceType.Control);
            ControlOther = new ComparisonSource(text, 2, "path/other", ComparisonSourceType.Control);
        }

        [Fact(DisplayName = "Two diffs are equal if all their properties are equal")]
        public void Test001()
        {
            var diff = new MissingNodeDiff(Control);
            var otherdiff = new MissingNodeDiff(Control);

            diff.Equals(otherdiff).ShouldBeTrue();
            diff.Equals((object)otherdiff).ShouldBeTrue();
            (diff == otherdiff).ShouldBeTrue();
            (diff != otherdiff).ShouldBeFalse();
            ((null as MissingNodeDiff)! == (null as MissingNodeDiff)!).ShouldBeTrue();
        }

        [Fact(DisplayName = "Two diffs are not equal if Control property are different")]
        public void Test002()
        {
            var diff = new MissingNodeDiff(Control);
            var otherdiff = new MissingNodeDiff(ControlOther);

            diff.Equals(otherdiff).ShouldBeFalse();
            diff.Equals((object)otherdiff).ShouldBeFalse();
            (diff == otherdiff).ShouldBeFalse();
            (diff != otherdiff).ShouldBeTrue();
            ((null as MissingNodeDiff)! == diff).ShouldBeFalse();
            (diff == (null as MissingNodeDiff)!).ShouldBeFalse();
        }

        [Fact(DisplayName = "GetHashCode correctly returns same value for two equal diffs")]
        public void Test003()
        {
            var diff = new MissingNodeDiff(Control);
            var otherdiff = new MissingNodeDiff(Control);

            diff.GetHashCode().ShouldBe(otherdiff.GetHashCode());
        }

        [Fact(DisplayName = "GetHashCode correctly returns different value for two different diffs")]
        public void Test004()
        {
            var diff = new MissingNodeDiff(Control);
            var otherdiff = new MissingNodeDiff(ControlOther);

            diff.GetHashCode().ShouldNotBe(otherdiff.GetHashCode());
        }
    }
}
