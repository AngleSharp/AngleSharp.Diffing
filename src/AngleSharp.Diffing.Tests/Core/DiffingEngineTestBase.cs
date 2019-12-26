using System;
using System.Collections.Generic;

namespace AngleSharp.Diffing.Core
{
    public abstract class DiffingEngineTestBase : DiffingTestBase
    {
        public DiffingEngineTestBase(DiffingTestFixture fixture) : base(fixture)
        {
        }

        protected static HtmlDiffer CreateHtmlDiffer(
                Func<IDiffContext, SourceCollection, SourceCollection, IEnumerable<Comparison>>? nodeMatcher = null,
                Func<IDiffContext, SourceMap, SourceMap, IEnumerable<AttributeComparison>>? attrMatcher = null,
                Func<ComparisonSource, FilterDecision>? nodeFilter = null,
                Func<AttributeComparisonSource, FilterDecision>? attrFilter = null,
                Func<Comparison, CompareResult>? nodeComparer = null,
                Func<AttributeComparison, CompareResult>? attrComparer = null
            )
        {
            return new HtmlDiffer(
                new MockDiffingStrategy(
                    nodeFilter, attrFilter,
                    nodeMatcher, attrMatcher,
                    nodeComparer, attrComparer
                )
            );
        }

        class MockDiffingStrategy : IDiffingStrategy
        {
            private readonly Func<ComparisonSource, FilterDecision>? _nodeFilter;
            private readonly Func<AttributeComparisonSource, FilterDecision>? _attrFilter;
            private readonly Func<IDiffContext, SourceCollection, SourceCollection, IEnumerable<Comparison>>? _nodeMatcher;
            private readonly Func<IDiffContext, SourceMap, SourceMap, IEnumerable<AttributeComparison>>? _attrMatcher;
            private readonly Func<Comparison, CompareResult>? _nodeCompare;
            private readonly Func<AttributeComparison, CompareResult>? _attrCompare;

            public MockDiffingStrategy(
                Func<ComparisonSource, FilterDecision>? nodeFilter = null,
                Func<AttributeComparisonSource, FilterDecision>? attrFilter = null,
                Func<IDiffContext, SourceCollection, SourceCollection, IEnumerable<Comparison>>? nodeMatcher = null,
                Func<IDiffContext, SourceMap, SourceMap, IEnumerable<AttributeComparison>>? attrMatcher = null,
                Func<Comparison, CompareResult>? nodeCompare = null,
                Func<AttributeComparison, CompareResult>? attrCompare = null)
            {
                _nodeFilter = nodeFilter;
                _attrFilter = attrFilter;
                _nodeMatcher = nodeMatcher;
                _attrMatcher = attrMatcher;
                _nodeCompare = nodeCompare;
                _attrCompare = attrCompare;
            }

            public FilterDecision Filter(in AttributeComparisonSource attributeComparisonSource)
                => _attrFilter!(attributeComparisonSource);

            public FilterDecision Filter(in ComparisonSource comparisonSource)
                => _nodeFilter!(comparisonSource);

            public IEnumerable<Comparison> Match(
                IDiffContext context,
                SourceCollection controlNodes,
                SourceCollection testNodes) => _nodeMatcher!(context, controlNodes, testNodes);

            public IEnumerable<AttributeComparison> Match(
                IDiffContext context,
                SourceMap controlAttributes,
                SourceMap testAttributes) => _attrMatcher!(context, controlAttributes, testAttributes);

            public CompareResult Compare(in Comparison comparison)
                => _nodeCompare!(comparison);

            public CompareResult Compare(in AttributeComparison comparison)
                => _attrCompare!(comparison);
        }
    }
}