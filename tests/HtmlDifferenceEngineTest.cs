using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Dom;
using Shouldly;
using Xunit;

namespace Egil.AngleSharp.Diffing
{
    public partial class HtmlDifferenceEngineTest : DiffingTestBase
    {
        [Theory(DisplayName = "When comparer gives result 'Same' no diff is returned")]
        [InlineData("", "")]
        [InlineData("<p/>", "<p/>")]
        [InlineData("<p/><span/>", "<p/><span/>")]
        [InlineData("textnode", "textnode")]
        [InlineData("<!--comment-->", "<!--comment-->")]
        public void NoDiffsWhenTwoEqualNodeListsAreCompared(string control, string test)
        {
            var strategy = new MockHtmlCompareStrategy(nodeComparer: _ => CompareResult.Same);
            var sut = new HtmlDifferenceEngine(strategy);

            var res = sut.Compare(ToNodeList(control), ToNodeList(test));

            res.ShouldBeEmpty();
        }

        [Theory(DisplayName = "Returns diff with type 'Missing*' when node matching fails")]
        [InlineData("<p/>", "", DiffType.MissingElement)]
        [InlineData("<p/><span/>", "<p/>", DiffType.MissingElement)]
        [InlineData("<p/><span/>", "<span/>", DiffType.MissingElement)]
        [InlineData("textnode", "", DiffType.MissingTextNode)]
        [InlineData("<!--comment-->", "", DiffType.MissingComment)]
        public void ReturnsMissingDiffWhenNodeMatcherFails(string control, string test, DiffType expectedDiffType)
        {
            var strategy = new MockHtmlCompareStrategy();
            var sut = new HtmlDifferenceEngine(strategy);

            var res = sut.Compare(ToNodeList(control), ToNodeList(test));

            res.ShouldHaveSingleItem().Type.ShouldBe(expectedDiffType);
        }

        [Theory(DisplayName = "Returns diff with type 'Unexpected*' when there are unmatched test-nodes")]
        [InlineData("", "<p/>", DiffType.UnexpectedElement)]
        [InlineData("<p/>", "<p/><span/>", DiffType.UnexpectedElement)]
        [InlineData("", "textnode", DiffType.UnexpectedTextNode)]
        [InlineData("", "<!--comment-->", DiffType.UnexpectedComment)]
        [InlineData("<p/>", "textnode<p/>", DiffType.UnexpectedTextNode)]
        public void ReturnsUnexpectedDiffWhenTestNodesAreUnmatched(string control, string test, DiffType expectedDiffType)
        {
            var strategy = new MockHtmlCompareStrategy();
            var sut = new HtmlDifferenceEngine(strategy);

            var res = sut.Compare(ToNodeList(control), ToNodeList(test));

            res.ShouldHaveSingleItem().Type.ShouldBe(expectedDiffType);
        }

        [Fact(DisplayName = "Returns diff with type 'Unexpected*' when there are unmatched test-nodes")]
        public void ReturnsUnexpectedDiffWhenTestNodesAreUnmatchedx()
        {
            var strategy = new MockHtmlCompareStrategy();
            var sut = new HtmlDifferenceEngine(strategy);

            var res = sut.Compare(ToNodeList("<p>1</p><p>2</p>"), ToNodeList("<div></div><span></span><p>1</p><p>2</p><br />"));

            res.ShouldAllBe(x => x.Type == DiffType.UnexpectedElement && x.Test.Value.Node.NodeName != "P");
        }

        [Theory(DisplayName = "Returns diff with type 'Different*' when comparer returns 'Different'")]
        [InlineData("<p/>", "<span/>", DiffType.DifferentElementTagName)]
        [InlineData("textnode", "nodetext", DiffType.DifferentTextNode)]
        [InlineData("<!--bar-->", "<!--foo-->", DiffType.DifferentComment)]
        public void ReturnsDifferentWhenComparerReturnsDifferent(string control, string test, DiffType expectedDiffType)
        {
            var strategy = new MockHtmlCompareStrategy(nodeComparer: _ => CompareResult.Different);
            var sut = new HtmlDifferenceEngine(strategy);

            var res = sut.Compare(ToNodeList(control), ToNodeList(test));

            res.ShouldHaveSingleItem().Type.ShouldBe(expectedDiffType);
        }

