using AngleSharp.Dom;

namespace Egil.AngleSharp.Diffing.Comparisons
{
    public interface IComparisonSource<out TNode> where TNode : INode
    {
        TNode Node { get; }
        int Index { get; }
        string Path { get; }
        ComparisonSourceType SourceType { get; }
    }
}
