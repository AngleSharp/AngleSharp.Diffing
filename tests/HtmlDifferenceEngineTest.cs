using System;
using System.Collections.Generic;
using System.Linq;
using AngleSharp.Dom;
using Egil.AngleSharp.Diffing.Comparisons;
using Shouldly;
using Xunit;

namespace Egil.AngleSharp.Diffing
{

    public class HtmlDifferenceEngineTest : DiffingTestBase
    {
        [Fact(DisplayName = "Unmatched nodes in control/test are returned as missing/unexpected diffs")]
        public void UnmatchedNodesBecomesMissingUnexpectedDiffs()
        {
            var sut = CreateHtmlDiffEngine(nodeMatcher: NoneNodeMatcher, nodeFilter: NoneNodeFilter);

            var results = sut.Compare(ToNodeList("<p></p><!--comment-->text"), ToNodeList("<p></p><!--comment-->text"));

            results.Count.ShouldBe(6);
            results[0].ShouldBeOfType<MissingDiff<IElement>>();
            results[1].ShouldBeOfType<MissingDiff<IComment>>();
            results[2].ShouldBeOfType<MissingDiff<IText>>();
            results[3].ShouldBeOfType<UnexpectedDiff<IElement>>();
            results[4].ShouldBeOfType<UnexpectedDiff<IComment>>();
            results[5].ShouldBeOfType<UnexpectedDiff<IText>>();
        }

        [Theory(DisplayName = "When partial match of nodes in control/test, remaining unmatched are returned as missing/unexpected diffs")]
        [InlineData(0)]
        [InlineData(1)]
        public void AnyUnmatchedNodesBecomesMissingUnexpectedDiffs(int matchIndex)
        {
            var nodes = ToNodeList("<p></p><span></span>");
            var sut = CreateHtmlDiffEngine(
                nodeMatcher: SpecificIndexNodeMatcher(matchIndex),
                nodeFilter: NoneNodeFilter,
                nodeComparer: AlwaysSameNoteComparer);

            var results = sut.Compare(nodes, nodes);

            results.Count.ShouldBe(2);
            results[0].ShouldBeOfType<MissingDiff<IElement>>().Control.Node.ShouldNotBe(nodes[matchIndex]);
            results[1].ShouldBeOfType<UnexpectedDiff<IElement>>().Test.Node.ShouldNotBe(nodes[matchIndex]);
        }

        [Theory(DisplayName = "Filtered out nodes does not take part in comparison")]
        [InlineData("<!-- filtered out comment --><p></p><span></span>")]
        [InlineData("<p></p><!-- filtered out comment --><span></span>")]
        [InlineData("<p></p><span></span><!-- filtered out comment -->")]
        public void FilteredOutNodesNotPartOfComparison(string html)
        {
            var nodes = ToNodeList(html);
            var sut = CreateHtmlDiffEngine(nodeMatcher: NoneNodeMatcher, nodeFilter: RemoveCommentNodeFilter);

            var results = sut.Compare(nodes, nodes);

            results.ShouldNotContain(diff => diff is MissingDiff<IComment> || diff is UnexpectedDiff<IComment>);
        }

        [Fact(DisplayName = "Index in comparison sources are based of input node lists (before filtering)")]
        public void IndexesAreBasedOnInputNodeLists()
        {
            var nodes = ToNodeList("<p></p><!--removed comment--><span></span>");
            var sut = CreateHtmlDiffEngine(nodeMatcher: NoneNodeMatcher, nodeFilter: RemoveCommentNodeFilter);

            var results = sut.Compare(nodes, nodes);

            results.Count.ShouldBe(4);
            results[0].ShouldBeOfType<MissingDiff<IElement>>().Control.Index.ShouldBe(0);
            results[1].ShouldBeOfType<MissingDiff<IElement>>().Control.Index.ShouldBe(2);
            results[2].ShouldBeOfType<UnexpectedDiff<IElement>>().Test.Index.ShouldBe(0);
            results[3].ShouldBeOfType<UnexpectedDiff<IElement>>().Test.Index.ShouldBe(2);
        }

        [Fact(DisplayName = "When matched control/test nodes are different, a diff is returned")]
        public void WhenNodesAreDifferentADiffIsReturned()
        {
            var nodes = ToNodeList("<p></p><!--comment-->textnode");
            var sut = CreateHtmlDiffEngine(
                nodeMatcher: OneToOneNodeListMatcher,
                nodeFilter: NoneNodeFilter,
                nodeComparer: AlwaysDiffNoteComparer);

            var results = sut.Compare(nodes, nodes);

            results[0].ShouldBeOfType<Diff<IElement>>().Control.Node.NodeName.ShouldBe("P");
            results[1].ShouldBeOfType<Diff<IComment>>().Control.Node.NodeValue.ShouldBe("comment");
            results[2].ShouldBeOfType<Diff<IText>>().Control.Node.NodeValue.ShouldBe("textnode");
        }

        // When a control/test element in a comparison only contains text, the text is compared as with the element??

