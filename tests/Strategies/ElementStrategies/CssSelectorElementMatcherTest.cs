using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Dom;
using Egil.AngleSharp.Diffing.Core;
using Egil.AngleSharp.Diffing.Strategies.NodeStrategies;
using Shouldly;
using Xunit;

namespace Egil.AngleSharp.Diffing.Strategies.ElementStrategies
{
    public class CssSelectorElementMatcherTest : DiffingTestBase
    {
        [Theory(DisplayName = "When a node is not an element and does not have the diff:match attribute, " +
                              "no match is attempted nor returned")]
        [InlineData("textnode")]
        [InlineData("<!--comment-->")]
        [InlineData("<p></p>")]
        public void Test001(string html)
        {
            var context = new DiffContext(null, null);
            var controls = ToSourceCollection(html, ComparisonSourceType.Control);
            var tests = ToSourceCollection(html, ComparisonSourceType.Test);

            var actual = CssSelectorElementMatcher.Match(context, controls, tests).ToList();

            actual.ShouldBeEmpty();
        }

        [Fact(DisplayName = "When a control element contains the diff:match attribute, " +
                            "its value is expected to be a CSS selector string, that can be used to search " +
                            "the test node tree for a matching node")]
        public void Test002()
        {
            var controls = ToSourceCollection(@"<p diff:match=""main > p:first-child""></p>", ComparisonSourceType.Control);
            var tests = ToSourceCollection("<p></p><main><p></p></main>", ComparisonSourceType.Test);
            var context = new DiffContext(controls[0].Node.GetRoot() as IElement, tests[0].Node.GetRoot() as IElement);

            var actual = CssSelectorElementMatcher.Match(context, controls, tests).ToList();

            actual.Count.ShouldBe(1);
            actual[0].ShouldSatisfyAllConditions(
                c => c.Control.Node.ShouldBe(controls[0].Node),
                c => c.Test.Node.ShouldBe(tests[1].Node.FirstChild)
            );
        }

        [Fact(DisplayName = "When a diff:match css selector finds more than one element and exception is thrown")]
        public void Test003()
        {
            var controls = ToSourceCollection(@"<p diff:match=""p""></p>", ComparisonSourceType.Control);
            var tests = ToSourceCollection("<p></p><p></p>", ComparisonSourceType.Test);
            var context = new DiffContext(controls[0].Node.GetRoot() as IElement, tests[0].Node.GetRoot() as IElement);

            Should.Throw<DiffMatchSelectorReturnedTooManyResultsException>(() => CssSelectorElementMatcher.Match(context, controls, tests).ToList());
        }

        [Fact(DisplayName = "When a diff:match css selector finds zero element no matches are created")]
        public void Test004()
        {
            var controls = ToSourceCollection(@"<p diff:match=""div""></p>", ComparisonSourceType.Control);
            var tests = ToSourceCollection("<p></p>", ComparisonSourceType.Test);
            var context = new DiffContext(controls[0].Node.GetRoot() as IElement, tests[0].Node.GetRoot() as IElement);

            var actual = CssSelectorElementMatcher.Match(context, controls, tests).ToList();

            actual.ShouldBeEmpty();
        }

        [Fact(DisplayName = "The matcher does not try to match control elements that are marked as matched already")]
        public void Test005()
        {
            var controls = ToSourceCollection(@"<p diff:match=""p""></p>", ComparisonSourceType.Control);
            var tests = ToSourceCollection("<p></p>", ComparisonSourceType.Test);
            var context = new DiffContext(controls[0].Node.GetRoot() as IElement, tests[0].Node.GetRoot() as IElement);
            controls.MarkAsMatched(controls[0]);

            var actual = CssSelectorElementMatcher.Match(context, controls, tests).ToList();

            actual.ShouldBeEmpty();
        }
    }
}
