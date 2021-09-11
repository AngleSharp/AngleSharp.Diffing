using AngleSharp.Diffing.Core;
using AngleSharp.Diffing.Extensions;
using AngleSharp.Dom;

namespace AngleSharp.Diffing.Strategies.ElementStrategies
{
    /// <summary>
    /// Represents the ignore attributes element comparer.
    /// </summary>
    public static class IgnoreAttributesElementComparer
    {
        private const string DIFF_IGNORE_ATTRIBUTES_ATTRIBUTE = "diff:ignoreattributes";

        /// <summary>
        /// The ignore attributes element comparer.
        /// </summary>
        public static CompareResult Compare(in Comparison comparison, CompareResult currentDecision)
        {
            if (currentDecision == CompareResult.Skip)
                return currentDecision;

            return ControlHasTruthyIgnoreAttributesAttribute(comparison)
                ? CompareResult.SkipAttributes
                : currentDecision;
        }

        private static bool ControlHasTruthyIgnoreAttributesAttribute(in Comparison comparison)
        {
            return comparison.Control.Node is IElement element &&
                    element.TryGetAttrValue(DIFF_IGNORE_ATTRIBUTES_ATTRIBUTE, out bool shouldIgnore) &&
                    shouldIgnore;
        }
    }
}