        // Unmatched attributes in control/test node are returned as missing/unexpected diffs
        // When partial match of attributes in control/test node, remaining unmatched are returned as missing/unexpected diffs
        // Filtered out attributes does not take part in comparison
        // When matched control/test attributes are different, a diff is returned

        // If both the control or test node in a comparison has child nodes, these nodelists are compared
        // If one of the control or test node in a comparison has child nodes, a diff is returned

        // Node path in comparison sources are based on nodes tree structure
        // Attribute path in comparison sources are based on nodes tree structure

        private static CompareResult AlwaysSameNoteComparer(IComparison<INode> comparison) => CompareResult.Same;

        private static CompareResult AlwaysDiffNoteComparer(IComparison<INode> comparison) => CompareResult.Different;

        private static bool NoneNodeFilter(IComparisonSource<INode> source) => true;

        private static bool RemoveCommentNodeFilter(IComparisonSource<INode> source) => source.Node.NodeType != NodeType.Comment;

        private static IReadOnlyList<IComparison<INode>> NoneNodeMatcher(IReadOnlyList<IComparisonSource<INode>> controlNodes, IReadOnlyList<IComparisonSource<INode>> testNodes)
            => Array.Empty<IComparison<INode>>();

        private static Func<IReadOnlyList<IComparisonSource<INode>>, IReadOnlyList<IComparisonSource<INode>>, IReadOnlyList<IComparison<INode>>> SpecificIndexNodeMatcher(int index)
            => (controlNodes, testNodes) => new List<IComparison<INode>> { ComparisonFactory.Create(controlNodes[index], testNodes[index]) };

        private static IReadOnlyList<IComparison<INode>> OneToOneNodeListMatcher(IReadOnlyList<IComparisonSource<INode>> controlNodes, IReadOnlyList<IComparisonSource<INode>> testNodes)
        {
            var result = new List<IComparison<INode>>();
            for (int i = 0; i < controlNodes.Count; i++)
            {
                result.Add(ComparisonFactory.Create(controlNodes[i], testNodes[i]));
            }
            return result;
        }

        //[Theory(DisplayName = "When comparer gives result 'Same' no diff is returned")]
        //[InlineData("", "")]
        //[InlineData("<p/>", "<p/>")]
        //[InlineData("<p/><span/>", "<p/><span/>")]
        //[InlineData("textnode", "textnode")]
        //[InlineData("<!--comment-->", "<!--comment-->")]
        //public void NoDiffsWhenTwoEqualNodeListsAreCompared(string control, string test)
        //{
        //    var strategy = new MockHtmlCompareStrategy(nodeComparer: _ => CompareResult.Same);
        //    var sut = new HtmlDiffEngine(strategy);

        //    var res = sut.Compare(ToNodeList(control), ToNodeList(test));

        //    res.ShouldBeEmpty();
        //}

        //[Theory(DisplayName = "Returns diff with type 'Missing*' when node matching fails")]
        //[InlineData("<p/>", "", DiffType.MissingElement)]
        //[InlineData("<p/><span/>", "<p/>", DiffType.MissingElement)]
        //[InlineData("<p/><span/>", "<span/>", DiffType.MissingElement)]
        //[InlineData("textnode", "", DiffType.MissingTextNode)]
        //[InlineData("<!--comment-->", "", DiffType.MissingComment)]
        //public void ReturnsMissingDiffWhenNodeMatcherFails(string control, string test, DiffType expectedDiffType)
        //{
        //    var strategy = new MockHtmlCompareStrategy();
        //    var sut = new HtmlDiffEngine(strategy);

        //    var res = sut.Compare(ToNodeList(control), ToNodeList(test));

        //    res.ShouldHaveSingleItem().Type.ShouldBe(expectedDiffType);
        //}

        //[Theory(DisplayName = "Returns diff with type 'Unexpected*' when there are unmatched test-nodes")]
        //[InlineData("", "<p/>", DiffType.UnexpectedElement)]
        //[InlineData("<p/>", "<p/><span/>", DiffType.UnexpectedElement)]
        //[InlineData("", "textnode", DiffType.UnexpectedTextNode)]
        //[InlineData("", "<!--comment-->", DiffType.UnexpectedComment)]
        //[InlineData("<p/>", "textnode<p/>", DiffType.UnexpectedTextNode)]
        //public void ReturnsUnexpectedDiffWhenTestNodesAreUnmatched(string control, string test, DiffType expectedDiffType)
        //{
        //    var strategy = new MockHtmlCompareStrategy();
        //    var sut = new HtmlDiffEngine(strategy);

        //    var res = sut.Compare(ToNodeList(control), ToNodeList(test));

        //    res.ShouldHaveSingleItem().Type.ShouldBe(expectedDiffType);
        //}

        //[Fact(DisplayName = "Returns diff with type 'Unexpected*' when there are unmatched test-nodes")]
        //public void ReturnsUnexpectedDiffWhenTestNodesAreUnmatchedx()
        //{
        //    var strategy = new MockHtmlCompareStrategy();
        //    var sut = new HtmlDiffEngine(strategy);

        //    var res = sut.Compare(ToNodeList("<p>1</p><p>2</p>"), ToNodeList("<div></div><span></span><p>1</p><p>2</p><br />"));

