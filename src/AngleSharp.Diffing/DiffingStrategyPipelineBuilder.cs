using System;
using AngleSharp.Diffing.Core;
using AngleSharp.Diffing.Strategies;

namespace AngleSharp.Diffing
{

    public class DiffingStrategyPipelineBuilder : IDiffingStrategyPipelineBuilder
    {
        private readonly DiffingStrategyPipeline _strategyPipeline = new DiffingStrategyPipeline();

        public IDiffingStrategyPipelineBuilder WithFilter(FilterStrategy<ComparisonSource> filterStrategy, bool isSpecializedFilter = true)
        {
            _strategyPipeline.AddFilter(filterStrategy, isSpecializedFilter);
            return this;
        }

        public IDiffingStrategyPipelineBuilder WithFilter(FilterStrategy<AttributeComparisonSource> filterStrategy, bool isSpecializedFilter = true)
        {
            _strategyPipeline.AddFilter(filterStrategy, isSpecializedFilter);
            return this;
        }

        public IDiffingStrategyPipelineBuilder WithMatcher(MatchStrategy<SourceCollection, Comparison> matchStrategy, bool isSpecializedMatcher = true)
        {
            _strategyPipeline.AddMatcher(matchStrategy, isSpecializedMatcher);
            return this;
        }

        public IDiffingStrategyPipelineBuilder WithMatcher(MatchStrategy<SourceMap, AttributeComparison> matchStrategy, bool isSpecializedMatcher = true)
        {
            _strategyPipeline.AddMatcher(matchStrategy, isSpecializedMatcher);
            return this;
        }

        public IDiffingStrategyPipelineBuilder WithComparer(CompareStrategy<Comparison> compareStrategy, bool isSpecializedComparer = true)
        {
            _strategyPipeline.AddComparer(compareStrategy, isSpecializedComparer);
            return this;
        }

        public IDiffingStrategyPipelineBuilder WithComparer(CompareStrategy<AttributeComparison> compareStrategy, bool isSpecializedComparer = true)
        {
            _strategyPipeline.AddComparer(compareStrategy, isSpecializedComparer);
            return this;
        }

        public DiffingStrategyPipeline Build()
        {
            if (!_strategyPipeline.HasMatchers)
                throw new InvalidOperationException("No comparer's has been added to the pipeline. Add at least one and try again.");
            if (!_strategyPipeline.HasComparers)
                throw new InvalidOperationException("No matcher's has been added to the pipeline. Add at least one and try again.");

            return _strategyPipeline;
        }
    }
}
