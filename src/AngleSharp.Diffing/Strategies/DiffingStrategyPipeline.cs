using System.Collections.Generic;
using AngleSharp.Diffing.Core;

namespace AngleSharp.Diffing.Strategies
{

    public delegate FilterDecision FilterStrategy<TSource>(in TSource source, FilterDecision currentDecision);
    public delegate IEnumerable<TComparison> MatchStrategy<in TSources, out TComparison>(IDiffContext context, TSources controlSources, TSources testSources);
    public delegate CompareResult CompareStrategy<TComparison>(in TComparison comparison, CompareResult currentDecision);

    public class DiffingStrategyPipeline : IDiffingStrategy, IDiffingStrategyCollection
    {
        private readonly List<FilterStrategy<ComparisonSource>> _nodeFilters = new List<FilterStrategy<ComparisonSource>>();
        private readonly List<FilterStrategy<AttributeComparisonSource>> _attrsFilters = new List<FilterStrategy<AttributeComparisonSource>>();
        private readonly List<MatchStrategy<SourceCollection, Comparison>> _nodeMatchers = new List<MatchStrategy<SourceCollection, Comparison>>();
        private readonly List<MatchStrategy<SourceMap, AttributeComparison>> _attrsMatchers = new List<MatchStrategy<SourceMap, AttributeComparison>>();
        private readonly List<CompareStrategy<Comparison>> _nodeComparers = new List<CompareStrategy<Comparison>>();
        private readonly List<CompareStrategy<AttributeComparison>> _attrComparers = new List<CompareStrategy<AttributeComparison>>();

        public bool HasMatchers => _nodeMatchers.Count > 0 && _attrsMatchers.Count > 0;
        public bool HasComparers => _nodeComparers.Count > 0 && _attrComparers.Count > 0;

        public FilterDecision Filter(in ComparisonSource comparisonSource) => Filter(comparisonSource, _nodeFilters);
        public FilterDecision Filter(in AttributeComparisonSource attributeComparisonSource) => Filter(attributeComparisonSource, _attrsFilters);

        public IEnumerable<Comparison> Match(IDiffContext context, SourceCollection controlSources, SourceCollection testSources)
        {
            foreach (var matcher in _nodeMatchers)
            {
                foreach (var comparison in matcher(context, controlSources, testSources))
                {
                    controlSources.MarkAsMatched(comparison.Control);
                    testSources.MarkAsMatched(comparison.Test);
                    yield return comparison;
                }
            }
        }
        public IEnumerable<AttributeComparison> Match(IDiffContext context, SourceMap controlAttrSources, SourceMap testAttrSources)
        {
            foreach (var matcher in _attrsMatchers)
            {
                foreach (var comparison in matcher(context, controlAttrSources, testAttrSources))
                {
                    controlAttrSources.MarkAsMatched(comparison.Control);
                    testAttrSources.MarkAsMatched(comparison.Test);
                    yield return comparison;
                }
            }
        }

        public CompareResult Compare(in Comparison comparison) => Compare(comparison, _nodeComparers, CompareResult.Different);
        public CompareResult Compare(in AttributeComparison comparison) => Compare(comparison, _attrComparers, CompareResult.Different);

        /// <summary>
        /// Adds a node filter to the pipeline.
        /// Specialized filters always execute after any generalized filters in the pipeline.
        /// That enables them to correct for the generic filters decision.
        /// </summary>
        /// <param name="filterStrategy"></param>
        /// <param name="isSpecializedFilter">true if <paramref name="filterStrategy"/> is a specialized filter, false if it is a generalized filter</param>
        public IDiffingStrategyCollection AddFilter(FilterStrategy<ComparisonSource> filterStrategy, bool isSpecializedFilter)
        {
            if (isSpecializedFilter)
                _nodeFilters.Add(filterStrategy);
            else
                _nodeFilters.Insert(0, filterStrategy);
            return this;
        }

