using System;
using System.Collections.Generic;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using Egil.AngleSharp.Diffing.Core;
using Egil.AngleSharp.Diffing.Strategies;

namespace Egil.AngleSharp.Diffing
{
    public class DiffBuilder
    {
        private readonly IBrowsingContext _context;
        private readonly IHtmlParser _htmlParser;
        private readonly IDocument _document;
        private readonly DiffingStrategyPipeline _strategyPipeline;

        private string _control = string.Empty;
        private string _test = string.Empty;

        public string Control { get => _control; set => _control = value ?? throw new ArgumentNullException(nameof(Control)); }

        public string Test { get => _test; set => _test = value ?? throw new ArgumentNullException(nameof(Test)); }

        private DiffBuilder(string control)
        {
            Control = control;
            var config = Configuration.Default.WithCss();
            _context = BrowsingContext.New(config);
            _htmlParser = _context.GetService<IHtmlParser>();
            _document = _context.OpenNewAsync().Result;
            _strategyPipeline = new DiffingStrategyPipeline();
        }

        public DiffBuilder WithTest(string test)
        {
            Test = test;
            return this;
        }

        public static DiffBuilder Compare(string control)
        {
            return new DiffBuilder(control);
        }

        /// <summary>
        /// Adds a node filter to the pipeline.
        /// Specialized filters always execute after any generalized filters in the pipeline.
        /// That enables them to correct for the generic filters decision.
        /// </summary>
        /// <param name="filterStrategy"></param>
        /// <param name="isSpecializedFilter">true if <paramref name="filterStrategy"/> is a specialized filter, false if it is a generalized filter</param>
        public DiffBuilder WithFilter(FilterStrategy<ComparisonSource> filterStrategy, bool isSpecializedFilter = true)
        {
            _strategyPipeline.AddFilter(filterStrategy, isSpecializedFilter);
            return this;
        }

        /// <summary>
        /// Adds an attribute filter to the pipeline.
        /// Specialized filters always execute after any generalized filters in the pipeline.
        /// That enables them to correct for the generic filters decision.
        /// </summary>
        /// <param name="filterStrategy"></param>
        /// <param name="isSpecializedFilter">true if <paramref name="filterStrategy"/> is a specialized filter, false if it is a generalized filter</param>
        public DiffBuilder WithFilter(FilterStrategy<AttributeComparisonSource> filterStrategy, bool isSpecializedFilter = true)
        {
            _strategyPipeline.AddFilter(filterStrategy, isSpecializedFilter);
            return this;
        }

        /// <summary>
        /// Adds a node matcher to the pipeline.
        /// Specialized matchers always execute before any generalized matchers in the pipeline.
        /// This enables the special matchers to handle special matching cases before the more simple generalized matchers process the rest.
        /// </summary>
        /// <param name="matchStrategy"></param>
        /// <param name="isSpecializedMatcher">true if <paramref name="matchStrategy"/> is a specialized matcher, false if it is a generalized matcher</param>
        public DiffBuilder WithMatcher(MatchStrategy<SourceCollection, Comparison> matchStrategy, bool isSpecializedMatcher = true)
        {
            _strategyPipeline.AddMatcher(matchStrategy, isSpecializedMatcher);
            return this;
        }

        /// <summary>
        /// Adds an attribute matcher to the pipeline.
        /// Specialized matchers always execute before any generalized matchers in the pipeline.
        /// This enables the special matchers to handle special matching cases before the more simple generalized matchers process the rest.
        /// </summary>
        /// <param name="matchStrategy"></param>
        /// <param name="isSpecializedMatcher">true if <paramref name="matchStrategy"/> is a specialized matcher, false if it is a generalized matcher</param>
        public DiffBuilder WithMatcher(MatchStrategy<SourceMap, AttributeComparison> matchStrategy, bool isSpecializedMatcher = true)
        {
            _strategyPipeline.AddMatcher(matchStrategy, isSpecializedMatcher);
            return this;
        }

        /// <summary>
        /// Adds a node comparer to the pipeline.
        /// Specialized comparers always execute after any generalized comparers in the pipeline.
        /// That enables them to correct for the generic comparers decision.
        /// </summary>
        /// <param name="compareStrategy"></param>
        /// <param name="isSpecializedComparer">true if <paramref name="compareStrategy"/> is a specialized comparer, false if it is a generalized comparer</param>
        public DiffBuilder WithComparer(CompareStrategy<Comparison> compareStrategy, bool isSpecializedComparer = true)
        {
            _strategyPipeline.AddComparer(compareStrategy, isSpecializedComparer);
            return this;
        }

        /// <summary>
        /// Adds a attribute comparer to the pipeline.
        /// Specialized comparers always execute after any generalized comparers in the pipeline.
        /// That enables them to correct for the generic comparers decision.
        /// </summary>
        /// <param name="compareStrategy"></param>
        /// <param name="isSpecializedComparer">true if <paramref name="compareStrategy"/> is a specialized comparer, false if it is a generalized comparer</param>
        public DiffBuilder WithComparer(CompareStrategy<AttributeComparison> compareStrategy, bool isSpecializedComparer = true)
        {
            _strategyPipeline.AddComparer(compareStrategy, isSpecializedComparer);
            return this;
        }

        public IList<IDiff> Build()
        {
            if (!_strategyPipeline.HasMatchers)
                throw new InvalidOperationException("No comparer's has been added to the builder. Add at least one and try again.");
            if (!_strategyPipeline.HasComparers)
                throw new InvalidOperationException("No matcher's has been added to the builder. Add at least one and try again.");

            return new HtmlDifferenceEngine(_strategyPipeline, _strategyPipeline, _strategyPipeline)
                .Compare(Parse(Control), Parse(Test));
        }

        private INodeList Parse(string html)
        {
            return _htmlParser.ParseFragment(html, _document.Body);
        }
    }
}
