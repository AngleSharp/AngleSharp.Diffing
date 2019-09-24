using System;
using AngleSharp.Dom;

namespace Egil.AngleSharp.Diffing
{
    public partial class HtmlDifferenceEngineTest
    {
        private class MockHtmlCompareStrategy : IHtmlCompareStrategy
        {
            private readonly Func<INode, bool> _nodeFilter;
            private readonly Func<IAttr, IElement, bool> _attrFilter;
            private readonly Func<Comparison, CompareResult> _comparer;

            public int CompareCalledTimes { get; set; }
            public int FindMatchCalledTimes { get; set; }
            public int AttributeFilterCalledTimes { get; set; }
            public int NodeFilterCalledTimes { get; set; }

            public MockHtmlCompareStrategy(
                Func<INode, bool>? nodeFilter = null,
                Func<IAttr, IElement, bool>? attrFilter = null,
                Func<Comparison, CompareResult>? comparer = null)
            {
                _nodeFilter = nodeFilter ?? (_ => true);
                _attrFilter = attrFilter ?? ((a, e) => true);
                _comparer = comparer ?? (_ => CompareResult.Same);
            }

            public bool NodeFilter(INode node)
            {
                NodeFilterCalledTimes++;
                return _nodeFilter(node);
            }

            public bool AttributeFilter(IAttr attribute, IElement owningElement)
            {
                AttributeFilterCalledTimes++;
                return _attrFilter(attribute, owningElement);
            }

            public CompareResult Compare(in Comparison comparison)
            {
                CompareCalledTimes++;
                return _comparer(comparison);
            }
        }
    }
}
