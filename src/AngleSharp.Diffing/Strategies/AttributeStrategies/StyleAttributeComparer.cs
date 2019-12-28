using System;

using AngleSharp.Css.Dom;
using AngleSharp.Diffing.Core;
using AngleSharp.Dom;

namespace AngleSharp.Diffing.Strategies.AttributeStrategies
{
    /// <summary>
    /// Represents the style attribute comparer strategy.
    /// </summary>
    public static class StyleAttributeComparer
    {
        /// <summary>
        /// The style attribute comparer strategy.
        /// </summary>
        public static CompareResult Compare(in AttributeComparison comparison, CompareResult currentDecision)
        {
            if (currentDecision.IsSameOrSkip())
                return currentDecision;

            return IsStyleAttributeComparison(comparison)
                ? CompareElementStyle(comparison)
                : currentDecision;
        }

        private static CompareResult CompareElementStyle(in AttributeComparison comparison)
        {
            var (ctrlElm, testElm) = comparison.GetAttributeElements();
            var ctrlStyle = ctrlElm.GetStyle();
            var testStyle = testElm.GetStyle();
            return ctrlStyle.CssText.Equals(testStyle.CssText, StringComparison.Ordinal)
                ? CompareResult.Same
                : CompareResult.Different;
        }

        private static bool IsStyleAttributeComparison(in AttributeComparison comparison)
        {
            return comparison.Control.Attribute.Name.Equals(AttributeNames.Style, StringComparison.Ordinal) &&
                comparison.Test.Attribute.Name.Equals(AttributeNames.Style, StringComparison.Ordinal);
        }
    }
}
