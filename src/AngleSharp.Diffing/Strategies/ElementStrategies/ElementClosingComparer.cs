using AngleSharp.Diffing.Core;
using AngleSharp.Dom;
using AngleSharp.Html.Parser.Tokens;

namespace AngleSharp.Diffing.Strategies.ElementStrategies
{
    /// <summary>
    /// Represents the element closing comparer strategy.
    /// </summary>
    public static class ElementClosingComparer
    {
        /// <summary>
        /// The element comparer closing strategy.
        /// </summary>
        public static CompareResult Compare(in Comparison comparison, CompareResult currentDecision)
        {
            if (currentDecision == CompareResult.Skip)
                return currentDecision;

            if (comparison.Test.Node is not IElement testElement || testElement.SourceReference is not HtmlTagToken testTag)
                return currentDecision;

            if (comparison.Control.Node is not IElement controlElement || controlElement.SourceReference is not HtmlTagToken controlTag)
                return currentDecision;

            return testTag.IsSelfClosing == controlTag.IsSelfClosing ? CompareResult.Same : CompareResult.Different;
        }
    }
}
