using AngleSharp.Diffing.Core;

namespace AngleSharp.Diffing.Strategies.NodeStrategies
{
    public static class NodeComparer
    {
        public static CompareResult Compare(in Comparison comparison, CompareResult currentDecision)
        {
            if(currentDecision.IsSameOrSkip()) return currentDecision;
            return comparison.AreNodeTypesEqual()
                ? CompareResult.Same
                : CompareResult.Different;
        }
    }
}
