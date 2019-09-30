using AngleSharp.Dom;
using Egil.AngleSharp.Diffing.Comparisons;

namespace Egil.AngleSharp.Diffing.Comparers.Chainable
{
    public delegate CompareResult ChainableNodeComparerStrategy<TNode>(in IComparison<TNode> comparison, CompareResult currentDecision)
        where TNode : INode;
}
