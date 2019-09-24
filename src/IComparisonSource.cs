using AngleSharp.Dom;

namespace Egil.AngleSharp.Diffing
{
    public interface IComparisonSource
    {
        int Index { get; }
        INode Node { get; }
    }
}