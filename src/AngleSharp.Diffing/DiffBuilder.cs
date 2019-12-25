using System;
using System.Collections.Generic;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using AngleSharp.Diffing.Core;
using AngleSharp.Diffing.Strategies;

namespace AngleSharp.Diffing
{
    public class DiffBuilder
    {
        private readonly IBrowsingContext _context;
        private readonly IHtmlParser _htmlParser;
        private readonly IDocument _document;
        private DiffingStrategyPipeline? _diffStrategy;
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
        }

        public static DiffBuilder Compare(string control)
        {
            return new DiffBuilder(control);
        }

        public DiffBuilder WithTest(string test)
        {
            Test = test;
            return this;
        }

        public DiffBuilder WithOptions(Action<IDiffingStrategyCollection> registerOptions)
        {
            _diffStrategy = new DiffingStrategyPipeline();
            registerOptions(_diffStrategy);
            return this;
        }

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

        protected INodeList Parse(string html)
        {
            return _htmlParser.ParseFragment(html, _document.Body);
        }
    }
}
