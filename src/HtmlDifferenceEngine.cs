using System;
using System.Collections.Generic;
using System.Linq;
using AngleSharp.Dom;

namespace Egil.AngleSharp.Diffing
{

    public enum CompareResult
    {
        Same,
        Different
    }

    public interface IHtmlCompareStrategy
    {
        bool NodeFilter(INode node);
        bool AttributeFilter(IAttr attribute, IElement owningElement);
        CompareResult CompareNode(in Comparison comparison);
        CompareResult CompareAttribute(string attributeName, in Comparison comparisonContext);
    }

    public class HtmlDifferenceEngine
    {
        private readonly IHtmlCompareStrategy _strategy;

        public HtmlDifferenceEngine(IHtmlCompareStrategy strategy)
        {
            _strategy = strategy;
        }

        public IReadOnlyList<Diff> Compare(INodeList controlNodes, INodeList testNodes) => DoCompare(controlNodes, testNodes);

        private IReadOnlyList<Diff> DoCompare(INodeList rootControlNodes, INodeList rootTestNodes)
        {
            if (rootControlNodes is null) throw new ArgumentNullException(nameof(rootControlNodes));
            if (rootTestNodes is null) throw new ArgumentNullException(nameof(rootTestNodes));

            var result = new List<Diff>();

            var compareQueue = new Queue<(INodeList ControlNodes, INodeList TestNodes)>();
            compareQueue.Enqueue((rootControlNodes, rootTestNodes));

            while (compareQueue.Count > 0)
            {
                var (controlNodes, testNodes) = compareQueue.Dequeue();
                var comparisons = GetComparisons(controlNodes, testNodes);

                foreach (var comparison in comparisons)
                {
                    result.AddRange(GetDifferences(in comparison));
                }

                var matchedTestNodes = comparisons.Where(x => x.Test.HasValue).Select(x => x.Test.Value);
                var unmatchedTestNodes = testNodes.Select((n, i) => new ComparisonSource(n, i)).Except(matchedTestNodes);

                // detect unmatched test nodes
                foreach (var node in unmatchedTestNodes)
                {
                    result.Add(node.Node.NodeType switch
                    {
                        NodeType.Comment => new Diff(DiffType.UnexpectedComment, test: node),
                        NodeType.Element => new Diff(DiffType.UnexpectedElement, test: node),
                        NodeType.Text => new Diff(DiffType.UnexpectedTextNode, test: node),
                        _ => throw new InvalidOperationException($"Unexpected nodetype, {node.Node.NodeType}, in test nodes list.")
                    });
                }

                foreach (var c in comparisons)
                {
                    if (c.Status == MatchStatus.TestNodeNotFound) continue;
                    if (c.Control.Node.HasChildNodes || (c.Test?.Node.HasChildNodes ?? false))
                        compareQueue.Enqueue((c.Control.Node.ChildNodes, c.Test?.Node.ChildNodes ?? EmptyNodeList.Instance));
                }
            }

            return result;
        }

        private List<Comparison> GetComparisons(INodeList controlNodes, INodeList testNodes)
        {
            var evenTreeBranch = controlNodes.Length == testNodes.Length;
            var result = new List<Comparison>(controlNodes.Length);
            var lastFoundIndex = -1;

            for (int index = 0; index < controlNodes.Length; index++)
            {
                var controlSource = new ComparisonSource(controlNodes[index], index);

                //if (ShouldSkipNode(controlNode)) continue;

                // How does this work when nodes are skipped due to skipping strategy?
                var testSource = evenTreeBranch
                    ? EqualTreeSizeNodeMatcher(in controlSource)
                    : ForwardSearchingNodeMatcher(in controlSource);

                result.Add(new Comparison(controlSource, testSource));
            }

            return result;

            //bool ShouldSkipNode(INode node) => !_strategy.NodeFilter(node);

            ComparisonSource? EqualTreeSizeNodeMatcher(in ComparisonSource comparisonSource)
            {
                // Consider skipping strategy effect
                return new ComparisonSource(testNodes[comparisonSource.Index], comparisonSource.Index);
            }

            ComparisonSource? ForwardSearchingNodeMatcher(in ComparisonSource comparisonSource)
            {
                // Consider skipping strategy effect
                ComparisonSource? result = null;

                // If there are more control nodes than test nodes, then search from the last found index.
                var index = testNodes.Length > comparisonSource.Index
                    ? Math.Max(comparisonSource.Index, lastFoundIndex + 1)
                    : lastFoundIndex + 1;

                while (result is null && testNodes.Length > index)
                {
                    // Should this be a stronger comparison than just nodename?
                    if (comparisonSource.Node.NodeName == testNodes[index].NodeName)
                    {
                        result = new ComparisonSource(testNodes[index], index);
                        lastFoundIndex = index;
                    }
                    index++;
                }

                return result;
            }
        }

        private ICollection<Diff> GetDifferences(in Comparison comparison) // in
        {
            var result = new List<Diff>(1);

            if (comparison.Status == MatchStatus.TestNodeNotFound)
            {
                result.Add(comparison.Control.Node.NodeType switch
                {
                    NodeType.Comment => new Diff(DiffType.MissingComment, comparison.Control),
                    NodeType.Element => new Diff(DiffType.MissingElement, comparison.Control),
                    NodeType.Text => new Diff(DiffType.MissingTextNode, comparison.Control),
                    _ => throw new InvalidOperationException($"Unexpected nodetype, {comparison.Control.Node.NodeType}, in test nodes list.")
                });
            }
            else
            {
                if (_strategy.CompareNode(in comparison) == CompareResult.Different)
                {
                    result.Add(comparison.Control.Node.NodeType switch
                    {
                        NodeType.Comment => new Diff(DiffType.DifferentComment, comparison.Control, comparison.Test),
                        NodeType.Element => new Diff(DiffType.DifferentElementTagName, comparison.Control, comparison.Test),
                        NodeType.Text => new Diff(DiffType.DifferentTextNode, comparison.Control, comparison.Test),
                        _ => throw new InvalidOperationException($"Unexpected nodetype, {comparison.Control.Node.NodeType}, in test nodes list.")
                    });
                }

                var controlElm = comparison.Control.Node as IElement;
                var testElm = comparison.Test?.Node as IElement;

                if (controlElm is { } && testElm is { })
                {
                    foreach (var controlAttr in controlElm.Attributes)
                    {
                        if (testElm.HasAttribute(controlAttr.Name))
                        {
                            var attrCompareRes = _strategy.CompareAttribute(controlAttr.Name, in comparison);
                            if(attrCompareRes== CompareResult.Different)
                            {
                                result.Add(new Diff(DiffType.DifferentAttribute, comparison.Control, comparison.Test));
                            }
                        }
                    }
                }
            }

            return result;
        }
    }
}
