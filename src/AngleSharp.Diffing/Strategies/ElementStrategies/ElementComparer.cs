using AngleSharp.Diffing.Core;
using AngleSharp.Dom;

namespace AngleSharp.Diffing.Strategies.ElementStrategies
{
    public static class ElementComparer
    {
        public static CompareResult Compare(in Comparison comparison, CompareResult currentDecision)
        {
            if (currentDecision.IsSameOrSkip()) return currentDecision;
            return comparison.Control.Node.NodeType == NodeType.Element && comparison.AreNodeTypesEqual()
                ? CompareResult.Same
                : CompareResult.Different;
        }
    }
}
