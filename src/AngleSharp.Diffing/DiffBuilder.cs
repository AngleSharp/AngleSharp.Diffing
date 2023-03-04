using System;
using System.Collections.Generic;

using AngleSharp.Diffing.Core;
using AngleSharp.Diffing.Extensions;
using AngleSharp.Diffing.Strategies;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;

namespace AngleSharp.Diffing
{
    /// <summary>
    /// Use the <see cref="DiffBuilder"/> to easily set up a comparison of a control and test DOM tree.
    /// </summary>
    public class DiffBuilder
    {
        private readonly IBrowsingContext _context;
        private readonly IHtmlParser _htmlParser;
        private readonly IDocument _document;
        private DiffingStrategyPipeline? _diffStrategy;
        private string _control = string.Empty;
        private string _test = string.Empty;

        /// <summary>
        /// Gets or sets the control markup string.
        /// </summary>
        public string Control { get => _control; set => _control = value ?? throw new ArgumentNullException(nameof(Control)); }

        /// <summary>
        /// Gets or sets the test markup string.
        /// </summary>
        public string Test { get => _test; set => _test = value ?? throw new ArgumentNullException(nameof(Test)); }

        private DiffBuilder(string control)
        {
            Control = control;

            // Create a custom config with a parser to allow access to the source reference from the AST.
            var config = Configuration.Default
                .WithCss()
                .With<IHtmlParser>(ctx => new HtmlParser(new HtmlParserOptions
                {
                    IsKeepingSourceReferences = true,
                    IsScripting = ctx?.IsScripting() ?? false
                }, ctx));

            _context = BrowsingContext.New(config);
            _htmlParser = _context.GetService<IHtmlParser>()
                ?? throw new InvalidOperationException("No IHtmlParser registered in the default AngleSharp browsing context.");
            _document = _context.OpenNewAsync().Result;
        }

        /// <summary>
        /// Creates a <see cref="DiffBuilder"/> with the provided control markup.
        /// </summary>
        public static DiffBuilder Compare(string control)
        {
            return new DiffBuilder(control);
        }

        /// <summary>
        /// Sets the <see cref="Test"/> markup used during comparison.
        /// </summary>
        public DiffBuilder WithTest(string test)
        {
            Test = test;
            return this;
        }

        /// <summary>
        /// Add any options/strategies that should be used during comparison.
        /// </summary>
        public DiffBuilder WithOptions(Action<IDiffingStrategyCollection> registerOptions)
        {
            _diffStrategy = new DiffingStrategyPipeline();
            registerOptions(_diffStrategy);
            return this;
        }

        /// <summary>
        /// Execute the comparison operation and returns any differences found.
        /// </summary>
        public IEnumerable<IDiff> Build()
        {
            if (_diffStrategy is null)
            {
                _diffStrategy = new DiffingStrategyPipeline();
                _diffStrategy.AddDefaultOptions();
            }

            var controls = Parse(Control).ToSourceCollection(ComparisonSourceType.Control);
            var tests = Parse(Test).ToSourceCollection(ComparisonSourceType.Test);

            return new HtmlDifferenceEngine(_diffStrategy, controls, tests).Compare();
        }

        /// <summary>
        /// Parse the provided markup into a AngleSharp DOM tree.
        /// </summary>
        protected INodeList Parse(string html)
        {
            return _htmlParser.ParseFragment(html, _document.Body ?? throw new UnexpectedDOMTreeStructureException());
        }
    }
}
