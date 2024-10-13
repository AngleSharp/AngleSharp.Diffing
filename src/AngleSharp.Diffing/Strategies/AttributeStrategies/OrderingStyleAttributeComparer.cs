namespace AngleSharp.Diffing.Strategies.AttributeStrategies;

/// <summary>
/// Represents the style attribute comparer strategy which orders the styles before comparing them.
/// </summary>
public static class OrderingStyleAttributeComparer
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
        return CompareCssStyleDeclarations(ctrlStyle, testStyle)
            ? CompareResult.Same
            : CompareResult.FromDiff(new AttrDiff(comparison, AttrDiffKind.Value));
    }

    private static bool CompareCssStyleDeclarations(ICssStyleDeclaration control, ICssStyleDeclaration test)
    {
        if (control.Length != test.Length)
            return false;

        var orderedControl = control.CssText.Split(';').Select(x => x.Trim()).OrderBy(x => x);
        var orderedTest = test.CssText.Split(';').Select(x => x.Trim()).OrderBy(x => x);

        return orderedControl.SequenceEqual(orderedTest, StringComparer.Ordinal);
    }
}
