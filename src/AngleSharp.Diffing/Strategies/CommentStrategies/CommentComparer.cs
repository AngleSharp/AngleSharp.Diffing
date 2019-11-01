using AngleSharp.Diffing.Core;
using AngleSharp.Dom;

namespace AngleSharp.Diffing.Strategies.CommentStrategies
{
    public static class CommentComparer
    {
        public static CompareResult Compare(in Comparison comparison, CompareResult currentDecision)
        {
            if (currentDecision.IsSameOrSkip()) return currentDecision;
            return comparison.Control.Node.NodeType == NodeType.Comment && comparison.AreNodeTypesEqual()
                ? CompareResult.Same
                : CompareResult.Different;
        }
    }
}
