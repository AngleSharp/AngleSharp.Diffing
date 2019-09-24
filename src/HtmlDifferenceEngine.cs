using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        CompareResult Compare(in Comparison comparison);
    }

    public class HtmlDifferenceEngine
    {
        private readonly IHtmlCompareStrategy _strategy;

        public HtmlDifferenceEngine(IHtmlCompareStrategy strategy)
        {
            _strategy = strategy;
        }

        public IReadOnlyCollection<Diff> Compare(INodeList controlNodes, INodeList testNodes) => DoCompare(controlNodes, testNodes);

        private IReadOnlyCollection<Diff> DoCompare(INodeList controlNodes, INodeList testNodes)
        {
            if (controlNodes is null) throw new ArgumentNullException(nameof(controlNodes));
            if (testNodes is null) throw new ArgumentNullException(nameof(testNodes));

            var result = new List<Diff>();

            var comparisons = GetComparisons(controlNodes, testNodes);

            foreach (var comparison in comparisons)
            {
                result.AddRange(GetDifferences(in comparison));
                //// Compare child nodes
                //if (comparison.Control.HasChildNodes)
                //    foreach (var diff in Compare(comparison.Control.ChildNodes, comparison.Test.ChildNodes))
                //        yield return diff;
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

            return result;
        }

        private ICollection<Comparison> GetComparisons(INodeList controlNodes, INodeList testNodes)
        {
            var evenTreeBranch = controlNodes.Length == testNodes.Length;
            var result = new Comparison[controlNodes.Length];
            var lastFoundIndex = -1;

            for (int index = 0; index < controlNodes.Length; index++)
            {
                var controlSource = new ComparisonSource(controlNodes[index], index);

                //if (ShouldSkipNode(controlNode)) continue;

                // How does this work when nodes are skipped due to skipping strategy?
                var testSource = evenTreeBranch
                    ? EqualTreeSizeNodeMatcher(in controlSource)
                    : ForwardSearchingNodeMatcher(in controlSource);

                result[index] = new Comparison(controlSource, testSource);
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
            else if (comparison.Status == MatchStatus.TestNodeFound && _strategy.Compare(in comparison) == CompareResult.Different)
            {
                result.Add(comparison.Control.Node.NodeType switch
                {
                    NodeType.Comment => new Diff(DiffType.DifferentComment, comparison.Control, comparison.Test),
                    NodeType.Element => new Diff(DiffType.DifferentElementTagName, comparison.Control, comparison.Test),
                    NodeType.Text => new Diff(DiffType.DifferentTextNode, comparison.Control, comparison.Test),
                    _ => throw new InvalidOperationException($"Unexpected nodetype, {comparison.Control.Node.NodeType}, in test nodes list.")
                });
            }

            return result;
        }
    }

    public enum DiffType
    {
        DifferentComment,
        DifferentElementTagName,
        DifferentTextNode,
        MissingComment,
        MissingElement,
        MissingTextNode,
        UnexpectedComment,
        UnexpectedElement,
        UnexpectedTextNode
    }

    [DebuggerDisplay("Diff={Type} Control={Control?.Node.NodeName}[{Control?.Index}] Test={Test?.Node.NodeName}[{Test?.Index}]")]
    public readonly struct Diff : IEquatable<Diff>
    {
        public DiffType Type { get; }

        public ComparisonSource? Control { get; }

        public ComparisonSource? Test { get; }

        public Diff(DiffType type, in ComparisonSource? control = null, in ComparisonSource? test = null)
        {
            Type = type;
            Control = control;
            Test = test;
        }

        #region Equals and Hashcode
        public bool Equals(Diff other) => Type == other.Type;
        public override int GetHashCode() => (Type).GetHashCode();
        public override bool Equals(object obj) => obj is Diff other && Equals(other);
        public static bool operator ==(Diff left, Diff right) => left.Equals(right);
        public static bool operator !=(Diff left, Diff right) => !(left == right);
        #endregion
    }

    public readonly struct Comparison : IEquatable<Comparison>
    {
        public ComparisonSource Control { get; }

        public ComparisonSource? Test { get; }

        public MatchStatus Status => Test is null ? MatchStatus.TestNodeNotFound : MatchStatus.TestNodeFound;

        public Comparison(in ComparisonSource control, in ComparisonSource? test)
        {
            Control = control;
            Test = test;
        }

        #region Equals and HashCode
        public bool Equals(Comparison other) => Control == other.Control && Test == other.Test;
        public override bool Equals(object obj) => obj is Comparison other && Equals(other);
        public override int GetHashCode() => (Control, Test).GetHashCode();
        public static bool operator ==(Comparison left, Comparison right) => left.Equals(right);
        public static bool operator !=(Comparison left, Comparison right) => !(left == right);
        #endregion
    }

    public readonly struct ComparisonSource : IEquatable<ComparisonSource>
    {
        public INode Node { get; }
        public int Index { get; }

        public ComparisonSource(INode node, int index)
        {
            Node = node;
            Index = index;
        }

        #region Equals and HashCode
        public bool Equals(ComparisonSource other) => Node == other.Node && Index == other.Index;
        public override int GetHashCode() => (Node, Index).GetHashCode();
        public override bool Equals(object obj) => obj is ComparisonSource other && Equals(other);
        public static bool operator ==(ComparisonSource left, ComparisonSource right) => left.Equals(right);
        public static bool operator !=(ComparisonSource left, ComparisonSource right) => !(left == right);
        #endregion
    }
}
