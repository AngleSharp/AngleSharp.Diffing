using Egil.AngleSharp.Diffing.Core;

namespace Egil.AngleSharp.Diffing.Strategies.NodeStrategies
{
    public static class NodeComparer
    {
        public static CompareResult Compare(in Comparison comparison, CompareResult currentDecision)
        {
            if(currentDecision.IsDecisionFinal()) return currentDecision;
            return comparison.AreNodeTypesEqual() 
                ? CompareResult.Same
                : CompareResult.Different;
        }
    }
}
