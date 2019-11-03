using System.Collections.Generic;
using AngleSharp.Dom;
using AngleSharp.Diffing.Extensions;

namespace AngleSharp.Diffing.Core
{
    public class DiffContext
    {
        private readonly IElement? _controlRoot;
        private readonly IElement? _testRoot;

        internal HashSet<ComparisonSource> MissingSources { get; }

        internal HashSet<ComparisonSource> UnexpectedSources { get; }

        internal HashSet<AttributeComparisonSource> MissingAttributeSources { get; }

        internal HashSet<AttributeComparisonSource> UnexpectedAttributeSources { get; }

        public DiffContext(IElement? controlRoot, IElement? testRoot)
        {
            _controlRoot = controlRoot;
            _testRoot = testRoot;
            MissingSources = new HashSet<ComparisonSource>();
            UnexpectedSources = new HashSet<ComparisonSource>();
            MissingAttributeSources = new HashSet<AttributeComparisonSource>();
            UnexpectedAttributeSources = new HashSet<AttributeComparisonSource>();
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

        internal IEnumerable<IDiff> GetDiffsFromUnmatched()
        {
            foreach (var source in MissingSources) yield return new MissingNodeDiff(source);
            foreach (var source in UnexpectedSources) yield return new UnexpectedNodeDiff(source);
            foreach (var source in MissingAttributeSources) yield return new MissingAttrDiff(source);
            foreach (var source in UnexpectedAttributeSources) yield return new UnexpectedAttrDiff(source);
        }
    }
}
