namespace AngleSharp.Diffing.Strategies.AttributeStrategies;

/// <summary>
/// Represents the ignore attribute comparer.
/// </summary>
[Obsolete("Has been moved to IgnoreAttributeStrategy")]
public static class IgnoreAttributeComparer
{
    private const string DIFF_IGNORE_POSTFIX = ":ignore";

    /// <summary>
    /// The ignore attribute comparer.
    /// </summary>
    public static CompareResult Compare(in AttributeComparison comparison, CompareResult currentDecision)
    {
        if (currentDecision.IsSameOrSkip)
            return currentDecision;

        return IsIgnoreAttribute(comparison.Control.Attribute)
            ? CompareResult.Same
            : currentDecision;
    }

    private static bool IsIgnoreAttribute(IAttr source)
    {
        return source.Name.EndsWith(DIFF_IGNORE_POSTFIX, StringComparison.OrdinalIgnoreCase);
    }
}