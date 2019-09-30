using AngleSharp.Dom;
using Egil.AngleSharp.Diffing.Comparisons;

namespace Egil.AngleSharp.Diffing.Filters.Chainable
{
    public interface IChainableNodeFilterStrategy<in TNode> where TNode : INode
    {
        bool Filter(IComparisonSource<TNode> comparisonSource, bool currentDecision);
    }
}
