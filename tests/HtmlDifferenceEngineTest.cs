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
            results[0].ShouldBeOfType<MissingDiff<IElement>>().ShouldSatisfyAllConditions(
                diff => diff.Result.ShouldBe(DiffResult.Missing),
                diff => diff.Target.ShouldBe(DiffTarget.Element)
            );
            results[1].ShouldBeOfType<MissingDiff<IComment>>().ShouldSatisfyAllConditions(
                diff => diff.Result.ShouldBe(DiffResult.Missing),
                diff => diff.Target.ShouldBe(DiffTarget.Comment)
            );
            results[2].ShouldBeOfType<MissingDiff<IText>>().ShouldSatisfyAllConditions(
                diff => diff.Result.ShouldBe(DiffResult.Missing),
                diff => diff.Target.ShouldBe(DiffTarget.Text)
            );
            results[3].ShouldBeOfType<UnexpectedDiff<IElement>>().ShouldSatisfyAllConditions(
                diff => diff.Result.ShouldBe(DiffResult.Unexpected),
                diff => diff.Target.ShouldBe(DiffTarget.Element)
            );
            results[4].ShouldBeOfType<UnexpectedDiff<IComment>>().ShouldSatisfyAllConditions(
                diff => diff.Result.ShouldBe(DiffResult.Unexpected),
                diff => diff.Target.ShouldBe(DiffTarget.Comment)
            );
            results[5].ShouldBeOfType<UnexpectedDiff<IText>>().ShouldSatisfyAllConditions(
                diff => diff.Result.ShouldBe(DiffResult.Unexpected),
                diff => diff.Target.ShouldBe(DiffTarget.Text)
            );
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
                nodeComparer: SameResultNodeComparer);

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
                nodeComparer: DiffResultNodeComparer);

            var results = sut.Compare(nodes, nodes);

            results.Count.ShouldBe(3);
            results[0].ShouldBeOfType<Diff<IElement>>().ShouldSatisfyAllConditions(
                diff => diff.Control.Node.NodeName.ShouldBe("P"),
                diff => diff.Result.ShouldBe(DiffResult.Different),
                diff => diff.Target.ShouldBe(DiffTarget.Element)
            );
            results[1].ShouldBeOfType<Diff<IComment>>().ShouldSatisfyAllConditions(
                diff => diff.Control.Node.NodeName.ShouldBe("#comment"),
                diff => diff.Result.ShouldBe(DiffResult.Different),
                diff => diff.Target.ShouldBe(DiffTarget.Comment)
            );
            results[2].ShouldBeOfType<Diff<IText>>().ShouldSatisfyAllConditions(
                diff => diff.Control.Node.NodeName.ShouldBe("#text"),
                diff => diff.Result.ShouldBe(DiffResult.Different),
                diff => diff.Target.ShouldBe(DiffTarget.Text)
            );
        }

        [Fact(DisplayName = "When matched control/test nodes are the same, no diffs are returned")]
        public void WhenNodesAreSameNoDiffIsReturned()
        {
            var nodes = ToNodeList("<p></p><!--comment-->textnode");
            var sut = CreateHtmlDiffEngine(
                nodeMatcher: OneToOneNodeListMatcher,
                nodeFilter: NoneNodeFilter,
                nodeComparer: SameResultNodeComparer);

            var results = sut.Compare(nodes, nodes);

            results.ShouldBeEmpty();
        }

        [Fact(DisplayName = "Unmatched attributes in control/test node are returned as missing/unexpected diffs")]
        public void UnmatchedAttr()
        {
            var nodes = ToNodeList(@"<p id=""foo""></p>");
            var expectedElementSource = (IElement)nodes[0];
            var sut = CreateHtmlDiffEngine(
                nodeMatcher: OneToOneNodeListMatcher,
                nodeFilter: NoneNodeFilter,
                nodeComparer: SameResultNodeComparer,
                attrMatcher: NoneAttributeMatcher,
                attrFilter: NoneAttrFilter,
                attrComparer: SameResultAttrComparer);

            var results = sut.Compare(nodes, nodes);

            results.Count.ShouldBe(2);
            results[0].ShouldBeOfType<MissingAttrDiff>().ShouldSatisfyAllConditions(
                diff => diff.Result.ShouldBe(DiffResult.Missing),
                diff => diff.Target.ShouldBe(DiffTarget.Attribute),
                diff => diff.Control.Attribute.Name.ShouldBe("id"),
                diff => diff.Control.ElementSource.Node.ShouldBe(expectedElementSource)
            );
            results[1].ShouldBeOfType<UnexpectedAttrDiff>().ShouldSatisfyAllConditions(
                diff => diff.Result.ShouldBe(DiffResult.Unexpected),
                diff => diff.Target.ShouldBe(DiffTarget.Attribute),
                diff => diff.Test.Attribute.Name.ShouldBe("id"),
                diff => diff.Test.ElementSource.Node.ShouldBe(expectedElementSource)
            );
        }

        [Theory(DisplayName = "When partial match of attributes in control/test node, remaining unmatched are returned as missing/unexpected diffs")]
        [InlineData("id")]
        [InlineData("lang")]
        [InlineData("custom")]
        public void PartialUnmatchedAttrs(string matchedAttr)
        {
            var nodes = ToNodeList(@"<p id=""foo"" lang=""bar"" custom=""baz""></p>");
            var sut = CreateHtmlDiffEngine(
                nodeMatcher: OneToOneNodeListMatcher,
                nodeFilter: NoneNodeFilter,
                nodeComparer: SameResultNodeComparer,
                attrMatcher: SpecificAttributeMatcher(matchedAttr),
                attrFilter: NoneAttrFilter,
                attrComparer: SameResultAttrComparer);

            var results = sut.Compare(nodes, nodes);

            results.Count.ShouldBe(4);
            results[0].ShouldBeOfType<MissingAttrDiff>().Control.Attribute.Name.ShouldNotBe(matchedAttr);
            results[1].ShouldBeOfType<MissingAttrDiff>().Control.Attribute.Name.ShouldNotBe(matchedAttr);
            results[2].ShouldBeOfType<UnexpectedAttrDiff>().Test.Attribute.Name.ShouldNotBe(matchedAttr);
            results[3].ShouldBeOfType<UnexpectedAttrDiff>().Test.Attribute.Name.ShouldNotBe(matchedAttr);
        }

        [Theory(DisplayName = "Filtered out attributes does not take part in comparison")]
        [InlineData("id")]
        [InlineData("lang")]
        [InlineData("custom")]
        public void FilteredAttrNotPartOfComparison(string filterOutAttrName)
        {
            var nodes = ToNodeList(@"<p id=""foo"" lang=""bar"" custom=""baz""></p>");

            var sut = CreateHtmlDiffEngine(
                nodeMatcher: OneToOneNodeListMatcher,
                nodeFilter: NoneNodeFilter,
                nodeComparer: SameResultNodeComparer,
                attrMatcher: NoneAttributeMatcher,
                attrFilter: SpecificAttrFilter(filterOutAttrName),
                attrComparer: SameResultAttrComparer);

            var results = sut.Compare(nodes, nodes);

            results.Count.ShouldBe(4);
            results[0].ShouldBeOfType<MissingAttrDiff>().Control.Attribute.Name.ShouldNotBe(filterOutAttrName);
            results[1].ShouldBeOfType<MissingAttrDiff>().Control.Attribute.Name.ShouldNotBe(filterOutAttrName);
            results[2].ShouldBeOfType<UnexpectedAttrDiff>().Test.Attribute.Name.ShouldNotBe(filterOutAttrName);
            results[3].ShouldBeOfType<UnexpectedAttrDiff>().Test.Attribute.Name.ShouldNotBe(filterOutAttrName);
        }

        [Fact(DisplayName = "When matched control/test attributes are different, a diff is returned")]
        public void WhenMatchedAttrsAreDiffAttrDiffIsReturned()
        {
            var nodes = ToNodeList(@"<p id=""foo""></p>");

            var sut = CreateHtmlDiffEngine(
                nodeMatcher: OneToOneNodeListMatcher,
                nodeFilter: NoneNodeFilter,
                nodeComparer: SameResultNodeComparer,
                attrMatcher: AttributeNameMatcher,
                attrFilter: NoneAttrFilter,
                attrComparer: DiffResultAttrComparer);

            var results = sut.Compare(nodes, nodes);

            results.Count.ShouldBe(1);
            results[0].ShouldBeOfType<AttrDiff>().ShouldSatisfyAllConditions(
                diff => diff.Control.Attribute.Name.ShouldBe("id"),
                diff => diff.Test.Attribute.Name.ShouldBe("id"),
                diff => diff.Result.ShouldBe(DiffResult.Different),
                diff => diff.Target.ShouldBe(DiffTarget.Attribute)
            );
        }

        [Fact(DisplayName = "When matched control/test attributes are the same, no diffs are returned")]
        public void WhenMatchedAttrsAreSameNoDiffIsReturned()
        {
            var nodes = ToNodeList(@"<p id=""foo"" lang=""bar"" custom=""baz""></p>");

            var sut = CreateHtmlDiffEngine(
                nodeMatcher: OneToOneNodeListMatcher,
                nodeFilter: NoneNodeFilter,
                nodeComparer: SameResultNodeComparer,
                attrMatcher: AttributeNameMatcher,
                attrFilter: NoneAttrFilter,
                attrComparer: SameResultAttrComparer);

            var results = sut.Compare(nodes, nodes);

            results.ShouldBeEmpty();
        }

        [Fact(DisplayName = "If both the control or test node in a comparison has child nodes, these nodelists are compared")]
        public void WhenBothTestAndControlHaveChildNodesTheseAreCompared()
        {
            var nodes = ToNodeList(@"<main><h1><!--foobar--><p>hello world</p></h1></main>");

            var sut = CreateHtmlDiffEngine(
                nodeMatcher: OneToOneNodeListMatcher,
                nodeFilter: NoneNodeFilter,
                nodeComparer: DiffResultNodeComparer);

            var results = sut.Compare(nodes, nodes);

            results.Count.ShouldBe(5);
            results[0].ShouldBeOfType<Diff<IElement>>().Control.Node.NodeName.ShouldBe("MAIN");
            results[1].ShouldBeOfType<Diff<IElement>>().Control.Node.NodeName.ShouldBe("H1");
            results[2].ShouldBeOfType<Diff<IComment>>().Control.Node.NodeValue.ShouldBe("foobar");
            results[3].ShouldBeOfType<Diff<IElement>>().Control.Node.NodeName.ShouldBe("P");
            results[4].ShouldBeOfType<Diff<IText>>().Control.Node.NodeName.ShouldBe("#text");
        }

        [Theory(DisplayName = "When only one of the control or test node in a comparison has child nodes, a missing/unexpected diff is returned")]
        [InlineData("<h1><p></p></h1>", "<h1></h1>", typeof(MissingDiff<IElement>))]
        [InlineData("<h1></h1>", "<h1><p></p></h1>", typeof(UnexpectedDiff<IElement>))]
        public void OnlyOnePartHasChildNodes(string control, string test, Type expectedDiffType)
        {
            var sut = CreateHtmlDiffEngine(
                nodeMatcher: OneToOneNodeListMatcher,
                nodeFilter: NoneNodeFilter,
                nodeComparer: DiffResultNodeComparer);

            var results = sut.Compare(ToNodeList(control), ToNodeList(test));

            results.Count.ShouldBe(2);
            results[0].ShouldBeOfType<Diff<IElement>>();
            results[1].ShouldBeOfType(expectedDiffType);
        }

        [Fact(DisplayName = "Path in Diffs is set correctly when nested nodes are compared")]
        public void PathIsSetCorrectly()
        {
            var ctrlNodes = ToNodeList(@"<main><h1><!--foo--><p>hello world</p></h1></main>");
            var testNodes = ToNodeList(@"<!--foo--><main><h1><p>hello world</p></h1></main>");

            var sut = CreateHtmlDiffEngine(
                nodeMatcher: OneToOneNodeListMatcher,
                nodeFilter: RemoveCommentNodeFilter,
                nodeComparer: DiffResultNodeComparer);

            var results = sut.Compare(ctrlNodes, testNodes);

            results.Count.ShouldBe(4);
            results[0].ShouldBeOfType<Diff<IElement>>().Control.Path.ShouldBe("main(0)");
            results[0].ShouldBeOfType<Diff<IElement>>().Test.Path.ShouldBe("main(1)");
            results[1].ShouldBeOfType<Diff<IElement>>().Control.Path.ShouldBe("main(0) > h1(0)");
            results[1].ShouldBeOfType<Diff<IElement>>().Test.Path.ShouldBe("main(1) > h1(0)");
            results[2].ShouldBeOfType<Diff<IElement>>().Control.Path.ShouldBe("main(0) > h1(0) > p(1)");
            results[2].ShouldBeOfType<Diff<IElement>>().Test.Path.ShouldBe("main(1) > h1(0) > p(0)");
            results[3].ShouldBeOfType<Diff<IText>>().Control.Path.ShouldBe("main(0) > h1(0) > p(1) > #text(0)");
            results[3].ShouldBeOfType<Diff<IText>>().Test.Path.ShouldBe("main(1) > h1(0) > p(0) > #text(0)");
        }

        [Fact(DisplayName = "Attribute path in comparison sources are based on nodes tree structure")]
        public void AttributeSourcePathisBasedOnParentElements()
        {
            var nodes = ToNodeList(@"<p id=""foo""></p>");

            var sut = CreateHtmlDiffEngine(
                nodeMatcher: OneToOneNodeListMatcher,
                nodeFilter: NoneNodeFilter,
                nodeComparer: SameResultNodeComparer,
                attrMatcher: AttributeNameMatcher,
                attrFilter: NoneAttrFilter,
                attrComparer: DiffResultAttrComparer);

            var results = sut.Compare(nodes, nodes);

            results.Count.ShouldBe(1);
            results[0].ShouldBeOfType<AttrDiff>().Control.Path.ShouldBe("p(0)[id]");
        }

        [Fact(DisplayName = "Comparison sources have their type set correctly")]
        public void ComparisonSourcesHaveCorrectType()
        {
            var nodes = ToNodeList(@"<p id=""foo""></p>");

            var sut = CreateHtmlDiffEngine(
                nodeMatcher: OneToOneNodeListMatcher,
                nodeFilter: NoneNodeFilter,
                nodeComparer: DiffResultNodeComparer,
                attrMatcher: AttributeNameMatcher,
                attrFilter: NoneAttrFilter,
                attrComparer: DiffResultAttrComparer);

            var results = sut.Compare(nodes, nodes);

            results.Count.ShouldBe(2);

            results[0].ShouldBeOfType<Diff<IElement>>().Control.SourceType.ShouldBe(ComparisonSourceType.Control);
            results[0].ShouldBeOfType<Diff<IElement>>().Test.SourceType.ShouldBe(ComparisonSourceType.Test);
            results[1].ShouldBeOfType<AttrDiff>().Control.SourceType.ShouldBe(ComparisonSourceType.Control);
            results[1].ShouldBeOfType<AttrDiff>().Test.SourceType.ShouldBe(ComparisonSourceType.Test);
        }

        [Fact(DisplayName = "When comparer returns SameAndBreak from an element comparison, none of the attributes or child nodes are compared")]
        public void Test1()
        {
            var sut = CreateHtmlDiffEngine(
                nodeMatcher: OneToOneNodeListMatcher,
                nodeFilter: NoneNodeFilter,
                nodeComparer: c => c.Control.Node.NodeName == "P" ? CompareResult.SameAndBreak : throw new Exception("NODE COMPARER SHOULD NOT BE CALLED ON CHILD NODES"),
                attrMatcher: (x,y) => throw new Exception("ATTR MATCHER SHOULD NOT BE CALLED"),
                attrFilter: _ => throw new Exception("ATTR FILTER SHOULD NOT BE CALLED"),
                attrComparer: _ => throw new Exception("ATTR COMPARER SHOULD NOT BE CALLED"));

            var results = sut.Compare(ToNodeList(@"<p id=""foo""><em>foo</em></p>"), ToNodeList(@"<p id=""bar""><span>baz</span></p>"));

            results.ShouldBeEmpty();
        }

        [Fact(DisplayName = "When comparer returns DifferentAndBreak from an element comparison, none of the attributes or child nodes are compared")]
        public void Test2()
        {
            var sut = CreateHtmlDiffEngine(
                nodeMatcher: OneToOneNodeListMatcher,
                nodeFilter: NoneNodeFilter,
                nodeComparer: c => c.Control.Node.NodeName == "P" ? CompareResult.DifferentAndBreak : throw new Exception("NODE COMPARER SHOULD NOT BE CALLED ON CHILD NODES"),
                attrMatcher: (x, y) => throw new Exception("ATTR MATCHER SHOULD NOT BE CALLED"),
                attrFilter: _ => throw new Exception("ATTR FILTER SHOULD NOT BE CALLED"),
                attrComparer: _ => throw new Exception("ATTR COMPARER SHOULD NOT BE CALLED"));

            var results = sut.Compare(ToNodeList(@"<p id=""foo""><em>foo</em></p>"), ToNodeList(@"<p id=""bar""><span>baz</span></p>"));

            results[0].ShouldBeOfType<Diff<IElement>>();
        }

        #region NodeFilters
        private static bool NoneNodeFilter(IComparisonSource<INode> source) => true;
        private static bool RemoveCommentNodeFilter(IComparisonSource<INode> source) => source.Node.NodeType != NodeType.Comment;
        #endregion

        #region NodeMatchers
        private static IEnumerable<IComparison<INode>> NoneNodeMatcher(IReadOnlyList<IComparisonSource<INode>> controlNodes, IReadOnlyList<IComparisonSource<INode>> testNodes)
            => Array.Empty<IComparison<INode>>();

        private static Func<IReadOnlyList<IComparisonSource<INode>>, IReadOnlyList<IComparisonSource<INode>>, IEnumerable<IComparison<INode>>> SpecificIndexNodeMatcher(int index)
            => (controlNodes, testNodes) => new List<IComparison<INode>> { ComparisonFactory.Create(controlNodes[index], testNodes[index]) };

        private static IEnumerable<IComparison<INode>> OneToOneNodeListMatcher(
            IReadOnlyList<IComparisonSource<INode>> controlNodes,
            IReadOnlyList<IComparisonSource<INode>> testNodes)
        {
            var matchLength = Math.Min(controlNodes.Count, testNodes.Count);
            for (int i = 0; i < matchLength; i++)
            {
                yield return ComparisonFactory.Create(controlNodes[i], testNodes[i]);
            }
        }

        #endregion

        #region NodeComparers
        private static CompareResult SameResultNodeComparer(IComparison<INode> comparison) => CompareResult.Same;
        private static CompareResult DiffResultNodeComparer(IComparison<INode> comparison) => CompareResult.Different;
        #endregion

        #region AttributeMatchers
        private static IReadOnlyList<IAttributeComparison> NoneAttributeMatcher(
            IReadOnlyList<IAttributeComparisonSource> controlAttributes,
            IReadOnlyList<IAttributeComparisonSource> testAttributes) => Array.Empty<IAttributeComparison>();

        private static Func<IReadOnlyList<IAttributeComparisonSource>, IReadOnlyList<IAttributeComparisonSource>, IEnumerable<IAttributeComparison>> SpecificAttributeMatcher(string matchAttrName)
        {
            return (ctrlAttrs, testAttrs) => new List<IAttributeComparison>
            {
                new AttributeComparison(
                    ctrlAttrs.Single(x => x.Attribute.Name == matchAttrName),
                    testAttrs.Single(x => x.Attribute.Name == matchAttrName)
                )
            };
        }

        private static IEnumerable<IAttributeComparison> AttributeNameMatcher(
            IReadOnlyList<IAttributeComparisonSource> controlAttributes,
            IReadOnlyList<IAttributeComparisonSource> testAttributes)
        {
            foreach (var ctrlAttrSrc in controlAttributes)
            {
                var testAttrSrc = testAttributes.SingleOrDefault(x => x.Attribute.Name == ctrlAttrSrc.Attribute.Name);
                if (testAttrSrc is { }) yield return new AttributeComparison(ctrlAttrSrc, testAttrSrc);
            }
        }

        #endregion

        #region AttributeFilters 

        private static bool NoneAttrFilter(IAttributeComparisonSource source) => true;

        private static Func<IAttributeComparisonSource, bool> SpecificAttrFilter(string attrName) =>
            source => source.Attribute.Name != attrName;

        #endregion

        #region AttributeComparers
        public static CompareResult SameResultAttrComparer(IAttributeComparison comparison) => CompareResult.Same;
        public static CompareResult DiffResultAttrComparer(IAttributeComparison comparison) => CompareResult.Different;
        #endregion
    }
}
