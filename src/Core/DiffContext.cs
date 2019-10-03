using System.Collections.Generic;
using AngleSharp.Dom;
using Egil.AngleSharp.Diffing.Extensions;

namespace Egil.AngleSharp.Diffing.Core
{
    public class DiffContext
    {
        private readonly IElement? _controlRoot;
        private readonly IElement? _testRoot;

        internal HashSet<ComparisonSource> MissingSources { get; }

        internal HashSet<ComparisonSource> UnexpectedSources { get; }

        public DiffContext(IElement? controlRoot, IElement? testRoot)
        {
            _controlRoot = controlRoot;
            _testRoot = testRoot;
            MissingSources = new HashSet<ComparisonSource>();
            UnexpectedSources = new HashSet<ComparisonSource>();
        }

        public IHtmlCollection<IElement> QueryControlRoot(string selector)
        {
            if (_controlRoot is null) return EmptyHtmlCollection<IElement>.Empty;
            return _controlRoot.QuerySelectorAll(selector);
        }

        public IHtmlCollection<IElement> QueryTestRoot(string selector)
        {
            if (_testRoot is null) return EmptyHtmlCollection<IElement>.Empty;
            return _testRoot.QuerySelectorAll(selector);
        }
    }
}
