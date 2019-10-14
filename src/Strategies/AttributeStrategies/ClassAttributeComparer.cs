using System.Linq;
using Egil.AngleSharp.Diffing.Core;

namespace Egil.AngleSharp.Diffing.Strategies.AttributeStrategies
{
    public class ClassAttributeComparer
    {
        public CompareResult Compare(in AttributeComparison comparison, CompareResult currentDecision)
        {
            if (currentDecision.IsDecisionFinal()) return currentDecision;
            if (!comparison.AttributeNameEquals("class")) return currentDecision;

            var (ctrlElm, testElm) = comparison.GetNodesAsElements();
            var sameLength = ctrlElm.ClassList.Length == testElm.ClassList.Length;
            if (!sameLength) return CompareResult.Different;
            return ctrlElm.ClassList.All(x => testElm.ClassList.Contains(x))
                ? CompareResult.Same
                : CompareResult.Different;
        }
    }
}
