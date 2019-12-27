using AngleSharp.Diffing.Core;
using AngleSharp.Diffing.Strategies;

namespace AngleSharp.Diffing.Strategies
{
    public interface IDiffingStrategyCollection
    {
        /// <summary>
        /// Adds a attribute comparer to the pipeline.
        /// Specialized comparers always execute after any generalized comparers in the pipeline.
        /// That enables them to correct for the generic comparers decision.
        /// </summary>
        /// <param name="compareStrategy"></param>
        /// <param name="strategyType">true if <paramref name="compareStrategy"/> is a specialized comparer, false if it is a generalized comparer.</param>
        IDiffingStrategyCollection AddComparer(CompareStrategy<AttributeComparison> compareStrategy, StrategyType strategyType = StrategyType.Specialized);
        /// <summary>
        /// Adds a node comparer to the pipeline.
        /// Specialized comparers always execute after any generalized comparers in the pipeline.
        /// That enables them to correct for the generic comparers decision.
        /// </summary>
        /// <param name="compareStrategy"></param>
        /// <param name="strategyType">true if <paramref name="compareStrategy"/> is a specialized comparer, false if it is a generalized comparer.</param>
        IDiffingStrategyCollection AddComparer(CompareStrategy<Comparison> compareStrategy, StrategyType strategyType = StrategyType.Specialized);
        /// <summary>
        /// Adds an attribute filter to the pipeline.
        /// Specialized filters always execute after any generalized filters in the pipeline.
        /// That enables them to correct for the generic filters decision.
        /// </summary>
        /// <param name="filterStrategy"></param>
        /// <param name="strategyType">true if <paramref name="filterStrategy"/> is a specialized filter, false if it is a generalized filter.</param>
        IDiffingStrategyCollection AddFilter(FilterStrategy<AttributeComparisonSource> filterStrategy, StrategyType strategyType = StrategyType.Specialized);
        /// <summary>
        /// Adds a node filter to the pipeline.
        /// Specialized filters always execute after any generalized filters in the pipeline.
        /// That enables them to correct for the generic filters decision.
        /// </summary>
        /// <param name="filterStrategy"></param>
        /// <param name="strategyType">true if <paramref name="filterStrategy"/> is a specialized filter, false if it is a generalized filter.</param>
        IDiffingStrategyCollection AddFilter(FilterStrategy<ComparisonSource> filterStrategy, StrategyType strategyType = StrategyType.Specialized);
        /// <summary>
        /// Adds a node matcher to the pipeline.
        /// Specialized matchers always execute before any generalized matchers in the pipeline.
        /// This enables the special matchers to handle special matching cases before the more simple generalized matchers process the rest.
        /// </summary>
        /// <param name="matchStrategy"></param>
        /// <param name="strategyType">true if <paramref name="matchStrategy"/> is a specialized matcher, false if it is a generalized matcher.</param>
        IDiffingStrategyCollection AddMatcher(MatchStrategy<SourceCollection, Comparison> matchStrategy, StrategyType strategyType = StrategyType.Specialized);
        /// <summary>
        /// Adds an attribute matcher to the pipeline.
        /// Specialized matchers always execute before any generalized matchers in the pipeline.
        /// This enables the special matchers to handle special matching cases before the more simple generalized matchers process the rest.
        /// </summary>
        /// <param name="matchStrategy"></param>
        /// <param name="strategyType">true if <paramref name="matchStrategy"/> is a specialized matcher, false if it is a generalized matcher.</param>
        IDiffingStrategyCollection AddMatcher(MatchStrategy<SourceMap, AttributeComparison> matchStrategy, StrategyType strategyType = StrategyType.Specialized);
    }
}