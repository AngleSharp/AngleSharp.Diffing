using System;
using System.Collections.Generic;
using AngleSharp;
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
        private readonly DiffingStrategyPipeline _diffStrategy;
        private string _control = string.Empty;
        private string _test = string.Empty;

        public string Control { get => _control; set => _control = value ?? throw new ArgumentNullException(nameof(Control)); }

        public string Test { get => _test; set => _test = value ?? throw new ArgumentNullException(nameof(Test)); }

        public DiffBuilder(DiffingStrategyPipeline diffStrategy)
        {
            if(diffStrategy is null) throw new ArgumentNullException(nameof(diffStrategy));
            _diffStrategy = diffStrategy;

            var config = Configuration.Default.WithCss();
            _context = BrowsingContext.New(config);
            _htmlParser = _context.GetService<IHtmlParser>();
            _document = _context.OpenNewAsync().Result;
        }

        public DiffBuilder Compare(string control)
        {
            Control = control;
            return this;
        }

        public DiffBuilder WithTest(string test)
        {
            Test = test;
            return this;
        }

        public IList<IDiff> Build()
        {
            return new HtmlDifferenceEngine(_diffStrategy, _diffStrategy, _diffStrategy)
                .Compare(Parse(Control), Parse(Test));
        }

        protected INodeList Parse(string html)
        {
            return _htmlParser.ParseFragment(html, _document.Body);
        }
    }
}
