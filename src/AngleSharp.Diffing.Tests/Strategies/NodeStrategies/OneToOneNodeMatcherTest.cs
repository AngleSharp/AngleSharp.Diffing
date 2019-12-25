using System.Linq;
using AngleSharp.Diffing.Core;
using Xunit;
using Shouldly;
using AngleSharp.Dom;

namespace AngleSharp.Diffing.Strategies.NodeStrategies
{
    public class OneToOneNodeMatcherTest : DiffingTestBase
    {
        public OneToOneNodeMatcherTest(DiffingTestFixture fixture) : base(fixture)
        {
        }

        [Fact(DisplayName = "With two equal length node lists, " +
                            "all control and test nodes are matched based on the order they are in the lists")]
        public void Test001()
        {
            var controls = ToSourceCollection("<p></p>text<!--comment-->", ComparisonSourceType.Control);
            var tests = ToSourceCollection("<!--comment--><p></p>text", ComparisonSourceType.Test);

            var actual = OneToOneNodeMatcher.Match(DummyContext, controls, tests).ToList();

            actual.Count.ShouldBe(3);
            actual[0].ShouldSatisfyAllConditions(
                c => c.Control.ShouldBe(controls[0]),
                c => c.Test.ShouldBe(tests[0])
            );
            actual[1].ShouldSatisfyAllConditions(
                c => c.Control.ShouldBe(controls[1]),
                c => c.Test.ShouldBe(tests[1])
            );
            actual[2].ShouldSatisfyAllConditions(
                c => c.Control.ShouldBe(controls[2]),
                c => c.Test.ShouldBe(tests[2])
            );
        }

        [Theory(DisplayName = "With two unequal length node lists, " +
                            "as many control and test nodes are matched as the shortest list allow, " +
                            "based on the order they are in the lists")]
        [InlineData("<p></p>text", "<!--comment--><p></p>text", 2)]
        [InlineData("<!--comment--><p></p>text", "<p></p>text", 2)]
        [InlineData("<p></p>", "<!--comment--><p></p>", 1)]
        [InlineData("<!--comment--><p></p>", "<p></p>", 1)]
        [InlineData("<p></p>", "", 0)]
        [InlineData("", "<p></p>", 0)]
        public void Test002(string controlHtml, string testHtml, int matchCount)
        {
            var controls = ToSourceCollection(controlHtml, ComparisonSourceType.Control);
            var tests = ToSourceCollection(testHtml, ComparisonSourceType.Test);

            var actual = OneToOneNodeMatcher.Match(DummyContext, controls, tests).ToList();

            actual.Count.ShouldBe(matchCount);
            actual.ShouldAllBe((c, idx) => c.Control == controls[idx] && c.Test == tests[idx]);
        }

        [Fact(DisplayName = "When a control node has previous been matched, it is not matched again")]
        public void Test003()
        {
            var controls = ToSourceCollection("<p></p>text<!--comment-->", ComparisonSourceType.Control);
            var tests = ToSourceCollection("<!--comment--><p></p>text", ComparisonSourceType.Test);
            controls.MarkAsMatched(controls[1]);

            var actual = OneToOneNodeMatcher.Match(DummyContext, controls, tests).ToList();

            actual.Count.ShouldBe(2);
            actual[0].ShouldSatisfyAllConditions(
                c => c.Control.ShouldBe(controls[0]),
                c => c.Test.ShouldBe(tests[0])
            );
            actual[1].ShouldSatisfyAllConditions(
                c => c.Control.ShouldBe(controls[2]),
                c => c.Test.ShouldBe(tests[1])
            );
        }

        [Fact(DisplayName = "When a test node has previous been matched by another matcher, it is not matched again")]
        public void Test004()
        {
            var controls = ToSourceCollection("<p></p>text<!--comment-->", ComparisonSourceType.Control);
            var tests = ToSourceCollection("<!--comment--><p></p>text", ComparisonSourceType.Test);
            tests.MarkAsMatched(tests[1]);

            var actual = OneToOneNodeMatcher.Match(DummyContext, controls, tests).ToList();

            actual.Count.ShouldBe(2);
            actual[0].ShouldSatisfyAllConditions(
                c => c.Control.ShouldBe(controls[0]),
                c => c.Test.ShouldBe(tests[0])
            );
            actual[1].ShouldSatisfyAllConditions(
                c => c.Control.ShouldBe(controls[1]),
                c => c.Test.ShouldBe(tests[2])
            );
        }
    }
}
