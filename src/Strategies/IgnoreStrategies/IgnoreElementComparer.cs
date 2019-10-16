using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Dom;
using Egil.AngleSharp.Diffing.Core;
using Egil.AngleSharp.Diffing.Extensions;

namespace Egil.AngleSharp.Diffing.Strategies.IgnoreStrategies
{
    public static class IgnoreElementComparer
    {
        private const string DIFF_IGNORE_ATTRIBUTE = "diff:ignore";

        public static CompareResult Compare(in Comparison comparison, CompareResult currentDecision)
        {
            if (currentDecision.IsDecisionFinal()) return currentDecision;

            return ControlHasTruthyIgnoreAttribute(comparison)
                ? CompareResult.SameAndBreak
                : currentDecision;
        }

        private static bool ControlHasTruthyIgnoreAttribute(in Comparison comparison)
        {
            return comparison.Control.Node is IElement element &&
                   element.TryGetAttrValue(DIFF_IGNORE_ATTRIBUTE, out bool shouldIgnore) &&
                   shouldIgnore;
        }
    }
}
