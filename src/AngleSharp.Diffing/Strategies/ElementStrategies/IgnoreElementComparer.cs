using AngleSharp.Diffing.Core;
using AngleSharp.Diffing.Extensions;
using AngleSharp.Dom;

namespace AngleSharp.Diffing.Strategies.ElementStrategies
{
    /// <summary>
    /// Represents the ignore element comparer.
    /// </summary>
    public static class IgnoreElementComparer
    {
        private const string DIFF_IGNORE_ATTRIBUTE = "diff:ignore";

        /// <summary>
        /// The ignore element comparer.
        /// </summary>
        public static CompareResult Compare(in Comparison comparison, CompareResult currentDecision)
        {
            if (currentDecision == CompareResult.Skip)
                return currentDecision;

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
