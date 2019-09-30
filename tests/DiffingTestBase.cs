using System;
using System.Collections.Generic;
using System.Linq;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using Egil.AngleSharp.Diffing.Comparisons;
using Shouldly;
using Xunit;

namespace Egil.AngleSharp.Diffing
{
    public abstract class DiffingTestBase
    {
        private readonly IBrowsingContext _context = BrowsingContext.New();
        private readonly IHtmlParser _htmlParser;
        private readonly IDocument _document;

        protected INodeList EmptyNodeList => ToNodeList("");

        protected DiffingTestBase()
        {
            _htmlParser = _context.GetService<IHtmlParser>();
            _document = _context.OpenNewAsync().Result;
        }

        protected INodeList ToNodeList(string? htmlsnippet)
        {
            var fragment = _htmlParser.ParseFragment(htmlsnippet, _document.Body);
            return fragment;
        }

        protected IEnumerable<IComparisonSource<INode>> ToComparisonSourceList(string html)
        {
            return ToNodeList(html).ToComparisonSourceList(ComparisonSourceType.Control);
        }

        protected INode ToNode(string htmlsnippet)
        {
            var fragment = _htmlParser.ParseFragment(htmlsnippet, _document.Body);
            return fragment[0];
        }

        protected IComparisonSource<TNode> ToComparisonSource<TNode>(string html)
            where TNode : INode
        {
            return (IComparisonSource<TNode>)ToComparisonSource(html);
        }

        protected IComparisonSource<INode> ToComparisonSource(string html)
        {
            return ToNode(html).ToComparisonSource(0, ComparisonSourceType.Control);
        }

        protected static HtmlDifferenceEngine CreateHtmlDiffEngine(
                Func<IReadOnlyList<IComparisonSource<INode>>, IReadOnlyList<IComparisonSource<INode>>, IEnumerable<IComparison<INode>>>? nodeMatcher = null,
                Func<IReadOnlyList<IAttributeComparisonSource>, IReadOnlyList<IAttributeComparisonSource>, IEnumerable<IAttributeComparison>>? attrMatcher = null,
                Func<IComparisonSource<INode>, bool>? nodeFilter = null,
                Func<IAttributeComparisonSource, bool>? attrFilter = null,
                Func<IComparison<INode>, CompareResult>? nodeComparer = null,
                Func<IAttributeComparison, CompareResult>? attrComparer = null
            )
        {
            return new HtmlDifferenceEngine(
                new MockFilterStrategy(nodeFilter, attrFilter),
                new MockMatcherStrategy(nodeMatcher, attrMatcher),
                new MockCompareStrategy(nodeComparer, attrComparer)
            );
        }

        class MockMatcherStrategy : IMatcherStrategy
        {
            private readonly Func<IReadOnlyList<IComparisonSource<INode>>, IReadOnlyList<IComparisonSource<INode>>, IEnumerable<IComparison<INode>>>? _nodeMatcher;
            private readonly Func<IReadOnlyList<IAttributeComparisonSource>, IReadOnlyList<IAttributeComparisonSource>, IEnumerable<IAttributeComparison>>? _attrMatcher;

            public MockMatcherStrategy(
                Func<IReadOnlyList<IComparisonSource<INode>>, IReadOnlyList<IComparisonSource<INode>>, IEnumerable<IComparison<INode>>>? nodeMatcher = null,
                Func<IReadOnlyList<IAttributeComparisonSource>, IReadOnlyList<IAttributeComparisonSource>, IEnumerable<IAttributeComparison>>? attrMatcher = null)
            {
                _nodeMatcher = nodeMatcher;
                _attrMatcher = attrMatcher;
            }

            public IEnumerable<IComparison<INode>> MatchNodes(
                IReadOnlyList<IComparisonSource<INode>> controlNodes,
                IReadOnlyList<IComparisonSource<INode>> testNodes) => _nodeMatcher!(controlNodes, testNodes);

            public IEnumerable<IAttributeComparison> MatchAttributes(
                IReadOnlyList<IAttributeComparisonSource> controlAttributes,
                IReadOnlyList<IAttributeComparisonSource> testAttributes) => _attrMatcher!(controlAttributes, testAttributes);
        }

        class MockFilterStrategy : IFilterStrategy
        {
            private readonly Func<IComparisonSource<INode>, bool>? _nodeFilter;
            private readonly Func<IAttributeComparisonSource, bool>? _attrFilter;

            public MockFilterStrategy(Func<IComparisonSource<INode>, bool>? nodeFilter = null, Func<IAttributeComparisonSource, bool>? attrFilter = null)
            {
                _nodeFilter = nodeFilter;
                _attrFilter = attrFilter;
            }

            public bool AttributeFilter(in IAttributeComparisonSource attributeComparisonSource)
                => _attrFilter!(attributeComparisonSource);

            public bool NodeFilter(in IComparisonSource<INode> comparisonSource)
                => _nodeFilter!(comparisonSource);
        }

        class MockCompareStrategy : ICompareStrategy
        {
            private readonly Func<IComparison<INode>, CompareResult>? _nodeCompare;
            private readonly Func<IAttributeComparison, CompareResult>? _attrCompare;

            public MockCompareStrategy(Func<IComparison<INode>, CompareResult>? nodeCompare = null, Func<IAttributeComparison, CompareResult>? attrCompare = null)
            {
                _nodeCompare = nodeCompare;
                _attrCompare = attrCompare;
            }

            public CompareResult Compare<TNode>(in IComparison<TNode> comparison) where TNode : INode
                => _nodeCompare!((IComparison<INode>)comparison);

            public CompareResult Compare(in IAttributeComparison comparison)
                => _attrCompare!(comparison);
        }
    }
}