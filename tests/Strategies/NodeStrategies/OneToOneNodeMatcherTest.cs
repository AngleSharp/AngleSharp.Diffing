using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Dom;
using Egil.AngleSharp.Diffing.Core;
using Xunit;
using Shouldly;

namespace Egil.AngleSharp.Diffing.Strategies.NodeStrategies
{
    public class OneToOneNodeMatcherTest : DiffingTestBase
    {
        [Fact(DisplayName = "With two equal length node lists, " +
                            "all control and test nodes are matched based on the order they are in the lists")]
        public void Test001()
        {
            var context = new DiffContext(null, null);
            var controlSources = ToSourceCollection("<p></p>text<!--comment-->", ComparisonSourceType.Control);
            var testSources = ToSourceCollection("<!--comment--><p></p>text", ComparisonSourceType.Test);

            var actual = OneToOneNodeMatcher.Match(context, controlSources, testSources).ToList();

            actual.Count.ShouldBe(3);
            actual[0].ShouldSatisfyAllConditions(
                c => c.Control.ShouldBe(controlSources[0]),
                c => c.Test.ShouldBe(testSources[0])
            );
            actual[1].ShouldSatisfyAllConditions(
                c => c.Control.ShouldBe(controlSources[1]),
                c => c.Test.ShouldBe(testSources[1])
            );
            actual[2].ShouldSatisfyAllConditions(
                c => c.Control.ShouldBe(controlSources[2]),
                c => c.Test.ShouldBe(testSources[2])
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
            var context = new DiffContext(null, null);
            var controlSources = ToSourceCollection(controlHtml, ComparisonSourceType.Control);
            var testSources = ToSourceCollection(testHtml, ComparisonSourceType.Test);

            var actual = OneToOneNodeMatcher.Match(context, controlSources, testSources).ToList();

            actual.Count.ShouldBe(matchCount);
            actual.ShouldAllBe((c, idx) => c.Control == controlSources[idx] && c.Test == testSources[idx]);
        }

        [Fact(DisplayName = "When a control node has previous been matched, it is not matched again")]
        public void Test003()
        {
            var context = new DiffContext(null, null);
            var controlSources = ToSourceCollection("<p></p>text<!--comment-->", ComparisonSourceType.Control);            
            var testSources = ToSourceCollection("<!--comment--><p></p>text", ComparisonSourceType.Test);
            controlSources.MarkAsMatched(controlSources[1]);

            var actual = OneToOneNodeMatcher.Match(context, controlSources, testSources).ToList();

            actual.Count.ShouldBe(2);
            actual[0].ShouldSatisfyAllConditions(
                c => c.Control.ShouldBe(controlSources[0]),
                c => c.Test.ShouldBe(testSources[0])
            );
            actual[1].ShouldSatisfyAllConditions(
                c => c.Control.ShouldBe(controlSources[2]),
                c => c.Test.ShouldBe(testSources[1])
            );
        }

        [Fact(DisplayName = "When a test node has previous been matched, it is not matched again")]
        public void Test004()
        {
            var context = new DiffContext(null, null);
            var controlSources = ToSourceCollection("<p></p>text<!--comment-->", ComparisonSourceType.Control);
            var testSources = ToSourceCollection("<!--comment--><p></p>text", ComparisonSourceType.Test);
            testSources.MarkAsMatched(testSources[1]);

            var actual = OneToOneNodeMatcher.Match(context, controlSources, testSources).ToList();

            actual.Count.ShouldBe(2);
            actual[0].ShouldSatisfyAllConditions(
                c => c.Control.ShouldBe(controlSources[0]),
                c => c.Test.ShouldBe(testSources[0])
            );
            actual[1].ShouldSatisfyAllConditions(
                c => c.Control.ShouldBe(controlSources[1]),
                c => c.Test.ShouldBe(testSources[2])
            );
        }
    }
}