        /// <summary>
        /// Adds an attribute filter to the pipeline.
        /// Specialized filters always execute after any generalized filters in the pipeline.
        /// That enables them to correct for the generic filters decision.
        /// </summary>
        /// <param name="filterStrategy"></param>
        /// <param name="isSpecializedFilter">true if <paramref name="filterStrategy"/> is a specialized filter, false if it is a generalized filter</param>
        public IDiffingStrategyCollection AddFilter(FilterStrategy<AttributeComparisonSource> filterStrategy, bool isSpecializedFilter)
        {
            if (isSpecializedFilter)
                _attrsFilters.Add(filterStrategy);
            else
                _attrsFilters.Insert(0, filterStrategy);
            return this;
        }

        /// <summary>
        /// Adds a node matcher to the pipeline.
        /// Specialized matchers always execute before any generalized matchers in the pipeline.
        /// This enables the special matchers to handle special matching cases before the more simple generalized matchers process the rest.
        /// </summary>
        /// <param name="matchStrategy"></param>
        /// <param name="isSpecializedMatcher">true if <paramref name="matchStrategy"/> is a specialized matcher, false if it is a generalized matcher</param>
        public IDiffingStrategyCollection AddMatcher(MatchStrategy<SourceCollection, Comparison> matchStrategy, bool isSpecializedMatcher)
        {
            if (isSpecializedMatcher)
                _nodeMatchers.Insert(0, matchStrategy);
            else
                _nodeMatchers.Add(matchStrategy);
            return this;
        }

        /// <summary>
        /// Adds an attribute matcher to the pipeline.
        /// Specialized matchers always execute before any generalized matchers in the pipeline.
        /// This enables the special matchers to handle special matching cases before the more simple generalized matchers process the rest.
        /// </summary>
        /// <param name="matchStrategy"></param>
        /// <param name="isSpecializedMatcher">true if <paramref name="matchStrategy"/> is a specialized matcher, false if it is a generalized matcher</param>
        public IDiffingStrategyCollection AddMatcher(MatchStrategy<SourceMap, AttributeComparison> matchStrategy, bool isSpecializedMatcher)
        {
            if (isSpecializedMatcher)
                _attrsMatchers.Insert(0, matchStrategy);
            else
                _attrsMatchers.Add(matchStrategy);
            return this;
        }

        /// <summary>
        /// Adds a node comparer to the pipeline.
        /// Specialized comparers always execute after any generalized comparers in the pipeline.
        /// That enables them to correct for the generic comparers decision.
        /// </summary>
        /// <param name="compareStrategy"></param>
        /// <param name="isSpecializedComparer">true if <paramref name="compareStrategy"/> is a specialized comparer, false if it is a generalized comparer</param>
        public IDiffingStrategyCollection AddComparer(CompareStrategy<Comparison> compareStrategy, bool isSpecializedComparer)
        {
            if (isSpecializedComparer)
                _nodeComparers.Add(compareStrategy);
            else
                _nodeComparers.Insert(0, compareStrategy);
            return this;
        }

        /// <summary>
        /// Adds a attribute comparer to the pipeline.
        /// Specialized comparers always execute after any generalized comparers in the pipeline.
        /// That enables them to correct for the generic comparers decision.
        /// </summary>
        /// <param name="compareStrategy"></param>
        /// <param name="isSpecializedComparer">true if <paramref name="compareStrategy"/> is a specialized comparer, false if it is a generalized comparer</param>
        public IDiffingStrategyCollection AddComparer(CompareStrategy<AttributeComparison> compareStrategy, bool isSpecializedComparer)
        {
            if (isSpecializedComparer)
                _attrComparers.Add(compareStrategy);
            else
                _attrComparers.Insert(0, compareStrategy);
            return this;
        }

        private FilterDecision Filter<T>(in T source, List<FilterStrategy<T>> filterStrategies)
        {
            var result = FilterDecision.Keep;
            for (int i = 0; i < filterStrategies.Count; i++)
            {
                result = filterStrategies[i](source, result);
            }
            return result;
        }

        private CompareResult Compare<TComparison>(in TComparison comparison, List<CompareStrategy<TComparison>> compareStrategies, CompareResult initialResult)
        {
            var result = initialResult;
            foreach (var comparer in compareStrategies)
            {
                result = comparer(comparison, result);
            }
            return result;
        }
    }
}
