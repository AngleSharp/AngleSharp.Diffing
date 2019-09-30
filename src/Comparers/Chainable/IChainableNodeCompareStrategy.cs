using AngleSharp.Dom;
using Egil.AngleSharp.Diffing.Comparisons;

namespace Egil.AngleSharp.Diffing.Comparers.Chainable
{
    public interface IChainableNodeCompareStrategy<in TNode> where TNode : INode
    {
        CompareResult Compare(IComparison<TNode> comparison, CompareResult currentDecision);
    }
}
