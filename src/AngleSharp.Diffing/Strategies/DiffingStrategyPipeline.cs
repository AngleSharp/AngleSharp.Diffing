namespace AngleSharp.Diffing.Strategies;

/// <summary>
/// Represents a <see cref="IDiffingStrategy"/> and a <see cref="IDiffingStrategyCollection"/>.
/// Use the pipeline to invoke the strategies registered within it as a pipeline, one at the time.
/// </summary>
public class DiffingStrategyPipeline : IDiffingStrategy, IDiffingStrategyCollection
{
    private readonly List<FilterStrategy<ComparisonSource>> _nodeFilters = new List<FilterStrategy<ComparisonSource>>();
    private readonly List<FilterStrategy<AttributeComparisonSource>> _attrsFilters = new List<FilterStrategy<AttributeComparisonSource>>();
    private readonly List<MatchStrategy<SourceCollection, Comparison>> _nodeMatchers = new List<MatchStrategy<SourceCollection, Comparison>>();
    private readonly List<MatchStrategy<SourceMap, AttributeComparison>> _attrsMatchers = new List<MatchStrategy<SourceMap, AttributeComparison>>();
    private readonly List<CompareStrategy<Comparison>> _nodeComparers = new List<CompareStrategy<Comparison>>();
    private readonly List<CompareStrategy<AttributeComparison>> _attrComparers = new List<CompareStrategy<AttributeComparison>>();

    /// <summary>
    /// Gets whether the pipeline have any matchers registered.
    /// </summary>
    public bool HasMatchers => _nodeMatchers.Count > 0 && _attrsMatchers.Count > 0;

    /// <summary>
    /// Gets whether the pipeline has any comparers registered.
    /// </summary>
    public bool HasComparers => _nodeComparers.Count > 0 && _attrComparers.Count > 0;

    /// <inheritdoc/>
    public FilterDecision Filter(in ComparisonSource comparisonSource) => Filter(comparisonSource, _nodeFilters);

    /// <inheritdoc/>
    public FilterDecision Filter(in AttributeComparisonSource attributeComparisonSource) => Filter(attributeComparisonSource, _attrsFilters);

    /// <inheritdoc/>
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

    /// <inheritdoc/>
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

    /// <inheritdoc/>
    public CompareResult Compare(in Comparison comparison) => Compare(comparison, _nodeComparers, CompareResult.Different(null));

    /// <inheritdoc/>
    public CompareResult Compare(in AttributeComparison comparison) => Compare(comparison, _attrComparers, CompareResult.Different(null));

    /// <inheritdoc/>
    public IDiffingStrategyCollection AddFilter(FilterStrategy<ComparisonSource> filterStrategy, StrategyType strategyType)
    {
        if (strategyType == StrategyType.Specialized)
            _nodeFilters.Add(filterStrategy);
        else
            _nodeFilters.Insert(0, filterStrategy);
        return this;
    }

    /// <inheritdoc/>
    public IDiffingStrategyCollection AddFilter(FilterStrategy<AttributeComparisonSource> filterStrategy, StrategyType strategyType)
    {
        if (strategyType == StrategyType.Specialized)
            _attrsFilters.Add(filterStrategy);
        else
            _attrsFilters.Insert(0, filterStrategy);
        return this;
    }

    /// <inheritdoc/>
    public IDiffingStrategyCollection AddMatcher(MatchStrategy<SourceCollection, Comparison> matchStrategy, StrategyType strategyType)
    {
        if (strategyType == StrategyType.Specialized)
            _nodeMatchers.Insert(0, matchStrategy);
        else
            _nodeMatchers.Add(matchStrategy);
        return this;
    }

    /// <inheritdoc/>
    public IDiffingStrategyCollection AddMatcher(MatchStrategy<SourceMap, AttributeComparison> matchStrategy, StrategyType strategyType)
    {
        if (strategyType == StrategyType.Specialized)
            _attrsMatchers.Insert(0, matchStrategy);
        else
            _attrsMatchers.Add(matchStrategy);
        return this;
    }

    /// <inheritdoc/>
    public IDiffingStrategyCollection AddComparer(CompareStrategy<Comparison> compareStrategy, StrategyType strategyType)
    {
        if (strategyType == StrategyType.Specialized)
            _nodeComparers.Add(compareStrategy);
        else
            _nodeComparers.Insert(0, compareStrategy);
        return this;
    }

    /// <inheritdoc/>
    public IDiffingStrategyCollection AddComparer(CompareStrategy<AttributeComparison> compareStrategy, StrategyType strategyType)
    {
        if (strategyType == StrategyType.Specialized)
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
