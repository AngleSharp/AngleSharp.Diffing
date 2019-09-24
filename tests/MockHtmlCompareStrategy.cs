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
            private readonly Func<Comparison, CompareResult> _nodeComparer;
            private readonly Func<string, Comparison, CompareResult> _attrComparer;

            public int CompareNodeCalledTimes { get; set; }
            public int CompareAttributeCalledTimes { get; set; }
            public int AttributeFilterCalledTimes { get; set; }
            public int NodeFilterCalledTimes { get; set; }

            public MockHtmlCompareStrategy(
                Func<INode, bool>? nodeFilter = null,
                Func<IAttr, IElement, bool>? attrFilter = null,
                Func<Comparison, CompareResult>? nodeComparer = null,
                Func<string, Comparison, CompareResult>? attrComparer = null)
            {
                _nodeFilter = nodeFilter ?? (_ => true);
                _attrFilter = attrFilter ?? ((a, e) => true);
                _nodeComparer = nodeComparer ?? (_ => CompareResult.Same);
                _attrComparer = attrComparer ?? ((a,c) => CompareResult.Same);
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

            public CompareResult CompareNode(in Comparison comparison)
            {
                CompareNodeCalledTimes++;
                return _nodeComparer(comparison);
            }

            public CompareResult CompareAttribute(string attributeName, in Comparison comparisonContext)
            {
                CompareAttributeCalledTimes++;
                return _attrComparer(attributeName, comparisonContext);
            }
        }
    }
}
