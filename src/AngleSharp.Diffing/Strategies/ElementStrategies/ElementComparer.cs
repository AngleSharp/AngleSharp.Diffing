using AngleSharp.Diffing.Core.Diffs;
using AngleSharp.Html.Parser.Tokens;

namespace AngleSharp.Diffing.Strategies.ElementStrategies;

/// <summary>
/// Represents the element comparer strategy.
/// </summary>
public class ElementComparer
{
    /// <summary>
    /// Gets whether or not the closing style of
    /// an element is considered during comparison.
    /// </summary>
    public bool EnforceTagClosing { get; }

    /// <summary>
    /// Creates an instance of the <see cref="ElementComparer"/>.
    /// </summary>
    /// <param name="enforceTagClosing">
    /// Whether or not the closing style of
    /// an element is considered during comparison.
    /// </param>
    public ElementComparer(bool enforceTagClosing)
    {
        EnforceTagClosing = enforceTagClosing;
    }

    /// <summary>
    /// The element comparer strategy.
    /// </summary>
    public CompareResult Compare(in Comparison comparison, CompareResult currentDecision)
    {
        if (currentDecision.IsSameOrSkip)
            return currentDecision;

        if (!comparison.TryGetNodesAsType<IElement>(out var controlElement, out var testElement))
            return currentDecision;

        var result = comparison.AreNodeTypesEqual
            ? CompareResult.Same
            : CompareResult.FromDiff(new ElementDiff(comparison, ElementDiffKind.Name));

        if (EnforceTagClosing && result == CompareResult.Same)
        {
            if (testElement.SourceReference is not HtmlTagToken testTag)
                throw new InvalidOperationException("No source reference attached to test element, cannot determine element tag closing style.");

            if (controlElement.SourceReference is not HtmlTagToken controlTag)
                throw new InvalidOperationException("No source reference attached to test element, cannot determine element tag closing style.");

            return testTag.IsSelfClosing == controlTag.IsSelfClosing
                ? result
                : CompareResult.FromDiff(new ElementDiff(comparison, ElementDiffKind.ClosingStyle));
        }

        return result;
    }
}
