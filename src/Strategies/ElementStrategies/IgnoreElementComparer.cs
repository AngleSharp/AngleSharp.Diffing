using AngleSharp.Dom;
using Egil.AngleSharp.Diffing.Core;
using Egil.AngleSharp.Diffing.Extensions;

namespace Egil.AngleSharp.Diffing.Strategies.ElementStrategies
{
    public static class IgnoreElementComparer
    {
        private const string DIFF_IGNORE_ATTRIBUTE = "diff:ignore";

        public static CompareResult Compare(in Comparison comparison, CompareResult currentDecision)
        {
            if (currentDecision.IsSameOrSkip()) return currentDecision;

            return ControlHasTruthyIgnoreAttribute(comparison)
                ? CompareResult.Skip
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
