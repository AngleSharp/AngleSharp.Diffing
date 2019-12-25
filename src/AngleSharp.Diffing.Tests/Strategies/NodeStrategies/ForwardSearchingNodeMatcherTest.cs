using System.Linq;
using AngleSharp.Dom;
using AngleSharp.Diffing.Core;
using Shouldly;
using Xunit;

namespace AngleSharp.Diffing.Strategies.NodeStrategies
{
    public class ForwardSearchingNodeMatcherTest : DiffingTestBase
    {
        public ForwardSearchingNodeMatcherTest(DiffingTestFixture fixture) : base(fixture)
        {
        }

        [Theory(DisplayName = "The matcher matches two nodes with the same node name")]
        [InlineData("textnode", "textnode")]
        [InlineData("<!--comment-->", "<!--comment-->")]
        [InlineData("<p></p>", "<p></p>")]
        public void Test001(string controlHtml, string testHtml)
        {
            var controls = ToSourceCollection(controlHtml, ComparisonSourceType.Control);
            var tests = ToSourceCollection(testHtml, ComparisonSourceType.Test);

            var actual = ForwardSearchingNodeMatcher.Match(DummyContext, controls, tests).ToList();

            actual.Count.ShouldBe(1);
            actual.ShouldAllBe((c, idx) => c.Control == controls[idx] && c.Test == tests[idx]);
        }

        [Theory(DisplayName = "The matcher matches two nodes with the same node name")]
        [InlineData("asdf<h1>Hello world</h1>asdf<h1>Hello world</h1>", "asdf<h1>Hello world</h1>asdf<h1>Hello world</h1>")]
        public void Test0011(string controlHtml, string testHtml)
        {
            var controls = ToSourceCollection(controlHtml, ComparisonSourceType.Control);
            var tests = ToSourceCollection(testHtml, ComparisonSourceType.Test);
            controls.Remove((in ComparisonSource x) => x.Node is IElement ? FilterDecision.Keep : FilterDecision.Exclude);
            tests.Remove((in ComparisonSource x) => x.Node is IElement ? FilterDecision.Keep : FilterDecision.Exclude);

            var actual = ForwardSearchingNodeMatcher.Match(DummyContext, controls, tests).ToList();

            actual.Count.ShouldBe(2);
            actual.ShouldAllBe(c => c.Control == controls[c.Control.Index] && c.Test == tests[c.Test.Index]);
        }

        [Theory(DisplayName = "The matcher does not matches two nodes with the different node names")]
        [InlineData("textnode", "<!--comment-->")]
        [InlineData("textnode", "<p></p>")]
        [InlineData("<!--comment-->", "textnode")]
        [InlineData("<!--comment-->", "<p></p>")]
        [InlineData("<p></p>", "textnode")]
        [InlineData("<p></p>", "<!--comment-->")]
        public void Test002(string controlHtml, string testHtml)
        {
            var controls = ToSourceCollection(controlHtml, ComparisonSourceType.Control);
            var tests = ToSourceCollection(testHtml, ComparisonSourceType.Test);

            var actual = ForwardSearchingNodeMatcher.Match(DummyContext, controls, tests).ToList();

            actual.ShouldBeEmpty();
        }

        [Fact(DisplayName = "The matcher searches the test nodes for matches for each control node, " +
                            "starting after the last matched test node")]
        public void Test003()
        {
            var controls = ToSourceCollection("<h1></h1><div></div>", ComparisonSourceType.Control);
            var tests = ToSourceCollection("<p></p><h1></h1><div></div>", ComparisonSourceType.Test);

            var actual = ForwardSearchingNodeMatcher.Match(DummyContext, controls, tests).ToList();

            actual.Count.ShouldBe(2);
            actual[0].ShouldSatisfyAllConditions(
                c => c.Control.Index.ShouldBe(0),
                c => c.Test.Index.ShouldBe(1)
            );
            actual[1].ShouldSatisfyAllConditions(
                c => c.Control.Index.ShouldBe(1),
                c => c.Test.Index.ShouldBe(2)
            );
        }

        [Fact(DisplayName = "When the matcher does not find a match for a control node, " +
                            "it continues with the next control node and searches for a test node, " +
                            "starting after the last matched test node")]
        public void Test004()
        {
            var controls = ToSourceCollection("<foo></foo><div></div>", ComparisonSourceType.Control);
            var tests = ToSourceCollection("<div></div>", ComparisonSourceType.Test);

            var actual = ForwardSearchingNodeMatcher.Match(DummyContext, controls, tests).ToList();

            actual.Count.ShouldBe(1);
            actual[0].ShouldSatisfyAllConditions(
                c => c.Control.Index.ShouldBe(1),
                c => c.Test.Index.ShouldBe(0)
            );
        }

        [Fact(DisplayName = "When a test node has previous been matched by another matcher, it is not matched again")]
        public void Test005()
        {
            var controls = ToSourceCollection("<p></p>text<!--comment-->", ComparisonSourceType.Control);
            var tests = ToSourceCollection("<!--comment--><p></p>text", ComparisonSourceType.Test);
            controls.MarkAsMatched(controls[0]); // mark <p> as matched
            tests.MarkAsMatched(tests[2]); // mark text as matched

            var actual = ForwardSearchingNodeMatcher.Match(DummyContext, controls, tests).ToList();

            actual.Count.ShouldBe(1);
            actual[0].ShouldSatisfyAllConditions(
                c => c.Control.Index.ShouldBe(2),
                c => c.Test.Index.ShouldBe(0)
            );
        }
    }
}
