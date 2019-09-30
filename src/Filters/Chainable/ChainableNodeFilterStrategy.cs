using AngleSharp.Dom;
using Egil.AngleSharp.Diffing.Comparisons;

namespace Egil.AngleSharp.Diffing.Filters.Chainable
{
    public delegate bool ChainableNodeFilterStrategy<TNode>(in IComparisonSource<TNode> comparisonSource, bool currentDecision)
        where TNode : INode;
}
