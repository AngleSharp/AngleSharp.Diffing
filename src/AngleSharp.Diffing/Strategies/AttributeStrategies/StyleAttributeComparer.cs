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
        return CompareCssStyleDeclarations(ctrlStyle, testStyle)
            ? CompareResult.Same
            : CompareResult.FromDiff(new AttrValueDiff(comparison, AttrValueDiffKind.Styles));
    }

    private static bool IsStyleAttributeComparison(in AttributeComparison comparison)
    {
        return comparison.Control.Attribute.Name.Equals(AttributeNames.Style, StringComparison.Ordinal) &&
            comparison.Test.Attribute.Name.Equals(AttributeNames.Style, StringComparison.Ordinal);
    }

    private static bool CompareCssStyleDeclarations(ICssStyleDeclaration control, ICssStyleDeclaration test)
    {
        if(control.Length != test.Length)
            return false;

        return control.CssText.Equals(test.CssText, StringComparison.Ordinal);
    }
}
