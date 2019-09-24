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
        [InlineData("<p></p><span><span/>", "<p></p><span><span/>")]
        [InlineData("textnode", "textnode")]
        [InlineData("<!--comment-->", "<!--comment-->")]
        public void NoDiffsWhenTwoEqualNodeListsAreCompared(string control, string test)
        {
            var strategy = new MockHtmlCompareStrategy(comparer: _ => CompareResult.Same);
            var sut = new HtmlDifferenceEngine(strategy);

            var res = sut.Compare(ToNodeList(control), ToNodeList(test));

            res.ShouldBeEmpty();
        }

        [Theory(DisplayName = "Returns diff with type 'Missing*' when node matching fails")]
        [InlineData("<p/>", "", DiffType.MissingElement)]
        [InlineData("<p></p><span><span/>", "<p/>", DiffType.MissingElement)]
        [InlineData("<p></p><span><span/>", "<span/>", DiffType.MissingElement)]
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
        [InlineData("<p/>", "<p><p/><span><span/>", DiffType.UnexpectedElement)]
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

            res.ShouldAllBe(x=>x.Type == DiffType.UnexpectedElement && x.Test.Value.Node.NodeName != "P");
        }


        [Theory(DisplayName = "Returns diff with type 'Different*' when comparer returns 'Different'")]
        [InlineData("<p/>", "<span/>", DiffType.DifferentElementTagName)]
        [InlineData("textnode", "nodetext", DiffType.DifferentTextNode)]
        [InlineData("<!--bar-->", "<!--foo-->", DiffType.DifferentComment)]
        public void ReturnsDifferentWhenComparerReturnsDifferent(string control, string test, DiffType expectedDiffType)
        {
            var strategy = new MockHtmlCompareStrategy(comparer: _ => CompareResult.Different);
            var sut = new HtmlDifferenceEngine(strategy);

            var res = sut.Compare(ToNodeList(control), ToNodeList(test));

            res.ShouldHaveSingleItem().Type.ShouldBe(expectedDiffType);
        }

        // child nodes 
        // when a parent node is the same, compare child nodes
        // when a parent node is different, skip comparing child nodes
        // attributes
    }
}
