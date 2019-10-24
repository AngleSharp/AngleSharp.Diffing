using Shouldly;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Egil.AngleSharp.Diffing.Core.Diffs
{
    public class DiffBaseTest : DiffingTestBase
    {
        protected ComparisonSource Control { get; }
        protected ComparisonSource ControlOther { get; }
        protected ComparisonSource Test { get; }
        protected ComparisonSource TestOther { get; }

        public DiffBaseTest(DiffingTestFixture fixture) : base(fixture)
        {
            var node = ToNode("<br>");
            var text = ToNode("text node");
            Control = new ComparisonSource(node, 1, "path", ComparisonSourceType.Control);
            ControlOther = new ComparisonSource(text, 2, "path/other", ComparisonSourceType.Control);
            Test = new ComparisonSource(node, 1, "path", ComparisonSourceType.Test);
            TestOther = new ComparisonSource(text, 2, "path/other", ComparisonSourceType.Test);
        }

        [Fact(DisplayName = "Two diffs are equal if all their properties are equal")]
        public void Test001()
        {
            var diff = new Diff(new Comparison(Control, Test));
            var otherdiff = new Diff(new Comparison(Control, Test));

            diff.Equals(otherdiff).ShouldBeTrue();
            diff.Equals((object)otherdiff).ShouldBeTrue();
            (diff == otherdiff).ShouldBeTrue();
            (diff != otherdiff).ShouldBeFalse();
            ((null as Diff)! == (null as Diff)!).ShouldBeTrue();
        }

        [Fact(DisplayName = "Two diffs are not equal if Control property are different")]
        public void Test002()
        {
            var diff = new Diff(new Comparison(Control, Test));
            var otherdiff = new Diff(new Comparison(ControlOther, Test));

            diff.Equals(otherdiff).ShouldBeFalse();
            diff.Equals((object)otherdiff).ShouldBeFalse();
            (diff == otherdiff).ShouldBeFalse();
            (diff != otherdiff).ShouldBeTrue();
            ((null as Diff)! == diff).ShouldBeFalse();
            (diff == (null as Diff)!).ShouldBeFalse();
        }

        [Fact(DisplayName = "Two diffs are not equal if Test property are different")]
        public void Test003()
        {
            var diff = new Diff(new Comparison(Control, Test));
            var otherdiff = new Diff(new Comparison(Control, TestOther));

            diff.Equals(otherdiff).ShouldBeFalse();
            diff.Equals((object)otherdiff).ShouldBeFalse();
            (diff == otherdiff).ShouldBeFalse();
            (diff != otherdiff).ShouldBeTrue();
            ((null as Diff)! == diff).ShouldBeFalse();
            (diff == (null as Diff)!).ShouldBeFalse();
        }

        [Fact(DisplayName = "Two diffs are not equal if both Control and Test property are different")]
        public void Test004()
        {
            var diff = new Diff(new Comparison(Control, Test));
            var otherdiff = new Diff(new Comparison(ControlOther, TestOther));

            diff.Equals(otherdiff).ShouldBeFalse();
            diff.Equals((object)otherdiff).ShouldBeFalse();
            (diff == otherdiff).ShouldBeFalse();
            (diff != otherdiff).ShouldBeTrue();
            ((null as Diff)! == diff).ShouldBeFalse();
            (diff == (null as Diff)!).ShouldBeFalse();
        }

        [Fact(DisplayName = "GetHashCode correctly returns same value for two equal diffs")]
        public void Test5()
        {
            var diff = new Diff(new Comparison(Control, Test));
            var otherdiff = new Diff(new Comparison(Control, Test));

            diff.GetHashCode().ShouldBe(otherdiff.GetHashCode());
        }

        [Fact(DisplayName = "GetHashCode correctly returns different value for two different diffs")]
        public void Test006()
        {
            var diff = new MissingNodeDiff(Control);
            var otherdiff = new Diff(new Comparison(ControlOther, TestOther));

            diff.GetHashCode().ShouldNotBe(otherdiff.GetHashCode());
        }
    }
}
