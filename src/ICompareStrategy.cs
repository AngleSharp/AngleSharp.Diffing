using AngleSharp.Dom;
using Egil.AngleSharp.Diffing.Comparisons;

namespace Egil.AngleSharp.Diffing
{
    public interface ICompareStrategy
    {
        CompareResult Compare<TNode>(in IComparison<TNode> comparison) where TNode : INode;
        CompareResult Compare(in IAttributeComparison comparison);
    }
}