        //    res.ShouldAllBe(x => x.Type == DiffType.UnexpectedElement && x.Test!.Value.Node.NodeName != "P");
        //}

        //[Theory(DisplayName = "Returns diff with type 'Different*' when comparer returns 'Different'")]
        //[InlineData("<p/>", "<span/>", DiffType.DifferentElementTagName)]
        //[InlineData("textnode", "nodetext", DiffType.DifferentTextNode)]
        //[InlineData("<!--bar-->", "<!--foo-->", DiffType.DifferentComment)]
        //public void ReturnsDifferentWhenComparerReturnsDifferent(string control, string test, DiffType expectedDiffType)
        //{
        //    var strategy = new MockHtmlCompareStrategy(nodeComparer: _ => CompareResult.Different);
        //    var sut = new HtmlDiffEngine(strategy);

        //    var res = sut.Compare(ToNodeList(control), ToNodeList(test));

        //    res.ShouldHaveSingleItem().Type.ShouldBe(expectedDiffType);
        //}

        //[Fact(DisplayName = "Child nodes of a matched control- and test-node are compared")]
        //public void ChildNodesOfMatchedNodesAreCompared()
        //{
        //    var sut = new HtmlDiffEngine(new MockHtmlCompareStrategy(nodeComparer: _ => CompareResult.Different));

        //    var res = sut.Compare(
        //        ToNodeList(@"<p>text<em>foo<!--foo--></em></p>"),
        //        ToNodeList(@"<div>xest<strong>bar<!--bar--></strong></div>")
        //        );

        //    res.Count.ShouldBe(5);
        //    res[0].ShouldSatisfyAllConditions(
        //        () => res[0].Type.ShouldBe(DiffType.DifferentElementTagName),
        //        () => res[0].Control?.Node.NodeName.ShouldBe("P"),
        //        () => res[0].Test?.Node.NodeName.ShouldBe("DIV")
        //    );
        //    res[1].ShouldSatisfyAllConditions(
        //        () => res[1].Type.ShouldBe(DiffType.DifferentTextNode),
        //        () => res[1].Control?.Node.NodeValue.ShouldBe("text"),
        //        () => res[1].Test?.Node.NodeValue.ShouldBe("xest")
        //    );
        //    res[2].ShouldSatisfyAllConditions(
        //        () => res[2].Type.ShouldBe(DiffType.DifferentElementTagName),
        //        () => res[2].Control?.Node.NodeName.ShouldBe("EM"),
        //        () => res[2].Test?.Node.NodeName.ShouldBe("STRONG")
        //    );
        //    res[3].ShouldSatisfyAllConditions(
        //        () => res[3].Type.ShouldBe(DiffType.DifferentTextNode),
        //        () => res[3].Control?.Node.NodeValue.ShouldBe("foo"),
        //        () => res[3].Test?.Node.NodeValue.ShouldBe("bar")
        //    );

        //    res[4].Type.ShouldBe(DiffType.DifferentComment);
        //}

        //[Theory(DisplayName = "Equal attributes (order independent) on control and test nodes yields no diffs")]
        //[InlineData(@"<p id=""x"" />", @"<p id=""x"" />")]
        //[InlineData(@"<p id=""x"" class=""sm-6""/>", @"<p id=""x"" class=""sm-6""/>")]
        //[InlineData(@"<p class=""sm-6"" id=""x"" />", @"<p id=""x"" class=""sm-6""/>")]
        //public void NoDiffsOnEqualAttr(string control, string test)
        //{
        //    var sut = new HtmlDiffEngine(new MockHtmlCompareStrategy(
        //        nodeComparer: _ => CompareResult.Same,
        //        attrComparer: (a, c) => CompareResult.Same));

        //    var result = sut.Compare(ToNodeList(control), ToNodeList(test));

        //    result.ShouldBeEmpty();
        //}

        //[Theory(DisplayName = "Unequal attributes on control and test nodes yields one diff per unequal attribute pair")]
        //[InlineData(@"<p id=""foo"" />", @"<p id=""bar"" />", 1)]
        //[InlineData(@"<p id=""foo"" class=""sm-6""/>", @"<p id=""bar"" class=""lg-6""/>", 2)]
        //[InlineData(@"<p class=""sm-6"" id=""foo"" />", @"<p id=""bar"" class=""lg-6""/>", 2)]
        //public void DiffsOnUnequalAttrs(string control, string test, int expectedCount)
        //{
        //    var sut = new HtmlDiffEngine(new MockHtmlCompareStrategy(attrComparer: (a, c) => CompareResult.Different));

        //    var result = sut.Compare(ToNodeList(control), ToNodeList(test));

        //    result.Count.ShouldBe(expectedCount);
        //}

        //[Theory(DisplayName = "A diff contains a path to each source-node")]
        //[InlineData()]
        //public void DiffContainsPath(string control, string test, string expectedControlPath)
        //{
        //    var sut = new HtmlDifferenceEngine(new MockHtmlCompareStrategy());

        //    var result = sut.Compare(ToNodeList(control), ToNodeList(test));

        //    result
        //}
    }
}
