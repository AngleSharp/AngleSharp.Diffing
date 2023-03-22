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
        if (currentDecision.IsSameOrSkip())
            return currentDecision;

        var result = comparison.Control.Node.NodeType == NodeType.Element && comparison.AreNodeTypesEqual
            ? CompareResult.Same
            : CompareResult.FromDiff(new NodeTypeDiff(comparison));

        if (EnforceTagClosing && result == CompareResult.Same)
        {
            if (comparison.Test.Node is not IElement testElement || testElement.SourceReference is not HtmlTagToken testTag)
                throw new InvalidOperationException("No source reference attached to test element, cannot determine element tag closing style.");

            if (comparison.Control.Node is not IElement controlElement || controlElement.SourceReference is not HtmlTagToken controlTag)
                throw new InvalidOperationException("No source reference attached to test element, cannot determine element tag closing style.");

            return testTag.IsSelfClosing == controlTag.IsSelfClosing
                ? result
                : CompareResult.FromDiff(new NodeClosingDiff(comparison));
        }

        return result;
    }
}