        [Fact(DisplayName = "Child nodes of a matched control- and test-node are compared")]
        public void ChildNodesOfMatchedNodesAreCompared()
        {
            var sut = new HtmlDifferenceEngine(new MockHtmlCompareStrategy(nodeComparer: _ => CompareResult.Different));

            var res = sut.Compare(
                ToNodeList(@"<p>text<em>foo<!--foo--></em></p>"),
                ToNodeList(@"<div>xest<strong>bar<!--bar--></strong></div>")
                );

            res.Count.ShouldBe(5);
            res[0].ShouldSatisfyAllConditions(
                () => res[0].Type.ShouldBe(DiffType.DifferentElementTagName),
                () => res[0].Control?.Node.NodeName.ShouldBe("P"),
                () => res[0].Test?.Node.NodeName.ShouldBe("DIV")
            );
            res[1].ShouldSatisfyAllConditions(
                () => res[1].Type.ShouldBe(DiffType.DifferentTextNode),
                () => res[1].Control?.Node.NodeValue.ShouldBe("text"),
                () => res[1].Test?.Node.NodeValue.ShouldBe("xest")
            );
            res[2].ShouldSatisfyAllConditions(
                () => res[2].Type.ShouldBe(DiffType.DifferentElementTagName),
                () => res[2].Control?.Node.NodeName.ShouldBe("EM"),
                () => res[2].Test?.Node.NodeName.ShouldBe("STRONG")
            );
            res[3].ShouldSatisfyAllConditions(
                () => res[3].Type.ShouldBe(DiffType.DifferentTextNode),
                () => res[3].Control?.Node.NodeValue.ShouldBe("foo"),
                () => res[3].Test?.Node.NodeValue.ShouldBe("bar")
            );

            res[4].Type.ShouldBe(DiffType.DifferentComment);
        }

        [Theory(DisplayName = "Equal attributes (order independent) on control and test nodes yields no diffs")]
        [InlineData(@"<p id=""x"" />", @"<p id=""x"" />")]
        [InlineData(@"<p id=""x"" class=""sm-6""/>", @"<p id=""x"" class=""sm-6""/>")]
        [InlineData(@"<p class=""sm-6"" id=""x"" />", @"<p id=""x"" class=""sm-6""/>")]
        public void NoDiffsOnEqualAttr(string control, string test)
        {
            var sut = new HtmlDifferenceEngine(new MockHtmlCompareStrategy(
                nodeComparer: _ => CompareResult.Same,
                attrComparer: (a, c) => CompareResult.Same));

            var result = sut.Compare(ToNodeList(control), ToNodeList(test));

            result.ShouldBeEmpty();
        }

        [Theory(DisplayName = "Unequal attributes on control and test nodes yields one diff per unequal attribute pair")]
        [InlineData(@"<p id=""foo"" />", @"<p id=""bar"" />", 1)]
        [InlineData(@"<p id=""foo"" class=""sm-6""/>", @"<p id=""bar"" class=""lg-6""/>", 2)]
        [InlineData(@"<p class=""sm-6"" id=""foo"" />", @"<p id=""bar"" class=""lg-6""/>", 2)]
        public void DiffsOnUnequalAttrs(string control, string test, int expectedCount)
        {
            var sut = new HtmlDifferenceEngine(new MockHtmlCompareStrategy(attrComparer: (a, c) => CompareResult.Different));

            var result = sut.Compare(ToNodeList(control), ToNodeList(test));

            result.Count.ShouldBe(expectedCount);
        }
    }
}
