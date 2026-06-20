namespace AngleSharp.Diffing.Strategies.AttributeStrategies;

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
        if (currentDecision.IsSameOrSkip)
            return currentDecision;

        return IsStyleAttributeComparison(comparison)
            ? CompareElementStyle(comparison)
            : currentDecision;
    }

    private static bool IsStyleAttributeComparison(in AttributeComparison comparison)
    {
        return comparison.Control.Attribute.Name.Equals(AttributeNames.Style, StringComparison.Ordinal) &&
            comparison.Test.Attribute.Name.Equals(AttributeNames.Style, StringComparison.Ordinal);
    }

    private static CompareResult CompareElementStyle(in AttributeComparison comparison)
    {
        var (ctrlElm, testElm) = comparison.AttributeElements;
        var ctrlStyle = ctrlElm.GetStyle();
        var testStyle = testElm.GetStyle();

        // GetStyle() returns null when an element exposes no inline CSS style declaration — e.g. a non-HTML
        // (SVG/MathML) element, or any element when the browsing context has no CSS parser registered. Fall
        // back to comparing the raw style attribute values in that case, so such elements are compared by
        // value instead of throwing a NullReferenceException.
        var areEqual = ctrlStyle is not null && testStyle is not null
            ? CompareCssStyleDeclarations(ctrlStyle, testStyle)
            : string.Equals(comparison.Control.Attribute.Value, comparison.Test.Attribute.Value, StringComparison.Ordinal);

        return areEqual
            ? CompareResult.Same
            : CompareResult.FromDiff(new AttrDiff(comparison, AttrDiffKind.Value));
    }

    private static bool CompareCssStyleDeclarations(ICssStyleDeclaration control, ICssStyleDeclaration test)
    {
        if(control.Length != test.Length)
            return false;

        return control.CssText.Equals(test.CssText, StringComparison.Ordinal);
    }
}
