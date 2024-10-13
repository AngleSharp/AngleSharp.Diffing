namespace AngleSharp.Diffing.Strategies.ElementStrategies;

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
        if (currentDecision == CompareResult.Skip || currentDecision == CompareResult.SkipAttributes || currentDecision == CompareResult.SkipChildrenAndAttributes)
            return currentDecision;

        if (!ControlHasTruthyIgnoreAttributesAttribute(comparison))
            return currentDecision;

        return currentDecision.Decision switch
        {
            CompareDecision.None => CompareResult.SkipAttributes,
            CompareDecision.Same => CompareResult.SkipAttributes,
            CompareDecision.Different => CompareResult.SkipAttributes,
            CompareDecision.SkipChildren => CompareResult.SkipChildrenAndAttributes,
            _ => currentDecision,
        };
    }

    private static bool ControlHasTruthyIgnoreAttributesAttribute(in Comparison comparison)
    {
        return comparison.Control.Node is IElement element &&
                element.TryGetAttrValue(DIFF_IGNORE_ATTRIBUTES_ATTRIBUTE, out bool shouldIgnore) &&
                shouldIgnore;
    }
}
